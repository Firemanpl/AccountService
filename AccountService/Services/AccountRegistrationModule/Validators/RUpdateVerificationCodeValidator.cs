using AccountService.Services.AccountRegistrationModule.Models;
using FluentValidation;

namespace AccountService.Services.AccountRegistrationModule.Validators
{
    public class RUpdateVerificationCodeValidator : AbstractValidator<RUpdateVerificationCodeDto>
    {
        public RUpdateVerificationCodeValidator()
        {
            RuleFor(v => v.VerificationCode).NotEmpty().Matches(@"^\d[0-9]{7}$").WithMessage("VerificationCode must have 8 numbers.");
        } 
    }
}