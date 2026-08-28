using System.Net;
using TransportERP.Application.Sync;

namespace TransportERP.Tests;

public sealed class SyncAuthSessionTransportFailureClassifierTests
{
    private const string SecretMarker = "SECRET_MESSAGE_MUST_NOT_ESCAPE";

    [Theory]
    [InlineData(HttpRequestError.SecureConnectionError, "AUTH_SESSION_TLS_FAILED")]
    [InlineData(HttpRequestError.NameResolutionError, "AUTH_SESSION_NAME_RESOLUTION_FAILED")]
    [InlineData(HttpRequestError.ConnectionError, "AUTH_SESSION_CONNECTION_FAILED")]
    [InlineData(HttpRequestError.HttpProtocolError, "AUTH_SESSION_HTTP_PROTOCOL_FAILED")]
    [InlineData(HttpRequestError.Unknown, "AUTH_SESSION_TRANSPORT_FAILED")]
    public void Classifier_returns_only_the_fixed_code(
        HttpRequestError error,
        string expected)
    {
        var exception = new HttpRequestException(
            error,
            SecretMarker,
            new InvalidOperationException(SecretMarker),
            HttpStatusCode.InternalServerError);

        var actual = SyncAuthSessionTransportFailureClassifier.Classify(exception);

        Assert.Equal(expected, actual);
        Assert.DoesNotContain(SecretMarker, actual, StringComparison.Ordinal);
        Assert.Matches("^[A-Z0-9_]{1,64}$", actual);
    }

    [Fact]
    public void Classifier_rejects_a_missing_exception()
    {
        Assert.Throws<ArgumentNullException>(() =>
            SyncAuthSessionTransportFailureClassifier.Classify(null!));
    }
}
