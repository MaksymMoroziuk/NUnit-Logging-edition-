using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;

namespace NUnitHomeworkP1MMorozyouk
{
    [TestFixture]
    public class NUnitHomework
    {
        IWebDriver driver;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait=TimeSpan.FromSeconds(10);

            Log.Logger.Debug("OneTime Setup finished");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();

            Log.Logger.Debug("OneTime TearDown finished");
        }

        [SetUp]
        public void Login()
        {
            driver.Url = "https://atata-framework.github.io/atata-sample-app/#!/signin";
            driver.FindElement(By.Id("email")).SendKeys("admin@mail.com");
            driver.FindElement(By.Id("password")).SendKeys("abc123");
            driver.FindElement(By.CssSelector("input[value='Sign In']")).Click();

            Log.Logger.Debug("Logged in to Atata sample app");
        }


        [TearDown]
        public void Logout()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Log.Logger.Debug(TestContext.CurrentContext.Result.Message);
                Log.Logger.Debug(TestContext.CurrentContext.Result.StackTrace);
            }

            driver.FindElement(By.ClassName("navbar-right")).Click();
            driver.FindElement(By.LinkText("Sign Out")).Click();

            Log.Logger.Debug("Logged out Atata sample app");
        }

        [Test]
        public void VerifyPlansHeader()
        {
            Log.Logger.Information("Navigating to Plans page");
            driver.FindElement(By.LinkText("Plans")).Click();

            var pageHeader = driver.FindElement(By.TagName("h1"));

            Log.Logger.Information("Verifing page header");
            Assert.AreEqual("plans", pageHeader.Text);
        }

        [Test]
        public void VerifyThatCalculationsAreCorrect()
        {
            Log.Logger.Information("Navigating to Calculations page");
            driver.FindElement(By.LinkText("Calculations")).Click();

            Log.Logger.Information("Entering fist value");
            driver.FindElement(By.Id("addition-value-1")).SendKeys("2");

            Log.Logger.Information("Entering second value");
            driver.FindElement(By.Id("addition-value-2")).SendKeys("2");

            var additionResult = driver.FindElement(By.Id("addition-result"));

            Log.Logger.Information("Verifing the result");
            Assert.AreEqual("4", additionResult.GetAttribute("value"));
        }

        [Test]
        public void VerifyPricesContainDollarSign()
        {
            Log.Logger.Information("Navigating to Products page");
            driver.FindElement(By.LinkText("Products")).Click();
            var prices = driver.FindElements(By.XPath("//tr/td[2]"));

            foreach(var price in prices)
            {
                Log.Logger.Information("Verifi that price contains dollar sign");
                Assert.That(price.Text, Does.Contain("$"));
            }
        }
    }
}
