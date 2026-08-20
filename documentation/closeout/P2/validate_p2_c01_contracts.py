#!/usr/bin/env python3
from __future__ import annotations

import csv
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent
RELEASE = "P2-C01-WAYBILL-SHIPPING-2026-08"

FILES = {
    "w1": ROOT / "P2_C01_W1_DATA_CONTRACT_REGISTER.csv",
    "w2": ROOT / "P2_C01_W2_ACTION_CONTRACT_REGISTER.csv",
    "w3": ROOT / "P2_C01_W3_SCREEN_CONTRACT_REGISTER.csv",
    "uat": ROOT / "P2_C01_ACCEPTANCE_TEST_REGISTER.csv",
    "coverage": ROOT / "P2_C01_DOMAIN_COVERAGE_REGISTER.csv",
    "security": ROOT / "P2_C01_SECURITY_ISOLATION_MATRIX.csv",
}

EXPECTED_COUNTS = {"w1": 27, "w2": 36, "w3": 43, "uat": 35, "coverage": 40}

errors: list[str] = []
warnings: list[str] = []


def load_csv(key: str) -> list[dict[str, str]]:
    path = FILES[key]
    if not path.exists():
        errors.append(f"MISSING_FILE:{path.name}")
        return []
    with path.open("r", encoding="utf-8-sig", newline="") as f:
        rows = list(csv.DictReader(f))
    if not rows:
        errors.append(f"EMPTY_REGISTER:{path.name}")
    return rows


def split_refs(value: str | None) -> list[str]:
    if not value:
        return []
    return [x.strip() for x in value.split(";") if x.strip()]


def unique_ids(rows: list[dict[str, str]], column: str, label: str) -> set[str]:
    values = [r.get(column, "").strip() for r in rows]
    blanks = [i + 2 for i, v in enumerate(values) if not v]
    if blanks:
        errors.append(f"{label}:BLANK_ID_ROWS:{blanks}")
    seen: set[str] = set()
    dup: set[str] = set()
    for value in values:
        if value in seen:
            dup.add(value)
        seen.add(value)
    if dup:
        errors.append(f"{label}:DUPLICATE_IDS:{sorted(dup)}")
    return {v for v in values if v}


def check_release(rows: list[dict[str, str]], label: str) -> None:
    bad = sorted({r.get("Release_ID", "") for r in rows if r.get("Release_ID", "") != RELEASE})
    if bad:
        errors.append(f"{label}:BAD_RELEASE_IDS:{bad}")


def check_required(rows: list[dict[str, str]], columns: list[str], label: str) -> None:
    for idx, row in enumerate(rows, start=2):
        missing = [c for c in columns if not row.get(c, "").strip()]
        if missing:
            errors.append(f"{label}:ROW_{idx}:MISSING:{'|'.join(missing)}")


def check_status(rows: list[dict[str, str]], label: str) -> None:
    allowed = {"READY_FOR_REVIEW", "CLOSED"}
    for idx, row in enumerate(rows, start=2):
        status = row.get("Status", "").strip()
        if status not in allowed:
            errors.append(f"{label}:ROW_{idx}:BAD_STATUS:{status}")


w1 = load_csv("w1")
w2 = load_csv("w2")
w3 = load_csv("w3")
uat = load_csv("uat")
coverage = load_csv("coverage")
security = load_csv("security")

for key, expected in EXPECTED_COUNTS.items():
    actual = len({"w1": w1, "w2": w2, "w3": w3, "uat": uat, "coverage": coverage}[key])
    if actual != expected:
        errors.append(f"COUNT_MISMATCH:{key}:expected={expected}:actual={actual}")

w1_ids = unique_ids(w1, "W1_Contract_ID", "W1")
w2_ids = unique_ids(w2, "Action_ID", "W2")
w3_ids = unique_ids(w3, "Screen_ID", "W3")
uat_ids = unique_ids(uat, "Acceptance_ID", "UAT")
rule_ids = unique_ids(coverage, "Rule_ID", "COVERAGE")
permission_ids = unique_ids(security, "Permission", "SECURITY")

check_release(w1, "W1")
check_release(w2, "W2")
check_release(w3, "W3")
check_release(uat, "UAT")
check_release(coverage, "COVERAGE")

check_required(w1, ["Entity_Code", "Columns_Spec", "Primary_Key", "Concurrency", "Audit", "Lifecycle", "Authority_ID", "Source_Ref", "Test_ID", "Evidence_ID", "Owner", "Reviewer", "Status"], "W1")
check_required(w2, ["Action_Code", "HTTP_Verb", "Route", "Request_DTO", "Response_DTO", "Required_Permission", "Scope", "State_Preconditions", "State_Transition", "Error_Codes", "Idempotency", "Concurrency", "Audit", "Offline_Policy", "W1_Contract_ID", "Test_ID", "Evidence_ID", "Owner", "Reviewer", "Status"], "W2")
check_required(w3, ["Screen_Code", "Device", "Role", "RTL_Layout", "Fields_Contract", "States", "Action_IDs", "W1_Contract_IDs", "Permissions", "Validation", "Empty_Load_Error_States", "Offline_Policy", "Audit", "Accessibility", "Reference_Evidence", "Test_ID", "Owner", "Reviewer", "Status"], "W3")
check_required(uat, ["Scenario", "Type", "Preconditions", "Action_or_Steps", "Expected_Result", "Related_W1", "Related_W2", "Related_W3", "Offline_Audit", "Status"], "UAT")
check_required(coverage, ["Requirement", "Category", "Related_W1", "Related_W2", "Related_W3", "Acceptance_IDs", "Source", "Status"], "COVERAGE")

