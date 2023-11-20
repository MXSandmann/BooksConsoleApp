using BooksConsoleApp.Context;
using BooksConsoleApp.Models;
using BooksConsoleApp.Services;
using BooksConsoleApp.Tests.Extensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BooksConsoleApp.Tests;

public class ImportServiceTests
{
    private readonly string _pathToApp = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\App");
    private readonly string _pathToCsv = Path.Combine(@"..\..\..\..\..\", "books.csv");

    [Fact]
    public async Task ShouldImport_WhenAllOk()
    {
        // Arrange
        var serviceProvider = TestServiceProvider.Build(Path.GetFullPath(_pathToApp));
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync();

        // Act
        await ImportService.ImportFromCsv(_pathToCsv, serviceProvider);

        // Assert
        var records = await context.Books
            .Include(x => x.Author)
            .Include(x => x.Genre)
            .Include(x => x.Publisher)
            .Select(x => BookDto.FromEntity(x))
            .ToListAsync();

        records.Count.Should().Be(42);
        records.Should()
            .OnlyContain(x => !string.IsNullOrWhiteSpace(x.Author))
            .And
            .OnlyContain(x => !string.IsNullOrWhiteSpace(x.Genre))
            .And
            .OnlyContain(x => !string.IsNullOrWhiteSpace(x.Publisher))
            .And
            .OnlyContain(x => x.Pages > 0)
            .And
            .OnlyContain(x => !string.IsNullOrWhiteSpace(x.ReleaseDate));
    }
}

