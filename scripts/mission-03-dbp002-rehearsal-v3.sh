#!/usr/bin/env bash
set -euo pipefail

BASE_SHA="5d1352b4fb6d56261dff8b8a622bacb2786f56d9"
BASELINE_MIGRATION="20260821191039_P2C01CWaybillVolumeContract"
CANDIDATE_MIGRATION="20260828224241_GreenfieldTenantMembershipIsolation"
export PGPASSWORD=postgres
mkdir -p evidence
PGCID="$(docker ps --filter ancestor=postgres:18.6-bookworm --format '{{.ID}}' | head -n1)"
test -n "$PGCID"

psqlq() {
  local db="$1" sql="$2"
  docker exec "$PGCID" psql -q -v ON_ERROR_STOP=1 -U postgres -d "$db" -At -c "$sql"
}

capture_structural() {
  local db="$1" p="$2"
  psqlq "$db" "SELECT n.nspname,c.relname,c.relkind,c.relrowsecurity,c.relforcerowsecurity FROM pg_class c JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' AND c.relkind IN ('r','p') ORDER BY 1,2;" > "evidence/${p}-tables.struct"
  psqlq "$db" "SELECT table_name,column_name,data_type,udt_schema,udt_name,is_nullable,is_identity,COALESCE(identity_generation,''),(column_default IS NOT NULL) FROM information_schema.columns WHERE table_schema='transport_erp' ORDER BY table_name,column_name;" > "evidence/${p}-columns.struct"
  psqlq "$db" "SELECT c.relname,con.conname,con.contype,con.condeferrable,con.condeferred,con.convalidated,COALESCE(nr.nspname,''),COALESCE(cr.relname,''),COALESCE((SELECT string_agg(a.attname,',' ORDER BY k.ord) FROM unnest(con.conkey) WITH ORDINALITY AS k(attnum,ord) JOIN pg_attribute a ON a.attrelid=con.conrelid AND a.attnum=k.attnum),''),COALESCE((SELECT string_agg(a.attname,',' ORDER BY k.ord) FROM unnest(con.confkey) WITH ORDINALITY AS k(attnum,ord) JOIN pg_attribute a ON a.attrelid=con.confrelid AND a.attnum=k.attnum),''),con.confupdtype,con.confdeltype,con.confmatchtype FROM pg_constraint con JOIN pg_class c ON c.oid=con.conrelid JOIN pg_namespace n ON n.oid=c.relnamespace LEFT JOIN pg_class cr ON cr.oid=con.confrelid LEFT JOIN pg_namespace nr ON nr.oid=cr.relnamespace WHERE n.nspname='transport_erp' ORDER BY c.relname,con.conname;" > "evidence/${p}-constraints.struct"
  psqlq "$db" "SELECT t.relname,i.relname,x.indisunique,x.indisprimary,x.indisvalid,x.indisready,x.indisclustered,x.indisreplident,x.indnkeyatts,x.indnatts,COALESCE((SELECT string_agg(CASE WHEN k.attnum=0 THEN '<expression>' ELSE a.attname END,',' ORDER BY k.ord) FROM unnest(x.indkey) WITH ORDINALITY AS k(attnum,ord) LEFT JOIN pg_attribute a ON a.attrelid=x.indrelid AND a.attnum=k.attnum),''),(x.indpred IS NOT NULL),(x.indexprs IS NOT NULL) FROM pg_index x JOIN pg_class i ON i.oid=x.indexrelid JOIN pg_class t ON t.oid=x.indrelid JOIN pg_namespace n ON n.oid=t.relnamespace WHERE n.nspname='transport_erp' ORDER BY t.relname,i.relname;" > "evidence/${p}-indexes.struct"
  psqlq "$db" "SELECT c.relname,p.polname,p.polcmd,p.polpermissive,COALESCE((SELECT string_agg(CASE WHEN u.oid=0 THEN 'PUBLIC' ELSE COALESCE(r.rolname,u.oid::text) END,',' ORDER BY u.ord) FROM unnest(p.polroles) WITH ORDINALITY AS u(oid,ord) LEFT JOIN pg_roles r ON r.oid=u.oid),''),(p.polqual IS NOT NULL),(p.polwithcheck IS NOT NULL) FROM pg_policy p JOIN pg_class c ON c.oid=p.polrelid JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' ORDER BY c.relname,p.polname;" > "evidence/${p}-policies.struct"
  psqlq "$db" "SELECT c.relname,CASE WHEN t.tgisinternal THEN '<internal>' ELSE t.tgname END,t.tgenabled,t.tgisinternal,t.tgdeferrable,t.tginitdeferred,t.tgtype,COALESCE(con.conname,'') FROM pg_trigger t JOIN pg_class c ON c.oid=t.tgrelid JOIN pg_namespace n ON n.oid=c.relnamespace LEFT JOIN pg_constraint con ON con.oid=t.tgconstraint WHERE n.nspname='transport_erp' ORDER BY c.relname,t.tgisinternal,COALESCE(con.conname,''),t.tgtype,CASE WHEN t.tgisinternal THEN '' ELSE t.tgname END;" > "evidence/${p}-triggers.struct"
  psqlq "$db" "SELECT p.proname,pg_get_function_identity_arguments(p.oid),p.provolatile,p.prosecdef,COALESCE(array_to_string(p.proconfig,','),'') FROM pg_proc p JOIN pg_namespace n ON n.oid=p.pronamespace WHERE n.nspname='transport_erp' AND (p.proname LIKE 'current_%' OR p.proname LIKE 'enforce_%') ORDER BY p.proname,2;" > "evidence/${p}-functions.struct"
  psqlq "$db" 'SELECT "MigrationId" FROM transport_erp."__EFMigrationsHistory" ORDER BY "MigrationId";' > "evidence/${p}-history.struct"
}

