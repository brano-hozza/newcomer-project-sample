using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

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