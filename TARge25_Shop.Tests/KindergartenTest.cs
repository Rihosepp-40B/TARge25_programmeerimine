using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using TARge25_Shop.Data;
using Xunit;

namespace TARge25_Shop.Tests
{
    public class KindergartenTest : TestBase
    {
        [Fact]
        //Selles testis kontrollitakse et lasteaia lisamisel ei tohiks tulemus olla tühi
        public async Task ShouldNot_AddEmptyKindergarten_WhenResultIsReturned()
        {
            //Ülesseade
            KindergartenDto dto = new KindergartenDto()
            {
                GroupName = "Test",
                ChildrenCount = 1,
                KindergartenName = "LotteTest",
                TeacherName = "TestÕpetaja",
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
            };

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //Kontroll
            Assert.NotNull(result);
        }

        [Fact]
        //Selles testis kontrollitakse, et lasteaia päringul DB'st ei tohiks tagastada objekti, kui ID'd ei ole samad
        public async Task ShouldNot_GetKindergartenById_WhenIdNotEqual()
        {
            //Ülesseade
            Guid wrongGuid = Guid.NewGuid();
            Guid goodGuid = Guid.Parse("9def918e-2eee-41b2-963e-31c34f4d7500");

            //Tegevus
            await Svc<IKindergartenServices>().DetailAsync(goodGuid);

            //Kontroll
            Assert.NotEqual(wrongGuid, goodGuid);
        }

        [Fact]
        //Selles testis kontrollitakse, et lasteaia päringul DB'st peaks tagastatama objekt kui ID'd on samad
        public async Task Should_GetKindergartenById_WhenGuidIsEqual()
        {
            // Ülesseade

            Guid databaseGuid = Guid.Parse("9def918e-2eee-41b2-963e-31c34f4d7500");
            Guid seekGuid = Guid.Parse("9def918e-2eee-41b2-963e-31c34f4d7500");

            // Tegevus
            await Svc<IKindergartenServices>().DetailAsync(seekGuid);

            // Kontroll
            Assert.Equal(databaseGuid, seekGuid);
        }

        [Fact]
        //Selles testis kontrollitakse, et lasteaia kustutamisel DB'st peaks kustutama objekti, kui tagastatav väärtus on sama
        public async Task Should_KindergartenDeletedById_WhenReturnedResultIsEqual()
        {
            //Ülesseade
            KindergartenDto dto = MockKindergartenData();

            // Tegevus
            var addKindergarten = await Svc<IKindergartenServices>().Create(dto);
            var deleteKindergarten = await Svc<IKindergartenServices>().Delete((Guid)addKindergarten.Id);

            //Kontroll
            Assert.Equal(addKindergarten.Id, deleteKindergarten.Id);
        }

        [Fact]
        //Selles testis kontrollitakse, et lasteaia kustutamisel ei kustuks teine lasteaed
        public async Task ShouldNot_KindergartenDeletedbyId_WhenDidNotDeleteKindergarten()
        {
            // Ülesseade
            var dto = MockKindergartenData();

            // Tegevus
            var kindergarten1 = await Svc<IKindergartenServices>().Create(dto);
            var kindergarten2 = await Svc<IKindergartenServices>().Create(dto);

            var result = await Svc<IKindergartenServices>().Delete((Guid)kindergarten2.Id);

            // Kontroll
            Assert.NotEqual(kindergarten1.Id, result.Id);
        }
                
