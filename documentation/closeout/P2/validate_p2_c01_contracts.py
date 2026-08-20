#!/usr/bin/env python3
from __future__ import annotations

import csv
import re
import sys
from collections import Counter
from pathlib import Path

ROOT = Path(__file__).resolve().parent
RELEASE = "P2-C01-WAYBILL-SHIPPING-2026-08"
errors: list[str] = []

FILES = {
    "w1_base": "P2_C01_W1_DATA_CONTRACT_REGISTER.csv",
    "w1_sup": "P2_C01_W1_DATA_CONTRACT_SUPPLEMENT_RR1.csv",
    "w2_base": "P2_C01_W2_ACTION_CONTRACT_REGISTER.csv",
    "w2_sup": "P2_C01_W2_ACTION_CONTRACT_SUPPLEMENT_RR1.csv",
    "w2_ovr": "P2_C01_W2_ACTION_CONTRACT_OVERRIDES_RR1.csv",
    "w3_base": "P2_C01_W3_SCREEN_CONTRACT_REGISTER.csv",
    "w3_ovr": "P2_C01_W3_SCREEN_CONTRACT_OVERRIDES_RR1.csv",
    "uat_base": "P2_C01_ACCEPTANCE_TEST_REGISTER.csv",
    "uat_sup": "P2_C01_ACCEPTANCE_TEST_SUPPLEMENT_RR1.csv",
    "cov_base": "P2_C01_DOMAIN_COVERAGE_REGISTER.csv",
    "cov_sup": "P2_C01_DOMAIN_COVERAGE_SUPPLEMENT_RR1.csv",
    "sec_base": "P2_C01_SECURITY_ISOLATION_MATRIX.csv",
    "sec_sup": "P2_C01_SECURITY_ISOLATION_SUPPLEMENT_RR1.csv",
    "trace": "P2_C01_CONTRACT_TRACEABILITY_RR1.csv",
}

EXPECTED_RAW = {
    "w1_base": 27, "w1_sup": 3,
    "w2_base": 36, "w2_sup": 8, "w2_ovr": 1,
    "w3_base": 43, "w3_ovr": 8,
    "uat_base": 35, "uat_sup": 7,
    "cov_base": 40, "cov_sup": 8,
    "sec_base": 41, "sec_sup": 7,
    "trace": 17,
}
EXPECTED_EFFECTIVE = {"W1": 30, "W2": 44, "W3": 43, "UAT": 42, "RULE": 48, "SECURITY": 48}


def load(key: str) -> list[dict[str, str]]:
    path = ROOT / FILES[key]
    if not path.exists():
        errors.append(f"MISSING_FILE:{path.name}")
        return []
    with path.open("r", encoding="utf-8-sig", newline="") as f:
        return list(csv.DictReader(f))


def refs(value: str | None) -> list[str]:
    return [x.strip() for x in (value or "").split(";") if x.strip()]


def ensure_unique(rows: list[dict[str, str]], id_col: str, label: str) -> None:
    vals = [r.get(id_col, "").strip() for r in rows]
    if any(not x for x in vals):
        errors.append(f"{label}:BLANK_ID")
    dup = sorted(k for k, n in Counter(vals).items() if k and n > 1)
    if dup:
        errors.append(f"{label}:DUPLICATE_IDS:{dup}")


def effective(base: list[dict[str, str]], supplement: list[dict[str, str]], overrides: list[dict[str, str]], id_col: str, label: str) -> list[dict[str, str]]:
    ensure_unique(base, id_col, f"{label}_BASE")
    ensure_unique(supplement, id_col, f"{label}_SUPPLEMENT")
    ensure_unique(overrides, id_col, f"{label}_OVERRIDES")
    base_ids = {r[id_col].strip() for r in base if r.get(id_col, "").strip()}
    sup_ids = {r[id_col].strip() for r in supplement if r.get(id_col, "").strip()}
    overlap = sorted(base_ids & sup_ids)
    if overlap:
        errors.append(f"{label}:SUPPLEMENT_COLLIDES_WITH_BASE:{overlap}")
    for row in overrides:
        rid = row.get(id_col, "").strip()
        if rid not in base_ids and rid not in sup_ids:
            errors.append(f"{label}:OVERRIDE_UNKNOWN_ID:{rid}")
    merged = {r[id_col].strip(): r for r in base + supplement if r.get(id_col, "").strip()}
    for row in overrides:
        rid = row.get(id_col, "").strip()
        if rid:
            merged[rid] = row
    return list(merged.values())


