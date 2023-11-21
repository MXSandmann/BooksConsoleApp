using System.Globalization;
using BooksConsoleApp.Context;
using BooksConsoleApp.Models;
using BooksConsoleApp.Models.Entities;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BooksConsoleApp.Services;

public static class ImportService
{
    /// <summary>
    /// Imports all records from a given csv file into a database as batches
    /// </summary>
    /// <param name="path"></param>
    /// <param name="provider"></param>
    public static async Task ImportFromCsv(string path, IServiceProvider provider)
    {
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<BookDto>().ToList();

        // Process in batches
        const int batchSize = 100;
        for (var i = 0; i < records.Count; i += batchSize)
        {
            var batch = records.Skip(i).Take(batchSize);
            await ProcessBatch(batch, context);
        }
    }

    private static async Task ProcessBatch(IEnumerable<BookDto> batch, DataContext context)
    {
        foreach (var record in batch)
        {
            if (await BookExists(context, record)) continue;

            var author = await GetOrAddAuthor(context, record);
            var genre = await GetOrAddGenre(context, record);
            var publisher = await GetOrAddPublisher(context, record);

            var book = Book.FromBookDto(record);
            book.AuthorId = author.Id;
            book.GenreId = genre.Id;
            book.PublisherId = publisher.Id;

            context.Books.Add(book);
        }
        await context.SaveChangesAsync();
    }

    private static async Task<Author> GetOrAddAuthor(DataContext context, BookDto dto)
    {
        var author = await context.Authors.FirstOrDefaultAsync(x => x.Name == dto.Author);
        if (author is not null)
            return author;
        var newAuthor = Author.FromBookDto(dto);
        await context.AddAsync(newAuthor);
        await context.SaveChangesAsync();
        return newAuthor;
    }
    
    private static async Task<Genre> GetOrAddGenre(DataContext context, BookDto dto)
    {
        var genre = await context.Genres.FirstOrDefaultAsync(x => x.Name == dto.Genre);
        if (genre is not null)
            return genre;
        var newGenre = Genre.FromBookDto(dto);
        await context.Genres.AddAsync(newGenre);
        await context.SaveChangesAsync();
        return newGenre;
    }
    
    private static async Task<Publisher> GetOrAddPublisher(DataContext context, BookDto dto)
    {
        var publisher = await context.Publishers.FirstOrDefaultAsync(x => x.Name == dto.Publisher);
        if (publisher is not null)
            return publisher;
        var newPublisher = Publisher.FromBookDto(dto);
        await context.Publishers.AddAsync(newPublisher);
        await context.SaveChangesAsync();
        return newPublisher;
    }

    private static async Task<bool> BookExists(DataContext context, BookDto record)
    {
        var bookExists = await context.Books
        .Include(x => x.Author)
        .Include(x => x.Publisher)
        .AnyAsync(x => x.Title == record.Title
            && x.Publisher.Name == record.Publisher
            && x.Author.Name == record.Author);
        return bookExists;
    }
}