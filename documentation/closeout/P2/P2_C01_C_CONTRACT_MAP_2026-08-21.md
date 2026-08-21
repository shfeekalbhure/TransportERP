# TransportERP — P2-C01-C Contract Map

**Baseline:** `master@22ee24108b3c682d94e9d8693a566d6b479f19c9`

| Runtime area | W1 | W2 | Governed screen |
|---|---|---|---|
| Release quantity | W1-P2C01-004; W1-P2C01-009; W1-P2C01-026 | W2-P2C01-014 | SHP-015; SHP-019 |
| Create Trip / planned route | W1-P2C01-010; W1-P2C01-011 | W2-P2C01-015 | SHP-025 |
| Allocate / unallocate | W1-P2C01-009; W1-P2C01-010; W1-P2C01-012; W1-P2C01-026 | W2-P2C01-016; W2-P2C01-017 | SHP-016; SHP-023; SHP-024 |
| Generate Manifest | W1-P2C01-010; W1-P2C01-012; W1-P2C01-013; W1-P2C01-014 | W2-P2C01-018 | SHP-023; SHP-024; SHP-028 |
| Load allocated quantity | W1-P2C01-012; W1-P2C01-014; W1-P2C01-015; W1-P2C01-026 | W2-P2C01-019 | SHP-027; SHP-028 |
| Finalize Manifest | W1-P2C01-012; W1-P2C01-013; W1-P2C01-014 | W2-P2C01-039 | SHP-028 |
| Handover / driver custody | W1-P2C01-010; W1-P2C01-013 | W2-P2C01-020 | SHP-029 |
| Start Trip / depart | W1-P2C01-010; W1-P2C01-013; W1-P2C01-015 | W2-P2C01-021 | SHP-030 |

## Boundary notes

- `W1-P2C01-015 MovementEvent` is realized only for the append-only `LOAD` and `DEPART` evidence required by C. Movement inquiry, ARRIVE, UNLOAD, REALLOCATE and DELIVER remain later.
- `W1-P2C01-026 WaybillHold` is a blocker dependency only. There is no Hold/ReleaseHold API or production screen in C.
- `TripStop` in C stores planned route data only. Actual stop arrival/departure belongs to the arrival/transit package.
- Vehicle and driver identifiers are references only; no Fleet tables are created.
- C introduces no revenue-posting behavior and does not change B financial status rules.
