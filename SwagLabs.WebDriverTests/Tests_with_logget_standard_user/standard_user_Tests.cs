using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace ContactBook.WebDriverTests
{
    public class SandartUserTests
    {
        private const string url = "https://www.saucedemo.com/";
        private IWebDriver driver;

        [SetUp]
        public void OpenBrowser_and_Loggin()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            driver.Navigate().GoToUrl(url);

            var username = driver.FindElement(By.XPath("//input[@id='user-name']"));
            username.Clear();
            username.SendKeys("standard_user");

            var password = driver.FindElement(By.XPath("//input[@id='password']"));
            password.Clear();
            password.SendKeys("secret_sauce");

            var loginBtn = driver.FindElement(By.XPath("//input[@id='login-button']"));
            loginBtn.Click();
        }

        [TearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_AddToCart_and_DeleteItem()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/inventory.html");

            driver.FindElement(By.CssSelector("*[data-test=\"add-to-cart-sauce-labs-backpack\"]")).Click();
            driver.FindElement(By.CssSelector(".shopping_cart_badge")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"remove-sauce-labs-backpack\"]")).Click();

            var cartItems = driver.FindElements(By.CssSelector("*[class=\"cart_item\"]"));

            Assert.That(cartItems.Count, Is.EqualTo(0));
        }

        [Test]
        public void Test_AddToCart_and_Checkout()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/inventory.html");

            driver.FindElement(By.CssSelector("*[data-test=\"add-to-cart-sauce-labs-backpack\"]")).Click();
            driver.FindElement(By.CssSelector(".shopping_cart_badge")).Click();

            driver.FindElement(By.CssSelector("*[data-test=\"checkout\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"firstName\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"firstName\"]")).SendKeys("Konstantin");
            driver.FindElement(By.CssSelector("*[data-test=\"lastName\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"lastName\"]")).SendKeys("Conkov");
            driver.FindElement(By.CssSelector("*[data-test=\"postalCode\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"postalCode\"]")).SendKeys("9000");
            driver.FindElement(By.CssSelector("*[data-test=\"continue\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"finish\"]")).Click();

            var thankYouMEssage = driver.FindElement(By.XPath("//h2[@class='complete-header']")).Text;

            Assert.That(thankYouMEssage, Is.EqualTo("THANK YOU FOR YOUR ORDER"));
        }

        [Test]
        public void Test_AddToCart_and_Checkout_EmptyPersonalDetails()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/inventory.html");

            driver.FindElement(By.CssSelector("*[data-test=\"add-to-cart-sauce-labs-backpack\"]")).Click();
            driver.FindElement(By.CssSelector(".shopping_cart_badge")).Click();

            driver.FindElement(By.CssSelector("*[data-test=\"checkout\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"firstName\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"firstName\"]")).SendKeys("");
            driver.FindElement(By.CssSelector("*[data-test=\"lastName\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"lastName\"]")).SendKeys("Conkov");
            driver.FindElement(By.CssSelector("*[data-test=\"postalCode\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"postalCode\"]")).SendKeys("9000");
            driver.FindElement(By.CssSelector("*[data-test=\"continue\"]")).Click();

            var errorMessage = driver.FindElement(By.XPath("//h3[@data-test='error']")).Text;

            Assert.That(errorMessage, Is.EqualTo("Error: First Name is required"));
        }

        [Test]
        public void Test_Checkout_EmptyCart()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/inventory.html");

            driver.FindElement(By.CssSelector(".shopping_cart_link")).Click();

            driver.FindElement(By.CssSelector("*[data-test=\"checkout\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"firstName\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"firstName\"]")).SendKeys("Konstantin");
            driver.FindElement(By.CssSelector("*[data-test=\"lastName\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"lastName\"]")).SendKeys("Conkov");
            driver.FindElement(By.CssSelector("*[data-test=\"postalCode\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"postalCode\"]")).SendKeys("9000");
            driver.FindElement(By.CssSelector("*[data-test=\"continue\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"finish\"]")).Click();

            var Message = driver.FindElement(By.CssSelector(".complete-header")).Text;

            Assert.That(Message, Is.EqualTo("Your order is not completed, please first choose some product."));
        }


        [Test]
        public void Test_AddTwoItemsInCart()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/inventory.html");

            driver.FindElement(By.CssSelector("*[data-test=\"add-to-cart-sauce-labs-backpack\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"add-to-cart-sauce-labs-onesie\"]")).Click();

            var cartNum = driver.FindElement(By.CssSelector(".shopping_cart_badge")).Text;

            Assert.That(cartNum, Is.EqualTo("2"));
        }

        [Test]
        public void Test_DeleteItemsFromCart()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/inventory.html");

            driver.FindElement(By.CssSelector("*[data-test=\"add-to-cart-sauce-labs-backpack\"]")).Click();
            driver.FindElement(By.CssSelector("*[data-test=\"add-to-cart-sauce-labs-onesie\"]")).Click();
            
            driver.FindElement(By.CssSelector(".shopping_cart_link")).Click();

            driver.FindElement(By.CssSelector("*[data-test=\"remove-sauce-labs-backpack\"]")).Click();

            var cartNum = driver.FindElement(By.CssSelector(".shopping_cart_badge")).Text;

            Assert.That(cartNum, Is.EqualTo("1"));
        }

        [Test]
        public void Test_Sort_AtoZ()
        {
            driver.Navigate().GoToUrl("https://www.saucedemo.com/inventory.html");
            driver.FindElement(By.CssSelector(".product_sort_container > option:nth-child(1)")).Click();

            var firstProductTitle = driver.FindElement(By.CssSelector("div[class='inventory_item_name']")).Text;

            Assert.That(firstProductTitle, Is.EqualTo("Sauce Labs Backpack"));
        }

    }
}