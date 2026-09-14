
using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;


namespace TARge25_Shop.Core.ServiceInterface
{
    public interface ISpaceshipServices
    {
        Task<Spaceship> Create(SpaceshipDto dto);
        Task<Spaceship> Update(SpaceshipDto dto);
        Task<Spaceship> DetailAsync(Guid id);
        Task<Spaceship> Delete(Guid id);
    }
}
