using AccountService.Services.AccountLoginModule.Models;
using FluentValidation;

namespace AccountService.Services.AccountLoginModule.Validators
{
    public class SVerificationCodeValidator: AbstractValidator<SVerificationCodeDto>
    {
        public SVerificationCodeValidator()
        {
            RuleFor(v => v.VerificationCode).NotEmpty().Matches(@"^\d[0-9]{7}$").WithMessage("VerificationCode must have 8 numbers.");
        }
    }
}