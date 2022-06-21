using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace ContactBook.WebDriverTests
{
    public class LoginTests 
    {
        private const string url = "https://www.saucedemo.com/";
        private IWebDriver driver;

        [SetUp]
        public void OpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_DirectAccessToProductsPageWitoutLogin()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/inventory.html");

            var errorMessageNoHaveAccess = driver.FindElement(By.XPath("//h3[@data-test='error']")).Text;
            Assert.AreEqual(errorMessageNoHaveAccess, "Epic sadface: You can only access '/inventory.html' when you are logged in.");

        }

        [Test]
        public void Test_StandartUserValidCredentials()
        {
            driver.Navigate().GoToUrl(url);

            var username = driver.FindElement(By.XPath("//input[@id='user-name']"));
            username.Clear();
            username.SendKeys("standard_user");

            var password = driver.FindElement(By.XPath("//input[@id='password']"));            
            password.Clear();
            password.SendKeys("secret_sauce");

            var loginBtn = driver.FindElement(By.XPath("//input[@id='login-button']"));
            loginBtn.Click();

            var headerTitleProducts = driver.FindElement(By.XPath("//span[@class='title']")).Text;

            Assert.AreEqual(headerTitleProducts, "PRODUCTS");

        }

        [Test]
        public void Test_StandartUserInvalidCredentials()
        {
            driver.Navigate().GoToUrl(url);

            var username = driver.FindElement(By.XPath("//input[@id='user-name']"));
            username.Clear();
            username.SendKeys("standard_user");

            var password = driver.FindElement(By.XPath("//input[@id='password']"));
            password.Clear();
            password.SendKeys("wrongpassword");

            var loginBtn = driver.FindElement(By.XPath("//input[@id='login-button']"));
            loginBtn.Click();

            var errorMessageInvalidCredentials = driver.FindElement(By.XPath("//h3[@data-test='error']")).Text;
            Assert.AreEqual(errorMessageInvalidCredentials, "Epic sadface: Username and password do not match any user in this service");
            
        }

        [Test]
        public void Test_LockedUserValidCredentials()
        {
            driver.Navigate().GoToUrl(url);

            var username = driver.FindElement(By.XPath("//input[@id='user-name']"));
            username.Clear();
            username.SendKeys("locked_out_user");

            var password = driver.FindElement(By.XPath("//input[@id='password']"));
            password.Clear();
            password.SendKeys("secret_sauce");

            var loginBtn = driver.FindElement(By.XPath("//input[@id='login-button']"));
            loginBtn.Click();

            var errorMessageInvalidCredentials = driver.FindElement(By.XPath("//h3[@data-test='error']")).Text;
            Assert.AreEqual(errorMessageInvalidCredentials, "Epic sadface: Sorry, this user has been locked out.");

        }

    }
}