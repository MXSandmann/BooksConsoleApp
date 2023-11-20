using BooksConsoleApp;
using BooksConsoleApp.Providers;
using BooksConsoleApp.Services;

var serviceProvider = ServiceBuilder.Get();
MigrationsService.UpdateDatabase(serviceProvider);
await UserInteractor.Start(serviceProvider);