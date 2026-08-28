-- DBP-003A read-only inventory. Run only on an authorized non-Production
-- sanitized safe copy. This script performs no DDL or data mutation.
BEGIN TRANSACTION READ ONLY ISOLATION LEVEL REPEATABLE READ;

SELECT current_database() AS database_name,
       current_setting('server_version') AS server_version,
       current_user AS execution_role,
       pg_is_in_recovery() AS is_replica;

SELECT "MigrationId", "ProductVersion"
FROM transport_erp."__EFMigrationsHistory"
ORDER BY "MigrationId";

SELECT extname, extversion FROM pg_extension ORDER BY extname;

-- Role metadata only. Password verifiers, connection strings and secrets are
-- intentionally excluded. Memberships and grants are required to reproduce
-- the authorization surface of the safe copy without guessing it.
SELECT rolname,
       rolsuper,
       rolinherit,
       rolcreaterole,
       rolcreatedb,
       rolcanlogin,
       rolreplication,
       rolbypassrls,
       rolconnlimit,
       rolvaliduntil
FROM pg_roles
ORDER BY rolname;

SELECT member_role.rolname AS member_role,
       granted_role.rolname AS granted_role,
       grantor_role.rolname AS grantor_role,
       membership.admin_option,
       membership.inherit_option,
       membership.set_option
FROM pg_auth_members membership
JOIN pg_roles member_role ON member_role.oid = membership.member
JOIN pg_roles granted_role ON granted_role.oid = membership.roleid
JOIN pg_roles grantor_role ON grantor_role.oid = membership.grantor
ORDER BY member_role.rolname, granted_role.rolname;

SELECT table_schema, table_name, grantor, grantee, privilege_type, is_grantable
FROM information_schema.role_table_grants
WHERE table_schema = 'transport_erp'
ORDER BY table_name, grantee, privilege_type;

SELECT table_schema, table_name, column_name, grantor, grantee, privilege_type, is_grantable
FROM information_schema.role_column_grants
WHERE table_schema = 'transport_erp'
ORDER BY table_name, column_name, grantee, privilege_type;

SELECT specific_schema, routine_name, grantor, grantee, privilege_type, is_grantable
FROM information_schema.role_routine_grants
WHERE specific_schema = 'transport_erp'
ORDER BY routine_name, grantee, privilege_type;

SELECT object_schema, object_name, object_type, grantor, grantee, privilege_type, is_grantable
FROM information_schema.role_usage_grants
WHERE object_schema = 'transport_erp'
ORDER BY object_type, object_name, grantee, privilege_type;

SELECT defaclrole::regrole::text AS owner_role,
       COALESCE(n.nspname, '<all schemas>') AS schema_name,
       d.defaclobjtype AS object_type,
       d.defaclacl::text AS default_acl
FROM pg_default_acl d
LEFT JOIN pg_namespace n ON n.oid = d.defaclnamespace
ORDER BY owner_role, schema_name, object_type;

SELECT n.nspname AS schema_name,
       c.relname AS relation_name,
       c.relrowsecurity AS rls_enabled,
       c.relforcerowsecurity AS rls_forced
FROM pg_class c
JOIN pg_namespace n ON n.oid = c.relnamespace
WHERE n.nspname = 'transport_erp' AND c.relkind IN ('r','p')
ORDER BY c.relname;

SELECT schemaname, tablename, policyname, permissive, roles, cmd, qual, with_check
FROM pg_policies
WHERE schemaname = 'transport_erp'
ORDER BY tablename, policyname;

SELECT 'users' AS object_name, count(*) AS row_count FROM transport_erp.users
UNION ALL SELECT 'companies', count(*) FROM transport_erp.companies
UNION ALL SELECT 'branches', count(*) FROM transport_erp.branches
UNION ALL SELECT 'roles', count(*) FROM transport_erp.roles
UNION ALL SELECT 'permissions', count(*) FROM transport_erp.permissions
UNION ALL SELECT 'user_roles', count(*) FROM transport_erp.user_roles
UNION ALL SELECT 'role_permissions', count(*) FROM transport_erp.role_permissions
UNION ALL SELECT 'user_permission_overrides', count(*) FROM transport_erp.user_permission_overrides
UNION ALL SELECT 'audit_events', count(*) FROM transport_erp.audit_events
UNION ALL SELECT 'sync_operations', count(*) FROM transport_erp.sync_operations
ORDER BY object_name;

SELECT
  count(*) FILTER (WHERE u."CompanyId" IS NULL) AS null_company,
  count(*) FILTER (WHERE u."BranchId" IS NULL) AS null_branch,
  count(*) FILTER (WHERE u."BranchId" IS NOT NULL AND b."Id" IS NULL) AS missing_branch,
  count(*) FILTER (WHERE u."BranchId" IS NOT NULL AND b."CompanyId" IS DISTINCT FROM u."CompanyId") AS branch_company_mismatch
FROM transport_erp.users u
LEFT JOIN transport_erp.branches b ON b."Id" = u."BranchId";

-- Sanitized aggregate only. Never select or export PasswordHash values.
SELECT
  CASE
    WHEN "PasswordHash" IS NULL THEN 'NULL'
    WHEN length("PasswordHash") = 0 THEN 'EMPTY'
    WHEN "PasswordHash" LIKE '$2%' THEN 'PREFIX_$2'
    WHEN "PasswordHash" LIKE '$argon2%' THEN 'PREFIX_ARGON2'
    WHEN "PasswordHash" LIKE 'AQAAAA%' THEN 'PREFIX_AQAAAA'
    ELSE 'OTHER_OR_UNKNOWN'
  END AS format_bucket,
  length("PasswordHash") AS character_length,
  count(*) AS row_count
FROM transport_erp.users
GROUP BY 1, 2
ORDER BY 1, 2;

SELECT con.conname, con.contype, pg_get_constraintdef(con.oid, true) AS definition
FROM pg_constraint con
JOIN pg_class rel ON rel.oid = con.conrelid
JOIN pg_namespace n ON n.oid = rel.relnamespace
WHERE n.nspname = 'transport_erp'
ORDER BY rel.relname, con.conname;

SELECT schemaname, tablename, indexname, indexdef
FROM pg_indexes
WHERE schemaname = 'transport_erp'
ORDER BY tablename, indexname;

ROLLBACK;
