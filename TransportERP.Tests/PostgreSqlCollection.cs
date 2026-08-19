using Xunit;

namespace TransportERP.Tests;

[CollectionDefinition("PostgreSql", DisableParallelization = true)]
public sealed class PostgreSqlCollectionDefinition
{
}

// All PostgreSQL integration test classes declare [Collection("PostgreSql")].
// DisableParallelization prevents concurrent MigrateAsync calls against the shared
// TRANSPORTERP_TEST_CONNSTR database; in-memory tests remain independently parallelizable.
