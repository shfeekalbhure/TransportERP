using TransportERP.Api.Policies;
using TransportERP.Api.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TransportERP.Application.Geo;
using TransportERP.Infrastructure.Geo;

namespace TransportERP.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        var jwtBearerSection = builder.Configuration.GetSection("Authentication:JwtBearer");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                jwtBearerSection.Bind(options);

                // The claim policy remains unchanged. This merely binds the configured trust
                // material so the API can validate a server-issued JWT before the policy runs.
                var signingKey = jwtBearerSection["SigningKey"];
                if (!string.IsNullOrWhiteSpace(signingKey))
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtBearerSection["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwtBearerSection["Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                }
            });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(LookupClaims.ReadPolicy, policy =>
                policy.Requirements.Add(new LookupReadRequirement()));
        });
        builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, LookupReadAuthorizationHandler>();

        var geoConnection = builder.Configuration.GetConnectionString("TransportERP")
            ?? builder.Configuration["ConnectionStrings:TransportERP"]
            ?? "Server=localhost;Database=transporterp;User=root;Password=;";
        // Server version is fixed here so API startup never probes a database; migrations and
        // connection validation remain explicit development/test operations.
        builder.Services.AddDbContext<TransportErpDbContext>(options =>
            options.UseMySql(geoConnection, new MySqlServerVersion(new Version(8, 0, 0))));
        builder.Services.AddScoped<IGeoRepository, EfGeoRepository>();
        builder.Services.AddScoped<IGeoService, GeoService>();
        builder.Services.AddSingleton<IGeoAuditSink, GeoAuditSink>();

        // All outbound API traffic uses the typed IApiClient and this resilience handler.
        builder.Services.AddTransient<SafeReadRetryHandler>();
        builder.Services.AddSingleton<IResilienceDelay, SystemResilienceDelay>();
        builder.Services.AddHttpClient<Clients.IApiClient, Clients.ApiClient>(client =>
        {
            client.Timeout = OutgoingRequestResiliencePolicy.TotalRequestTimeout;
        }).AddHttpMessageHandler<SafeReadRetryHandler>();
        builder.Services.AddScoped<Services.IDownstreamStatusService, Services.DownstreamStatusService>();
        builder.Services.AddSingleton<ReferenceData.IReferenceLookupProvider, ReferenceData.InMemoryReferenceLookupProvider>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
