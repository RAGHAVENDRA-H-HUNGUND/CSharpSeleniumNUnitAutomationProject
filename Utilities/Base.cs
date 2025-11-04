using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Configuration;
using WebDriverManager.DriverConfigs.Impl;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;

namespace CSharpSeleniumNUnitAutomationProject.Utilities
{
    public class Base
    {
        public ThreadLocal<IWebDriver> driver = new();
        protected WebDriverWait? wait;
        public ExtentReports extent;
        protected ExtentTest test;

        [SetUp]
        public void StartBrowser()
        {
            //string? browserName = ConfigurationManager.AppSettings["browser"];
            //string? baseUrl = ConfigurationManager.AppSettings["baseUrl"];

            //if (string.IsNullOrEmpty(browserName))
            //{
            //    throw new ArgumentNullException("Browser name is not specified in App.config");
            //}
            // Start the ExtentTest for the current test
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            
            string browserName = TestContext.Parameters["browserName"]!;
            if (string.IsNullOrEmpty(browserName))
            {
                //browserName = ConfigurationManager.AppSettings["browser"];
                browserName = "Chrome";
            }

            string baseUrl = /*ConfigurationManager.AppSettings["baseUrl"];*/"https://opensource-demo.orangehrmlive.com/web/index.php/auth/login";

            if (string.IsNullOrEmpty(browserName))
            {
                throw new ArgumentNullException("Browser name is not specified in App.config");
            }

            InitBrowser(browserName);

            if (driver.Value != null)
            {
                driver.Value.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                driver.Value.Manage().Window.Maximize();
                driver.Value.Navigate().GoToUrl(baseUrl!);

                // Initialize WebDriverWait
                wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(10));
            }
        }

        private IWebDriver GetDriver() => driver.Value!;

        private void InitBrowser(string browserName)
        {
            switch (browserName)
            {
                case "Firefox":
                    new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());                    
                    driver.Value = new FirefoxDriver();
                    break;
                case "Chrome":
                    var chromeOptions = new ChromeOptions();
                    var chromeDriverService = ChromeDriverService.CreateDefaultService();
                    driver.Value = new ChromeDriver(chromeDriverService, chromeOptions, TimeSpan.FromMinutes(3));
                    break;
                case "Edge":
                    var edgeOptions = new EdgeOptions();
                    var edgeDriverService = EdgeDriverService.CreateDefaultService();
                    driver.Value = new EdgeDriver(edgeDriverService, edgeOptions, TimeSpan.FromMinutes(3));
                    break;
                default:
                    throw new ArgumentException("Browser name is not recognized.");
            }

            if (driver.Value == null)
            {
                throw new InvalidOperationException("WebDriver initialization failed.");
            }
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            // Set up ExtentReports
            String workingDirectory = Environment.CurrentDirectory;

            string projectDirectory = Directory.GetParent(workingDirectory)!.Parent!.Parent!.FullName;
            string reportPath = projectDirectory + "//index.html";
            //Path.Combine(projectDirectory, "index.html");

            //var htmlReporter = new ExtentHtmlReporter(reportPath);
            var htmlReporter = new ExtentSparkReporter(reportPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent.AddSystemInfo("Host Name", "localhost");
            extent.AddSystemInfo("Environment", "QA");
            htmlReporter.Config.DocumentTitle = "Automation Test Report";
            htmlReporter.Config.ReportName = "OrangeHRM LogIN Page Test Report";
            htmlReporter.Config.Theme = Theme.Dark;
        }

        [TearDown]
        public void AfterTest()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var Stacktrace = TestContext.CurrentContext.Result.StackTrace;
            DateTime time = DateTime.Now;
            String fileName = "screenshot_" + time.ToString("h_mm_ss") + ".png";
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                ? ""
                : string.Format("{0}", TestContext.CurrentContext.Result.StackTrace);
            var errorMessage = TestContext.CurrentContext.Result.Message;

            switch (status)
            {
                case TestStatus.Failed:
                    // Capture screenshot on failure
                    test.Fail("Test Failed", CaptureScreenshot(driver.Value!, fileName))
                        .Log(Status.Fail, errorMessage)
                        .Log(Status.Fail, stacktrace);

                    break;
                case TestStatus.Skipped:
                    test.Skip("Test Skipped").Log(Status.Skip, errorMessage);
                    break;
                case TestStatus.Passed:
                    test.Pass("Test Passed", CaptureScreenshot(driver.Value!, fileName));
                    break;
                default:
                    test.Info("Test Finished with no specific status.");
                    break;
            }
            if (driver.Value != null)
            {
                driver.Value.Quit();
                driver.Value.Dispose();
                driver.Value = null!;
            }
        }
       
        public Media CaptureScreenshot(IWebDriver driver, String screenshotName)
        {
            try
            {
                ITakesScreenshot ts = (ITakesScreenshot)driver;
                var screenshot = ts.GetScreenshot().AsBase64EncodedString;
                return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenshotName).Build();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error capturing screenshot: " + ex.Message);
                return null;
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            // Flush the report
            extent.Flush();
        }
    }
}
