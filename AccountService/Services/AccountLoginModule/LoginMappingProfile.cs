using AccountService.Entities;
using AccountService.Services.AccountLoginModule.Models;
using AutoMapper;

namespace AccountService.Services.AccountLoginModule
{
    public class LoginMappingProfile : Profile
    {
        public LoginMappingProfile()
        {
            CreateMap<LUserSettingsDto, User>()
                .ForPath(m => m.Address.City, c => c.MapFrom(s => s.City))
                .ForPath(m => m.Address.Street, c => c.MapFrom(s => s.Street))
                .ForPath(m => m.Address.PostalCode, c => c.MapFrom(s => s.PostalCode));
            CreateMap<User, LUserSettingsDto>()
                .ForPath(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForPath(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForPath(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));
            CreateMap<LoginUserDto, User>()
                .ForMember(r => r.Address,
                    c => c.MapFrom(dto => new Address()
                        { City = null, PostalCode = null, Street = null }));
        }
    }
}