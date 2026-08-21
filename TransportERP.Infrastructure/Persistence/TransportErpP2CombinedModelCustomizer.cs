using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TransportERP.Infrastructure.Persistence;

/// <summary>
/// Composes the closed P2-C01-A foundation, P2-C01-B finance model, and the additive
/// P2-C01-C release/trip/manifest execution model.
/// </summary>
public sealed class TransportErpP2CombinedModelCustomizer(ModelCustomizerDependencies dependencies)
    : ModelCustomizer(dependencies)
{
    private readonly TransportErpP2ModelCustomizer _foundation = new(dependencies);

    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        _foundation.Customize(modelBuilder, context);
        TransportErpP2FinanceModel.Configure(modelBuilder);
        TransportErpP2ShippingModel.Configure(modelBuilder);
    }
}