capture_raw() {
  local db="$1" p="$2"
  psqlq "$db" "SELECT c.relname,con.conname,pg_get_constraintdef(con.oid,true) FROM pg_constraint con JOIN pg_class c ON c.oid=con.conrelid JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' ORDER BY c.relname,con.conname;" > "evidence/${p}-constraints.raw"
  psqlq "$db" "SELECT tablename,indexname,indexdef FROM pg_indexes WHERE schemaname='transport_erp' ORDER BY tablename,indexname;" > "evidence/${p}-indexes.raw"
  psqlq "$db" "SELECT c.relname,p.polname,pg_get_expr(p.polqual,p.polrelid),pg_get_expr(p.polwithcheck,p.polrelid) FROM pg_policy p JOIN pg_class c ON c.oid=p.polrelid JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' ORDER BY c.relname,p.polname;" > "evidence/${p}-policies.raw"
  psqlq "$db" "SELECT table_name,ordinal_position,column_name,COALESCE(column_default,'') FROM information_schema.columns WHERE table_schema='transport_erp' ORDER BY table_name,ordinal_position;" > "evidence/${p}-defaults.raw"
  psqlq "$db" "SELECT c.relname,t.tgname,t.tgenabled,t.tgisinternal,t.tgdeferrable,t.tginitdeferred,t.tgtype,COALESCE(con.conname,'') FROM pg_trigger t JOIN pg_class c ON c.oid=t.tgrelid JOIN pg_namespace n ON n.oid=c.relnamespace LEFT JOIN pg_constraint con ON con.oid=t.tgconstraint WHERE n.nspname='transport_erp' ORDER BY c.relname,t.tgname;" > "evidence/${p}-triggers.raw"
}

reconcile_structural() {
  local left="$1" right="$2"
  for x in tables columns constraints indexes policies triggers functions history; do
    diff -u "evidence/${left}-${x}.struct" "evidence/${right}-${x}.struct"
  done
}

printf 'HEAD=%s\nTREE=%s\nPARENT=%s\n' "$GITHUB_SHA" "$(git rev-parse HEAD^{tree})" "$(git rev-parse HEAD^)" | tee evidence/git-identity.txt
psqlq postgres 'SHOW server_version;' | tee evidence/postgresql-version.txt
grep -q '^18\.6' evidence/postgresql-version.txt

: > evidence/original-ten-preservation.txt
while IFS= read -r p; do
  b="$(git rev-parse "${BASE_SHA}:${p}")"; h="$(git rev-parse "HEAD:${p}")"
  printf '%s|%s|%s\n' "$p" "$b" "$h" | tee -a evidence/original-ten-preservation.txt
  test "$b" = "$h"
done < <(git ls-tree -r --name-only "$BASE_SHA" TransportERP.Infrastructure/Persistence/Migrations | grep -E '/202608(19|20|21)[0-9_].*\.cs$' | sort)
test "$(wc -l < evidence/original-ten-preservation.txt)" -eq 19
git diff "$BASE_SHA" -- TransportERP.Infrastructure/Persistence/Migrations/TransportErpDbContextModelSnapshot.cs > evidence/model-snapshot.diff
test -s evidence/model-snapshot.diff
sha256sum \
  TransportERP.Infrastructure/Persistence/Migrations/20260828224241_GreenfieldTenantMembershipIsolation.cs \
  TransportERP.Infrastructure/Persistence/Migrations/20260828224241_GreenfieldTenantMembershipIsolation.Designer.cs \
  TransportERP.Infrastructure/Persistence/Migrations/TransportErpDbContextModelSnapshot.cs \
  TransportERP.Infrastructure/Persistence/Migrations/GreenfieldDbp002PhysicalSql.cs \
  TransportERP.Infrastructure/Persistence/PersistentPermissionResolver.cs \
  TransportERP.Tests/GreenfieldDbp002AuthorizationTests.cs \
  | tee evidence/candidate-source-sha256.txt

