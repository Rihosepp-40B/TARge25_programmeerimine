
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateServices
    {
        public readonly TARge25_ShopContext _context;

        public RealEstateServices(TARge25_ShopContext context)
        {
            _context = context;
        }
    }
}
