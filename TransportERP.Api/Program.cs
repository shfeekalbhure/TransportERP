using TransportERP.Api.Policies;

namespace TransportERP.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        // Outbound calls must use this named client. It has a 15-second timeout and retries
        // only safe read methods through SafeReadRetryHandler.
        builder.Services.AddTransient<SafeReadRetryHandler>();
        builder.Services.AddHttpClient("TransportERP.SafeReadClient", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(OutgoingRequestResiliencePolicy.TimeoutSeconds);
        }).AddHttpMessageHandler<SafeReadRetryHandler>();

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
