using System;
using System.Collections.Generic;
using System.Linq;
using AccountService.Entities;
using AccountService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class AccountController : ControllerBase
    {
        private readonly AccountDbContext _dbContext;
        private readonly IMapper _mapper;

        public AccountController(AccountDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost]
        public ActionResult CreatePayment([FromBody] CreateUserPaymentDto dto)
        {
            DateTime now = DateTime.Now;
            var createPayment = _mapper.Map<UserPayments>(dto);
            createPayment.Time = now;
            _dbContext.UserPayments.Add(createPayment);
            _dbContext.SaveChanges();
            return Created($"/api/restaurant/{createPayment.Id}", createPayment);
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetUserPayments()
        {
            var userPayments = _dbContext.User.Include(r=>r.Address).Include(r=>r.UserPayments).ToList();
            var userPaymentsDto = _mapper.Map<List<UserDto>>(userPayments);
            return Ok(userPaymentsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetIdUserPayments([FromRoute] int id)
        {
            var getUser = _dbContext.User.Include(r=>r.Address).Include(r=>r.UserPayments).FirstOrDefault(r => r.Id == id);
            var getUserDtos = _mapper.Map<UserDto>(getUser);
            if (getUser is null)
            {
                return NotFound("Not found User Details");
            }
            return Ok(getUserDtos);
        }
    }
}