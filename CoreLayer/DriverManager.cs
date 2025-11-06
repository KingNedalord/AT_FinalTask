using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace CoreLayer
{
    /// <summary>
    /// Thread-safe singleton driver manager.
    /// Each test thread gets its own IWebDriver instance stored in a ThreadLocal.
    /// Initialize the driver with DriverManager.Instance.CreateDriver(); 
    /// Quit with DriverManager.Instance.QuitDriver();
    /// Default browser is Chrome. Headless only when HEADLESS=true.
    /// </summary>
    public sealed class DriverManager
    {
        private static readonly Lazy<DriverManager> lazy = new Lazy<DriverManager>(() => new DriverManager());
        public static DriverManager Instance => lazy.Value;

        // One driver per thread
        private readonly ThreadLocal<IWebDriver> threadDriver = new ThreadLocal<IWebDriver>();

        private DriverManager() { }

        public IWebDriver Driver => threadDriver.IsValueCreated ? threadDriver.Value : null;

        public void CreateDriver(string browser = null)
        {
            if (threadDriver.IsValueCreated && threadDriver.Value != null)
                return; // already created for this thread

            var selected = (browser ?? Environment.GetEnvironmentVariable("BROWSER") ?? "Chrome").ToLowerInvariant();
            var headless = (Environment.GetEnvironmentVariable("HEADLESS") ?? "false").ToLowerInvariant() == "true";

            IWebDriver driver;
            if (selected.Contains("firefox"))
            {
                var options = new FirefoxOptions();
                if (headless)
                    options.AddArgument("-headless");
                driver = new FirefoxDriver(options);
            }
            else
            {
                var options = new ChromeOptions();
                if (headless)
                    options.AddArgument("--headless=new");
                // maximize windows to reduce locator issues
                options.AddArgument("--start-maximized");
                driver = new ChromeDriver(options);
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            threadDriver.Value = driver;
        }

        public void QuitDriver()
        {
            if (threadDriver.IsValueCreated && threadDriver.Value != null)
            {
                try
                {
                    threadDriver.Value.Quit();
                    threadDriver.Value.Dispose();
                }
                catch { /* swallow exceptions during cleanup */ }
                finally
                {
                    threadDriver.Value = null;
                }
            }
        }
    }
}
