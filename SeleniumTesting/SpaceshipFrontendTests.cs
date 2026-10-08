using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit;

namespace TARge25_Shop.SeleniumTesting
{
    public class SpaceshipFrontendTests
    {
        [Fact]
        public void Should_NavigateToCreate_AddSpaceship_WithCorrectData_ReturnToIndex()
        {
            //Sisestatavad andmed
            IWebDriver driver = SetupAndNavigatesIndex();

            IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();

            InsertSpaceshipData(driver);

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
        public void Should_NavigateToDetails_OfASpaceship_WithPreviouslyCorrectData()
        {
            //Ülesehitus
            IWebDriver driver = SetupAndNavigatesIndex();
            ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));

            //foreach (var tr in table)
            //{

            //    if(tr.FindElement(By.Id("IndexNameSpaceship")).Text == "i add Name for spaceship")
            //    {
            //        tr.FindElement(By.Id("IndexSpaceshipDetails")).Click();
            //        break;
            //    }
            //}

            foreach (var tr in table)
            {
                var nameElements = tr.FindElements(By.Id("IndexNameSpaceship"));

                if (nameElements.Count > 0 &&
                    nameElements[0].Text == "i add Name for spaceship")
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

        //[Fact]
        //public void Should_UpdateDetails_OfASpaceShip_WithNewData()
        //{
        //    //ülesseade
        //    IWebDriver driver = SetupAndNavigatesIndex();

        //    //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
        //    //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
        //    var table = driver.FindElement(By.Id("IndexTable"));
        //    List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
        //    rows.RemoveAt(0);
        //    //tsükkel käib kõik read läbi

        //    foreach (var row in rows)
        //    {

        //        var locatedelement = row.FindElement(By.Id("IndexNameSpaceship"));
        //        if (locatedelement.Text == "i add name for spaceship")
        //        {
        //            //siis vajuta selles reas asuvat details nuppu
        //            var rowelement = row.FindElement(By.Id("IndexActionsSpaceship"));
        //            var button = rowelement.FindElement(By.Id("IndexSpaceshipUpdate"));
        //            button.Click();
        //            //pärast õiget vajutust, tsükkel katkestatakse
        //            //arvuti tudub
        //            Thread.Sleep(500);
        //            break;
        //        }
        //    }
        //    //sisestatavad andmed
        //    InsertSpaceshipData(driver, true);

        //    IWebElement cu_CreateSpaceship = driver.FindElement(By.Id("CU_CreateSpaceship"));
        //    cu_CreateSpaceship.Click();

        //    //arvuti tudub
        //    Thread.Sleep(1000);
        //}


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

        private void InsertSpaceshipData(IWebDriver driver)
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
}
