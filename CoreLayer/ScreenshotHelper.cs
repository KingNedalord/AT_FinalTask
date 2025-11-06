using System;
using System.IO;
using OpenQA.Selenium;

namespace CoreLayer
{
    public static class ScreenshotHelper
    {
        public static string SaveScreenshot(IWebDriver driver, string testName)
        {
            try
            {
                if (driver == null) return string.Empty;
                var screenshotDriver = driver as ITakesScreenshot;
                if (screenshotDriver == null) return string.Empty;

                // Place screenshots in ./screenshots relative to test run folder(bin/debug/...)
                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screenshots");
                Directory.CreateDirectory(dir);
                var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmssfff");
                var fileName = $"{SanitizeFileName(testName)}_{timestamp}.png";
                var path = Path.Combine(dir, fileName);
                screenshotDriver.GetScreenshot().SaveAsFile(path);
                return path;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }
    }
}
