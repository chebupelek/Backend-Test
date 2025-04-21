using MCC.TestTask.App.Setup;

var builder = WebApplication.CreateBuilder(args);

SetupResults.Setup(builder);

SetupAspNet.AddAspNet(builder);
SetupSwagger.AddSwagger(builder);
SetupDatabase.AddDatabase(builder);
SetupAuth.AddAuth(builder);
SetupServices.AddServices(builder.Services, builder.Configuration, builder.Environment);
SetupHangfire.AddHangfire(builder);

var app = builder.Build();

SetupHangfire.UseHangfire(app);
SetupSwagger.UseSwagger(app);
SetupAspNet.UseAspNet(app);
SetupAuth.UseAuth(app);

await SetupDatabase.RunMigrations(app);

app.Run();