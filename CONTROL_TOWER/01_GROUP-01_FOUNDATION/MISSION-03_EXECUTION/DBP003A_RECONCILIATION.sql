-- DBP-003A pre/post reconciliation template. Read-only. Run each section on
-- the authorized restored safe copy before and after a DB-GOV-approved candidate.
BEGIN TRANSACTION READ ONLY ISOLATION LEVEL REPEATABLE READ;

SELECT 'users' AS object_name, count(*) AS row_count,
       count(DISTINCT "Id") AS distinct_ids
FROM transport_erp.users
UNION ALL
SELECT 'audit_events', count(*), count(DISTINCT "Id") FROM transport_erp.audit_events
UNION ALL
SELECT 'sync_operations', count(*), count(DISTINCT "Id") FROM transport_erp.sync_operations
UNION ALL
SELECT 'journal_entries', count(*), count(DISTINCT "Id") FROM transport_erp.journal_entries
ORDER BY object_name;

SELECT count(*) AS orphan_user_company
FROM transport_erp.users u
LEFT JOIN transport_erp.companies c ON c."Id" = u."CompanyId"
WHERE u."CompanyId" IS NOT NULL AND c."Id" IS NULL;

SELECT count(*) AS orphan_user_branch
FROM transport_erp.users u
LEFT JOIN transport_erp.branches b ON b."Id" = u."BranchId"
WHERE u."BranchId" IS NOT NULL AND b."Id" IS NULL;

SELECT count(*) AS user_branch_company_mismatch
FROM transport_erp.users u
JOIN transport_erp.branches b ON b."Id" = u."BranchId"
WHERE b."CompanyId" IS DISTINCT FROM u."CompanyId";

SELECT "NormalizedUserName", "CompanyId", count(*) AS duplicate_count
FROM transport_erp.users
WHERE "DeletedAt" IS NULL
GROUP BY "NormalizedUserName", "CompanyId"
HAVING count(*) > 1
ORDER BY duplicate_count DESC;

-- The following queries are expected only after an approved DBP-003A candidate
-- exists. Their absence before migration is an expected pre-state.
SELECT to_regclass('transport_erp.user_security_state') AS security_state_relation,
       to_regclass('transport_erp.auth_sessions') AS auth_sessions_relation;

-- Execute conditionally in the rehearsal harness after both relations exist:
-- SELECT count(*) FROM transport_erp.user_security_state s
-- LEFT JOIN transport_erp.users u ON u."Id" = s."UserId" WHERE u."Id" IS NULL;
-- SELECT count(*) FROM transport_erp.auth_sessions s
-- LEFT JOIN transport_erp.users u ON u."Id" = s."UserId" WHERE u."Id" IS NULL;
-- SELECT "FamilyId", count(*) FROM transport_erp.auth_sessions
-- WHERE "LifecycleState" = 'ACTIVE' GROUP BY "FamilyId" HAVING count(*) > 1;
-- SELECT "ReplacedBySessionId", count(*) FROM transport_erp.auth_sessions
-- WHERE "ReplacedBySessionId" IS NOT NULL GROUP BY "ReplacedBySessionId" HAVING count(*) > 1;

ROLLBACK;
