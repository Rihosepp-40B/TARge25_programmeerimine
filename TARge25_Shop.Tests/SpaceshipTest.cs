using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;
using Xunit;

namespace TARge25_Shop.Tests
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
            //Ülesseade
            SpaceshipDto dto = MockSpaceshipData();

            // Tegevus
            var addSpaceship = await Svc<ISpaceshipServices>().Create(dto);
            var deleteSpaceship = await Svc<ISpaceshipServices>().Delete((Guid)addSpaceship.Id);

            //Kontroll
            Assert.Equal(addSpaceship.Id, deleteSpaceship.Id);
        }

        [Fact]
        public async Task ShouldNot_SpaceshipDeletedbyId_WhenDidNotDeleteSpaceship()
        {
            // Ülesseade
            var dto = MockSpaceshipData();

            // Tegevus
            var spaceship1 = await Svc<ISpaceshipServices>().Create(dto);
            var spaceship2 = await Svc<ISpaceshipServices>().Create(dto);

            var result = await Svc<ISpaceshipServices>().Delete((Guid)spaceship2.Id);

            // Kontroll
            Assert.NotEqual(spaceship1.Id, result.Id);
        }

        // Test, mis kontrollib, et spaceship uuendatakse, uute andmete korral
        [Fact]
        public async Task Should_UpdateSpaceshipById_WhenUpdatingData()
        {
            // Ülesseade            
            SpaceshipDto dto = MockSpaceshipData();
            var result = await Svc<ISpaceshipServices>().Create(dto);

            var guid = (Guid)result.Id;
            SpaceshipDto domain = new();

            domain.Id = guid;
            domain.EnginePower = 1000;
            domain.Name = "Igor Mang 2";
            domain.ShipType = "Püramiid";
            domain.Crew = 422;
            domain.CreatedAt = dto.CreatedAt;
            domain.UpdatedAt = DateTime.Now;

            // Tegevus
            await Svc<ISpaceshipServices>().Update(domain);
            //kill first connection: added tracker clearing to update
            Svc<TARge25_ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<ISpaceshipServices>().DetailAsync(guid);

            // Kontroll
            Assert.Equal(domain.Id, result2.Id);
            Assert.NotEqual(dto.EnginePower, result2.EnginePower);
            Assert.NotEqual(dto.Name, result2.Name);
            Assert.DoesNotMatch(dto.Crew.ToString(), result2.Crew.ToString());
            Assert.DoesNotMatch(dto.ShipType, result2.ShipType);
            Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
        }
                
        [Fact]
        public async Task ShouldNot_UpdateSpaceshipById_WhenNoDataIsUpdated()
        {
            // Ülesseade
            SpaceshipDto dto = MockSpaceshipData();
            var createdSpaceship = await Svc<ISpaceshipServices>().Create(dto);

            // Tegevus
            SpaceshipDto nullDto = MockSpaceshipNullData();
            var result = await Svc<ISpaceshipServices>().Update(nullDto);

            // Kontroll
            Assert.NotEqual(createdSpaceship.Id, result.Id);
        }

        // Kuna mootor ei saa olla negatiivse võimususega, kontrollime et ei saaks
        // lisada negatiivse väärtusega võimsust
        [Fact]
        public async Task ShouldNot_CreateSpaceshipWithNegativeEnginePower_WhenEnginePowerNegative()
        {
            // Ülesseade
            SpaceshipDto dto = MockSpaceshipData(true);
            dto.EnginePower -= (dto.EnginePower * 2);

            // Tegevus
            var result = await Svc<ISpaceshipServices>().Create(dto);

            // kontroll
            Assert.True(result.EnginePower > 0);
        }

        // Test mis kontrollib, et meeskond on suurem kui 3 liiget, service ei tohi lisada sellest vähema arvuga objekti, service võib selle probleemi lahendada ükskõik kuidas

        [Fact]
        public async Task ShouldNot_CreateSpaceship_WhenCrewIsLessThanFour()
        {
            // Ülesseade
            SpaceshipDto dto = MockSpaceshipData();
            dto.Crew = 2;

            // Tegevus
            var result = await Svc<ISpaceshipServices>().Create(dto);

            // Kontroll
            Assert.True(result.Crew > 3);
        }

        [Fact]
        public async Task Should_RemoveSpaceshipFromDB_WhenSpaceshipIsDeleted()
        {
            // Ülesseade
            SpaceshipDto dto = MockSpaceshipData();

            // Tegevus
            var createdSpaceship = await Svc<ISpaceshipServices>().Create(dto);
            var deletedSpaceship = await Svc<ISpaceshipServices>().Delete((Guid)createdSpaceship.Id);
            var result = await Svc<ISpaceshipServices>().DetailAsync((Guid)createdSpaceship.Id);

            // Kontroll
            Assert.Equal(createdSpaceship.Id, deletedSpaceship.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task ShouldNot_RemoveSpaceshipFromDB_WhenSpaceshipIdIsDifferent()
        {
            //Ülesseade
            SpaceshipDto dto = MockSpaceshipData();

            // Tegevus
            var createdSpaceship = await Svc<ISpaceshipServices>().Create(dto);
            var createdSpaceship2 = await Svc<ISpaceshipServices>().Create(dto);
            var deletedSpaceship = await Svc<ISpaceshipServices>().Delete((Guid)createdSpaceship2.Id);
            var result = await Svc<ISpaceshipServices>().DetailAsync((Guid)createdSpaceship.Id);

            // Kontroll
            Assert.NotEqual(createdSpaceship.Id, deletedSpaceship.Id);
            Assert.NotNull(result);
            Assert.Equal(createdSpaceship.Id, result.Id);
        }
            
        /* üleval testid, all abimeetodid */
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

        /// <summary>
        /// Returns a nulled object for testing purposes
        /// </summary>
        /// <returns></returns>

        private SpaceshipDto MockSpaceshipNullData()
        {
            return new SpaceshipDto
            {
                Id = null,
                Name = "",
                ShipType = "",
                Crew = 0,
                EnginePower = 0,
                UpdatedAt = DateTime.MinValue,
                CreatedAt = DateTime.MinValue
            };            
        }
    }
}
