namespace SearchSuggestions.UnitTests
{
    using System.IO;
    using Microsoft.Extensions.Configuration;

    internal static class ConfigurationHelper
    {
        public static IConfigurationRoot BuildConfiguration()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json")
                .Build();

            return config;
        }
    }
}