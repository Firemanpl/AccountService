using AccountService.Entities;
using AccountService.Models;
using AccountService.Models.AccountService;
using AccountService.Models.PaymentService;
using AutoMapper;


namespace AccountService.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // PaymentService
            CreateMap<User, UserDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));
            CreateMap<UserPayments,UserPaymentsDto>();
            CreateMap<CreatePaymentDto,UserPayments>();
            // AccountService
            CreateMap<CreateUserDto, User>()
                .ForMember(r => r.Address,
                    c => c.MapFrom(dto => new Address()
                        { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));
            CreateMap<UpdateUserDto, User>()
                .ForPath(m => m.Address.City, c => c.MapFrom(s => s.City))
                .ForPath(m => m.Address.Street, c => c.MapFrom(s => s.Street))
                .ForPath(m => m.Address.PostalCode, c => c.MapFrom(s => s.PostalCode));
        }
    }
}