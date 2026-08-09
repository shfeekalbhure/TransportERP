using System.Net;

namespace TransportERP.Api.Policies;

/// <summary>
/// Applies the approved bounded resilience contract. Unsafe methods can retry only when the
/// caller explicitly supplies an Idempotency-Key.
/// </summary>
public sealed class SafeReadRetryHandler : DelegatingHandler
{
    private readonly IResilienceDelay _delay;
    private readonly ResilienceExecutionOptions _options;

    public SafeReadRetryHandler(IResilienceDelay? delay = null, ResilienceExecutionOptions? options = null)
    {
        _delay = delay ?? new SystemResilienceDelay();
        _options = options ?? ResilienceExecutionOptions.FromApprovedPolicy();
    }

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
        totalTimeout.CancelAfter(_options.TotalTimeout);
        for (var attempt = 1; attempt <= _options.MaximumAttempts; attempt++)
        {
            try
            {
                using var attemptTimeout = CancellationTokenSource.CreateLinkedTokenSource(totalTimeout.Token);
                attemptTimeout.CancelAfter(_options.AttemptTimeout);
                var response = await base.SendAsync(request, attemptTimeout.Token);
                if (!OutgoingRequestResiliencePolicy.IsTransient(response.StatusCode) ||
                    attempt == _options.MaximumAttempts)
                {
                    return response;
                }

                var retryAfter = GetRetryAfter(response);
                response.Dispose();
                await DelayAsync(retryAfter, attempt, totalTimeout.Token);
            }
            catch (HttpRequestException) when (attempt < _options.MaximumAttempts)
            {
                await DelayAsync(null, attempt, totalTimeout.Token);
            }
            catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested && !totalTimeout.IsCancellationRequested &&
                                                attempt < _options.MaximumAttempts)
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

    private Task DelayAsync(TimeSpan? retryAfter, int retryNumber, CancellationToken cancellationToken)
    {
        var jitter = OutgoingRequestResiliencePolicy.UseJitter
            ? Random.Shared.Next(0, OutgoingRequestResiliencePolicy.MaximumJitterMilliseconds + 1)
            : 0;
        var exponential = _options.GetBackoff(retryNumber, jitter);
        return _delay.DelayAsync(retryAfter is { } delay && delay > exponential ? delay : exponential, cancellationToken);
    }
}

public interface IResilienceDelay
{
    Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken);
}

public sealed class SystemResilienceDelay : IResilienceDelay
{
    public Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken) => Task.Delay(delay, cancellationToken);
}

/// <summary>
/// Production construction always uses the approved policy. The public override exists solely to
/// make cancellation behaviour observable with a deterministic, short-running test transport.
/// </summary>
public sealed record ResilienceExecutionOptions(TimeSpan TotalTimeout, TimeSpan AttemptTimeout, int MaximumAttempts)
{
    public static ResilienceExecutionOptions FromApprovedPolicy() => new(
        OutgoingRequestResiliencePolicy.TotalRequestTimeout,
        OutgoingRequestResiliencePolicy.AttemptTimeout,
        OutgoingRequestResiliencePolicy.MaximumAttempts);

    public TimeSpan GetBackoff(int retryNumber, int jitterMilliseconds) =>
        OutgoingRequestResiliencePolicy.GetBackoff(retryNumber, jitterMilliseconds);
}
