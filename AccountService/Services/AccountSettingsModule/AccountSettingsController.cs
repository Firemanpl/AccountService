using System.Threading.Tasks;
using AccountService.Services.AccountLoginModule.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Services.AccountSettingsModule
{
    [ApiController]
    [Authorize]
    [Route("api/account/settings")]
    public class AccountSettingsController : ControllerBase
    {
        private readonly IAccountSettingsService _service;

        public AccountSettingsController(IAccountSettingsService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult> GetUserSettings()
        {
            SUserSettingsDto result = await  _service.GetSettings();
            return Ok(result);
        }
        
        [HttpPut]
        public async Task<ActionResult> UpdateUserSettings([FromBody] SUserSettingsDto dto)
        {
            await _service.UpdateSettings(dto);
            return Ok("User updated.");
        }

        [HttpPut("changeNumberPhone")]
        public async Task<ActionResult> ChangeNumberPhone([FromBody] SLoginUserDto dto)
        {
            await _service.ChangePhoneNumberAsync(dto);
            return Ok();
        }

        [HttpPut("verifyChangePhoneNumber")]
        public async Task<ActionResult> ChangeVerifyPhoneNumber([FromBody]SVerificationCodeDto dto)
        {
            await _service.VerifyChangedPhoneNumberAsync(dto);
            return Ok();
        }
        
        [HttpDelete]
        public async Task<ActionResult> DeleteUser()
        {
            await _service.DeleteAsync();
            return NoContent();
        }
    }
}