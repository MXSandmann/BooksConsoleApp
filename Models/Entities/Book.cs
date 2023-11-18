namespace BooksConsoleApp.Models.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; } = null!;
    public int Pages { get; set; }
    public Guid GenreId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid PublisherId { get; set; }
    public DateTime ReleaseDate { get; set; }
    public static Book FromBookDto(BookDto dto)
    {
        // Try to parse the ReleaseDate from the DTO. If parsing fails, use DateTime.MinValue
        if (!DateTime.TryParse(dto.ReleaseDate, out var releaseDate))
        {
            releaseDate = default;
        }
    
        return new Book 
        {
            Title = dto.Title, 
            Pages = dto.Pages, 
            ReleaseDate = releaseDate
        };
    }
}