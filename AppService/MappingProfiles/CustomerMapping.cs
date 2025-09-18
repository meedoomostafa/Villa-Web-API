using AppModels.Models;
using AppModels.Models.DTOs.ProfilesDTOs;
using AutoMapper;

namespace AppService.MappingProfiles;

public sealed class CustomerMapping : Profile
{
    public CustomerMapping()
    {
        CreateMap<Customer, CustomerProfileDTO>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.UserName : null))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.Email : null))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.PhoneNumber : null));
        
        CreateMap<CustomerProfileDTO, Customer>()
            .AfterMap((src, dest) =>
            {
                if (dest.ApplicationUser != null)
                    dest.ApplicationUser.PhoneNumber = src.PhoneNumber;
            });
    }
}