namespace BooksConsoleApp.Exceptions;

public class FilterValidationException : Exception
{
    public FilterValidationException(string message) : base(message) { }
}