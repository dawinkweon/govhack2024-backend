using System.Text.Json.Serialization;

namespace GoldenCastle.Govhack2024.Model.Api;

public class SearchPropertyResponse
{
    [JsonPropertyName("Results")]
    public Result[] Results { get; set; }
}

public class Result
{
    [JsonPropertyName("Title")]
    public string Title { get; set; }
    [JsonPropertyName("City")]
    public string City { get; set; }
    [JsonPropertyName("Suburb")]
    public string Suburb { get; set; }
    [JsonPropertyName("Street")]
    public string Address { get; set; }
    [JsonPropertyName("StreetNumber")]
    public int? StreetNumber { get; set; }
}