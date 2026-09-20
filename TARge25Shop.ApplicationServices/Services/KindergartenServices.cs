
using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        public readonly TARge25_ShopContext _context;

        public KindergartenServices
            (
                TARge25_ShopContext context
            )
        {
            _context = context;
        }
    }
}
