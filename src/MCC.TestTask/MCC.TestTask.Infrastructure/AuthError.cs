using FluentResults;

namespace MCC.TestTask.Infrastructure;

public class AuthError : Error
{
    public AuthError(string message) : base(message)
    {
    }
}