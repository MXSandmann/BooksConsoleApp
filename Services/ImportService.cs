using System.Globalization;
using BooksConsoleApp.Context;
using BooksConsoleApp.Helpers;
using BooksConsoleApp.Models;
using BooksConsoleApp.Models.Entities;
using CsvHelper;
using Microsoft.EntityFrameworkCore;

namespace BooksConsoleApp.Services;

public static class ImportService
{
    public static async Task ImportFromCsv(string path)
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<BookDto>().ToList();
    
        var connectionString = ConfigurationHelper.GetConnectionString();
        await using var context = new DataContext(connectionString);

        foreach (var record in records)
        {
            // Add authors, genres and publishers first
            var authorTask = GetOrAddAuthor(connectionString, record);
            var publisherTask =  GetOrAddPublisher(connectionString, record);
            var genreTask =  GetOrAddGenre(connectionString, record);

            await Task.WhenAll(authorTask, publisherTask, genreTask);

            var author = await authorTask;
            var genre = await genreTask;
            var publisher = await publisherTask;
            
            // Add books with foreign keys
            var bookExists = await context.Books
                .Include(x => x.Author)
                .Include(x => x.Publisher)
                .AnyAsync(x => x.Title == record.Title
                    && x.Publisher.Name == record.Publisher
                    && x.Author.Name == record.Author);
            if (bookExists) continue;

            var book = Book.FromBookDto(record);
            book.AuthorId = author.Id;
            book.GenreId = genre.Id;
            book.PublisherId = publisher.Id;
            context.Books.Add(book);
            await context.SaveChangesAsync();
        }
    }

    private static async Task<Author> GetOrAddAuthor(string connectionString, BookDto dto)
    {
        await using var context = new DataContext(connectionString);
        var author = await context.Authors.FirstOrDefaultAsync(x => x.Name == dto.Author);
        if (author is not null)
            return author;
        var newAuthor = Author.FromBookDto(dto);
        await context.AddAsync(newAuthor);
        await context.SaveChangesAsync();
        return newAuthor;
    }
    
    private static async Task<Genre> GetOrAddGenre(string connectionString, BookDto dto)
    {
        await using var context = new DataContext(connectionString);
        var genre = await context.Genres.FirstOrDefaultAsync(x => x.Name == dto.Genre);
        if (genre is not null)
            return genre;
        var newGenre = Genre.FromBookDto(dto);
        await context.Genres.AddAsync(newGenre);
        await context.SaveChangesAsync();
        return newGenre;
    }
    
    private static async Task<Publisher> GetOrAddPublisher(string connectionString, BookDto dto)
    {
        await using var context = new DataContext(connectionString);
        var publisher = await context.Publishers.FirstOrDefaultAsync(x => x.Name == dto.Publisher);
        if (publisher is not null)
            return publisher;
        var newPublisher = Publisher.FromBookDto(dto);
        await context.Publishers.AddAsync(newPublisher);
        await context.SaveChangesAsync();
        return newPublisher;
    }
}