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


        var left = results[0];
        var right = results[1];
        var andExpression = Expression.AndAlso(left.Criteria.Body, right.Criteria.Body);
        var paramExpression = Expression.Parameter(typeof(Book));
        andExpression =
            (BinaryExpression)new ParameterReplacer(Expression.Parameter(typeof(Book))).Visit(andExpression);
        var lambda = Expression.Lambda<Func<Book, bool>>(andExpression, Expression.Parameter(typeof(Book)));

        return lambda;
    }
}