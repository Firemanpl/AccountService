using System.Linq;
using AccountService.Entities;
using AccountService.Services.AccountLoginModule.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Services.AccountLoginModule.Validators
{
    public class LoginUserValidator: AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator(AccountDbContext dbContext)
        {
            RuleFor(n => n.Nationality).NotNull().NotEmpty().Must(n=>n.Length==2).WithMessage("Nationality must has two letters.(ISO)");
            RuleFor(x => x.PhoneNumber).MustAsync(async (dto, phone, context, something) =>
            {
                var verifyCodeInUse = await dbContext.Users.AnyAsync(u =>
                    u.PhoneNumber == dto.PhoneNumber && u.Nationality == dto.Nationality);
                if (verifyCodeInUse)
                {
                    context.AddFailure("PhoneNumber", "This PhoneNumber with nationality is already taken.");
                }

                if (phone.Length != 9)
                {
                    context.AddFailure("PhoneNumber", "PhoneNumber must have 9 digits.");
                }

                return true;
            }).NotEmpty().NotNull().Matches(@"^\d[2-9]{9}$");
            // RuleFor(v => v.VerificationCode).NotEmpty().Matches(@"^\d[0-9]{7}$").WithMessage("VerificationCode must have 8 numbers.");
        }
    }
}