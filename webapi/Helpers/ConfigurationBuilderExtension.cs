using System.IO;
using Microsoft.Extensions.Configuration;

namespace WebApi.Host
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder SetBasePath(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory());

            return configurationBuilder;
        }

        public static IConfigurationBuilder AddAppSettingsJSON(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return configurationBuilder;
        }
    }
}
