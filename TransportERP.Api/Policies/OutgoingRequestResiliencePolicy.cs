using System.Net;

namespace TransportERP.Api.Policies;

/// <summary>
/// The single approved policy for automatic retries of outbound read requests.
/// Mutating requests are never retried automatically.
/// </summary>
public static class OutgoingRequestResiliencePolicy
{
    public const int TimeoutSeconds = 15;
    public const int MaximumAttempts = 3;
    public const int MaximumRetries = MaximumAttempts - 1;
    public const int InitialBackoffMilliseconds = 250;
    public const int MaximumJitterMilliseconds = 100;

    public static bool IsAutomaticRetryAllowed(HttpMethod method) =>
        method == HttpMethod.Get ||
        method == HttpMethod.Head ||
        method == HttpMethod.Options;

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

        var exponentialDelay = InitialBackoffMilliseconds * (1 << (retryNumber - 1));
        return TimeSpan.FromMilliseconds(exponentialDelay + jitterMilliseconds);
    }
}
