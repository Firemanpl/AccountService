using System;
using System.Threading.Tasks;
using AccountService.Entities;
using AccountService.Middleware;
using AccountService.Models.AccountServiceDtos;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;



namespace AccountService.Services
{
    public interface IAccountService
    {
        Task<User> Create(CreateUserDto dto);
        Task Update(int id, UpdateUserDto dto);
        Task Delete(int id);
        Task Update(int id,UpdatePhoneNumberDto dto);
    }

    public class AccountService : IAccountService
    {
        private readonly AccountDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(AccountDbContext dbContext, IMapper mapper, ILogger<AccountService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<User> Create(CreateUserDto dto)
        {
            var createUser = _mapper.Map<User>(dto);
            DateTime now = DateTime.Now;
            createUser.RegistrationTime = now;
            createUser.RoleId = 1;
            await _dbContext.Users.AddAsync(createUser);
            await _dbContext.SaveChangesAsync();
            return createUser;
        }

        public async Task Update(int id,UpdatePhoneNumberDto dto)
        {
            var phoneNumber = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id==id);
            if (phoneNumber is null) throw new NotFoundExcepion("User not exist.");
            phoneNumber.PhoneNumber = dto.PhoneNumber;
            _dbContext.Users.Update(phoneNumber);
            await _dbContext.SaveChangesAsync();
        }
        public async Task Update (int id, UpdateUserDto dto)
        {
            var getUserFromDb = await _dbContext
                .Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (getUserFromDb is null)
                throw new NotFoundExcepion("User Not Found.");
            var updateUser = _mapper.Map(dto, getUserFromDb);
            _dbContext.Users.Update(updateUser);
            await _dbContext.SaveChangesAsync();
        }

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