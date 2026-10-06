using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;
using TARge25_Shop.Models.RealEstate;
using TARge25_Shop.Models.Spaceship;
using static System.Net.Mime.MediaTypeNames;

namespace TARge25_Shop.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly IRealEstateServices _realEstateServices;
        private readonly TARge25_ShopContext _context;

        private readonly IFileServices _fileServices;

        public RealEstateController
            (
            IRealEstateServices realEstateServices,
            TARge25_ShopContext context,
            IFileServices fileServices)
        {
            _realEstateServices = realEstateServices;
            _context = context;
            _fileServices = fileServices;
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

                // Failide lisamine
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        RealEstateId = x.RealEstateId,
                        ImageTitle = x.ImageTitle,
                        ImageData = x.ImageData,
                    }).ToArray()
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

            RealEstateImageViewModel[] images = await FileFromDatabase(id);

            var vm = new RealEstateCreateUpdateViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Location = realEstate.Location;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.BuildingType = realEstate.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Image.AddRange(images);

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
                ModifiedAt = vm.ModifiedAt,

                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        RealEstateId = x.RealEstateId,
                        ImageTitle = x.ImageTitle,
                        ImageData = x.ImageData,
                    }).ToArray()
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

            RealEstateImageViewModel[] images = await FileFromDatabase(Id);
            //See on vaheinstants domaini ja vm vahel
            var vm = new RealEstateDeleteViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Location = realEstate.Location;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.BuildingType = realEstate.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Image.AddRange(images);

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

            RealEstateImageViewModel[] images = await FileFromDatabase(Id);

            //See on vaheinstants domaini ja vm vahel
            var vm = new RealEstateDetailViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.Location = realEstate.Location;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.BuildingType = realEstate.BuildingType;
            vm.CreatedAt = realEstate.CreatedAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Image.AddRange(images);

            return View(vm);
        }

        private async Task<RealEstateImageViewModel[]> FileFromDatabase(Guid id)
        {
            return await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                    ImageId = y.Id,
                    RealEstateId = y.RealEstateId,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(RealEstateImageViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.ImageId
            };

            var image = await _fileServices.RemoveImageFromDatabase(dto);

            var realEstateId = image.RealEstateId;

            if (image != null)
            {
                return RedirectToAction(nameof(Index));
            }

            // ID on vajalik, et teaks milline Update lehekülg tuleb kuvada, muidu läheb index lehele.
            return RedirectToAction(nameof(Update), new { id = realEstateId });
        }
    }
}
