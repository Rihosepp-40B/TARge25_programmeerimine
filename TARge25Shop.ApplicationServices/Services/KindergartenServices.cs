using Microsoft.EntityFrameworkCore;
using TARge25_Shop.Core.Domain;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        public readonly TARge25_ShopContext _context;

        public KindergartenServices
            (
                TARge25_ShopContext context
            )
        {
            _context = context;
        }
        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            //Vaheinstants dto ja domain vahel
            Kindergarten kindergarten = new Kindergarten();

            kindergarten.Id = Guid.NewGuid();
            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.TeacherName = dto.TeacherName;
            kindergarten.CreatedAt = DateTime.Now;
            kindergarten.UpdatedAt = DateTime.Now;

            //Andmete salvestamine andmebaasi
            await _context.Kindergartens.AddAsync(kindergarten);
            await _context.SaveChangesAsync();

            return kindergarten;
        }

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            Kindergarten kindergarten = new();

            kindergarten.Id = dto.Id;
            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.TeacherName = dto.TeacherName;
            kindergarten.CreatedAt = dto.CreatedAt;
            kindergarten.UpdatedAt = DateTime.Now;

            _context.Kindergartens.Update(kindergarten);
            await _context.SaveChangesAsync();

            return kindergarten;
        }

        //Meetod aitab vastuvõtta dto ja uuendab olemasolevat
        public async Task<Kindergarten> DetailAsync(Guid id)
        {
            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            return kindergarten;
        }
    }
}
