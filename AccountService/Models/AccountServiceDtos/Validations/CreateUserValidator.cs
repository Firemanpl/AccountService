using System.Linq;
using AccountService.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;


namespace AccountService.Models.AccountServiceDtos.Validations
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator(AccountDbContext dbContext)
        {
            RuleFor(v => v.VerificationCode).NotEmpty().Must(c => c.Length == 8).WithMessage("VerificationCode must have 8 numbers.");
            RuleFor(n => n.Nationality).NotNull().NotEmpty();
            RuleFor(p => p.PostalCode).Matches("[0-99]-[0-999]").WithMessage("Postal code not a valid.");
        }
        
    }
}