using Microsoft.EntityFrameworkCore;
using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly TARge25_ShopContext _context;

        public FileServices
            (
                TARge25_ShopContext context
            )
        {
            _context = context;
        }
        public void UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain)
        {
            // Toimub kontroll, kas on faile või ei ole
            if (dto.Files != null && dto.Files.Count > 0)
            {
                // Tuleb kasutada foreachi, et mitu faili ülesse laadida
                foreach (var file in dto.Files)
                {
                    //teha muutuja, mis salvestab faili nime
                    using (var target = new MemoryStream())
                    {
                        FileToDatabase files = new FileToDatabase()
                        {
                            Id = Guid.NewGuid(),
                            ImageTitle = file.FileName,
                            KindergartenId = domain.Id
                        };

                        file.CopyTo(target);
                        files.ImageData = target.ToArray();

                        _context.FileToDatabases.AddAsync(files);
                    }
                }
            }
        }

        public async Task<FileToDatabase> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos)
        {
            foreach (var dto in dtos)
            {
                var imageId = await _context.FileToDatabases
                    .Where(x => x.Id == dto.Id)
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                _context.FileToDatabases.Remove(imageId);
                await _context.SaveChangesAsync();
            }

            return null;
        }

        public async Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto dto)
        {
            var image = await _context.FileToDatabases
                 .Where(x => x.Id == dto.Id)
                 .FirstOrDefaultAsync();

            _context.FileToDatabases.Remove(image);
            await _context.SaveChangesAsync();

            return image;
        }
    }
}
