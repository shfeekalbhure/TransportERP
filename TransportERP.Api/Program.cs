using TransportERP.Api.Policies;
using TransportERP.Api.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TransportERP.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => builder.Configuration.GetSection("Authentication:JwtBearer").Bind(options));
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(LookupClaims.ReadPolicy, policy =>
                policy.Requirements.Add(new LookupReadRequirement()));
        });
        builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, LookupReadAuthorizationHandler>();

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
