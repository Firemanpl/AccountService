using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountService.Entities;
using AccountService.Exceptions;
using AccountService.SendSMS;
using AccountService.Services.AccountLoginModule.Models;
using AccountService.Services.AccountRegistrationModule.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AccountService.Services.AccountLoginModule
{
    public interface IAccountLoginService
    {
        Task<int> SendVerifyCode(LoginUserDto dto);
        Task<string> LoginFromVerifyCode(LUpdateVerificationCodeDto dto);
        Task Reset(LUpdateVerificationCodeDto dto);
        Task UpdateSettings(LUpdateUserSettingsDto dto, int id);
        Task Delete(int id);
    }

    public class AccountLoginService : IAccountLoginService
    {
        private readonly AccountDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountLoginService> _logger;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly ISendMessage _message;

        public AccountLoginService(AccountDbContext dbContext, IMapper mapper, ILogger<AccountLoginService> logger, AuthenticationSettings authenticationSettings, ISendMessage message)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authenticationSettings = authenticationSettings;
            _message = message;
        }

        public async Task<int> SendVerifyCode(LoginUserDto dto)
        {
            
            var userFromDb = await _dbContext.Users.FirstOrDefaultAsync(n=>n.Nationality==dto.Nationality && n.PhoneNumber == dto.PhoneNumber);
            if (userFromDb is null) throw new NotFoundExcepion("User doesn't exist.");
            Random random = new Random();
            userFromDb.VerificationCode = random.Next(00000000,99999999).ToString(); 
            //SMS
            SmsDto newSms = new SmsDto()
            {
                Nationality = userFromDb.Nationality,
                PhoneNumber = userFromDb.PhoneNumber,
                VerificationCode = userFromDb.VerificationCode,
            };
            _message.AddSmsToQueue(newSms);
            _dbContext.Users.Update(userFromDb);
            await _dbContext.SaveChangesAsync();
            return userFromDb.Id;
        }

        public async Task<string> LoginFromVerifyCode(LUpdateVerificationCodeDto dto)
        {
            var getNewUser = await _dbContext.Users.FirstOrDefaultAsync(n=>n.Nationality==dto.Nationality && n.PhoneNumber==dto.PhoneNumber);
            if (getNewUser is null) throw new BadRequestException("There is no such user.");
            getNewUser.LoginAttempts++;
            if (getNewUser.LoginAttempts >=3)
            {
                getNewUser.LoginAttempts = 0;
                Random random = new Random();
                getNewUser.VerificationCode = random.Next(00000000,99999999).ToString(); 
                //Send SMS 
                SmsDto newSms = new SmsDto()
                {
                    Nationality = getNewUser.Nationality,
                    PhoneNumber = getNewUser.PhoneNumber,
                    VerificationCode = getNewUser.VerificationCode,
                };
                _message.AddSmsToQueue(newSms);
                _dbContext.Users.Update(getNewUser);
                await _dbContext.SaveChangesAsync();
                throw new WarningException("Too many Login Attempts. New VerifyCode has been sent an SMS.");
            }
            
            if (getNewUser.VerificationCode!=dto.VerificationCode)
            {
                throw new BadRequestException("Bad VerifyCode");
            }else
            {
                //get access token
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, getNewUser.Id.ToString()),
                    new Claim(ClaimTypes.Name,$"{getNewUser.FirstName}{getNewUser.LastName}"),
                    new Claim(ClaimTypes.Role,$"{getNewUser.Role.Name}"),
                    new Claim("Nationality",$"{getNewUser.Nationality}"),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
                var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                    claims,
                    expires:expires,
                    signingCredentials:cred);
                
                var tokenHandler = new JwtSecurityTokenHandler();
                _dbContext.Users.Update(getNewUser);
                await _dbContext.SaveChangesAsync();
                return tokenHandler.WriteToken(token);
            }
        }
        public async Task Reset(LUpdateVerificationCodeDto dto)
        { 
            var resetRegisterUser = await _dbContext.Users.FirstOrDefaultAsync(u=>u.Nationality==dto.Nationality && u.PhoneNumber ==dto.PhoneNumber);
            if (resetRegisterUser is null) throw new NotFoundExcepion("User Not Found.");
            resetRegisterUser.LoginAttempts = 0;
            Random random = new Random();
            resetRegisterUser.VerificationCode = random.Next(00000000,99999999).ToString(); 
            _dbContext.Users.Update(resetRegisterUser);
            await _dbContext.SaveChangesAsync();
        } 
        
        public async Task UpdateSettings(LUpdateUserSettingsDto dto,int id)
        {
            var userFromDb =  await _dbContext.Users.Include(u=>u.Address).FirstOrDefaultAsync(u=>u.Id == id);
            if (userFromDb is null) throw new NotFoundExcepion("User Not Found.");
            var updateUser = _mapper.Map(dto, userFromDb);
            // var newUser = new User()
            // {
            //     Id = id,
            //     Email = dto.Email,
            //     FirstName = dto.FirstName,
            //     LastName = dto.LastName,
            // };
            // var newAddress = new Address()
            // {
            //     Id = userFromDb.AddressId,
            //     City = dto.City,
            //     Street = dto.Street,
            //     PostalCode = dto.PostalCode,
            // };
            // _dbContext.Addresses.Update(newAddress);
            _dbContext.Users.Update(updateUser);
            await _dbContext.SaveChangesAsync();
        }

        // public async Task UpdatePhoneNumber(LoginUserDto dto,int id)
        // {
        //     var userFromDb = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id==id);
        //     if (userFromDb is null) throw new NotFoundExcepion("User not exist.");
        //     //Send SMS
        //     userFromDb.Nationality = dto.Nationality;
        //     userFromDb.PhoneNumber = dto.PhoneNumber;
        //     _dbContext.Users.Update(userFromDb);
        //     await _dbContext.SaveChangesAsync();
        // }
        //
        // public async Task VerifyCode(LUpdateVerificationCodeDto dto, int id)
        // {
        //     var userFromDb = await _dbContext.Users.FirstOrDefaultAsync(u=>u.Id == id);
        //     if (userFromDb is null) throw new NotFoundExcepion("User Not Found.");
        //     if (userFromDb.VerificationCode == dto.VerificationCode)
        //     {
        //         userFromDb.VerificationCode = dto.VerificationCode;
        //         _dbContext.Users.Update(userFromDb);
        //         await _dbContext.SaveChangesAsync();
        //     }
        // }

        public async Task Delete(int id)
        {
            _logger.LogWarning($"User with id: {id} DELETE action invoked.");
            var getUserFromDb = await _dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (getUserFromDb is null)
                throw new NotFoundExcepion("User Not Found.");
            var getAddressFromDb = await _dbContext
                .Addresses
                .FirstOrDefaultAsync(u => u.Id == getUserFromDb.AddressId);
            var getUserPaymentsFromDb = await _dbContext
                .UserPayments
                .FirstOrDefaultAsync(u => u.UserId == getUserFromDb.Id);
            if (getAddressFromDb is not null)
            {
                _dbContext.Addresses.Remove(getAddressFromDb);
            }
            if (getUserPaymentsFromDb is not null)
            {
                _dbContext.UserPayments.Remove(getUserPaymentsFromDb);
            }
            _dbContext.Users.Remove(getUserFromDb);
            await _dbContext.SaveChangesAsync();
        }
    }
}