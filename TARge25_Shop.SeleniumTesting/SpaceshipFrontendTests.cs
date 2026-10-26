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
        public static string thisRunGuid {  get; set; }

        //[Fact]
        //[Order(1)] // Järjesti millel on väärtus "1", järjestatakse väiksem enne.
        //public void Should_NavigateToCreate_AddSpaceship_WithCorrectData_ReturnToIndex()
        //{
        //    //Sisestatavad andmed
        //    IWebDriver driver = SetupAndNavigatesIndex();

        //    //tegevus
        //    IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
        //    createInIndex.Click();

        //    //sisestatavad andmed
        //    InsertSpaceshipData(driver, false);

        //    IWebElement cu_CreateSpaceship = driver.FindElement(By.Id("CU_CreateSpaceship"));
        //    cu_CreateSpaceship.Click();

        //    Thread.Sleep(1000);

        //    // Andmete kogumine pärast reloadi
        //    IWebElement indexNameSpaceship = driver.FindElement(By.Id("IndexNameSpaceship"));
        //    var spaceshipNameData = indexNameSpaceship.Text;

        //    IWebElement indexTypeSpaceship = driver.FindElement(By.Id("IndexTypeSpaceship"));
        //    var spaceshipTypeData = indexTypeSpaceship.Text;

        //    IWebElement indexCrewSpaceship = driver.FindElement(By.Id("IndexCrewSpaceship"));
        //    var spaceshipCrewData = indexCrewSpaceship.Text;

        //    Assert.Equal(spaceshipNameData, "i add Name for spaceship");

        //    Assert.True(spaceshipTypeData == "i add Type for spaceship");

        //    Assert.Equal(spaceshipCrewData, "12345");
        //}

        //[Fact]
        //[Order(2)]
        //public void Should_NavigateToDetails_OfASpaceship_WithPreviouslyCorrectData()
        //{
        //    //Ülesehitus
        //    IWebDriver driver = SetupAndNavigatesIndex();
        //    ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));

        //    foreach (var tr in table)
        //    {
        //        var rowElements = tr.FindElements(By.Id("IndexNameSpaceship"));

        //        if (rowElements.Count > 0 &&
        //            rowElements[0].Text == "i add Name for spaceship")
        //        {
        //            tr.FindElement(By.Id("IndexSpaceshipDetails")).Click();
        //            break;
        //        }
        //    }

        //    Thread.Sleep(500);

        //    IWebElement details_SpaceshipId = driver.FindElement(By.Id("Details_SS_Id"));
        //    var details_id = details_SpaceshipId.GetAttribute("value");

        //    IWebElement details_SpaceshipName = driver.FindElement(By.Id("Details_SS_Name"));
        //    var details_name = details_SpaceshipName.GetAttribute("value");

        //    IWebElement details_SpaceshipType = driver.FindElement(By.Id("Details_SS_Type"));
        //    var details_type = details_SpaceshipType.GetAttribute("value");

        //    IWebElement details_SpaceshipCrew = driver.FindElement(By.Id("Details_SS_Crew"));
        //    var details_crew = details_SpaceshipCrew.GetAttribute("value");

        //    IWebElement details_SpaceshipPower = driver.FindElement(By.Id("Details_SS_EP"));
        //    var details_power = details_SpaceshipPower.GetAttribute("value");

        //    //Kontroll
        //    Assert.NotNull(details_id);
        //    Assert.True(details_id.Contains("-") && details_id.Count('-') == 4);
        //    Assert.True(details_id.Substring(0, 9).EndsWith("-"));
        //    Assert.Equal("i add Name for spaceship", details_name);
        //    Assert.Equal("i add Type for spaceship", details_type);
        //    Assert.Equal("12345", details_crew);
        //    Assert.Equal("66666667", details_power);
        //}

        //[Fact]
        //[Order(3)]
        //public void Should_UpdateDetails_OfASpaceShip_WithNewData()
        //{
        //    //ülesseade
        //    IWebDriver driver = SetupAndNavigatesIndex();
        //    ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));

        //    //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
        //    foreach (var tr in table)
        //    {
        //        var rowElements = tr.FindElements(By.Id("IndexNameSpaceship"));

        //        if (rowElements.Count > 0 &&
        //            rowElements[0].Text == "i add Name for spaceship")
        //        {
        //            tr.FindElement(By.Id("IndexSpaceshipUpdate")).Click();
        //            break;
        //        }
        //    }
        //    //sisestatavad andmed muutuseks
        //    InsertSpaceshipData(driver, true);

        //    IWebElement cu_UpdateSpaceship = driver.FindElement(By.Id("CU_UpdateSpaceship"));
        //    cu_UpdateSpaceship.Click();

        //}



        //[Fact]
        //[Order(4)]
        //public void Should_DeleteSpaceship_FromAppDeleteView()
        //{
        //    //ülesseade
        //    IWebDriver driver = SetupAndNavigatesIndex();
        //    ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));

        //    //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
        //    //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
        //    foreach (var tr in table)
        //    {
        //        var rowElements = tr.FindElements(By.Id("IndexNameSpaceship"));

        //        if (rowElements.Count > 0 &&
        //            rowElements[0].Text == "This is a Spaceship")
        //        {
        //            tr.FindElement(By.Id("IndexSpaceshipDelete")).Click();
        //            break;
        //        }
        //    }

        //    IWebElement cu_DeleteSpaceship = driver.FindElement(By.Id("D_Delete"));
        //    cu_DeleteSpaceship.Click();

        //    int rowCount = driver.FindElements(By.TagName("tr")).Count();
        //    Assert.True(rowCount < 2);
        //}


        [Fact]
        [Order(5)]
        public void Should_NavigateToCreate_AddUniqueSpaceship_WithCorrectData_ReturnToIndex()
        {
            //Sisestatavad andmed
            IWebDriver driver = SetupAndNavigatesIndex();

            //tegevus
            IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();

            //sisestatavad andmed, siin tagastub ka kasutatud nimi
            string concreteName = InsertSpaceshipData_ReturnName(driver, false);
            thisRunGuid = concreteName;

            IWebElement cu_CreateSpaceship = driver.FindElement(By.Id("CU_CreateSpaceship"));
            cu_CreateSpaceship.Click();

            //Thread.Sleep(1000);

            // Andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);

            bool isObjectFound = false;
            var spaceshipNameData = "";
            var spaceshipTypeData = "";
            var spaceshipCrewData = "";
            var spaceshipBuiltAtData = "";

            bool doesItExist = false;

            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexNameSpaceship")).Text == thisRunGuid)
                {
                    doesItExist = true;
                    spaceshipNameData = thisRunGuid;
                    spaceshipTypeData = row.FindElement(By.Id("IndexTypeSpaceship")).Text;
                    spaceshipCrewData = row.FindElement(By.Id("IndexCrewSpaceship")).Text;
                    spaceshipBuiltAtData = row.FindElement(By.Id("IndexBuiltDateSpaceship")).Text;
                    break;
                }
            }

            //Kui on desitexist on true
            driver.Quit();
            //Kontroll
            //Assert.True(concreteName == indexNameSpaceship.Text);
            Assert.True(true == doesItExist);

            Assert.Equal(spaceshipNameData, thisRunGuid);

            Assert.True(spaceshipTypeData == "i add Type for spaceship");

            Assert.Equal(spaceshipCrewData, "12345");

            Assert.True((thisRunGuid == spaceshipNameData)
                && ("i add Type for spaceship" == spaceshipTypeData)
                && ("12345" == spaceshipCrewData));
        }

        [Fact]
        [Order(6)]
        public void Should_NavigateToDetails_OfThisSpaceship_WithPreviouslyCorrectData()
        {
            //Ülesehitus
            IWebDriver driver = SetupAndNavigatesIndex();
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool isObjectFound = false;

            //tsükkel käib kõik read läbi            
            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));
                if (locatedelement.Text == thisRunGuid)
                {
                    isObjectFound = true;
                    row.FindElement(By.Id("IndexSpaceshipDetails")).Click();
                    break;
                }
            }

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

            driver.Quit();

            //Kontroll
            Assert.True(isObjectFound);
            Assert.NotNull(details_id);
            // Et ei oleks tõhi kontrollid kõikidele andmetele
            Assert.NotEmpty(details_name);
            Assert.NotNull(details_type);
            Assert.False(string.IsNullOrEmpty(details_crew));
            Assert.True(!string.IsNullOrEmpty(details_power));

            Assert.Equal(thisRunGuid, details_name);
            Assert.Equal("i add Type for spaceship", details_type);
            Assert.Equal("12345", details_crew);
            Assert.Equal("66666667", details_power);
            Assert.True(int.Parse(details_crew).GetType() == typeof(int));
            Assert.True(int.Parse(details_power).GetType() == typeof(int));
        }

        [Fact]
        [Order(7)]
        public void Should_UpdateDetails_OfThisSpaceShip_WithNewData()
        {
            ///Ülesehitus
            IWebDriver driver = SetupAndNavigatesIndex();
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;

            //tsükkel käib kõik read läbi            
            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));
                if (locatedelement.Text == thisRunGuid)
                {
                    doesItExist = true;
                    row.FindElement(By.Id("IndexSpaceshipUpdate")).Click();
                    break;
                }
            }

            //sisestatavad andmed
            InsertSpaceshipData(driver, thisRunGuid, true);

            IWebElement cu_UpdateSpaceship = driver.FindElement(By.Id("CU_UpdateSpaceship"));
            cu_UpdateSpaceship.Click();

            //andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string spaceShipNameData = "";
            string spaceShipTypeData = "";
            string spaceShipCrewData = "";
            string spaceShipBuiltAtData = "";
            doesItExist = false;

            foreach (var row in rowsOfTable)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));
                if (locatedelement.Text == thisRunGuid)
                {
                    doesItExist = true;
                    spaceShipNameData = thisRunGuid;
                    spaceShipTypeData = row.FindElement(By.Id("IndexTypeSpaceship")).Text;
                    spaceShipCrewData = row.FindElement(By.Id("IndexCrewSpaceship")).Text;
                    spaceShipBuiltAtData = row.FindElement(By.Id("IndexBuiltDateSpaceship")).Text;
                    break;
                }
            }

            driver.Quit();

            ////arvuti tudub
            //Thread.Sleep(1000);
            Assert.True(doesItExist);
            Assert.NotNull(spaceShipNameData);
            Assert.Equal(thisRunGuid, spaceShipNameData);
            Assert.False(string.IsNullOrEmpty(spaceShipTypeData));
            Assert.NotEqual(spaceShipTypeData, "i add ship type for spaceship");
            Assert.True(spaceShipTypeData == "This is Type of Spaceship");
            Assert.False(string.IsNullOrEmpty(spaceShipTypeData));
        }

        [Fact]
        [Order(8)]
        public void Should_DeleteThisSpaceship_FromAppVIADeleteView()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigatesIndex();

            //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
            //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;
            //tsükkel käib kõik read läbi

            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));
                if (locatedelement.Text == thisRunGuid)
                {
                    doesItExist = true;
                    row.FindElement(By.Id("IndexSpaceshipDelete")).Click();
                    break;
                }
            }
            IWebElement d_DeleteSpaceship = driver.FindElement(By.Id("D_Delete"));
            d_DeleteSpaceship.Click();

            bool isDeleted = false;

            table = driver.FindElement(By.Id("IndexTable"));
            rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            foreach (var row in rows)
            {
                var nameElement = row.FindElement(By.Id("IndexNameSpaceship"));
                if (nameElement.Text == thisRunGuid)
                {
                    isDeleted = false;
                    break;
                }
                else
                {
                    isDeleted = true;
                }
            }

            driver.Quit();

            Assert.True(isDeleted);
            Assert.False(!isDeleted);
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

        private void InsertSpaceshipData(IWebDriver driver, string name = null, bool isOneOrTwo = false)
        {
            if (isOneOrTwo)
            {
                IWebElement cu_NameEntrySpaceship = driver.FindElement(By.Id("CU_NameEntrySpaceship"));
                cu_NameEntrySpaceship.Clear();


                if (name != null)
                {
                    cu_NameEntrySpaceship.SendKeys(name);
                }
                else
                {
                    cu_NameEntrySpaceship.SendKeys("This is a Spaceship");
                }

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
