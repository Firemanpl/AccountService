using System;
using System.Linq;
using AccountService.Entities;
using AccountService.Models;
using AutoMapper;
using NLog;

namespace AccountService.Services
{
    public interface IAccountService
    {
        User Create(CreateUserDto dto);
        bool Update(UpdateUserDto dto, int id);
        public bool Delete(int id);
    }

    public class AccountService : IAccountService
    {
        private readonly AccountDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountService(AccountDbContext dbContext, IMapper mapper)//, ILogger logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
           // _logger = logger;
        }

        public User Create(CreateUserDto dto)
        {
            var createUser = _mapper.Map<User>(dto);
            DateTime now = DateTime.Now;
            createUser.RegistrationTime = now;
            createUser.Role.NameRole = "User";
            _dbContext.User.Add(createUser);
            _dbContext.SaveChanges();
                return createUser;
        }

        public bool Update(UpdateUserDto dto, int id)
        {
            var updateUser = _mapper.Map<User>(dto);
            var getUserFromDb = _dbContext.User.FirstOrDefault(u => u.Id == id);
            if (getUserFromDb is null)
            {
                return false;
            }
            if (updateUser != null && updateUser.Id > 0 )
            {
                getUserFromDb = updateUser;
                _dbContext.SaveChanges();
            }

            return true;
        }

        public bool Delete(int id)
        {
            var getUserFromDb = _dbContext.User.FirstOrDefault(u=>u.Id == id);
            if (getUserFromDb is null) return false;
            _dbContext.User.Remove(getUserFromDb);
            _dbContext.SaveChanges();
            return true;
        }
    }
}