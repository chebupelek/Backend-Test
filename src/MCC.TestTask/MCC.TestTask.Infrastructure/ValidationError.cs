using FluentResults;

namespace MCC.TestTask.Infrastructure;

public class ValidationError : Error
{
    public ValidationError(string message) : base(message)
    {
    }
}