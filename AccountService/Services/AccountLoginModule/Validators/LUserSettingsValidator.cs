using System.Linq;
using AccountService.Entities;
using AccountService.Services.AccountLoginModule.Models;
using FluentValidation;

namespace AccountService.Services.AccountLoginModule.Validators
{
    public class LUserSettingsValidator : AbstractValidator<LUserSettingsDto>
    {
        public LUserSettingsValidator(AccountDbContext dbContext)
        {
            RuleFor(p => p.Email).Custom((value, context) =>
            {
                var verifyCodeInUse = dbContext.Users.Any(u => u.PhoneNumber == value);
                if (verifyCodeInUse)
                {
                    context.AddFailure("Email", "This Email is already taken.");
                }
            }).NotEmpty().NotNull();
            
            RuleFor(p => p.PostalCode).Matches(@"^\d{2}-\d{3}$").WithMessage("Postal code not a valid.");
        }
    }
}  