using System.Linq.Expressions;

namespace BooksConsoleApp.Specifications;

public interface ISpecification<T> where T: class
{
    Expression<Func<T, bool>> Criteria { get; }
}