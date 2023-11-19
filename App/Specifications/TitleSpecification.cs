using System.Linq.Expressions;
using BooksConsoleApp.Models.Entities;

namespace BooksConsoleApp.Specifications;

public class TitleSpecification : ISpecification<Book>
{
    private readonly string _title;
    public TitleSpecification(string title) => _title = title;
    public Expression<Func<Book, bool>> Criteria => book => book.Title == _title;
}