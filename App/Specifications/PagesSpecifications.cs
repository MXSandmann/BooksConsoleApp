using System.Linq.Expressions;
using BooksConsoleApp.Models.Entities;

namespace BooksConsoleApp.Specifications;

public class MoreThenPagesSpecifications : ISpecification<Book>
{
    private readonly int _pages;
    public MoreThenPagesSpecifications(int pages) => _pages = pages;
    public Expression<Func<Book, bool>> Criteria => book => book.Pages >= _pages;
}

public class LessThenPagesSpecifications : ISpecification<Book>
{
    private readonly int _pages;
    public LessThenPagesSpecifications(int pages) => _pages = pages;
    public Expression<Func<Book, bool>> Criteria => book => book.Pages < _pages;
}