using System;
using System.Collections.Generic;
using System.Linq;
using AccountService.Entities;
using AccountService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountService.Services
{
    public interface IPaymentService
    {
        UserDto GetId(int id);
        IEnumerable<UserDto> GetAll();
        UserPayments Create(CreatePaymentDto dto);
    }

    public class PaymentService : IPaymentService
    {
        private readonly AccountDbContext _dbContext;
        private readonly IMapper _mapper;
        //private readonly ILogger<PaymentService> _logger;

        public PaymentService(AccountDbContext dbContext, IMapper mapper)//, ILogger<PaymentService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            //_logger = logger;
        }



        public UserDto GetId(int id)
        {
            var getUser = _dbContext.User.Include(r=>r.Address).Include(r=>r.UserPayments).FirstOrDefault(r => r.Id == id);
            if (getUser is null)
            {
                return null;
            }
            var result = _mapper.Map<UserDto>(getUser);
            return result;
        }

        public IEnumerable<UserDto> GetAll()
        {
            var userPayments = _dbContext.User.Include(r=>r.Address).Include(r=>r.UserPayments).ToList();
            var userPaymentsDto = _mapper.Map<List<UserDto>>(userPayments);
            return userPaymentsDto;
        }

        public UserPayments Create(CreatePaymentDto dto)
        {
            var createPayment = _mapper.Map<UserPayments>(dto);
            DateTime now = DateTime.Now;
            createPayment.Time = now;
            var user = _dbContext.User.OrderByDescending(u=>u.Id).FirstOrDefault();
            if (user != null && createPayment.UserId > 0 && createPayment.UserId <= user.Id)
            {
                _dbContext.UserPayments.Add(createPayment);
                _dbContext.SaveChanges();
                return createPayment;
            }
            return null;
        }
    }
}