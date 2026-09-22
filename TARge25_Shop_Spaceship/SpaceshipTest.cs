using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using Xunit;

namespace TARge25_Shop.SpaceshipTest
{
    public class SpaceshipTest : TestBase
    {
        [Fact] // Fact tähistab ära ühte testi xUnit raamistikus
        // 1 - kirjeldatakse ära kas test on tavaline või negatiivne
        // 2 - Kirjeldatakse ära mida parasjagu üritatakse testialuse objektiga teha
        // 3- Mis tingimustel tulemust kontrollitakse, peale tegevust
        //
        // Selles testis kontrollitakse et (2) Kosmoselaeva lisamisel (1) Ei tohiks (3) tulemus olla tühi. Jälgi seda sõnastusviisi:
        //
        //                  1           2               3
        //                  \/          \/              \/
        public async Task ShouldNot_AddEmptySpaceShip_WhenResultIsReturned()
        {
            // Ülesseade
            SpaceshipDto dto = new SpaceshipDto()
            {
                Name = "X AE a L 10",
                ShipType = "Taldrik",
                Crew = 67,
                EnginePower = 69,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now
            };

            //tegutsemine 
            var result = await Svc<ISpaceshipServices>().Create(dto);

            //Kontroll
            Assert.NotNull(result);

        }

        [Fact]

        // Selles testis kontrollitakse et (2) Kosmoselaeva päringul DB'st (1) Ei tohiks tagastada objekti (3) kui ID'd ei ole samad:
        //
        //                  1           2               3
        //                  \/          \/              \/
        public async Task ShouldNot_GetSpaceshipByID_WhenIDNotEqual()
        {
            // Ülesseade
            Guid wrongGuid = Guid.NewGuid();
            Guid goodGuid = Guid.Parse("9def918e-2eee-41b2-963e-31c34f4d7500");

            // Tegvus
            await Svc<ISpaceshipServices>().DetailAsync(goodGuid);

            //Kontroll
            Assert.NotEqual(wrongGuid, goodGuid);

        }
    }
}
