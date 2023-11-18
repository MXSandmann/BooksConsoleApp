using BooksConsoleApp;
using BooksConsoleApp.Services;

MigrationsService.UpdateDatabase();
await UserInteractor.Start();



