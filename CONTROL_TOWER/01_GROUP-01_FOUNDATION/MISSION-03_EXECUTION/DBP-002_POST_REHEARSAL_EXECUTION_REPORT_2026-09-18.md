# DBP-002 POST-REHEARSAL EXACT-HEAD EXECUTION REPORT

Date: 2026-09-18
Package: MISSION-03-DBP002-POST-REHEARSAL-v1.2
Evidence source: GitHub repository, GitHub Actions run metadata, job steps/logs, and GitHub Actions artifact metadata only.

## Frozen candidate identity

- Branch at execution time: `codex/mission-03-execution-20260828`
- SHA: `ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`
- Tree: `e828941817432bdc73f3e6fc31e74219e74fcf33`
- Parent: `f128d24dce7baf76a6ac8af4e62a331b80447311`
- Commit message: `MISSION-03 DBP-002: trigger full rehearsal after authoritative fixture fix`

This package is evidence packaging for the frozen DBP-002 candidate. It is not a Product change and is not a DBP-002 governance acceptance decision.

## Exact-head run result

| Run | Workflow | Job(s) | Exact head | Result |
|---:|---|---|---|---|
| 33222541097 | MISSION-03 DBP-002 Full Rehearsal v3 | 99019447103 | ffdf1087... | SUCCESS |
| 33222541108 | MISSION-03 W0 Disposable Baseline | 99019515884, 99019515734 | ffdf1087... | SUCCESS |
| 33222541109 | MISSION-03 W7 Disposable Recovery | 99019447043 | ffdf1087... | SUCCESS |
| 33222541073 | MISSION-03 DBP-002 Full Rehearsal v2 | 99019446824 | ffdf1087... | FAILURE — retained |

All four runs are bound by GitHub to the same frozen SHA. v3, W0, and W7 independently emit the same tree/parent identity where those jobs capture it.

## Full Rehearsal v3

Run 33222541097 completed its exact-head end-to-end step and evidence upload successfully on PostgreSQL 18.6.

Recorded candidate/source SHA-256 values:

- `fea6220fbd8bc8d1a7bda8890f058166cb5b978f684663e1bbc96f87bdabbd9a` — `20260828224241_GreenfieldTenantMembershipIsolation.cs`
- `73d7eed6a5915820b45326e1239b540eb5a68d079960cbbb8aeb7267733b1b4c` — candidate migration Designer
- `fd7314fc074fa80474323d1745cdd7f381c8ad426d7c90330cd64e932b1ca5d4` — `TransportErpDbContextModelSnapshot.cs`
- `8217e320b51786741d0cd3eb8b11d5413b9b5f31e564eacca1b4ebd486427c9b` — `GreenfieldDbp002PhysicalSql.cs`
- `0559f0ca64a30545ad29010e2a5eb3edb46b294cad88d2cbbefcf3b0d6af64d9` — `PersistentPermissionResolver.cs`
- `b5ff58afb259023e713d50379c9c110dc4d1c292503c0d46d68e2f2f96c8c044` — generated Migration 11 SQL
- `574a924dc9709b65c9bf6b94376d8205761869bcfd9a1875b9616d6cbae05630` — baseline dump
- `d6164a4291aa28d530a97e6d2dd2a81de414ad785309a3b2263d3ae1b2874ec0` — final candidate dump

The v3 log records:
- no pending EF model changes;
- semantic generated-SQL versus EF physical-state reconciliation;
- RLS scope and negative-path checks;
- full regression `Passed 155; Failed 0; Skipped 0`;
- final candidate backup/restore structural reconciliation;
- terminal marker `DBP-002 TECHNICAL REHEARSAL PASS` with the frozen SHA/tree/parent.

Artifact 9705722045 is retained and unexpired at package preparation time. GitHub reports artifact digest:
`sha256:232fd71285c2675dc7fbcdf9ad207d6d97fbe407a222fe6ac98db8729732527c`.

## W0 exact-head baseline

Run 33222541108 completed both core/PostgreSQL/API/Mobile and Windows Desktop jobs successfully.

The core log records:
- SHA/tree/parent = frozen DBP-002 identity;
- no pending EF model changes;
- full regression `Passed 155; Failed 0; Skipped 0`;
- protected API probe HTTP 401 as expected.

GitHub artifact digests:
- Linux artifact 9705724131: `sha256:09f0265a3f8cceaefc18401980a6d2aa0663c677fc16d792268c431809d09f49`
- Desktop artifact 9705704277: `sha256:a6af8b223a5714f07b80a6111939b84a67d99c9ee59ab7d337d328748e2db2bf`

## W7 disposable recovery

Run 33222541109 completed successfully and is bound to the frozen SHA/tree/parent.

The job records:
- no pending EF model changes;
- source migrations = 11;
- restored migrations = 11;
- `restore_result=PASS`;
- disposable dump SHA-256 `61398c2c32c2004fece4e3e8e7f7971a3c2b1eb7d2b06022b38466f5edd2dc60`;
- recovery-result SHA-256 `cb65af52f6c375ad6e8466fecc6630265ef792f068fe2e17ca78f178a343f384`.

Artifact 9705704957 digest:
`sha256:6e2e6edc14a8d738eed4da3746f16b97163f31c579d464fe8a06255c5cefd308`.

## Retained v2 failure

Run 33222541073 remains `FAILURE`. It fails at `Baseline catalog backup restore reconciliation` before candidate SQL execution, candidate migration application, RLS checks, full regression, and candidate recovery.

The failure diff is on the restored ten-migration baseline and shows PostgreSQL deparse text changes such as:

`ARRAY['ACTIVE'::character varying, 'INACTIVE'::character varying]::text[]`

versus:

`ARRAY['ACTIVE'::character varying::text, 'INACTIVE'::character varying::text]`

The v2 failure is therefore retained exactly as an observed workflow failure. Its technical disposition is documented separately and is not rewritten to PASS.

Artifact 9705708441 digest:
`sha256:8653061f7f0449c670170dd0c31d5a9b81f3e01613956357285e42bc91c2bd84`.

## Scope and governance status

Technical evidence supports the frozen DBP-002 candidate being presented for fresh independent DB-GOV re-review. This report does not itself accept DBP-002.

The following remain unchanged and outside this package:
- `master@2ec6cccf42624ec0d0e9aaf2332f5dc2273969a5`;
- PR #69 at `601f2d1cad61d62e590a6714ad84e307eb84fe5f`, OPEN / DRAFT / UNMERGED;
- later DBP-004 execution commits, including `1750fe82...` and `c3f2b7b4...`, remain preserved unaccepted evidence under HOLD/STOP;
- MISSION-03 remains OPEN / NOT SEALED until independent re-review;
- no Production authority, successor-mission readiness, seal, or final handoff is asserted.
