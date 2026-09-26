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


        // Seleta kodus lahti testi sisu
        [Fact]
        // Selles testis kontrollitakse et (2) Kosmoselaeva päringul DB'st (1) peaks tagastama objekti (3) kui ID'd on samad:
        //
        //                  1           2               3
        //                  \/          \/              \/
        public async Task Should_GetSpaceshipByID_WhenGuidIsEqual()
        {
            // Ülesseade

            Guid databaseGuid = Guid.Parse("9def918e-2eee-41b2-963e-31c34f4d7500");
            Guid seekGuid = Guid.Parse("9def918e-2eee-41b2-963e-31c34f4d7500");

            // Tegevus
            await Svc<ISpaceshipServices>().DetailAsync(seekGuid);

            // Kontroll
            Assert.Equal(databaseGuid, seekGuid);
        }

        // Seleta kodus lahti testi sisu
        [Fact]
        // Selles testis kontrollitakse et (2) Kosmoselaeva kustutamisel DB'st (1) peaks kustutama objekti (3) kui tagastatav väärtus on sama:
        //
        //                  1           2               3
        //                  \/          \/              \/
        public async Task Should_SpaceshipDeletedbyID_WhenReturnedResultIsEqual()
        {
            //Ülesanne
            SpaceshipDto dto = MockSpaceshipData();

            // Tegevus
            var addSpaceship = await Svc<ISpaceshipServices>().Create(dto);
            var deleteSpaceship = await Svc<ISpaceshipServices>().Delete((Guid)addSpaceship.Id);

            //Kontroll
            Assert.Equal(addSpaceship.Id, deleteSpaceship.Id);
        }
            
        private SpaceshipDto MockSpaceshipData(bool isOneOrTwo = false)
        {
            if (isOneOrTwo == false)
            {
                return new SpaceshipDto
                {
                    Name = "X AE a L 10",
                    ShipType = "Taldrik",
                    Crew = 67,
                    EnginePower = 69,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };
            }
            else
            {
                return new SpaceshipDto
                {
                    Name = "RAKETT69",
                    ShipType = "Pointi",
                    Crew = 1,
                    EnginePower = 69,
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now
                };
            }
        }
    }
}
