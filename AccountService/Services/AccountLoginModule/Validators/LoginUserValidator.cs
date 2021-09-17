using System.Globalization;
using System.Linq;
using AccountService.Entities;
using AccountService.Services.AccountLoginModule.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace AccountService.Services.AccountLoginModule.Validators
{
    public class LoginUserValidator : AbstractValidator<LoginUserDto>
    {

        public LoginUserValidator(AccountDbContext dbContext, IUserContextService userContextService)
        {
            
            RuleFor(n => n.Nationality).NotNull().NotEmpty().Must(n=>n.Length==3).WithMessage("Nationality must has two letters.(ISO)");
            RuleFor(x => x.PhoneNumber).MustAsync(async (dto, phone, context, something) =>
            {
                var verifyCodeInUse = await dbContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber && u.Nationality == dto.Nationality);
                if (verifyCodeInUse is not null && userContextService.GetUserId != verifyCodeInUse.Id)
                {
                    context.AddFailure("PhoneNumber", "This PhoneNumber with nationality is already taken.");
                }
                return true;
            }).NotEmpty().Matches(@"^\d[2-9]{8}$").WithMessage("PhoneNumber must have 9 digits.");
        }
    }
}