using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSeleniumNUnitAutomationProject.PageObjects
{
    public class HomePage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        // Define elements using direct initialization
        private readonly IWebElement livingRoomsNavLink;
        private readonly IWebElement diningRoomsNavLink;
        private readonly IWebElement salesNavLink;
        private readonly IWebElement shopMattressSaleImage;
        private readonly IWebElement swiperWrapper;

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Initialize elements directly
            livingRoomsNavLink = driver.FindElement(By.CssSelector("a[title='Living Rooms']"));
            diningRoomsNavLink = driver.FindElement(By.CssSelector("a[title='Dining Rooms']"));
            salesNavLink = driver.FindElement(By.CssSelector("a[title='Sales']"));
            shopMattressSaleImage = driver.FindElement(By.CssSelector("img[alt='Shop Mattress Sale']"));
            swiperWrapper = driver.FindElement(By.CssSelector(".swiper-wrapper"));
        }

        public void ClickLivingRooms()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(livingRoomsNavLink));
            livingRoomsNavLink.Click();
        }

        public void ClickDiningRooms()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(diningRoomsNavLink));
            diningRoomsNavLink.Click();
        }

        public void ClickSales()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(salesNavLink));
            salesNavLink.Click();
        }

        public void ScrollToShopMattressSale()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", shopMattressSaleImage);
        }

        public void ClickShopKidsSale()
        {
            ScrollToShopMattressSale();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(".swiper-wrapper")));
            var swiperItems = swiperWrapper.FindElements(By.CssSelector(".swiper-slide"));

            foreach (var item in swiperItems)
            {
                var titleElement = item.FindElement(By.CssSelector(".css-7g1can"));
                string titleText = titleElement.Text;

                if (titleText == "Shop Kids Sale")
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    js.ExecuteScript("arguments[0].scrollIntoView(true);", item);
                    item.Click();
                    break;
                }
            }
        }
    }
}
