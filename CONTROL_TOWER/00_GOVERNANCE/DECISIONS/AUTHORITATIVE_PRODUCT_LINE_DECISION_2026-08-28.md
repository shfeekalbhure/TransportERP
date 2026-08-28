# AUTHORITATIVE PRODUCT LINE DECISION — 2026-08-28

## Decision

Owner authority designates the authoritative current product line for GROUP-01 governance and MISSION-01 revalidation as:

`refs/heads/master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`

Repository: `shfeekalbhure/TransportERP`

Decision state: `APPROVED — OWNER AUTHORITY`

Decision date: `2026-08-28` (Asia/Aden)

## Classification of other relevant lines

- `governance/control-tower-20260828`: governance/workspace line only; not the authoritative product line.
- PR #69 / `codex/p1-security-device-sync-offline-20260825@601f2d1cad61d62e590a6714ad84e307eb84fe5f`: `UNMERGED REMEDIATION / FINAL CANDIDATE`; remains `OPEN + DRAFT + UNMERGED`; it is not promoted to CURRENT and no merge is authorized by this decision.
- Other branches/PRs/local-only work remain classified according to existing preservation and reconciliation registers unless separately changed by evidence and authority.

## Effect

1. The authoritative-line blocker is resolved.
2. MASTER/GATE must be reopened for revalidation on the exact authoritative SHA above.
3. Revalidation must preserve all other critical evidence gaps and P0/P1 constraints; this decision does not mark the gate READY by itself.
4. MISSION-02 may start only if the revalidated sealed gate becomes `READY FOR REMEDIATION PLANNING`.
5. PR #69 may be used as comparative remediation evidence during planning but must not be merged or treated as current product state without a separate authorized gate.
6. No Source, Tests, Migrations, Database, Production, merge, cleanup, or destructive Git action is authorized by this decision.

## No-guessing rule

All claims after this decision must be revalidated against `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5` or explicitly classified as evidence from another line.
