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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchWeather (OpenWeatherSearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("GetWeather", "OpenWeather", new { city = model.City });
            }
            return View(model);
        }
    }
}
