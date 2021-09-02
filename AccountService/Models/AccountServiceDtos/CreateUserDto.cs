using System.ComponentModel.DataAnnotations;

namespace AccountService.Models.AccountServiceDtos
{
    public class CreateUserDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string VerificationCode { get; set; }
        public string Nationality { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }
}