dotnet restore TransportERP.Tests/TransportERP.Tests.csproj | tee evidence/restore.log
dotnet build TransportERP.Tests/TransportERP.Tests.csproj -c Release --no-restore | tee evidence/build.log
dotnet tool install --global dotnet-ef --version 10.0.0
export TRANSPORTERP_DESIGN_CONNSTR='Host=127.0.0.1;Port=5432;Database=postgres;Username=postgres;Password=postgres;Include Error Detail=true'
dotnet ef migrations has-pending-model-changes --project TransportERP.Infrastructure/TransportERP.Infrastructure.csproj --startup-project TransportERP.Infrastructure/TransportERP.Infrastructure.csproj | tee evidence/no-model-drift.log
grep -q 'No changes have been made to the model since the last migration' evidence/no-model-drift.log
dotnet ef migrations script "$BASELINE_MIGRATION" "$CANDIDATE_MIGRATION" --project TransportERP.Infrastructure/TransportERP.Infrastructure.csproj --startup-project TransportERP.Infrastructure/TransportERP.Infrastructure.csproj --output evidence/migration11-generated.sql
test -s evidence/migration11-generated.sql
grep -q 'user_memberships' evidence/migration11-generated.sql
grep -q 'ENABLE ROW LEVEL SECURITY' evidence/migration11-generated.sql
grep -q 'FORCE ROW LEVEL SECURITY' evidence/migration11-generated.sql
grep -q 'IS NOT DISTINCT FROM transport_erp.current_branch_id()' evidence/migration11-generated.sql
sha256sum evidence/migration11-generated.sql | tee evidence/migration11-generated.sql.sha256

# Baseline: exactly the immutable original ten migrations on a fresh empty database.
docker exec "$PGCID" createdb -U postgres mission03_baseline
export TRANSPORTERP_DESIGN_CONNSTR='Host=127.0.0.1;Port=5432;Database=mission03_baseline;Username=postgres;Password=postgres;Include Error Detail=true'
dotnet ef database update "$BASELINE_MIGRATION" --project TransportERP.Infrastructure/TransportERP.Infrastructure.csproj --startup-project TransportERP.Infrastructure/TransportERP.Infrastructure.csproj | tee evidence/baseline-10.log
psqlq mission03_baseline 'SELECT "MigrationId" FROM transport_erp."__EFMigrationsHistory" ORDER BY "MigrationId";' | tee evidence/baseline-history.txt
test "$(wc -l < evidence/baseline-history.txt)" -eq 10
test "$(tail -n1 evidence/baseline-history.txt)" = "$BASELINE_MIGRATION"
! grep -q "$CANDIDATE_MIGRATION" evidence/baseline-history.txt
capture_structural mission03_baseline baseline
capture_raw mission03_baseline baseline

docker exec "$PGCID" pg_dump -U postgres -Fc mission03_baseline > evidence/baseline.dump
docker exec "$PGCID" createdb -U postgres mission03_baseline_restore
cat evidence/baseline.dump | docker exec -i "$PGCID" pg_restore -U postgres -d mission03_baseline_restore
capture_structural mission03_baseline_restore baseline-restore
capture_raw mission03_baseline_restore baseline-restore
reconcile_structural baseline baseline-restore
sha256sum evidence/baseline.dump | tee evidence/baseline.dump.sha256

# Apply the exact generated SQL to an independent copy restored from the ten-migration baseline.
docker exec "$PGCID" createdb -U postgres mission03_sql_candidate
cat evidence/baseline.dump | docker exec -i "$PGCID" pg_restore -U postgres -d mission03_sql_candidate
cat evidence/migration11-generated.sql | docker exec -i "$PGCID" psql -q -v ON_ERROR_STOP=1 -U postgres -d mission03_sql_candidate | tee evidence/generated-sql-execution.log
psqlq mission03_sql_candidate 'SELECT "MigrationId" FROM transport_erp."__EFMigrationsHistory" ORDER BY "MigrationId";' | tee evidence/sql-candidate-history.txt
test "$(wc -l < evidence/sql-candidate-history.txt)" -eq 11
test "$(tail -n1 evidence/sql-candidate-history.txt)" = "$CANDIDATE_MIGRATION"

