using Microsoft.EntityFrameworkCore;

namespace TARge25_Shop.Data
{
    // Nimetasime classi TARge25_ShopContext, mis pärib DBContext klassi
    public class TARge25_ShopContext : DbContext
    {
        // Tegime konteksti, mis pärib DBContext klassi
        public TARge25_ShopContext(DbContextOptions<TARge25_ShopContext> option) : base(option) // consturctor
        { }

    }
}
