# TransportERP — Repository Layout and Ownership V1

## 1. Principle
Design documentation is separated from executable projects. The existing solution projects remain code ownership boundaries. Design specifications live under `documentation/design`.

## 2. Repository layout
```text
TransportERP/
├─ documentation/
│  ├─ architecture/
│  ├─ closeout/
│  ├─ recovery/
│  └─ design/
│     ├─ README.md
│     ├─ 00_DESIGN_OPERATING_MODEL_V1.md
│     ├─ 01_REPOSITORY_LAYOUT_AND_OWNERSHIP_V1.md
│     ├─ 02_SCREEN_WORKFLOW_AND_TEAM_HANDOFF_V1.md
│     ├─ 03_SCREEN_SPECIFICATION_TEMPLATE_V1.md
│     ├─ 04_SCREEN_WORK_QUEUE.csv
│     ├─ 05_COREUI_ADOPTION_RULES_V1.md
│     └─ screens/
│        └─ <Domain>/
│           └─ <ScreenCode>/
│              ├─ screen-spec.md
│              ├─ wireframe/
│              ├─ visual/
│              └─ review/
│
├─ TransportERP.Api/                 # code only
├─ TransportERP.Application/         # code only
├─ TransportERP.Contracts/           # code only
├─ TransportERP.Desktop/             # desktop code only
├─ TransportERP.Infrastructure/      # persistence/integration code only
├─ TransportERP.Mobile.Admin/        # code only
├─ TransportERP.Mobile.Customer/     # code only
├─ TransportERP.Mobile.Driver/       # code only
├─ TransportERP.Tests/               # tests only
└─ TransportERP/                     # domain code
```

## 3. Ownership boundaries
- `documentation/design`: design intent, screen contracts, visual evidence, review evidence.
- `TransportERP.Desktop/CoreUI`: executable shared UI implementation.
- `TransportERP.Desktop/<Domain>`: screen implementation only after authority permits implementation.
- `Contracts/Application/Api/Infrastructure/Domain`: remain governed technical layers; no design document may silently redefine them.

## 4. Per-screen folder rule
Create a per-screen folder only when the screen enters active design. The canonical screen specification is `screen-spec.md`. Wireframes, final visuals and review evidence are attachments to that specification, not competing authorities.

## 5. Kurrasa rule
The kurrasa remains governing source. Do not copy its full contents into the repository. Store only exact references/IDs and the derived screen-design artifact authorized by those references.
