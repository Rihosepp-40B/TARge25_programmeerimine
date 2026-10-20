using TARge25_Shop.Core.Dto;

namespace TARge25_Shop.Core.ServiceInterface
{
    public interface ICocktailServices
    {
        Task<CocktailDto> CocktailRootDto(CocktailDto dto);
    }
}
