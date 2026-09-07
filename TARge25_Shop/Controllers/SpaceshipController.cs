using Microsoft.AspNetCore.Mvc;

namespace TARge25_Shop.Controllers
{
    public class SpaceshipController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
