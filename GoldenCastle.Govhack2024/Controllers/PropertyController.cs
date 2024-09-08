using System.Globalization;
using System.Text;
using System.Text.Json;
using AutoMapper;
using GoldenCastle.Govhack2024.Api;
using GoldenCastle.Govhack2024.Model.Api;
using GoldenCastle.Govhack2024.Model.Dto;
using Microsoft.AspNetCore.Mvc;

namespace GoldenCastle.Govhack2024.Controllers;

[ApiController]
[Route("[controller]")]
public class PropertyController : ControllerBase
{
    private readonly ILogger<PropertyController> _logger;
    private readonly IHomesApi _homesApi;
    private readonly IHomesGatewayApi _homesGatewayApi;
    private readonly IMapper _mapper;

    public PropertyController(ILogger<PropertyController> logger, IHomesApi homesApi, IHomesGatewayApi homesGatewayApi, IMapper mapper)
    {
        _logger = logger;
        _homesApi = homesApi;
        _homesGatewayApi = homesGatewayApi;
        _mapper = mapper;
    }

    [HttpGet("/Search")]
    public async Task<IEnumerable<SearchPropertyResultDto>> Search([FromQuery] string address)
    {
        SearchPropertyResponse apiResponse = await _homesApi.SearchProperty(address);
        _logger.LogDebug("From the api: {}", JsonSerializer.Serialize(apiResponse));

        return apiResponse.Results
            .Where(IsValid)
            .Select(result => _mapper.Map<SearchPropertyResultDto>(result)!);
    }

    private bool IsValid(SearchPropertyResult searchPropertyResult)
    {
        return !string.IsNullOrEmpty(searchPropertyResult.Title) && !string.IsNullOrEmpty(searchPropertyResult.Address) && !string.IsNullOrEmpty(searchPropertyResult.City) && searchPropertyResult.StreetNumber.HasValue && !string.IsNullOrEmpty(searchPropertyResult.Suburb);
    }

    [HttpPost("/GetDetails")]
    public async Task<GetPropertyDetailsResponseDto> Get([FromBody] GetPropertyDetailsRequest request)
    {
        FindPropertyResponse property = await _homesApi.FindProperty(
            Normalise(request.City).ToLower().Replace(" ", "-"),
            Normalise(request.Suburb).ToLower().Replace(" ", "-"),
            request.StreetNumber.ToString().ToLower().Replace(" ", "-"), 
            Normalise(request.Address).ToLower().Replace(" ", "-"));
        
        GetPropertyDetailsResponse propertyDetails = await _homesGatewayApi.GetPropertyDetails(property.Card.PropertyId);

        GetPropertyBoundariesResponse propertyBoundaries = await _homesApi.GetPropertyBoundaries(property.Card.Point.Lat.ToString(),
            property.Card.Point.Lon.ToString(), request.StreetNumber.ToString());

        return _mapper.Map<GetPropertyDetailsResponseDto>((property, propertyDetails, propertyBoundaries))!;
    }
    
    private static string Normalise(string value)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        byte[] tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(value);
        string asciiStr = Encoding.UTF8.GetString(tempBytes);
        return asciiStr;
    }
}
