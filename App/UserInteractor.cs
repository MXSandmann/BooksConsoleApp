using BooksConsoleApp.Context;
using BooksConsoleApp.Extensions;
using BooksConsoleApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BooksConsoleApp;

public static class UserInteractor
{
    public static async Task Start(IServiceProvider provider)
    {
        while (true)
        {
            Console.WriteLine("Choose your option:\n\t1: Add books from file\n\t2: Search books using filter");

            await Select(Console.ReadKey(true).KeyChar, provider);
            
            Console.WriteLine("Press any key to continue or Esc to exit:");
            var keyInfo = Console.ReadKey(true);

            if (keyInfo.Key != ConsoleKey.Escape) continue;
            Console.WriteLine("Goodbye!");
            break;
        }
    }

    private static Task Select(char key, IServiceProvider provider) =>

        key switch
        {
            '1' => Import(provider),
            '2' => Query(provider),
            _ => Task.CompletedTask
        };

    private static async Task Import(IServiceProvider provider)
    {
        Console.WriteLine("Please enter path to the file:");
        try
        {
            var path = Console.ReadLine();
            await ImportService.ImportFromCsv(path!, provider);
            Console.WriteLine("Data successfully imported!");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while importing file: {e.Message} {e.InnerException}");
        }
    }

    private static async Task Query(IServiceProvider provider)
    {
        try
        {
            Console.WriteLine("Applying filter, printing the search results...");
                        
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();
            var filter = scope.ServiceProvider.GetRequiredService<IOptions<Filter>>();
            var results = await QueryService.SearchWithFilter(context, filter);
                        
            Console.WriteLine(results.Print());
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while querying the database: {e.Message} {e.InnerException}");
        }
    }
}