using Grace.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using WebApi.Host;

namespace WebApi.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            HostHelper.BuildAndRun(BuildConfiguration, CreateHostBuilder, args);
        }
        private static Result<IConfigurationRoot> BuildConfiguration() =>
                Result<IConfigurationRoot>.Ok(
                    new ConfigurationBuilder()
                        .AddAppSettingsJSON()
                        .Build());

        private static IHostBuilder CreateHostBuilder(IConfigurationRoot configuration, string[] args) =>
            Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(builder => builder.AddConfiguration(configuration))
                .ConfigureAppConfiguration(builder => builder.AddConfiguration(configuration))
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                })
                .UseGrace()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel(options => options.AddServerHeader = false)
                        .UseIISIntegration()
                        .UseStartup<Startup>();
                }).ConfigureLogging(logBuilder =>
                {
                    logBuilder.ClearProviders(); // removes all providers from LoggerFactory
                    logBuilder.AddConsole();
                    logBuilder.AddTraceSource("Information, ActivityTracing"); // Add Trace listener provider
                });
    }
}
