using Hangfire;
using Hangfire.PostgreSql;

namespace MCC.TestTask.App.Setup;

public static class SetupHangfire
{
    public static void AddHangfire(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("HangfireDbConnection");

        builder.Services.AddHangfire(config =>
            config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(connectionString)));
        builder.Services.AddHangfireServer();
    }

    public static void UseHangfire(IApplicationBuilder app)
    {
    }
}