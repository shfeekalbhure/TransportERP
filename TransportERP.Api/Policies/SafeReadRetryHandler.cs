using System.Net;

namespace TransportERP.Api.Policies;

/// <summary>
/// Applies the approved bounded resilience contract. Unsafe methods can retry only when the
/// caller explicitly supplies an Idempotency-Key.
/// </summary>
public sealed class SafeReadRetryHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var canRetry = OutgoingRequestResiliencePolicy.IsAutomaticRetryAllowed(
            request.Method,
            request.Headers.Contains("Idempotency-Key"));
        if (!canRetry)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        using var totalTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        totalTimeout.CancelAfter(OutgoingRequestResiliencePolicy.TotalRequestTimeout);
        for (var attempt = 1; attempt <= OutgoingRequestResiliencePolicy.MaximumAttempts; attempt++)
        {
            try
            {
                using var attemptTimeout = CancellationTokenSource.CreateLinkedTokenSource(totalTimeout.Token);
                attemptTimeout.CancelAfter(OutgoingRequestResiliencePolicy.AttemptTimeout);
                var response = await base.SendAsync(request, attemptTimeout.Token);
                if (!OutgoingRequestResiliencePolicy.IsTransient(response.StatusCode) ||
                    attempt == OutgoingRequestResiliencePolicy.MaximumAttempts)
                {
                    return response;
                }

                var retryAfter = GetRetryAfter(response);
                response.Dispose();
                await DelayAsync(retryAfter, attempt, totalTimeout.Token);
            }
            catch (HttpRequestException) when (attempt < OutgoingRequestResiliencePolicy.MaximumAttempts)
            {
                await DelayAsync(null, attempt, totalTimeout.Token);
            }
            catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested && !totalTimeout.IsCancellationRequested &&
                                                attempt < OutgoingRequestResiliencePolicy.MaximumAttempts)
            {
                await DelayAsync(null, attempt, totalTimeout.Token);
            }
        }

        throw new InvalidOperationException("The retry loop ended without a response.");
    }

    public static TimeSpan GetRetryAfter(HttpResponseMessage response)
    {
        var retryAfter = response.Headers.RetryAfter;
        if (retryAfter?.Delta is { } delta)
        {
            return delta;
        }

        if (retryAfter?.Date is { } date)
        {
            return date - DateTimeOffset.UtcNow > TimeSpan.Zero ? date - DateTimeOffset.UtcNow : TimeSpan.Zero;
        }

        return TimeSpan.Zero;
    }

    private static Task DelayAsync(TimeSpan? retryAfter, int retryNumber, CancellationToken cancellationToken)
    {
        var jitter = OutgoingRequestResiliencePolicy.UseJitter
            ? Random.Shared.Next(0, OutgoingRequestResiliencePolicy.MaximumJitterMilliseconds + 1)
            : 0;
        var exponential = OutgoingRequestResiliencePolicy.GetBackoff(retryNumber, jitter);
        return Task.Delay(retryAfter is { } delay && delay > exponential ? delay : exponential, cancellationToken);
    }
}
