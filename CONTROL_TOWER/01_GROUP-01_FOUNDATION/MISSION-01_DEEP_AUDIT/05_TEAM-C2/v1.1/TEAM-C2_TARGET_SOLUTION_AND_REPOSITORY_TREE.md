# TEAM-C2 Target Solution and Repository Tree

Status: `PROPOSED — NOT IMPLEMENTED`. Assembly creation, deletion, move, rename, or split requires a separately approved implementation plan and preservation proof. `AUTHORITATIVE CURRENT LINE` remains unknown.

## 1. Candidate target Visual Studio solution tree

```text
TransportERP.slnx
├── 00-Build-and-Governance
│   ├── build configuration / analyzers / package policy        [files, not business runtime]
│   └── CONTROL_TOWER                                           [governance artifacts]
├── 01-Building-Blocks
│   ├── TransportERP.SharedKernel                               [PROPOSED]
│   ├── TransportERP.Contracts                                  [EXISTING; narrow/version]
│   └── TransportERP.Platform.Infrastructure                    [PROPOSED extraction]
├── 02-Modules
│   ├── TransportERP.Modules.Organization                       [PROPOSED]
│   ├── TransportERP.Modules.IdentityAccess                     [PROPOSED]
│   ├── TransportERP.Modules.Accounting                         [PROPOSED]
│   ├── TransportERP.Modules.Waybills                           [PROPOSED from partial runtime]
│   ├── TransportERP.Modules.Shipping                           [PROPOSED from partial runtime]
│   ├── TransportERP.Modules.Ticketing                          [PROPOSED; not currently implemented]
│   ├── TransportERP.Modules.OfflineSync                        [PROPOSED; policy-gated]
│   ├── TransportERP.Modules.AuditCompliance                    [PROPOSED]
│   └── TransportERP.Modules.Reporting                          [PROPOSED; not currently implemented]
├── 03-Hosts
│   ├── TransportERP.Api                                        [EXISTING; composition host]
│   └── TransportERP.Worker                                     [PROPOSED; only when authorized]
├── 04-Clients
│   ├── TransportERP.Desktop                                    [EXISTING asset; target executable]
│   ├── TransportERP.Mobile.Admin                               [EXISTING placeholder; proposed app]
│   ├── TransportERP.Mobile.Customer                            [EXISTING placeholder; proposed app]
│   └── TransportERP.Mobile.Driver                              [EXISTING placeholder; proposed app]
└── 05-Tests
    ├── TransportERP.Tests.Unit                                 [PROPOSED split]
    ├── TransportERP.Tests.Architecture                         [PROPOSED]
    ├── TransportERP.Tests.Contracts                            [PROPOSED]
    ├── TransportERP.Tests.PostgreSql                           [PROPOSED split]
    ├── TransportERP.Tests.Api                                  [PROPOSED split]
    ├── TransportERP.Tests.Offline                              [PROPOSED]
    ├── TransportERP.Tests.Desktop                              [PROPOSED]
    └── TransportERP.Tests.EndToEnd                             [PROPOSED]
```

This is a candidate steady-state tree, not a command to create all projects immediately. The first authorized wave may retain the current Domain/Application/Infrastructure assemblies and introduce the same module boundaries as folders plus architecture tests. Project extraction is permitted only when dependency, transaction, migration, and deployment impact is evidenced.

## 2. Candidate target physical repository tree

