# Wave Execution Register

| Wave | Entry result | Work performed | Exit result | Product modification | Next action |
|---:|---|---|---|---|---|
| `W0` | `PASS — START AUTHORIZED AND M02 v1.2 HASHES VERIFIED` | exact refs/trees, Git state, 50 remote heads, two worktrees, empty local stash, 378-file tree, 10 projects, 10 migrations, 22 C# test files, 103 static Fact/Theory attributes, 7 workflows, PR69 delta, bundle and recovery test, runtime/CI probes | `BLOCKED — T-000 AND EXTERNAL PRESERVATION INVENTORY INCOMPLETE` | none | provide executable .NET/PostgreSQL/client environment and external workspace inventory; rerun T-000 |
| `W1` | `FAIL-CLOSED — W0 exit and DBP-001 review absent` | static prerequisite check only | `NOT STARTED` | none | review DBP-001 and satisfy W0 |
| `W2` | dependency gate unmet | none | `NOT STARTED` | none | W1 then tenant/IdP/DB authority |
| `W3` | dependency/canonical authority unmet | none | `NOT STARTED` | none | approved UoW/accounting ADR and DB entries |
| `W4` | dependency/offline authority unmet | none | `NOT STARTED` | none | W2/W3 and operation-level authority |
| `W5` | dependency/scope/signing unmet | none | `NOT STARTED` | none | stable W2/W4 and client environments |
| `W6` | dependency/canonical requirements unmet | none | `NOT STARTED` | none | governed requirements and DB entries |
| `W7` | stable candidate/external evidence absent | none | `NOT STARTED` | none | prior waves plus release/recovery/privacy evidence |
| `W8` | must remain last | none | `NOT STARTED` | none | W7 stable parity baseline |
