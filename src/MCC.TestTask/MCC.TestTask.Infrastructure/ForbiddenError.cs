using FluentResults;

namespace MCC.TestTask.Infrastructure;

public class ForbiddenError : Error
{
    public ForbiddenError(string message) : base(message)
    {
    }
}