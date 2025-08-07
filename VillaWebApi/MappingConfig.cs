using AutoMapper;
using VillaModels.Models;
using VillaModels.Models.DTOs.AdminManagementDTOs;
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
            .ForMember(dest => dest.UserFullName,
                opt => opt.MapFrom(src => src.ApplicationUser.FirstName + " " + src.ApplicationUser.LastName))
            .ForMember(dest => dest.VillaSpecialDetails,
                opt => opt.MapFrom(src => src.VillaNumber.SpetialDeatils))
            .ForMember(dest => dest.VillaName,
                opt => opt.MapFrom(src => src.VillaNumber.Villa.Name))
            .ForMember(dest => dest.VillaRate,
                opt => opt.MapFrom(src => src.VillaNumber.Villa.rate))
            .ForMember(dest => dest.VillaImageUrl,
                opt => opt.MapFrom(src => src.VillaNumber.Villa.ImageUrl));
        CreateMap<Booking, BookingCreateDTO>().ReverseMap();
        CreateMap<Booking, BookingUpdateDTO>().ReverseMap();
        
        CreateMap<Company, CompanyDTO>().ReverseMap();
        CreateMap<Company, CompanyCreateDTO>().ReverseMap();
        CreateMap<Company, CompanyUpdateDTO>().ReverseMap();
        CreateMap<Company, PendingCompanyDTO>();

        CreateMap<RegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.DateOfBirth,
                opt => opt.MapFrom(src => src.BirthOfDate ?? DateTime.MinValue))
            .ReverseMap();
    }
}
 
