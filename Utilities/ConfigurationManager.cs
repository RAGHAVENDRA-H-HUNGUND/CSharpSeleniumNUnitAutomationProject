using Microsoft.Extensions.Configuration;

namespace CSharpSeleniumNUnitAutomationProject.Utilities
{
    public static class ConfigurationManager
    {
        private readonly static IConfigurationRoot config;

        static ConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");

            config = builder.Build();
        }

        public static string BaseUrl => config["AppSettings:BaseUrl"];
    }
}
