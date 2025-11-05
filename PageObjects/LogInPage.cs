using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CSharpSeleniumNUnitAutomationProject.PageObjects
{
    public class LogInPage
    {
        
        private IWebDriver _driver;
        private readonly WebDriverWait wait;

        private IWebElement Username =>
        new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name("username")));

        private IWebElement Password =>
        new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name("password")));

        private IWebElement Loginbtn =>
        new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//button[@type='submit']")));

        public IWebElement InvalidCredential =>
        new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[text()='Invalid credentials']")));

        public IWebElement UsernameRequiredFieldMsg =>
        new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@name='username']/following::span")));

        public IWebElement PasswordRequiredFieldMsg =>
        new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
        .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@name='password']/following::span")));

        public LogInPage(IWebDriver driver)
        {
            _driver = driver;
            this.wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Initialize elements directly
            //userName = driver.FindElement(By.Name("username"));
            //password = driver.FindElement(By.Name("password"));
            //loginBtn = driver.FindElement(By.XPath("//button[@type='submit']"));
        }

        public void SendDataToUserNameField(string text)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(Username));
            Username.Clear();
            Username.Click();
            Username.SendKeys(text);
        }

        public void SendDataToPasswordField(string pwd)
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(Password));
            Password.Clear();
            Password.Click();
            Password.SendKeys(pwd);
        }

        public void ClickLogIn()
        {
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(Loginbtn));
            Loginbtn.Click();
        }
    }
}
