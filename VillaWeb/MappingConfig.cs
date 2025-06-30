using AutoMapper;
using VillaWeb.Models.DTOs.VillaNumberDTOs;
using VillaWeb.Models.DTOs.VillaDTOs;
namespace VillaWeb;

public sealed class MappingConfig :  Profile
{
    public MappingConfig()
    {
        CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
        CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
        
        CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
        CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
    }
}
