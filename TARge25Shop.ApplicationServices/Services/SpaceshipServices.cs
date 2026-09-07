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
            Spaceship spaceShip = new();

            spaceShip.Id = dto.Id;
            spaceShip.Name = dto.Name;
            spaceShip.ShipType = dto.ShipType;
            spaceShip.Crew = dto.Crew;
            spaceShip.EnginePower = dto.EnginePower;
            spaceShip.CreatedAt = dto.CreatedAt;
            spaceShip.UpdatedAt = dto.UpdatedAt;

            // Andmete salvestamine andmebaasi
        _context.Spaceships.Add(spaceShip);
        await _context.SaveChangesAsync();

        return spaceShip;
        }
    }
}
