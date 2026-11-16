using Microsoft.EntityFrameworkCore;
using TARge25_Shop.Core.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace TARge25_Shop.Data
{
    // Nimetasime classi TARge25_ShopContext, mis pärib DBContext klassi
    public class TARge25_ShopContext : IdentityDbContext<ApplicationUser>
    {
        // Tegime konteksti, mis pärib DBContext klassi
        public TARge25_ShopContext(DbContextOptions<TARge25_ShopContext> options)
            : base(options) { } // consturctor


        // vaja lisada dbSet, mis on seotud meie domain klassiga Spaceship
        public DbSet<Spaceship> Spaceships { get; set; }

        public DbSet<FileToApi> FileToApis { get; set; }

        public DbSet<RealEstate> RealEstates { get; set; }

        public DbSet<FileToDatabase> FileToDatabases { get; set; }

        // Kindergarten dbSet
        public DbSet<Kindergarten> Kindergartens { get; set; }

    }
}
