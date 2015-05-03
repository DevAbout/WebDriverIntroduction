using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace WebDriverIntroduction
{
    [TestClass]
    public class WikipediaSearchTest
    {
        private const string ExistingPage = "Christiaan Barnard";
        private const string NonExistingPage = "Abcxyz";
        private IWebDriver _driver;

        [TestInitialize]
        public void TestInit()
        {
            _driver = new FirefoxDriver();
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            _driver.Navigate().GoToUrl("http://en.wikipedia.org/wiki/Main_Page");
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void Search_FindsResult_ResultPageIsShown()
        {
            SearchFor(ExistingPage);
            Assert.AreEqual(ExistingPage, GetFirstHeading());
        }
        
        [TestMethod]
        public void Search_FindsNoResult_ShowNoResultsMessage()
        {
            SearchFor(NonExistingPage);
            var expected = String.Concat("The page \"", NonExistingPage, "\" does not exist.");
            Assert.IsTrue(GetCreateLinkMessage().Contains(expected));
        }

        private string GetCreateLinkMessage()
        {
            return _driver.FindElement(By.ClassName("mw-search-createlink")).Text;
        }

        private string GetFirstHeading()
        {
            return _driver.FindElement(By.Id("firstHeading")).Text;
        }

        private void SearchFor(string searchTerm)
        {
            var searchInput = _driver.FindElement(By.Id("searchInput"));
            searchInput.SendKeys(searchTerm);
            searchInput.SendKeys(Keys.Enter);
        }
    }
}
