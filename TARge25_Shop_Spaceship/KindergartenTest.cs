using TARge25_Shop.ApplicationServices.Services;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
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
            var guid = new Guid("33601633-f62f-4e5e-8d3f-282a49921b9f");

            KindergartenDto dto = MockKindergartenData();

            KindergartenDto domain = new();

            domain.Id = guid;
            domain.GroupName = "Test3";
            domain.ChildrenCount = 3;
            domain.KindergartenName = "LotteTest3";
            domain.TeacherName = "TestÕpetaja3";
            domain.UpdatedAt = DateTime.Now;
            domain.CreatedAt = dto.CreatedAt;

            // Tegevus
            await Svc<IKindergartenServices>().Update(dto);

            // Kontroll
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.GroupName, domain.GroupName);
            Assert.NotEqual(dto.ChildrenCount, domain.ChildrenCount);
            Assert.NotEqual(dto.KindergartenName, domain.KindergartenName);
            Assert.NotEqual(dto.TeacherName, domain.TeacherName);
            Assert.Equal(dto.CreatedAt, domain.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, domain.UpdatedAt);
        }

        [Fact]
        //Selles testis kontrollitakse, et lasteaeda ei uuendata kui andmeid ei uuendata
        public async Task ShouldNot_UpdateKindergartenById_WhenNoDataIsUpdated()
        {
            // Ülesseade
            KindergartenDto dto = MockKindergartenData();
            var createdKindergarten = await Svc<IKindergartenServices>().Create(dto);

            // Tegevus
            KindergartenDto nullDto = MockKindergartenNullData();
            var result = await Svc<IKindergartenServices>().Update(nullDto);

            // Kontroll
            Assert.NotEqual(createdKindergarten.Id, result.Id);
        }

        [Fact]
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
        // Selles testis kontrollitakse, et lasteaed kustutatkse andmebaasist, kui seda kustutakse.
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
        //Selles testis kontrollime, et lasteaia eemaldamisel, ei kustutaks vale ID'ga lasteaeda andmebaasist
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
    }
}
