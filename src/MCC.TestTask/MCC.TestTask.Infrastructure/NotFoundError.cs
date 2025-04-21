using FluentResults;

namespace MCC.TestTask.Infrastructure;

public class NotFoundError : Error
{
    public NotFoundError(string message) : base(message)
    {
    }
}