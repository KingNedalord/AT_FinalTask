```markdown
# AT Final task — README

Task: Automate login scenarios for https://www.saucedemo.com/ using Selenium WebDriver and xUnit.

Supported features (implemented)
- Browsers: Chrome and Firefox (select via environment variable BROWSER) — default: Chrome
- Locators: XPath
- Test runner: xUnit
- Assertions: FluentAssertions
- Logging: Serilog (Console + rolling file)
- Parallel execution: supported (one WebDriver instance per test thread)
- Data provider: xUnit [Theory] with MemberData for accepted usernames

User stories (UC)
- UC-1: Login with empty credentials -> verify error "Username is required"
- UC-2: Login with missing password -> verify error "Password is required"
- UC-3: Login success for accepted usernames (password = secret_sauce) -> verify dashboard title "Swag Labs"

Quick start
1. Restore packages: `dotnet restore` (Selenium, FluentAssertions, Serilog)
2. Choose browser: set environment variable `BROWSER` to `Chrome` or `Firefox`. Default: Chrome.
3. Run tests: `dotnet test`
4. Logs:
   - Console: visible during test run
   - Log file: `./logs/test-log-.log` (rolling daily, keep 2 days)
   - Screenshots on failure: saved to `./screenshots`

Project layout
- CoreLayer/
  - DriverManager.cs
  - SerilogConfig.cs
  - ScreenshotHelper.cs
- BusinessLayer/
  - Pages/LoginPage.cs
  - Pages/InventoryPage.cs
- TestLayer/
  - Fixtures/TestBase.cs
  - Tests/LoginTests.cs
  - TestData/AcceptedUsersData.cs
  - TestAutomationFrameworkPractice.Tests.csproj

Notes
- All locators use XPath stored in Page Objects.
- Screenshots are captured on test failure and saved in `./screenshots` (not embedded in output).
```
