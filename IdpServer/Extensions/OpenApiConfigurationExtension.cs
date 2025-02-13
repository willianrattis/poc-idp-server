namespace IdpServer.Extensions;

public static class OpenApiConfigurationExtension
{
    public static void AddOpenApiConfig(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info.Version = Environment.GetEnvironmentVariable("DOCKER_IMAGE_VERSION") ?? "1.0.0";
                document.Info.Title = "IDP Server .NET 9 API";
                document.Info.Description = "IDP Server .NET 9 API";
                return Task.CompletedTask;
            });
        });
    }

    public static void UseOpenApiConfig(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference("/scalar/idp-server", options => options
            .WithTitle("IDP Server API")
            .WithOpenApiRoutePattern("/openapi/v1.json")
            .WithSidebar(true)
            .WithDotNetFlag(true)
            .WithLayout(ScalarLayout.Classic)
            .WithTheme(ScalarTheme.BluePlanet)
            .WithDarkMode(true)
        );
    }
}