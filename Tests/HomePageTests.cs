using CSharpSeleniumNUnitAutomationProject.PageObjects;
using CSharpSeleniumNUnitAutomationProject.Utilities;

namespace CSharpSeleniumNUnitAutomationProject.Tests
{
    public class HomePageTests:Base
    {
        [Test]
        public void VerifyHomePageTitleTest()
        {
            HomePage hp = new HomePage(driver.Value!);
            hp.GetPageTitle();

        }

        [Test]
        public void LogOutTest()
        {
            HomePage hp = new HomePage(driver.Value!);
            LogInPage lp = hp.Logout();
            Assert.That(driver.Value.Url.Contains("login"), Is.True);
        }
    }
}
