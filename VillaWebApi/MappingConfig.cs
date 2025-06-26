using AutoMapper;
using VillaWebApi.Models;
using VillaWebApi.Models.DTOs;

namespace VillaWebApi;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDTO>().ReverseMap();
        CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
        CreateMap<Villa, VillaCreateDTO>().ReverseMap();
    }
}
 