# Independently prove ten -> eleven using EF update on another fresh database.
docker exec "$PGCID" createdb -U postgres mission03_candidate
export TRANSPORTERP_DESIGN_CONNSTR='Host=127.0.0.1;Port=5432;Database=mission03_candidate;Username=postgres;Password=postgres;Include Error Detail=true'
dotnet ef database update "$BASELINE_MIGRATION" --project TransportERP.Infrastructure/TransportERP.Infrastructure.csproj --startup-project TransportERP.Infrastructure/TransportERP.Infrastructure.csproj | tee evidence/candidate-first-ten.log
test "$(psqlq mission03_candidate 'SELECT count(*) FROM transport_erp."__EFMigrationsHistory";')" = "10"
dotnet ef database update "$CANDIDATE_MIGRATION" --project TransportERP.Infrastructure/TransportERP.Infrastructure.csproj --startup-project TransportERP.Infrastructure/TransportERP.Infrastructure.csproj | tee evidence/candidate-migration11.log
psqlq mission03_candidate 'SELECT "MigrationId" FROM transport_erp."__EFMigrationsHistory" ORDER BY "MigrationId";' | tee evidence/candidate-history.txt
test "$(wc -l < evidence/candidate-history.txt)" -eq 11
test "$(tail -n1 evidence/candidate-history.txt)" = "$CANDIDATE_MIGRATION"

q() { psqlq mission03_candidate "$1"; }
test "$(q "SELECT count(*) FROM pg_class c JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' AND c.relkind='r' AND c.relname NOT IN ('currencies','permissions','global_settings') AND (NOT c.relrowsecurity OR NOT c.relforcerowsecurity);")" = "0"
test "$(q "SELECT count(*) FROM pg_class c JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' AND c.relname IN ('currencies','permissions','global_settings') AND c.relrowsecurity;")" = "0"
test "$(q "SELECT count(*) FROM pg_roles WHERE rolname IN ('transporterp_schema_owner','transporterp_migrator','transporterp_app','transporterp_worker','transporterp_readonly') AND NOT rolcanlogin AND NOT rolbypassrls;")" = "5"
test "$(q "SELECT count(*) FROM pg_proc p JOIN pg_namespace n ON n.oid=p.pronamespace WHERE n.nspname='transport_erp' AND p.proname IN ('current_user_id','current_membership_id','current_company_id','current_branch_id','current_session_id','current_device_id','current_security_version') AND NOT p.prosecdef AND array_to_string(p.proconfig,',') LIKE '%search_path=%';")" = "7"
test "$(q "SELECT count(*) FROM pg_namespace n CROSS JOIN LATERAL aclexplode(COALESCE(n.nspacl,acldefault('n',n.nspowner))) a WHERE n.nspname='transport_erp' AND a.grantee=0 AND a.privilege_type='CREATE';")" = "0"
test "$(q "SELECT has_table_privilege('transporterp_app','transport_erp.user_roles','INSERT') OR has_table_privilege('transporterp_app','transport_erp.user_permission_overrides','INSERT');")" = "f"
test "$(q "SELECT count(*) FROM pg_constraint con JOIN pg_class c ON c.oid=con.conrelid JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' AND c.relname IN ('user_memberships','user_role_grants','user_permission_grants') AND con.contype='f';")" -ge 10
test "$(q "SELECT count(*) FROM pg_trigger t JOIN pg_constraint con ON con.oid=t.tgconstraint JOIN pg_class c ON c.oid=t.tgrelid JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' AND c.relname IN ('user_role_grants','user_permission_grants') AND NOT t.tgisinternal AND con.condeferrable AND con.condeferred;")" = "3"
test "$(q "SELECT count(*) FROM pg_indexes WHERE schemaname='transport_erp' AND indexname='IX_user_memberships_UserId_CompanyId_BranchId' AND indexdef ILIKE '%NULLS NOT DISTINCT%';")" = "1"
test "$(q "SELECT count(*) FROM pg_indexes WHERE schemaname='transport_erp' AND indexname IN ('IX_user_permission_grants_MembershipId_PermissionId','IX_user_role_grants_MembershipId_RoleId') AND indexdef ILIKE '%Status%ACTIVE%';")" = "2"
q "SELECT c.relname,c.relrowsecurity,c.relforcerowsecurity FROM pg_class c JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' AND c.relkind='r' ORDER BY c.relname;" | tee evidence/rls-catalog.txt
q "SELECT c.relname,p.polname,pg_get_expr(p.polqual,p.polrelid),pg_get_expr(p.polwithcheck,p.polrelid) FROM pg_policy p JOIN pg_class c ON c.oid=p.polrelid JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' AND c.relname IN ('users','role_permissions','user_memberships','user_role_grants','user_permission_grants') ORDER BY c.relname;" | tee evidence/dbp002-sensitive-policies.txt
policy_expr() {
  q "SELECT replace(replace(replace(COALESCE(pg_get_expr(p.polqual,p.polrelid),''), chr(10), ' '), chr(13), ' '), chr(9), ' ') FROM pg_policy p JOIN pg_class c ON c.oid=p.polrelid JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' AND c.relname='$1' AND p.polname='tenant_scope';"
}
users_policy="$(policy_expr users)"
role_permissions_policy="$(policy_expr role_permissions)"
permission_grants_policy="$(policy_expr user_permission_grants)"
role_grants_policy="$(policy_expr user_role_grants)"
[[ "$users_policy" == *"current_membership_id"* && "$users_policy" == *"current_security_version"* && "$users_policy" == *"current_company_id"* && "$users_policy" == *"current_branch_id"* ]]
[[ "$role_permissions_policy" == *"current_membership_id"* && "$role_permissions_policy" == *"current_security_version"* && "$role_permissions_policy" == *"current_company_id"* && "$role_permissions_policy" == *"current_branch_id"* && "$role_permissions_policy" == *"IS DISTINCT FROM"* ]]
[[ "$permission_grants_policy" == *"current_membership_id"* && "$permission_grants_policy" == *"current_security_version"* && "$permission_grants_policy" == *"current_company_id"* && "$permission_grants_policy" == *"current_branch_id"* ]]
[[ "$role_grants_policy" == *"current_membership_id"* && "$role_grants_policy" == *"current_security_version"* && "$role_grants_policy" == *"current_company_id"* && "$role_grants_policy" == *"current_branch_id"* ]]
printf '%s\n' "users|$users_policy" "role_permissions|$role_permissions_policy" "user_permission_grants|$permission_grants_policy" "user_role_grants|$role_grants_policy" > evidence/dbp002-sensitive-policy-assertions.normalized.txt
q "SELECT c.relname,con.conname,con.contype,con.condeferrable,con.condeferred,pg_get_constraintdef(con.oid,true) FROM pg_constraint con JOIN pg_class c ON c.oid=con.conrelid JOIN pg_namespace n ON n.oid=c.relnamespace WHERE n.nspname='transport_erp' AND c.relname IN ('user_memberships','user_role_grants','user_permission_grants') ORDER BY c.relname,con.conname;" | tee evidence/dbp002-constraints.raw
q "SELECT tablename,indexname,indexdef FROM pg_indexes WHERE schemaname='transport_erp' AND tablename IN ('user_memberships','user_role_grants','user_permission_grants') ORDER BY tablename,indexname;" | tee evidence/dbp002-indexes.raw
q "SELECT rolname,rolcanlogin,rolbypassrls FROM pg_roles WHERE rolname LIKE 'transporterp_%' ORDER BY rolname;" | tee evidence/runtime-roles.txt

