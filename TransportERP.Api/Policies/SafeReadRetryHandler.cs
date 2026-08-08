using System.Net;

namespace TransportERP.Api.Policies;

/// <summary>
/// Retries only idempotent read requests after transient failures. POST, PUT, PATCH and DELETE
/// remain a single attempt even when a downstream service is unavailable.
/// </summary>
public sealed class SafeReadRetryHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!OutgoingRequestResiliencePolicy.IsAutomaticRetryAllowed(request.Method))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        for (var attempt = 1; attempt <= OutgoingRequestResiliencePolicy.MaximumAttempts; attempt++)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                if (!OutgoingRequestResiliencePolicy.IsTransient(response.StatusCode) ||
                    attempt == OutgoingRequestResiliencePolicy.MaximumAttempts)
                {
                    return response;
                }

                response.Dispose();
            }
            catch (HttpRequestException) when (attempt < OutgoingRequestResiliencePolicy.MaximumAttempts)
            {
                // The bounded retry below is intentionally limited to safe read methods.
            }
            catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested &&
                                                attempt < OutgoingRequestResiliencePolicy.MaximumAttempts)
            {
                // A downstream timeout may be retried only while the caller has not cancelled.
            }

            var jitter = Random.Shared.Next(0, OutgoingRequestResiliencePolicy.MaximumJitterMilliseconds + 1);
            await Task.Delay(
                OutgoingRequestResiliencePolicy.GetBackoff(attempt, jitter),
                cancellationToken);
        }

        throw new InvalidOperationException("The retry loop ended without a response.");
    }
}
