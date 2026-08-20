#!/usr/bin/env python3
from __future__ import annotations

import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[3]
errors: list[str] = []

REQUIRED = {
    "TransportERP.Contracts/Core/MoneyContracts.cs": ["record MoneyAmount", "record FxSnapshot", "ConvertToAccounting"],
    "TransportERP.Contracts/Party/PartyContracts.cs": ["enum PartyRole", "record OperationalPartySnapshot", "record WaybillPartySnapshot"],
    "TransportERP.Contracts/Attachments/AttachmentContracts.cs": ["record AttachmentDescriptor", "ContentHash", "StorageRef"],
    "TransportERP.Contracts/Tracking/MovementContracts.cs": ["record MovementEnvelope", "ReversesEventId", "ClientOperationId"],
    "TransportERP.Contracts/Geo/GeoAddressContracts.cs": ["record GeoAddressSnapshot", "EnsureUsable"],
    "TransportERP.Contracts/Numbering/NumberingContracts.cs": ["INumberReservationService", "NumberReservationStates", "IdempotencyKey"],
    "TransportERP.Tests/W05SharedKernelTests.cs": ["class W05SharedKernelTests", "MoneyAndFx", "NumberReservationContract"],
    "documentation/closeout/P2/P2_C01_W0_5_SHARED_KERNEL_SCOPE_2026-08-20.md": ["P2-C01-A", "MUST NOT"],
    "documentation/closeout/P2/P2_C01_W0_5_INDEPENDENT_REVIEW_ASSIGNMENT_2026-08-20.md": ["PASS", "P2-C01-A MUST NOT START"],
}

ALLOWED_PREFIXES = (
    "TransportERP.Contracts/Core/",
    "TransportERP.Contracts/Party/",
    "TransportERP.Contracts/Attachments/",
    "TransportERP.Contracts/Tracking/",
    "TransportERP.Contracts/Geo/",
    "TransportERP.Contracts/Numbering/",
    "TransportERP.Tests/W05SharedKernelTests.cs",
    "documentation/closeout/P2/P2_C01_W0_5_",
    "documentation/closeout/P2/validate_p2_c01_w0_5_shared_kernel.py",
    ".github/workflows/p2-c01-w0-5-shared-kernel.yml",
)

FORBIDDEN_FRAGMENTS = (
    "TransportERP.Infrastructure/",
    "TransportERP.Api/",
    "TransportERP.Desktop/",
    "/Migrations/",
    "P1Entities.cs",
)


def run(*args: str) -> str:
    result = subprocess.run(args, cwd=ROOT, text=True, capture_output=True, check=False)
    if result.returncode != 0:
        errors.append(f"COMMAND_FAILED:{' '.join(args)}:{result.stderr.strip()}")
        return ""
    return result.stdout


for rel, tokens in REQUIRED.items():
    path = ROOT / rel
    if not path.exists():
        errors.append(f"MISSING_REQUIRED_FILE:{rel}")
        continue
    text = path.read_text(encoding="utf-8-sig")
    for token in tokens:
        if token not in text:
            errors.append(f"REQUIRED_TOKEN_MISSING:{rel}:{token}")

changed_text = run("git", "diff", "--name-only", "origin/master...HEAD")
changed = [line.strip().replace("\\", "/") for line in changed_text.splitlines() if line.strip()]
if not changed:
    errors.append("NO_CHANGED_FILES_DETECTED")

for path in changed:
    if any(fragment in path for fragment in FORBIDDEN_FRAGMENTS):
        errors.append(f"FORBIDDEN_PHASE_CHANGE:{path}")
    if not any(path == prefix or path.startswith(prefix) for prefix in ALLOWED_PREFIXES):
        errors.append(f"OUT_OF_SCOPE_CHANGE:{path}")

if any("Migration" in Path(path).name for path in changed):
    errors.append("MIGRATION_FILE_INTRODUCED")

# Explicit semantic guards.
numbering = (ROOT / "TransportERP.Contracts/Numbering/NumberingContracts.cs").read_text(encoding="utf-8-sig")
if "Server-authoritative numbering boundary" not in numbering:
    errors.append("NUMBERING_AUTHORITY_GUARD_MISSING")

movement = (ROOT / "TransportERP.Contracts/Tracking/MovementContracts.cs").read_text(encoding="utf-8-sig")
if "cannot reverse itself" not in movement:
    errors.append("MOVEMENT_REVERSAL_GUARD_MISSING")

money = (ROOT / "TransportERP.Contracts/Core/MoneyContracts.cs").read_text(encoding="utf-8-sig")
if "Same-currency conversion must use a rate of 1" not in money:
    errors.append("FX_SNAPSHOT_GUARD_MISSING")

print("P2-C01 W0-5 shared-kernel validator")
print(f"CHANGED_FILES={len(changed)}")
for path in changed:
    print(f"CHANGED: {path}")
if errors:
    for error in errors:
        print(f"ERROR: {error}")
    print(f"RESULT=FAIL ERROR_COUNT={len(errors)}")
    sys.exit(1)
print("RESULT=PASS ERROR_COUNT=0")
