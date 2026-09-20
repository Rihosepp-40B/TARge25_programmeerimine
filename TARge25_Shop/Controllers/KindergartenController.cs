using Microsoft.AspNetCore.Mvc;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;
using TARge25_Shop.Models.Kindergarten;

namespace TARge25_Shop.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly IKindergartenServices _kindergartenServices;
        private readonly TARge25_ShopContext _context;

        public KindergartenController
            (
            IKindergartenServices kindergartenServices,
            TARge25_ShopContext context)
        {
            _kindergartenServices = kindergartenServices;
            _context = context;
        }
        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    TeacherName = x.TeacherName,
                    CreatedAt = x.CreatedAt,
                });
            return View(result);
        }
    }
}
