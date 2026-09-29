using Microsoft.AspNetCore.Mvc;
using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;
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

        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel result = new();

            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto
            {
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
            };

            //Teenuse väljakutsumine, et luua uus
            var result = await _realEstateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realEstate = await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            { return NotFound(); }

            var vm = new RealEstateCreateUpdateViewModel
            {
                Id = realEstate.Id,
                Area = realEstate.Area,
                Location = realEstate.Location,
                RoomNumber = realEstate.RoomNumber,
                BuildingType = realEstate.BuildingType,
                CreatedAt = realEstate.CreatedAt,
                ModifiedAt = realEstate.ModifiedAt

            };

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto
            {
                Id = vm.Id,
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt
            };

            var result = await _realEstateServices.Update(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var realEstate = await _realEstateServices.DetailAsync(Id);

            if (realEstate == null)
            {
                return NotFound();
            }

            //See on vaheinstants domaini ja vm vahel
            var vm = new RealEstateDeleteViewModel
            {
                Id = realEstate.Id,
                Area = realEstate.Area,
                Location = realEstate.Location,
                RoomNumber = realEstate.RoomNumber,
                BuildingType = realEstate.BuildingType,
                CreatedAt = realEstate.CreatedAt,
                ModifiedAt = realEstate.ModifiedAt
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var realEstate = await _realEstateServices.Delete(id);

            if (realEstate == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var realEstate = await _realEstateServices.DetailAsync(Id);

            if (realEstate == null)
            {
                return NotFound();
            }

            //See on vaheinstants domaini ja vm vahel
            var vm = new RealEstateDetailViewModel
            {
                Id = realEstate.Id,
                Area = realEstate.Area,
                Location = realEstate.Location,
                RoomNumber = realEstate.RoomNumber,
                BuildingType = realEstate.BuildingType,
                CreatedAt = realEstate.CreatedAt,
                ModifiedAt = realEstate.ModifiedAt
            };

            return View(vm);
        }
    }
}
