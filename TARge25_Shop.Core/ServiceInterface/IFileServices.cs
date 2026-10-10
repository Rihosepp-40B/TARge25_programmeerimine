using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;

namespace TARge25_Shop.Core.ServiceInterface
{
    public interface IFileServices
    {
        void UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain);
        Task<FileToDatabase> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos);
        Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto dto);
    }
}
