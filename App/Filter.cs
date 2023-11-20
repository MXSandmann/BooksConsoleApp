using System.Linq.Expressions;
using BooksConsoleApp.Exceptions;
using BooksConsoleApp.Models.Entities;
using BooksConsoleApp.Specifications;

namespace BooksConsoleApp;

public class Filter
{
    public const string SectionName = "Filter";
    public string? Title { get; init; }
    public string? Genre { get; init; }
    public string? Author { get; init; }
    public string? Publisher { get; init; }
    public int? MoreThenPages { get; init; }
    public int? LessThenPages { get; init; }
    public string? PublishedBefore { get; init; }
    public string? PublishedAfter { get; init; }

    public Filter Validate()
    {
        if (MoreThenPages is < 0)
            throw new FilterValidationException($"{nameof(MoreThenPages)} should be greater then 0!");
        if (LessThenPages is < 0)
            throw new FilterValidationException($"{nameof(LessThenPages)} should be greater then 0!");
        if (LessThenPages.HasValue
            && MoreThenPages.HasValue
            && LessThenPages.Value < MoreThenPages.Value)
            throw new FilterValidationException($"{nameof(MoreThenPages)} should be greater then {nameof(LessThenPages)}!");
        if (!string.IsNullOrWhiteSpace(PublishedBefore) 
            && !DateTime.TryParse(PublishedBefore, out _))
            throw new FilterValidationException($"{nameof(PublishedBefore)} is not DateTime format!");
        if (!string.IsNullOrWhiteSpace(PublishedAfter)
            && !DateTime.TryParse(PublishedAfter, out _))
            throw new FilterValidationException($"{nameof(PublishedAfter)} is not DateTime format!");
        return this;
    }

    public Expression<Func<Book, bool>> PrepareSpecifications()
    {
        var results = new List<ISpecification<Book>>();
        
        if (!string.IsNullOrWhiteSpace(Title))
            results.Add(new TitleSpecification(Title));
        
        if (!string.IsNullOrWhiteSpace(Author))
            results.Add(new AuthorSpecification(Author));
        
        if (!string.IsNullOrWhiteSpace(Genre))
            results.Add(new GenreSpecification(Genre));
        
        if (!string.IsNullOrWhiteSpace(Publisher))
            results.Add(new PublisherSpecification(Publisher));
        
        if (MoreThenPages.HasValue)
            results.Add(new MoreThenPagesSpecifications(MoreThenPages.Value));
        
        if (LessThenPages.HasValue)
            results.Add(new LessThenPagesSpecifications(LessThenPages.Value));

        if (!string.IsNullOrWhiteSpace(PublishedBefore))
            results.Add(new PublishedBeforeSpecification(DateTime.Parse(PublishedBefore)));
        
        if (!string.IsNullOrWhiteSpace(PublishedAfter))
            results.Add(new PublishedAfterSpecification(DateTime.Parse(PublishedAfter)));

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