using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using CSharpSeleniumNUnitAutomationProject.TestData;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using WebDriverManager.DriverConfigs.Impl;

namespace CSharpSeleniumNUnitAutomationProject.Utilities
{
    public class Base
    {
        public ThreadLocal<IWebDriver> driver = new();
        protected WebDriverWait? wait;
        public ExtentReports extent;
        protected ThreadLocal<ExtentTest> test = new();
        private BrowserType _browser = BrowserType.Chrome;
        public string excelPath;
        public List<Dictionary<string, string>> testData;

        [SetUp]
        public void BeforeTest()
        {
            // Start the ExtentTest for the current test
            test.Value = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            string baseDirectory = TestContext.CurrentContext.TestDirectory;
            excelPath = Path.Combine(baseDirectory, "TestData", "TestData.xlsx");
            testData = ExcelReader.ReadExcel(excelPath, "LoginData");
            
            InitBrowser(_browser);

            if (driver.Value != null)
            {
                driver.Value.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                driver.Value.Manage().Window.Maximize();
                driver.Value.Navigate().GoToUrl(ConfigurationManager.BaseUrl);

                // Initialize WebDriverWait
                wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(10));
            }
        }

        private void InitBrowser(BrowserType browser)
        {
            switch (browser)
            {
                case BrowserType.Firefox:
                    new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());                    
                    driver.Value = new FirefoxDriver();
                    break;
                case BrowserType.Chrome:
                    var chromeOptions = new ChromeOptions();
                    var chromeDriverService = ChromeDriverService.CreateDefaultService();
                    driver.Value = new ChromeDriver(chromeDriverService, chromeOptions, TimeSpan.FromMinutes(3));
                    break;
                case BrowserType.Edge:
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
            
            var htmlReporter = new ExtentSparkReporter(reportPath);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
            extent.AddSystemInfo("Host Name", "localhost");
            extent.AddSystemInfo("Environment", "QA");
            extent.AddSystemInfo("User", "Tester");
            htmlReporter.Config.DocumentTitle = "Automation Test Report";
            htmlReporter.Config.ReportName = "OrangeHRM " + TestContext.CurrentContext.Test.Name + " Report";
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
            var message = TestContext.CurrentContext.Result.Message;

            Status logstatus = status switch
            {
                TestStatus.Failed => Status.Fail,
                TestStatus.Passed => Status.Pass,
                TestStatus.Skipped => Status.Skip,
                TestStatus.Inconclusive => Status.Info,
                TestStatus.Warning => Status.Warning,
                _ => Status.Info
            };

            if (status is TestStatus.Passed or TestStatus.Failed)
            {
                string screenshot = ScreenshotHelper.CaptureScreenshot(driver.Value, test.Value.Test.FullName, logstatus);
                test.Value.Log(logstatus, status.ToString());
                test.Value.AddScreenCaptureFromPath(screenshot);
            }
            
            if (driver.Value != null)
            {
                driver.Value.Quit();
                driver.Value.Dispose();
                driver.Value = null!;
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
