namespace BooksConsoleApp.Models.Entities;

public class Author : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<Book> Books { get; set; } = null!;
    public static Author FromBookDto(BookDto dto) => new() { Name = dto.Author };
}