using System.Text.Json;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class ChuckNorrisServices : IChuckNorrisServices
    {
        private readonly HttpClient _httpClient;

        public ChuckNorrisServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ChuckNorrisRootDto> ChuckNorrisResultHttpClient()
        {
            //var response on vaja ühendada https://api.chucknorris.io/jokes/random
            var response = await _httpClient.GetAsync("https://api.chucknorris.io/jokes/random");

            //Annab veateate, kui resoinse ei ole edukas
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                //Olek true laseb ignoreerida täpset tähe suurustundlikust JSON muutujate nimes
                PropertyNameCaseInsensitive = true,
            };

            //Muutuja var joke tuleb deserialiseerida
            var joke = JsonSerializer.Deserialize<ChuckNorrisRootDto>(json, options);

            //null andestav operaator(!) on turvaline viis kuna API alati tagastab JSON
            return joke!;
        }
    }
}
