using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;

namespace TransportERP.Api.Security;

public sealed class IdentityRateLimiter : IAsyncDisposable
{
    public readonly record struct Decision(bool IsAcquired, TimeSpan? RetryAfter);

    private readonly PartitionedRateLimiter<string> loginIp;
    private readonly PartitionedRateLimiter<string> loginAccountDevice;
    private readonly PartitionedRateLimiter<string> refreshIp;
    private readonly PartitionedRateLimiter<string> refreshDevice;

    public IdentityRateLimiter(IOptions<TransportSecurityOptions> options)
    {
        var value = options.Value;
        loginIp = Create(value.LoginRateLimitPermitCount, value.RateLimitWindowSeconds);
        loginAccountDevice = Create(value.LoginRateLimitPermitCount, value.RateLimitWindowSeconds);
        refreshIp = Create(value.RefreshRateLimitPermitCount, value.RateLimitWindowSeconds);
        refreshDevice = Create(value.RefreshRateLimitPermitCount, value.RateLimitWindowSeconds);
    }

    public async Task<Decision> TryAcquireLoginAsync(string ip, string normalizedLogin, string normalizedDevice,
        CancellationToken cancellationToken)
    {
        var ipDecision = await AcquireAsync(loginIp, ip, cancellationToken);
        return ipDecision.IsAcquired
            ? await AcquireAsync(loginAccountDevice, $"{normalizedLogin}|{normalizedDevice}", cancellationToken)
            : ipDecision;
    }

    public async Task<Decision> TryAcquireRefreshAsync(string ip, string refreshPartitionHash,
        string normalizedDevice, CancellationToken cancellationToken)
    {
        var ipDecision = await AcquireAsync(refreshIp, ip, cancellationToken);
        return ipDecision.IsAcquired
            ? await AcquireAsync(refreshDevice, $"{refreshPartitionHash}|{normalizedDevice}", cancellationToken)
            : ipDecision;
    }

    private static PartitionedRateLimiter<string> Create(int permits, int windowSeconds)
        => PartitionedRateLimiter.Create<string, string>(key => RateLimitPartition.GetFixedWindowLimiter(key, _ => new()
        {
            PermitLimit = permits,
            Window = TimeSpan.FromSeconds(windowSeconds),
            QueueLimit = 0,
            AutoReplenishment = true
        }));

    private static async Task<Decision> AcquireAsync(PartitionedRateLimiter<string> limiter, string key, CancellationToken ct)
    {
        using var lease = await limiter.AcquireAsync(key, 1, ct);
        return new Decision(lease.IsAcquired,
            lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter) ? retryAfter : null);
    }

    public async ValueTask DisposeAsync()
    {
        await loginIp.DisposeAsync();
        await loginAccountDevice.DisposeAsync();
        await refreshIp.DisposeAsync();
        await refreshDevice.DisposeAsync();
    }
}
