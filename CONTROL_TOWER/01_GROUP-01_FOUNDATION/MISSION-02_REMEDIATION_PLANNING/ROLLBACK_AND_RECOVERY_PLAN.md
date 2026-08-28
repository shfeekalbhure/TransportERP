# Rollback and Recovery Plan

| Wave | Rollback unit | Recovery preparation | Trigger | Recovery acceptance |
|---:|---|---|---|---|
| W0 | governance/evidence commit only | immutable refs, bundles and hashes | missing/mismatched asset | all original refs/bytes recoverable |
| W1 | mapper code separate from any data action | pre-change test baseline; safe-copy backup; row-level repair journal | Volume regression/ambiguous repair | old code recoverable; DB restored or exact inverse proved; rows reconcile |
| W2 | feature-scoped security code + forward DB migration | break-glass tested offline; role/config snapshot without secrets; backup | lockout/cross-tenant/device bypass | access restored without weakening tenant boundary; audit preserved |
| W3 | posting/audit feature toggle + forward migrations | source/journal reconciliation, backup/restore, legacy hash verifier | imbalance, orphan journal/audit, legacy-chain failure | no partial posting; all source↔ledger links reconcile; chains verify |
| W4 | protocol/worker version and kill switch | compatible reader, queue quarantine/export, store backup | duplicate effect, replay bypass, incompatibility | workers stopped safely; queues readable; no lost/duplicate business effect |
| W5 | signed client artifact version | previous installer/package and local-store migration backup | startup/auth/data loss/signing failure | previous client starts and reads compatible local data |
| W6 | per-module/increment feature flag | state snapshot, compensating business actions, DB restore rehearsal | invalid custody/accounting/seat state | operational and ledger state reconcile; immutable audit explains compensation |
| W7 | immutable release artifact + DB/client rollback pair | tested runbooks, off-site backup metadata, RPO/RTO | deployment/upgrade/privacy/supply failure | prior service restored within approved RTO/RPO with hashes/invariants |
| W8 | isolated refactor commits | before/after manifests and full regression | behavior/contract/test/evidence drift | revert restores identical behavior and paths or approved compatibility shim |

## Hard constraints

- A migration `Down()` method is not recovery proof.
- Posted accounting history is reversed, never edited/deleted.
- Old audit events are verified by their original hash version, never rewritten.
- Production data repair, destructive DDL, asset deletion, history rewrite or merge requires explicit owner authority.
- If rollback would lose newer legitimate business data, stop and use a reviewed forward correction or restore/reconciliation plan; never overwrite by assumption.
