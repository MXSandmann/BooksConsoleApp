using BooksConsoleApp.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BooksConsoleApp.Services;

public static class MigrationsService
{
    public static void UpdateDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        context.Database.Migrate();
        Console.WriteLine("Successfully connected and updated the database!");
    }
}