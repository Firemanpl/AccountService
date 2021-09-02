using AccountService.Entities;
using FluentValidation;

namespace AccountService.Models.PaymentServiceDtos.Validations
{
    public class CreatePaymentValidator:AbstractValidator<CreatePaymentDto>
    {
        public CreatePaymentValidator(AccountDbContext dbContext)
        {
            RuleFor(v => v.VehicleId).NotEmpty().NotNull();
            RuleFor(k => k.Kilometers).NotEmpty().NotNull();
            RuleFor(k => k.KWh).NotEmpty().NotNull();
            RuleFor(c=>c.Currency).NotEmpty().NotNull().Must(c=>c.Length==3);
            RuleFor(p => p.Payment).NotEmpty().NotNull();
        }
    }
}