using System.Linq;
using AccountService.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace AccountService.Models.AccountServiceDtos.Validations
{
    public class UpdateUserValidator :AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator(AccountDbContext dbContext)
        {
            RuleFor(p => p.PostalCode).Matches("[0-99]-[0-999]").WithMessage("Postal code not a valid.");
        }
    }
}