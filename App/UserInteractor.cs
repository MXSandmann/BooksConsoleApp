using BooksConsoleApp.Services;
using Microsoft.Extensions.Configuration;

namespace BooksConsoleApp;

public static class UserInteractor
{
    public static async Task Start(IServiceProvider provider, IConfiguration configuration)
    {
        while (true)
        {
            Console.WriteLine("Choose your option:");
            Console.WriteLine("1: Add books from file");
            Console.WriteLine("2: Search books using filter");

            switch (Console.ReadKey(true).KeyChar)
            {
                case '1':
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
                    break;
                }
                case '2':
                    try
                    {
                        Console.WriteLine("Applying filter, printing the search results...");
                        var results = await QueryService.SearchWithFilter(provider);
                        results.ForEach(Console.WriteLine);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error while querying the database: {e.Message} {e.InnerException}");
                    }
                    break;
            }

            Console.WriteLine("Press any key to continue or Esc to exit:");
            var keyInfo = Console.ReadKey(true);

            if (keyInfo.Key != ConsoleKey.Escape) continue;
            Console.WriteLine("Goodbye!");
            break;
        }
    }
}