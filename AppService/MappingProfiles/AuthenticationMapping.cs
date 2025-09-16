using AutoMapper;
using VillaModels.Models;
using VillaModels.Models.DTOs.AuthenticationDTOs;

namespace AppService.MappingProfiles;

public class AuthenticationMapping : Profile
{
    public AuthenticationMapping()
    {
        CreateMap<RegisterCustomerDTO,Customer>()
            .ForMember(dest => dest.DateOfBirth,
                opt => opt.MapFrom(src => src.BirthOfDate ?? DateTime.MinValue))
            .ForMember(dest => dest.Address,opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.FullName , opt => opt.MapFrom(src => src.FullName))
            .ReverseMap();

        CreateMap<RegisterCompanyDTO, Company>()
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
            .ForMember(dest => dest.CommercialRegistrationDocUrl,
                opt => opt.MapFrom(src => src.CommercialRegistrationDocUrl))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ReverseMap();

        CreateMap<ApplicationUser, RegisterCustomerDTO>().ReverseMap();
        CreateMap<ApplicationUser, RegisterCompanyDTO>().ReverseMap();
    }
}