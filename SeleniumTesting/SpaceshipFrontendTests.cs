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
            //Firefoxi käskiv ja juhtiv draiver
            IWebDriver driver = new FirefoxDriver();

            //Aadress millele draiver navigeerib
            driver.Url = "https://localhost:7292/";

            //Lehelt otsitav element
            IWebElement navigateToSpaceship = driver.FindElement(By.LinkText("Spaceship"));

            //Selle elemendiga tehtav tegevus
            IWebElement createInIndex = driver.FindElement(By.Id("SpaceshipNavigate"));
        }
    }
}
