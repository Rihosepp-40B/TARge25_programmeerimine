using Microsoft.EntityFrameworkCore;
using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;


namespace TARge25_Shop.ApplicationServices.Services
{
    public class SpaceshipServices : ISpaceshipServices
    {
        private readonly TARge25_ShopContext _context;
        private readonly IFileServices _fileServices;

        public SpaceshipServices
            (
            TARge25_ShopContext context,
            IFileServices fileServices

            )
        {
            _context = context;
            _fileServices = fileServices;
        }

        //See meetod on vaja controlleris esile kutsuda
        //Peab lisama interface, et kutsuda see meetod välja
        public async Task<Spaceship> Create(SpaceshipDto dto)
        {
            //siin peab tegema vaheinstansi dto ja domain vahel,
            //et andmed liiguvad dto-st domain objekt
            Spaceship spaceship = new Spaceship();

            spaceship.Id = Guid.NewGuid();
            spaceship.Name = dto.Name;
            spaceship.ShipType = dto.ShipType;
            spaceship.Crew = dto.Crew;
            spaceship.EnginePower = dto.EnginePower;
            spaceship.CreatedAt = DateTime.Now;
            spaceship.UpdatedAt = DateTime.Now;
            // Kui uus ankeet on loodud, siis toimub ka faili salvestamine
            //saab kutsuda teise service classi meetotit esile service classis
            _fileServices.FilesToApi(dto, spaceship);

            // Andmete salvestamine andmebaasi
            await _context.Spaceships.AddAsync(spaceship);
            await _context.SaveChangesAsync();

            return spaceship;
        }

        public async Task<Spaceship> Update(SpaceshipDto dto)
        {
            Spaceship spaceship = new();

            spaceship.Id = dto.Id;
            spaceship.Name = dto.Name;
            spaceship.ShipType = dto.ShipType;
            spaceship.Crew = dto.Crew;
            spaceship.EnginePower = dto.EnginePower;
            spaceship.CreatedAt = dto.CreatedAt;
            spaceship.UpdatedAt = DateTime.Now;

            _context.Spaceships.Update(spaceship);
            await _context.SaveChangesAsync();

            return spaceship;
        }
        //teha update meetod, mis võtab vastu dto ja uuendab olmasolevat kosmoselaeva
        public async Task<Spaceship> DetailAsync(Guid id)
        {
            var spaceship = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            return spaceship;
        }

        public async Task<Spaceship> Delete(Guid id)
        {
            var result = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Spaceships.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
