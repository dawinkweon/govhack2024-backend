using System.Text.Json.Serialization;
using Refit;

namespace govhack2024_backend.Api;

public interface IHomesApi
{
    // [Get("/property?url=/{city}auckland/te-atatu-peninsula/594-te-atatu-road/GKeek")]
    [Get("/property?url=/{city}/{district}/{address}/GKeek")]
    [Headers("Content-Type: application/json")]
    Task<GetPropertyResponse> GetProperty(string city, string district, string address);
}

public class GetPropertyResponse
{
    public Card Card { get; set; }
};

public class Card
{
    public PropertyDetails PropertyDetails { get; set; }
    public Point point { get; set; }
}
public class PropertyDetails{}

public class Point
{
    public double Lat { get; set; }
    public double Lon { get; set; }
};
