using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace MCC.TestTask.App.Setup;

public class SetupAspNet
{
    public static void AddAspNet(WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddHealthChecks();

        builder.Services
            .AddControllers(AddGlobalFilters)
            .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); }
            );
    }

    private static void AddGlobalFilters(MvcOptions options)
    {
        options.Filters.Add(
            new ProducesResponseTypeAttribute(typeof(ProblemDetails), 404)
        );
        options.Filters.Add(
            new ProducesResponseTypeAttribute(typeof(ProblemDetails), 401)
        );
        options.Filters.Add(
            new ProducesResponseTypeAttribute(typeof(ProblemDetails), 400)
        );
        options.Filters.Add(new ProducesResponseTypeAttribute(200));
    }

    public static void UseAspNet(WebApplication app)
    {
        app.MapControllers();

        //app.UseEndpoints(endpoints => { });

        app.MapHealthChecks("/health");

        if (!app.Environment.IsDevelopment())
            app.UseHttpsRedirection();
        app.MapControllers();
    }
}