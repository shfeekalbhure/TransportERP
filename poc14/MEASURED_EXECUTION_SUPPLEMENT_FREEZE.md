# TransportERP POC-14 — Measured Execution Supplement Freeze

Status: POC-ONLY / NO PRODUCTION AUTHORIZATION.

This supplement is frozen before any PostgreSQL/MySQL measured result on this branch. It does not modify the previously frozen workload matrix, scoring weights, RPO/RTO targets, acceptance gates, V4.1R2, W1, or production code.

Candidate runtime environment: PostgreSQL 18.x/PostGIS 3.6.x and MySQL 8.4.10 in isolated containers on the same GitHub Actions runner. The workflow must record exact runtime versions before measurement.

The earlier pre-execution manifest named PostGIS 3.6.2. Current upstream container documentation reports a newer 3.6.x line. This is a pre-result deviation; spatial evidence cannot close final POC-14 until the exact runtime version is accepted through governance or rerun on the originally frozen version.

Pilot workload subset: W-001, W-014, W-016, W-017, W-018, W-021, W-022, W-028 and W-029. This subset is real measured evidence but is not full POC-14 completion. Full 30-workload execution, HA/failover/PITR and independent review remain gates.

Fairness: same runner, same logical data, same program, identical CPU/memory caps, alternating candidate order, reset database before each measured repetition, raw artifacts retained, no selective rerun.

FINAL DATABASE SELECTION = OPEN.
