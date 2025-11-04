using CSharpSeleniumNUnitAutomationProject.PageObjects;
using CSharpSeleniumNUnitAutomationProject.Utilities;

namespace CSharpSeleniumNUnitAutomationProject.Tests
{
    public class Tests:Base
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LogInTest()
        {
            LogInPage logInPage = new LogInPage(driver.Value!);
            logInPage.SendDataToUserNameField("Admin");
            logInPage.SendDataToPasswordField("admin123");
            logInPage.ClickLogIn();
            Assert.IsTrue(driver.Value!.Url.Contains("dashboard"));            

            Thread.Sleep(5000);
        }

        [Test]
        public void LogInWithEmptyUserNameTest()
        {
            LogInPage logInPage = new LogInPage(driver.Value!);
            logInPage.SendDataToUserNameField("");
            logInPage.SendDataToPasswordField("admin123");
            logInPage.ClickLogIn();
            Assert.That(logInPage.UsernameRequiredFieldMsg.Text, Is.EqualTo("Required"));            
        }
    }
}