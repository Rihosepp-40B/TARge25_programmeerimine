using Microsoft.Extensions.Hosting;
using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;


namespace TARge25_Shop.ApplicationServices.Services
{
    public class FileServices :IFileServices
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
    }
}
