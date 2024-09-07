using System.Globalization;
using System.Text;
using System.Text.Json;
using AutoMapper;
using GoldenCastle.Govhack2024.Api;
using GoldenCastle.Govhack2024.Model.Api;
using GoldenCastle.Govhack2024.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Result = GoldenCastle.Govhack2024.Model.Api.Result;

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
    public async Task<GoldenCastle.Govhack2024.Model.Dto.ResultDto[]> Search([FromQuery] string address)
    {
        Model.Api.SearchPropertyResponse apiResponse = await _homesApi.SearchProperty(address);
        _logger.LogDebug("From the api: {}", JsonSerializer.Serialize(apiResponse));


        var rss = apiResponse.Results.Where(result =>
            {
                return !string.IsNullOrEmpty(result.Title)
                       && !string.IsNullOrEmpty(result.Address)
                       && !string.IsNullOrEmpty(result.City)
                       && result.StreetNumber.HasValue
                       && !string.IsNullOrEmpty(result.Suburb);
            })
            .Select(async result =>
            {
                var property = await _homesApi.FindProperty(
                    Normalise(result.City).ToLower().Replace(" ", "-"),
                    Normalise(result.Suburb).ToLower().Replace(" ", "-"),
                    result.StreetNumber.ToString().ToLower().Replace(" ", "-"),
                    Normalise(result.Address).ToLower().Replace(" ", "-"));
                var r = _mapper.Map<GoldenCastle.Govhack2024.Model.Dto.ResultDto>(result)!;
                _logger.LogDebug("Got PROPERTY: {}", JsonSerializer.Serialize(property));

                r.PropertyId = property.Card.PropertyId;
                return r;
            });
        var res = await Task.WhenAll(rss);
        _logger.LogDebug("Got properties: {}", JsonSerializer.Serialize(res));
        return res;
    }
    
    [HttpGet("{id}")]
    public async Task<GetPropertyDetailsResponseDto> Get(Guid id)
    {
        GetPropertyDetailsResponse property = await _homesGatewayApi.GetPropertyDetails(id.ToString());
        return _mapper.Map<GetPropertyDetailsResponseDto>(property)!;
    }
    
    private static string Normalise(string value)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        byte[] tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(value);
        string asciiStr = Encoding.UTF8.GetString(tempBytes);
        return asciiStr;
    }
}
