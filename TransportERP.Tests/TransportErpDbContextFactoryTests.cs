using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("Environment variables")]
public sealed class TransportErpDbContextFactoryTests
{
    [Fact]
    public void CreateDbContext_WithoutDesignConnectionString_FailsClosed()
    {
        const string variableName = "TRANSPORTERP_DESIGN_CONNSTR";
        var originalValue = Environment.GetEnvironmentVariable(variableName);

        try
        {
            Environment.SetEnvironmentVariable(variableName, null);

            var exception = Assert.Throws<InvalidOperationException>(
                () => new TransportErpDbContextFactory().CreateDbContext([]));

            Assert.Equal(
                "TRANSPORTERP_DESIGN_CONNSTR must be set for EF Core design-time operations.",
                exception.Message);
        }
        finally
        {
            Environment.SetEnvironmentVariable(variableName, originalValue);
        }
    }
}

[CollectionDefinition("Environment variables", DisableParallelization = true)]
public sealed class EnvironmentVariableCollectionDefinition
{
}
