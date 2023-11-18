using BooksConsoleApp.Context;
using BooksConsoleApp.Helpers;
using BooksConsoleApp.Services;

namespace BooksConsoleApp;

public static class UserInteractor
{
    public static async Task Start()
    {
        while (true)
        {
            Console.WriteLine("Choose your option:");
            Console.WriteLine("1: Add books from file");
            Console.WriteLine("2: Search books using filter");

            var key = Console.ReadKey(true).KeyChar;

            switch (key)
            {
                case '1':
                {
                    Console.WriteLine("Please enter path to the file:");
                    var path = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        Console.WriteLine("Empty path is provided");
                        break;
                    }
                    await ImportService.ImportFromCsv(path);
                    break;
                }
                case '2':
                    Console.WriteLine("Applying filter, printing the search results...");
                    break;
            }
        }
    }
}