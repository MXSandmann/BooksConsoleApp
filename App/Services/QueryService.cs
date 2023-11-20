using System.Text.Json;
using BooksConsoleApp.Context;
using BooksConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BooksConsoleApp.Services;

public static class QueryService
{
    public static async Task<List<BookDto>> SearchWithFilter(IServiceProvider provider)
    {
        var jsonDocument = JsonDocument.Parse(await File.ReadAllTextAsync(Configurations.PathToAppsettings));
        var filterJson = jsonDocument.RootElement.GetProperty(Filter.SectionName).GetRawText();

        var filter = JsonSerializer.Deserialize<Filter>(filterJson);
        ArgumentNullException.ThrowIfNull(filter);

        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        var specifications = filter
            .Validate()
            .PrepareSpecifications();

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