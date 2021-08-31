using AccountService.Entities;
using AccountService.Middleware;
using AccountService.Models;
using AccountService.Models.AccountService;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService accountService)
        {
            _service = accountService;
        }

        [HttpPost]
        public ActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            var result = _service.Create(dto);
            return Created($"api/account/{result.Id}", result);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser([FromBody] UpdateUserDto dto, [FromRoute] int id)
        {
            _service.Update(id, dto);
            return Ok("User updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            var isDeleted = _service.Delete(id);
            if (!isDeleted)
            {
                throw new NotFoundExcepion("User not Found.");
            }
            return NoContent();
        }
    }
}