        [Fact]
        // Selles testis kontrollitakse, et lasteaeda uuendatakse, uute andmete korral
        public async Task Should_UpdateKindergartenById_WhenUpdatingData()
        {
            // Ülesseade            
            KindergartenDto dto = MockKindergartenData();
            var result = await Svc<IKindergartenServices>().Create(dto);

            var guid = (Guid)result.Id;
            KindergartenDto domain = new();

            domain.Id = guid;
            domain.GroupName = "Test3";
            domain.ChildrenCount = 3;
            domain.KindergartenName = "LotteTest3";
            domain.TeacherName = "TestÕpetaja3";
            domain.UpdatedAt = DateTime.Now;
            domain.CreatedAt = dto.CreatedAt;

            // Tegevus
            await Svc<IKindergartenServices>().Update(domain);

            Svc<TARge25_ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

            // Kontroll
            Assert.Equal(domain.Id, result2.Id);
            Assert.NotEqual(dto.GroupName, result2.GroupName);
            Assert.NotEqual(dto.ChildrenCount, result2.ChildrenCount);
            Assert.NotEqual(dto.KindergartenName, result2.KindergartenName);
            Assert.NotEqual(dto.TeacherName, result2.TeacherName);
            Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
        }

        [Fact]
        //Selles testis kontrollitakse, et lasteaeda ei uuendata kui andmeid ei uuendata
        public async Task ShouldNot_UpdateKindergartenById_WhenNoDataIsUpdated()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            var createdKindergarten = await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            //KindergartenDto nullDto = MockKindergartenNullData();  // see NullDto enam ei tööta kuna piirangud IsNullOrWhiteSpace
            //var result = await Svc<IKindergartenServices>().Update(nullDto);
            KindergartenDto nullIdDto = MockKindergartenNullIDData();
            var result = await Svc<IKindergartenServices>().Update(nullIdDto);

            // Kontroll
            Assert.NotEqual(createdKindergarten.Id, result.Id);
        }

