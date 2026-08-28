#!/usr/bin/env bash
set -euo pipefail

: "${ALLOW_DISPOSABLE_RECOVERY:?Set ALLOW_DISPOSABLE_RECOVERY=1 for an isolated rehearsal.}"
: "${PGHOST:=127.0.0.1}"
: "${PGPORT:=5432}"
: "${PGUSER:=postgres}"
: "${PGPASSWORD:=postgres}"
: "${SOURCE_DB:=transporterptest}"
: "${RESTORE_DB:=transporterprestore}"
: "${EVIDENCE_DIR:=mission-03-recovery-artifacts}"

if [[ "${ALLOW_DISPOSABLE_RECOVERY}" != "1" ]]; then
  echo "Refusing recovery rehearsal without explicit disposable authorization." >&2
  exit 2
fi

for value in "${SOURCE_DB}" "${RESTORE_DB}"; do
  if [[ ! "${value}" =~ ^transporterp(test|restore)[a-z0-9_]*$ ]]; then
    echo "Refusing non-disposable database name: ${value}" >&2
    exit 2
  fi
done

if [[ "${PGHOST}" != "127.0.0.1" && "${PGHOST}" != "localhost" ]]; then
  echo "Refusing non-local database host: ${PGHOST}" >&2
  exit 2
fi

mkdir -p "${EVIDENCE_DIR}"
evidence_abs="$(cd "${EVIDENCE_DIR}" && pwd)"

pg18() {
  docker run --rm -i --network host \
    -e PGPASSWORD="${PGPASSWORD}" \
    -v "${evidence_abs}:/evidence" \
    postgres:18.6-bookworm "$@"
}

pg18 psql -v ON_ERROR_STOP=1 -h "${PGHOST}" -p "${PGPORT}" -U "${PGUSER}" -d "${SOURCE_DB}" <<'SQL'
CREATE SCHEMA IF NOT EXISTS mission03_recovery_probe;
CREATE TABLE IF NOT EXISTS mission03_recovery_probe.marker (
    marker_id uuid PRIMARY KEY,
    marker_text text NOT NULL
);
TRUNCATE TABLE mission03_recovery_probe.marker;
INSERT INTO mission03_recovery_probe.marker(marker_id, marker_text)
VALUES ('00000000-0000-0000-0000-000000000003', 'MISSION-03-DISPOSABLE-RECOVERY');
SQL

pg18 pg_dump -h "${PGHOST}" -p "${PGPORT}" -U "${PGUSER}" \
  --format=custom --no-owner --no-privileges \
  --file=/evidence/transporterp-disposable.dump "${SOURCE_DB}"

pg18 dropdb -h "${PGHOST}" -p "${PGPORT}" -U "${PGUSER}" --if-exists "${RESTORE_DB}"
pg18 createdb -h "${PGHOST}" -p "${PGPORT}" -U "${PGUSER}" "${RESTORE_DB}"
pg18 pg_restore -h "${PGHOST}" -p "${PGPORT}" -U "${PGUSER}" \
  --exit-on-error --single-transaction --no-owner --no-privileges \
  --dbname="${RESTORE_DB}" /evidence/transporterp-disposable.dump

source_marker="$(pg18 psql -At -h "${PGHOST}" -p "${PGPORT}" -U "${PGUSER}" -d "${SOURCE_DB}" -c "SELECT marker_text FROM mission03_recovery_probe.marker WHERE marker_id='00000000-0000-0000-0000-000000000003'")"
restored_marker="$(pg18 psql -At -h "${PGHOST}" -p "${PGPORT}" -U "${PGUSER}" -d "${RESTORE_DB}" -c "SELECT marker_text FROM mission03_recovery_probe.marker WHERE marker_id='00000000-0000-0000-0000-000000000003'")"
source_migrations="$(pg18 psql -At -h "${PGHOST}" -p "${PGPORT}" -U "${PGUSER}" -d "${SOURCE_DB}" -c 'SELECT COUNT(*) FROM transport_erp."__EFMigrationsHistory"')"
restored_migrations="$(pg18 psql -At -h "${PGHOST}" -p "${PGPORT}" -U "${PGUSER}" -d "${RESTORE_DB}" -c 'SELECT COUNT(*) FROM transport_erp."__EFMigrationsHistory"')"

test "${source_marker}" = "MISSION-03-DISPOSABLE-RECOVERY"
test "${restored_marker}" = "${source_marker}"
test "${source_migrations}" = "${restored_migrations}"

{
  echo "source_db=${SOURCE_DB}"
  echo "restore_db=${RESTORE_DB}"
  echo "marker=${restored_marker}"
  echo "source_migrations=${source_migrations}"
  echo "restored_migrations=${restored_migrations}"
  echo "restore_result=PASS"
} | tee "${EVIDENCE_DIR}/recovery-result.txt"

sha256sum "${EVIDENCE_DIR}/transporterp-disposable.dump" \
  "${EVIDENCE_DIR}/recovery-result.txt" \
  | tee "${EVIDENCE_DIR}/SHA256SUMS.txt"

pg18 dropdb -h "${PGHOST}" -p "${PGPORT}" -U "${PGUSER}" --if-exists "${RESTORE_DB}"
