using Microsoft.AspNetCore.Mvc;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Models.Cocktail;

namespace TARge25_Shop.Controllers
{
    public class CocktailController : Controller
    {
        private readonly ICocktailServices _cocktailServices;

        // Konstruktor
        public CocktailController(ICocktailServices cocktailServices)
        {
            _cocktailServices = cocktailServices;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchCocktail(CocktailSearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Cocktail", "Cocktail", new { cocktail = model.CocktailName });
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Cocktail(string cocktail)
        {
            var dto = new CocktailDto { StrDrink = cocktail};

            await _cocktailServices.CocktailRootDto(dto);

            var vm = new CocktailViewModel
            {
                Drink = dto.StrDrink ?? cocktail,
                IdDrink = dto.IdDrink,
                StrAlcoholic = dto.StrAlcoholic,
                StrGlass = dto.StrGlass,
                StrInstructions = dto.StrInstructions,
                StrDrinkThumb = dto.StrDrinkThumb,
                StrIngredient1 = dto.StrIngredient1,
                StrIngredient2 = dto.StrIngredient2,
                StrIngredient3 = dto.StrIngredient3,
                StrIngredient4 = dto.StrIngredient4,
                StrIngredient5 = dto.StrIngredient5,
                StrIngredient6 = dto.StrIngredient6,
                StrIngredient7 = dto.StrIngredient7,
                StrIngredient8 = dto.StrIngredient8,
                StrIngredient9 = dto.StrIngredient9,
                StrIngredient10 = dto.StrIngredient10,
                StrIngredient11 = dto.StrIngredient11,
                StrIngredient12 = dto.StrIngredient12,
                StrIngredient13 = dto.StrIngredient13,
                StrIngredient14 = dto.StrIngredient14,
                StrIngredient15 = dto.StrIngredient15,
                StrMeasure1 = dto.StrMeasure1,
                StrMeasure2 = dto.StrMeasure2,
                StrMeasure3 = dto.StrMeasure3,
                StrMeasure4 = dto.StrMeasure4,
                StrMeasure5 = dto.StrMeasure5,
                StrMeasure6 = dto.StrMeasure6,
                StrMeasure7 = dto.StrMeasure7,
                StrMeasure8 = dto.StrMeasure8,
                StrMeasure9 = dto.StrMeasure9,
                StrMeasure10 = dto.StrMeasure10,
                StrMeasure11 = dto.StrMeasure11,
                StrMeasure12 = dto.StrMeasure12,
                StrMeasure13 = dto.StrMeasure13,
                StrMeasure14 = dto.StrMeasure14,
                StrMeasure15 = dto.StrMeasure15
            };

            return View(vm);
        }
    }
}