def check_release(rows: list[dict[str, str]], label: str) -> None:
    bad = sorted({r.get("Release_ID", "") for r in rows if r.get("Release_ID", "") != RELEASE})
    if bad:
        errors.append(f"{label}:BAD_RELEASE_IDS:{bad}")


def check_status(rows: list[dict[str, str]], label: str) -> None:
    for i, row in enumerate(rows, start=2):
        status = row.get("Status", "").strip()
        if status not in {"READY_FOR_REVIEW", "CLOSED"}:
            errors.append(f"{label}:ROW_{i}:BAD_STATUS:{status}")


def check_required(rows: list[dict[str, str]], cols: list[str], label: str) -> None:
    for i, row in enumerate(rows, start=2):
        missing = [c for c in cols if not row.get(c, "").strip()]
        if missing:
            errors.append(f"{label}:ROW_{i}:MISSING:{'|'.join(missing)}")


raw = {k: load(k) for k in FILES}
for key, expected in EXPECTED_RAW.items():
    actual = len(raw[key])
    if actual != expected:
        errors.append(f"RAW_COUNT:{key}:expected={expected}:actual={actual}")

w1 = effective(raw["w1_base"], raw["w1_sup"], [], "W1_Contract_ID", "W1")
w2 = effective(raw["w2_base"], raw["w2_sup"], raw["w2_ovr"], "Action_ID", "W2")
w3 = effective(raw["w3_base"], [], raw["w3_ovr"], "Screen_ID", "W3")
uat = effective(raw["uat_base"], raw["uat_sup"], [], "Acceptance_ID", "UAT")
cov = effective(raw["cov_base"], raw["cov_sup"], [], "Rule_ID", "RULE")
sec = effective(raw["sec_base"], raw["sec_sup"], [], "Permission", "SECURITY")
trace = raw["trace"]
ensure_unique(trace, "Trace_ID", "TRACE")

for label, rows in [("W1", w1), ("W2", w2), ("W3", w3), ("UAT", uat), ("RULE", cov), ("SECURITY", sec)]:
    expected = EXPECTED_EFFECTIVE[label]
    if len(rows) != expected:
        errors.append(f"EFFECTIVE_COUNT:{label}:expected={expected}:actual={len(rows)}")

for rows, label in [(w1, "W1"), (w2, "W2"), (w3, "W3"), (uat, "UAT"), (cov, "RULE"), (trace, "TRACE")]:
    check_release(rows, label)
for rows, label in [(w1, "W1"), (w2, "W2"), (w3, "W3"), (uat, "UAT"), (cov, "RULE"), (trace, "TRACE")]:
    check_status(rows, label)

check_required(w1, ["Entity_Code", "Columns_Spec", "Primary_Key", "Concurrency", "Audit", "Lifecycle", "Authority_ID", "Source_Ref", "Test_ID", "Evidence_ID", "Owner", "Reviewer", "Status"], "W1")
check_required(w2, ["Action_Code", "HTTP_Verb", "Route", "Request_DTO", "Response_DTO", "Required_Permission", "Scope", "State_Preconditions", "State_Transition", "Error_Codes", "Idempotency", "Concurrency", "Audit", "Offline_Policy", "W1_Contract_ID", "Test_ID", "Evidence_ID", "Owner", "Reviewer", "Status"], "W2")
check_required(w3, ["Screen_Code", "Device", "Role", "RTL_Layout", "Fields_Contract", "States", "Action_IDs", "W1_Contract_IDs", "Permissions", "Validation", "Empty_Load_Error_States", "Offline_Policy", "Audit", "Accessibility", "Reference_Evidence", "Test_ID", "Owner", "Reviewer", "Status"], "W3")
check_required(uat, ["Scenario", "Type", "Preconditions", "Action_or_Steps", "Expected_Result", "Related_W1", "Related_W2", "Related_W3", "Offline_Audit", "Status"], "UAT")
check_required(cov, ["Requirement", "Category", "Related_W1", "Related_W2", "Related_W3", "Acceptance_IDs", "Source", "Status"], "RULE")
check_required(trace, ["Layer", "Contract_IDs", "Contract_Test_IDs", "Acceptance_IDs", "Rule_IDs", "Status", "Note"], "TRACE")

