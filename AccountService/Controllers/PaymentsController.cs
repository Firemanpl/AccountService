using System.Collections.Generic;
using AccountService.Models;
using AccountService.Models.PaymentServiceDtos;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/account/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("{userId}")]
        public ActionResult CreatePayment([FromBody] CreatePaymentDto dto, int userId)
        {
            var create = _paymentService.Create(userId,dto);
            if (create == null)
            {
                return BadRequest("UserId doesn't exist!");
            }
            return Created($"/api/restaurant/{create.Id}", dto);
        }
 
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetUserPayments()
        {
            var getAll = _paymentService.GetAll();
            return Ok(getAll);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetIdUserPayments([FromRoute] int id)
        {
            var getUser = _paymentService.GetId(id);
            return Ok(getUser);
        }
    }
}