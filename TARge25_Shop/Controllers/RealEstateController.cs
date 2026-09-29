using Microsoft.AspNetCore.Mvc;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;
using TARge25_Shop.Models.RealEstate;

namespace TARge25_Shop.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly IRealEstateServices _realEstateServices;
        private readonly TARge25_ShopContext _context;

        public RealEstateController
            (
            IRealEstateServices realEstateServices,
            TARge25_ShopContext context)
        {
            _realEstateServices = realEstateServices;
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.RealEstates
                .Select(x => new RealEstateIndexViewModel
                {
                    Id = x.Id,
                    Area = x.Area,
                    Location = x.Location,
                    RoomNumber = x.RoomNumber,
                    BuildingType = x.BuildingType,
                    CreatedAt = x.CreatedAt,
                });
            return View(result);
        }
    }
}
