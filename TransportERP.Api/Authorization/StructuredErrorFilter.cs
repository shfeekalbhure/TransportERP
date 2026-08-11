using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TransportERP.Contracts.Core;

namespace TransportERP.Api.Authorization;

/// <summary>Converts approved application validation/lifecycle tokens into TransportError; raw exception details never cross the API boundary.</summary>
public sealed class StructuredErrorFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var code = context.Exception switch
        {
            ArgumentException => TransportErrorCode.ValidationFailed,
            KeyNotFoundException => TransportErrorCode.NotFound,
            InvalidOperationException { Message: "CONCURRENCY_CONFLICT" } => TransportErrorCode.ConcurrencyConflict,
            InvalidOperationException { Message: "DUPLICATE_NUMBER" } => TransportErrorCode.DuplicateNumber,
            InvalidOperationException { Message: "NUMBER_SEQUENCE_INACTIVE" } => TransportErrorCode.NumberSequenceInactive,
            InvalidOperationException { Message: "STATE_TRANSITION_INVALID" } => TransportErrorCode.StateTransitionInvalid,
            InvalidOperationException { Message: "NUMBERING_STATE_INVALID" } => TransportErrorCode.NumberingStateInvalid,
            InvalidOperationException { Message: "IDEMPOTENCY_CONFLICT" } => TransportErrorCode.IdempotencyConflict,
            _ => (TransportErrorCode?)null
        };
        if (code is null) return;
        var correlation = context.HttpContext.Request.Headers.TryGetValue("X-Correlation-Id", out var header) && Guid.TryParse(header, out var id) ? id : Guid.CreateVersion7();
        context.Result = new ObjectResult(new TransportError(code.Value, correlation, $"error.{code.Value}")) { StatusCode = code is TransportErrorCode.NotFound ? 404 : code is TransportErrorCode.ConcurrencyConflict or TransportErrorCode.DuplicateNumber ? 409 : 400 };
        context.ExceptionHandled = true;
    }
}
