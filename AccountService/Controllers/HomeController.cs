using System.Collections.Generic;
using System.Linq;
using AccountService.Entities;
using AccountService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        private readonly AccountDbContext _dbContext;
        private readonly IMapper _mapper;

        public HomeController(AccountDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserPayments>> GetUserPayments()
        {
            var userPayments = _dbContext.UserPayments.ToList();
            var userPaymentsDto = _mapper.Map<List<UserPaymentsDto>>(userPayments);
            return Ok(userPaymentsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<UserPayments> GetIdUserPayments([FromRoute] int id)
        {
            var getUserPayments = _dbContext.UserPayments.Where(r => r.UserId == id);
            var getUserPaymentsDto = _mapper.Map<UserPaymentsDto>(getUserPayments);
            if (getUserPayments.Count() == 0)
            {
                return NotFound("Not found User history");
            }
            return Ok(getUserPaymentsDto);
        }
    }
}