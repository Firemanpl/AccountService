using System.Collections.Generic;
using System.Threading.Tasks;
using AccountService.Entities;
using AccountService.Models;
using AccountService.Models.PaymentServiceDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Services.PaymentsModule
{
    [ApiController]
    [Authorize]
    [Route("api/account/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
        {
            UserPayments create = await _paymentService.CreateAsync(dto);
            if (create == null)
            {
                return BadRequest("UserId doesn't exist!");
            }
            return Created($"/api/restaurant/{create.Id}", dto);
        }

        [HttpGet("all")]
        [Authorize(Roles="Admin,Manager")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUserPayments()
        {
            IEnumerable<UserDto> getAll = await _paymentService.GetAllAsync();
            return Ok(getAll);
        }
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetIdUserPayments()
        {
            UserDto getUser = await _paymentService.GetIdAsync();
            return Ok(getUser);
        }
    }
}