using BooksConsoleApp.Context;
using BooksConsoleApp.Exceptions;
using BooksConsoleApp.Services;
using BooksConsoleApp.Tests.Helpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BooksConsoleApp.Tests;

public class QueryServiceTests
{
    private readonly string _pathToApp = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\App");

    [Theory]
    [MemberData(nameof(ProvideValidFilters))]
    public async Task ShouldReturnQueryResults_CorrespondingToFilter(Filter filter, int countRecords)
    {
        // Arrange
        var serviceProvider = TestServiceProvider.Build(Path.GetFullPath(_pathToApp));
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync();
        
        // Act
        var results = await QueryService.SearchWithFilter(context, Options.Create(filter));

        // Assert
        results.Count.Should().Be(countRecords);
    }
    
    [Theory]
    [MemberData(nameof(ProvideInvalidFilters))]
    public async Task ShouldThrow_WhenInvalidFilter(Filter filter, string errorMessage)
    {
        // Arrange
        var serviceProvider = TestServiceProvider.Build(Path.GetFullPath(_pathToApp));
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync();
        
        // Act
        var func = async () => await QueryService.SearchWithFilter(context, Options.Create(filter));

        // Assert
        var ex = await func.Should().ThrowExactlyAsync<FilterValidationException>();
        ex.WithMessage(errorMessage);
    }

    public static IEnumerable<object[]> ProvideValidFilters()
    {
        yield return new object[]
        {
            new Filter {Title = "Hobbit"}, 1
        };
        yield return new object[]
        {
            new Filter {LessThenPages = 1000, MoreThenPages = 600}, 6
        };
        yield return new object[]
        {
            new Filter {Publisher = "Doubleday"}, 2
        };
        yield return new object[]
        {
            new Filter { Author = "George Orwell" }, 3
        };
        yield return new object[]
        {
            new Filter { Genre = "Fantasy" }, 4
        };
        yield return new object[]
        {
            new Filter { PublishedBefore = "2003-05-29", PublishedAfter = "1939-04-14"}, 16
        };
        yield return new object[]
        {
            new Filter { Title = "A", PublishedBefore = "2003-05-29", PublishedAfter = "1939-04-14"}, 10
        };
        yield return new object[]
        {
            new Filter { Title = "A", PublishedBefore = "2003-05-29", PublishedAfter = "1939-04-14", LessThenPages = 200}, 4
        };
    }

    public static IEnumerable<object[]> ProvideInvalidFilters()
    {
        yield return new object[]
        {
            new Filter {LessThenPages = -5}, "LessThenPages should be greater then 0!"
        };
        yield return new object[]
        {
            new Filter {MoreThenPages = -8}, "MoreThenPages should be greater then 0!"
        };
        yield return new object[]
        {
            new Filter {LessThenPages = 2, MoreThenPages = 3}, "MoreThenPages should be greater then LessThenPages!"
        };
        yield return new object[]
        {
            new Filter {PublishedAfter = "bla"}, "PublishedAfter is not DateTime format!"
        };
        yield return new object[]
        {
            new Filter {PublishedBefore = "bla"}, "PublishedBefore is not DateTime format!"
        };
    }
}