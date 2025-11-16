using OpenQA.Selenium;

namespace BusinessLayer.Pages
{
    public class InventoryPage
    {
        private readonly IWebDriver _driver;

        /// 54 seconds with this constructor
        public InventoryPage(IWebDriver driver) => _driver = driver;
        /// 39 seconds with this constructor
        // public InventoryPage(string browser = "Chrome")
        // {
        //     DriverManager.Instance.CreateDriver(browser);
        //     _driver = DriverManager.Instance.Driver;
        // }

        public string GetPageTitle()
        {
            try
            {
                var elem = _driver.FindElement(By.XPath("//span[contains(text(),'Swag Labs') ]"));
                return elem.Text.Trim();
            }
            catch
            {
                return _driver.Title;
            }
        }
    }
}