```text
/
├── global.json                                                [PROPOSED SDK pin]
├── Directory.Build.props                                      [PROPOSED common build policy]
├── Directory.Packages.props                                   [PROPOSED central versions]
├── NuGet.config                                               [PROPOSED approved sources]
├── TransportERP.slnx
├── .github/
│   ├── workflows/
│   └── dependency-policy/
├── build/
│   ├── analyzers/
│   ├── scripts/
│   └── release/
├── src/
│   ├── BuildingBlocks/
│   │   ├── SharedKernel/
│   │   ├── Contracts/
│   │   └── Platform.Infrastructure/
│   ├── Modules/
│   │   ├── Organization/
│   │   ├── IdentityAccess/
│   │   ├── Accounting/
│   │   ├── Waybills/
│   │   ├── Shipping/
│   │   ├── Ticketing/
│   │   ├── OfflineSync/
│   │   ├── AuditCompliance/
│   │   └── Reporting/
│   ├── Hosts/
│   │   ├── Api/
│   │   └── Worker/
│   └── Clients/
│       ├── Desktop/
│       │   ├── Shell/
│       │   ├── SharedUI/{RTL,Resources,Validation,Dialogs,Lookups}/
│       │   └── Features/{Setup,Accounting,Waybills,Shipping,Ticketing,Reports}/
│       └── Mobile/{Admin,Customer,Driver,SharedClient}/
├── database/
│   ├── governance/                                            [change proposals/impact/recovery]
│   ├── migrations/                                            [existing lineage preserved first]
│   ├── verification/                                          [fresh/upgrade/drift/restore tests]
│   └── operations/                                            [runbooks; no secrets]
├── tests/
│   ├── Unit/
│   ├── Architecture/
│   ├── Contracts/
│   ├── PostgreSql/
│   ├── Api/
│   ├── Offline/
│   ├── Desktop/
│   ├── Mobile/
│   └── EndToEnd/
├── docs/
│   ├── architecture/{ADRs,contexts,dependencies,data,security,offline}/
│   ├── requirements/{canonical,crosswalks}/
│   ├── operations/{install,upgrade,rollback,restore}/
│   └── release/
└── CONTROL_TOWER/                                             [governing audit/work cycle]
```

## 3. Internal module template

```text
ModuleName/
├── Domain/             aggregates, value objects, invariants, events
├── Application/
│   ├── Commands/
│   ├── Queries/
│   └── Ports/
├── Contracts/          public/versioned boundary only
├── Infrastructure/
│   ├── Persistence/
│   ├── Integration/
│   └── Configuration/
└── Composition/        module registration/endpoints, no business rules
```

The template is selective. A module that has no infrastructure or public contract must not add empty folders to simulate completeness.

## 4. Placement answers required by the master command

| Concern | Target placement |
|---|---|
| Accounting | `src/Modules/Accounting` + client `Features/Accounting` + accounting tests |
| Waybills | `src/Modules/Waybills` |
| Shipping / trips / custody | `src/Modules/Shipping` |
| Ticketing / passengers | `src/Modules/Ticketing` |
| Setup / master data | Organization + IdentityAccess modules; client Setup features |
| Reports | Reporting module + client Reports features |
| Screens | each client under `Features/<Module>` |
| Shared dialogs/lookups | client `SharedUI/Dialogs` and `SharedUI/Lookups`, backed by typed query contracts |
| Shared services | minimal BuildingBlocks/Platform; domain-specific services remain in their module |
| API contracts | module/public Contracts; HTTP-only models in API/module composition |
| Database/migrations | existing lineage preserved; proposed `database/` governance/verification surface |
| Offline | OfflineSync module + authorized shared client engine + Worker |
| Mobile | `src/Clients/Mobile/{Admin,Customer,Driver}` |
| Tests | separated under `tests/` and solution folder `05-Tests` |

## 5. Preservation boundary for tree changes

Before any physical move or project split:

1. name the authoritative source ref/full SHA;
2. hash/preserve all local/unmerged assets;
3. establish a path/namespace/API/screen/migration trace map;
4. prove unchanged runtime/contract behavior at exact SHA;
5. retain Git history where practical and record supersession where not;
6. keep migration names/order/snapshot and database meanings unchanged unless an approved forward migration says otherwise;
7. prohibit removal of prototype/shared-looking types until direct and external consumers are checked;
8. obtain owner authority for destructive or irreversible operations.

