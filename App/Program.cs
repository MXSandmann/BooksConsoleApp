using BooksConsoleApp;
using BooksConsoleApp.Context;
using BooksConsoleApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(Configurations.PathToAppsettings, optional: true, reloadOnChange: true)
    .AddJsonFile(Configurations.PathToAppsettings, optional: true, reloadOnChange: true)
    .Build();

var connectionString = configuration.GetConnectionString("SqlServer")!;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddDbContext<DataContext>(opt => opt.UseSqlServer(connectionString))
    .AddOptions<Filter>().Bind(configuration.GetSection(Filter.SectionName)).Services
    .BuildServiceProvider();

MigrationsService.UpdateDatabase(serviceProvider);
await UserInteractor.Start(serviceProvider);