using AutoMapper;
using GoldenCastle.Govhack2024.Model.Api;
using GoldenCastle.Govhack2024.Model.Dto;

namespace GoldenCastle.Govhack2024.Middleware;

public class PropertyProfile : Profile
{
    public PropertyProfile()
    {
        CreateMap<SearchPropertyResponse, SearchPropertyResponseDto>();
        CreateMap<SearchPropertyResult, SearchPropertyResultDto>();
        
        CreateMap<PropertyBoundaries, PropertyBoundariesDto>();
        CreateMap<PropertyBoundaryPoint, PropertyBoundaryPointDto>();

        CreateMap<(FindPropertyResponse, GetPropertyDetailsResponse ,GetPropertyBoundariesResponse), GetPropertyDetailsResponseDto>()
            .ForMember(nameof(GetPropertyDetailsResponseDto.Card),
                options => options.MapFrom((source) => source.Item1.Card))
            .ForMember(nameof(GetPropertyDetailsResponseDto.Property),
                options => options.MapFrom((source) => source.Item2.Property))
            .ForMember(nameof(GetPropertyDetailsResponseDto.PropertyBoundaries),
            options => options.MapFrom((source) => source.Item3.PropertyBoundaries));
    }
}