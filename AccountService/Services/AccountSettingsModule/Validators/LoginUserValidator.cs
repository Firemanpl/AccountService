using AccountService.Entities;
using AccountService.Services.AccountLoginModule.Models;
using FluentValidation;

namespace AccountService.Services.AccountSettingsModule.Validators
{
    public class SLoginUserValidator: AbstractValidator<SLoginUserDto>
    {
        public SLoginUserValidator(AccountDbContext dbContext)
        {
            RuleFor(n => n.Nationality).NotNull().NotEmpty().Must(n=>n.Length==2).WithMessage("Nationality must has two letters.(ISO)");
            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().Matches(@"^\d[2-9]{8}$").WithMessage("Phone must have 9 numbers.");
        }
    }
}