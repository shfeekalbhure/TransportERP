# TEAM-E Review of TEAM-C2 Target Design Feasibility

- Status: `FINAL — SEALED`
- Design class reviewed: `PROPOSED — NOT IMPLEMENTED`
- Advisory result: `CONDITIONALLY SUITABLE — IMPLEMENTATION ADR/PLANNING CONDITIONS REMAIN`

## Multidisciplinary conclusions

| Area | Advisory conclusion | Mandatory condition before planning/implementation |
|---|---|---|
| Architecture | Modular monolith and logical-first boundaries fit the partial foundation better than immediate microservices. | Name authoritative SHA; use architecture tests; avoid project explosion and big-bang extraction. |
| Security/tenant | Server-derived TenantContext, persistent authorization, device registry/PoP, and DB defense are directionally correct. | Define hierarchy/cardinality and IdP/device authority; include user/device ownership on every sync lifecycle action and bidirectional tenant negatives. |
| Offline | Typed allowlisted outbox/inbox/pull/conflict flow is safer than generic JSON execution. | Current `OFFLINE_WRITE=0 / Can Queue=NO` remains binding; authorize operations individually; prove encryption, versioning, replay/restart/revocation. |
| Database | Forward-only, lineage-preserving, safe-copy and recovery controls conform to DB-GOV-001. | Populate database current-state/change registers; no schema/DbContext/RLS/trigger/data action before impact/preservation/test/recovery approval. |
| Accounting | Balanced immutable posting/reversal and source-to-ledger traceability are required and appropriate. | Add a transaction-ownership ADR defining how source state/link, journal, audit, and outbox are atomic without cross-module entity/table reach-through. |
| Audit/privacy | Versioned hash lineage, append-only behavior, classification/redaction/retention are appropriate. | Define backward verifier, transactional port, legal/retention authority, encryption/keying and export policy. |
| Desktop/Mobile | Executable hosts and trust-specific clients are valid targets. | Canonical screen/version crosswalk, platform scope, API integration, secure storage/signing and exact-SHA E2E evidence. |
| Shipping/Ticketing/Reporting | Incremental shipping closure plus separate Ticketing/Reporting boundaries are sensible. | Do not invent requirements; bind every increment to canonical workflow/accounting/security/offline evidence. |
| QA/CI/supply/release | Proposed exact-SHA matrix and artifact/recovery chain addresses observed gaps. | Pin SDK/packages/actions; retain evidence; prove install/upgrade/rollback/restore and operator runbooks. |
| Preservation | Crosswalk appropriately protects migrations, IDs, data meanings, partial runtime, screens, tests and local/unmerged assets. | No move/merge/split/rename/cleanup until parity, semantic disposition and required owner authority. |

## Cross-module transaction ADR required

The proposal currently combines four statements:

1. Accounting, Waybills, Shipping, and Audit have independent ownership.
2. Direct cross-module entity/table reach-through is prohibited.
3. Cross-module effects normally use outbox/inbox.
4. Source state/link, journal, audit, and outbox must commit atomically for posting.

A vNext design must make them jointly implementable. The safest first-step candidate is one in-process orchestration/UoW over the unchanged single DbContext, using module ports and database-owned transaction boundaries, while treating audit/outbox as transactional ports. A different choice is permitted only if it retains `no POSTED without balanced GL`, idempotency, reversal, failure atomicity, and recovery evidence.

This is an advisory design requirement, not authorization to implement or modify the database. It does not require another C2 reopen because v1.1 remains explicitly proposed, preserves cross-module settlement as unresolved, and does not claim implementation readiness.
