namespace AccountService.Services.AccountLoginModule.Models
{
    public class LUpdateVerificationCodeDto
    {
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; } 
        public string VerificationCode { get; set; }
    }
}