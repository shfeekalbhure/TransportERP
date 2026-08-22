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
    public void All_thirteen_owner_authorized_wave1_surfaces_are_registered()
    {
        using var factory = CreateFactory();
        var routes = GetRoutes(factory).Select(Normalize).ToArray();
        var required = new[]
        {
            "/api/v1/general/countries", "/api/v1/general/countries/print",
            "/api/v1/general/governorates", "/api/v1/general/directorates", "/api/v1/general/cities", "/api/v1/general/areas",
            "/api/v1/general/number-sequences", "/api/v1/general/number-reservations/{reservationId:guid}/commit",
            "/api/v1/general/languages", "/api/v1/accounting/account-classifications",
            "/api/v1/accounting/reports/customer-aging/query", "/api/v1/accounting/reports/supplier-aging/query",
            "/api/v1/accounting/reports/balance-sheet/query", "/api/v1/accounting/reports/cash-flow/query",
            "/api/v1/accounting/reports/detailed-trial-balance/query"
        };
        foreach (var route in required) Assert.Contains(Normalize(route), routes, StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void Authorized_services_are_registered_while_historical_mixed_services_remain_unregistered()
    {
        using var factory = CreateFactory(); using var scope = factory.Services.CreateScope();
        Assert.NotNull(scope.ServiceProvider.GetService<Wave1CountryAuthorityService>());
        Assert.NotNull(scope.ServiceProvider.GetService<Wave1NumberingAuthorityService>());
        Assert.NotNull(scope.ServiceProvider.GetService<Wave1AccountClassificationAuthorityService>());
        Assert.NotNull(scope.ServiceProvider.GetService<Wave1AgingAuthorityService>());
        Assert.NotNull(scope.ServiceProvider.GetService<Wave1CashFlowAuthorityService>());
        Assert.NotNull(scope.ServiceProvider.GetService<Wave1LanguageService>());
        Assert.Null(scope.ServiceProvider.GetService<Wave1ReferenceService>());
        Assert.Null(scope.ServiceProvider.GetService<Wave1FinancialReportService>());
        Assert.Null(scope.ServiceProvider.GetService<Wave1NumberingService>());
    }

    [Fact]
    public void Active_models_use_governing_entities_not_legacy_denormalized_hold_entities()
    {
        using var factory = CreateFactory(); using var scope = factory.Services.CreateScope();
        var reference = scope.ServiceProvider.GetRequiredService<Wave1ReferenceDbContext>();
        Assert.Null(reference.Model.FindEntityType(typeof(Wave1AccountClassificationEntity)));
        Assert.Null(reference.Model.FindEntityType(typeof(Wave1AccountingOpenItemEntity)));
        Assert.NotNull(reference.Model.FindEntityType(typeof(Wave1LanguageEntity)));

        var accounting = scope.ServiceProvider.GetRequiredService<Wave1AccountingAuthorityDbContext>();
        Assert.NotNull(accounting.Model.FindEntityType(typeof(Wave1AccountGroupRecord)));
        Assert.NotNull(accounting.Model.FindEntityType(typeof(Wave1AccountTypeRecord)));
        Assert.NotNull(accounting.Model.FindEntityType(typeof(Wave1OpenItemRecord)));
        Assert.NotNull(accounting.Model.FindEntityType(typeof(Wave1PaymentAllocationRecord)));
        Assert.NotNull(accounting.Model.FindEntityType(typeof(Wave1CashFlowAccountMappingRecord)));
        Assert.NotNull(accounting.Model.FindEntityType(typeof(AuditEvent)));
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
        => factory.Services.GetServices<EndpointDataSource>().SelectMany(x => x.Endpoints).OfType<RouteEndpoint>()
            .Select(x => x.RoutePattern.RawText).Where(x => !string.IsNullOrWhiteSpace(x)).Cast<string>()
            .Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToArray();
    private static string Normalize(string route) => route.Length > 1 ? route.TrimEnd('/') : route;
}
