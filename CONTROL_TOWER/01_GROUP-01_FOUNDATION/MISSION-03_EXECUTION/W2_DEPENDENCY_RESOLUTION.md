# W2 Dependency Resolution

Baseline: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`; PR #69: `601f2d1cad61d62e590a6714ad84e307eb84fe5f`, evidence only.

Control Tower independently revalidated and rebound the bounded design determinations below. The remaining items continue to block only their own persistence/issuer/override packages.

| Dependency | Resolution | Residual bounded item | Effect |
|---|---|---|---|
| `DEP-005` | `CONTROL TOWER REVALIDATED` by ADR-W2-001 | live rows/roles/RLS | code-only tenant controls adopted; DBP-002 remains blocked |
| `DEP-006` | `CONTROL TOWER REVALIDATED FOR AUTHORITY-NEUTRAL CODE-ONLY IMPLEMENTATION` by ADR-W2-002 | `AUTH-001` Production external-vs-local authority | server membership/RBAC controls adopted; issuer-specific sessions/Production integration blocked |
| `DEP-007` | `CONTROL TOWER REVALIDATED FOR BOUNDED CODE-ONLY IMPLEMENTATION` by ADR-W2-003 | emergency override, external attestation/retention | fail-closed owner checks adopted; registry/PoP persistence blocked |

## Unknown separation

| Can be settled now | Disposable evidence | Live external evidence | Owner decision |
|---|---|---|---|
| Company-root hierarchy, current singular user assignment, claim-driven gaps, fail-closed target, Sync owner binding | code-only membership/owner/RBAC negative tests on empty migrated PostgreSQL | live row mismatches, applied history, DB roles/RLS, IdP/device/MDM configuration | `AUTH-001`; emergency override role; retention/legal policy |

No unresolved bounded item authorizes a guess. It blocks only the affected package recorded in `W2_EXECUTION_PLAN.md`.
