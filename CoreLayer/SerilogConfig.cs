using System;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace CoreLayer
{
    public static class SerilogConfig
    {
        public static void Configure(string minimumLevel = "Information")
        {
            var level = LogEventLevel.Information;
            if (!Enum.TryParse(minimumLevel, true, out level))
                level = LogEventLevel.Information;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(level)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("App", "SauceDemoTests")
                .WriteTo.Console()
                .WriteTo.File(new CompactJsonFormatter(), "logs/test-log-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 2)
                .CreateLogger();
        }
    }
}
