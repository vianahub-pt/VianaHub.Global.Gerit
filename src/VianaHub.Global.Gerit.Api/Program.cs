using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using VianaHub.Global.Gerit.Api.Configuration;
using VianaHub.Global.Gerit.Api.Configuration.Swagger;
using VianaHub.Global.Gerit.Api.Helpers;
using VianaHub.Global.Gerit.Api.Services;
using VianaHub.Global.Gerit.Domain.Interfaces;
using VianaHub.Global.Gerit.Infra.Data.Context;
using VianaHub.Global.Gerit.Infra.Data.Interceptors;
using VianaHub.Global.Gerit.Infra.IoC;
using Hangfire;
using Hangfire.SqlServer;
using VianaHub.Global.Gerit.Infra.Job.Services;
using VianaHub.Global.Gerit.Api.Security;
using VianaHub.Global.Gerit.Infra.Data.Tools;
using VianaHub.Global.Gerit.Api.Endpoints.Identity;
using VianaHub.Global.Gerit.Application.Mappings.Identity;
using VianaHub.Global.Gerit.Api.Endpoints.Billing;
using VianaHub.Global.Gerit.Api.Endpoints.Business;
using VianaHub.Global.Gerit.Api.Endpoints.Job;

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
        builder.Services.AddScoped<TenantSessionCommandInterceptor>();
        builder.Services.AddScoped<TelemetryInterceptor>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwagger(builder.Configuration, builder.Environment);

        builder.Services.AddAutoMapper(typeof(ActionMappingProfile).Assembly);
        builder.Services.AddDbContext<GeritDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            var tenantInterceptor = serviceProvider.GetRequiredService<TenantSessionConnectionInterceptor>();
            var tenantCommandInterceptor = serviceProvider.GetRequiredService<TenantSessionCommandInterceptor>();
            var telemetryInterceptor = serviceProvider.GetRequiredService<TelemetryInterceptor>();
            options.AddInterceptors(tenantInterceptor, tenantCommandInterceptor, telemetryInterceptor);
        });
        builder.Services.AddJwt(builder.Configuration);
        builder.Services.AddRouteValidatorSetup();
        // Add authorization and register named policies
        builder.Services.AddAuthorization(options =>
        {
            // 'BackOffice' policy used by endpoints. Requires authenticated user; adjust requirements as needed.
            options.AddPolicy("BackOffice", policy =>
            {
                policy.RequireAuthenticatedUser();
                // Additional requirements (roles/claims) can be added here, e.g.:
                // policy.RequireRole("Admin");
            });
        });
        builder.Services.AddRateLimitingConfiguration(builder.Configuration);
        builder.Services.AddCorsConfiguration(builder.Configuration);

        // Hangfire configuration - usa conexão separada se fornecida (HangfireConnection)
        var hangfireConn = builder.Configuration.GetConnectionString("HangfireConnection")
                          ?? builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddHangfire(configuration =>
        {
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                         .UseSimpleAssemblyNameTypeSerializer()
                         .UseRecommendedSerializerSettings()
                         .UseSqlServerStorage(hangfireConn, new SqlServerStorageOptions
                         {
                             SchemaName = "dbo",
                             PrepareSchemaIfNecessary = true,
                             QueuePollInterval = TimeSpan.FromSeconds(15)
                         });
        });

        builder.Services.AddHangfireServer();

        // Registrar infra services - chama o método que registra todos os serviços da aplicação
        builder.Services.AddGeritInfrastructure();

        // Localization service can be resolved from root during app startup (health endpoint)
        // and is thread-safe (uses internal cache), so register as singleton to avoid
        // 'Cannot resolve scoped service from root provider' errors.
        builder.Services.AddScoped<ICurrentUserService, CurrentUserApiService>();
        builder.Services.AddSingleton<ILocalizationService, LocalizationService>();

        var app = builder.Build();

        // Inicializar ServiceProviderHolder para uso em helpers estáticos
        ServiceProviderHolder.ServiceProvider = app.Services;

        // Garantir inicialização do banco de dados (migrations + validação do schema)
        try
        {
            var autoMigrate = builder.Configuration.GetValue<bool>("Database:AutoMigrate", true);
            // Executa de forma síncrona no startup
            app.InitializeDatabaseAsync(autoMigrate).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Falha ao inicializar o banco de dados");
            // Rethrow to fail fast
            throw;
        }

        // Habilita arquivos estáticos (custom.css e custom.js do Swagger)
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Show developer exception page in development to capture any startup/runtime errors
            app.UseDeveloperExceptionPage();

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

        // Hangfire dashboard (protegido por autorização em produção)
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
        });

        app.MapActionEndpoints();
        app.MapResourceEndpoints();
        app.MapRoleEndpoints();
        app.MapPlanEndpoints();
        app.MapTenantEndpoints();
        app.MapSubscriptionEndpoints();
        app.MapUserEndpoints();
        app.MapUserRoleEndpoints();
        app.MapRolePermissionEndpoints();
        app.MapAuthEndpoints();
        app.MapJobEndpoints();
        app.MapJwtKeyEndpoints();
        app.MapFunctionEndpoints();
        app.MapEquipmentEndpoints();
        app.MapVehicleEndpoints();

        // Minimal API Endpoints
        var localization = app.Services.GetService(typeof(ILocalizationService)) as ILocalizationService;

        app.MapGet("/health", () => Results.Ok(new
        {
            Status = localization?.GetMessage("Api.Health.Status") ?? "Api.Health.Status",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
        }))
        .WithName("HealthCheck")
        .WithTags("Health")
        .Produces(200)
        .WithSummary(localization?.GetMessage("Api.Health.Summary") ?? "Api.Health.Summary")
        .WithDescription(localization?.GetMessage("Api.Health.Description") ?? "Api.Health.Description");

        app.Run();
    }
}