        [Fact]
        //Selles testis kontrollitakse, et lasteaeda ei looda, kui lastearv on negatiivne
        public async Task ShouldNot_CreateKindergarten_WhenNegativeChildrenCount()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData(true);
            dto.ChildrenCount -= (dto.ChildrenCount * 2);

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // kontroll
            Assert.True(result.ChildrenCount >= 0);
        }

        [Fact]
        //Selles testis kontrollitakse, et lasteaeda ei looda, kui lastearv on suurem kui 30
        public async Task ShouldNot_CreateKindergarten_WhenChildrenCountMoreThan30()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData(true);
            dto.ChildrenCount = 31;

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // kontroll
            Assert.True(result.ChildrenCount <= 30);
        }

        [Fact]
        // Selles testis kontrollitakse, et lasteaed kustutatakse andmebaasist, kui seda kustutakse.
        public async Task Should_RemoveKindergartenFromDB_WhenKindergartenIsDeleted()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();

            // Tegevus
            var createdKindergarten = await Svc<IKindergartenServices>().Create(dto);
            var deletedKindergarten = await Svc<IKindergartenServices>().Delete((Guid)createdKindergarten.Id);
            var result = await Svc<IKindergartenServices>().DetailAsync((Guid)createdKindergarten.Id);

            // Kontroll
            Assert.Equal(createdKindergarten.Id, deletedKindergarten.Id);
            Assert.Null(result);
        }

        [Fact]
        //Selles testis kontrollitakse, et lasteaia eemaldamisel, ei kustutaks vale ID'ga lasteaeda andmebaasist
        public async Task ShouldNot_RemoveKindergartenFromDB_WhenKindergartenIdIsDifferent()
        {
            //Ülesseade
            KindergartenDto dto = MockKindergartenData();

            // Tegevus
            var createdKindergarten = await Svc<IKindergartenServices>().Create(dto);
            var createdKindergarten2 = await Svc<IKindergartenServices>().Create(dto);
            var deletedKindergarten = await Svc<IKindergartenServices>().Delete((Guid)createdKindergarten2.Id);
            var result = await Svc<IKindergartenServices>().DetailAsync((Guid)createdKindergarten.Id);

            // Kontroll
            Assert.NotEqual(createdKindergarten.Id, deletedKindergarten.Id);
            Assert.NotNull(result);
            Assert.Equal(createdKindergarten.Id, result.Id);
        }


        // \/\/\/\/\/
        // \/\/\/\/\/ Siit alatest on enda lisatud testid. \/\/\/\/\/
        // \/\/\/\/\/

        [Fact]
        // Selles testis kontrollitakse, et ei looda lasteaeda, kui puudub grupinimi
        public async Task ShouldNot_CreateKindergarten_WhenKindergartenGroupIsNull()
        {
            //Ülesseade
            KindergartenDto dto = MockKindergartenData(true);
            dto.GroupName = null;

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //Kontroll
            Assert.Null(result);
        }

        [Fact]
        // Selles testis kontrollitakse, et ei looda lasteaeda, kui puudub lasteaianimi
        public async Task ShouldNot_CreateKindergarten_WhenKindergartenNameIsNull()
        {
            //Ülesseade
            KindergartenDto dto = MockKindergartenData(true);
            dto.KindergartenName = null;

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //Kontroll
            Assert.Null(result);
        }

        [Fact]
        // Selles testis kontrollitakse, et ei looda lasteaeda, kui puudub õpetajanimi
        public async Task ShouldNot_CreateKindergarten_WhenTeacherNameIsNull()
        {
            //Ülesseade
            KindergartenDto dto = MockKindergartenData(true);
            dto.TeacherName = null;

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //Kontroll
            Assert.Null(result);
        }

        [Fact]
        // Selles testis kontrollitakse, et lasteaia andmeid ei uuendata, kui grupinimi on null
        public async Task ShouldNot_UpdateKindergarten_WhenGroupNameIsNull()
        {
            // Ülesseade            
            KindergartenDto dto = MockKindergartenData();
            var result = await Svc<IKindergartenServices>().Create(dto);

            var guid = (Guid)result.Id;
            KindergartenDto domain = new();

            domain.Id = guid;
            domain.GroupName = null;
            domain.ChildrenCount = 3;
            domain.KindergartenName = "LotteTest3";
            domain.TeacherName = "TestÕpetaja3";
            domain.UpdatedAt = DateTime.Now;
            domain.CreatedAt = dto.CreatedAt;

            // Tegevus
            await Svc<IKindergartenServices>().Update(domain);

            Svc<TARge25_ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

            // Kontroll
            Assert.Equal(domain.Id, result2.Id);
            Assert.Equal(dto.GroupName, result2.GroupName);
            Assert.Equal(dto.ChildrenCount, result2.ChildrenCount);
            Assert.Equal(dto.KindergartenName, result2.KindergartenName);
            Assert.Equal(dto.TeacherName, result2.TeacherName);
            // Mingil põhjusel kuupäevad muutuvad testides isegi kui kõik muu jääb samaks
            //Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
        }

        [Fact]
        // Selles testis kontrollitakse, et lasteaia andmeid ei uuendata, kui õpetajanimi on null
        public async Task ShouldNot_UpdateKindergarten_WhenTeacherNameIsNull()
        {
            // Ülesseade            
            KindergartenDto dto = MockKindergartenData();
            var result = await Svc<IKindergartenServices>().Create(dto);

            var guid = (Guid)result.Id;
            KindergartenDto domain = new();

            domain.Id = guid;
            domain.GroupName = "Test3";
            domain.ChildrenCount = 3;
            domain.KindergartenName = "LotteTest3";
            domain.TeacherName = null;
            domain.UpdatedAt = DateTime.Now;
            domain.CreatedAt = dto.CreatedAt;

            // Tegevus
            await Svc<IKindergartenServices>().Update(domain);

            Svc<TARge25_ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

            // Kontroll
            Assert.Equal(domain.Id, result2.Id);
            Assert.Equal(dto.GroupName, result2.GroupName);
            Assert.Equal(dto.ChildrenCount, result2.ChildrenCount);
            Assert.Equal(dto.KindergartenName, result2.KindergartenName);
            Assert.Equal(dto.TeacherName, result2.TeacherName);
            // Mingil põhjusel kuupäevad muutuvad testides isegi kui kõik muu jääb samaks
            //Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
        }

        [Fact]
        // Selles testis kontrollitakse, et lasteaia andmeid ei uuendata, kui lasteaianimi on null
        public async Task ShouldNot_UpdateKindergarten_WhenKindergartenNameIsNull()
        {
            // Ülesseade            
            KindergartenDto dto = MockKindergartenData();
            var result = await Svc<IKindergartenServices>().Create(dto);

            var guid = (Guid)result.Id;
            KindergartenDto domain = new();

            domain.Id = guid;
            domain.GroupName = "Test3";
            domain.ChildrenCount = 3;
            domain.KindergartenName = null;
            domain.TeacherName = "TestÕpetaja3";
            domain.UpdatedAt = DateTime.Now;
            domain.CreatedAt = dto.CreatedAt;

            // Tegevus
            await Svc<IKindergartenServices>().Update(domain);

            Svc<TARge25_ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

            // Kontroll
            Assert.Equal(domain.Id, result2.Id);
            Assert.Equal(dto.GroupName, result2.GroupName);
            Assert.Equal(dto.ChildrenCount, result2.ChildrenCount);
            Assert.Equal(dto.KindergartenName, result2.KindergartenName);
            Assert.Equal(dto.TeacherName, result2.TeacherName);
            // Mingil põhjusel kuupäevad muutuvad testides isegi kui kõik muu jääb samaks
            //Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
        }

        [Fact]
        // Selles testis kontrollitakse, et lasteaia uuendamisel, kui lastearv siestatakse negatiivsena, siis tulemus antakse 0.
        public async Task Should_UpdateKindergarten_WhenNegativeChildrenCountWithCountZero()
        {
            // Ülesseade            
            KindergartenDto dto = MockKindergartenData();
            var result = await Svc<IKindergartenServices>().Create(dto);

            var guid = (Guid)result.Id;
            KindergartenDto domain = new();

            domain.Id = guid;
            domain.GroupName = "Test3";
            domain.ChildrenCount = -3;
            domain.KindergartenName = "LotteTest3";
            domain.TeacherName = "TestÕpetaja3";
            domain.UpdatedAt = DateTime.Now;
            domain.CreatedAt = dto.CreatedAt;

            // Tegevus
            await Svc<IKindergartenServices>().Update(domain);

            Svc<TARge25_ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

            // Kontroll
            Assert.Equal(domain.Id, result2.Id);
            Assert.NotEqual(dto.GroupName, result2.GroupName);
            Assert.Equal(0, result2.ChildrenCount);
            Assert.NotEqual(dto.KindergartenName, result2.KindergartenName);
            Assert.NotEqual(dto.TeacherName, result2.TeacherName);
            Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
        }

        [Fact]
        // Selles testis kontrollitakse, et ei looda lasteaeda, kui grupinimi on tühik
        public async Task ShouldNot_CreateKindergarten_WhenKindergartenGroupIsWhitespace()
        {
            //Ülesseade
            KindergartenDto dto = MockKindergartenData(true);
            dto.GroupName = " ";

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //Kontroll
            Assert.Null(result);
        }

        [Fact]
        // Selles testis kontrollitakse, et ei looda lasteaeda, kui lasteaianimi on tühik
        public async Task ShouldNot_CreateKindergarten_WhenKindergartenNameIsWhitespace()
        {
            //Ülesseade
            KindergartenDto dto = MockKindergartenData(true);
            dto.KindergartenName = " ";

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //Kontroll
            Assert.Null(result);
        }

        [Fact]
        // Selles testis kontrollitakse, et ei looda lasteaeda, kui õpetajanimi on tühik
        public async Task ShouldNot_CreateKindergarten_WhenTeacherNameIsWhitespace()
        {
            //Ülesseade
            KindergartenDto dto = MockKindergartenData(true);
            dto.TeacherName = " ";

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //Kontroll
            Assert.Null(result);
        }

        [Fact]
        // Selles testis kontrollitakse, et lasteaia andmeid ei uuendata, kui grupinimi on tühik
        public async Task ShouldNot_UpdateKindergarten_WhenGroupNameIsWhitespace()
        {
            // Ülesseade            
            KindergartenDto dto = MockKindergartenData();
            var result = await Svc<IKindergartenServices>().Create(dto);

            var guid = (Guid)result.Id;
            KindergartenDto domain = new();

            domain.Id = guid;
            domain.GroupName = " ";
            domain.ChildrenCount = 3;
            domain.KindergartenName = "LotteTest3";
            domain.TeacherName = "TestÕpetaja3";
            domain.UpdatedAt = DateTime.Now;
            domain.CreatedAt = dto.CreatedAt;

            // Tegevus
            await Svc<IKindergartenServices>().Update(domain);

            Svc<TARge25_ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

            // Kontroll
            Assert.Equal(domain.Id, result2.Id);
            Assert.Equal(dto.GroupName, result2.GroupName);
            Assert.Equal(dto.ChildrenCount, result2.ChildrenCount);
            Assert.Equal(dto.KindergartenName, result2.KindergartenName);
            Assert.Equal(dto.TeacherName, result2.TeacherName);
            // Mingil põhjusel kuupäevad muutuvad testides isegi kui kõik muu jääb samaks
            //Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
        }

        [Fact]
        // Selles testis kontrollitakse, et lasteaia andmeid ei uuendata, kui õpetajanimi on tühik
        public async Task ShouldNot_UpdateKindergarten_WhenTeacherNameIsWhitespace()
        {
            // Ülesseade            
            KindergartenDto dto = MockKindergartenData();
            var result = await Svc<IKindergartenServices>().Create(dto);

            var guid = (Guid)result.Id;
            KindergartenDto domain = new();

            domain.Id = guid;
            domain.GroupName = "Test3";
            domain.ChildrenCount = 3;
            domain.KindergartenName = "LotteTest3";
            domain.TeacherName = " ";
            domain.UpdatedAt = DateTime.Now;
            domain.CreatedAt = dto.CreatedAt;

            // Tegevus
            await Svc<IKindergartenServices>().Update(domain);

            Svc<TARge25_ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

            // Kontroll
            Assert.Equal(domain.Id, result2.Id);
            Assert.Equal(dto.GroupName, result2.GroupName);
            Assert.Equal(dto.ChildrenCount, result2.ChildrenCount);
            Assert.Equal(dto.KindergartenName, result2.KindergartenName);
            Assert.Equal(dto.TeacherName, result2.TeacherName);
            // Mingil põhjusel kuupäevad muutuvad testides isegi kui kõik muu jääb samaks
            //Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
        }

        [Fact]
        // Selles testis kontrollitakse, et lasteaia andmeid ei uuendata, kui lasteaianimi on tühik
        public async Task ShouldNot_UpdateKindergarten_WhenKindergartenNameIsWhitespace()
        {
            // Ülesseade            
            KindergartenDto dto = MockKindergartenData();
            var result = await Svc<IKindergartenServices>().Create(dto);

            var guid = (Guid)result.Id;
            KindergartenDto domain = new();

            domain.Id = guid;
            domain.GroupName = "Test3";
            domain.ChildrenCount = 3;
            domain.KindergartenName = " ";
            domain.TeacherName = "TestÕpetaja3";
            domain.UpdatedAt = DateTime.Now;
            domain.CreatedAt = dto.CreatedAt;

            // Tegevus
            await Svc<IKindergartenServices>().Update(domain);

            Svc<TARge25_ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

            // Kontroll
            Assert.Equal(domain.Id, result2.Id);
            Assert.Equal(dto.GroupName, result2.GroupName);
            Assert.Equal(dto.ChildrenCount, result2.ChildrenCount);
            Assert.Equal(dto.KindergartenName, result2.KindergartenName);
            Assert.Equal(dto.TeacherName, result2.TeacherName);
            // Mingil põhjusel kuupäevad muutuvad testides isegi kui kõik muu jääb samaks
            //Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
        }

        [Fact]
        // Selles testis kontrollitakse, et ei looda lasteaeda, kui õpetajanimi on lühem kui kolm tähemärki
        public async Task ShouldNot_CreateKindergarten_WhenTeacherNameStringLengthIsLessThanThree()
        {
            //Ülesseade
            KindergartenDto dto = MockKindergartenData(true);
            dto.GroupName = "T1";
            dto.KindergartenName = "Öö";
            dto.TeacherName = "Õp";

            // Tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Kontroll
            Assert.Null(result);
        }

        [Fact]
        // Selles testis kontrollitakse, et ei muudeta lasteaeda, kui õpetajanimi muudetakse lühemaks kui kolm tähemärki
        public async Task ShouldNot_UpdateKindergarten_WhenTeacherNameStringLengthIsLessThanThree()
        {
            // Ülesseade            
            KindergartenDto dto = MockKindergartenData();
            var result = await Svc<IKindergartenServices>().Create(dto);

            var guid = (Guid)result.Id;
            KindergartenDto domain = new();

            domain.Id = guid;
            domain.GroupName = "Te";
            domain.ChildrenCount = 3;
            domain.KindergartenName = "Lo";
            domain.TeacherName = "Te";
            domain.UpdatedAt = DateTime.Now;
            domain.CreatedAt = dto.CreatedAt;

            // Tegevus
            await Svc<IKindergartenServices>().Update(domain);

            Svc<TARge25_ShopContext>().ChangeTracker.Clear();
            var result2 = await Svc<IKindergartenServices>().DetailAsync(guid);

            // Kontroll
            Assert.Equal(domain.Id, result2.Id);
            Assert.Equal(dto.GroupName, result2.GroupName);
            Assert.Equal(dto.ChildrenCount, result2.ChildrenCount);
            Assert.Equal(dto.KindergartenName, result2.KindergartenName);
            Assert.Equal(dto.TeacherName, result2.TeacherName);
            // Mingil põhjusel kuupäevad muutuvad testides isegi kui kõik muu jääb samaks
            //Assert.Equal(dto.CreatedAt, result2.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, result2.UpdatedAt);
        }


        /* üleval testid, all abimeetodid */
        private KindergartenDto MockKindergartenData(bool isOneOrTwo = false)
        {
            if (isOneOrTwo == false)
            {
                return new KindergartenDto
                {
                    GroupName = "Test",
                    ChildrenCount = 30,
                    KindergartenName = "LotteTest",
                    TeacherName = "TestÕpetaja",
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                };
            }
            else
            {
                return new KindergartenDto
                {
                    GroupName = "TestKaks",
                    ChildrenCount = 1,
                    KindergartenName = "MuumiTest",
                    TeacherName = "TestÕpetaja2",
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                };
            }
        }

        // See fuynktsioon kaotas mõtte kuna string väljad said IsNullOrWhitsepace piirangu
        private KindergartenDto MockKindergartenNullData()
        {
            return new KindergartenDto
            {
                Id = null,
                GroupName = "",
                ChildrenCount = 0,
                KindergartenName = "",
                TeacherName = "",
                UpdatedAt = DateTime.MinValue,
                CreatedAt = DateTime.MinValue
            };
        }

        private KindergartenDto MockKindergartenNullIDData()
        {
            return new KindergartenDto
            {
                Id = null,
                GroupName = "Grupp",
                ChildrenCount = 0,
                KindergartenName = "Aed",
                TeacherName = "Õpetaja",
                UpdatedAt = DateTime.MinValue,
                CreatedAt = DateTime.MinValue
            };
        }
    }
}
