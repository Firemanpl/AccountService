using System.Collections.Generic;
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
        public ActionResult CreatePayment([FromBody] CreatePaymentDto dto)
        {
            var create = _paymentService.Create(dto);
            if (create == null)
            {
                return BadRequest("UserId doesn't exist!");
            }
            return Created($"/api/restaurant/{create.Id}", dto);
        }

        [HttpGet("all")]
        [Authorize(Roles="Admin,Manager")]
        public ActionResult<IEnumerable<UserDto>> GetUserPayments()
        {
            var getAll = _paymentService.GetAll();
            return Ok(getAll);
        }
        [HttpGet]
        public ActionResult<UserDto> GetIdUserPayments()
        {
            var getUser = _paymentService.GetId();
            return Ok(getUser);
        }
    }
}