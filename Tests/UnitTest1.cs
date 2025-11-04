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
        public void Test1()
        {
            LogInPage logInPage = new LogInPage(driver.Value);
            logInPage.SendDataToUserNameField("Admin");
            logInPage.SendDataToPasswordField("admin1234");
            logInPage.ClickLogIn();
            Assert.IsTrue(driver.Value.Url.Contains("dashboard"));            

            Thread.Sleep(5000);
        }

        [Test]
        public void LogInWithEmptyUserNameTest()
        {
            LogInPage logInPage = new LogInPage(driver.Value);
            logInPage.SendDataToUserNameField("");
            logInPage.SendDataToPasswordField("admin123");
            logInPage.ClickLogIn();
            Assert.That(logInPage.UsernameRequiredFieldMsg.Text, Is.EqualTo("Required"));
            //pageLogin.AssertionWithActualExpectedResult(pageLogin.UsernameRequiredFieldMsg.Text, expectedResult);
        }
    }
}