# Deterministic two-tenant data for direct raw-SQL isolation probes.
cat > /tmp/seed.sql <<'SQL'
INSERT INTO transport_erp.currencies ("Id","Code","NameAr","NameEn","MinorUnit","IsBase","Status","CreatedAt","UpdatedAt","RowVersion") VALUES
 ('00000000-0000-0000-0000-000000000001','YER','YER',NULL,2,true,'ACTIVE',now(),now(),decode('','hex'));
INSERT INTO transport_erp.companies ("Id","Code","LegalNameAr","LegalNameEn","TaxIdentifier","BaseCurrencyId","DefaultCalendarId","Status","CreatedAt","UpdatedAt","RowVersion") VALUES
 ('10000000-0000-0000-0000-000000000001','A','Tenant A',NULL,NULL,'00000000-0000-0000-0000-000000000001','90000000-0000-0000-0000-000000000001','ACTIVE',now(),now(),decode('','hex')),
 ('20000000-0000-0000-0000-000000000001','B','Tenant B',NULL,NULL,'00000000-0000-0000-0000-000000000001','90000000-0000-0000-0000-000000000002','ACTIVE',now(),now(),decode('','hex'));
INSERT INTO transport_erp.branches ("Id","CompanyId","Code","NameAr","NameEn","BranchType","Address","Timezone","Status","CreatedAt","UpdatedAt","RowVersion") VALUES
 ('11000000-0000-0000-0000-000000000001','10000000-0000-0000-0000-000000000001','A1','A Branch',NULL,NULL,NULL,'UTC','ACTIVE',now(),now(),decode('','hex')),
 ('22000000-0000-0000-0000-000000000001','20000000-0000-0000-0000-000000000001','B1','B Branch',NULL,NULL,NULL,'UTC','ACTIVE',now(),now(),decode('','hex'));
INSERT INTO transport_erp.users ("Id","UserName","NormalizedUserName","DisplayName","Email","Phone","PasswordHash","Status","CompanyId","BranchId","LastLoginAt","DeletedAt","CreatedAt","UpdatedAt","RowVersion") VALUES
 ('11100000-0000-0000-0000-000000000001','usera','USERA','User A',NULL,NULL,'x','ACTIVE','10000000-0000-0000-0000-000000000001','11000000-0000-0000-0000-000000000001',NULL,NULL,now(),now(),decode('','hex')),
 ('22200000-0000-0000-0000-000000000001','userb','USERB','User B',NULL,NULL,'x','ACTIVE','20000000-0000-0000-0000-000000000001','22000000-0000-0000-0000-000000000001',NULL,NULL,now(),now(),decode('','hex'));
