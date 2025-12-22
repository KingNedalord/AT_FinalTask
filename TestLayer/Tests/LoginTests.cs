using BusinessLayer.Pages;
using CoreLayer;
using TestLayer.TestData;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
namespace TestLayer.Tests
{
    public class LoginTests : Fixtures.TestBase
    {
        public LoginTests(ITestOutputHelper output) : base(output, "firefox")
        {
        }

        [Fact(DisplayName = "UC-1: Login with empty credentials shows Username is required")]
        public void UC1_Login_EmptyCredentials_ShowsUsernameRequired()
        {
            ExecuteTest(() =>
            {
                var login = new LoginPage(DriverManager.Instance.Driver);
                login.GoTo();

                login.EnterUsername("foo");
                login.EnterPassword("bar");
                login.ClearUsername();
                login.ClearPassword();

                login.ClickLogin();
                var err = login.GetErrorMessage();
                err.Should().Contain("Epic sadface: Username is required");
            }, nameof(UC1_Login_EmptyCredentials_ShowsUsernameRequired));
        }

        [Fact(DisplayName = "UC-2: Login missing password shows Password is required")]
        public void UC2_Login_MissingPassword_ShowsPasswordRequired()
        {
            ExecuteTest(() =>
            {
                var login = new LoginPage(DriverManager.Instance.Driver);
                login.GoTo();

                login.EnterUsername("some_user");
                login.EnterPassword("some_pass");
                login.ClearPassword();

                login.ClickLogin();

                var err = login.GetErrorMessage();
                err.Should().Contain("Epic sadface: Password is required");

            }, nameof(UC2_Login_MissingPassword_ShowsPasswordRequired));
        }

        [Theory(DisplayName = "UC-3: Login accepted users succeed")]
        [MemberData(nameof(AcceptedUsersData.GetAcceptedUsers), MemberType = typeof(AcceptedUsersData))]
        public void UC3_Login_AcceptedUsers_Success(string username)
        {
            ExecuteTest(() =>
            {
                var login = new LoginPage(DriverManager.Instance.Driver);
                login.GoTo();

                login.EnterUsername(username);
                login.EnterPassword("secret_sauce");
                login.ClickLogin();


                var inventory = new InventoryPage(DriverManager.Instance.Driver);
                var title = inventory.GetPageTitle();

                title.Should().Contain("Swag Labs");

            }, $"UC3_Login_AcceptedUsers_Success_{username}");
        }
    }
}
