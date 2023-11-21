using System.Text;
using BooksConsoleApp.Models;

namespace BooksConsoleApp.Services;

public static class CsvWriterService
{
    private static string _fullPath = default!;
    
    /// <summary>
    /// Creates a Csv File based on the query results
    /// </summary>
    /// <param name="books"></param>
    /// <param name="filter"></param>
    /// <param name="outputPath"></param>
    /// <returns>Name of the created file</returns>
    /// <exception cref="IOException"></exception>
    public static async Task<string> SaveToCsv(List<BookDto> books, Filter filter, string outputPath)
    {
        try
        {
            // Get the current date and time in a safe file name format
            var dateTimeNow = DateTime.Now.ToString("MM/dd/yyyy");
            var fileName = $"Books_{dateTimeNow}_{filter.GetHashCode()}.csv";
        
            Directory.CreateDirectory(outputPath);

            // Create a new StringBuilder for CSV content
            var csvContent = new StringBuilder();

            // Add CSV header
            csvContent.AppendLine("Title,Pages,Genre,ReleaseDate,Author,Publisher");

            // Add book data
            foreach (var book in books)
                csvContent.AppendLine($"{Escape(book.Title)},{book.Pages},{Escape(book.Genre)},{book.ReleaseDate},{Escape(book.Author)},{Escape(book.Publisher)}");

            // Write to file
            _fullPath = Path.Combine(outputPath, fileName);
            await File.WriteAllTextAsync(_fullPath, csvContent.ToString());
            return fileName;
        }
        catch (Exception e)
        {
            if(File.Exists(_fullPath))
                File.Delete(_fullPath);
            throw new IOException(e.Message, e);
        }
        
    }

    // Escape CSV special characters
    private static string Escape(string input)
    {
        if (input.Contains(',') || input.Contains('"') || input.Contains('\n'))
            return $"\"{input.Replace("\"", "\"\"")}\"";
        return input;
    }
}