using System.Threading.Tasks;
using AccountService.Entities;
using AccountService.Services.AccountLoginModule.Models;
using AccountService.Services.AccountLoginModule.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Services.AccountLoginModule
{
    [ApiController]
    [Authorize]
    [Route("api/account/login")]
    
    public class AccountLoginController : ControllerBase
    {
        private readonly IAccountLoginService _service;

        public AccountLoginController(IAccountLoginService service)
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpPost("SendAuthSms")]
        public async Task<ActionResult> LoginPhoneNumber([FromBody] LoginUserDto dto)
        {
            var result = await _service.SendVerifyCode(dto);
            if (result is true)
            {
                return Ok("Account created and VerifySms has sent.");
            }
            return Ok("Logged in and VerifySms has sent.");
        }
        [AllowAnonymous]
        [HttpPut]
        public async Task<ActionResult> GenerateJwtToken([FromBody] LVerificationCodeDto dto)
        {
            var token = await _service.LoginFromVerifyCode(dto);
            return Ok(token);
        }
        [AllowAnonymous]
        [HttpPut("resetVerifyCode")]
        public async Task<ActionResult> ResetVerificationCode([FromBody] LVerificationCodeDto dto)
        {
            await _service.Reset(dto);
            return Ok("The verification code was renewed and sent again.");
        }
    }
}