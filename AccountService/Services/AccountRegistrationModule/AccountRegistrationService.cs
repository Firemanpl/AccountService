using System;
using System.Linq;
using System.Threading.Tasks;
using AccountService.Entities;
using AccountService.Exceptions;
using AccountService.SendSMS;
using AccountService.Services.AccountRegistrationModule.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Services.AccountRegistrationModule
{
    public interface IAccountRegistrationService
    {
        Task<int> RegisterAccount(RegisterUserDto dto);
        Task CreateFirstSettings(RUpdateUserSettingsDto settingsDto, int id);
    }
    public class AccountRegistrationService : IAccountRegistrationService
    {
        private readonly AccountDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ISendMessage _message;

        public AccountRegistrationService(AccountDbContext dbContext, IMapper mapper, ISendMessage message)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _message = message;
        }

        public async Task<int> RegisterAccount(RegisterUserDto dto)
        {
            Random random = new Random();
            var newUser = new User()
            {
                Nationality = dto.Nationality,
                PhoneNumber = dto.PhoneNumber,
                RoleId = 1,
            };
            var isUserExist = await _dbContext.Users.FirstOrDefaultAsync(n => n.PhoneNumber == newUser.PhoneNumber && n.Nationality == newUser.Nationality);
            if (isUserExist is not null)
            {
                newUser = isUserExist;
                newUser.VerificationCode = random.Next(00000000,99999999).ToString();
                SmsDto newSms = new SmsDto()
                {
                    Nationality = newUser.Nationality,
                    PhoneNumber = newUser.PhoneNumber,
                    VerificationCode = newUser.VerificationCode,
                };
                _message.AddSmsToQueue(newSms);
                _dbContext.Users.Update(newUser);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                newUser.VerificationCode = random.Next(00000000,99999999).ToString(); 
                SmsDto newSms = new SmsDto()
                {
                    Nationality = newUser.Nationality,
                    PhoneNumber = newUser.PhoneNumber,
                    VerificationCode = newUser.VerificationCode,
                };
                _message.AddSmsToQueue(newSms);
                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();
            }
            return newUser.Id;
        }
        
        public async Task CreateFirstSettings(RUpdateUserSettingsDto dto,int id)
        {
            var userFromDb =  await _dbContext.Users.FirstOrDefaultAsync(u=>u.Id == id);
            if (userFromDb is null) throw new NotFoundExcepion("User Not Found.");
            var newUser = new User()
            {
                Id = id,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
            };
            var newAddress = new Address()
            {
                Id = userFromDb.AddressId,
                City = dto.City,
                Street = dto.Street,
                PostalCode = dto.PostalCode,
            };
            _dbContext.Users.Update(newUser);
            await _dbContext.Addresses.AddAsync(newAddress);
            await _dbContext.SaveChangesAsync();
        }
    }
}