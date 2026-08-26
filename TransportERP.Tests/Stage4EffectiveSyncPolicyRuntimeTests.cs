using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TransportERP.Api.Security;
using TransportERP.Api.Sync;
using TransportERP.Application.Sync;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Stage4EffectiveSyncPolicyRuntimeTests
{
    [Fact]
    public async Task Missing_device_policy_in_immutable_source_fails_closed()
    {
        var scope = Scope();
        var configuration = Configuration(
            scope,
            includeCompany: true,
            includeBranch: true,
            includeDevice: false);
        var provider = Provider(configuration,
            new HashSet<string>(["CreateWaybillDraft"], StringComparer.Ordinal));

        var effective = await provider.ResolveAsync(scope.Current);

        Assert.False(effective.Enabled);
        Assert.Empty(effective.AllowedActions);
        Assert.Equal("SYNC_SCOPE_POLICY_MISSING", effective.ClosedReason);
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public async Task Missing_company_or_branch_restriction_falls_back_toward_global(
        bool includeCompany,
        bool includeBranch)
    {
        var scope = Scope();
        var configuration = Configuration(scope, includeCompany, includeBranch, includeDevice: true);
        var provider = Provider(configuration,
            new HashSet<string>(["CreateWaybillDraft"], StringComparer.Ordinal));

        var effective = await provider.ResolveAsync(scope.Current);

        Assert.True(effective.Enabled);
        Assert.Contains("CreateWaybillDraft", effective.AllowedActions);
        Assert.Null(effective.ClosedReason);
    }

    [Theory]
    [InlineData("company")]
    [InlineData("branch")]
    [InlineData("device")]
    public void Startup_validation_rejects_every_widening_or_unknown_lower_scope(string level)
    {
        var scope = Scope();
        var company = new SyncPolicyRestriction(
            AllowedActions: level == "company"
                ? ["CreateWaybillDraft", "UnknownAction"]
                : ["CreateWaybillDraft", "UpdateWaybillDraft"],
            MaxBatchOperations: 50);
        var branch = new SyncPolicyRestriction(
            AllowedActions: level == "branch"
                ? ["CreateWaybillDraft", "UpdateWaybillDraft", "RecordCollection"]
                : ["CreateWaybillDraft"],
            MaxBatchOperations: level == "branch" ? 51 : 25);
        var deviceActions = level == "device"
            ? new HashSet<string>(["CreateWaybillDraft", "UpdateWaybillDraft"], StringComparer.Ordinal)
            : new HashSet<string>(["CreateWaybillDraft"], StringComparer.Ordinal);
        var configuration = EffectivePolicyConfiguration.Create(
            new Dictionary<Guid, SyncPolicyRestriction> { [scope.CompanyId] = company },
            new Dictionary<(Guid, Guid), SyncPolicyRestriction> { [(scope.CompanyId, scope.BranchId)] = branch },
            [new ConfiguredDeviceSyncPolicy(
                scope.RegisteredDeviceId, scope.CompanyId, scope.BranchId, scope.DeviceId, deviceActions)]);
        var options = Options.Create(Global(offlineEnabled: true));

        var result = new EffectivePolicyConfigurationValidator(
            new SyncEffectivePolicyResolver(options), options).Validate(configuration);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Configuration_source_binds_exact_company_branch_and_device_scope()
    {
        var scope = Scope();
        var values = new Dictionary<string, string?>
        {
            ["Sync:EffectivePolicy:SourceVersion"] = "tenant-policy-v7",
            [$"Sync:EffectivePolicy:Companies:{scope.CompanyId:D}:MaxBatchOperations"] = "50",
            [$"Sync:EffectivePolicy:Companies:{scope.CompanyId:D}:AllowedActions:0"] = "CreateWaybillDraft",
            [$"Sync:EffectivePolicy:Branches:{scope.CompanyId:D}:{scope.BranchId:D}:MaxBatchOperations"] = "25",
            [$"Sync:EffectivePolicy:Branches:{scope.CompanyId:D}:{scope.BranchId:D}:AllowedActions:0"] = "CreateWaybillDraft",
            [$"Sync:EffectivePolicy:Devices:{scope.RegisteredDeviceId:D}:CompanyId"] = scope.CompanyId.ToString("D"),
            [$"Sync:EffectivePolicy:Devices:{scope.RegisteredDeviceId:D}:BranchId"] = scope.BranchId.ToString("D"),
            [$"Sync:EffectivePolicy:Devices:{scope.RegisteredDeviceId:D}:DeviceId"] = scope.DeviceId,
            [$"Sync:EffectivePolicy:Devices:{scope.RegisteredDeviceId:D}:AllowedActions:0"] = "CreateWaybillDraft"
        };

        var source = EffectivePolicyConfiguration.Load(
            new ConfigurationBuilder().AddInMemoryCollection(values).Build());

        Assert.Empty(source.LoadErrors);
        Assert.Equal("tenant-policy-v7", source.SourceVersion);
        Assert.Matches("^[0-9a-f]{64}$", source.SourceFingerprint);
        Assert.True(source.TryGetCompany(scope.CompanyId, out var company));
        Assert.Equal(50, company.MaxBatchOperations);
        Assert.True(source.TryGetBranch(scope.CompanyId, scope.BranchId, out var branch));
        Assert.Equal(25, branch.MaxBatchOperations);
        Assert.True(source.TryGetDevice(scope.RegisteredDeviceId, out var device));
        Assert.Equal(scope.DeviceId, device.DeviceId);
    }

    [Fact]
    public void Malformed_configuration_scope_key_fails_startup_validation()
    {
        var values = new Dictionary<string, string?>
        {
            ["Sync:EffectivePolicy:Companies:not-a-guid:MaxBatchOperations"] = "50"
        };
        var source = EffectivePolicyConfiguration.Load(
            new ConfigurationBuilder().AddInMemoryCollection(values).Build());
        var options = Options.Create(Global(offlineEnabled: false));

        var result = new EffectivePolicyConfigurationValidator(
            new SyncEffectivePolicyResolver(options), options).Validate(source);

        Assert.True(result.Failed);
        Assert.Contains(result.Failures, x => x.Contains("non-empty company UUID", StringComparison.Ordinal));
    }

    [Fact]
    public void Missing_policy_source_version_fails_startup_validation()
    {
        var source = EffectivePolicyConfiguration.Load(
            new ConfigurationBuilder().AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Sync:Offline:Enabled"] = "false"
                }).Build());
        var options = Options.Create(Global(offlineEnabled: false));

        var result = new EffectivePolicyConfigurationValidator(
            new SyncEffectivePolicyResolver(options), options).Validate(source);

        Assert.True(result.Failed);
        Assert.Contains(result.Failures, x => x.Contains("SourceVersion", StringComparison.Ordinal));
    }

    [Theory]
    [InlineData(false, true, false)]
    [InlineData(true, false, true)]
    public void Lower_scope_without_optional_parent_uses_fallback_and_validates(
        bool includeCompany,
        bool includeBranch,
        bool includeDevice)
    {
        var scope = Scope();
        var source = Configuration(scope, includeCompany, includeBranch, includeDevice);
        var options = Options.Create(Global(offlineEnabled: false));

        var result = new EffectivePolicyConfigurationValidator(
            new SyncEffectivePolicyResolver(options), options).Validate(source);

        Assert.False(result.Failed);
    }

    [Fact]
    public async Task Provider_applies_global_company_branch_device_then_live_permission_intersection()
    {
        var scope = Scope();
        var configuration = EffectivePolicyConfiguration.Create(
            new Dictionary<Guid, SyncPolicyRestriction>
            {
                [scope.CompanyId] = new(
                    AllowedActions: ["CreateWaybillDraft", "UpdateWaybillDraft", "RecordCollection"],
                    MaxBatchOperations: 50)
            },
            new Dictionary<(Guid, Guid), SyncPolicyRestriction>
            {
                [(scope.CompanyId, scope.BranchId)] = new(
                    AllowedActions: ["CreateWaybillDraft", "UpdateWaybillDraft"],
                    MaxBatchOperations: 25)
            },
            [new ConfiguredDeviceSyncPolicy(
                scope.RegisteredDeviceId, scope.CompanyId, scope.BranchId, scope.DeviceId,
                new HashSet<string>(["UpdateWaybillDraft", "RecordCollection"], StringComparer.Ordinal))]);
        var provider = Provider(configuration,
            new HashSet<string>(["UpdateWaybillDraft", "RecordCollection"], StringComparer.Ordinal));

        var effective = await provider.ResolveAsync(scope.Current);

        Assert.True(effective.Enabled);
        Assert.Equal(25, effective.MaxBatchOperations);
        Assert.Equal("UpdateWaybillDraft", Assert.Single(effective.AllowedActions));
        Assert.Null(effective.ClosedReason);
        Assert.Equal("test-policy-v1", effective.SourceVersion);
        Assert.Matches("^[0-9a-f]{64}$", effective.SourceFingerprint);
    }

    [Fact]
    public async Task Fully_configured_scope_cannot_reopen_owner_closed_global_offline_gate()
    {
        var scope = Scope();
        var configuration = Configuration(
            scope, includeCompany: true, includeBranch: true, includeDevice: true);
        var provider = Provider(
            configuration,
            new HashSet<string>(["CreateWaybillDraft"], StringComparer.Ordinal),
            offlineEnabled: false);

        var effective = await provider.ResolveAsync(scope.Current);

        Assert.False(effective.Enabled);
        Assert.Equal("OFFLINE_DISABLED", effective.ClosedReason);
    }

    [Fact]
    public async Task Worker_retry_projection_applies_company_branch_and_device_tightening()
    {
        var scope = Scope();
        var deviceRestriction = new SyncPolicyRestriction(
            ServerExecutionMaxRetryCount: 2,
            ServerExecutionBaseSeconds: 15,
            ServerExecutionMaxDelayMinutes: 31);
        var configuration = EffectivePolicyConfiguration.Create(
            new Dictionary<Guid, SyncPolicyRestriction>
            {
                [scope.CompanyId] = new(
                    AllowedActions: ["CreateWaybillDraft"],
                    ServerExecutionMaxRetryCount: 4,
                    ServerExecutionBaseSeconds: 6)
            },
            new Dictionary<(Guid, Guid), SyncPolicyRestriction>
            {
                [(scope.CompanyId, scope.BranchId)] = new(
                    AllowedActions: ["CreateWaybillDraft"],
                    ServerExecutionMaxRetryCount: 3,
                    ServerExecutionBaseSeconds: 10)
            },
            [new ConfiguredDeviceSyncPolicy(
                scope.RegisteredDeviceId, scope.CompanyId, scope.BranchId, scope.DeviceId,
                new HashSet<string>(["CreateWaybillDraft"], StringComparer.Ordinal),
                deviceRestriction)]);
        var options = Options.Create(Global(offlineEnabled: false));
        var workerPolicies = new EffectiveSyncRetryPolicyResolver(
            configuration, new SyncEffectivePolicyResolver(options), options);

        var effective = await workerPolicies.ResolveAsync(
            scope.CompanyId, scope.BranchId, scope.RegisteredDeviceId, scope.DeviceId);
        var missingDevice = await workerPolicies.ResolveAsync(
            scope.CompanyId, scope.BranchId, Guid.NewGuid(), scope.DeviceId);

        Assert.NotNull(effective);
        Assert.Equal(2, effective!.MaxRetryCount);
        Assert.Equal(TimeSpan.FromSeconds(15), effective.BaseDelay);
        Assert.Equal(TimeSpan.FromMinutes(31), effective.MaxDelay);
        Assert.Null(missingDevice);
    }

    [Fact]
    public async Task Worker_reapplies_enabled_and_action_hierarchy_before_execution()
    {
        var scope = Scope();
        var configuration = Configuration(
            scope, includeCompany: true, includeBranch: true, includeDevice: true);
        var closedOptions = Options.Create(Global(offlineEnabled: false));
        var closed = new EffectiveSyncRetryPolicyResolver(
            configuration, new SyncEffectivePolicyResolver(closedOptions), closedOptions);
        var openOptions = Options.Create(Global(offlineEnabled: true));
        var open = new EffectiveSyncRetryPolicyResolver(
            configuration, new SyncEffectivePolicyResolver(openOptions), openOptions);

        var disabled = await closed.AuthorizeExecutionAsync(
            scope.CompanyId, scope.BranchId, scope.RegisteredDeviceId, scope.DeviceId,
            "CreateWaybillDraft");
        var allowed = await open.AuthorizeExecutionAsync(
            scope.CompanyId, scope.BranchId, scope.RegisteredDeviceId, scope.DeviceId,
            "CreateWaybillDraft");
        var tightened = await open.AuthorizeExecutionAsync(
            scope.CompanyId, scope.BranchId, scope.RegisteredDeviceId, scope.DeviceId,
            "UpdateWaybillDraft");

        Assert.False(disabled.IsAllowed);
        Assert.Equal("OFFLINE_DISABLED", disabled.ErrorCode);
        Assert.Equal("test-policy-v1", disabled.PolicySourceVersion);
        Assert.Matches("^[0-9a-f]{64}$", disabled.PolicySourceFingerprint);
        Assert.True(allowed.IsAllowed);
        Assert.Equal("test-policy-v1", allowed.PolicySourceVersion);
        Assert.False(tightened.IsAllowed);
        Assert.Equal("SCOPE_DENIED", tightened.ErrorCode);
    }

    [Fact]
    public async Task Runtime_batch_enforces_effective_batch_protocol_payload_and_action_limits()
    {
        var scope = Scope();
        var policy = OpenPolicy(
            allowedActions: new HashSet<string>(["CreateWaybillDraft"], StringComparer.Ordinal),
            maxBatch: 1,
            maximumPayloadBytes: 2);

        var oversizedBatch = Request(scope.DeviceId, [Operation("CreateWaybillDraft"), Operation("CreateWaybillDraft")]);
        var batchResult = await HandleAsync(scope, policy, oversizedBatch);
        Assert.Equal("BATCH_SIZE_INVALID", await ErrorCodeAsync(batchResult));

        var wrongProtocol = oversizedBatch with { ProtocolVersion = "sync-v2", Operations = [Operation("CreateWaybillDraft")] };
        var protocolResult = await HandleAsync(scope, policy, wrongProtocol);
        Assert.Equal("PROTOCOL_VERSION_UNSUPPORTED", await ErrorCodeAsync(protocolResult));

        var payloadResult = await HandleAsync(scope, policy,
            Request(scope.DeviceId, [Operation("CreateWaybillDraft", "{\"x\":1}")]));
        var payloadResponse = Assert.IsType<SyncBatchResponse>(
            Assert.IsAssignableFrom<IValueHttpResult>(payloadResult).Value);
        Assert.Equal("PAYLOAD_TOO_LARGE", Assert.Single(payloadResponse.Results).ErrorCode);

        var deniedResult = await HandleAsync(scope, policy,
            Request(scope.DeviceId, [Operation("UpdateWaybillDraft")]));
        var deniedResponse = Assert.IsType<SyncBatchResponse>(
            Assert.IsAssignableFrom<IValueHttpResult>(deniedResult).Value);
        Assert.Equal("SCOPE_DENIED", Assert.Single(deniedResponse.Results).ErrorCode);
    }

    private static async Task<IResult> HandleAsync(
        TestScope scope,
        EffectiveSyncPolicy policy,
        SyncBatchRequest request)
    {
        var body = JsonSerializer.SerializeToUtf8Bytes(request, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var accepted = new AcceptedSyncHttpRequest(
            scope.Current,
            new SyncProofSecurityContext(
                scope.UserId, scope.CompanyId, scope.BranchId, scope.RegisteredDeviceId, scope.DeviceId),
            new AcceptedSyncProofContext(
                Guid.NewGuid(), scope.UserId, scope.CompanyId, scope.BranchId,
                scope.RegisteredDeviceId, scope.DeviceId, 1, 1, new string('t', 43), Guid.NewGuid()),
            body,
            Guid.NewGuid(),
            policy);
        return await SyncApiModule.HandleBatchAsync(
            new DefaultHttpContext(), new AcceptedAuthenticator(accepted), null!, null!,
            NoOpRejectionAuditSink.Instance, default);
    }

    private static Task<string?> ErrorCodeAsync(IResult result)
    {
        var value = Assert.IsAssignableFrom<IValueHttpResult>(result).Value;
        using var json = JsonDocument.Parse(
            JsonSerializer.Serialize(value, new JsonSerializerOptions(JsonSerializerDefaults.Web)));
        return Task.FromResult(json.RootElement.GetProperty("errorCode").GetString());
    }

    private static SyncBatchRequest Request(string deviceId, IReadOnlyList<SyncBatchOperationRequest?> operations)
        => new(deviceId, "sync-v1", operations);

    private static SyncBatchOperationRequest Operation(string action, string payload = "{}")
    {
        var definition = SyncActionCatalog.Definitions.Single(x => x.ActionCodeValue == action);
        return new SyncBatchOperationRequest(
            action, definition.OperationTypeValue, definition.EntityTypeValue,
            definition.EntityId == SyncValueRequirement.Required ? Guid.NewGuid() : null,
            Guid.NewGuid().ToString("N"), payload,
            Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(payload))).ToLowerInvariant(),
            DateTimeOffset.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'"),
            Guid.NewGuid(),
            definition.BaseVersion == SyncValueRequirement.Required ? 1 : null);
    }

    private static EffectiveSyncPolicyProvider Provider(
        EffectivePolicyConfiguration configuration,
        IReadOnlySet<string> allowedActionCodes,
        bool offlineEnabled = true)
    {
        var options = Options.Create(Global(offlineEnabled));
        var allowedPermissions = SyncActionCatalog.Definitions
            .Where(x => allowedActionCodes.Contains(x.ActionCodeValue))
            .Select(x => x.RequiredPermission)
            .ToHashSet(StringComparer.Ordinal);
        return new EffectiveSyncPolicyProvider(
            configuration,
            new SyncEffectivePolicyResolver(options),
            new StaticPermissionResolver(allowedPermissions));
    }

    private static EffectivePolicyConfiguration Configuration(
        TestScope scope,
        bool includeCompany,
        bool includeBranch,
        bool includeDevice)
        => EffectivePolicyConfiguration.Create(
            includeCompany
                ? new Dictionary<Guid, SyncPolicyRestriction>
                    { [scope.CompanyId] = new(AllowedActions: ["CreateWaybillDraft"]) }
                : new Dictionary<Guid, SyncPolicyRestriction>(),
            includeBranch
                ? new Dictionary<(Guid, Guid), SyncPolicyRestriction>
                    { [(scope.CompanyId, scope.BranchId)] = new(AllowedActions: ["CreateWaybillDraft"]) }
                : new Dictionary<(Guid, Guid), SyncPolicyRestriction>(),
            includeDevice
                ? [new ConfiguredDeviceSyncPolicy(
                    scope.RegisteredDeviceId, scope.CompanyId, scope.BranchId, scope.DeviceId,
                    new HashSet<string>(["CreateWaybillDraft"], StringComparer.Ordinal))]
                : []);

    private static SyncRuntimePolicyOptions Global(bool offlineEnabled) => new()
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

    private static EffectiveSyncPolicy OpenPolicy(
        IReadOnlySet<string> allowedActions,
        int maxBatch,
        int maximumPayloadBytes)
        => new(true, allowedActions, new HashSet<string>(["sync-v1"], StringComparer.Ordinal),
            maxBatch, 2_097_152, maximumPayloadBytes, 5, 5, 5, 5, 30, 30, 24, 7, 90, 24, null);

    private static TestScope Scope()
    {
        var scope = new TestScope(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "governed-device");
        return scope with
        {
            Current = new CurrentSecurityContext(
                scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid(), scope.DeviceId, true,
                scope.RegisteredDeviceId, 1)
        };
    }

    private sealed record TestScope(
        Guid UserId,
        Guid CompanyId,
        Guid BranchId,
        Guid RegisteredDeviceId,
        string DeviceId)
    {
        public CurrentSecurityContext Current { get; init; } = null!;
    }

    private sealed class StaticPermissionResolver(IReadOnlySet<string> allowed) : IEffectivePermissionResolver
    {
        public Task<bool> HasPermissionAsync(
            Guid userId,
            Guid companyId,
            Guid? branchId,
            string permissionCode,
            CancellationToken cancellationToken = default)
            => Task.FromResult(allowed.Contains(permissionCode));
    }

    private sealed class AcceptedAuthenticator(AcceptedSyncHttpRequest accepted) : ISyncPopHttpRequestAuthenticator
    {
        public Task<SyncHttpAuthenticationResult> AuthenticateAsync(
            HttpContext http,
            string canonicalPath,
            TryReadSyncRequestDeviceId? tryReadBodyDeviceId,
            CancellationToken cancellationToken)
            => Task.FromResult(new SyncHttpAuthenticationResult(accepted, null));
    }

    private sealed class NoOpRejectionAuditSink : ISyncBatchRejectionAuditSink
    {
        public static readonly NoOpRejectionAuditSink Instance = new();

        public Task WriteAsync(
            AcceptedSyncProofContext proof,
            Guid? operationCorrelationId,
            string errorCode,
            CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}
