using Microsoft.EntityFrameworkCore;
using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;


namespace TARge25_Shop.ApplicationServices.Services
{
    public class SpaceshipServices : ISpaceshipServices
    {
        public readonly TARge25_ShopContext _context;

        public SpaceshipServices
            (
            TARge25_ShopContext context
            )
        {
            _context = context;
        }

        //See meetod on vaja controlleris esile kutsuda
        //Peab lisama interface, et kutsuda see meetod välja
        public async Task<Spaceship> Create(SpaceshipDto dto)
        {
            //siin peab tegema vaheinstansi dto ja domain vahel,
            //et andmed liiguvad dto-st domain objekt
            Spaceship spaceShip = new Spaceship();

            spaceShip.Id = Guid.NewGuid();
            spaceShip.Name = dto.Name;
            spaceShip.ShipType = dto.ShipType;
            spaceShip.Crew = dto.Crew;
            spaceShip.EnginePower = dto.EnginePower;
            spaceShip.CreatedAt = DateTime.Now;
            spaceShip.UpdatedAt = DateTime.Now;

            // Andmete salvestamine andmebaasi
            await _context.Spaceships.AddAsync(spaceShip);
            await _context.SaveChangesAsync();

            return spaceShip;
        }

        public async Task<Spaceship> Update(SpaceshipDto dto)
        {
            Spaceship spaceShip = new();

            spaceShip.Id = dto.Id;
            spaceShip.Name = dto.Name;
            spaceShip.ShipType = dto.ShipType;
            spaceShip.Crew = dto.Crew;
            spaceShip.EnginePower = dto.EnginePower;
            spaceShip.CreatedAt = dto.CreatedAt;
            spaceShip.UpdatedAt = DateTime.Now;

            _context.Spaceships.Update(spaceShip);
            await _context.SaveChangesAsync();

            return spaceShip;
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
