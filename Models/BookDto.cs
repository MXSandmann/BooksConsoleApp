namespace BooksConsoleApp.Models;

public record BookDto(
    string Title,
    int Pages,
    string Genre,
    string ReleaseDate,
    string Author,
    string Publisher);