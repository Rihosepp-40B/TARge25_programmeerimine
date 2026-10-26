using TARge25_Shop.Core.Dto;

namespace TARge25_Shop.Core.ServiceInterface
{
    public interface IOpenWeatherServices
    {
        Task<OpenWeatherDto> OpenWeatherApiCall(string city);
    }
}
