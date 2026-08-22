using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TransportERP.Infrastructure.Persistence;

/// <summary>
/// Final runtime-model containment for the Wave1 reference context.
/// The historical migration chain may contain held artifacts, but the active model must not
/// expose ACC-036 or ACC-074/075 persistence until their governing W1 reconciliation closes.
/// </summary>
public sealed class Wave1ReferenceRuntimeModelCustomizer(ModelCustomizerDependencies dependencies)
    : ModelCustomizer(dependencies)
{
    public override void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        base.Customize(modelBuilder, context);
        modelBuilder.Ignore<Wave1AccountClassificationEntity>();
        modelBuilder.Ignore<Wave1AccountingOpenItemEntity>();
    }
}
