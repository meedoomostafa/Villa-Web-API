using AutoMapper;
using AppModels.Models;
using AppModels.Models.DTOs.BookingDTOs;

namespace AppService.MappingProfiles;

public class BookingMapping : Profile
{
    public BookingMapping()
    {
        CreateMap<Booking, BookingDTO>()
            .ForMember(dest => dest.CustomerFullName,
                opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.VillaSpecialDetails,
                opt => opt.MapFrom(src => src.VillaNumber.SpecialDetails))
            .ForMember(dest => dest.VillaName,
                opt => opt.MapFrom(src => src.VillaNumber.Villa.Name))
            .ForMember(dest => dest.VillaImageUrl,
                opt => opt.MapFrom(src => src.VillaNumber.Villa.ImageUrl));
        CreateMap<Booking, BookingCreateDTO>().ReverseMap();
        CreateMap<Booking, BookingUpdateDTO>().ReverseMap();
    }
}