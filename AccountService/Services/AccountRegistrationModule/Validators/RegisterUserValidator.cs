using System.Linq;
using System.Threading.Tasks;
using AccountService.Entities;
using AccountService.Services.AccountRegistrationModule.Models;
using FluentValidation;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Services.AccountRegistrationModule.Validators
{
    public class RegisterUserValidator: AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(AccountDbContext dbContext)
        {
            RuleFor(n => n.Nationality).NotNull().NotEmpty().Must(n=>n.Length==2).WithMessage("Nationality must has two letters.(ISO)");
            RuleFor(x => x.PhoneNumber).MustAsync(async (dto, phone, context, something) =>
            {
                var verifyCodeInUse = await dbContext.Users.AnyAsync(u => u.PhoneNumber == dto.PhoneNumber && u.Nationality == dto.Nationality);
                if (verifyCodeInUse)
                {
                    context.AddFailure("PhoneNumber", "This PhoneNumber with nationality is already taken.");
                }
                if (phone.Length !=9)
                {
                    context.AddFailure("PhoneNumber","PhoneNumber must have 9 digits.");
                }
                return true;
            }).NotEmpty().NotNull().Matches(@"^\d[2-9]{10}$").WithMessage("PhoneNumber must have 9 digits.");;
            // RuleFor(v => v.VerificationCode).NotEmpty().Matches(@"^\d[0-9]{7}$").WithMessage("VerificationCode must have 8 numbers.");
        }


    }

}