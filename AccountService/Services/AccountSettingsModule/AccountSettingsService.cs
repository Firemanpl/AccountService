using System;
using System.Threading.Tasks;
using AccountService.Entities;
using AccountService.Exceptions;
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
        Task ChangePhoneNumberAsync(SLoginUserDto dto);
        Task VerifyChangedPhoneNumberAsync(SVerificationCodeDto dto);
        Task DeleteAsync();
    }

    public class AccountSettingsService : IAccountSettingsService
    {
        private readonly AccountDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<AccountSettingsService> _logger;
        private readonly ISendMessage _sendMessage;


        public AccountSettingsService(AccountDbContext dbContext, IMapper mapper, IUserContextService userContextService, ILogger<AccountSettingsService> logger,ISendMessage sendMessage)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
            _logger = logger;
            _sendMessage = sendMessage;

        }
        public async Task<SUserSettingsDto> GetSettings()
        {
            User userFromDb =  await _dbContext.Users.Include(u=>u.Address).FirstOrDefaultAsync(u=>u.Id == _userContextService.GetUserId);
            SUserSettingsDto getUserSettings = _mapper.Map<SUserSettingsDto>(userFromDb);
            return getUserSettings;
        }
        
        public async Task UpdateSettings(SUserSettingsDto dto)
        {
            User userFromDb =  await _dbContext.Users.Include(u=>u.Address).FirstOrDefaultAsync(u=>u.Id == _userContextService.GetUserId);
            if (userFromDb is null) throw new NotFoundExcepion("User Not Found.");
            User updateUser = _mapper.Map(dto, userFromDb);
            _dbContext.Users.Update(updateUser);
            await _dbContext.SaveChangesAsync();
        }



        public async Task ChangePhoneNumberAsync(SLoginUserDto dto)
        {
            User userFromDb = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == _userContextService.GetUserId);
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
            _sendMessage.AddSmsToQueue(newSms);
            _dbContext.Users.Update(userFromDb);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task VerifyChangedPhoneNumberAsync(SVerificationCodeDto dto)
        {
            User userFromDb = await _dbContext.Users.FirstOrDefaultAsync(u=>u.PhoneNumber == dto.PhoneNumber && u.Nationality == dto.Nationality);
            if (userFromDb is null) throw new NotFoundExcepion("User Not Found.");
            if (userFromDb.VerificationCode!=dto.VerificationCode)
            {
                throw new BadRequestException("Bad VerifyCode");
            }
            _dbContext.Users.Update(userFromDb);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync()
        {
            _logger.LogWarning($"User with id: {_userContextService.GetUserId} DELETE action invoked.");
            User getUserFromDb = await _dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Id == _userContextService.GetUserId);
            if (getUserFromDb is null)
                throw new NotFoundExcepion("User Not Found.");
            Address getAddressFromDb = await _dbContext
                .Addresses
                .FirstOrDefaultAsync(u => u.Id == getUserFromDb.AddressId);
            UserPayments getUserPaymentsFromDb = await _dbContext
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