INSERT INTO transport_erp.permissions ("Id","Code","NameAr","Resource","Action","ScopeType","IsSystem","Status","DeletedAt","CreatedAt","UpdatedAt","RowVersion") VALUES
 ('30000000-0000-0000-0000-000000000001','TEST.READ','Read','TEST','READ','BRANCH',true,'ACTIVE',NULL,now(),now(),decode('','hex'));
INSERT INTO transport_erp.roles ("Id","Code","NameAr","NameEn","Description","IsSystem","CompanyId","Status","DeletedAt","CreatedAt","UpdatedAt","RowVersion") VALUES
 ('31000000-0000-0000-0000-000000000001','ROLE.A','Role A',NULL,NULL,false,'10000000-0000-0000-0000-000000000001','ACTIVE',NULL,now(),now(),decode('','hex')),
 ('32000000-0000-0000-0000-000000000001','ROLE.B','Role B',NULL,NULL,false,'20000000-0000-0000-0000-000000000001','ACTIVE',NULL,now(),now(),decode('','hex'));
INSERT INTO transport_erp.role_permissions ("RoleId","PermissionId","ScopeType","CompanyId","BranchId","CreatedAt","UpdatedAt","RowVersion") VALUES
 ('31000000-0000-0000-0000-000000000001','30000000-0000-0000-0000-000000000001','BRANCH','10000000-0000-0000-0000-000000000001','11000000-0000-0000-0000-000000000001',now(),now(),decode('','hex')),
 ('32000000-0000-0000-0000-000000000001','30000000-0000-0000-0000-000000000001','BRANCH','20000000-0000-0000-0000-000000000001','22000000-0000-0000-0000-000000000001',now(),now(),decode('','hex'));
INSERT INTO transport_erp.user_memberships ("Id","UserId","CompanyId","BranchId","ScopeType","Status","SecurityVersion","ValidFrom","ValidTo","CreatedAt","UpdatedAt","CreatedBy","RevokedBy","RevokeReason","ConcurrencyVersion") VALUES
 ('11110000-0000-0000-0000-000000000001','11100000-0000-0000-0000-000000000001','10000000-0000-0000-0000-000000000001','11000000-0000-0000-0000-000000000001','BRANCH','ACTIVE',1,now()-interval '1 minute',NULL,now(),now(),'11100000-0000-0000-0000-000000000001',NULL,NULL,1),
 ('22220000-0000-0000-0000-000000000001','22200000-0000-0000-0000-000000000001','20000000-0000-0000-0000-000000000001','22000000-0000-0000-0000-000000000001','BRANCH','ACTIVE',1,now()-interval '1 minute',NULL,now(),now(),'22200000-0000-0000-0000-000000000001',NULL,NULL,1);
INSERT INTO transport_erp.user_permission_grants ("Id","MembershipId","UserId","CompanyId","BranchId","PermissionId","Effect","Status","ValidFrom","ValidTo","GrantedBy","RevokedBy","Reason","CreatedAt","UpdatedAt","ConcurrencyVersion") VALUES
 ('41000000-0000-0000-0000-000000000001','11110000-0000-0000-0000-000000000001','11100000-0000-0000-0000-000000000001','10000000-0000-0000-0000-000000000001','11000000-0000-0000-0000-000000000001','30000000-0000-0000-0000-000000000001','ALLOW','ACTIVE',now()-interval '1 minute',NULL,'11100000-0000-0000-0000-000000000001',NULL,NULL,now(),now(),1),
 ('42000000-0000-0000-0000-000000000001','22220000-0000-0000-0000-000000000001','22200000-0000-0000-0000-000000000001','20000000-0000-0000-0000-000000000001','22000000-0000-0000-0000-000000000001','30000000-0000-0000-0000-000000000001','ALLOW','ACTIVE',now()-interval '1 minute',NULL,'22200000-0000-0000-0000-000000000001',NULL,NULL,now(),now(),1);
DO $$BEGIN IF NOT EXISTS(SELECT 1 FROM pg_roles WHERE rolname='mission03_rls_probe') THEN CREATE ROLE mission03_rls_probe NOLOGIN NOBYPASSRLS; END IF; END$$;
GRANT transporterp_app TO mission03_rls_probe;
SQL
docker exec -i "$PGCID" psql -q -v ON_ERROR_STOP=1 -U postgres -d mission03_candidate < /tmp/seed.sql | tee evidence/seed.log

