using System;
using System.Linq;
using AccountService.Entities;
using AccountService.Middleware;
using AccountService.Models;
using AccountService.Models.AccountService;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ILogger = NLog.ILogger;


namespace AccountService.Services
{
    public interface IAccountService
    {
        User Create(CreateUserDto dto);
        void Update(int id, UpdateUserDto dto);
        bool Delete(int id);
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

        public User Create(CreateUserDto dto)
        {
            var createUser = _mapper.Map<User>(dto);
            DateTime now = DateTime.Now;
            createUser.RegistrationTime = now;
            createUser.RoleId = 1;
            _dbContext.Users.Add(createUser);
            _dbContext.SaveChanges();
            return createUser;
        }

        public void Update (int id, UpdateUserDto dto)
        {
            var getUserFromDb = _dbContext
                .Users
                .Include(u => u.Address)
                .FirstOrDefault(u => u.Id == id);
            if (getUserFromDb is null)
                throw new NotFoundExcepion("User not Found.");
            var updateUser = _mapper.Map(dto, getUserFromDb);
            _dbContext.Users.Update(updateUser);
            _dbContext.SaveChanges();
        }

        public bool Delete(int id)
        {
            //_logger.LogWarning($"User with id: {id} DELETE action invoked.");
            var getUserFromDb = _dbContext
                .Users
                .FirstOrDefault(u => u.Id == id);
            if (getUserFromDb is null)
                return false;
            var getAddressFromDb = _dbContext
                .Addresses
                .FirstOrDefault(u => u.Id == getUserFromDb.AddressId);
            if (getAddressFromDb is null)
                return false;
            var getUserPaymentsFromDb = _dbContext
                .UserPayments
                .FirstOrDefault(u => u.UserId == getUserFromDb.Id);
            if (getUserPaymentsFromDb is null)
                return false;
            _dbContext.Users.Remove(getUserFromDb);
            _dbContext.Addresses.Remove(getAddressFromDb);
            _dbContext.UserPayments.Remove(getUserPaymentsFromDb);
            _dbContext.SaveChanges();
            return true;
        }
    }
}