using GoldenCastle.Govhack2024.Model.Api;
using GoldenCastle.Govhack2024.Model.Dto;

namespace GoldenCastle.Govhack2024.Service;

public interface IPropertyService
{
    Task<IEnumerable<SearchPropertyResultDto>> SearchProperties(string address);
    Task<GetPropertyDetailsResponseDto> FindProperty(string city, string suburb, string address, int streetNumber);
}