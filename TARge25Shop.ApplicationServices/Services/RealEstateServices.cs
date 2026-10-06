using Microsoft.EntityFrameworkCore;
using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateServices
    {
        public readonly TARge25_ShopContext _context;
        private readonly IFileServices _fileServices;

        public RealEstateServices(TARge25_ShopContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            //Vaheinstants dto ja domain vahel
            RealEstate realEstate = new RealEstate();

            realEstate.Id = Guid.NewGuid();
            realEstate.Area = dto.Area;
            realEstate.Location = dto.Location;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreatedAt = DateTime.Now;
            realEstate.ModifiedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, realEstate);
            }

            //Andmete salvestamine andmebaasi
            await _context.RealEstates.AddAsync(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            RealEstate realEstate = new();

            realEstate.Id = dto.Id;
            realEstate.Area = dto.Area;
            realEstate.Location = dto.Location;
            realEstate.RoomNumber = dto.RoomNumber;
            realEstate.BuildingType = dto.BuildingType;
            realEstate.CreatedAt = dto.CreatedAt;
            realEstate.ModifiedAt = DateTime.Now;

            _context.RealEstates.Update(realEstate);
            await _context.SaveChangesAsync();

            return realEstate;
        }

        //Meetod aitab vastuvõtta dto ja uuendab olemasolevat
        public async Task<RealEstate> DetailAsync(Guid id)
        {
            var realEstate = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            return realEstate;
        }

        public async Task<RealEstate> Delete(Guid id)
        {
            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new FileToDatabaseDto
                {
                    Id = y.Id,
                    ImageTitle = y.ImageTitle,
                    RealEstateId = y.RealEstateId,
                }).ToArrayAsync();

            await _fileServices.RemoveImagesFromDatabase(images);

            _context.RealEstates.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
