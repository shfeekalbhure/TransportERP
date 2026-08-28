using System.Text.Json;
using TransportERP.Api.Sync;
using TransportERP.Infrastructure.Persistence;
using TransportERP.Offline.Transport;

namespace TransportERP.Tests;

public sealed class Stage4SyncConflictReviewResponseTests
{
    [Fact]
    public void Batch_result_exposes_only_allowlisted_conflict_review_metadata()
    {
        var entityId = Guid.NewGuid();
        var resolverId = Guid.NewGuid().ToString("D");
        var replacementId = Guid.NewGuid();
        var operation = new SyncOperation
        {
            Id = Guid.NewGuid(),
            ClientOperationId = Guid.NewGuid().ToString("D"),
            OperationCorrelationId = Guid.NewGuid(),
            ActionCode = "UpdateWaybillDraft",
            EntityType = "Waybill",
            EntityId = entityId,
            Status = "CONFLICT",
            ErrorCode = "BASE_VERSION_CONFLICT",
            ConflictCase = new ConflictCase
            {
                Id = Guid.NewGuid(),
                BaseVersion = 7,
                ConflictReason = "BASE_VERSION_CONFLICT",
                Status = "RESOLVED",
                Resolution = SyncConflictResolutionDecisions.ReapplyAsNew,
                ResolvedBy = resolverId,
                ResolvedAt = DateTimeOffset.Parse("2026-08-27T10:00:00Z"),
                ReplacedByOperationId = replacementId,
                DeviceSnapshot = JsonSerializer.Serialize(new
                {
                    ActionCode = "UpdateWaybillDraft",
                    EntityType = "Waybill",
                    EntityId = entityId,
                    RequestedBaseVersion = 7,
                    PayloadJson = "top-secret-business-value",
                    Proof = "raw-proof"
                }),
                ServerSnapshot = JsonSerializer.Serialize(new
                {
                    EntityType = "Waybill",
                    EntityId = entityId,
                    Exists = true,
                    CurrentVersion = 8,
                    CustomerName = "must-not-cross-boundary"
                })
            }
        };

        var result = SyncBatchOperationResult.From(operation, DateTimeOffset.UtcNow);
        var review = Assert.IsType<SyncConflictReviewResult>(result.ConflictReview);
        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        Assert.Equal(7, review.BaseVersion);
        Assert.Equal("BASE_VERSION_CONFLICT", review.ConflictReason);
        Assert.Equal(7, review.LocalSnapshot!.RequestedBaseVersion);
        Assert.Equal(8, review.ServerSnapshot!.CurrentVersion);
        Assert.True(review.ResolvedByAuthorizedUser);
        Assert.Equal(replacementId, review.ReplacedByOperationId);
        Assert.DoesNotContain(resolverId, json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("top-secret-business-value", json, StringComparison.Ordinal);
        Assert.DoesNotContain("raw-proof", json, StringComparison.Ordinal);
        Assert.DoesNotContain("must-not-cross-boundary", json, StringComparison.Ordinal);
        Assert.DoesNotContain("PayloadJson", json, StringComparison.OrdinalIgnoreCase);

        var envelopeJson = JsonSerializer.Serialize(new SyncBatchResponse(
            "sync-v1", [result], DateTimeOffset.UtcNow, Guid.NewGuid()),
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var clientEnvelope = JsonSerializer.Deserialize<SyncV1BatchResponse>(envelopeJson,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var clientReview = Assert.IsType<SyncV1ConflictReview>(Assert.Single(clientEnvelope!.Results).ConflictReview);
        Assert.Equal(7, clientReview.BaseVersion);
        Assert.Equal(8, clientReview.ServerSnapshot!.CurrentVersion);
    }

    [Fact]
    public void Invalid_or_redacted_snapshot_content_is_not_reflected_to_the_client()
    {
        var conflict = new ConflictCase
        {
            BaseVersion = 4,
            ConflictReason = "unsafe reason with spaces",
            Status = "unexpected-status",
            DeviceSnapshot = "{not-json",
            ServerSnapshot = "[]"
        };

        var review = Assert.IsType<SyncConflictReviewResult>(SyncConflictReviewResult.From(conflict));

        Assert.Equal("CONFLICT", review.ConflictReason);
        Assert.Equal("UNKNOWN", review.Status);
        Assert.Null(review.LocalSnapshot);
        Assert.Null(review.ServerSnapshot);
        Assert.False(review.ResolvedByAuthorizedUser);
    }
}
