using AccountService.Services.AccountLoginModule.Models;
using FluentValidation;

namespace AccountService.Services.AccountLoginModule.Validators
{
    public class LUpdateVerificationCodeValidator: AbstractValidator<LUpdateVerificationCodeDto>
    {
        public LUpdateVerificationCodeValidator()
        {
            RuleFor(v => v.VerificationCode).NotEmpty().Matches(@"^\d[0-9]{7}$").WithMessage("VerificationCode must have 8 numbers.");
        }
    }
}