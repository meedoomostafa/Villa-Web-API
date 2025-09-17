using AppModels.Models;
using AutoMapper;
using AppModels.Models;
using AppModels.Models.DTOs.AuthenticationDTOs;

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
            .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<ApplicationUser, RegisterCustomerDTO>().ReverseMap();
        CreateMap<ApplicationUser, RegisterCompanyDTO>().ReverseMap();
    }
}