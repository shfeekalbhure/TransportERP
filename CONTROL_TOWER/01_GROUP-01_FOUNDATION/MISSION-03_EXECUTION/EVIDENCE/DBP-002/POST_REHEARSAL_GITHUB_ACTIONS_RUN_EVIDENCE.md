# DBP-002 POST-REHEARSAL GITHUB ACTIONS RUN EVIDENCE

Evidence capture basis: GitHub API metadata, GitHub Actions job step metadata/logs, workflow definitions at the frozen SHA, and artifact metadata only.

## Frozen identity

```text
SHA=ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce
TREE=e828941817432bdc73f3e6fc31e74219e74fcf33
PARENT=f128d24dce7baf76a6ac8af4e62a331b80447311
```

## Run 33222541097 — Full Rehearsal v3

- Workflow: `.github/workflows/mission-03-dbp002-rehearsal-v3.yml`
- Job: 99019447103 / `dbp002-rehearsal-v3`
- Conclusion: SUCCESS
- Head SHA: exact frozen SHA
- PostgreSQL: 18.6
- Exact-head end-to-end step: SUCCESS
- Evidence upload step: SUCCESS
- Full regression: 155 passed, 0 failed, 0 skipped
- EF pending model changes: none
- Raw SQL negative summary: PASS A->B, B->A, exact scope, missing/partial/malformed/stale context, branch/role mismatch negatives
- Generated Migration 11 SQL SHA-256: `b5ff58afb259023e713d50379c9c110dc4d1c292503c0d46d68e2f2f96c8c044`
- Baseline dump SHA-256: `574a924dc9709b65c9bf6b94376d8205761869bcfd9a1875b9616d6cbae05630`
- Candidate dump SHA-256: `d6164a4291aa28d530a97e6d2dd2a81de414ad785309a3b2263d3ae1b2874ec0`
- Artifact: 9705722045 / `mission-03-dbp002-rehearsal-v3-ffdf1087ab4a6435cd1f2b19c5ab9ff58ce206ce`
- Artifact size: 322440 bytes
- Artifact digest: `sha256:232fd71285c2675dc7fbcdf9ad207d6d97fbe407a222fe6ac98db8729732527c`
- Artifact expiry recorded by GitHub: 2026-11-27T00:06:37Z
- Run URL: https://github.com/shfeekalbhure/TransportERP/actions/runs/33222541097

Candidate/source SHA-256:
```text
fea6220fbd8bc8d1a7bda8890f058166cb5b978f684663e1bbc96f87bdabbd9a  TransportERP.Infrastructure/Persistence/Migrations/20260828224241_GreenfieldTenantMembershipIsolation.cs
73d7eed6a5915820b45326e1239b540eb5a68d079960cbbb8aeb7267733b1b4c  TransportERP.Infrastructure/Persistence/Migrations/20260828224241_GreenfieldTenantMembershipIsolation.Designer.cs
fd7314fc074fa80474323d1745cdd7f381c8ad426d7c90330cd64e932b1ca5d4  TransportERP.Infrastructure/Persistence/Migrations/TransportErpDbContextModelSnapshot.cs
8217e320b51786741d0cd3eb8b11d5413b9b5f31e564eacca1b4ebd486427c9b  TransportERP.Infrastructure/Persistence/Migrations/GreenfieldDbp002PhysicalSql.cs
0559f0ca64a30545ad29010e2a5eb3edb46b294cad88d2cbbefcf3b0d6af64d9  TransportERP.Infrastructure/Persistence/PersistentPermissionResolver.cs
```

## Run 33222541108 — W0 Disposable Baseline

- Workflow: `.github/workflows/mission-03-w0-disposable-baseline.yml`
- Jobs: 99019515884 Core/PostgreSQL/API/Mobile; 99019515734 Desktop Windows
- Conclusion: SUCCESS
- Head SHA: exact frozen SHA
- Core exact identity: frozen SHA/tree/parent
- Full regression: 155 passed, 0 failed, 0 skipped
- EF pending model changes: none
- API protected boundary: HTTP 401 expected
- Linux artifact 9705724131 digest: `sha256:09f0265a3f8cceaefc18401980a6d2aa0663c677fc16d792268c431809d09f49`
- Desktop artifact 9705704277 digest: `sha256:a6af8b223a5714f07b80a6111939b84a67d99c9ee59ab7d337d328748e2db2bf`
- Run URL: https://github.com/shfeekalbhure/TransportERP/actions/runs/33222541108

## Run 33222541109 — W7 Disposable Recovery

- Workflow: `.github/workflows/mission-03-w7-disposable-recovery.yml`
- Job: 99019447043 / `W7 / PostgreSQL backup and restore rehearsal`
- Conclusion: SUCCESS
- Head SHA: exact frozen SHA
- Exact identity: frozen SHA/tree/parent
- EF pending model changes: none
- Source migrations: 11
- Restored migrations: 11
- Restore result: PASS
- Disposable dump SHA-256: `61398c2c32c2004fece4e3e8e7f7971a3c2b1eb7d2b06022b38466f5edd2dc60`
- Recovery-result SHA-256: `cb65af52f6c375ad6e8466fecc6630265ef792f068fe2e17ca78f178a343f384`
- Artifact 9705704957 digest: `sha256:6e2e6edc14a8d738eed4da3746f16b97163f31c579d464fe8a06255c5cefd308`
- Run URL: https://github.com/shfeekalbhure/TransportERP/actions/runs/33222541109

## Run 33222541073 — Full Rehearsal v2 retained failure

- Workflow: `.github/workflows/mission-03-dbp002-rehearsal-v2.yml`
- Job: 99019446824 / `dbp002-rehearsal-v2`
- Conclusion: FAILURE
- Head SHA: exact frozen SHA
- Steps through ten-migration baseline application: SUCCESS
- Failing step: `Baseline catalog backup restore reconciliation`
- Candidate generated-SQL execution: SKIPPED
- Candidate EF Migration 11 application: SKIPPED
- RLS/negative tests: SKIPPED
- Full regression: SKIPPED
- Candidate backup/restore: SKIPPED
- Evidence upload: SUCCESS
- Artifact 9705708441 digest: `sha256:8653061f7f0449c670170dd0c31d5a9b81f3e01613956357285e42bc91c2bd84`
- Run URL: https://github.com/shfeekalbhure/TransportERP/actions/runs/33222541073

The observed v2 diff is a textual PostgreSQL CHECK-expression deparse difference on the pre-candidate ten-migration baseline. The retained failure and its artifact remain part of this evidence set.
