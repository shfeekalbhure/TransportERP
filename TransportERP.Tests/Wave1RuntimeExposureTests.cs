using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Wave1RuntimeExposureTests
{
    private const string TestConnection = "Host=localhost;Port=5432;Database=transporterp_wave1_route_probe;Username=postgres;Password=postgres";
    private const string Issuer = "TransportERP.Wave1.RouteProbe";
    private const string Audience = "TransportERP.Wave1.RouteProbe.Api";
    private const string SigningKey = "transport-erp-wave1-route-probe-signing-key-2026";

    [Fact]
    public void Governing_hold_routes_are_absent_from_the_runtime_endpoint_table()
    {
        using var factory = CreateFactory();
        var routes = GetRoutes(factory);

        var blockedPrefixes = new[]
        {
            "/api/v1/general/countries",
            "/api/v1/general/number-sequences",
            "/api/v1/general/number-reservations",
            "/api/v1/accounting/account-classifications",
            "/api/v1/accounting/reports/customer-aging",
            "/api/v1/accounting/reports/supplier-aging",
            "/api/v1/accounting/reports/cash-flow"
        };

        foreach (var prefix in blockedPrefixes)
            Assert.DoesNotContain(routes, route => route.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Review_required_wave1_routes_remain_exposed_after_hold_containment()
    {
        using var factory = CreateFactory();
        var routes = GetRoutes(factory).Select(Normalize).ToArray();

        var requiredRoutes = new[]
        {
            "/api/v1/general/governorates",
            "/api/v1/general/directorates",
            "/api/v1/general/cities",
            "/api/v1/general/areas",
            "/api/v1/general/languages",
            "/api/v1/accounting/reports/balance-sheet/query",
            "/api/v1/accounting/reports/detailed-trial-balance/query"
        };

        foreach (var required in requiredRoutes)
            Assert.Contains(Normalize(required), routes, StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void Held_reference_services_and_entities_are_not_part_of_the_runtime_container_or_model()
    {
        using var factory = CreateFactory();
        using var scope = factory.Services.CreateScope();

        Assert.Null(scope.ServiceProvider.GetService<Wave1ReferenceService>());
        Assert.Null(scope.ServiceProvider.GetService<Wave1FinancialReportService>());
        Assert.NotNull(scope.ServiceProvider.GetService<Wave1LanguageService>());

        var db = scope.ServiceProvider.GetRequiredService<Wave1ReferenceDbContext>();
        Assert.Null(db.Model.FindEntityType(typeof(Wave1AccountClassificationEntity)));
        Assert.Null(db.Model.FindEntityType(typeof(Wave1AccountingOpenItemEntity)));
        Assert.NotNull(db.Model.FindEntityType(typeof(Wave1LanguageEntity)));
    }

    private static WebApplicationFactory<Program> CreateFactory()
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", TestConnection);
            builder.UseSetting("Auth:Issuer", Issuer);
            builder.UseSetting("Auth:Audience", Audience);
            builder.UseSetting("Auth:SigningKey", SigningKey);
        });

    private static string[] GetRoutes(WebApplicationFactory<Program> factory)
        => factory.Services
            .GetServices<EndpointDataSource>()
            .SelectMany(source => source.Endpoints)
            .OfType<RouteEndpoint>()
            .Select(endpoint => endpoint.RoutePattern.RawText)
            .Where(route => !string.IsNullOrWhiteSpace(route))
            .Cast<string>()
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(route => route, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private static string Normalize(string route)
        => route.Length > 1 ? route.TrimEnd('/') : route;
}
