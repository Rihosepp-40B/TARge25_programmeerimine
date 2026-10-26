using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit;

namespace TARge25_Shop.SeleniumTesting
{
    [TestCaseOrderer("TARge25_Shop.SeleniumTesting.PriorityOrderer", "TARge25_Shop.SeleniumTesting")]
    public class KindergartenFrontendTest
    {
        public static string thisRunGuid { get; set; }

        [Fact]
        [Order(1)]
        public void Should_NavigateToCreate_AddUniqueKindergarten_WithCorrectData_ReturnToIndex()
        {
            //Sisestatavad andmed
            IWebDriver driver = SetupAndNavigateIndex();

            //tegevus
            IWebElement createInIndex = driver.FindElement(By.Id("I_Kg_CreBtn"));
            createInIndex.Click();

            //sisestatavad andmed, siin tagastub ka kasutatud nimi
            string concreteName = InsertKindergartenData_ReturnGName(driver, false);
            thisRunGuid = concreteName;

            IWebElement cu_CreateKindergarten = driver.FindElement(By.Id("CU_Kg_CreBtn"));
            cu_CreateKindergarten.Click();

            // Andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);

            var kindergartenGNameData = "";
            var kindergartenKgNameData = "";
            var kindergartenTNameData = "";
            var kindergartenCreatedAtData = "";

            bool doesItExist = false;

            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("I_Kg_GN")).Text == thisRunGuid)
                {
                    doesItExist = true;
                    kindergartenGNameData = thisRunGuid;
                    kindergartenKgNameData = row.FindElement(By.Id("I_Kg_KN")).Text;
                    kindergartenTNameData = row.FindElement(By.Id("I_Kg_TN")).Text;
                    kindergartenCreatedAtData = row.FindElement(By.Id("I_Kg_CA")).Text;
                    break;
                }
            }

            //Kui on desitexist on true
            driver.Quit();
            //Kontroll
            //Assert.True(true == doesItExist);

            Assert.Equal(kindergartenGNameData, thisRunGuid);

            Assert.Equal(kindergartenKgNameData, "another Kindergarten");

            Assert.Equal(kindergartenKgNameData, "another Kindergarten");

            Assert.True((thisRunGuid == kindergartenGNameData)
                && ("another Kindergarten" == kindergartenKgNameData)
                && ("another Teacher" == kindergartenTNameData));
        }

        [Fact]
        [Order(2)]
        public void Should_NavigateToDetails_OfThisKindergarten_WithPreviouslyCorrectData()
        {
            //Ülesehitus
            IWebDriver driver = SetupAndNavigateIndex();
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool isObjectFound = false;

            //tsükkel käib kõik read läbi            
            foreach (var row in rows)
            {

                var findKindergartenGN = row.FindElement(By.Id("I_Kg_GN"));
                if (findKindergartenGN.Text == thisRunGuid)
                {
                    isObjectFound = true;
                    row.FindElement(By.Id("I_Kg_DetBtn")).Click();
                    break;
                }
            }

            IWebElement det_Kg_Id = driver.FindElement(By.Id("Det_Kg_Id"));
            var details_Id = det_Kg_Id.Text;

            IWebElement det_Kg_GN = driver.FindElement(By.Id("Det_Kg_GN"));
            var details_GN = det_Kg_GN.Text;

            IWebElement det_Kg_CC = driver.FindElement(By.Id("Det_Kg_CC"));
            var details_CC = det_Kg_CC.Text;

            IWebElement det_Kg_KN = driver.FindElement(By.Id("Det_Kg_KN"));
            var details_KN = det_Kg_KN.Text;

            IWebElement det_Kg_TN = driver.FindElement(By.Id("Det_Kg_TN"));
            var details_TN = det_Kg_TN.Text;

            driver.Quit();

            //Kontroll
            Assert.True(isObjectFound);
            Assert.NotNull(details_Id);
            // Et ei oleks tühi kontrollime kõiki andmeid
            Assert.NotEmpty(details_GN);
            Assert.NotNull(details_CC);
            Assert.False(string.IsNullOrEmpty(details_KN));
            Assert.True(!string.IsNullOrEmpty(details_TN));

            Assert.Equal(thisRunGuid, details_GN);
            Assert.Equal("20", details_CC);
            Assert.Equal("another Kindergarten", details_KN);
            Assert.Equal("another Teacher", details_TN);
            Assert.True(int.Parse(details_CC).GetType() == typeof(int));
        }

        [Fact]
        [Order(3)]
        public void Should_UpdateDetails_OfThisKindergarten_WithNewData()
        {
            //Ülesehitus
            IWebDriver driver = SetupAndNavigateIndex();
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;

            //tsükkel käib kõik read läbi            
            foreach (var row in rows)
            {

                var findKindergartenGN = row.FindElement(By.Id("I_Kg_GN"));
                if (findKindergartenGN.Text == thisRunGuid)
                {
                    row.FindElement(By.Id("I_Kg_UpdBtn")).Click();
                    break;
                }
            }

            //sisestatavad andmed
            InsertKindergartenData(driver, thisRunGuid, true);

            IWebElement cu_UpdateKg = driver.FindElement(By.Id("CU_Kg_UpdBtn"));
            cu_UpdateKg.Click();

            //andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);

            string kindergartenGNameData = "";
            string kindergartenKgNameData = "";
            string kindergartenTNameData = "";
            string kindergartenCreatedAtData = "";

            foreach (var row in rowsOfTable)
            {

                var locatedelement = row.FindElement(By.Id("I_Kg_GN"));
                if (locatedelement.Text == thisRunGuid)
                {
                    doesItExist = true;
                    kindergartenGNameData = thisRunGuid;
                    kindergartenKgNameData = row.FindElement(By.Id("I_Kg_KN")).Text;
                    kindergartenTNameData = row.FindElement(By.Id("I_Kg_TN")).Text;
                    kindergartenCreatedAtData = row.FindElement(By.Id("I_Kg_CA")).Text;
                    break;
                }
            }

            driver.Quit();

            ////arvuti tudub
            //Thread.Sleep(1000);
            Assert.True(doesItExist);
            Assert.NotNull(kindergartenGNameData);
            Assert.Equal(thisRunGuid, kindergartenGNameData);
            Assert.False(string.IsNullOrEmpty(kindergartenKgNameData));
            Assert.NotEqual(kindergartenTNameData, "another Teacher");
            Assert.True(kindergartenTNameData == "a Teacher");
            Assert.False(string.IsNullOrEmpty(kindergartenTNameData));
        }
                
        [Fact]
        [Order(8)]
        public void Should_DeleteThisKindergarten_FromAppVIADeleteView()
        {
            //Ülesehitus
            IWebDriver driver = SetupAndNavigateIndex();
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;

            //tsükkel käib kõik read läbi            
            foreach (var row in rows)
            {

                var findKindergartenGN = row.FindElement(By.Id("I_Kg_GN"));
                if (findKindergartenGN.Text == thisRunGuid)
                {
                    row.FindElement(By.Id("I_Kg_DelBtn")).Click();
                    break;
                }
            }

            IWebElement d_DeleteKg = driver.FindElement(By.Id("Del_Kg_Del"));
            d_DeleteKg.Click();

            bool isDeleted = false;

            table = driver.FindElement(By.Id("IndexTable"));
            rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            foreach (var row in rows)
            {
                var nameElement = row.FindElement(By.Id("I_Kg_GN"));
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

         //
        // ENDA 4 testi tulevad alates siit
        //---------------------------||-||-||-||
        //---------------------------\/-\/-\/-\/

        [Fact]
        [Order(4)]
        public void ShouldNot_UpdateThisKindergarten_WhenGroupNameIsWhitespace()
        {
            //Ülesehitus
            IWebDriver driver = SetupAndNavigateIndex();
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;
            bool isGNupdated = true;
            string startKN = "";
            string startTN = "";


            //tsükkel käib kõik read läbi            
            foreach (var row in rows)
            {
                var findKgGN = row.FindElement(By.Id("I_Kg_GN"));
                if (findKgGN.Text == thisRunGuid)
                {
                    startKN = row.FindElement(By.Id("I_Kg_KN")).Text;
                    startTN = row.FindElement(By.Id("I_Kg_TN")).Text;
                    row.FindElement(By.Id("I_Kg_UpdBtn")).Click();
                    break;
                }
            }

            //sisestatavad andmed
            InsertKindergartenData(driver, thisRunGuid, true);

            IWebElement cu_KgGN_Entry = driver.FindElement(By.Id("CU_Kg_GN"));
            cu_KgGN_Entry.Clear();
            cu_KgGN_Entry.SendKeys(" ");


            IWebElement cu_UpdateKg = driver.FindElement(By.Id("CU_Kg_UpdBtn"));
            cu_UpdateKg.Click();

            //andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);

            string dataKgGN = "";
            string dataKgKN = "";
            string dataKgTN = "";
            string dataKgCA = "";

            foreach (var row in rowsOfTable)
            {
                var locatedelement = row.FindElement(By.Id("I_Kg_GN"));
                if (locatedelement.Text == thisRunGuid)
                {
                    doesItExist = true;
                    isGNupdated = false;
                    dataKgGN = thisRunGuid;
                    dataKgKN = row.FindElement(By.Id("I_Kg_KN")).Text;
                    dataKgTN = row.FindElement(By.Id("I_Kg_TN")).Text;
                    dataKgCA = row.FindElement(By.Id("I_Kg_CA")).Text;
                    break;
                }
            }

            driver.Quit();

            Assert.True(doesItExist);
            Assert.NotNull(dataKgGN);
            Assert.False(isGNupdated);
            Assert.Equal(thisRunGuid, dataKgGN);
            Assert.False(string.IsNullOrEmpty(dataKgGN));
            Assert.False(string.IsNullOrEmpty(dataKgKN));
            Assert.False(string.IsNullOrEmpty(dataKgTN));
            Assert.Equal(dataKgKN, startKN);
            Assert.Equal(dataKgTN, startTN);
        }
        
        [Fact]
        [Order(5)]
        public void ShouldNot_UpdateThisKindergarten_WhenKindergartenNameIsWhitespace()
        {
            //Ülesehitus
            IWebDriver driver = SetupAndNavigateIndex();
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;
            bool isKNupdated = true;
            string startKN = "";
            string startTN = "";


            //tsükkel käib kõik read läbi            
            foreach (var row in rows)
            {
                var findKgGN = row.FindElement(By.Id("I_Kg_GN"));
                if (findKgGN.Text == thisRunGuid)
                {
                    startKN = row.FindElement(By.Id("I_Kg_KN")).Text;
                    startTN = row.FindElement(By.Id("I_Kg_TN")).Text;
                    row.FindElement(By.Id("I_Kg_UpdBtn")).Click();
                    break;
                }
            }

            //sisestatavad andmed
            InsertKindergartenData(driver, thisRunGuid, true);

            IWebElement cu_KgKN_Entry = driver.FindElement(By.Id("CU_Kg_KN"));
            cu_KgKN_Entry.Clear();
            cu_KgKN_Entry.SendKeys(" ");


            IWebElement cu_UpdateKg = driver.FindElement(By.Id("CU_Kg_UpdBtn"));
            cu_UpdateKg.Click();

            //andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);

            string dataKgGN = "";
            string dataKgKN = "";
            string dataKgTN = "";
            string dataKgCA = "";

            foreach (var row in rowsOfTable)
            {
                var locatedelement = row.FindElement(By.Id("I_Kg_KN"));
                if (locatedelement.Text == startKN)
                {
                    doesItExist = true;
                    isKNupdated = false;
                    dataKgGN = thisRunGuid;
                    dataKgKN = row.FindElement(By.Id("I_Kg_KN")).Text;
                    dataKgTN = row.FindElement(By.Id("I_Kg_TN")).Text;
                    dataKgCA = row.FindElement(By.Id("I_Kg_CA")).Text;
                    break;
                }
            }

            driver.Quit();

            Assert.True(doesItExist);
            Assert.NotNull(dataKgGN);
            Assert.False(isKNupdated);
            Assert.Equal(thisRunGuid, dataKgGN);
            Assert.False(string.IsNullOrEmpty(dataKgGN));
            Assert.False(string.IsNullOrEmpty(dataKgKN));
            Assert.False(string.IsNullOrEmpty(dataKgTN));
            Assert.Equal(dataKgKN, startKN);
            Assert.Equal(dataKgTN, startTN);
        }
        
        [Fact]
        [Order(6)]
        public void ShouldNot_UpdateThisKindergarten_WhenTeacherNameIsWhitespace()
        {
            //Ülesehitus
            IWebDriver driver = SetupAndNavigateIndex();
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;
            bool isTNupdated = true;
            string startKN = "";
            string startTN = "";


            //tsükkel käib kõik read läbi            
            foreach (var row in rows)
            {
                var findKgGN = row.FindElement(By.Id("I_Kg_GN"));
                if (findKgGN.Text == thisRunGuid)
                {
                    startKN = row.FindElement(By.Id("I_Kg_KN")).Text;
                    startTN = row.FindElement(By.Id("I_Kg_TN")).Text;
                    row.FindElement(By.Id("I_Kg_UpdBtn")).Click();
                    break;
                }
            }

            //sisestatavad andmed
            InsertKindergartenData(driver, thisRunGuid, true);

            IWebElement cu_KgTN_Entry = driver.FindElement(By.Id("CU_Kg_TN"));
            cu_KgTN_Entry.Clear();
            cu_KgTN_Entry.SendKeys(" ");


            IWebElement cu_UpdateKg = driver.FindElement(By.Id("CU_Kg_UpdBtn"));
            cu_UpdateKg.Click();

            //andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);

            string dataKgGN = "";
            string dataKgKN = "";
            string dataKgTN = "";
            string dataKgCA = "";

            foreach (var row in rowsOfTable)
            {
                var locatedelement = row.FindElement(By.Id("I_Kg_TN"));
                if (locatedelement.Text == startTN)
                {
                    doesItExist = true;
                    isTNupdated = false;
                    dataKgGN = thisRunGuid;
                    dataKgKN = row.FindElement(By.Id("I_Kg_KN")).Text;
                    dataKgTN = row.FindElement(By.Id("I_Kg_TN")).Text;
                    dataKgCA = row.FindElement(By.Id("I_Kg_CA")).Text;
                    break;
                }
            }

            driver.Quit();

            Assert.True(doesItExist);
            Assert.NotNull(dataKgGN);
            Assert.False(isTNupdated);
            Assert.Equal(thisRunGuid, dataKgGN);
            Assert.False(string.IsNullOrEmpty(dataKgGN));
            Assert.False(string.IsNullOrEmpty(dataKgKN));
            Assert.False(string.IsNullOrEmpty(dataKgTN));
            Assert.Equal(dataKgKN, startKN);
            Assert.Equal(dataKgTN, startTN);
        }
        
        [Fact]
        [Order(7)]
        public void Should_UpdateThisKindergarten_WithZeroChildren_WhenChildrenCountNegative()
        {
            //Ülesehitus
            IWebDriver driver = SetupAndNavigateIndex();
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;
            string startKN = "";
            string startTN = "";
            int startCC = 0;

            foreach (var row in rows)
            {

                var findGN = row.FindElement(By.Id("I_Kg_GN"));
                if (findGN.Text == thisRunGuid)
                {
                    row.FindElement(By.Id("I_Kg_DetBtn")).Click();
                    break;
                }
            }

            IWebElement det_Kg_CC = driver.FindElement(By.Id("Det_Kg_CC"));
            startCC = int.Parse(det_Kg_CC.Text);

            IWebElement det_Kg_KN = driver.FindElement(By.Id("Det_Kg_KN"));
            startKN = det_Kg_KN.Text;

            IWebElement det_Kg_TN = driver.FindElement(By.Id("Det_Kg_TN"));
            startTN = det_Kg_TN.Text;

            IWebElement det_Back = driver.FindElement(By.Id("Det_Kg_Back"));
            det_Back.Click();


            var table2 = driver.FindElement(By.Id("IndexTable"));
            var rows2 = table2.FindElements(By.TagName("tr")).ToList();
            rows2.RemoveAt(0);

            //tsükkel käib kõik read läbi            
            foreach (var row in rows2)
            {
                var findKgGN = row.FindElements(By.Id("I_Kg_GN"));
                if (findKgGN.Count == 0)
                {
                    continue;
                }

                if (findKgGN[0].Text == thisRunGuid)
                {
                    row.FindElement(By.Id("I_Kg_UpdBtn")).Click();
                    break;
                }
            }

            //sisestatavad andmed

            IWebElement cu_KgCC_Entry = driver.FindElement(By.Id("CU_Kg_CC"));
            cu_KgCC_Entry.Clear();
            cu_KgCC_Entry.SendKeys("-10");

            IWebElement cu_KgKN_Entry = driver.FindElement(By.Id("CU_Kg_KN"));
            cu_KgKN_Entry.Clear();
            cu_KgKN_Entry.SendKeys("Not another one");

            IWebElement cu_KgTN_Entry = driver.FindElement(By.Id("CU_Kg_TN"));
            cu_KgTN_Entry.Clear();
            cu_KgTN_Entry.SendKeys("Teacher Teacher Teacher");


            IWebElement cu_UpdateKg = driver.FindElement(By.Id("CU_Kg_UpdBtn"));
            cu_UpdateKg.Click();

            //andmete kogumine pärast reloadi

            var table3 = driver.FindElement(By.Id("IndexTable"));
            var rows3 = table3.FindElements(By.TagName("tr")).ToList();
            rows2.RemoveAt(0);

            //tsükkel käib kõik read läbi            
            foreach (var row in rows3)
            {
                var findKgGN = row.FindElements(By.Id("I_Kg_GN"));
                if (findKgGN.Count == 0)
                {
                    continue;
                }
                if (findKgGN[0].Text == thisRunGuid)
                {
                    doesItExist = true;
                    row.FindElement(By.Id("I_Kg_DetBtn")).Click();
                    break;
                }
            }

            IWebElement det_Kg_GN = driver.FindElement(By.Id("Det_Kg_GN"));
            string details_GN = det_Kg_GN.Text;

            IWebElement det_Kg_CC2 = driver.FindElement(By.Id("Det_Kg_CC"));
            int details_CC = int.Parse(det_Kg_CC2.Text);

            IWebElement det_Kg_KN2 = driver.FindElement(By.Id("Det_Kg_KN"));
            string details_KN = det_Kg_KN2.Text;

            IWebElement det_Kg_TN2 = driver.FindElement(By.Id("Det_Kg_TN"));
            string details_TN = det_Kg_TN2.Text;

            driver.Quit();

            Assert.True(doesItExist);
            Assert.NotNull(details_GN);
            Assert.Equal(thisRunGuid, details_GN);
            Assert.True(startCC > 0);
            Assert.False(string.IsNullOrEmpty(details_GN));
            Assert.False(string.IsNullOrEmpty(details_KN));
            Assert.False(string.IsNullOrEmpty(details_TN));
            Assert.True(details_CC == 0);
            Assert.NotEqual(startCC, details_CC);
            Assert.NotEqual(details_KN, startKN);
            Assert.NotEqual(details_TN, startTN);
        }
        
        //
        // Abifunktsioonid
        //---------------------------||-||-||-||
        //---------------------------\/-\/-\/-\/

        private static IWebDriver SetupAndNavigateIndex()
        {
            //Firefoxi käskiv ja juhtiv draiver
            IWebDriver driver = new FirefoxDriver();

            //Aadress millele draiver navigeerib
            driver.Url = "https://localhost:7292/";

            //Lehelt otsitav element
            IWebElement navigateToKindergarten = driver.FindElement(By.LinkText("Kindergarten"));
            navigateToKindergarten.Click();
            //Selle elemendiga tehtav tegevus
            return driver;
        }
       
        private void InsertKindergartenData(IWebDriver driver, string name = null, bool isOneOrTwo = false)
        {
            if (isOneOrTwo)
            {
                IWebElement cu_GNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_GN"));
                cu_GNameEntryKindergarten.Clear();


                if (name != null)
                {
                    cu_GNameEntryKindergarten.SendKeys(name);
                }
                else
                {
                    cu_GNameEntryKindergarten.SendKeys("a Group");
                }

                IWebElement cu_CCountEntryKindergarten = driver.FindElement(By.Id("CU_Kg_CC"));
                cu_CCountEntryKindergarten.Clear();
                cu_CCountEntryKindergarten.SendKeys("10");

                IWebElement cu_KgNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_KN"));
                cu_KgNameEntryKindergarten.Clear();
                cu_KgNameEntryKindergarten.SendKeys("a Kindergarten");

                IWebElement cu_TNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_TN"));
                cu_TNameEntryKindergarten.Clear();
                cu_TNameEntryKindergarten.SendKeys("a Teacher");
            }
            else
            {
                IWebElement cu_GNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_GN"));
                cu_GNameEntryKindergarten.Clear();
                cu_GNameEntryKindergarten.SendKeys("another Group");

                IWebElement cu_CCountEntryKindergarten = driver.FindElement(By.Id("CU_Kg_CC"));
                cu_CCountEntryKindergarten.Clear();
                cu_CCountEntryKindergarten.SendKeys("20");

                IWebElement cu_KgNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_KN"));
                cu_KgNameEntryKindergarten.Clear();
                cu_KgNameEntryKindergarten.SendKeys("another Kindergarten");

                IWebElement cu_TNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_TN"));
                cu_TNameEntryKindergarten.Clear();
                cu_TNameEntryKindergarten.SendKeys("another Teacher");
            }
        }

        private string InsertKindergartenData_ReturnGName(IWebDriver driver, bool isOneOrTwo = false)
        {
            Guid returnGuid = Guid.NewGuid();
            if (isOneOrTwo)
            {
                IWebElement cu_GNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_GN"));
                cu_GNameEntryKindergarten.Clear();
                cu_GNameEntryKindergarten.SendKeys(returnGuid.ToString());

                IWebElement cu_CCountEntryKindergarten = driver.FindElement(By.Id("CU_Kg_CC"));
                cu_CCountEntryKindergarten.Clear();
                cu_CCountEntryKindergarten.SendKeys("10");

                IWebElement cu_KgNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_KN"));
                cu_KgNameEntryKindergarten.Clear();
                cu_KgNameEntryKindergarten.SendKeys("a Kindergarten");

                IWebElement cu_TNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_TN"));
                cu_TNameEntryKindergarten.Clear();
                cu_TNameEntryKindergarten.SendKeys("a Teacher");
            }
            else
            {
                IWebElement cu_GNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_GN"));
                cu_GNameEntryKindergarten.Clear();
                cu_GNameEntryKindergarten.SendKeys(returnGuid.ToString());

                IWebElement cu_CCountEntryKindergarten = driver.FindElement(By.Id("CU_Kg_CC"));
                cu_CCountEntryKindergarten.Clear();
                cu_CCountEntryKindergarten.SendKeys("20");

                IWebElement cu_KgNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_KN"));
                cu_KgNameEntryKindergarten.Clear();
                cu_KgNameEntryKindergarten.SendKeys("another Kindergarten");

                IWebElement cu_TNameEntryKindergarten = driver.FindElement(By.Id("CU_Kg_TN"));
                cu_TNameEntryKindergarten.Clear();
                cu_TNameEntryKindergarten.SendKeys("another Teacher");
            }

            return returnGuid.ToString();
        }
    }
}