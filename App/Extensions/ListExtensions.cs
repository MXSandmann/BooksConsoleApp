using System.Text;
using BooksConsoleApp.Models;

namespace BooksConsoleApp.Extensions;

public static class ListExtensions
{
    public static string Print(this List<BookDto> list)
    {
        var sb = new StringBuilder($"Found {list.Count} items: ");
        foreach (var item in list)
        {
            sb.Append($"\n\t{item.ToString()}");
        }
        return sb.ToString();
    }
}