w1_ids = {r["W1_Contract_ID"] for r in w1}
w2_ids = {r["Action_ID"] for r in w2}
w3_ids = {r["Screen_ID"] for r in w3}
uat_ids = {r["Acceptance_ID"] for r in uat}
rule_ids = {r["Rule_ID"] for r in cov}
perm_ids = {r["Permission"] for r in sec}

expected_sets = {
    "W1": ({f"W1-P2C01-{i:03d}" for i in range(1, 31)}, w1_ids),
    "W2": ({f"W2-P2C01-{i:03d}" for i in range(1, 45)}, w2_ids),
    "W3": ({f"W3-P2C01-{i:03d}" for i in range(1, 44)}, w3_ids),
    "UAT": ({f"UAT-P2C01-{i:03d}" for i in range(1, 43)}, uat_ids),
    "RULE": ({f"BR-SHP-{i:03d}" for i in range(1, 49)}, rule_ids),
    "TRACE": ({f"TR-P2C01-{i:03d}" for i in range(1, 18)}, {r.get("Trace_ID", "") for r in trace}),
}
for label, (expected, actual) in expected_sets.items():
    if expected != actual:
        errors.append(f"{label}:SEQUENCE:missing={sorted(expected-actual)}:unexpected={sorted(actual-expected)}")

# Canonical contract-test IDs.
test_by_contract: dict[str, str] = {}
for rows, id_col, pat in [
    (w1, "W1_Contract_ID", r"^T-P2C01-W1-\d{3}$"),
    (w2, "Action_ID", r"^T-P2C01-W2-\d{3}$"),
    (w3, "Screen_ID", r"^T-P2C01-W3-\d{3}$"),
]:
    for row in rows:
        cid, tid = row[id_col], row.get("Test_ID", "")
        test_by_contract[cid] = tid
        if not re.match(pat, tid):
            errors.append(f"{cid}:BAD_TEST_ID:{tid}")

# W2 resolves to W1 and security.
route_keys: set[tuple[str, str]] = set()
for row in w2:
    aid = row["Action_ID"]
    for ref in refs(row.get("W1_Contract_ID")):
        if ref not in w1_ids:
            errors.append(f"{aid}:UNKNOWN_W1:{ref}")
    perm = row.get("Required_Permission", "").strip()
    if perm not in perm_ids:
        errors.append(f"{aid}:PERMISSION_NOT_IN_SECURITY:{perm}")
    key = (row.get("HTTP_Verb", "").upper(), row.get("Route", ""))
    if key in route_keys:
        errors.append(f"W2:DUPLICATE_ROUTE:{key[0]} {key[1]}")
    route_keys.add(key)

# W3 resolves to W1/W2/security and remains RTL.
screen_codes: set[str] = set()
for row in w3:
    sid = row["Screen_ID"]
    code = row.get("Screen_Code", "").strip()
    if code in screen_codes:
        errors.append(f"W3:DUPLICATE_SCREEN_CODE:{code}")
    screen_codes.add(code)
    if row.get("RTL_Layout", "").strip() != "REQUIRED":
        errors.append(f"{sid}:RTL_NOT_REQUIRED")
    for ref in refs(row.get("Action_IDs")):
        if ref not in w2_ids:
            errors.append(f"{sid}:UNKNOWN_W2:{ref}")
    for ref in refs(row.get("W1_Contract_IDs")):
        if ref not in w1_ids:
            errors.append(f"{sid}:UNKNOWN_W1:{ref}")
    for perm in refs(row.get("Permissions")):
        if perm not in perm_ids:
            errors.append(f"{sid}:PERMISSION_NOT_IN_SECURITY:{perm}")

