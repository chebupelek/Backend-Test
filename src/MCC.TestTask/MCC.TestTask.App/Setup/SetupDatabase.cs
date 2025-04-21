using System.Data;
using MCC.TestTask.App.Services.Seeding;
using MCC.TestTask.Persistance;
using Microsoft.EntityFrameworkCore;
using NeinLinq;
using Npgsql;

namespace MCC.TestTask.App.Setup;

public static class SetupDatabase
{
    public static async Task RunMigrations(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        await using var blogContext = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
        await blogContext.Database.MigrateAsync();

        blogContext.ReloadTypesForEnumSupport();

        // If you'd like to modify this class, consider adding your custom code in the SetupDatabase.partial.cs
        // This will make it easier to pull changes from Template when Template is updated
        // (actually this file will be overwritten by a file from template, which will make your changes disappear)

        var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
        await seedingService.SeedDatabase();
    }

    public static void AddDatabase(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        IConfiguration configuration = builder.Configuration;

        services.AddDbContext<BlogDbContext>(options =>
            options
                .UseNpgsql(configuration.GetConnectionString("BlogDbConnection"))
                .WithLambdaInjection());

        services.AddScoped<SeedingService>();

        // If you'd like to modify this class, consider adding your custom code in the SetupDatabase.partial.cs
        // This will make it easier to pull changes from Template when Template is updated
        // (actually this file will be overwritten by a file from template, which will make your changes disappear)
        // AddSeeders(services, configuration);
    }
}

public static class DbContextExtensions
{
    /// <summary>
    ///     Postgresql specific action.
    ///     If you are using context.Database.Migrate() to create your enums,
    ///     you need to instruct Npgsql to reload all types after applying your migrations
    ///     For more info refer to: https://www.npgsql.org/efcore/mapping/enum.html?tabs=tabid-1#creating-your-database-enum
    /// </summary>
    public static void ReloadTypesForEnumSupport(this DbContext context)
    {
        // This is for enum support in PostgreSQL.
        // Details here: https://www.npgsql.org/efcore/mapping/enum.html#creating-your-database-enum
        var conn = (NpgsqlConnection)context.Database.GetDbConnection();
        if (conn.State != ConnectionState.Open) conn.Open();
        conn.ReloadTypes();
    }
}