using Microsoft.AspNetCore.Mvc;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;
using TARge25_Shop.Models.Spaceship;

namespace TARge25_Shop.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipServices;
        private readonly TARge25_ShopContext _context;

        public SpaceshipController
            (
            ISpaceshipServices spaceshipServices,
            TARge25_ShopContext context)
        {
            _spaceshipServices = spaceshipServices;
            _context = context;
        }
        public IActionResult Index()
        {
            var result = _context.Spaceships
                .Select(x => new SpaceshipIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ShipType = x.ShipType,
                    CreatedAt = x.CreatedAt,
                    Crew = x.Crew,
                });
            
            // kutsume teenuse välja, et saada kõik kosmoselaevad.
            //Construktoris tuleb välja kutsuda DBcontext, et saaksime andmeid kätte. Seejärel kutsume teenuse välja.
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                ShipType = vm.ShipType,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower
            };

            // nüüd kutsume teenuse välja, et luua uus kosmoselaev. See on
            //asünkroone tegevus ja kasutame await.

            var result = await _spaceshipServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _spaceshipServices.DetailAsync(id);

            if (spaceship == null)
            { return NotFound(); }

            var vm = new SpaceshipUpdateViewModel
            {
                Id = spaceship.Id,
                Name = spaceship.Name,
                ShipType = spaceship.ShipType,
                Crew = spaceship.Crew,
                EnginePower = spaceship.EnginePower,
                CreatedAt = spaceship.CreatedAt,
                UpdatedAt = spaceship.UpdatedAt

            };

            return View(vm);

        }

        public async Task<IActionResult> Update(SpaceshipUpdateViewModel vm)
        {
            var dto = new SpaceshipDto
            {
                Id = vm.Id,
                Name = vm.Name,
                ShipType = vm.ShipType,
                Crew= vm.Crew,
                EnginePower= vm.EnginePower,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt
            };

            var result = await _spaceshipServices.Update(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid Id)
        {
            return View();
        }
        
    }
}
