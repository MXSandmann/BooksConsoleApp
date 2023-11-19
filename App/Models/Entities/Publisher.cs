namespace BooksConsoleApp.Models.Entities;

public class Publisher : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<Book> Books { get; set; } = null!;
    public static Publisher FromBookDto(BookDto dto) => new() { Name = dto.Publisher };
}