using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace CSharpSeleniumNUnitAutomationProject.Utilities
{
    internal class ScreenshotHelper
    {
        private const string ReportsDir = "Reports";
        private const string ScreenshotsDir = "Screenshots";
        private const string PassedDir = "Passed";
        private const string FailedDir = "Failed";
        private const string OtherDir = "Other";

        public static string CaptureScreenshot(IWebDriver driver, string testName, Status status)
        {
            var ts = (ITakesScreenshot)driver;
            var screenshot = ts.GetScreenshot();

            string timestamp = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
            string fileName = $"Screenshot_{testName}_{timestamp}.png";
            string basePath = PathUtility.GetBasePath();

            string statusFolder = status == Status.Fail ? FailedDir : PassedDir ;
            
            string screenshotFolder = Path.Combine(basePath, ReportsDir, ScreenshotsDir, statusFolder);

            Directory.CreateDirectory(screenshotFolder);
            DeleteOldScreenshots(screenshotFolder);

            string filePath = Path.Combine(screenshotFolder, fileName);
            screenshot.SaveAsFile(filePath);
            //LogHelper.Info($"Screenshot saved: {filePath}");
            return filePath;
        }
        private static void DeleteOldScreenshots(string folder)
        {
            if (!Directory.Exists(folder)) return;

            foreach (var file in Directory.GetFiles(folder))
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.LastWriteTime < DateTime.Now.AddDays(-1))
                        File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting {file}: {ex.Message}");
                }
            }
        }
    }
}
