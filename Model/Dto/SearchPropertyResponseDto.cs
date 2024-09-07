namespace GoldenCastle.Govhack2024.Model.Dto;


public class SearchPropertyResponseDto
{
    public ResultDto[] Results { get; set; }
};

public class ResultDto
{
    public string Title { get; set; }
    public string City { get; set; }
    public string Suburb { get; set; }
    
    public string Address { get; set; }
    public int StreetNumber { get; set; }
    public string PropertyId { get; set; }
}