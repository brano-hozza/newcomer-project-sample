using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WebApi.Helpers;
public static class HostHelper
{
    public static void BuildAndRun(Func<Result<IConfigurationRoot>> buildConfiguration, Func<IConfigurationRoot, string[], IHostBuilder> createHostBuilder, string[] args)
    {
        Result<IConfigurationRoot> buildConfigurationResult = buildConfiguration();

        var host = createHostBuilder(buildConfigurationResult.Value, args).Build();
        host.Run();
    }
}