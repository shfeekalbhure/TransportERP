using TransportERP.Api.Policies;

namespace TransportERP.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        // All outbound API traffic uses the typed IApiClient and this resilience handler.
        builder.Services.AddTransient<SafeReadRetryHandler>();
        builder.Services.AddHttpClient<Clients.IApiClient, Clients.ApiClient>(client =>
        {
            client.Timeout = OutgoingRequestResiliencePolicy.TotalRequestTimeout;
        }).AddHttpMessageHandler<SafeReadRetryHandler>();
        builder.Services.AddSingleton<ReferenceData.IReferenceLookupProvider, ReferenceData.InMemoryReferenceLookupProvider>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
