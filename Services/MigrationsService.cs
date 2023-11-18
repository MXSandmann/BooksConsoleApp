using BooksConsoleApp.Context;
using BooksConsoleApp.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BooksConsoleApp.Services;

public static class MigrationsService
{
    public static void UpdateDatabase()
    {
        var connectionString = ConfigurationHelper.GetConnectionString();
        using var context = new DataContext(connectionString);
        context.Database.Migrate();
        Console.WriteLine("Successfully connected and updated the database!");
    }
}