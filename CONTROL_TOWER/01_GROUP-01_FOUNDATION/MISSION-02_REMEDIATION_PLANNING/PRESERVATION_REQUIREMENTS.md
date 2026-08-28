# Preservation Requirements

| ID | Asset/invariant | Required action before change | Closure evidence | Forbidden without authority |
|---|---|---|---|---|
| `PRES-001` | `master@2ec6cccf...` tree `516247dd...` | record ref/commit/tree and full tracked manifest | Git object/ref transcript + manifest hash | silent baseline substitution |
| `PRES-002` | governance HEAD at M02 start `f2fc5a73...` | retain as planning parent | commit/tree evidence | rewriting prior Control Tower history |
| `PRES-003` | MASTER/GATE v1.0/v2.0 and accepted A/B/C1/D/C2/E packages | keep immutable; verify detached checksums | all package hash checks | edit/overwrite/silent reseal |
| `PRES-004` | PR69 `601f2d1c...` tree `bfbcd140...` | freeze ref/tree/diff; review by component | 206-file inventory and comparison | merge/rebase/delete/force-push/blind copy |
| `PRES-005` | WAVE-1/W0/P2-D and registered local/external work | inventory owner/ref/path, hash and bundle | recoverable bundle + owner/disposition register | reset/prune/delete/history rewrite |
| `PRES-006` | migration lineage and model snapshot | record ordered names/hashes/applied history; forward-only | current-state DB register + rehearsal logs | reorder/squash/drop/rewrite |
| `PRES-007` | live data and `Volume` meaning | read-only impact query on approved safe copy; preserve explicit value/precision/nullability | row counts, ambiguous rows, backup/restore proof | derive/overwrite/repair by assumption |
| `PRES-008` | CAS, idempotency, constraints, triggers, serializable paths | baseline and negative regression tests | before/after invariant report | weakening/removal during remediation |
| `PRES-009` | audit history and hash algorithms | version algorithm; retain old bytes and verifier | legacy/new chain verification | rehashing or mutating old events |
| `PRES-010` | accounting history, document/journal IDs, currency precision | immutable source/link/reversal and reconciliation inventory | balance/idempotency/reversal report | update/delete posted history or duplicate journal |
| `PRES-011` | API/contracts/permission codes | consumer and serialization snapshot; compatibility plan | contract diff + negative parity | silent breaking contract or access widening |
| `PRES-012` | Offline payload/version/idempotency/conflict history | compatible protocol version and quarantine strategy | replay/restart/upgrade/downgrade tests | generic executor or incompatible queue rewrite |
| `PRES-013` | Desktop/Mobile names, screen IDs, RTL and design assets | canonical crosswalk, screenshots/accessibility baseline | screen→route/API/permission/test matrix | destructive rename/consolidation |
| `PRES-014` | test IDs, fixtures, expected counts and raw evidence | capture discovery list and artifacts before test split | before/after case parity and TRX | dropping tests to make CI green |
| `PRES-015` | secrets and personal data | use synthetic/redacted values; scan logs/artifacts | clean secret/PII scan | Production secrets/data in tests or evidence |

Every Move/Rename/Merge/Split/Refactor requires before/after dependency, contract, data, runtime, test and evidence maps. `CANDIDATE FOR REMOVAL` is the strongest allowed label until consumer, preservation and owner review completes.
