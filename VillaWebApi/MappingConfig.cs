using AutoMapper;
using VillaModels.Models;
using VillaModels.Models.DTOs.AuthenticationDTOs;
using VillaModels.Models.DTOs.BookingDTOs;
using VillaModels.Models.DTOs.VillaDTOs;
using VillaModels.Models.DTOs.VillaNumberDTOs;

namespace VillaWebApi;

public sealed class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDTO>().ReverseMap();
        CreateMap<Villa, VillaWithVillaNumbersDTO>().ReverseMap();
        CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        CreateMap<Villa, VillaCreateDTO>().ReverseMap();
        
        CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
        
        CreateMap<Booking, BookingDTO>()
            .ForMember(dest => dest.CustomerFullName,
                opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.VillaSpecialDetails,
                opt => opt.MapFrom(src => src.VillaNumber.SpecialDetails))
            .ForMember(dest => dest.VillaName,
                opt => opt.MapFrom(src => src.VillaNumber.Villa.Name))
            .ForMember(dest => dest.VillaRate,
                opt => opt.MapFrom(src => src.VillaNumber.Villa.Rate))
            .ForMember(dest => dest.VillaImageUrl,
                opt => opt.MapFrom(src => src.VillaNumber.Villa.ImageUrl));
        CreateMap<Booking, BookingCreateDTO>().ReverseMap();
        CreateMap<Booking, BookingUpdateDTO>().ReverseMap();
        

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
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ReverseMap();

        CreateMap<ApplicationUser, RegisterCustomerDTO>().ReverseMap();
        CreateMap<ApplicationUser, RegisterCompanyDTO>().ReverseMap();
    }
}
 
