using System.Text.Json.Serialization;

namespace TARge25_Shop.Core.Dto
{
    public class OpenWeatherDto
    {
        public double Lon {  get; set; }
        public double Lat {  get; set; }
    }

    public class OpenWeatherRootDto
    {
        [JsonPropertyName("weather")]
        public List<OpenWeatherDto> Weather { get; set; }

        [JsonPropertyName("main")]
        public List<OpenWeatherDto> Main { get; set; }

        [JsonPropertyName("wind")]
        public List<OpenWeatherDto> Wind { get; set; }

        [JsonPropertyName("clouds")]
        public List<OpenWeatherDto> Clouds { get; set; }



    }
}
