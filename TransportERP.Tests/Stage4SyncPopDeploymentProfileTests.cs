using Microsoft.Extensions.Configuration;
using TransportERP.Api.Sync;

namespace TransportERP.Tests;

public sealed class Stage4SyncPopDeploymentProfileTests
{
    [Fact]
    public void Direct_https_profile_is_valid_and_does_not_trust_forwarded_headers()
    {
        var configuration = Configuration(RequiredSettings());

        var profile = SyncPopDeploymentProfile.Load(configuration);

        Assert.True(profile.IsValid);
        Assert.False(profile.ForwardedHeadersEnabled);
        Assert.Equal("https://sync.example.test/api/v1/sync/operations:batch", profile.CanonicalHtu);
    }

    [Fact]
    public void Conflict_canonical_htu_uses_configured_public_origin_and_exact_route_not_request_host()
    {
        var values = RequiredSettings();
        values["Sync:Proof:PublicOrigin"] = "https://sync.example.test:8443";
        var profile = SyncPopDeploymentProfile.Load(Configuration(values));
        var conflictId = Guid.Parse("aaaaaaaa-bbbb-4ccc-8ddd-eeeeeeeeeeee");

        Assert.Equal(
            $"https://sync.example.test:8443/api/v1/sync/conflicts/{conflictId:D}:resolve",
            profile.CanonicalHtuForPath($"/api/v1/sync/conflicts/{conflictId:D}:resolve"));
        Assert.Null(profile.CanonicalHtuForPath("/api/v1/sync/conflicts/x:resolve?host=attacker.invalid"));
    }

    [Theory]
    [InlineData("Sync:Proof:MaximumPastSeconds")]
    [InlineData("Sync:Proof:MaximumFutureSeconds")]
    [InlineData("Sync:Proof:NonceLifetimeSeconds")]
    [InlineData("Sync:Proof:ReplayRetentionSeconds")]
    [InlineData("Sync:Proof:MaximumRequestBodyBytes")]
    [InlineData("Sync:Proof:MaximumPayloadBytes")]
    public void Profile_rejects_a_missing_mandatory_security_setting(string missingKey)
    {
        var values = RequiredSettings();
        values.Remove(missingKey);

        Assert.False(SyncPopDeploymentProfile.Load(Configuration(values)).IsValid);
    }

    [Theory]
    [InlineData("*")]
    [InlineData("")]
    public void Proxy_profile_rejects_wildcard_or_missing_allowed_hosts(string allowedHosts)
    {
        var values = RequiredSettings();
        values["Sync:Proof:ForwardedHeadersEnabled"] = "true";
        values["Sync:Proof:ForwardLimit"] = "1";
        values["Sync:Proof:KnownProxies:0"] = "10.0.0.10";
        values["AllowedHosts"] = allowedHosts;
        var configuration = Configuration(values);

        Assert.False(SyncPopDeploymentProfile.Load(configuration).IsValid);
    }

    [Fact]
    public void Proxy_profile_requires_explicit_trusted_proxy_or_network()
    {
        var values = RequiredSettings();
        values["Sync:Proof:ForwardedHeadersEnabled"] = "true";
        values["Sync:Proof:ForwardLimit"] = "1";
        values["AllowedHosts"] = "sync.example.test";
        var configuration = Configuration(values);

        Assert.False(SyncPopDeploymentProfile.Load(configuration).IsValid);
    }

    [Fact]
    public void Proxy_profile_requires_an_explicit_forward_limit()
    {
        var values = RequiredSettings();
        values["Sync:Proof:ForwardedHeadersEnabled"] = "true";
        values["Sync:Proof:KnownProxies:0"] = "10.0.0.10";
        values["AllowedHosts"] = "sync.example.test";

        Assert.False(SyncPopDeploymentProfile.Load(Configuration(values)).IsValid);
    }

    [Fact]
    public void Proxy_profile_with_explicit_bounded_topology_is_valid()
    {
        var values = RequiredSettings();
        values["Sync:Proof:ForwardedHeadersEnabled"] = "true";
        values["Sync:Proof:ForwardLimit"] = "1";
        values["Sync:Proof:KnownProxies:0"] = "10.0.0.10";
        values["AllowedHosts"] = "sync.example.test";

        var profile = SyncPopDeploymentProfile.Load(Configuration(values));

        Assert.True(profile.IsValid);
        Assert.True(profile.ForwardedHeadersEnabled);
        Assert.Equal(1, profile.ForwardLimit);
    }

    private static IConfiguration Configuration(IReadOnlyDictionary<string, string?> values)
        => new ConfigurationBuilder().AddInMemoryCollection(values).Build();

    private static Dictionary<string, string?> RequiredSettings() => new()
    {
        ["Sync:Proof:PublicOrigin"] = "https://sync.example.test",
        ["Sync:Proof:MaximumPastSeconds"] = "120",
        ["Sync:Proof:MaximumFutureSeconds"] = "30",
        ["Sync:Proof:NonceLifetimeSeconds"] = "300",
        ["Sync:Proof:ReplayRetentionSeconds"] = "600",
        ["Sync:Proof:MaximumRequestBodyBytes"] = "2097152",
        ["Sync:Proof:MaximumPayloadBytes"] = "16384"
    };
}
