# MASTER/GATE Domain Coverage Matrix

| Domain | Final audit coverage | Governing inputs | Remaining gate effect |
|---|---|---|---|
| Governance/evidence assurance | REVIEWED | A/B/C1/D/C2/E + central records | authority line unresolved; B limitation retained |
| Architecture/Solution | REVIEWED SNAPSHOT | C1 v1.1, D v1.1, C2 v1.1, E v1.1 | target conditional; E-BLK-013 |
| Database/Migrations | REVIEWED STATIC / LIVE BLOCKED | A/B/D/E; DB-GOV-001 | blocks DB/remediation/release readiness |
| Security/Authentication | REVIEWED STATIC / EXTERNAL BLOCKED | A/B/D/E | IdP/session evidence unknown |
| Multi-Tenant/RBAC | REVIEWED STATIC / RUNTIME PARTIAL | A/B/D/E | systemic defense and runtime negatives absent |
| Offline/Sync | REVIEWED STATIC | A/B/D/C2/E | foundation only; lifecycle ownership gap; operation authority absent |
| Accounting/Finance | REVIEWED STATIC | A/B/D/C2/E | posting rules/live DB/UoW unresolved |
| Audit/Compliance | REVIEWED STATIC | A/B/D/C2/E | hash/atomicity/live enforcement partial |
| Desktop | REVIEWED STATIC | A/B/C1/D/E | disconnected Library/prototype |
| Mobile | REVIEWED STATIC | A/B/C1/D/E | source-empty placeholders |
| Waybill/Shipping | REVIEWED CRITICAL PATH | A/B/C1/D/C2/E | Volume P0; lifecycle partial |
| Ticketing/Passenger | REVIEWED INVENTORY | A/B/C1/D/C2/E | absent; canonical scope required |
| Screens/UX/RTL | REVIEWED VERSION-BOUND | A/B/C1/D/C2/E | canonical ID/version unresolved |
| Tests/Acceptance | REVIEWED STATIC | A/B/C1/D/E | exact-target execution absent |
| CI/CD/Supply Chain | REVIEWED CONFIG | A/B/D/C2/E | locks/SBOM/SCA/provenance/artifacts incomplete |
| Release/Deployment/Recovery | REPOSITORY REVIEWED / EXTERNAL BLOCKED | A/B/D/E | blocks READY |
| Privacy/Sensitive Data | PARTIAL / EXTERNAL BLOCKED | A/B/D/E | environmental/legal controls unknown |
| Kurrasa/Requirements Authority | PARTIAL / VERSION-BOUND | A/B/D/E | blocks affected implementation scope |
| Git/PR/Workspace Preservation | REVIEWED FOR LISTED ASSETS | A/B/D/E | external inventory/disposition incomplete; P0 preservation |
| Reporting | REVIEWED INVENTORY/TARGET | D/C2/E | subsystem absent; proposal only |

No required domain is omitted. `PARTIAL`, `BLOCKED`, and snapshot-only states remain explicit and prevent a false READY determination.
