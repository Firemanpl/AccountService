using AccountService.Entities;
using AccountService.Models;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController: ControllerBase
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
            return Created($"api/account/{result.Id}",result);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateUser([FromBody] UpdateUserDto dto, [FromRoute] int id)
        {
            var result = _service.Update(id, dto);
            if (!result)
            {
                return NotFound();
            }
            return Ok("User updated.");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            var isDeleted = _service.Delete(id);
            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}