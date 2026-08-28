# Unknown and Blockers Register

No row below is disguised as fact. Each row blocks only the named later gate.

| ID | Unknown / current evidence | Resolution action | Owner decision? | Blocks |
|---|---|---|---|---|
| `M02-BLK-001` | `dotnet` absent locally; existing master CI is partial | disposable exact-master restore/build/test/migrate/boot/client matrix | no | W0 exit, later implementation evidence |
| `M02-BLK-002` | live schema/data/migration history/roles/RLS/backups and affected Volume rows inaccessible | approved read-only inventory and impact query on safe copy; restore drill | only for Production/data mutation | DBP-001/002 and release |
| `M02-BLK-003` | external IdP/session/revocation/device configuration unavailable | redacted configuration and authorized sandbox negative tests | Production IdP change only | W2 security exit |
| `M02-BLK-004` | Production deploy/privacy/rollback/recovery topology unavailable | non-secret topology, runbooks, artifact and backup/restore drill | Production/release action yes | W7 release |
| `M02-BLK-005` | latest canonical Kurrasa, screens and offline authority not registered | immutable version/supersession and requirement→screen→operation crosswalk | canonical authority may require owner | W4 offline actions, W5/W6 affected scope |
| `M02-BLK-006` | full local/external workspace ownership and value incomplete | global inventory, hashes/bundles, semantic review | yes before destructive disposition | cleanup/delete/merge/history rewrite |
| `M02-BLK-007` | accounting mappings, periods, SoD, reversal and subledger rules incomplete | reviewed canonical accounting record and ADR | yes if rule choice reserved | W3 accounting implementation |
| `M02-BLK-008` | cross-module UoW owner unresolved (`E-BLK-013`) | approve ADR: initial single DbContext orchestration or revised consistency model | architecture approval; owner only if reserved | W3 and affected later waves |
| `M02-BLK-009` | offline authority conflicts with version-bound `OFFLINE_WRITE=0`; PR69 has five available candidate actions | operation-level authority matrix; keep all Production offline writes disabled meanwhile | yes for new business-write authority | W4 activation |
| `M02-BLK-010` | master Sync lifecycle exposure and privileged non-owner override policy incomplete | enumerate every caller/route; require owner binding or explicit reasoned audited override | security approval | W2/W4 safe exposure |
| `M02-BLK-011` | EF design-time execution unproved | isolated exact-SHA EF command with synthetic `TRANSPORTERP_DESIGN_CONNSTR` | no | DB tooling claim |
| `M02-BLK-012` | PR69 semantic adoption not independently accepted | complete component/finding review, tests and DB-GOV; never merge as shortcut | any merge yes | all PR69 adoption |
| `M02-BLK-013` | resolved dependency/advisory/license graph unknown | capture current and candidate graphs; lock/SBOM/SCA/license review | policy exceptions may require owner | W7 supply gate |
| `M02-BLK-014` | exact release/mobile delivery scope and signing authority unknown | approved topology/platform/signing decision | Production/distribution authority yes | W5/W7 |
| `M02-BLK-015` | Ticketing and post-departure Shipping canonical acceptance incomplete | authoritative requirements and numbered cases | scope decision if absent | W6 |
| `BLK-B-001` | TEAM-B single-session provenance | retain limitation; require independent M03 implementation review and M04 verification | no | does not block M03 intake; affects assurance narrative |

## Owner-decision activation rule

There is no active owner hold on completing or sealing this plan. A hold activates when an actual proposed step would mutate Production, repair/destructively alter data, delete preserved assets, merge/delete/force-push/rewrite history, accept an irreversible risk, or choose a canonically unresolved business/accounting/offline rule reserved to the owner.
