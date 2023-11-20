using BooksConsoleApp.Context;
using BooksConsoleApp.Models;
using BooksConsoleApp.Providers;
using BooksConsoleApp.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BooksConsoleApp.Tests;

public class ImportServiceTests
{
    private readonly string _pathToAppDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\App");
    private readonly string _pathToCsv = Path.Combine(@"..\..\..\..\..\", "books.csv");
    private readonly string _connectionString;
    private readonly ConfigurationProvider _provider;

    public ImportServiceTests()
    {
        _provider = new ConfigurationProvider(Path.GetFullPath(_pathToAppDirectory));
        _connectionString = _provider.GetConnectionString();
    }

    [Fact]
    public async Task ShouldImport_WhenAllOk()
    {
        // Arrange
        await using var context = new DataContext(_connectionString);
        
        // Act
        await ImportService.ImportFromCsv(_pathToCsv, _provider);

        // Assert
        var records = await context.Books
            .Include(x => x.Author)
            .Include(x => x.Genre)
            .Include(x => x.Publisher)
            .Select(x => BookDto.FromEntity(x))
            .ToListAsync();

        records.Count.Should().Be(42);
        records.Should().OnlyContain(x => !string.IsNullOrWhiteSpace(x.Author));
        records.Should().OnlyContain(x => !string.IsNullOrWhiteSpace(x.Genre));
        records.Should().OnlyContain(x => !string.IsNullOrWhiteSpace(x.Publisher));
    }
}

