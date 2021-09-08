using System.Threading.Tasks;
using AccountService.Services.AccountRegistrationModule.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Services.AccountRegistrationModule
{   
    [ApiController]
    [Route("api/account/")]
    
    public class AccountRegistrationController : ControllerBase
    {
        private readonly IAccountRegistrationService _service;

        public AccountRegistrationController(IAccountRegistrationService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAccount([FromBody] RegisterUserDto dto)
        { 
            var result =  await _service.RegisterAccount(dto);
            return Created($"api/account/{result}", null);;
        }
        
        [HttpPut("settings/{id}")]
        public async Task<ActionResult> CreateUserData([FromBody] RUpdateUserSettingsDto settingsDto, [FromRoute] int id)
        {
            await _service.CreateFirstSettings(settingsDto, id);
            return Ok("UserSettingsSaved.");
        }
    }
}