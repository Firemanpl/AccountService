using AccountService.Entities;
using AccountService.Services.AccountLoginModule.Models;
using AutoMapper;

namespace AccountService.Services.AccountLoginModule
{
    public class LoginMappingProfile : Profile
    {
        public LoginMappingProfile()
        {
            CreateMap<LUpdateUserSettingsDto, User>()
                .ForPath(m => m.Address.City, c => c.MapFrom(s => s.City))
                .ForPath(m => m.Address.Street, c => c.MapFrom(s => s.Street))
                .ForPath(m => m.Address.PostalCode, c => c.MapFrom(s => s.PostalCode));
        }
    }
}