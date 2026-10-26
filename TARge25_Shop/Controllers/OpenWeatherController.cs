using Microsoft.AspNetCore.Mvc;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Models.OpenWeather;

namespace TARge25_Shop.Controllers
{
    public class OpenWeatherController : Controller
    {
        private readonly IOpenWeatherServices _openWeatherServices;

        // Konstruktor
        public OpenWeatherController(IOpenWeatherServices openWeatherServices)
        {
            _openWeatherServices = openWeatherServices;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchCity(OpenWeatherSearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                // RedirectToAction esimene osa suunab lehehele ja funktsioonile, teiene nätab mis controller, kolmas on lisatud väärtused "?city=Tallinn"
                return RedirectToAction("SearchResult", "OpenWeather", new { city = model.City });
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SearchResult(string city)
        {
            //var dto = new OpenWeatherDto { Name = city };
            var dto = await _openWeatherServices.OpenWeatherApiCall(city);

            if (dto == null) return RedirectToAction(nameof(Index));

            var weather = dto.Weather?.FirstOrDefault();

            var vm = new OpenWeatherViewModel
            {
                WeatherMain = dto.Weather[0]?.Main,
                WeatherDescription = dto.Weather[0]?.Description,
                WeatherIcon = dto.Weather[0]?.Icon,
                MainTemp = dto.Main.Temp,
                MainFeelsLike = dto.Main.FeelsLike,
                MainPressure = dto.Main.Pressure,
                MainHumidity = dto.Main.Humidity,
                Visibility = dto.Visibility,
                WindSpeed = dto.Wind.Speed,
                WindDeg = dto.Wind.Deg,
                CloudsAll = dto.Clouds.All,
                DateTime = dto.Dt,
                SysCountry = dto.Sys.Country,
                SysSunrise = dto.Sys.Sunrise,
                SysSunset = dto.Sys.Sunset,
                Timezone = dto.Timezone,
                Name = dto.Name,
            };
            return View(vm);
        }
    }
}
