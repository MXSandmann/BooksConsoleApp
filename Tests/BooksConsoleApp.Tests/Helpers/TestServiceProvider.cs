using BooksConsoleApp.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BooksConsoleApp.Tests.Helpers;

internal static class TestServiceProvider
{
    internal static IServiceProvider Build(string pathToApp)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(pathToApp)
            .AddJsonFile(Path.Combine(pathToApp, Configurations.PathToAppsettings), optional: true, reloadOnChange: true)
            .AddJsonFile(Path.Combine(pathToApp, Configurations.PathToSecrets), optional: true, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("SqlServer")!;

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddDbContext<DataContext>(opt => opt.UseSqlServer(connectionString))
            .BuildServiceProvider();
        return serviceProvider;
    }
}