for rows, label in [(w1, "W1"), (w2, "W2"), (w3, "W3"), (uat, "UAT"), (coverage, "COVERAGE")]:
    check_status(rows, label)

expected_w1 = {f"W1-P2C01-{i:03d}" for i in range(1, 28)}
expected_w2 = {f"W2-P2C01-{i:03d}" for i in range(1, 37)}
expected_w3 = {f"W3-P2C01-{i:03d}" for i in range(1, 44)}
expected_uat = {f"UAT-P2C01-{i:03d}" for i in range(1, 36)}
expected_rules = {f"BR-SHP-{i:03d}" for i in range(1, 41)}
for label, actual, expected in [("W1", w1_ids, expected_w1), ("W2", w2_ids, expected_w2), ("W3", w3_ids, expected_w3), ("UAT", uat_ids, expected_uat), ("RULES", rule_ids, expected_rules)]:
    if actual != expected:
        errors.append(f"{label}:SEQUENCE_MISMATCH:missing={sorted(expected-actual)}:unexpected={sorted(actual-expected)}")

# W2 -> W1 and permission references.
for row in w2:
    action = row["Action_ID"]
    for ref in split_refs(row.get("W1_Contract_ID")):
        if ref not in w1_ids:
            errors.append(f"{action}:UNKNOWN_W1:{ref}")
    permission = row.get("Required_Permission", "").strip()
    if permission and permission not in permission_ids:
        errors.append(f"{action}:PERMISSION_NOT_IN_SECURITY_MATRIX:{permission}")

# W3 -> W1/W2/security references and unique screen codes.
screen_codes: set[str] = set()
for row in w3:
    screen = row["Screen_ID"]
    code = row.get("Screen_Code", "").strip()
    if code in screen_codes:
        errors.append(f"W3:DUPLICATE_SCREEN_CODE:{code}")
    screen_codes.add(code)
    for ref in split_refs(row.get("Action_IDs")):
        if ref not in w2_ids:
            errors.append(f"{screen}:UNKNOWN_W2:{ref}")
    for ref in split_refs(row.get("W1_Contract_IDs")):
        if ref not in w1_ids:
            errors.append(f"{screen}:UNKNOWN_W1:{ref}")
    for permission in split_refs(row.get("Permissions")):
        if permission not in permission_ids:
            errors.append(f"{screen}:PERMISSION_NOT_IN_SECURITY_MATRIX:{permission}")
    if row.get("RTL_Layout", "").strip() != "REQUIRED":
        errors.append(f"{screen}:RTL_NOT_REQUIRED")

# UAT cross references.
for row in uat:
    test = row["Acceptance_ID"]
    for ref, pool, kind in [
        (row.get("Related_W1"), w1_ids, "W1"),
        (row.get("Related_W2"), w2_ids, "W2"),
        (row.get("Related_W3"), w3_ids, "W3"),
    ]:
        for item in split_refs(ref):
            if item not in pool:
                errors.append(f"{test}:UNKNOWN_{kind}:{item}")

# Business-rule coverage cross references.
covered_w1: set[str] = set()
covered_w2: set[str] = set()
covered_w3: set[str] = set()
covered_uat: set[str] = set()
for row in coverage:
    rule = row["Rule_ID"]
    for field, pool, sink, kind in [
        ("Related_W1", w1_ids, covered_w1, "W1"),
        ("Related_W2", w2_ids, covered_w2, "W2"),
        ("Related_W3", w3_ids, covered_w3, "W3"),
        ("Acceptance_IDs", uat_ids, covered_uat, "UAT"),
    ]:
        for ref in split_refs(row.get(field)):
            if ref not in pool:
                errors.append(f"{rule}:UNKNOWN_{kind}:{ref}")
            else:
                sink.add(ref)

# Every contract/screen/action must participate in either direct UAT or business-rule coverage.
direct_uat_w1 = {x for r in uat for x in split_refs(r.get("Related_W1"))}
direct_uat_w2 = {x for r in uat for x in split_refs(r.get("Related_W2"))}
direct_uat_w3 = {x for r in uat for x in split_refs(r.get("Related_W3"))}
for label, pool, covered in [
    ("W1", w1_ids, covered_w1 | direct_uat_w1),
    ("W2", w2_ids, covered_w2 | direct_uat_w2),
    ("W3", w3_ids, covered_w3 | direct_uat_w3),
]:
    missing = sorted(pool - covered)
    if missing:
        errors.append(f"{label}:NO_UAT_OR_RULE_COVERAGE:{missing}")

# Basic route hygiene: no duplicate write verb+route pairs.
route_keys: set[tuple[str, str]] = set()
for row in w2:
    key = (row.get("HTTP_Verb", "").upper(), row.get("Route", ""))
    if key in route_keys:
        errors.append(f"W2:DUPLICATE_ROUTE:{key[0]} {key[1]}")
    route_keys.add(key)

# Contract-only phase must not pretend physical implementation is approved.
for row in w1:
    if row.get("Physical_Status", "") != "CONTRACT_ONLY":
        errors.append(f"{row['W1_Contract_ID']}:PHYSICAL_STATUS_MUST_BE_CONTRACT_ONLY_DURING_W0_3")

print("P2-C01 W0-3 contract validator")
print(f"W1={len(w1)} W2={len(w2)} W3={len(w3)} UAT={len(uat)} RULES={len(coverage)} SECURITY={len(security)}")
if warnings:
    for warning in warnings:
        print(f"WARNING: {warning}")
if errors:
    for error in errors:
        print(f"ERROR: {error}")
    print(f"RESULT=FAIL ERROR_COUNT={len(errors)}")
    sys.exit(1)
print("RESULT=PASS ERROR_COUNT=0")
