using MCC.TestTask.Infrastructure;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace MCC.TestTask.App.Setup;

public static class SetupResults
{
    public static void Setup(WebApplicationBuilder builder)
    {
        using var provider = builder.Services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ResultEndpointProfile>>();

        AspNetCoreResult.Setup(config => { config.DefaultProfile = new ResultEndpointProfile(logger); });
    }

    public class ResultEndpointProfile : DefaultAspNetCoreResultEndpointProfile
    {
        private readonly ILogger<ResultEndpointProfile> _logger;

        public ResultEndpointProfile(ILogger<ResultEndpointProfile> logger)
        {
            _logger = logger;
        }

        public override ActionResult TransformFailedResultToActionResult(
            FailedResultToActionResultTransformationContext context)
        {
            var result = context.Result;

            if (result.HasError<ForbiddenError>(out var forbiddenErrors))
                return new UnauthorizedObjectResult(new ProblemDetails { Detail = forbiddenErrors.First().Message });

            if (result.HasError<NotFoundError>(out var notFoundErrors))
                return new NotFoundObjectResult(new ProblemDetails { Detail = notFoundErrors.First().Message });

            if (result.HasError<ValidationError>(out var validationErrors))
                return new BadRequestObjectResult(new ProblemDetails { Detail = validationErrors.First().Message });

            if(result.HasError<AuthError>())
                return new UnauthorizedResult();
            
            if (result.IsFailed)
                _logger.LogError("Unhandled error result: @Result", new { Result = result });

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}