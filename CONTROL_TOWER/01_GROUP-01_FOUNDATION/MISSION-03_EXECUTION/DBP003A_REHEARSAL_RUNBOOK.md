# DBP-003A Safe-copy and Rehearsal Runbook

State: `PREPARED — EXECUTION REQUIRES INDEPENDENT DB-GOV AUTHORIZATION`

## Entry evidence

Record the exact execution SHA/tree, candidate migration SHA, PostgreSQL image
digest/version, operator, UTC time, named non-Production source, authorization
record, and an explicit statement that no Production credentials/data are used.

Required artifacts before candidate apply:

1. schema-only dump and SHA-256;
2. custom-format recoverable backup and SHA-256;
3. migration history, roles, extensions and RLS inventory;
4. sanitized data-shape and PasswordHash-format aggregates;
5. pre-state reconciliation output and SHA-256;
6. successful restore of the backup into a new disposable database;
7. restore reconciliation equal to the source safe copy.

No raw PasswordHash, token, signing key, pepper, private key or Production
connection string may enter logs or artifacts.

## Rehearsal sequence

1. Prove backup restore before applying candidate work.
2. Run the read-only inventory and reconciliation scripts on source and restored
   copy; retain stdout/stderr and hashes.
3. Restore/build the exact candidate and run EF pending-model verification.
4. Apply only the independently authorized candidate migration to the restored
   disposable database.
5. Re-run constraint/index/catalog inventory and reconciliation.
6. Boot the API with local issuance disabled.
7. Run all legacy regression tests before activating any session adapter.
8. If separately authorized, activate the durable adapter only in the disposable
   environment and run login/rotation/reuse/logout/security-version tests.
9. Execute two-process concurrent refresh and the complete failure-injection
   matrix from `DBP-003A_REHEARSAL_RESUBMISSION.md`.
10. Run cross-company/branch direct-SQL negative tests and verify atomic audit.
11. Exercise feature-disable and forward-correction recovery.
12. Restore the original backup again into a second fresh instance and prove the
    recorded recovery path independently.

## Stop conditions

Stop and preserve all evidence on any unauthorized schema delta, migration
history mismatch, reconciliation mismatch, unexpected raw secret/PII output,
more than one active successor, successor without atomic audit, partial rollback,
cross-tenant success, model drift, restore failure or ambiguous authorization.

No destructive down migration, history rewrite, table drop or data repair is
part of this runbook.

## Exit package

The package must contain exact commands, exit codes, logs, TRX/results, catalog
outputs, backup/restore digests, pre/post/restore reconciliation, failures and
their recovery, and the final disposable database disposal record. Only an
independent DB-GOV decision can advance from rehearsal to implementation.
