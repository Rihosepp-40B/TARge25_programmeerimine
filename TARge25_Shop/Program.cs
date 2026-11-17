using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using TARge25_Shop.ApplicationServices.Services;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;
using TARge25_Shop.Core.Domain;

namespace TARge25_Shop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ISpaceshipServices, SpaceshipServices>();

            builder.Services.AddScoped<IFileServices, FileServices>();

            builder.Services.AddScoped<IRealEstateServices, RealEstateServices>();

            builder.Services.AddScoped<IKindergartenServices, KindergartenServices>();

            builder.Services.AddHttpClient<IChuckNorrisServices, ChuckNorrisServices>();

            builder.Services.AddScoped<ICocktailServices, CocktailServices>();
            builder.Services.AddScoped<IEmailServices, EmailServices>();

            //On vaja alla laadida Microsoft.EntityFrameworkCore.SqlServer NuGet pakett,
            //et kasutada UseSqlServer meetodit
            builder.Services.AddDbContext<TARge25_ShopContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // nõuab, et konto oleks kinnitatud enne sisselogimist
                options.SignIn.RequireConfirmedAccount = true;
                // salasõna pikkus peab olema 3 tähemärki
                options.Password.RequiredLength = 3;

                //vaikepakkuja asemel panime oma tokeni pakkuja selle asemele
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                // 3 korda paned valesti, siis lukustab
                options.Lockout.MaxFailedAccessAttempts = 3;
                // lukustamisest teeb lahti 1 minut peale lukustamisest
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            })
                .AddEntityFrameworkStores<TARge25_ShopContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
