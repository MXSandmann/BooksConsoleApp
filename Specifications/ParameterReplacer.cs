using System.Linq.Expressions;

namespace BooksConsoleApp.Specifications;

public class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _parameterExpression;
    public ParameterReplacer(ParameterExpression parameterExpression) => _parameterExpression = parameterExpression;
    protected override Expression VisitParameter(ParameterExpression node) => base.VisitParameter(_parameterExpression);
}