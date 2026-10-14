using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TARge25_Shop.SeleniumTesting
{
    // Järjestus - Järjesti klass, mis omab omadusi mille järgi teste saab järjestada
    public class OrderAttribute : Attribute
    {
        public int Value { get; }
        public OrderAttribute(int value)
        {
            Value = value;
        }
    }

    //XunitJärjesti mis implementeerib ITestCaseOrdereri liidesest "OrderTestCase" meetodi, mis on see mis testide käivitamisel järjestab nad vastavalt "Order" omadustele
    public class PriorityOrderer : ITestCaseOrderer
    {
        // Meetod võtab sisse kõik test cased järjestatuna, arendaja defineeritud järjesti järgi.
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            return testCases.OrderBy(test =>
            {
                // Leitakse omadus                                      Omaduse tüüp
                var method = test.TestMethod.Method.GetCustomAttributes(typeof(OrderAttribute)).FirstOrDefault();

                //Kui ei leita
                if (method == null)
                {
                    //Antakse maksimaalne väärus, ehk test teostatakse kohe, ega ei panda teiste potensiaalsetet järjekorras olevate testide vahele
                    return int.MaxValue;
                }
                // Kui leitakse
                else
                {
                    // tagastatakse omaduse "Order" väärtus
                    return method.GetNamedArgument<int>(nameof(OrderAttribute.Value));
                }
            }).ToList(); // Pannakse loendisse, ToList on linq meetod.
        }
    }

    //Enne testi klassi kirjeldatakse ära mille järgi testid sorteeritakse tunnuse "TestCaseOrder'i" järgi. Sulgude vahel esimene väärtus on klassi asukoht ise, teine väärtus on nimeruum, kus klass asub
    [TestCaseOrderer("TARge25_Shop.SeleniumTesting.PriorityOrderer", "TARge25_Shop.SeleniumTesting")]    
    public class SpaceshipFrontendTests
    {
        
        [Fact]
        [Order(1)] // Järjesti millel on väärtus "1", järjestatakse väiksem enne.
        public void Should_NavigateToCreate_AddSpaceship_WithCorrectData_ReturnToIndex()
        {
            //Sisestatavad andmed
            IWebDriver driver = SetupAndNavigatesIndex();

            //tegevus
            IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();

            //sisestatavad andmed
            InsertSpaceshipData(driver, false);

            IWebElement cu_CreateSpaceship = driver.FindElement(By.Id("CU_CreateSpaceship"));
            cu_CreateSpaceship.Click();

            Thread.Sleep(1000);

            // Andmete kogumine pärast reloadi
            IWebElement indexNameSpaceship = driver.FindElement(By.Id("IndexNameSpaceship"));
            var spaceshipNameData = indexNameSpaceship.Text;

            IWebElement indexTypeSpaceship = driver.FindElement(By.Id("IndexTypeSpaceship"));
            var spaceshipTypeData = indexTypeSpaceship.Text;

            IWebElement indexCrewSpaceship = driver.FindElement(By.Id("IndexCrewSpaceship"));
            var spaceshipCrewData = indexCrewSpaceship.Text;

            Assert.Equal(spaceshipNameData, "i add Name for spaceship");

            Assert.True(spaceshipTypeData == "i add Type for spaceship");

            Assert.Equal(spaceshipCrewData, "12345");
        }

        [Fact]
        [Order(2)]
        public void Should_NavigateToDetails_OfASpaceship_WithPreviouslyCorrectData()
        {
            //Ülesehitus
            IWebDriver driver = SetupAndNavigatesIndex();
            ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));

            foreach (var tr in table)
            {
                var rowElements = tr.FindElements(By.Id("IndexNameSpaceship"));

                if (rowElements.Count > 0 &&
                    rowElements[0].Text == "i add Name for spaceship")
                {
                    tr.FindElement(By.Id("IndexSpaceshipDetails")).Click();
                    break;
                }
            }

            Thread.Sleep(500);

            IWebElement details_SpaceshipId = driver.FindElement(By.Id("Details_SS_Id"));
            var details_id = details_SpaceshipId.GetAttribute("value");

            IWebElement details_SpaceshipName = driver.FindElement(By.Id("Details_SS_Name"));
            var details_name = details_SpaceshipName.GetAttribute("value");

            IWebElement details_SpaceshipType = driver.FindElement(By.Id("Details_SS_Type"));
            var details_type = details_SpaceshipType.GetAttribute("value");

            IWebElement details_SpaceshipCrew = driver.FindElement(By.Id("Details_SS_Crew"));
            var details_crew = details_SpaceshipCrew.GetAttribute("value");

            IWebElement details_SpaceshipPower = driver.FindElement(By.Id("Details_SS_EP"));
            var details_power = details_SpaceshipPower.GetAttribute("value");

            //Kontroll
            Assert.NotNull(details_id);
            Assert.True(details_id.Contains("-") && details_id.Count('-') == 4);
            Assert.True(details_id.Substring(0, 9).EndsWith("-"));
            Assert.Equal("i add Name for spaceship", details_name);
            Assert.Equal("i add Type for spaceship", details_type);
            Assert.Equal("12345", details_crew);
            Assert.Equal("66666667", details_power);
        }

        [Fact]
        [Order(3)]
        public void Should_UpdateDetails_OfASpaceShip_WithNewData()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigatesIndex();
            ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));

            //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
            //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
            foreach (var tr in table)
            {
                var rowElements = tr.FindElements(By.Id("IndexNameSpaceship"));

                if (rowElements.Count > 0 &&
                    rowElements[0].Text == "i add Name for spaceship")
                {
                    tr.FindElement(By.Id("IndexSpaceshipUpdate")).Click();
                    break;
                }
            }
            //sisestatavad andmed muutuseks
            InsertSpaceshipData(driver, true);

            IWebElement cu_UpdateSpaceship = driver.FindElement(By.Id("CU_UpdateSpaceship"));
            cu_UpdateSpaceship.Click();

        }

        [Fact]
        [Order(4)]
        public void Should_DeleteSpaceship_FromAppDeleteView()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigatesIndex();
            ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));

            //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
            //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
            foreach (var tr in table)
            {
                var rowElements = tr.FindElements(By.Id("IndexNameSpaceship"));

                if (rowElements.Count > 0 &&
                    rowElements[0].Text == "This is a Spaceship")
                {
                    tr.FindElement(By.Id("IndexSpaceshipDelete")).Click();
                    break;
                }
            }
            
            IWebElement cu_DeleteSpaceship = driver.FindElement(By.Id("D_ConfirmDeleteSpaceship"));
            cu_DeleteSpaceship.Click();
                       
            int rowCount = driver.FindElements(By.TagName("tr")).Count();
            Assert.True(rowCount < 2);
        }

        [Fact]
        [Order(5)] // Järjesti millel on väärtus "1", järjestatakse väiksem enne.
        public void Should_NavigateToCreate_AddSpaceship_WithCorrectData_ReturnToIndex_Proper()
        {
            //Sisestatavad andmed
            IWebDriver driver = SetupAndNavigatesIndex();

            //tegevus
            IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();

            //sisestatavad andmed, siin tagastub ka kasutatud nimi
            string concreteName = InsertSpaceshipData_ReturnName(driver, false);

            IWebElement cu_CreateSpaceship = driver.FindElement(By.Id("CU_CreateSpaceship"));
            cu_CreateSpaceship.Click();

            //Thread.Sleep(1000);

            // Andmete kogumine pärast reloadi
            IWebElement indexNameSpaceship = driver.FindElement(By.Id("IndexNameSpaceship"));
            var spaceshipNameData = indexNameSpaceship.Text;

            IWebElement indexTypeSpaceship = driver.FindElement(By.Id("IndexTypeSpaceship"));
            var spaceshipTypeData = indexTypeSpaceship.Text;

            IWebElement indexCrewSpaceship = driver.FindElement(By.Id("IndexCrewSpaceship"));
            var spaceshipCrewData = indexCrewSpaceship.Text;

            bool doesItExist = false;

            // Otsi kõik read
            //Otsi nime elemendist concretName võrdlusega
            //Kui on desitexist on true

            //Kontroll
            //Assert.True(concreteName == indexNameSpaceship.Text);
            Assert.True(true == doesItExist);

            Assert.Equal(spaceshipNameData, "i add Name for spaceship");

            Assert.True(spaceshipTypeData == "i add Type for spaceship");

            Assert.Equal(spaceshipCrewData, "12345");
        }


        private static IWebDriver SetupAndNavigatesIndex()
        {
            //Firefoxi käskiv ja juhtiv draiver
            IWebDriver driver = new FirefoxDriver();

            //Aadress millele draiver navigeerib
            driver.Url = "https://localhost:7292/";

            //Lehelt otsitav element
            IWebElement navigateToSpaceship = driver.FindElement(By.LinkText("Spaceship"));
            navigateToSpaceship.Click();
            //Selle elemendiga tehtav tegevus
            return driver;
        }

        private void InsertSpaceshipData(IWebDriver driver, bool isOneOrTwo = false)
        {
            if (isOneOrTwo)
            {
                IWebElement cu_NameEntrySpaceship = driver.FindElement(By.Id("CU_NameEntrySpaceship"));
                cu_NameEntrySpaceship.Clear();
                cu_NameEntrySpaceship.SendKeys("This is a Spaceship");

                IWebElement cu_ShipTypeEntrySpaceship = driver.FindElement(By.Id("CU_ShipTypeEntrySpaceship"));
                cu_ShipTypeEntrySpaceship.Clear();
                cu_ShipTypeEntrySpaceship.SendKeys("This is Type of Spaceship");

                IWebElement cu_CrewEntrySpaceship = driver.FindElement(By.Id("CU_CrewEntrySpaceship"));
                cu_CrewEntrySpaceship.Clear();
                cu_CrewEntrySpaceship.SendKeys("54321");

                IWebElement cu_EnginePowerEntrySpaceship = driver.FindElement(By.Id("CU_EnginePowerEntrySpaceship"));
                cu_EnginePowerEntrySpaceship.Clear();
                cu_EnginePowerEntrySpaceship.SendKeys("777777776");
            }
            else
            {
                IWebElement cu_NameEntrySpaceship = driver.FindElement(By.Id("CU_NameEntrySpaceship"));
                cu_NameEntrySpaceship.Clear();
                cu_NameEntrySpaceship.SendKeys("i add Name for spaceship");

                IWebElement cu_ShipTypeEntrySpaceship = driver.FindElement(By.Id("CU_ShipTypeEntrySpaceship"));
                cu_ShipTypeEntrySpaceship.Clear();
                cu_ShipTypeEntrySpaceship.SendKeys("i add Type for spaceship");

                IWebElement cu_CrewEntrySpaceship = driver.FindElement(By.Id("CU_CrewEntrySpaceship"));
                cu_CrewEntrySpaceship.Clear();
                cu_CrewEntrySpaceship.SendKeys("12345");

                IWebElement cu_EnginePowerEntrySpaceship = driver.FindElement(By.Id("CU_EnginePowerEntrySpaceship"));
                cu_EnginePowerEntrySpaceship.Clear();
                cu_EnginePowerEntrySpaceship.SendKeys("66666667");
            }            
        }

        private string InsertSpaceshipData_ReturnName(IWebDriver driver, bool isOneOrTwo = false)
        {
            Guid returnGuid = Guid.NewGuid();
            if (isOneOrTwo)
            {
                IWebElement cu_NameEntrySpaceship = driver.FindElement(By.Id("CU_NameEntrySpaceship"));
                cu_NameEntrySpaceship.Clear();
                cu_NameEntrySpaceship.SendKeys(returnGuid.ToString());

                IWebElement cu_ShipTypeEntrySpaceship = driver.FindElement(By.Id("CU_ShipTypeEntrySpaceship"));
                cu_ShipTypeEntrySpaceship.Clear();
                cu_ShipTypeEntrySpaceship.SendKeys("This is Type of Spaceship");

                IWebElement cu_CrewEntrySpaceship = driver.FindElement(By.Id("CU_CrewEntrySpaceship"));
                cu_CrewEntrySpaceship.Clear();
                cu_CrewEntrySpaceship.SendKeys("54321");

                IWebElement cu_EnginePowerEntrySpaceship = driver.FindElement(By.Id("CU_EnginePowerEntrySpaceship"));
                cu_EnginePowerEntrySpaceship.Clear();
                cu_EnginePowerEntrySpaceship.SendKeys("777777776");
            }
            else
            {
                IWebElement cu_NameEntrySpaceship = driver.FindElement(By.Id("CU_NameEntrySpaceship"));
                cu_NameEntrySpaceship.Clear();
                cu_NameEntrySpaceship.SendKeys(returnGuid.ToString());

                IWebElement cu_ShipTypeEntrySpaceship = driver.FindElement(By.Id("CU_ShipTypeEntrySpaceship"));
                cu_ShipTypeEntrySpaceship.Clear();
                cu_ShipTypeEntrySpaceship.SendKeys("i add Type for spaceship");

                IWebElement cu_CrewEntrySpaceship = driver.FindElement(By.Id("CU_CrewEntrySpaceship"));
                cu_CrewEntrySpaceship.Clear();
                cu_CrewEntrySpaceship.SendKeys("12345");

                IWebElement cu_EnginePowerEntrySpaceship = driver.FindElement(By.Id("CU_EnginePowerEntrySpaceship"));
                cu_EnginePowerEntrySpaceship.Clear();
                cu_EnginePowerEntrySpaceship.SendKeys("66666667");
            }

            return returnGuid.ToString();
        }
    }
}
