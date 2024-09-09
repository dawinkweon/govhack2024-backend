using System.Text;
using System.Text.Json;
using AutoMapper;
using GoldenCastle.Govhack2024.Api;
using GoldenCastle.Govhack2024.Model.Api;
using GoldenCastle.Govhack2024.Model.Dto;

namespace GoldenCastle.Govhack2024.Service;

public class PropertyService : IPropertyService
{
    private readonly ILogger<PropertyService> _logger;
    private readonly IHomesApi _homesApi;
    private readonly IHomesGatewayApi _homesGatewayApi;
    private readonly IMapper _mapper;

    public PropertyService(ILogger<PropertyService> logger, IHomesApi homesApi, IHomesGatewayApi homesGatewayApi, IMapper mapper)
    {
        _logger = logger;
        _homesApi = homesApi;
        _homesGatewayApi = homesGatewayApi;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<SearchPropertyResultDto>> SearchProperties(string address)
    {
        SearchPropertyResponse apiResponse = await _homesApi.SearchProperty(address);
        _logger.LogDebug("From the api: {}", JsonSerializer.Serialize(apiResponse));

        return apiResponse.Results
            .Where(IsValid)
            .Select(result => _mapper.Map<SearchPropertyResultDto>(result)!);
    }

    public async Task<GetPropertyDetailsResponseDto> FindProperty(string city, string suburb, string address,
        int streetNumber)
    {
        FindPropertyResponse property = await _homesApi.FindProperty(city, suburb, streetNumber, address);

        GetPropertyDetailsResponse propertyDetails =
            await _homesGatewayApi.GetPropertyDetails(property.Card.PropertyId);
        GetPropertyBoundariesResponse propertyBoundaries = await _homesApi.GetPropertyBoundaries(
            property.Card.Point.Lat, property.Card.Point.Lon, streetNumber);

        return _mapper.Map<GetPropertyDetailsResponseDto>((property, propertyDetails, propertyBoundaries))!;
    }
    
    private static bool IsValid(SearchPropertyResult searchPropertyResult)
    {
        return !string.IsNullOrEmpty(searchPropertyResult.Title) &&
               !string.IsNullOrEmpty(searchPropertyResult.Address) &&
               !string.IsNullOrEmpty(searchPropertyResult.City) && searchPropertyResult.StreetNumber.HasValue &&
               !string.IsNullOrEmpty(searchPropertyResult.Suburb);
    }
}