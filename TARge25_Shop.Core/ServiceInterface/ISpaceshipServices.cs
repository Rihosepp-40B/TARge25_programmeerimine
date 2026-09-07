using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;

namespace TARge25_Shop.Core.ServiceInterface
{
    public interface ISpaceshipServices
    {
        Task<Spaceship> Create(SpaceshipDto);
    }
}
