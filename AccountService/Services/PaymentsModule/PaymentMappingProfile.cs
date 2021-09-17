using AccountService.Entities;
using AccountService.Models;
using AccountService.Models.PaymentServiceDtos;
using AutoMapper;

namespace AccountService.Services.PaymentsModule
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            // PaymentService
            CreateMap<User, UserDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));
            CreateMap<UserPayments,UserPaymentsDto>();
            CreateMap<CreatePaymentDto,UserPayments>();
            
        }
    }
}