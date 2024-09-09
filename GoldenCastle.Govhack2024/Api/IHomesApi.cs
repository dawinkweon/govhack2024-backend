using System.Text;
using GoldenCastle.Govhack2024.Model.Api;
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
    internal Task<FindPropertyResponse> FindPropertyInternal(string city, string suburb, string streetNumber,
        string address);

    Task<FindPropertyResponse> FindProperty(string city, string suburb, int streetNumber, string address)
    {
        return FindPropertyInternal(EncodeForHomesApi(city), EncodeForHomesApi(suburb),
            EncodeForHomesApi(streetNumber.ToString()), EncodeForHomesApi(address));
    }
    
    [Get("/linz/boundary/point?lat={lat}&lon={lon}&street_number={streetNumber}")]
    [Headers("Content-Type: application/json")]
    Task<GetPropertyBoundariesResponse> GetPropertyBoundaries(double lat, double lon, int streetNumber);
    
    private static string EncodeForHomesApi(string value)
    {
        return Normalise(value).ToLower().Replace(" ", "-");
    }

    private static string Normalise(string value)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        byte[] tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(value);
        string asciiStr = Encoding.UTF8.GetString(tempBytes);
        return asciiStr;
    }
}