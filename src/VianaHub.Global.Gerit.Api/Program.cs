using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using VianaHub.Global.Gerit.Api.Configuration;
using VianaHub.Global.Gerit.Api.Configuration.Swagger;
using VianaHub.Global.Gerit.Api.Endpoints;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Api.Services;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Infra.Data.Context;
using VianaHub.Global.Gerit.Infra.Data.Interceptors;
using VianaHub.Global.Gerit.Infra.IoC;
using VianaHub.Global.Gerit.Application.Mappings;

namespace VianaHub.Global.Gerit.Api;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build())
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithProperty("Application", "VianaHub Gerit")
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        ConfigurationValidator.ValidateConfiguration(builder.Configuration, builder.Environment);

        builder.Host.UseSerilog();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<TenantSessionConnectionInterceptor>();
        builder.Services.AddScoped<TelemetryInterceptor>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwagger(builder.Configuration, builder.Environment);

        builder.Services.AddAutoMapper(typeof(ActionMappingProfile).Assembly);
        builder.Services.AddDbContext<GeritDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            var tenantInterceptor = serviceProvider.GetRequiredService<TenantSessionConnectionInterceptor>();
            var telemetryInterceptor = serviceProvider.GetRequiredService<TelemetryInterceptor>();
            options.AddInterceptors(tenantInterceptor, telemetryInterceptor);
        });
        builder.Services.AddJwt(builder.Configuration);
        builder.Services.AddRouteValidatorSetup();
        builder.Services.AddAuthorization();
        builder.Services.AddRateLimitingConfiguration(builder.Configuration);
        builder.Services.AddCorsConfiguration(builder.Configuration);

        builder.Services.AddGeritInfrastructure();

        builder.Services.AddScoped<ICurrentUserService, CurrentUserApiService>();
        builder.Services.AddScoped<ILocalizationService, LocalizationService>();

        var app = builder.Build();

        // Inicializar ServiceProviderHolder para uso em helpers estáticos
        ServiceProviderHolder.ServiceProvider = app.Services;

        // Habilita arquivos estáticos (custom.css e custom.js do Swagger)
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerConfiguration();
        }

        app.UseHttpsRedirection();

        // Rate Limiting (apenas se habilitado)
        var rateLimitingEnabled = builder.Configuration.GetValue<bool>("RateLimiting:EnableRateLimiting", true);
        if (rateLimitingEnabled)
        {
            app.UseRateLimiter();
        }

        // CORS (apenas se habilitado)
        var corsEnabled = builder.Configuration.GetValue<bool>("Cors:EnableCors", true);
        if (corsEnabled)
        {
            var policyName = builder.Configuration.GetValue<string>("Cors:PolicyName") ?? "GeritCorsPolicy";
            app.UseCors(policyName);
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapActionEndpoints();
        app.MapResourceEndpoints();
        app.MapRoleEndpoints();
        app.MapPlanEndpoints();
        app.MapTenantEndpoints();
        app.MapSubscriptionEndpoints();
        app.MapUserEndpoints();

        // Minimal API Endpoints
        app.MapGet("/health", () => Results.Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
        }))
        .WithName("HealthCheck")
        .WithTags("Health")
        .Produces(200)
        .WithSummary("Health Check")
        .WithDescription("Verifica se a API está funcionando corretamente");

        app.Run();
    }
}
