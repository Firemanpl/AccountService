using AccountService.Entities;
using AccountService.Services.AccountRegistrationModule.Models;
using AutoMapper;

namespace AccountService.Services.AccountRegistrationModule
{
    public class RegisterMappingProfile : Profile
    {
        public RegisterMappingProfile()
        {
            CreateMap<RUpdateUserSettingsDto, User>()
                .ForMember(r => r.Address, c => c.MapFrom(dto => 
                    new Address() { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));
        }
    }
}