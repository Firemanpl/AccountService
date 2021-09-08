using System.ComponentModel.DataAnnotations;

namespace AccountService.Services.AccountRegistrationModule.Models
{
    public class RUpdateUserSettingsDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}