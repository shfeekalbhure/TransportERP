# TEAM-E Domain Coverage Matrix

- Status: `FINAL — SEALED`

| Domain | TEAM-E coverage | Review lenses | Evidence | Result / remaining critical gap |
|---|---|---|---|---|
| Governance/evidence assurance | REVIEWED | governance + technical | E-EV-001..005,022..025 | reopen defects preserved; accepted C1/D/C2 v1.1 chain verified; BLK-B-001 mitigated without changing B provenance |
| Architecture/Solution | REVIEWED | enterprise/software/release | E-EV-021,024,025 | conditional C2 v1.1 direction suitable; transaction ADR and authority pending |
| Database/Migrations | REVIEWED STATIC / LIVE BLOCKED | PostgreSQL/EF/security/accounting | E-EV-006,009,012,013 | DB-GOV-001; live state/recovery/Volume population unknown |
| Security/Authentication | REVIEWED STATIC / EXTERNAL BLOCKED | security/architecture/QA | E-EV-008 | claims foundation; IdP/session evidence unknown |
| Multi-Tenant/RBAC | REVIEWED STATIC / RUNTIME PARTIAL | security/DB/application | E-EV-008,009 | user/tenant binding and systemic DB defense incomplete |
| Offline/Sync | REVIEWED STATIC | security/offline/DB/QA | E-EV-010,024,025 | foundation only; lifecycle ownership gap reconciled and assigned owner-bound target treatment; exposure/runtime remains conditional |
| Accounting/Finance | REVIEWED STATIC | accounting/DB/application | E-EV-012,013 | status-only posting; canonical rules/live DB unknown |
| Audit/Compliance | REVIEWED STATIC | compliance/DB/security | E-EV-011,013 | hash/atomicity partial; live enforcement unknown |
| Desktop | REVIEWED STATIC | WinForms/UX/QA/release | E-EV-014,019 | Library/prototype; executable/runtime evidence absent |
| Mobile | REVIEWED STATIC | MAUI/Android/security/release | E-EV-014 | source-empty placeholders; platform/signing unknown |
| Waybill/Shipping | REVIEWED CRITICAL PATH | logistics/DB/accounting/offline | E-EV-006,015 | Volume P0; lifecycle partial |
| Ticketing/Passenger | REVIEWED INVENTORY | ticketing/domain/accounting | E-EV-015 | absent on snapshot; canonical requirements required |
| Screens/UX/RTL | REVIEWED VERSION-BOUND | UX/domain/governance | E-EV-014,019 | screen/version authority unresolved |
| Tests/Acceptance | REVIEWED STATIC | QA/domain/security | E-EV-016,020 | exact-target execution absent |
| CI/CD/Supply Chain | REVIEWED CONFIG | DevOps/security/release | E-EV-016,017 | client/artifact/SBOM/SCA/provenance gaps |
| Release/Deployment/Recovery | REPOSITORY REVIEWED / EXTERNAL BLOCKED | release/operations/DB | E-EV-017 | artifact→install→upgrade/restore chain unproved |
| Privacy/Sensitive Data | PARTIAL | privacy/security/operations | E-EV-018 | data surfaces known; legal/environment controls blocked |
| Kurrasa/Governance Authority | PARTIAL VERSION-BOUND | governance/domain/UX | E-EV-019 | latest authority unknown |
| Git/PR/Workspace Preservation | REVIEWED FOR LISTED ASSETS | governance/release | E-EV-007 | external inventory/disposition incomplete; P0 preservation |
| Reporting | REVIEWED INVENTORY/TARGET | accounting/domain/security | D/C2 sealed inputs, E-EV-021 | subsystem absent; target proposed only |

No critical domain is omitted. `PARTIAL/BLOCKED` states remain explicit gate constraints.
