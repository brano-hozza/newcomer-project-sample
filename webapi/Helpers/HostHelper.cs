using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WebApi.Host;
public static class HostHelper
{
    public static void BuildAndRun(Func<Result<IConfigurationRoot>> BuildConfiguration, Func<IConfigurationRoot, string[], IHostBuilder> CreateHostBuilder, string[] args)
    {
        Result<IConfigurationRoot> buildConfigurationResult = BuildConfiguration();

        var host = CreateHostBuilder(buildConfigurationResult.Value, args).Build();
        host.Run();
    }
}