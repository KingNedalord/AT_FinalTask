using OpenQA.Selenium;

namespace BusinessLayer.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private const string BaseUrl = "https://www.saucedemo.com/";

        // this approach 54 seconds
        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }
        // this approach 39 seconds
        // public LoginPage(string browser = "Chrome")
        // {
        //     DriverManager.Instance.CreateDriver(browser);
        //     _driver = DriverManager.Instance.Driver;
        // }

        // XPath locators
        private By UsernameInput => By.XPath("//input[@id='user-name']");
        private By PasswordInput => By.XPath("//input[@id='password']");
        private By LoginButton => By.XPath("//input[@id='login-button']");
        private By ErrorMessageContainer => By.XPath("//h3[@data-test='error']");

        public void GoTo() => _driver.Navigate().GoToUrl(BaseUrl);

        public void EnterUsername(string username)
        {
            var el = _driver.FindElement(UsernameInput);
            el.Clear();
            el.SendKeys(username);
            // Thread.Sleep(300);
        }

        public void EnterPassword(string password)
        {
            var el = _driver.FindElement(PasswordInput);
            el.Clear();
            el.SendKeys(password);
            // Thread.Sleep(300);
        }

        public void ClearUsername()
        {
            var usernameField = _driver.FindElement(UsernameInput);

            usernameField.SendKeys(Keys.Control + "a");
            usernameField.SendKeys(Keys.Delete);
            usernameField.Clear();
        }

        public void ClearPassword()
        {
            var passwordField = _driver.FindElement(PasswordInput);

            passwordField.SendKeys(Keys.Control + "a");
            passwordField.SendKeys(Keys.Delete);
            passwordField.Clear();
        }

        public void ClickLogin() => _driver.FindElement(LoginButton).Click();

        public string GetErrorMessage()
        {
            try
            {
                return _driver.FindElement(ErrorMessageContainer).Text.Trim();
            }
            catch (NoSuchElementException)
            {
                return string.Empty;
            }
        }
    }
}
