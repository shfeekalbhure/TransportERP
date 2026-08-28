# TEAM-E P2/P3 Sample Matrix

- Status: `FINAL v1.1 — SEALED`
- Revalidation: all `8/8` P2/P3 rows were re-read during v1.1 reissue; no disposition changed.
- Population: `8` original findings (`6 P2`, `2 P3`).
- Selection: full census because the population is small; this exceeds a smaller justified sample and covers architecture, UI/API, QA, audit, preservation, prototype divergence, and repository/build layout.
- Limit: primarily static/snapshot review; no TEAM-E runtime execution.

| ID | P | Domain | Direct/reconciled evidence | TEAM-E result | C2 treatment |
|---|---:|---|---|---|---|
| `A-ARCH-005` | P2 | Desktop/application | forms expose contracts/events without host/client/subscriber composition | CONCUR | executable client/screen crosswalk proposed |
| `A-ARCH-006` | P2 | API/UI duplication | repeated boundary helpers and RTL/form mechanics | CONCUR | common tested pipeline/shared UI proposed |
| `A-QA-005` | P2 | Coverage | coverlet reference without threshold/upload/retained coverage gate | CONCUR | exact-SHA coverage/artifact gate proposed |
| `TB-F-013` | P2 | Audit integrity | hash omits persisted fields | CONCUR | versioned backward-compatible hash proposed |
| `TB-F-016` | P2 | Git/workspace | divergent/unmerged/local assets | CONCUR | preservation/semantic disposition gate proposed |
| `TB-F-017` | P2 | Prototype divergence | in-memory P1 service is test/prototype, not API-composed runtime | CONCUR | consumer/semantic parity before removal |
| `A-ARCH-012` | P3 | Physical layout | Domain placement differs; solution flat | CONCUR | staged target tree; no forced move |
| `TB-F-021` | P3 | Build/layout conventions | SDK/package/layout debt | CONCUR | build policy/locks/tree proposed with preservation |

No sample result changes a TEAM-D determination. Cleanup remains subordinate to P0 preservation and exact-SHA parity.
