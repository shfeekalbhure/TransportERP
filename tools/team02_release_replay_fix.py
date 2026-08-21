from pathlib import Path

p = Path('TransportERP.Infrastructure/Persistence/ShippingExecutionPersistence.cs')
text = p.read_text(encoding='utf-8')
old1 = '''            if (replay.WaybillItemId != itemId || replay.Quantity != request.Quantity ||
                !SameInstant(replay.ReleasedAt, request.ReleasedAt))
                throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");'''
new1 = '''            if (replay.WaybillItemId != itemId || replay.Quantity != request.Quantity)
                throw new WaybillPersistenceException("IDEMPOTENCY_CONFLICT");'''
old2 = '''            if (replay is not null && replay.WaybillItemId == itemId && replay.Quantity == request.Quantity &&
                SameInstant(replay.ReleasedAt, request.ReleasedAt))
                return await ItemState(context, waybillId, itemId, cancellationToken);'''
new2 = '''            if (replay is not null && replay.WaybillItemId == itemId && replay.Quantity == request.Quantity)
                return await ItemState(context, waybillId, itemId, cancellationToken);'''
for old, new in ((old1, new1), (old2, new2)):
    if text.count(old) != 1:
        raise SystemExit('TEAM-02 release replay anchor mismatch')
    text = text.replace(old, new, 1)
p.write_text(text, encoding='utf-8')
print('TEAM-02 release replay semantics restored to W2-P2C01-014 contract.')
