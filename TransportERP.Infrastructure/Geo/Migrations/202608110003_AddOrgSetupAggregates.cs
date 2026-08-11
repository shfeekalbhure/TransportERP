using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportERP.Infrastructure.Geo.Migrations;

/// <summary>
/// Additive W1-SETUP-ORG schema only. This migration deliberately creates no
/// FiscalPeriod, GL, offline/sync/cache, or external-rate-provider storage.
/// </summary>
public partial class AddOrgSetupAggregates : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            CREATE TABLE gen_currencies (
              id BINARY(16) NOT NULL, code VARCHAR(3) NOT NULL, arabic_name VARCHAR(200) NOT NULL,
              english_name VARCHAR(200) NOT NULL, symbol VARCHAR(16) NULL, decimal_places TINYINT UNSIGNED NOT NULL,
              is_active TINYINT(1) NOT NULL DEFAULT 1, version INT UNSIGNED NOT NULL,
              created_at_utc DATETIME(6) NOT NULL, updated_at_utc DATETIME(6) NOT NULL,
              CONSTRAINT PK_gen_currencies PRIMARY KEY (id), CONSTRAINT uq_currency_code UNIQUE (code)
            ) CHARACTER SET utf8mb4;
            CREATE TABLE gen_companies (
              id BINARY(16) NOT NULL, code VARCHAR(50) NOT NULL, arabic_name VARCHAR(200) NOT NULL,
              english_name VARCHAR(200) NOT NULL, legal_name VARCHAR(200) NOT NULL, tax_number VARCHAR(100) NULL,
              base_currency_id BINARY(16) NOT NULL, logo_uri VARCHAR(500) NULL, notes VARCHAR(2000) NULL,
              is_active TINYINT(1) NOT NULL DEFAULT 1, version INT UNSIGNED NOT NULL,
              created_at_utc DATETIME(6) NOT NULL, updated_at_utc DATETIME(6) NOT NULL,
              CONSTRAINT PK_gen_companies PRIMARY KEY (id), CONSTRAINT uq_company_code UNIQUE (code),
              CONSTRAINT FK_gen_companies_currency FOREIGN KEY (base_currency_id) REFERENCES gen_currencies(id) ON DELETE RESTRICT
            ) CHARACTER SET utf8mb4;
            CREATE TABLE gen_branches (
              id BINARY(16) NOT NULL, company_id BINARY(16) NOT NULL, code VARCHAR(50) NOT NULL,
              arabic_name VARCHAR(200) NOT NULL, english_name VARCHAR(200) NOT NULL, time_zone VARCHAR(64) NULL,
              notes VARCHAR(2000) NULL, is_active TINYINT(1) NOT NULL DEFAULT 1, version INT UNSIGNED NOT NULL,
              created_at_utc DATETIME(6) NOT NULL, updated_at_utc DATETIME(6) NOT NULL,
              CONSTRAINT PK_gen_branches PRIMARY KEY (id), CONSTRAINT uq_branch_company_code UNIQUE (company_id, code),
              CONSTRAINT FK_gen_branches_company FOREIGN KEY (company_id) REFERENCES gen_companies(id) ON DELETE RESTRICT
            ) CHARACTER SET utf8mb4;
            CREATE TABLE gen_exchange_rates (
              id BINARY(16) NOT NULL, company_id BINARY(16) NOT NULL, base_currency_id BINARY(16) NOT NULL,
              quote_currency_id BINARY(16) NOT NULL, rate DECIMAL(20,10) NOT NULL, effective_from DATE NOT NULL,
              effective_to DATE NULL, minimum_rate DECIMAL(20,10) NOT NULL, maximum_rate DECIMAL(20,10) NOT NULL,
              source VARCHAR(100) NOT NULL, is_active TINYINT(1) NOT NULL DEFAULT 1, version INT UNSIGNED NOT NULL,
              created_at_utc DATETIME(6) NOT NULL, updated_at_utc DATETIME(6) NOT NULL,
              CONSTRAINT PK_gen_exchange_rates PRIMARY KEY (id),
              CONSTRAINT FK_gen_exchange_rates_company FOREIGN KEY (company_id) REFERENCES gen_companies(id) ON DELETE RESTRICT,
              CONSTRAINT FK_gen_exchange_rates_base FOREIGN KEY (base_currency_id) REFERENCES gen_currencies(id) ON DELETE RESTRICT,
              CONSTRAINT FK_gen_exchange_rates_quote FOREIGN KEY (quote_currency_id) REFERENCES gen_currencies(id) ON DELETE RESTRICT,
              INDEX IX_gen_exchange_rates_scope (company_id, base_currency_id, quote_currency_id, effective_from)
            ) CHARACTER SET utf8mb4;
            CREATE TABLE gen_fiscal_years (
              id BINARY(16) NOT NULL, company_id BINARY(16) NOT NULL, code VARCHAR(50) NOT NULL,
              start_date DATE NOT NULL, end_date DATE NOT NULL, status VARCHAR(16) NOT NULL,
              is_active TINYINT(1) NOT NULL DEFAULT 1, version INT UNSIGNED NOT NULL,
              created_at_utc DATETIME(6) NOT NULL, updated_at_utc DATETIME(6) NOT NULL,
              CONSTRAINT PK_gen_fiscal_years PRIMARY KEY (id), CONSTRAINT uq_fiscal_year_company_code UNIQUE (company_id, code),
              CONSTRAINT FK_gen_fiscal_years_company FOREIGN KEY (company_id) REFERENCES gen_companies(id) ON DELETE RESTRICT,
              INDEX IX_gen_fiscal_years_range (company_id, start_date, end_date)
            ) CHARACTER SET utf8mb4;
            CREATE TABLE gen_number_sequences (
              id BINARY(16) NOT NULL, code VARCHAR(100) NOT NULL, arabic_name VARCHAR(200) NOT NULL,
              english_name VARCHAR(200) NOT NULL, scope_type VARCHAR(32) NOT NULL, document_type VARCHAR(32) NULL,
              company_id BINARY(16) NULL, branch_id BINARY(16) NULL, fiscal_year_id BINARY(16) NULL,
              prefix VARCHAR(32) NULL, last_number BIGINT UNSIGNED NOT NULL, reset_policy VARCHAR(32) NULL,
              scope_key VARCHAR(512) GENERATED ALWAYS AS (CONCAT_WS('|', code, COALESCE(HEX(company_id), '-'), COALESCE(HEX(branch_id), '-'), COALESCE(HEX(fiscal_year_id), '-'), COALESCE(document_type, '-'))) STORED,
              is_active TINYINT(1) NOT NULL DEFAULT 1, version INT UNSIGNED NOT NULL,
              created_at_utc DATETIME(6) NOT NULL, updated_at_utc DATETIME(6) NOT NULL,
              CONSTRAINT PK_gen_number_sequences PRIMARY KEY (id), CONSTRAINT uq_sequence_scope UNIQUE (scope_key)
            ) CHARACTER SET utf8mb4;
            CREATE TABLE gen_number_reservations (
              id BINARY(16) NOT NULL, sequence_id BINARY(16) NOT NULL, number_value BIGINT UNSIGNED NOT NULL,
              rendered_number VARCHAR(128) NOT NULL, state VARCHAR(16) NOT NULL, idempotency_key VARCHAR(128) NOT NULL,
              reason VARCHAR(1000) NULL, created_at_utc DATETIME(6) NOT NULL,
              CONSTRAINT PK_gen_number_reservations PRIMARY KEY (id),
              CONSTRAINT uq_reservation_sequence_number UNIQUE (sequence_id, number_value),
              CONSTRAINT uq_reservation_idempotency UNIQUE (sequence_id, idempotency_key),
              CONSTRAINT FK_gen_number_reservations_sequence FOREIGN KEY (sequence_id) REFERENCES gen_number_sequences(id) ON DELETE RESTRICT
            ) CHARACTER SET utf8mb4;
            CREATE TABLE gen_languages (
              id BINARY(16) NOT NULL, language_code VARCHAR(35) NOT NULL, arabic_name VARCHAR(200) NOT NULL,
              english_name VARCHAR(200) NOT NULL, direction VARCHAR(3) NOT NULL,
              is_active TINYINT(1) NOT NULL DEFAULT 1, version INT UNSIGNED NOT NULL,
              created_at_utc DATETIME(6) NOT NULL, updated_at_utc DATETIME(6) NOT NULL,
              CONSTRAINT PK_gen_languages PRIMARY KEY (id), CONSTRAINT uq_language_code UNIQUE (language_code)
            ) CHARACTER SET utf8mb4;
            CREATE TABLE gen_setting_definitions (
              id BINARY(16) NOT NULL, property_code VARCHAR(128) NOT NULL, group_name VARCHAR(100) NOT NULL,
              value_type VARCHAR(32) NOT NULL, built_in_default VARCHAR(4000) NOT NULL, allowed_scopes VARCHAR(128) NOT NULL,
              resolution_policy VARCHAR(32) NOT NULL, created_at_utc DATETIME(6) NOT NULL, updated_at_utc DATETIME(6) NOT NULL,
              CONSTRAINT PK_gen_setting_definitions PRIMARY KEY (id), CONSTRAINT uq_setting_property_code UNIQUE (property_code)
            ) CHARACTER SET utf8mb4;
            CREATE TABLE gen_setting_overrides (
              id BINARY(16) NOT NULL, definition_id BINARY(16) NOT NULL, scope_type VARCHAR(16) NOT NULL,
              scope_id BINARY(16) NULL, typed_value VARCHAR(4000) NOT NULL, effective_from DATE NULL, effective_to DATE NULL,
              is_active TINYINT(1) NOT NULL DEFAULT 1, version INT UNSIGNED NOT NULL,
              created_at_utc DATETIME(6) NOT NULL, updated_at_utc DATETIME(6) NOT NULL,
              CONSTRAINT PK_gen_setting_overrides PRIMARY KEY (id),
              CONSTRAINT uq_setting_override UNIQUE (definition_id, scope_type, scope_id),
              CONSTRAINT FK_gen_setting_overrides_definition FOREIGN KEY (definition_id) REFERENCES gen_setting_definitions(id) ON DELETE RESTRICT
            ) CHARACTER SET utf8mb4;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            DROP TABLE gen_setting_overrides; DROP TABLE gen_setting_definitions; DROP TABLE gen_languages;
            DROP TABLE gen_number_reservations; DROP TABLE gen_number_sequences; DROP TABLE gen_fiscal_years;
            DROP TABLE gen_exchange_rates; DROP TABLE gen_branches; DROP TABLE gen_companies; DROP TABLE gen_currencies;
            """);
    }
}
