using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;
using TARge25_Shop.Models.Kindergarten;

namespace TARge25_Shop.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly IKindergartenServices _kindergartenServices;
        private readonly TARge25_ShopContext _context;

        private readonly IFileServices _fileServices;

        public KindergartenController
            (
            IKindergartenServices kindergartenServices,
            TARge25_ShopContext context,
            IFileServices fileServices
            )
        {
            _kindergartenServices = kindergartenServices;
            _context = context;
            _fileServices = fileServices;
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

        [HttpGet]
        public IActionResult Create()
        {
            KindergartenCreateUpdateViewModel result = new();

            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,

                // Failide lisamine
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        KindergartenId = x.KindergartenId,
                        ImageTitle = x.ImageTitle,
                        ImageData = x.ImageData,
                    }).ToArray()
            };

            //Teenuse väljakutsumine, et luua uus Kindergarten
            var result = await _kindergartenServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            { return NotFound(); }

            KindergartenImageViewModel[] images = await FileFromDatabase(id);

            var vm = new KindergartenCreateUpdateViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;
            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt,

                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        KindergartenId = x.KindergartenId,
                        ImageTitle = x.ImageTitle,
                        ImageData = x.ImageData,
                    }).ToArray()
            };

            var result = await _kindergartenServices.Update(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(Id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            KindergartenImageViewModel[] images = await FileFromDatabase(Id);

            //See on vaheinstants domaini ja vm vahel
            var vm = new KindergartenDeleteViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;
            vm.Image.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var kindergarten = await _kindergartenServices.Delete(id);

            if (kindergarten == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(Id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            KindergartenImageViewModel[] images = await FileFromDatabase(Id);

            //See on vaheinstants domaini ja vm vahel
            var vm = new KindergartenDetailViewModel();

            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.TeacherName = kindergarten.TeacherName;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;
            vm.Image.AddRange(images);
            
            return View(vm);
        }

        private async Task<KindergartenImageViewModel[]> FileFromDatabase(Guid id)
        {
            return await _context.FileToDatabases
                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    ImageId = y.Id,
                    KindergartenId = y.KindergartenId,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();
        }


        [HttpPost]
        public async Task<IActionResult> RemoveImage(KindergartenImageViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.ImageId
            };

            var image = await _fileServices.RemoveImageFromDatabase(dto);

            var kindergartenId = image.KindergartenId;

            if (image != null)
            {
                return RedirectToAction(nameof(Index));
            }

            // ID on vajalik, et teaks milline Update lehekülg tuleb kuvada, muidu läheb index lehele.
            return RedirectToAction(nameof(Update), new { id = kindergartenId });
        }
    }
}
