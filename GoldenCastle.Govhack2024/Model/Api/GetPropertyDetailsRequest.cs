using System.ComponentModel.DataAnnotations;

namespace GoldenCastle.Govhack2024.Model.Api;

public class GetPropertyDetailsRequest
{
    [Required]
    public string City { get; set; }
    [Required]
    public string Suburb { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public int StreetNumber { get; set; }
}