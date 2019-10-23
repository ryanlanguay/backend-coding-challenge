namespace SearchSuggestions.UnitTests
{
    using System.IO;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Helper to build configuration in test projects
    /// </summary>
    internal static class ConfigurationHelper
    {
        /// <summary>
        /// Builds the test app configuration object
        /// </summary>
        /// <returns>The test configuration</returns>
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