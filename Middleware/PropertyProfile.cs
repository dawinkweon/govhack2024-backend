using AutoMapper;
using GoldenCastle.Govhack2024.Model.Api;
using GoldenCastle.Govhack2024.Model.Dto;
using Result = GoldenCastle.Govhack2024.Model.Api.Result;

namespace GoldenCastle.Govhack2024.Middleware;

public class PropertyProfile : Profile
{
    public PropertyProfile()
    {
        CreateMap<SearchPropertyResponse, SearchPropertyResponseDto>();
        CreateMap<Result, ResultDto>().ForMember(nameof(ResultDto.PropertyId), options => options.Ignore());
        CreateMap<GetPropertyDetailsResponse, GetPropertyDetailsResponseDto>();
    }
}