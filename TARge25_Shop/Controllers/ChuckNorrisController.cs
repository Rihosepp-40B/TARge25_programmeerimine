using Microsoft.AspNetCore.Mvc;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Models.ChuckNorris;

namespace TARge25_Shop.Controllers
{
    public class ChuckNorrisController : Controller
    {
        private readonly IChuckNorrisServices _chuckNorrisServices;

        //Konstuktor
        public ChuckNorrisController(IChuckNorrisServices chuckNorrisServices)
        {
            _chuckNorrisServices = chuckNorrisServices;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
                
        public IActionResult SearchChuckNorrisJoke()
        {
            return RedirectToAction(nameof(Joke));
        }

        [HttpGet]
        public async Task<IActionResult> Joke()
        {
            var joke = await _chuckNorrisServices.ChuckNorrisResultHttpClient();

            ChuckNorrisViewModel vm = new();

            //vm.Categories = joke.Categories;
            vm.CreatedAt = joke.CreatedAt;
            vm.IconUrl = joke.IconUrl;
            vm.Id = joke.Id;
            vm.UpdatedAt = joke.UpdatedAt;
            vm.Url = joke.Url;
            vm.Value = joke.Value;

            return View(vm);
        }
    }
}
