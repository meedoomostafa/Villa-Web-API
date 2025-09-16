using AutoMapper;
using VillaModels.Models;
using VillaModels.Models.DTOs.VillaNumberDTOs;

namespace AppService.MappingProfiles;

public class VillaNumberMapping : Profile
{
    public VillaNumberMapping()
    {
        CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
        CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
    }
}