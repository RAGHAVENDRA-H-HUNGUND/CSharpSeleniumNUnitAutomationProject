using CSharpSeleniumNUnitAutomationProject.PageObjects;
using CSharpSeleniumNUnitAutomationProject.TestData;
using CSharpSeleniumNUnitAutomationProject.Utilities;

namespace CSharpSeleniumNUnitAutomationProject.Tests
{
    [TestFixture(BrowserType.Chrome)]
    [TestFixture(BrowserType.Firefox)]
    [TestFixture(BrowserType.Edge)]
    [Parallelizable(ParallelScope.Self)]
    public class LogInPageTests:Base
    {
        private readonly BrowserType _browser;
        public LogInPageTests(BrowserType browser)
        {
            _browser = browser;
        }

        [Test, Category("Smoke"), Category("Regression")]
        public void LogInTest()
        {
            LogInPage logInPage = new LogInPage(driver.Value!);
            logInPage.SendDataToUserNameField(testData[0]["UserName"]);
            logInPage.SendDataToPasswordField(testData[0]["Password"]);
            logInPage.ClickLogIn();
            Assert.IsTrue(driver.Value!.Url.Contains("dashboard"));            

            Thread.Sleep(5000);
        }

        [Test, Category("Sanity")]
        public void LogInWithEmptyUserNameTest()
        {
            LogInPage logInPage = new LogInPage(driver.Value!);
            logInPage.SendDataToUserNameField(testData[2]["UserName"]);
            logInPage.SendDataToPasswordField(testData[2]["Password"]);
            logInPage.ClickLogIn();
            Assert.That(logInPage.UsernameRequiredFieldMsg.Text, Is.EqualTo(testData[2]["Expected Result"]));            
        }

        [Test, Category("Sanity")]
        public void LogInWithEmptyPasswordTest()
        {
            LogInPage logInPage = new LogInPage(driver.Value!);
            logInPage.SendDataToUserNameField(testData[3]["UserName"]);
            logInPage.SendDataToPasswordField(testData[3]["Password"]);
            logInPage.ClickLogIn();
            Assert.That(logInPage.PasswordRequiredFieldMsg.Text, Is.EqualTo(testData[3]["Expected Result"]));

        }

        [Test, Category("Regression")]
        public void LogInWithEmptyFieldsTest()
        {
            LogInPage logInPage = new LogInPage(driver.Value!);
            logInPage.SendDataToUserNameField(testData[4]["UserName"]);
            logInPage.SendDataToPasswordField(testData[4]["Password"]);
            logInPage.ClickLogIn();

            Assert.Multiple(() =>
            {
                //Assert.That(pageLogin.UsernameRequiredFieldMsg.Text, Is.EqualTo(expectedResult));
                Assert.That(logInPage.UsernameRequiredFieldMsg.Text, Is.EqualTo(testData[4]["Expected Result"]));
                //Assert.That(pageLogin.PasswordRequiredFieldMsg.Text, Is.EqualTo(expectedResult));
                Assert.That(logInPage.PasswordRequiredFieldMsg.Text, Is.EqualTo(testData[4]["Expected Result"]));
            });

        }

        [Test, Category("Regression")]
        public void LogInWithWrongCredentialsTest()
        {
            LogInPage logInPage = new LogInPage(driver.Value!);
            logInPage.SendDataToUserNameField(testData[1]["UserName"]);
            logInPage.SendDataToPasswordField(testData[1]["Password"]);
            logInPage.ClickLogIn();
            Assert.That(logInPage.InvalidCredential.Text, Is.EqualTo(testData[1]["Expected Result"]));

        }
    }
}