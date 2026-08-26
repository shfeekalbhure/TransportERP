using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Persistence.Migrations;

[DbContext(typeof(TransportErpDbContext))]
[Migration("20260826070000_P0OperationalPartyScopeHardening")]
public partial class P0OperationalPartyScopeHardening : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.operational_parties, transport_erp.waybills,
          transport_erp.waybill_parties, transport_erp.payment_plan_lines,
          transport_erp.collection_transactions IN ACCESS EXCLUSIVE MODE;

        DO $preflight$
        BEGIN
          IF EXISTS (
            SELECT 1 FROM transport_erp.operational_parties p
            JOIN transport_erp.branches b ON b."Id"=p."BranchId"
            WHERE p."BranchId" IS NOT NULL AND b."CompanyId"<>p."CompanyId"
          ) THEN RAISE EXCEPTION 'P0 preflight: operational party branch scope mismatch'; END IF;

          IF EXISTS (
            SELECT 1 FROM transport_erp.waybill_parties r
            JOIN transport_erp.waybills w ON w."Id"=r."WaybillId"
            JOIN transport_erp.operational_parties p ON p."Id"=r."OperationalPartyId"
            WHERE r."OperationalPartyId" IS NOT NULL AND
              (p."CompanyId"<>w."CompanyId" OR (p."BranchId" IS NOT NULL AND p."BranchId"<>w."BranchId") OR p."Status"<>'ACTIVE')
          ) THEN RAISE EXCEPTION 'P0 preflight: waybill party scope mismatch'; END IF;

          IF EXISTS (
            SELECT 1 FROM transport_erp.payment_plan_lines r
            JOIN transport_erp.waybills w ON w."Id"=r."WaybillId"
            JOIN transport_erp.operational_parties p ON p."Id"=r."PartyId"
            WHERE r."PartyId" IS NOT NULL AND
              (p."CompanyId"<>w."CompanyId" OR (p."BranchId" IS NOT NULL AND p."BranchId"<>w."BranchId") OR p."Status"<>'ACTIVE')
          ) THEN RAISE EXCEPTION 'P0 preflight: payment plan party scope mismatch'; END IF;

          IF EXISTS (
            SELECT 1 FROM transport_erp.collection_transactions r
            JOIN transport_erp.waybills w ON w."Id"=r."WaybillId"
            LEFT JOIN transport_erp.operational_parties p ON p."Id"=r."PartyId"
            WHERE r."CompanyId"<>w."CompanyId" OR r."BranchId"<>w."BranchId" OR
              (r."PartyId" IS NOT NULL AND
                (p."Id" IS NULL OR p."CompanyId"<>w."CompanyId" OR
                 (p."BranchId" IS NOT NULL AND p."BranchId"<>w."BranchId") OR p."Status"<>'ACTIVE'))
          ) THEN RAISE EXCEPTION 'P0 preflight: collection party scope mismatch'; END IF;
        END $preflight$;

        DROP INDEX IF EXISTS transport_erp."IX_operational_parties_CompanyId_ClientOperationId";
        CREATE UNIQUE INDEX "IX_operational_parties_CompanyId_BranchId_ClientOperationId"
          ON transport_erp.operational_parties ("CompanyId", "BranchId", "ClientOperationId");

        CREATE FUNCTION transport_erp.enforce_operational_party_reference_scope()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        DECLARE scope_company uuid; scope_branch uuid; party_id uuid;
        BEGIN
          IF TG_TABLE_NAME='waybill_parties' THEN
            party_id := NEW."OperationalPartyId";
          ELSE
            party_id := NEW."PartyId";
          END IF;

          SELECT w."CompanyId", w."BranchId" INTO scope_company, scope_branch
            FROM transport_erp.waybills w WHERE w."Id"=NEW."WaybillId" FOR KEY SHARE;
          IF NOT FOUND THEN RAISE EXCEPTION 'operational party reference scope denied'; END IF;
          IF TG_TABLE_NAME='collection_transactions' AND
             (NEW."CompanyId"<>scope_company OR NEW."BranchId"<>scope_branch) THEN
            RAISE EXCEPTION 'operational party reference scope denied';
          END IF;
          IF party_id IS NULL THEN RETURN NEW; END IF;

          PERFORM pg_advisory_xact_lock(hashtextextended('operational-party-scope|' || party_id::text, 0));
          PERFORM 1 FROM transport_erp.operational_parties p
            WHERE p."Id"=party_id AND p."CompanyId"=scope_company AND p."Status"='ACTIVE'
              AND (p."BranchId" IS NULL OR p."BranchId"=scope_branch)
            FOR KEY SHARE;
          IF NOT FOUND THEN RAISE EXCEPTION 'operational party reference scope denied'; END IF;
          RETURN NEW;
        END $body$;

        CREATE TRIGGER trg_waybill_party_operational_scope
          BEFORE INSERT OR UPDATE OF "WaybillId","OperationalPartyId" ON transport_erp.waybill_parties
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_operational_party_reference_scope();
        CREATE TRIGGER trg_payment_plan_operational_scope
          BEFORE INSERT OR UPDATE OF "WaybillId","PartyId" ON transport_erp.payment_plan_lines
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_operational_party_reference_scope();
        CREATE TRIGGER trg_collection_operational_scope
          BEFORE INSERT OR UPDATE OF "WaybillId","CompanyId","BranchId","PartyId" ON transport_erp.collection_transactions
          FOR EACH ROW EXECUTE FUNCTION transport_erp.enforce_operational_party_reference_scope();

        CREATE FUNCTION transport_erp.prevent_operational_party_scope_drift()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          PERFORM pg_advisory_xact_lock(hashtextextended('operational-party-scope|' || NEW."Id"::text, 0));
          IF NEW."BranchId" IS NOT NULL THEN
            PERFORM 1 FROM transport_erp.branches b
              WHERE b."Id"=NEW."BranchId" AND b."CompanyId"=NEW."CompanyId" FOR KEY SHARE;
            IF NOT FOUND THEN RAISE EXCEPTION 'operational party owner scope denied'; END IF;
          END IF;
          IF EXISTS (
              SELECT 1 FROM transport_erp.waybill_parties r JOIN transport_erp.waybills w ON w."Id"=r."WaybillId"
              WHERE r."OperationalPartyId"=NEW."Id" AND
                (NEW."CompanyId"<>w."CompanyId" OR (NEW."BranchId" IS NOT NULL AND NEW."BranchId"<>w."BranchId") OR NEW."Status"<>'ACTIVE'))
             OR EXISTS (
              SELECT 1 FROM transport_erp.payment_plan_lines r JOIN transport_erp.waybills w ON w."Id"=r."WaybillId"
              WHERE r."PartyId"=NEW."Id" AND
                (NEW."CompanyId"<>w."CompanyId" OR (NEW."BranchId" IS NOT NULL AND NEW."BranchId"<>w."BranchId") OR NEW."Status"<>'ACTIVE'))
             OR EXISTS (
              SELECT 1 FROM transport_erp.collection_transactions r JOIN transport_erp.waybills w ON w."Id"=r."WaybillId"
              WHERE r."PartyId"=NEW."Id" AND
                (NEW."CompanyId"<>w."CompanyId" OR (NEW."BranchId" IS NOT NULL AND NEW."BranchId"<>w."BranchId") OR NEW."Status"<>'ACTIVE'))
          THEN RAISE EXCEPTION 'operational party scope change would strand tenant-scoped references'; END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_operational_party_scope_drift
          BEFORE INSERT OR UPDATE OF "CompanyId","BranchId","Status" ON transport_erp.operational_parties
          FOR EACH ROW EXECUTE FUNCTION transport_erp.prevent_operational_party_scope_drift();

        CREATE FUNCTION transport_erp.prevent_waybill_scope_drift()
        RETURNS trigger LANGUAGE plpgsql AS $body$
        BEGIN
          IF EXISTS (
              SELECT 1 FROM transport_erp.waybill_parties r JOIN transport_erp.operational_parties p ON p."Id"=r."OperationalPartyId"
              WHERE r."WaybillId"=NEW."Id" AND
                (p."CompanyId"<>NEW."CompanyId" OR (p."BranchId" IS NOT NULL AND p."BranchId"<>NEW."BranchId") OR p."Status"<>'ACTIVE'))
             OR EXISTS (
              SELECT 1 FROM transport_erp.payment_plan_lines r JOIN transport_erp.operational_parties p ON p."Id"=r."PartyId"
              WHERE r."WaybillId"=NEW."Id" AND
                (p."CompanyId"<>NEW."CompanyId" OR (p."BranchId" IS NOT NULL AND p."BranchId"<>NEW."BranchId") OR p."Status"<>'ACTIVE'))
             OR EXISTS (SELECT 1 FROM transport_erp.collection_transactions r WHERE r."WaybillId"=NEW."Id" AND
                         (r."CompanyId"<>NEW."CompanyId" OR r."BranchId"<>NEW."BranchId"))
          THEN RAISE EXCEPTION 'waybill scope change would strand tenant-scoped references'; END IF;
          RETURN NEW;
        END $body$;
        CREATE TRIGGER trg_waybill_operational_scope_drift
          BEFORE UPDATE OF "CompanyId","BranchId" ON transport_erp.waybills
          FOR EACH ROW EXECUTE FUNCTION transport_erp.prevent_waybill_scope_drift();
        """);

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.Sql("""
        LOCK TABLE transport_erp.operational_parties, transport_erp.waybills,
          transport_erp.waybill_parties, transport_erp.payment_plan_lines,
          transport_erp.collection_transactions IN ACCESS EXCLUSIVE MODE;

        DO $preflight$
        BEGIN
          IF EXISTS (SELECT 1 FROM transport_erp.operational_parties
                     GROUP BY "CompanyId","ClientOperationId" HAVING count(*)>1) THEN
            RAISE EXCEPTION 'P0 DOWN blocked: company-wide party idempotency keys collide across branches';
          END IF;
        END $preflight$;

        DROP TRIGGER IF EXISTS trg_waybill_operational_scope_drift ON transport_erp.waybills;
        DROP TRIGGER IF EXISTS trg_operational_party_scope_drift ON transport_erp.operational_parties;
        DROP TRIGGER IF EXISTS trg_collection_operational_scope ON transport_erp.collection_transactions;
        DROP TRIGGER IF EXISTS trg_payment_plan_operational_scope ON transport_erp.payment_plan_lines;
        DROP TRIGGER IF EXISTS trg_waybill_party_operational_scope ON transport_erp.waybill_parties;
        DROP FUNCTION IF EXISTS transport_erp.prevent_waybill_scope_drift();
        DROP FUNCTION IF EXISTS transport_erp.prevent_operational_party_scope_drift();
        DROP FUNCTION IF EXISTS transport_erp.enforce_operational_party_reference_scope();

        DROP INDEX IF EXISTS transport_erp."IX_operational_parties_CompanyId_BranchId_ClientOperationId";
        CREATE UNIQUE INDEX "IX_operational_parties_CompanyId_ClientOperationId"
          ON transport_erp.operational_parties ("CompanyId", "ClientOperationId");
        """);
}
