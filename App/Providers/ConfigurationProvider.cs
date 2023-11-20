using Microsoft.Extensions.Configuration;

namespace BooksConsoleApp.Providers;

// public class ConfigurationProvider
// {
//     private readonly string _dir;
//     public ConfigurationProvider(string dir) => _dir = dir;
//
//     public string GetConnectionString()
//     {
//         //var dir = Directory.GetCurrentDirectory();
//         var builder = new ConfigurationBuilder()
//             .SetBasePath(_dir)
//             .AddJsonFile(Configurations.PathToAppsettings, optional: true, reloadOnChange: true);
//
//         var configuration = builder.Build();
//         var connectionString = configuration.GetConnectionString("SqlServer")!;
//         return connectionString;
//     }
// }