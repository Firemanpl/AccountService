using AccountService.Entities;
using AccountService.Services.AccountLoginModule.Models;
using AutoMapper;

namespace AccountService.Services.AccountSettingsModule
{
    public class SettingsMappingProfile : Profile
    {
        public SettingsMappingProfile()
        {
            CreateMap<SUserSettingsDto, User>()
                .ForPath(m => m.Address.City, c => c.MapFrom(s => s.City))
                .ForPath(m => m.Address.Street, c => c.MapFrom(s => s.Street))
                .ForPath(m => m.Address.PostalCode, c => c.MapFrom(s => s.PostalCode));
            CreateMap<User, SUserSettingsDto>()
                .ForPath(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForPath(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForPath(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

        }
    }
}