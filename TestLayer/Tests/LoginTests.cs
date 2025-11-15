using BusinessLayer.Pages;
using CoreLayer;
using OpenQA.Selenium;
using TestLayer.TestData;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
namespace TestLayer.Tests
{
    public class LoginTests : Fixtures.TestBase
    {
        public LoginTests(ITestOutputHelper output) : base(output, "firefox") // if we don't specify chrome by default
        {
            // DriverManager.Instance.CreateDriver("Chrome");
        }

        private static IWebDriver Driver => DriverManager.Instance.Driver;

        [Fact(DisplayName = "UC-1: Login with empty credentials shows Username is required")]
        public void UC1_Login_EmptyCredentials_ShowsUsernameRequired()
        {
            ExecuteTest(() =>
            {
                var login = new LoginPage(Driver);
                login.GoTo();

                login.EnterUsername("foo");
                login.EnterPassword("bar");
                login.ClearUsername();
                login.ClearPassword();

                login.ClickLogin();
                // Thread.Sleep(5000);
                var err = login.GetErrorMessage();

                // Assert.Contains("", err);
                //Your deadline is November 17th. You must submit your task, wait for 
                // the review, and schedule a defense session (during which you will present and
                //  discuss your completed task with your reviewer). AFTER THAT, if your defense is
                //  successful, we will schedule a general and technical pre-lab interview for you.
                err.Should().Contain("Epic sadface: Username is required");
            }, nameof(UC1_Login_EmptyCredentials_ShowsUsernameRequired));
        }

        [Fact(DisplayName = "UC-2: Login missing password shows Password is required")]
        public void UC2_Login_MissingPassword_ShowsPasswordRequired()
        {
            ExecuteTest(() =>
            {
                var login = new LoginPage(Driver);
                login.GoTo();

                login.EnterUsername("some_user");
                login.EnterPassword("some_pass");
                login.ClearPassword();

                login.ClickLogin();

                var err = login.GetErrorMessage();
                // Assert.Contains("", err);
                err.Should().Contain("Epic sadface: Password is required");

            }, nameof(UC2_Login_MissingPassword_ShowsPasswordRequired));
        }

        [Theory(DisplayName = "UC-3: Login accepted users succeed")]
        [MemberData(nameof(AcceptedUsersData.GetAcceptedUsers), MemberType = typeof(AcceptedUsersData))]
        public void UC3_Login_AcceptedUsers_Success(string username)
        {
            ExecuteTest(() =>
            {
                var login = new LoginPage(Driver);
                login.GoTo();

                login.EnterUsername(username);
                login.EnterPassword("secret_sauce");
                login.ClickLogin();

                var inventory = new InventoryPage(Driver);
                var title = inventory.GetPageTitle();

                // Assert.Contains("", title);
                title.Should().Contain("Swag Labs");

            }, $"UC3_Login_AcceptedUsers_Success_{username}");
        }
    }
}
