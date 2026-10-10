using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;
using Microsoft.AspNetCore.Http;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly IHostEnvironment _webHost;
        private readonly TARge25_ShopContext _context;

        //Konstruktor
        public FileServices
            (
                IHostEnvironment webHost,
                TARge25_ShopContext context
            )
        {
            _webHost = webHost;
            _context = context;
        }

        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                // Kui directoryt ei ole olemas, siis tee Directory
                // \\wwwroot\\multipleFileUpload\\
                // if
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    // Tuleb teha muutuja, kus on failide asukoht e kuhu hakatakse salvestama
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    // Muutuja Faili unikaalseks nimeks
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                    // muutuja faili asukohaks
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        FileToApi path = new FileToApi
                        {
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            SpaceshipId = domain.Id
                        };

                        _context.FileToApis.AddAsync(path);
                    }
                }
            }
        }

        public async Task<FileToApi> RemoveImageFromApi(FileToApiDto dto)
        {
            //kui soovin kustutada faili, siis pean läbi Id pildi ülesse otsima
            var imageId = await _context.FileToApis.FirstOrDefaultAsync(x => x.Id == dto.Id);

            
            //teha muutuja filePath, mis näitab failide asukohta
            //kus asuvad pildid, mida hakatakse kustutama
            var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                + imageId.ExistingFilePath;

            //Kui fail asub selles kaustas, siis kustuta
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.FileToApis.Remove(imageId);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dtos)
        {
            //mitu pilti peab korraga ära kustutama
            foreach (var dto in dtos)
            {
                var imageId = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.ExistingFilePath == dto.ExistingFilePath);

                var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                    + imageId.ExistingFilePath;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _context.FileToApis.Remove(imageId);
            }

            await _context.SaveChangesAsync();

            return null;
        }

        public void UploadRealEstateFilesToDatabase(RealEstateDto dto, RealEstate domain)
        {
            UploadFilesToDataBase(dto.Files, file => new FileToDatabase
            {
                Id = Guid.NewGuid(),
                ImageTitle = file.FileName,
                RealEstateId = domain.Id
            });
        }
        public void UploadKindergartenFilesToDatabase(KindergartenDto dto, Kindergarten domain)
        {
            UploadFilesToDataBase(dto.Files, file => new FileToDatabase
            {
                Id = Guid.NewGuid(),
                ImageTitle = file.FileName,
                KindergartenId = domain.Id
            });
        }

        //Ühine failiüleslaadimise meetod, võtab sisse dto failid ja andmbaasis täidetavad (tekstilised) andmed, failidest teeb funktsioon ise vastavate veergude andmed
        private void UploadFilesToDataBase(List<IFormFile> files, Func<IFormFile, FileToDatabase> fileToDatabase)
        {
            // Toimub kontroll, kas on faile või ei ole
            if (files != null && files.Count > 0)
            {
                // Tuleb kasutada foreachi, et mitu faili ülesse laadida
                foreach (var file in files)
                {
                    //teha muutuja, mis salvestab faili nime
                    using(var target = new MemoryStream())
                    {
                        FileToDatabase files = FileToDatabase(file);
                        
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
            }

            await _context.SaveChangesAsync();

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
