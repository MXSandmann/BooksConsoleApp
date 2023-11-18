namespace BooksConsoleApp;

public class Filter
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public string? Author { get; set; }
    public string? Publisher { get; set; }
    public int? MoreThenPages { get; set; }
    public int? LessThenPages { get; set; }
    public string? PublishedBefore { get; set; }
    public string? PublishedAfter { get; set; }
}