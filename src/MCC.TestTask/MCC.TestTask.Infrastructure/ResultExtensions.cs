using FluentResults;

namespace MCC.TestTask.Infrastructure;

public static class CustomErrors
{
    public static Result ValidationError(string message)
    {
        return Result.Fail(new ValidationError(message));
    }

    public static Result NotFound(string message)
    {
        return Result.Fail(new NotFoundError(message));
    }

    public static Result Forbidden(string message)
    {
        return Result.Fail(new ForbiddenError(message));
    }

    public static Result AuthError(string message)
    {
        return Result.Fail(new AuthError(message));
    }
}