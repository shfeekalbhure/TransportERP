using TransportERP.Contracts.Wave1;

namespace TransportERP.Api.Wave1;

public static class Wave1ScreenApiModule
{
    public static IEndpointRouteBuilder MapWave1ScreenCatalog(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/wave1").RequireAuthorization("Authenticated");

        group.MapGet("/screens", () => Results.Ok(Wave1ScreenCatalog.All));

        group.MapGet("/screens/{screenId}", (string screenId) =>
        {
            try
            {
                return Results.Ok(Wave1ScreenCatalog.GetRequired(screenId));
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound(new { ErrorCode = "WAVE1_SCREEN_NOT_FOUND", ScreenId = screenId });
            }
        });

        // Runtime registration is restricted to identities that are not in HOLD.
        // GEN-003 is withheld inside MapWave1Geography; GEN-004..007 remain reviewable.
        app.MapWave1Geography();
        // GEN-013 numbering is HOLD and intentionally not registered.
        app.MapWave1ReferenceMasters();
        // ACC-036 / ACC-050 / ACC-074 / ACC-075 remain HOLD and are intentionally not registered.
        // ACC-049 / ACC-058 are exposed only through dedicated report services after exact contract/E2E reconciliation.
        app.MapWave1BalanceSheet();
        app.MapWave1DetailedTrialBalance();
        return app;
    }
}
