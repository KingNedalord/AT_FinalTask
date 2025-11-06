using System;
using CoreLayer;
using Serilog;
using Xunit.Abstractions;

namespace TestLayer.Fixtures
{
    /// <summary>
    /// Base test class that ensures Driver is created per test and disposed after.
    /// Also configures Serilog once (per process).
    /// Provides ExecuteTest wrapper to capture screenshots on failure.
    /// </summary>
    public abstract class TestBase : IDisposable
    {
        protected readonly ITestOutputHelper Output;

        protected TestBase(ITestOutputHelper output)
        {
            Output = output;
            SerilogConfig.Configure();
            DriverManager.Instance.CreateDriver();
        }

        protected void ExecuteTest(Action testAction, string testName)
        {
            try
            {
                testAction();
            }
            catch (Exception ex)
            {
                try
                {
                    var path = ScreenshotHelper.SaveScreenshot(DriverManager.Instance.Driver, testName);
                    Log.Error(ex, "Test {TestName} failed, screenshot saved to {Screenshot}", testName, path);
                    if (!string.IsNullOrEmpty(path)) Output.WriteLine($"Screenshot: {path}");
                }
                catch (Exception inner)
                {
                    Log.Error(inner, "Failed to take screenshot for {TestName}", testName);
                }

                throw;
            }
        }

        public void Dispose()
        {
            DriverManager.Instance.QuitDriver();
        }
    }
}