A="SET ROLE mission03_rls_probe; SET app.user_id='11100000-0000-0000-0000-000000000001'; SET app.membership_id='11110000-0000-0000-0000-000000000001'; SET app.company_id='10000000-0000-0000-0000-000000000001'; SET app.branch_id='11000000-0000-0000-0000-000000000001'; SET app.security_version='1';"
B="SET ROLE mission03_rls_probe; SET app.user_id='22200000-0000-0000-0000-000000000001'; SET app.membership_id='22220000-0000-0000-0000-000000000001'; SET app.company_id='20000000-0000-0000-0000-000000000001'; SET app.branch_id='22000000-0000-0000-0000-000000000001'; SET app.security_version='1';"
for spec in \
  "$A|user_memberships|10000000-0000-0000-0000-000000000001|20000000-0000-0000-0000-000000000001" \
  "$B|user_memberships|20000000-0000-0000-0000-000000000001|10000000-0000-0000-0000-000000000001"; do
  IFS='|' read -r ctx table own foreign <<< "$spec"
  test "$(q "$ctx SELECT count(*) FROM transport_erp.$table;")" = "1"
  test "$(q "$ctx SELECT count(*) FROM transport_erp.$table WHERE \"CompanyId\"='$foreign';")" = "0"
done
test "$(q "$A SELECT count(*) FROM transport_erp.users;")" = "1"
test "$(q "$A SELECT count(*) FROM transport_erp.users WHERE \"Id\"='22200000-0000-0000-0000-000000000001';")" = "0"
test "$(q "$B SELECT count(*) FROM transport_erp.users;")" = "1"
test "$(q "$B SELECT count(*) FROM transport_erp.users WHERE \"Id\"='11100000-0000-0000-0000-000000000001';")" = "0"
test "$(q "$A SELECT count(*) FROM transport_erp.role_permissions;")" = "1"
test "$(q "$B SELECT count(*) FROM transport_erp.role_permissions;")" = "1"
test "$(q "$A SELECT count(*) FROM transport_erp.user_permission_grants;")" = "1"
test "$(q "$B SELECT count(*) FROM transport_erp.user_permission_grants;")" = "1"

if q "$A INSERT INTO transport_erp.user_memberships (\"Id\",\"UserId\",\"CompanyId\",\"BranchId\",\"ScopeType\",\"Status\",\"SecurityVersion\",\"ValidFrom\",\"CreatedAt\",\"UpdatedAt\",\"CreatedBy\",\"ConcurrencyVersion\") VALUES ('99990000-0000-0000-0000-000000000001','22200000-0000-0000-0000-000000000001','20000000-0000-0000-0000-000000000001','22000000-0000-0000-0000-000000000001','BRANCH','ACTIVE',1,now(),now(),now(),'22200000-0000-0000-0000-000000000001',1);" > /tmp/cross-insert.log 2>&1; then
  echo 'cross-tenant insert unexpectedly succeeded' >&2; exit 1
else
  cp /tmp/cross-insert.log evidence/cross-tenant-insert-negative.txt
fi
test "$(q "$A UPDATE transport_erp.user_memberships SET \"UpdatedAt\"=now() WHERE \"CompanyId\"='20000000-0000-0000-0000-000000000001'; SELECT count(*) FROM transport_erp.user_memberships WHERE \"CompanyId\"='20000000-0000-0000-0000-000000000001';")" = "0"
test "$(q "$B UPDATE transport_erp.user_memberships SET \"UpdatedAt\"=now() WHERE \"CompanyId\"='10000000-0000-0000-0000-000000000001'; SELECT count(*) FROM transport_erp.user_memberships WHERE \"CompanyId\"='10000000-0000-0000-0000-000000000001';")" = "0"

MISSING="SET ROLE mission03_rls_probe;"
MISSING_BRANCH="SET ROLE mission03_rls_probe; SET app.user_id='11100000-0000-0000-0000-000000000001'; SET app.membership_id='11110000-0000-0000-0000-000000000001'; SET app.company_id='10000000-0000-0000-0000-000000000001'; SET app.security_version='1';"
MALFORMED="SET ROLE mission03_rls_probe; SET app.user_id='bad'; SET app.membership_id='bad'; SET app.company_id='bad'; SET app.branch_id='bad'; SET app.security_version='bad';"
STALE="SET ROLE mission03_rls_probe; SET app.user_id='11100000-0000-0000-0000-000000000001'; SET app.membership_id='11110000-0000-0000-0000-000000000001'; SET app.company_id='10000000-0000-0000-0000-000000000001'; SET app.branch_id='11000000-0000-0000-0000-000000000001'; SET app.security_version='2';"
for ctx in "$MISSING" "$MISSING_BRANCH" "$MALFORMED" "$STALE"; do
  test "$(q "$ctx SELECT count(*) FROM transport_erp.user_memberships;")" = "0"
  test "$(q "$ctx SELECT count(*) FROM transport_erp.users;")" = "0"
  test "$(q "$ctx SELECT count(*) FROM transport_erp.role_permissions;")" = "0"
  test "$(q "$ctx SELECT count(*) FROM transport_erp.user_permission_grants;")" = "0"
done

