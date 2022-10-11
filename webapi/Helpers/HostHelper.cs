using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Hosting;

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