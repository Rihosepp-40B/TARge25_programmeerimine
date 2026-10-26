using System.Text.Json;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class OpenWeatherServices : IOpenWeatherServices
    {
        private readonly HttpClient _httpClient;

        public OpenWeatherServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OpenWeatherDto> OpenWeatherApiCall(string city)
        {
            string APIkey = "83eaff76f7bbcf3a7d1891033bb84f4c";
            var urlWeather = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={APIkey}");

            if (!urlWeather.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await urlWeather.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                //Olek true laseb ignoreerida täpset tähe suurustundlikust JSON muutujate nimes
                PropertyNameCaseInsensitive = true,
            };
            var weather = JsonSerializer.Deserialize<OpenWeatherDto>(json, options);

            return weather!;
        }
    }
}
