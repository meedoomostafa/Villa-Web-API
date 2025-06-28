using AutoMapper;
using VillaModels.Models;
using VillaModels.Models.DTOs.VillaDTOs;
using VillaModels.Models.DTOs.VillaNumberDTOs;

namespace VillaWebApi;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDTO>().ReverseMap();
        CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        CreateMap<Villa, VillaCreateDTO>().ReverseMap();
        
        CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
    }
}
 
