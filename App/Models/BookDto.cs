using System.Globalization;
using BooksConsoleApp.Models.Entities;

namespace BooksConsoleApp.Models;

public record BookDto(
    string Title,
    int Pages,
    string Genre,
    string ReleaseDate,
    string Author,
    string Publisher)
{
    public static BookDto FromEntity(Book book)
    {
        return new BookDto(
            book.Title,
            book.Pages,
            book.Genre.Name,
            book.ReleaseDate.ToString(CultureInfo.InvariantCulture),
            book.Author.Name,
            book.Publisher.Name);
    }
}