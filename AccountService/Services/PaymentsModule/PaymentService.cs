using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountService.Entities;
using AccountService.Exceptions;
using AccountService.Models;
using AccountService.Models.PaymentServiceDtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Services.PaymentsModule
{
    public interface IPaymentService
    {
        Task<UserDto> GetId();
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserPayments> Create(CreatePaymentDto dto);
    }
    public class PaymentService : IPaymentService
    {
        private readonly AccountDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public PaymentService(AccountDbContext dbContext, IMapper mapper, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        
        public async Task<UserDto> GetId()
        {
            var getUser = await _dbContext.Users.Include(r=>r.Address).Include(r=>r.UserPayments).FirstOrDefaultAsync(r => r.Id == _userContextService.GetUserId);
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

        public async Task<UserPayments> Create( CreatePaymentDto dto)
        {
            var createPayment = _mapper.Map<UserPayments>(dto);
            var id = _userContextService.GetUserId;
            DateTime now = DateTime.Now;
            createPayment.Time = now;
            var getUser = await _dbContext.Users.OrderByDescending(u=>u.Id).FirstOrDefaultAsync();
            if (getUser != null && id <= getUser.Id && id > 0)
            {
                createPayment.UserId = (int) id;
                _dbContext.UserPayments.Add(createPayment);
                await _dbContext.SaveChangesAsync();
                return createPayment;
            }
            return null;
        }
    }
}