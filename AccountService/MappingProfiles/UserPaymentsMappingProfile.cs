using AccountService.Entities;
using AccountService.Models;
using AutoMapper;


namespace AccountService.MappingProfiles
{
    public class UserPaymentsMappingProfile : Profile
    {
        public UserPaymentsMappingProfile()
        {
            CreateMap<UserPayments, UserPaymentsDto>()
                .ForMember(m => m.Currency, c => c.MapFrom(s => s.Currency))
                .ForMember(m => m.Kilometers, c => c.MapFrom(s => s.Kilometers))
                .ForMember(m => m.Payment, c => c.MapFrom(s => s.Payment))
                .ForMember(m => m.KWh, c => c.MapFrom(s => s.KWh))
                .ForMember(m => m.VehicleId, c => c.MapFrom(s => s.VehicleId))
                .ForMember(m => m.UserId, c => c.MapFrom(s => s.UserId));
        }

    }
}