# UAT and business-rule references resolve against effective contracts.
for row in uat:
    uid = row["Acceptance_ID"]
    for field, pool, label in [("Related_W1", w1_ids, "W1"), ("Related_W2", w2_ids, "W2"), ("Related_W3", w3_ids, "W3")]:
        for ref in refs(row.get(field)):
            if ref not in pool:
                errors.append(f"{uid}:UNKNOWN_{label}:{ref}")

for row in cov:
    rid = row["Rule_ID"]
    for field, pool, label in [("Related_W1", w1_ids, "W1"), ("Related_W2", w2_ids, "W2"), ("Related_W3", w3_ids, "W3"), ("Acceptance_IDs", uat_ids, "UAT")]:
        for ref in refs(row.get(field)):
            if ref not in pool:
                errors.append(f"{rid}:UNKNOWN_{label}:{ref}")

# Complete contract traceability: every effective W1/W2/W3 appears exactly once in the trace map,
# and every mapped contract test / UAT / rule resolves.
trace_contracts: dict[str, list[str]] = {"W1": [], "W2": [], "W3": []}
for row in trace:
    tid = row["Trace_ID"]
    layer = row.get("Layer", "").strip()
    if layer not in trace_contracts:
        errors.append(f"{tid}:BAD_LAYER:{layer}")
        continue
    contracts = refs(row.get("Contract_IDs"))
    tests = refs(row.get("Contract_Test_IDs"))
    if len(contracts) != len(tests):
        errors.append(f"{tid}:CONTRACT_TEST_CARDINALITY_MISMATCH:{len(contracts)}!={len(tests)}")
    for idx, contract in enumerate(contracts):
        pool = {"W1": w1_ids, "W2": w2_ids, "W3": w3_ids}[layer]
        if contract not in pool:
            errors.append(f"{tid}:UNKNOWN_{layer}:{contract}")
        trace_contracts[layer].append(contract)
        if idx < len(tests):
            expected_test = test_by_contract.get(contract)
            if tests[idx] != expected_test:
                errors.append(f"{tid}:TEST_MISMATCH:{contract}:{tests[idx]}!={expected_test}")
    for ref in refs(row.get("Acceptance_IDs")):
        if ref not in uat_ids:
            errors.append(f"{tid}:UNKNOWN_UAT:{ref}")
    for ref in refs(row.get("Rule_IDs")):
        if ref not in rule_ids:
            errors.append(f"{tid}:UNKNOWN_RULE:{ref}")

for layer, pool in [("W1", w1_ids), ("W2", w2_ids), ("W3", w3_ids)]:
    counts = Counter(trace_contracts[layer])
    missing = sorted(pool - set(counts))
    duplicate = sorted(k for k, n in counts.items() if n > 1)
    if missing:
        errors.append(f"TRACE:{layer}:MISSING_CONTRACTS:{missing}")
    if duplicate:
        errors.append(f"TRACE:{layer}:DUPLICATE_CONTRACTS:{duplicate}")

# W0-3 remains contract-only: no physical authorization hidden in W1.
for row in w1:
    if row.get("Physical_Status", "") != "CONTRACT_ONLY":
        errors.append(f"{row['W1_Contract_ID']}:PHYSICAL_STATUS_MUST_BE_CONTRACT_ONLY")

print("P2-C01 W0-3 effective contract validator")
print(f"EFFECTIVE W1={len(w1)} W2={len(w2)} W3={len(w3)} UAT={len(uat)} RULES={len(cov)} SECURITY={len(sec)} TRACE={len(trace)}")
print(f"OVERRIDES W2={len(raw['w2_ovr'])} W3={len(raw['w3_ovr'])}")
if errors:
    for error in errors:
        print(f"ERROR: {error}")
    print(f"RESULT=FAIL ERROR_COUNT={len(errors)}")
    sys.exit(1)
print("RESULT=PASS ERROR_COUNT=0")
