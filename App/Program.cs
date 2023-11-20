using BooksConsoleApp;
using BooksConsoleApp.Context;
using BooksConsoleApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(Configurations.PathToAppsettings, optional: true, reloadOnChange: true)
    .Build();

var connectionString = configuration.GetConnectionString("SqlServer")!;

var serviceProvider = new ServiceCollection()
    .AddDbContext<DataContext>(opt => opt.UseSqlServer(connectionString))
    //.Configure<Filter>(configuration.GetSection(Configurations.FilterSectionName))
    .BuildServiceProvider();

MigrationsService.UpdateDatabase(serviceProvider);
await UserInteractor.Start(serviceProvider, configuration);