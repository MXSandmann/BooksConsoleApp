using BooksConsoleApp.Context;
using BooksConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BooksConsoleApp.Services;

public static class QueryService
{
    public static async Task<List<BookDto>> SearchWithFilter(DataContext context, IOptions<Filter> filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var specifications = filter.Value
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