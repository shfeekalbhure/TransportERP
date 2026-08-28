# ADR-W2-001 — Tenant hierarchy and cardinality

Control Tower disposition: `DEP-005 = CONTROL TOWER REVALIDATED` at execution baseline `9c5b7a12e59d2c42e682717b8e90c491f8699b96`. Live rows/roles/RLS and physical tenant defenses remain bounded to DBP-002 and do not block the code-only controls.

- Decision date: `2026-08-28`
- Execution baseline: `069a311b8f0e66f5d1ee3fdcffed13ec13d0a91a`
- Governing dependency: `DEP-005`
- Findings: `A-SEC-002`, `A-DB-003`, `A-DB-004`, `TB-F-003`, `TB-F-012`
- Decision: `DEP-005 = RESOLVED FOR EXECUTION DESIGN`
- Governance acceptance: `PENDING CONTROL TOWER INDEPENDENT REVALIDATION/REBIND UNDER c274f9a HOLD`
- DB execution: `NOT AUTHORIZED — DBP-002 remains gated`

## Evidence and current state

1. `P1Entities.cs` makes `Company` the only organization root and gives it a collection of `Branch`; every `Branch` has a non-null `CompanyId`.
2. `TransportErpDbContext.cs` enforces `Branch -> Company` and the alternate key `(Branch.Id, Branch.CompanyId)`. `BranchSetting` already demonstrates a tenant-consistent composite FK.
3. The current `User` has optional singular `CompanyId` and `BranchId`. There is no membership entity or collection. Current persisted cardinality is therefore `User 0..1 Company` and `User 0..1 Branch`, not proved multi-company or multi-branch membership.
4. The current `User.BranchId` FK is independent of `User.CompanyId`; a user row can therefore reference a branch owned by a different company unless application code prevents it.
5. `UserRole`, `RolePermission`, and `UserPermissionOverride` carry optional company/branch scope fields, but their keys and relationships do not fully enforce scope shape or branch/company consistency. `UserRole(UserId,RoleId)` also cannot represent the same role in multiple branch scopes.
6. HTTP modules derive `UserId`, `CompanyId`, `BranchId`, permissions and device flags directly from authenticated claims. The Sync service validates an active user ID but does not bind the stored user company/branch to the claimed scope.
7. Business repositories usually predicate both `CompanyId` and `BranchId`, which is a valuable current control, but there is no systemic tenant query filter or proven RLS policy.
8. Current migrations are the ten migrations verified in W0. No tenant-membership table or RLS policy exists in that lineage.
9. PR #69 strengthens composite FKs, request-time lookups and scoped permission evaluation, but retains the singular optional `User.CompanyId/BranchId` model and allows a company-null user to select a scope through `auth.scope.select`. It does not settle explicit multi-company membership and is evidence only.

## Decision

- `Company` is the tenant root and the security/data isolation boundary.
- `Branch` is a child of exactly one `Company`; a branch identifier is never authoritative without its company identifier.
- The target identity model supports one user with zero or more explicit active memberships. Each membership binds exactly one company and either one branch or a deliberately company-wide scope. Multi-company and multi-branch access are represented by multiple explicit memberships, never by null-as-wildcard.
- Every request executes in exactly one server-resolved `TenantContext`: `User + Membership + Company + optional Branch + effective permissions + session + optional registered device`.
- A token may carry a requested scope selector or opaque membership/session handle, but the server must reconcile it with current active persistence before authorization. A body/query/header cannot create or widen tenant scope.
- The following are never client-authoritative: company access, branch access, platform access, role, permission, device registration/trust, membership status, or override authority.
- Company-wide operations are explicit and permission-scoped. Branch-scoped product operations require a branch and preserve current company/branch predicates until stronger DB controls pass parity tests.
- Background workers use a named workload identity and an explicit tenant work item; they do not synthesize a platform user or consume interactive client claims.

## Proposed target cardinalities

| Relationship | Target cardinality | Rule |
|---|---:|---|
| Company → Branch | `1 : 0..N` | branch belongs to exactly one company |
| User → Membership | `1 : 0..N` | no implicit membership from a claim |
| Membership → Company | `N : 1` | exactly one tenant root |
| Membership → Branch | `N : 0..1` | null means explicit company-wide membership, not all companies |
| Request → TenantContext | `1 : 1` | one selected active membership per request |
| Role grant → Membership/scope | `N : 1` or explicit scope tuple | no scope-free wildcard by null inference |
| Device assignment → Membership | `N : 1` | device use is bound to user/company/branch membership |

## Alternatives considered

| Alternative | Result | Reason |
|---|---|---|
| Keep singular `User.CompanyId/BranchId` as the permanent model | Rejected as target | cannot safely express approved selection across explicit memberships and encourages null-as-wildcard logic |
| Treat JWT company/branch claims as authority | Rejected | confirmed current finding; revocation and membership changes would not be authoritative at request time |
| Treat `User.CompanyId = null` as platform access | Rejected | absence of scope is not an entitlement |
| Company-only isolation without branch binding | Rejected | current product, Sync and negative-test requirements are branch-aware |
| RLS alone | Rejected | cannot replace application authorization/session/device decisions and live role topology is unknown |

## Consequences

- Security: scope resolution fails closed on missing, inactive, ambiguous, mismatched or stale membership. Bidirectional A↔B tests are mandatory.
- Database: DBP-002/003 must stage memberships, tenant-consistent FKs/checks/indexes and reviewed RLS/equivalent. Existing IDs and current predicates must be preserved. No DB change is authorized by this ADR.
- API: all endpoint modules converge on one resolver/authorization pipeline; query/body company/branch values are filters constrained by resolved context, not authority.
- Sync: enqueue and every lifecycle mutation bind to the resolved company, branch, user and device. Non-owner access is denied unless a later narrowly governed override exists.
- Migration: additive membership backfill must be rehearsed on a safe copy, detect ambiguous/null/mismatched users and stop rather than infer. Cutover and compatibility require a forward migration and recovery plan.
- Rollback: code-only validation changes are independently revertible. A future membership migration uses forward correction or safe-copy restore; it must not drop legacy columns until parity and recovery gates pass.

## Required negative tests

- Company A user/token/request against Company B and the reverse.
- Branch A against Branch B within one company and the reverse.
- Branch identifier paired with the wrong company.
- Active token after membership disable/remove.
- User with no membership, ambiguous memberships without a selector, inactive company/branch, and null-as-platform attempts.
- Permission grant/deny in the wrong company or branch.
- Worker work item with a tenant different from its resolved workload grant.
- Direct SQL insertion/update of mismatched tenant relationships after DBP-002.

## Bounded unknowns

| Unknown | Classification | Blocks |
|---|---|---|
| Live users with null/mismatched company/branch and actual role scope population | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` | DBP-002 backfill/execution only |
| Live PostgreSQL roles/RLS/equivalent and applied migration history | `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION` | physical DB defense choice/execution only |
| Exact Production issuer/workload identity configuration | external evidence | Production integration, not this cardinality decision |
