using System.Threading.Tasks;
using AccountService.Models.AccountServiceDtos;
using AccountService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            var result = await _service.Create(dto);
            return Created($"api/account/{result.Id}", result);
        }
        [HttpPut("phonenumber/{id}")]
        public async Task<ActionResult> UpdatePhoneNumber([FromBody] UpdatePhoneNumberDto dto, int id)
        {
            await _service.Update(id,dto);
            return Ok("PhoneNumber Updated.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto dto, [FromRoute] int id)
        {
            await _service.Update(id,dto);
            return Ok("User updated.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}