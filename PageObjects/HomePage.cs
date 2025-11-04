using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CSharpSeleniumNUnitAutomationProject.PageObjects
{
    public class HomePage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private IWebElement LogoutMenu =>
        new WebDriverWait(driver, TimeSpan.FromSeconds(10))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By
            .XPath("//img[@alt='profile picture']/following-sibling::i")));

        private IWebElement LogoutOption =>
        new WebDriverWait(driver, TimeSpan.FromSeconds(10))
            .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By
            .XPath("//*[@id='app']/div[1]/div[1]/header/div[1]/div[3]/ul/li/ul/li[4]/a")));

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
            LogInPage lp = new LogInPage(driver);
            lp.SendDataToUserNameField("Admin");
            lp.SendDataToPasswordField("admin123");
            lp.ClickLogIn();           
           
        }

        public void GetPageTitle()
        {
            bool isTrue = driver.Url.Contains("dashboard");
            Assert.IsTrue(isTrue);
        }

        public LogInPage Logout()
        {
            LogoutMenu?.Click();
            LogoutOption?.Click();
            return new LogInPage(driver);
        }
    }
}
