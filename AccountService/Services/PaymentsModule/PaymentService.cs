using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountService.Entities;
using AccountService.Exceptions;
using AccountService.Middleware;
using AccountService.Models;
using AccountService.Models.PaymentServiceDtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountService.Services
{
    public interface IPaymentService
    {
        Task<UserDto> GetId(int id);
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserPayments> Create(int id, CreatePaymentDto dto);
    }
    public class PaymentService : IPaymentService
    {
        private readonly AccountDbContext _dbContext;
        private readonly IMapper _mapper;

        public PaymentService(AccountDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }



        public async Task<UserDto> GetId(int id)
        {
            var getUser = await _dbContext.Users.Include(r=>r.Address).Include(r=>r.UserPayments).FirstOrDefaultAsync(r => r.Id == id);
            if (getUser is null)
                throw new NotFoundExcepion("User not Found.");
                
            var result = _mapper.Map<UserDto>(getUser);
            return result;
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var userPayments = await _dbContext.Users.Include(r=>r.Address).Include(r=>r.UserPayments).ToListAsync();
            var userPaymentsDto = _mapper.Map<List<UserDto>>(userPayments);
            return userPaymentsDto;
        }

        public async Task<UserPayments> Create(int id, CreatePaymentDto dto)
        {
            var createPayment = _mapper.Map<UserPayments>(dto);
            DateTime now = DateTime.Now;
            createPayment.Time = now;
            var getUser = await _dbContext.Users.OrderByDescending(u=>u.Id).FirstOrDefaultAsync();
            if (getUser != null && id <= getUser.Id && id > 0)
            {
                createPayment.UserId = id;
                _dbContext.UserPayments.Add(createPayment);
                _dbContext.SaveChanges();
                return createPayment;
            }
            return null;
        }
    }
}