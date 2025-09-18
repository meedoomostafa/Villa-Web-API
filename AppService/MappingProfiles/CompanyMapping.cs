using AppModels.Models;
using AppModels.Models.DTOs.ProfilesDTOs;
using AutoMapper;

namespace AppService.MappingProfiles;

public class CompanyMapping : Profile
{
    public CompanyMapping()
    {
        // Mapping from the Company entity to the DTO for display.
        CreateMap<Company, CompanyProfileDTO>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.UserName : null))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.Email : null))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.ApplicationUser != null ? src.ApplicationUser.PhoneNumber : null));

        CreateMap<CompanyProfileDTO, Company>()
            .AfterMap((src, dest) =>
            {
                if (dest.ApplicationUser != null)
                    dest.ApplicationUser.PhoneNumber = src.PhoneNumber;
            });
    }
}
