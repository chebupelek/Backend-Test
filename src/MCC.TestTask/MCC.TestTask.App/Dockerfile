FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
USER $APP_UID
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY MCC.TestTask.App/MCC.TestTask.App.csproj MCC.TestTask.App/
COPY MCC.TestTask.Domain/MCC.TestTask.Domain.csproj MCC.TestTask.Domain/
COPY MCC.TestTask.Persistence/MCC.TestTask.Persistence.csproj MCC.TestTask.Persistence/
COPY MCC.TestTask.Infrastructure/MCC.TestTask.Infrastructure.csproj MCC.TestTask.Infrastructure/

RUN dotnet restore MCC.TestTask.App/MCC.TestTask.App.csproj
COPY . .
WORKDIR /src/MCC.TestTask.App
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build
FROM builder AS publisher
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM runtime AS final
WORKDIR /app
COPY --from=publisher /app/publish .
ENTRYPOINT ["dotnet", "MCC.TestTask.App.dll"]
