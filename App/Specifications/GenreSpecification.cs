using System.Linq.Expressions;
using BooksConsoleApp.Models.Entities;

namespace BooksConsoleApp.Specifications;

public class GenreSpecification : ISpecification<Book>
{
    private readonly string _name;
    public GenreSpecification(string name) => _name = name;
    public Expression<Func<Book, bool>> Criteria => book => book.Genre.Name == _name;
}