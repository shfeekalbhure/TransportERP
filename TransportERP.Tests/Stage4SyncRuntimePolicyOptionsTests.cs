using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransportERP.Api.Sync;
using TransportERP.Application.Sync;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Stage4SyncRuntimePolicyOptionsTests
{
    [Fact]
    public void Complete_global_ceiling_is_valid_and_explicitly_offline_closed()
    {
        var options = SyncRuntimePolicyOptions.Load(Configuration(RequiredSettings()));

        var result = new SyncRuntimePolicyOptionsValidator().Validate(null, options);

        Assert.True(result.Succeeded);
        Assert.False(options.OfflineEnabled!.Value);
        Assert.False(options.ServerExecutionEnabled!.Value);
        Assert.Equal(new[] { "sync-v1" }, options.AllowedProtocolVersions);
        Assert.Equal(SyncApiModule.MaximumBatchOperations, options.MaxBatchOperations);
        Assert.Equal(SyncApiModule.MaximumRequestBodyBytes, options.MaximumRequestBodyBytes);
        Assert.Equal(SyncApiModule.MaximumPayloadBytes, options.MaximumPayloadBytes);
    }

    [Theory]
    [InlineData("Sync:Offline:Enabled")]
    [InlineData("Sync:ServerExecution:Enabled")]
    [InlineData("Sync:Protocol:AllowedVersions:0")]
    [InlineData("Sync:Batch:MaxOperations")]
    [InlineData("Sync:Retry:ServerExecution:MaxCount")]
    [InlineData("Sync:Proof:MaximumRequestBodyBytes")]
    [InlineData("Sync:Proof:MaximumPayloadBytes")]
    public void Missing_governed_setting_fails_closed(string missingKey)
    {
        var values = RequiredSettings();
        values.Remove(missingKey);

        var result = new SyncRuntimePolicyOptionsValidator().Validate(null,
            SyncRuntimePolicyOptions.Load(Configuration(values)));

        Assert.True(result.Failed);
    }

    [Fact]
    public void Configuration_cannot_silently_enable_offline()
    {
        var values = RequiredSettings();
        values["Sync:Offline:Enabled"] = "true";

        var result = new SyncRuntimePolicyOptionsValidator().Validate(null,
            SyncRuntimePolicyOptions.Load(Configuration(values)));

        Assert.True(result.Failed);
        Assert.Contains(result.Failures, x => x.Contains("G5", StringComparison.Ordinal));
    }

    [Fact]
    public void Server_execution_worker_is_disabled_by_default_but_composition_is_complete()
    {
        var options = SyncRuntimePolicyOptions.Load(Configuration(RequiredSettings()));
        var services = new ServiceCollection();

        services.AddSyncBusinessExecution(options.ServerExecutionEnabled == true);

        Assert.DoesNotContain(services, descriptor =>
            descriptor.ServiceType == typeof(IHostedService) &&
            descriptor.ImplementationType == typeof(SyncExecutionWorker));
        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(ISyncActionExecutor));
        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(SyncExecutionProcessor));
    }

    [Fact]
    public void Server_execution_worker_is_registered_only_for_explicit_true()
    {
        var disabled = new ServiceCollection();
        var enabled = new ServiceCollection();

        disabled.AddSyncBusinessExecution(false);
        enabled.AddSyncBusinessExecution(true);

        Assert.DoesNotContain(disabled, x => x.ServiceType == typeof(IHostedService));
        Assert.Contains(enabled, x => x.ServiceType == typeof(IHostedService) &&
            x.ImplementationType == typeof(SyncExecutionWorker));
    }

    [Theory]
    [InlineData("sync-v2")]
    [InlineData("SYNC-V1")]
    [InlineData("")]
    public void Protocol_allowlist_accepts_only_exact_sync_v1(string version)
    {
        var values = RequiredSettings();
        values["Sync:Protocol:AllowedVersions:0"] = version;

        Assert.True(new SyncRuntimePolicyOptionsValidator().Validate(null,
            SyncRuntimePolicyOptions.Load(Configuration(values))).Failed);
    }

    [Fact]
    public void Unknown_or_duplicate_action_fails_global_configuration()
    {
        var unknown = WithAllowedActions(ValidOptions(), ["UnknownAction"]);
        var duplicate = WithAllowedActions(ValidOptions(), ["CreateWaybillDraft", "CreateWaybillDraft"]);
        var validator = new SyncRuntimePolicyOptionsValidator();

        Assert.True(validator.Validate(null, unknown).Failed);
        Assert.True(validator.Validate(null, duplicate).Failed);
    }

    [Fact]
    public void Company_then_branch_then_device_and_permission_only_narrow_actions()
    {
        var resolver = Resolver(WithAllowedActions(ValidOptions(offlineEnabled: true),
            ["CreateWaybillDraft", "UpdateWaybillDraft", "RecordCollection"]));

        var effective = resolver.Resolve(
            new SyncPolicyRestriction(AllowedActions: ["CreateWaybillDraft", "UpdateWaybillDraft"]),
            new SyncPolicyRestriction(AllowedActions: ["UpdateWaybillDraft"]),
            ["CreateWaybillDraft", "UpdateWaybillDraft", "RecordCollection"],
            ["UpdateWaybillDraft", "RecordCollection"]);

        Assert.True(effective.Enabled);
        Assert.Equal("UpdateWaybillDraft", Assert.Single(effective.AllowedActions));
        Assert.Null(effective.ClosedReason);
    }

    [Fact]
    public void Invalid_lower_scope_widening_disables_scope_without_fallback()
    {
        var resolver = Resolver(WithAllowedActions(ValidOptions(offlineEnabled: true), ["CreateWaybillDraft"]));

        var effective = resolver.Resolve(
            new SyncPolicyRestriction(AllowedActions: ["CreateWaybillDraft", "RecordCollection"]),
            null,
            ["CreateWaybillDraft"],
            ["CreateWaybillDraft"]);

        Assert.False(effective.Enabled);
        Assert.Empty(effective.AllowedActions);
        Assert.Equal("INVALID_SCOPE_OVERRIDE", effective.ClosedReason);
    }

    [Fact]
    public void Lower_scope_cannot_raise_batch_or_retry_count_or_reduce_base_delay()
    {
        var resolver = Resolver(ValidOptions(offlineEnabled: true));
        var widening = new[]
        {
            new SyncPolicyRestriction(MaxBatchOperations: 101),
            new SyncPolicyRestriction(MaximumPayloadBytes: 16_385),
            new SyncPolicyRestriction(ServerExecutionMaxRetryCount: 6),
            new SyncPolicyRestriction(ServerExecutionBaseSeconds: 4),
            new SyncPolicyRestriction(ServerExecutionMaxDelayMinutes: 29)
        };

        foreach (var invalid in widening)
        {
            var effective = resolver.Resolve(invalid, null,
                SyncActionCatalog.Definitions.Select(x => x.ActionCodeValue).ToArray(),
                SyncActionCatalog.Definitions.Select(x => x.ActionCodeValue).ToArray());
            Assert.False(effective.Enabled);
            Assert.Equal("INVALID_SCOPE_OVERRIDE", effective.ClosedReason);
        }
    }

    [Fact]
    public void Device_and_permission_policies_are_mandatory_final_intersections()
    {
        var resolver = Resolver(WithAllowedActions(ValidOptions(offlineEnabled: true),
            ["CreateWaybillDraft", "UpdateWaybillDraft"]));

        var missing = resolver.Resolve(null, null, null, ["CreateWaybillDraft"]);
        var noIntersection = resolver.Resolve(null, null,
            ["CreateWaybillDraft"], ["UpdateWaybillDraft"]);

        Assert.Equal("DEVICE_OR_PERMISSION_POLICY_MISSING", missing.ClosedReason);
        Assert.False(noIntersection.Enabled);
        Assert.Empty(noIntersection.AllowedActions);
        Assert.Equal("OFFLINE_DISABLED", noIntersection.ClosedReason);
    }

    [Fact]
    public void Empty_protocol_intersection_closes_the_scope()
    {
        var resolver = Resolver(WithAllowedActions(ValidOptions(offlineEnabled: true), ["CreateWaybillDraft"]));

        var effective = resolver.Resolve(
            new SyncPolicyRestriction(AllowedProtocolVersions: []),
            null,
            ["CreateWaybillDraft"],
            ["CreateWaybillDraft"]);

        Assert.False(effective.Enabled);
        Assert.Empty(effective.AllowedProtocolVersions);
        Assert.Equal("OFFLINE_DISABLED", effective.ClosedReason);
    }

    [Fact]
    public void Company_or_branch_cannot_reopen_a_closed_global_gate()
    {
        var resolver = Resolver(WithAllowedActions(ValidOptions(offlineEnabled: false), ["CreateWaybillDraft"]));

        var effective = resolver.Resolve(
            new SyncPolicyRestriction(Enabled: true),
            new SyncPolicyRestriction(Enabled: true),
            ["CreateWaybillDraft"],
            ["CreateWaybillDraft"]);

        Assert.False(effective.Enabled);
        Assert.Equal("OFFLINE_DISABLED", effective.ClosedReason);
    }

    private static IConfiguration Configuration(IReadOnlyDictionary<string, string?> values)
        => new ConfigurationBuilder().AddInMemoryCollection(values).Build();

    private static SyncEffectivePolicyResolver Resolver(SyncRuntimePolicyOptions options)
        => new(Options.Create(options));

    private static SyncRuntimePolicyOptions ValidOptions(bool offlineEnabled = false) => new()
    {
        OfflineEnabled = offlineEnabled,
        ServerExecutionEnabled = false,
        AllowedActions = SyncActionCatalog.Definitions.Select(x => x.ActionCodeValue).ToArray(),
        AllowedProtocolVersions = ["sync-v1"],
        ClientTransportMaxRetryCount = 5,
        ClientTransportBaseSeconds = 5,
        ClientTransportMaxDelayMinutes = 30,
        ServerExecutionMaxRetryCount = 5,
        ServerExecutionBaseSeconds = 5,
        ServerExecutionMaxDelayMinutes = 30,
        MaxBatchOperations = 100,
        ConflictAutoMerge = false,
        LocalSuccessHours = 24,
        LocalRejectedDays = 7,
        ServerPayloadDays = 90,
        CacheMaxAgeHours = 24,
        MaximumRequestBodyBytes = 2_097_152,
        MaximumPayloadBytes = 16_384
    };

    private static SyncRuntimePolicyOptions WithAllowedActions(
        SyncRuntimePolicyOptions source,
        string[] actions) => new()
    {
        OfflineEnabled = source.OfflineEnabled,
        ServerExecutionEnabled = source.ServerExecutionEnabled,
        AllowedActions = actions,
        AllowedProtocolVersions = source.AllowedProtocolVersions,
        ClientTransportMaxRetryCount = source.ClientTransportMaxRetryCount,
        ClientTransportBaseSeconds = source.ClientTransportBaseSeconds,
        ClientTransportMaxDelayMinutes = source.ClientTransportMaxDelayMinutes,
        ServerExecutionMaxRetryCount = source.ServerExecutionMaxRetryCount,
        ServerExecutionBaseSeconds = source.ServerExecutionBaseSeconds,
        ServerExecutionMaxDelayMinutes = source.ServerExecutionMaxDelayMinutes,
        MaxBatchOperations = source.MaxBatchOperations,
        ConflictAutoMerge = source.ConflictAutoMerge,
        LocalSuccessHours = source.LocalSuccessHours,
        LocalRejectedDays = source.LocalRejectedDays,
        ServerPayloadDays = source.ServerPayloadDays,
        CacheMaxAgeHours = source.CacheMaxAgeHours,
        MaximumRequestBodyBytes = source.MaximumRequestBodyBytes,
        MaximumPayloadBytes = source.MaximumPayloadBytes
    };

    private static Dictionary<string, string?> RequiredSettings()
    {
        var values = new Dictionary<string, string?>
        {
            ["Sync:Offline:Enabled"] = "false",
            ["Sync:ServerExecution:Enabled"] = "false",
            ["Sync:Protocol:AllowedVersions:0"] = "sync-v1",
            ["Sync:Retry:ClientTransport:MaxCount"] = "5",
            ["Sync:Retry:ClientTransport:BaseSeconds"] = "5",
            ["Sync:Retry:ClientTransport:MaxDelayMinutes"] = "30",
            ["Sync:Retry:ServerExecution:MaxCount"] = "5",
            ["Sync:Retry:ServerExecution:BaseSeconds"] = "5",
            ["Sync:Retry:ServerExecution:MaxDelayMinutes"] = "30",
            ["Sync:Batch:MaxOperations"] = "100",
            ["Sync:Conflict:AutoMerge"] = "false",
            ["Sync:Retention:LocalSuccessHours"] = "24",
            ["Sync:Retention:LocalRejectedDays"] = "7",
            ["Sync:Retention:ServerPayloadDays"] = "90",
            ["Sync:Cache:MaxAgeHours"] = "24",
            ["Sync:Proof:MaximumRequestBodyBytes"] = "2097152",
            ["Sync:Proof:MaximumPayloadBytes"] = "16384"
        };
        var index = 0;
        foreach (var action in SyncActionCatalog.Definitions)
            values[$"Sync:Offline:AllowedActions:{index++}"] = action.ActionCodeValue;
        return values;
    }
}
