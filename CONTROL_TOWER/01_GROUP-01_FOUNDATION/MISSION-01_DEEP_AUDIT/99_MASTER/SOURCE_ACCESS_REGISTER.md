# MASTER/GATE Source Access Register

| Source/environment | Access state | Use in Master/Gate | Constraint |
|---|---|---|---|
| Governing `CONTROL_TOWER/` files | AVAILABLE | direct governing source | sole operational authority |
| Centrally accepted sealed team packages | AVAILABLE | main synthesis evidence | versioned/snapshot-bound |
| Detached checksum lists | AVAILABLE | package integrity verification | no semantic/runtime proof by hash alone |
| Product source tree | NOT DIRECTLY ACCESSED BY MASTER | accepted D/E reconciliation only | no new product finding asserted |
| Product authoritative line | `UNKNOWN — REQUIRES VERIFICATION` | gate blocker | default/observed ref is not authority |
| GitHub/PR latest state | NOT DIRECTLY ACCESSED BY MASTER | accepted line register only | moving state; no present-tense claim |
| Exact-SHA build/test environment | ACCESS BLOCKED | none | no restore/build/test/migrate/boot PASS |
| Live/Production database | ACCESS BLOCKED | none | `DB-GOV-001`; no schema/data/affected-row claim |
| IdP/session/device environment | ACCESS BLOCKED | none | external control and exploitability unknown |
| Production/deployment/recovery environment | ACCESS BLOCKED | none | no artifact/install/upgrade/rollback/restore claim |
| Latest Kurrasa/Library authority | PARTIAL / VERSION-BOUND VIA PACKAGES | requirement-gap qualification | latest canonical version unknown |
| External developer/Codex workspaces | ACCESS BLOCKED / INCOMPLETE | preservation-only conclusion | global inventory not proved |

Unavailable items are `ACCESS BLOCKED — UNKNOWN — REQUIRES VERIFICATION`. Their absence from Master access is not evidence of nonexistence.
