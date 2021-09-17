using System;
using System.Threading.Tasks;
using AccountService.Entities;
using AccountService.Exceptions;
using AccountService.SendSMS;
using AccountService.Services.AccountLoginModule.Models;
using AccountService.SMSender;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;

namespace AccountService.Services.AccountSettingsModule
{
    public interface IAccountSettingsService
    {
        Task<SUserSettingsDto> GetSettings();
        Task UpdateSettings(SUserSettingsDto dto);
        Task ChangePhoneNumber(SLoginUserDto dto);
        Task VerifyChangedPhoneNumber(SVerificationCodeDto dto);
        Task Delete();
    }

    public class AccountSettingsService : IAccountSettingsService
    {
        private readonly AccountDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ISendMessage _message;
        private readonly ILogger<AccountSettingsService> _logger;


        public AccountSettingsService(AccountDbContext dbContext, IMapper mapper, IUserContextService userContextService, ISendMessage message, ILogger<AccountSettingsService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
            _message = message;
            _logger = logger;
        }
        public async Task<SUserSettingsDto> GetSettings()
        {
            var userFromDb =  await _dbContext.Users.Include(u=>u.Address).FirstOrDefaultAsync(u=>u.Id == _userContextService.GetUserId);
            SUserSettingsDto getUserSettings = _mapper.Map<SUserSettingsDto>(userFromDb);
            return getUserSettings;
        }
        
        public async Task UpdateSettings(SUserSettingsDto dto)
        {
            var userFromDb =  await _dbContext.Users.Include(u=>u.Address).FirstOrDefaultAsync(u=>u.Id == _userContextService.GetUserId);
            if (userFromDb is null) throw new NotFoundExcepion("User Not Found.");
            var updateUser = _mapper.Map(dto, userFromDb);
            _dbContext.Users.Update(updateUser);
            await _dbContext.SaveChangesAsync();
        }



        public async Task ChangePhoneNumber(SLoginUserDto dto)
        {
            var userFromDb = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == _userContextService.GetUserId);
            if (userFromDb is null) throw new NotFoundExcepion("User not exist.");
            Random random = new Random();
            userFromDb.VerificationCode = random.Next(10000000,99999999).ToString(); 
            //Send SMS - added 
            SmsDto newSms = new SmsDto()
            {
                Nationality = userFromDb.Nationality,
                PhoneNumber = userFromDb.PhoneNumber,
                VerificationCode = userFromDb.VerificationCode,
            };
            _message.AddSmsToQueue(newSms);
            _dbContext.Users.Update(userFromDb);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task VerifyChangedPhoneNumber(SVerificationCodeDto dto)
        {
            var userFromDb = await _dbContext.Users.FirstOrDefaultAsync(u=>u.PhoneNumber == dto.PhoneNumber && u.Nationality == dto.Nationality);
            if (userFromDb is null) throw new NotFoundExcepion("User Not Found.");
            if (userFromDb.VerificationCode!=dto.VerificationCode)
            {
                throw new BadRequestException("Bad VerifyCode");
            }
            _dbContext.Users.Update(userFromDb);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete()
        {
            _logger.LogWarning($"User with id: {_userContextService.GetUserId} DELETE action invoked.");
            var getUserFromDb = await _dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Id == _userContextService.GetUserId);
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