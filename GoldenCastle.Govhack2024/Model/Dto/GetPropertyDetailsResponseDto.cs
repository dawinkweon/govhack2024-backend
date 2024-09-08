namespace GoldenCastle.Govhack2024.Model.Dto;

public class GetPropertyDetailsResponseDto
{
    public object Card { get; set; }
    public object Property { get; set; }
    
    public PropertyBoundariesDto[] PropertyBoundaries { get; set; }
}