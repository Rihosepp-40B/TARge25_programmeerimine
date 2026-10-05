
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;
using Xunit;

namespace TARge25_Shop.KinderkartenTest
{
    public class KindergartenTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyKinderkarten_WhenResultIsReturned()
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


    }
}
