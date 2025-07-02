using AutoMapper;
using VillaModels.Models;
using VillaModels.Models.DTOs.AuthenticationDTOs;
using VillaModels.Models.DTOs.VillaDTOs;
using VillaModels.Models.DTOs.VillaNumberDTOs;

namespace VillaWebApi;

public sealed class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDTO>().ReverseMap();
        CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        CreateMap<Villa, VillaCreateDTO>().ReverseMap();
        
        CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();

        CreateMap<RegisterDTO, ApplicationUser>()
            .ForMember(dest => dest.DateOfBirth,
                opt => opt.MapFrom(src => src.BirthOfDate ?? DateTime.MinValue))
            .ReverseMap();
    }
}
 
