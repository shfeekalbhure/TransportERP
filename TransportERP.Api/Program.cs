using TransportERP.Api.Policies;
using TransportERP.Api.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Http.Resilience;

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

        // The typed IApiClient is the only downstream route.  Its standard resilience
        // handler owns the approved W2 policy; no independent retry handler is registered.
        builder.Services.AddHttpClient<Clients.IApiClient, Clients.ApiClient>(client =>
        {
            client.Timeout = OutgoingRequestResiliencePolicy.TotalRequestTimeout;
        }).AddStandardResilienceHandler(options =>
        {
            options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(30);
            options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
            options.Retry.MaxRetryAttempts = 3;
            options.Retry.Delay = TimeSpan.FromSeconds(2);
            options.Retry.BackoffType = DelayBackoffType.Exponential;
            options.Retry.UseJitter = true;
            // Conservative W2 default: the shared pipeline never retries unsafe verbs.
            // A future idempotency-key contract may opt in explicitly at its call site.
            options.Retry.DisableForUnsafeHttpMethods();
        });
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
