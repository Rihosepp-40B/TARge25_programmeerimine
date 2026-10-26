namespace TARge25_Shop.Models.OpenWeather
{    
    public class OpenWeatherViewModel
    {
        public string? WeatherMain { get; set; }
        public string? WeatherDescription { get; set; }
        public string? WeatherIcon { get; set; }

        public double MainTemp { get; set; }
        public double? MainFeelsLike { get; set; }
        public int? MainPressure { get; set; }
        public int? MainHumidity { get; set; }

        public int? Visibility { get; set; }

        public double? WindSpeed { get; set; }
        public int? WindDeg { get; set; }

        public int? CloudsAll { get; set; }
        public int DateTime { get; set; }

        public string? SysCountry { get; set; }
        public long? SysSunrise { get; set; }
        public long SysSunset { get; set; }

        public int Timezone { get; set; }

        public string? Name { get; set; }

        //public string FormattedTemp => Main != null ? $"{Math.Round(Main.Temp)} °C" : string.Empty;

        public string IconUrl => !string.IsNullOrEmpty(WeatherIcon) ? $"https://openweathermap.org/img/wn/{WeatherIcon}@2x.png" : string.Empty;
    }
}
