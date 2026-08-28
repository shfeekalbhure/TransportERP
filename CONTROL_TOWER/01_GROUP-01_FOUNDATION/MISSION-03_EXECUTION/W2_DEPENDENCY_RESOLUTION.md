# W2 Dependency Resolution

Baseline: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`; PR #69: `601f2d1cad61d62e590a6714ad84e307eb84fe5f`, evidence only.

Worker design determinations below are submitted as candidates. Superseding Control Tower head `c274f9a...` requires independent verification and rebinding before any determination releases a Product execution gate.

| Dependency | Resolution | Residual bounded item | Effect |
|---|---|---|---|
| `DEP-005` | `RESOLVED FOR EXECUTION DESIGN` by ADR-W2-001 | live rows/roles/RLS | opens code-only tenant controls; DBP-002 remains blocked |
| `DEP-006` | `RESOLVED FOR IMPLEMENTATION` of authority-neutral request pipeline by ADR-W2-002 | `AUTH-001` Production external-vs-local authority | opens server membership/RBAC controls; local sessions/Production IdP integration blocked |
| `DEP-007` | `RESOLVED FOR IMPLEMENTATION` by ADR-W2-003 | emergency override, external attestation/retention | opens fail-closed owner checks; registry/PoP persistence blocked |

## Unknown separation

| Can be settled now | Disposable evidence | Live external evidence | Owner decision |
|---|---|---|---|
| Company-root hierarchy, current singular user assignment, claim-driven gaps, fail-closed target, Sync owner binding | code-only membership/owner/RBAC negative tests on empty migrated PostgreSQL | live row mismatches, applied history, DB roles/RLS, IdP/device/MDM configuration | `AUTH-001`; emergency override role; retention/legal policy |

No unresolved bounded item authorizes a guess. It blocks only the affected package recorded in `W2_EXECUTION_PLAN.md`.
