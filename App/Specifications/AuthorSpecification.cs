using System.Linq.Expressions;
using BooksConsoleApp.Models.Entities;

namespace BooksConsoleApp.Specifications;

public class AuthorSpecification : ISpecification<Book>
{
    private readonly string _name;
    public AuthorSpecification(string name) => _name = name;
    public Expression<Func<Book, bool>> Criteria => book => book.Author.Name == _name;
}