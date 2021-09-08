using System.Threading.Tasks;
using AccountService.Services.AccountLoginModule.Models;
using AccountService.Services.AccountRegistrationModule.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Services.AccountLoginModule
{
    [ApiController]
    [Route("api/account/login")]
    
    public class AccountLoginController : ControllerBase
    {
        private readonly IAccountLoginService _service;

        public AccountLoginController(IAccountLoginService service)
        {
            _service = service;
        }

        [HttpPut]
        public async Task<ActionResult> LoginPhoneNumber([FromBody] LoginUserDto dto)
        {
            await _service.SendVerifyCode(dto);
            return Ok("SMS SENDED");
        }

        [HttpPut("phoneNumber")]
        public async Task<ActionResult> UpdatePhoneNumber([FromBody] LUpdateVerificationCodeDto dto)
        {
            var token = await _service.LoginFromVerifyCode(dto);
            return Ok(token);
        }

        [HttpPut("resetVerifyCode")]
        public async Task<ActionResult> ResetVerificationCode([FromBody] LUpdateVerificationCodeDto dto)
        {
            await _service.Reset(dto);
            return Ok("The verification code was renewed and sent again.");
        }
        
        [HttpPut("settings/{id}")]
        public async Task<ActionResult> UpdateUser([FromBody] LUpdateUserSettingsDto dto, [FromRoute] int id)
        {
            await _service.UpdateSettings(dto, id);
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