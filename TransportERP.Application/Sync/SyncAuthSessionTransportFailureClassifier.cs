namespace TransportERP.Application.Sync;

/// <summary>
/// Converts the first authenticated-session HTTP transport failure into a fixed diagnostic code.
/// Exception messages, inner exceptions, request URIs and certificate details are intentionally
/// outside this result surface.
/// </summary>
public static class SyncAuthSessionTransportFailureClassifier
{
    public static string Classify(HttpRequestException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        return exception.HttpRequestError switch
        {
            HttpRequestError.SecureConnectionError => "AUTH_SESSION_TLS_FAILED",
            HttpRequestError.NameResolutionError => "AUTH_SESSION_NAME_RESOLUTION_FAILED",
            HttpRequestError.ConnectionError => "AUTH_SESSION_CONNECTION_FAILED",
            HttpRequestError.HttpProtocolError => "AUTH_SESSION_HTTP_PROTOCOL_FAILED",
            _ => "AUTH_SESSION_TRANSPORT_FAILED"
        };
    }
}
