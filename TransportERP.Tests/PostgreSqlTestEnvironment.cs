namespace TransportERP.Tests;

internal static class PostgreSqlTestEnvironment
{
    internal static string RequireConnection()
    {
        var connection = Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR")
            ?? Environment.GetEnvironmentVariable("TRANSPORTERP_P1_POSTGRES_CONNECTION");

        if (string.IsNullOrWhiteSpace(connection))
        {
            throw new InvalidOperationException(
                "PostgreSQL integration tests require TRANSPORTERP_TEST_CONNSTR " +
                "or TRANSPORTERP_P1_POSTGRES_CONNECTION. The test is fail-closed and cannot be silently skipped.");
        }

        return connection;
    }
}
