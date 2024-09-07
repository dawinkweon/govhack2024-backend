using GoldenCastle.Govhack2024.Model.Api;
using GoldenCastle.Govhack2024.Model.Dto;
using Refit;
using SearchPropertyResponse = GoldenCastle.Govhack2024.Model.Api.SearchPropertyResponse;

namespace GoldenCastle.Govhack2024.Api;

public interface IHomesGatewayApi
{
    [Get("/details?property_id={propertyId}")]
    [Headers("Content-Type: application/json")]
    Task<GetPropertyDetailsResponse> GetPropertyDetails(string propertyId);
}