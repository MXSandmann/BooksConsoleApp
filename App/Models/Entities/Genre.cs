namespace BooksConsoleApp.Models.Entities;

public class Genre : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<Book> Books { get; set; } = null!;
    public static Genre FromBookDto(BookDto dto) => new() { Name = dto.Genre };
}