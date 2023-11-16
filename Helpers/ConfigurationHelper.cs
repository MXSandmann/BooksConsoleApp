using Microsoft.Extensions.Configuration;

namespace BooksConsoleApp.Helpers;

internal static class ConfigurationHelper
{
    internal static string GetConnectionString()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        var configuration = builder.Build();
        var connectionString = configuration.GetConnectionString("SqlServer")!;
        return connectionString;
    }
}