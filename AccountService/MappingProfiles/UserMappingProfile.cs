using AccountService.Entities;
using AccountService.Models;
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
                .ForMember(r => r.Address,
                    c => c.MapFrom(dto => new Address()
                        { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));
        }
    }
}