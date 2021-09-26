using System;
using System.Globalization;
using System.Linq;
using AccountService.Entities;
using AccountService.Services.AccountLoginModule.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhoneNumbers;


namespace AccountService.Services.AccountLoginModule.Validators
{
    public class LoginUserValidator : AbstractValidator<LoginUserDto>
    {

        public LoginUserValidator(AccountDbContext dbContext)
        {
            
            RuleFor(n => n.Nationality).NotNull().NotEmpty().Must(n=>n.Length==2).WithMessage("Nationality must has two letters.(ISO)");
            RuleFor(x => x.PhoneNumber).Must((dto,phone,context) =>
            {
                try
                {
                    var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                    var isValidPhone = phoneNumberUtil.Parse(dto.PhoneNumber,dto.Nationality);
                    phoneNumberUtil.IsValidNumber(isValidPhone);
                }
                catch (Exception e)
                {
                    context.AddFailure("PhoneNumber", "Wrong NumberPhone or Nationality");
                }
                
                return true;
            }).NotEmpty().Matches(@"^\d[1-9]{8}$").WithMessage("PhoneNumber must have 9 digits.");
        }
    }
}