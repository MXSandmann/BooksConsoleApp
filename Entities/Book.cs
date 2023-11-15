namespace BooksConsoleApp.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; } = null!;
    public int Pages { get; set; }
    public Guid GenreId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid PublisherId { get; set; }
    public DateTime ReleaseDate { get; set; }
}