using AutoBogus;
using BooksConsoleApp.Models;
using BooksConsoleApp.Services;
using FluentAssertions;

namespace BooksConsoleApp.Tests;

public class CsvWriterServiceTests
{
    private readonly string _path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "test_output"));
    
    [Fact]
    public async Task CreateCsv_WhenAllOk()
    {
        try
        {
            // Arrange
            var filter = new Filter {Title = "Test"};
            var filterHash = filter.GetHashCode();
            var books = new List<BookDto>(10);
            var faker = AutoFaker.Create();
            for (var i = 0; i < 10; i++)
            {
                var book = faker.Generate<BookDto>();
                books.Add(book);
            }

            // Act
            await CsvWriterService.SaveToCsv(books, filter, _path);

            // Assert
            var files = Directory.GetFiles(_path, $"*{filterHash}*");
            files.Should().ContainSingle(file => file.Contains(filterHash.ToString()));
        }
        finally
        {
            if (Directory.Exists(_path))
            {
                Directory.Delete(_path, true);
            }
        }
    }

    [Fact]
    public async Task CreateCsv_ShouldThrow_WhenNotOk()
    {
        // Arrange

        // Act
        var func = async () => await CsvWriterService.SaveToCsv(new List<BookDto>(), null!, _path);

        // Assert
        await func.Should().ThrowExactlyAsync<IOException>();
    }
}