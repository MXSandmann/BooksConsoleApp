using System.Linq.Expressions;
using BooksConsoleApp.Models.Entities;
using BooksConsoleApp.Specifications;

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

    public Expression<Func<Book, bool>> PrepareSpecifications()
    {
        var results = new List<ISpecification<Book>>();
        
        if (!string.IsNullOrWhiteSpace(Title))
            results.Add(new TitleSpecification(Title));
        
        if (MoreThenPages.HasValue)
            results.Add(new MoreThenPagesSpecifications(MoreThenPages.Value));
        
        if (LessThenPages.HasValue)
            results.Add(new LessThenPagesSpecifications(LessThenPages.Value));

        if (!results.Any())
            return book => true; // Fallback if no specifications

        var parameter = Expression.Parameter(typeof(Book), "book");
        Expression combinedExpression = Expression.Invoke(results.First().Criteria, parameter);

        foreach (var spec in results.Skip(1))
        {
            var invokedExpr = Expression.Invoke(spec.Criteria, parameter);
            combinedExpression = Expression.AndAlso(combinedExpression, invokedExpr);
        }

        return Expression.Lambda<Func<Book, bool>>(combinedExpression, parameter);
    }
}