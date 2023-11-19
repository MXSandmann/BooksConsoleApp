using System.Text.Json;
using BooksConsoleApp.Context;
using BooksConsoleApp.Helpers;
using BooksConsoleApp.Models;
using BooksConsoleApp.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BooksConsoleApp.Services;

public static class QueryService
{
    public static async Task<List<BookDto>> SearchWithFilter()
    {
        var jsonDocument = JsonDocument.Parse(await File.ReadAllTextAsync(Configurations.PathToAppsettings));
        var filterJson = jsonDocument.RootElement.GetProperty(Configurations.FilterSectionName).GetRawText();

        var filter = JsonSerializer.Deserialize<Filter>(filterJson);
        ArgumentNullException.ThrowIfNull(filter);

        var connectionString = ConfigurationHelper.GetConnectionString();
        await using var context = new DataContext(connectionString);

        var specifications = filter.PrepareSpecifications();

        var results = await context.Books
            .Include(x => x.Author)
            .Include(x => x.Genre)
            .Include(x => x.Publisher)
            .Where(specifications)
            .Select(x => BookDto.FromEntity(x))
            .ToListAsync();
        return results;
    }
}