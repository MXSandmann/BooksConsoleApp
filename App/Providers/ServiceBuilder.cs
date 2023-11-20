using BooksConsoleApp.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BooksConsoleApp.Providers;

public static class ServiceBuilder
{
    public static IServiceProvider Get()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(Configurations.PathToAppsettings, optional: true, reloadOnChange: true)
            .AddJsonFile(Configurations.PathToSecrets, optional: true, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("SqlServer")!;

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddDbContext<DataContext>(opt => opt.UseSqlServer(connectionString))
            .AddOptions<Filter>().Bind(configuration.GetSection(Filter.SectionName)).Services
            .BuildServiceProvider();

        return serviceProvider;
    }
}