if q "BEGIN; INSERT INTO transport_erp.user_permission_grants (\"Id\",\"MembershipId\",\"UserId\",\"CompanyId\",\"BranchId\",\"PermissionId\",\"Effect\",\"Status\",\"ValidFrom\",\"GrantedBy\",\"CreatedAt\",\"UpdatedAt\",\"ConcurrencyVersion\") VALUES ('43000000-0000-0000-0000-000000000001','11110000-0000-0000-0000-000000000001','11100000-0000-0000-0000-000000000001','10000000-0000-0000-0000-000000000001',NULL,'30000000-0000-0000-0000-000000000001','DENY','ACTIVE',now(),'11100000-0000-0000-0000-000000000001',now(),now(),1); SET CONSTRAINTS ALL IMMEDIATE; COMMIT;" > /tmp/branch-mismatch.log 2>&1; then
  echo 'branch mismatch unexpectedly succeeded' >&2; exit 1
else
  cp /tmp/branch-mismatch.log evidence/branch-mismatch-negative.txt
fi
if q "BEGIN; INSERT INTO transport_erp.user_role_grants (\"Id\",\"MembershipId\",\"UserId\",\"CompanyId\",\"BranchId\",\"RoleId\",\"Status\",\"ValidFrom\",\"GrantedBy\",\"CreatedAt\",\"UpdatedAt\",\"ConcurrencyVersion\") VALUES ('44000000-0000-0000-0000-000000000001','11110000-0000-0000-0000-000000000001','11100000-0000-0000-0000-000000000001','10000000-0000-0000-0000-000000000001','11000000-0000-0000-0000-000000000001','32000000-0000-0000-0000-000000000001','ACTIVE',now(),'11100000-0000-0000-0000-000000000001',now(),now(),1); SET CONSTRAINTS ALL IMMEDIATE; COMMIT;" > /tmp/role-mismatch.log 2>&1; then
  echo 'role/company mismatch unexpectedly succeeded' >&2; exit 1
else
  cp /tmp/role-mismatch.log evidence/role-company-mismatch-negative.txt
fi
printf 'PASS A->B B->A raw SQL; users/role_permissions/grants exact scope; missing/partial/malformed/stale context; branch/role mismatch negatives\n' | tee evidence/raw-sql-negative-summary.txt

# Generated SQL and EF update must produce the same semantic catalog structure.
capture_structural mission03_candidate ef-candidate
capture_raw mission03_candidate ef-candidate
capture_structural mission03_sql_candidate sql-candidate
capture_raw mission03_sql_candidate sql-candidate
reconcile_structural ef-candidate sql-candidate

# Full regression, including bypass-RLS resolver regression, on a third fresh database.
docker exec "$PGCID" createdb -U postgres mission03_regression
export TRANSPORTERP_TEST_POSTGRESQL='Host=127.0.0.1;Port=5432;Database=mission03_regression;Username=postgres;Password=postgres;Include Error Detail=true'
dotnet test TransportERP.Tests/TransportERP.Tests.csproj -c Release --no-restore --logger 'trx;LogFileName=dbp002-full-regression-v3.trx' | tee evidence/full-regression.log
grep -Eq 'Passed!|Passed:' evidence/full-regression.log
find TransportERP.Tests -name dbp002-full-regression-v3.trx -exec cp {} evidence/ \; || true

# Final candidate backup/restore structural reconciliation. Raw textual forms are
# retained as evidence but deliberately excluded from equality gates.
docker exec "$PGCID" pg_dump -U postgres -Fc mission03_candidate > evidence/candidate.dump
docker exec "$PGCID" createdb -U postgres mission03_candidate_restore
cat evidence/candidate.dump | docker exec -i "$PGCID" pg_restore -U postgres -d mission03_candidate_restore
capture_structural mission03_candidate final-source
capture_raw mission03_candidate final-source
capture_structural mission03_candidate_restore final-restore
capture_raw mission03_candidate_restore final-restore
reconcile_structural final-source final-restore
for table in users role_permissions user_memberships user_role_grants user_permission_grants; do
  psqlq mission03_candidate "SELECT '$table',count(*) FROM transport_erp.$table;" >> evidence/final-source-key-counts.txt
  psqlq mission03_candidate_restore "SELECT '$table',count(*) FROM transport_erp.$table;" >> evidence/final-restore-key-counts.txt
done
diff -u evidence/final-source-key-counts.txt evidence/final-restore-key-counts.txt
sha256sum evidence/candidate.dump | tee evidence/candidate.dump.sha256
find evidence -type f ! -name EVIDENCE_SHA256.txt -print0 | sort -z | xargs -0 sha256sum > evidence/EVIDENCE_SHA256.txt
printf 'DBP-002 TECHNICAL REHEARSAL PASS\nHEAD=%s\nTREE=%s\nPARENT=%s\n' "$GITHUB_SHA" "$(git rev-parse HEAD^{tree})" "$(git rev-parse HEAD^)" | tee evidence/DBP002_TECHNICAL_PASS.txt
