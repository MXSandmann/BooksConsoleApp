using System.Linq.Expressions;
using BooksConsoleApp.Models.Entities;

namespace BooksConsoleApp.Specifications;

public class PublisherSpecification : ISpecification<Book>
{
    private readonly string _name;
    public PublisherSpecification(string name) => _name = name;
    public Expression<Func<Book, bool>> Criteria => book => book.Publisher.Name == _name;
}