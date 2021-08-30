using System.Collections.Generic;
using AccountService.Models;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/account/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _restaurantService;

        public PaymentsController(IPaymentService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost("{userId}")]
        public ActionResult CreatePayment([FromBody] CreatePaymentDto dto, int userId)
        {
            var create = _restaurantService.Create(userId,dto);
            if (create == null)
            {
                return BadRequest("UserId doesn't exist!");
            }
            return Created($"/api/restaurant/{create.Id}", dto);
        }
 
        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetUserPayments()
        {
            var getAll = _restaurantService.GetAll();
            return Ok(getAll);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetIdUserPayments([FromRoute] int id)
        {
            var getUser = _restaurantService.GetId(id);
            if (getUser is null) return NotFound("Not found User Details");
            return Ok(getUser);
        }
    }
}