using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountService.Entities;
using AccountService.Exceptions;
using AccountService.Services.AccountLoginModule.Models;
using AccountService.SMSender;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using IBackgroundTaskQueue = AccountService.SMSender.IBackgroundTaskQueue;

namespace AccountService.Services.AccountLoginModule
{
    public interface IAccountLoginService
    {
        Task<bool> SendVerifyCodeAsync(LoginUserDto dto);
        Task<string> LoginFromVerifyCodeAsync(LVerificationCodeDto dto);
        Task ResetAsync(LVerificationCodeDto dto);
    }

    public class AccountLoginService : IAccountLoginService
    {
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly AccountDbContext _dbContext;
        private readonly ILogger<AccountLoginService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IValidateSms _validateSms;

        public AccountLoginService(AccountDbContext dbContext, IMapper mapper, ILogger<AccountLoginService> logger,
            AuthenticationSettings authenticationSettings, IUserContextService userContextService,IBackgroundTaskQueue backgroundTaskQueue,IValidateSms validateSms)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
             _authenticationSettings = authenticationSettings;
            _userContextService = userContextService;
            _backgroundTaskQueue = backgroundTaskQueue;
            _validateSms = validateSms;
        }

        public async Task<bool> SendVerifyCodeAsync(LoginUserDto dto)
        {
            Random random = new Random();
            User userFromDb = await _dbContext.Users.FirstOrDefaultAsync(n =>
                n.Nationality == dto.Nationality && n.PhoneNumber == dto.PhoneNumber);
            if (userFromDb is null)
            {
                var newUser = _mapper.Map<User>(dto);
                newUser.RegistrationTime = DateTimeOffset.Now;
                newUser.RoleId = 1;
                newUser.VerificationCode = random.Next(10000000, 99999999).ToString();
                //SMS
                SmsDto smsForNewAccount = new SmsDto
                {
                    Nationality = newUser.Nationality,
                    PhoneNumber = newUser.PhoneNumber,
                    VerificationCode = newUser.VerificationCode
                };
                await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async token =>
                {
                    bool isCorrect = await _validateSms.ValidateAndSendSms(smsForNewAccount, token);
                }); 
                // bool isCorrect = _message.AddSmsToQueue(smsForNewAccount);
                // if (!isCorrect) return false;
                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();
               return true;
            }
            userFromDb.VerificationCode = random.Next(10000000, 99999999).ToString();
            //SMS
            SmsDto smsForExistingAccount = new SmsDto
            {
                Nationality = userFromDb.Nationality,
                PhoneNumber = userFromDb.PhoneNumber,
                VerificationCode = userFromDb.VerificationCode
            };
            await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async token =>
            {
                bool isCorrect = await _validateSms.ValidateAndSendSms(smsForExistingAccount, token);
            }); 
            _dbContext.Users.Update(userFromDb);
            await _dbContext.SaveChangesAsync();
            return false;
        }

        public async Task<string> LoginFromVerifyCodeAsync(LVerificationCodeDto dto)
        {
            User getNewUser = await _dbContext.Users.Include(r => r.Role)
                .FirstOrDefaultAsync(n => n.Nationality == dto.Nationality && n.PhoneNumber == dto.PhoneNumber);
            if (getNewUser is null) throw new BadRequestException("There is no such user.");
            getNewUser.LoginAttempts++;
            if (getNewUser.LoginAttempts >= 3)
            {
                getNewUser.LoginAttempts = 0;
                var random = new Random();
                getNewUser.VerificationCode = random.Next(10000000, 99999999).ToString();
                //Send SMS- added 
                SmsDto newSms = new SmsDto
                {
                    Nationality = getNewUser.Nationality,
                    PhoneNumber = getNewUser.PhoneNumber,
                    VerificationCode = getNewUser.VerificationCode
                };
                await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async token =>
                {
                    bool isCorrect = await _validateSms.ValidateAndSendSms(newSms, token);
                }); 
                _dbContext.Users.Update(getNewUser);
                await _dbContext.SaveChangesAsync();
                throw new WarningException("Too many Login Attempts. New VerifyCode has been sent an SMS.");
            }

            if (getNewUser.VerificationCode != dto.VerificationCode) throw new BadRequestException("Bad VerifyCode");
            getNewUser.LoginAttempts--;
            //get access token - added
            List<Claim> claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, getNewUser.Id.ToString()),
                new(ClaimTypes.Role, $"{getNewUser.Role.Name}"),
                new("Nationality", $"{getNewUser.Nationality}")
            };
            if (!string.IsNullOrEmpty(getNewUser.FirstName))
                claims.Add(new Claim(ClaimTypes.Name, $"{getNewUser.FirstName}"));
            else if (!string.IsNullOrEmpty(getNewUser.LastName))
                claims.Add(new Claim(ClaimTypes.Name, $"{getNewUser.LastName}"));
            else if (!string.IsNullOrEmpty(getNewUser.FirstName) && !string.IsNullOrEmpty(getNewUser.LastName))
                claims.Add(new Claim(ClaimTypes.Name, $"{getNewUser.FirstName}{getNewUser.LastName}"));
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
            JwtSecurityToken token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            _dbContext.Users.Update(getNewUser);
            await _dbContext.SaveChangesAsync();
            return tokenHandler.WriteToken(token);
        }

        public async Task ResetAsync(LVerificationCodeDto dto)
        {
            User resetRegisterUser = await _dbContext.Users.FirstOrDefaultAsync(u =>
                u.Nationality == dto.Nationality && u.PhoneNumber == dto.PhoneNumber);
            if (resetRegisterUser is null) throw new NotFoundExcepion("User Not Found.");
            resetRegisterUser.LoginAttempts = 0;
            Random random = new Random();
            resetRegisterUser.VerificationCode = random.Next(00000000, 99999999).ToString();
            _dbContext.Users.Update(resetRegisterUser);
            await _dbContext.SaveChangesAsync();
        }
    }
}