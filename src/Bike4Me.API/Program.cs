using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Bike4Me.API.Configurations;
using Bike4Me.API.Extensions;
using Bike4Me.API.Infrastructure;
using Bike4Me.API.OpenApi;
using Bike4Me.API.OutputCaching;
using Bike4Me.Application.Extensions;
using Bike4Me.Infrastructure.Database.Seed;
using Bike4Me.Infrastructure.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.OutputCaching;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddJwtAuthentication(builder.Configuration);

// Serilog
builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },

            Array.Empty<string>()
        }
    });
});

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddStackExchangeRedisOutputCache(o =>
{
    o.Configuration = builder.Configuration.GetConnectionString("Cache");
    o.InstanceName = "bike4me-";
});

builder.Services.AddOutputCache(o =>
{
    o.AddPolicy("CustomPerUser", b => b
        .AddPolicy<CustomPolicy>()
        .SetCacheKeyPrefix("custom-")
        .Tag("all")
        .Expire(TimeSpan.FromMinutes(10))
        .VaryByValue((ctx, _) =>
        {
            var key = nameof(ClaimsPrincipalExtensions.UserId);
            var val = ctx.User.UserId().ToString();
            return ValueTask.FromResult(new KeyValuePair<string, string>(key, val));
        })
    );
});

WebApplication app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

app.Use(async (ctx, next) =>
{
    await next();

    var feat = ctx.Features.Get<IOutputCacheFeature>();

    Console.WriteLine($"[CACHE-TRACE] Path={ctx.Request.Path}  " +
                      $"HasStarted={ctx.Response.HasStarted}  " +
                      $"AllowCacheStorage={feat?.Context.AllowCacheStorage}");
});

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .ReportApiVersions()
    .Build();

RouteGroupBuilder versionedGroup = app
    .MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

app.MapEndpoints(versionedGroup);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

        foreach (ApiVersionDescription description in descriptions)
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
        }
    });

    app.ApplyMigrations();
    await app.SeedDatabaseAsync(new Bike4MeContextSeed());
}

app.UseHttpsRedirection();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

await app.ConfigureEventBus();

await app.RunAsync();

public partial class Program;