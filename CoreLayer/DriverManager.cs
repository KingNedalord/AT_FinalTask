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
    /// </summary>
    public sealed class DriverManager
    {
        private static readonly Lazy<DriverManager> lazy = new Lazy<DriverManager>(() => new DriverManager());
        public static DriverManager Instance => lazy.Value;

        // One driver per thread
        private readonly ThreadLocal<IWebDriver> threadDriver = new ThreadLocal<IWebDriver>();

        private DriverManager() { }

        public IWebDriver Driver => threadDriver.Value ?? throw new InvalidOperationException(
            "WebDriver not initialized. Call DriverManager.Instance.CreateDriver(browser) before accessing Driver.");

        public void CreateDriver(string browser)
        {
            if (threadDriver.IsValueCreated && threadDriver.Value != null)
                return;

            // var selected = (browser ?? Environment.GetEnvironmentVariable("BROWSER") ?? "Chrome").ToLowerInvariant();

            var selected = browser.ToLowerInvariant();
            IWebDriver driver;
            if (selected.Contains("firefox"))
            {
                driver = new FirefoxDriver();
            }
            else
            {
                driver = new ChromeDriver();
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            threadDriver.Value = driver;
            // maximize windows to reduce locator issues
            driver.Manage().Window.Maximize();
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
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    //The ! after null - tells the compiler "I know this looks like it could be null, but trust me, it's safe"
                    threadDriver.Value = null!;
                }
            }
        }
    }
}
