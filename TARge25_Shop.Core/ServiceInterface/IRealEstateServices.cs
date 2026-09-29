using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;

namespace TARge25_Shop.Core.ServiceInterface
{
    public interface IRealEstateServices
    {
        Task<RealEstate> Create(RealEstateDto dto);
        Task<RealEstate> Update(RealEstateDto dto);
        Task<RealEstate> DetailAsync(Guid id);
        Task<RealEstate> Delete(Guid id);
    }
}
