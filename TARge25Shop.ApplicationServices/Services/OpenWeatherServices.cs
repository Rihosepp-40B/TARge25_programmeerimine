using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class OpenWeatherServices : IOpenWeatherServices
    {
        public async Task<OpenWeatherDto> OpenWeatherApiCall(OpenWeatherDto dto)
        {
            string APIkey = "83eaff76f7bbcf3a7d1891033bb84f4c";
            string urlWeather = $"https://api.openweathermap.org/data/2.5/weather?q={dto.City}&units=metricppid={APIkey}";


        }
    }
}
