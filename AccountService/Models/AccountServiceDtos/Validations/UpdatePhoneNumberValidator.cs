using System.Linq;
using AccountService.Entities;
using AccountService.Services;
using FluentValidation;

namespace AccountService.Models.AccountServiceDtos.Validations
{
    public class UpdatePhoneNumberValidator: AbstractValidator<UpdatePhoneNumberDto>
    {
        public UpdatePhoneNumberValidator(AccountDbContext dbContext)
        {
            RuleFor(p => p.PhoneNumber).Custom((value, context) =>
            {
                var verifyCodeInUse = dbContext.Users.Any(u => u.PhoneNumber == value);
                if (verifyCodeInUse)
                {
                    context.AddFailure("PhoneNumber", "This PhoneNumber is already taken.");
                }
            }).NotEmpty().NotNull().Must(v=>v.Count()==9).Matches(@"[2-9]").WithMessage("PhoneNumber must have 9 digits."); 
        }
    }
}