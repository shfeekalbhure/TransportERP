#!/usr/bin/env python3
import csv
import hashlib
import subprocess
from pathlib import Path

ROOT = Path(__file__).resolve().parent
P1 = ROOT / 'P1'
errors = []

required_files = [
    P1 / 'P0_BASELINE_MANIFEST.md',
    P1 / 'P1_RELEASE_SCOPE.md',
    P1 / 'W1_DATA_CONTRACT_REGISTER.csv',
    P1 / 'W2_ACTION_CONTRACT_REGISTER.csv',
    P1 / 'W3_SCREEN_CONTRACT_REGISTER.csv',
]
for path in required_files:
    if not path.exists() or path.stat().st_size == 0:
        errors.append(f'MISSING_OR_EMPTY:{path.relative_to(ROOT)}')

try:
    commit = subprocess.check_output(['git', 'rev-parse', 'HEAD'], cwd=ROOT.parent.parent, text=True).strip()
except Exception as exc:
    errors.append(f'GIT_READ_ERROR:{exc}')
    commit = ''

baseline = (P1 / 'P0_BASELINE_MANIFEST.md').read_text(encoding='utf-8') if (P1 / 'P0_BASELINE_MANIFEST.md').exists() else ''
if commit and 'fc607fc6e735f7b554f80dd9ad5d668bf50659c3' not in baseline:
    errors.append('BASELINE_COMMIT_NOT_DECLARED_OR_MISMATCHED')
if 'P1-PLATFORM-SETTINGS-ACCOUNTING-2026-08' not in baseline:
    errors.append('RELEASE_ID_NOT_DECLARED_IN_BASELINE')

register_specs = {
    'W1_DATA_CONTRACT_REGISTER.csv': ['W1_Contract_ID','Release_ID','Entity_Arabic','Physical_Status','Authority_ID','Source_Ref','Test_ID','Evidence_ID','Status'],
    'W2_ACTION_CONTRACT_REGISTER.csv': ['Action_ID','Release_ID','Action_Arabic','HTTP_Verb','Route','Request_DTO','Response_DTO','Required_Permission','Error_Codes','Idempotency','Concurrency','Audit','W1_Contract_ID','Test_ID','Evidence_ID','Status'],
    'W3_SCREEN_CONTRACT_REGISTER.csv': ['Screen_ID','Release_ID','Screen_Arabic','RTL_Layout','Fields_Contract','States','Action_IDs','W1_Contract_IDs','Permissions','Validation','Offline_Policy','Audit','Test_ID','Status'],
}
counts = {}
for filename, required_columns in register_specs.items():
    path = P1 / filename
    if not path.exists():
        continue
    with path.open(encoding='utf-8-sig', newline='') as fh:
        reader = csv.DictReader(fh)
        headers = reader.fieldnames or []
        missing = [c for c in required_columns if c not in headers]
        if missing:
            errors.append(f'{filename}:MISSING_COLUMNS:{"|".join(missing)}')
        rows = list(reader)
        counts[filename] = len(rows)
        for index, row in enumerate(rows, start=2):
            identity = row.get(required_columns[0], '').strip()
            if not identity:
                errors.append(f'{filename}:ROW_{index}:EMPTY_ID')
            if row.get('Status','').strip() == 'CLOSED':
                for field in ('Authority_ID','Test_ID','Evidence_ID'):
                    if field in row and not row.get(field, '').strip():
                        errors.append(f'{filename}:ROW_{index}:CLOSED_WITHOUT_{field}')

print('P0_P1_VALIDATION')
print(f'GIT_HEAD={commit}')
for name, count in counts.items():
    print(f'{name}_ROWS={count}')
print(f'ERROR_COUNT={len(errors)}')
for error in errors:
    print(f'ERROR={error}')
raise SystemExit(1 if errors else 0)
