using AutoMapper;
using GoldenCastle.Govhack2024.Model.Api;
using GoldenCastle.Govhack2024.Model.Dto;
using GoldenCastle.Govhack2024.Service;
using Microsoft.AspNetCore.Mvc;

namespace GoldenCastle.Govhack2024.Controllers;

[ApiController]
[Route("[controller]")]
public class PropertyController : ControllerBase
{
    private readonly ILogger<PropertyController> _logger;
    private readonly IPropertyService _propertyService;
    private readonly IMapper _mapper;

    public PropertyController(ILogger<PropertyController> logger, IPropertyService propertyService, IMapper mapper)
    {
        _logger = logger;
        _propertyService = propertyService;
        _mapper = mapper;
    }

    [HttpGet("/Search")]
    public async Task<IEnumerable<SearchPropertyResultDto>> Search([FromQuery] string address)
    {
        return await _propertyService.SearchProperties(address);
    }

    [HttpPost("/GetDetails")]
    public async Task<GetPropertyDetailsResponseDto> Get([FromBody] GetPropertyDetailsRequest request)
    {
        return await _propertyService.FindProperty(request.City, request.Suburb, request.Address, request.StreetNumber);
    }
}
