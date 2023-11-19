using System.Linq.Expressions;
using BooksConsoleApp.Models.Entities;

namespace BooksConsoleApp.Specifications;

public class PublishedBeforeSpecification : ISpecification<Book>
{
    private readonly DateTime _releaseDate;

    public PublishedBeforeSpecification(DateTime releaseDate) => _releaseDate = releaseDate;
    public Expression<Func<Book, bool>> Criteria => book => book.ReleaseDate < _releaseDate;
}

public class PublishedAfterSpecification : ISpecification<Book>
{
    private readonly DateTime _releaseDate;

    public PublishedAfterSpecification(DateTime releaseDate) => _releaseDate = releaseDate;
    public Expression<Func<Book, bool>> Criteria => book => book.ReleaseDate > _releaseDate;
}