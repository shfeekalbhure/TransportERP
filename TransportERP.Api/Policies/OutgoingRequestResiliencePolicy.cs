using System.Net;

namespace TransportERP.Api.Policies;

/// <summary>
/// The single approved policy for automatic retries of outbound read requests.
/// Mutating requests are never retried automatically.
/// </summary>
public static class OutgoingRequestResiliencePolicy
{
    public static readonly TimeSpan TotalRequestTimeout = TimeSpan.FromSeconds(30);
    public static readonly TimeSpan AttemptTimeout = TimeSpan.FromSeconds(10);
    public const int MaximumAttempts = 3;
    public const int MaximumRetries = MaximumAttempts - 1;
    public static readonly TimeSpan InitialDelay = TimeSpan.FromSeconds(2);
    public const bool UseJitter = true;
    public const int MaximumJitterMilliseconds = 250;

    public static bool IsAutomaticRetryAllowed(HttpMethod method, bool hasIdempotencyKey = false) =>
        method == HttpMethod.Get ||
        method == HttpMethod.Head ||
        method == HttpMethod.Options ||
        (hasIdempotencyKey && (method == HttpMethod.Post || method == HttpMethod.Put || method.Method == "PATCH" || method == HttpMethod.Delete));

    public static bool IsTransient(HttpStatusCode statusCode) =>
        statusCode == HttpStatusCode.RequestTimeout ||
        statusCode == (HttpStatusCode)429 ||
        (int)statusCode >= 500;

    public static TimeSpan GetBackoff(int retryNumber, int jitterMilliseconds)
    {
        if (retryNumber is < 1 or > MaximumRetries)
        {
            throw new ArgumentOutOfRangeException(nameof(retryNumber));
        }

        if (jitterMilliseconds is < 0 or > MaximumJitterMilliseconds)
        {
            throw new ArgumentOutOfRangeException(nameof(jitterMilliseconds));
        }

        var exponentialDelay = InitialDelay.TotalMilliseconds * (1 << (retryNumber - 1));
        return TimeSpan.FromMilliseconds(exponentialDelay + (UseJitter ? jitterMilliseconds : 0));
    }
}
