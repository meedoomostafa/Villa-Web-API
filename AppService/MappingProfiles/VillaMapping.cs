using AutoMapper;
using AppModels.Models;
using AppModels.Models.DTOs.VillaDTOs;

namespace AppService.MappingProfiles;

public class VillaMapping : Profile
{
    public VillaMapping()
    {
        CreateMap<Villa, VillaDTO>().ReverseMap();
        CreateMap<Villa, VillaWithVillaNumbersDTO>().ReverseMap();
        CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        CreateMap<Villa, VillaCreateDTO>().ReverseMap();    
    }
}