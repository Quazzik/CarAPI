using AutoMapper;
using CarAPI.Models;
using CarAPI.DTOs;

namespace CarAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CarBrand, EntityDto>();
            CreateMap<TrimLevel, EntityDto>();
            CreateMap<CreateEntityDto, CarBrand>();
            CreateMap<CreateEntityDto, TrimLevel>();
            
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.CarBrandName,
                    opt => opt.MapFrom(src => src.CarBrand.Name))
                .ForMember(dest => dest.TrimLevelName,
                    opt => opt.MapFrom(src => src.TrimLevel.Name));
            CreateMap<CreateCarDto, Car>();
        }
    }
}