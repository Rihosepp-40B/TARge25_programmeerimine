using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;

namespace TARge25_Shop.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(SpaceshipDto dto, Spaceship domain);
    }
}
