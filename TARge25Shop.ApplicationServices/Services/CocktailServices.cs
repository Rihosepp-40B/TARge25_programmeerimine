using System.Text.Json;
using System.Net;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class CocktailServices : ICocktailServices
    {
        public async Task<CocktailDto> CocktailRootDto(CocktailDto dto)
        {
            string urlDrink = $"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={dto.StrDrink}";

            using (WebClient client = new WebClient())
            {
                string json = await client.DownloadStringTaskAsync(urlDrink);
                CocktailRootDto cocktailRoot = JsonSerializer.Deserialize<CocktailRootDto>(json);

                if (cocktailRoot?.Cocktails == null || cocktailRoot.Cocktails.Count == 0)
                {
                    return null;
                }

                var cocktail = cocktailRoot.Cocktails[0];

                dto.IdDrink = cocktail.IdDrink;
                dto.StrDrink = cocktail.StrDrink;
                dto.StrDrinkAlternate = cocktail.StrDrinkAlternate;
                dto.StrTags = cocktail.StrTags;
                dto.StrVideo = cocktail.StrVideo;
                dto.StrCategory = cocktail.StrCategory;
                dto.StrIBA = cocktail.StrIBA;
                dto.StrAlcoholic = cocktail.StrAlcoholic;
                dto.StrGlass = cocktail.StrGlass;
                dto.StrInstructions = cocktail.StrInstructions;
                dto.StrInstructionsES = cocktail.StrInstructionsES;
                dto.StrInstructionsDE = cocktail.StrInstructionsDE;
                dto.StrInstructionsFR = cocktail.StrInstructionsFR;
                dto.StrInstructionsIT = cocktail.StrInstructionsIT;
                dto.StrInstructionsZhHans = cocktail.StrInstructionsZhHans;
                dto.StrInstructionsZhHant = cocktail.StrInstructionsZhHant;
                dto.StrDrinkThumb = cocktail.StrDrinkThumb;

                dto.StrIngredient1 = cocktail.StrIngredient1;
                dto.StrIngredient2 = cocktail.StrIngredient2;
                dto.StrIngredient3 = cocktail.StrIngredient3;
                dto.StrIngredient4 = cocktail.StrIngredient4;
                dto.StrIngredient5 = cocktail.StrIngredient5;
                dto.StrIngredient6 = cocktail.StrIngredient6;
                dto.StrIngredient7 = cocktail.StrIngredient7;
                dto.StrIngredient8 = cocktail.StrIngredient8;
                dto.StrIngredient9 = cocktail.StrIngredient9;
                dto.StrIngredient10 = cocktail.StrIngredient10;
                dto.StrIngredient11 = cocktail.StrIngredient11;
                dto.StrIngredient12 = cocktail.StrIngredient12;
                dto.StrIngredient13 = cocktail.StrIngredient13;
                dto.StrIngredient14 = cocktail.StrIngredient14;
                dto.StrIngredient15 = cocktail.StrIngredient15;

                dto.StrMeasure1 = cocktail.StrMeasure1;
                dto.StrMeasure2 = cocktail.StrMeasure2;
                dto.StrMeasure3 = cocktail.StrMeasure3;
                dto.StrMeasure4 = cocktail.StrMeasure4;
                dto.StrMeasure5 = cocktail.StrMeasure5;
                dto.StrMeasure6 = cocktail.StrMeasure6;
                dto.StrMeasure7 = cocktail.StrMeasure7;
                dto.StrMeasure8 = cocktail.StrMeasure8;
                dto.StrMeasure9 = cocktail.StrMeasure9;
                dto.StrMeasure10 = cocktail.StrMeasure10;
                dto.StrMeasure11 = cocktail.StrMeasure11;
                dto.StrMeasure12 = cocktail.StrMeasure12;
                dto.StrMeasure13 = cocktail.StrMeasure13;
                dto.StrMeasure14 = cocktail.StrMeasure14;
                dto.StrMeasure15 = cocktail.StrMeasure15;

                dto.StrImageSource = cocktail.StrImageSource;
                dto.StrImageAttribution = cocktail.StrImageAttribution;
                dto.StrCreativeCommonsConfirmed = cocktail.StrCreativeCommonsConfirmed;
                dto.DateModified = cocktail.DateModified;
            }
            return dto;
        }
    }
}
