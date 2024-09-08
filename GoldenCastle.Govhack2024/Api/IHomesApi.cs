using GoldenCastle.Govhack2024.Model.Api;
using GoldenCastle.Govhack2024.Model.Dto;
using Refit;
using SearchPropertyResponse = GoldenCastle.Govhack2024.Model.Api.SearchPropertyResponse;

namespace GoldenCastle.Govhack2024.Api;

public interface IHomesApi
{
    [Get("/address/search?Address={address}")]
    [Headers("Content-Type: application/json")]
    Task<SearchPropertyResponse> SearchProperty(string address);
    
    [Get("/property?url=/{city}/{suburb}/{streetNumber}-{address}/GKeek")]
    [Headers("Content-Type: application/json")]
    Task<FindPropertyResponse> FindProperty(string city, string suburb, string streetNumber, string address);
    
    [Get("/linz/boundary/point?lat={lat}&lon={lon}&street_number={streetNumber}")]
    [Headers("Content-Type: application/json")]
    Task<GetPropertyBoundariesResponse> GetPropertyBoundaries(string lat, string lon, string streetNumber);
}