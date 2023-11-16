using BooksConsoleApp.Context;
using BooksConsoleApp.Helpers;

namespace BooksConsoleApp;

public static class UserInteractor
{
    public static void Start()
    {
        var connectionString = ConfigurationHelper.GetConnectionString();
        using var context = new DataContext(connectionString);
        
        while (true)
        {
            Console.WriteLine("Choose your option:");
            Console.WriteLine(" 1: add books from file");
            Console.WriteLine(" 2: search books");

            var key = Console.ReadKey().KeyChar;

            if (key == '1')
            {
                Console.WriteLine("Please enter path to file:");
                var path = Console.ReadLine();
            }
        }
    }

    private static string GetOutput(DataContext context, char key) =>
        key switch
        {
            'a' => string.Join(';', context.Authors.ToList()),
            'b' => string.Join(';', context.Books.ToList()),
            'p' => string.Join(';', context.Publishers.ToList()),
            'g' => string.Join(';', context.Genres.ToList()),
            _ => "Please enter a valid option..."
        };
}