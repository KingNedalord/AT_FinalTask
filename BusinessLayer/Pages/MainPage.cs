using OpenQA.Selenium;

namespace BusinessLayer.Pages
{
    public class InventoryPage
    {
        private readonly IWebDriver _driver;
        public InventoryPage(IWebDriver driver) => _driver = driver;

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
