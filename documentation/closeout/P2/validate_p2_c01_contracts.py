#!/usr/bin/env python3
from __future__ import annotations
import csv, re, sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent
RELEASE = "P2-C01-WAYBILL-SHIPPING-2026-08"
FILES = {
    "w1": "P2_C01_W1_DATA_CONTRACT_REGISTER.csv",
    "w2": "P2_C01_W2_ACTION_CONTRACT_REGISTER.csv",
    "w3": "P2_C01_W3_SCREEN_CONTRACT_REGISTER.csv",
    "uat": "P2_C01_ACCEPTANCE_TEST_REGISTER.csv",
    "coverage": "P2_C01_DOMAIN_COVERAGE_REGISTER.csv",
    "security": "P2_C01_SECURITY_ISOLATION_MATRIX.csv",
}
EXPECTED = {"w1": 27, "w2": 36, "w3": 43, "uat": 35, "coverage": 40}
errors: list[str] = []

def load(key):
    p = ROOT / FILES[key]
    if not p.exists():
        errors.append(f"MISSING_FILE:{p.name}"); return []
    with p.open("r", encoding="utf-8-sig", newline="") as f:
        return list(csv.DictReader(f))

def refs(v):
    return [x.strip() for x in (v or "").split(";") if x.strip()]

def ids(rows, col, label):
    vals=[r.get(col,"").strip() for r in rows]
    if any(not x for x in vals): errors.append(f"{label}:BLANK_ID")
    if len(vals)!=len(set(vals)): errors.append(f"{label}:DUPLICATE_ID")
    return set(x for x in vals if x)

def req(rows, cols, label):
    for n,r in enumerate(rows,2):
        miss=[c for c in cols if not r.get(c,"").strip()]
        if miss: errors.append(f"{label}:ROW_{n}:MISSING:{'|'.join(miss)}")

def status(rows,label):
    for n,r in enumerate(rows,2):
        if r.get("Status","").strip() not in {"READY_FOR_REVIEW","CLOSED"}:
            errors.append(f"{label}:ROW_{n}:BAD_STATUS:{r.get('Status','')}")

def release(rows,label):
    bad=sorted({r.get("Release_ID","") for r in rows if r.get("Release_ID","")!=RELEASE})
    if bad: errors.append(f"{label}:BAD_RELEASE:{bad}")

w1,w2,w3,uat,cov,sec=[load(k) for k in ("w1","w2","w3","uat","coverage","security")]
for k,n in EXPECTED.items():
    actual=len({"w1":w1,"w2":w2,"w3":w3,"uat":uat,"coverage":cov}[k])
    if actual!=n: errors.append(f"COUNT:{k}:expected={n}:actual={actual}")

w1i=ids(w1,"W1_Contract_ID","W1"); w2i=ids(w2,"Action_ID","W2"); w3i=ids(w3,"Screen_ID","W3")
uati=ids(uat,"Acceptance_ID","UAT"); rules=ids(cov,"Rule_ID","COVERAGE"); perms=ids(sec,"Permission","SECURITY")
for rows,label in [(w1,"W1"),(w2,"W2"),(w3,"W3"),(uat,"UAT"),(cov,"COVERAGE")]: release(rows,label); status(rows,label)

req(w1,["Entity_Code","Columns_Spec","Primary_Key","Concurrency","Audit","Lifecycle","Authority_ID","Source_Ref","Test_ID","Evidence_ID","Owner","Reviewer","Status"],"W1")
req(w2,["Action_Code","HTTP_Verb","Route","Request_DTO","Response_DTO","Required_Permission","Scope","State_Preconditions","State_Transition","Error_Codes","Idempotency","Concurrency","Audit","Offline_Policy","W1_Contract_ID","Test_ID","Evidence_ID","Owner","Reviewer","Status"],"W2")
req(w3,["Screen_Code","Device","Role","RTL_Layout","Fields_Contract","States","Action_IDs","W1_Contract_IDs","Permissions","Validation","Empty_Load_Error_States","Offline_Policy","Audit","Accessibility","Reference_Evidence","Test_ID","Owner","Reviewer","Status"],"W3")
req(uat,["Scenario","Type","Preconditions","Action_or_Steps","Expected_Result","Related_W1","Related_W2","Related_W3","Offline_Audit","Status"],"UAT")
req(cov,["Requirement","Category","Related_W1","Related_W2","Related_W3","Acceptance_IDs","Source","Status"],"COVERAGE")

expected_sets={
 "W1":({f"W1-P2C01-{i:03d}" for i in range(1,28)},w1i),
 "W2":({f"W2-P2C01-{i:03d}" for i in range(1,37)},w2i),
 "W3":({f"W3-P2C01-{i:03d}" for i in range(1,44)},w3i),
 "UAT":({f"UAT-P2C01-{i:03d}" for i in range(1,36)},uati),
 "RULE":({f"BR-SHP-{i:03d}" for i in range(1,41)},rules),
}
for label,(exp,act) in expected_sets.items():
    if exp!=act: errors.append(f"{label}:SEQUENCE:missing={sorted(exp-act)}:unexpected={sorted(act-exp)}")

# Contract-level tests are mandatory for every W1/W2/W3 row; UAT is end-to-end coverage and need not mirror every auxiliary screen one-for-one.
patterns=[(w1,"W1_Contract_ID","Test_ID",r"^T-P2C01-W1-\d{3}$"),(w2,"Action_ID","Test_ID",r"^T-P2C01-W2-\d{3}$"),(w3,"Screen_ID","Test_ID",r"^T-P2C01-W3-\d{3}$")]
for rows,idcol,testcol,pat in patterns:
    for r in rows:
        if not re.match(pat,r.get(testcol,"")): errors.append(f"{r.get(idcol)}:BAD_TEST_ID:{r.get(testcol,'')}")

for r in w2:
    a=r["Action_ID"]
    for x in refs(r.get("W1_Contract_ID")):
        if x not in w1i: errors.append(f"{a}:UNKNOWN_W1:{x}")
    p=r.get("Required_Permission","").strip()
    if p not in perms: errors.append(f"{a}:MISSING_PERMISSION:{p}")

codes=set(); routes=set()
for r in w3:
    s=r["Screen_ID"]; c=r.get("Screen_Code","").strip()
    if c in codes: errors.append(f"W3:DUPLICATE_SCREEN_CODE:{c}")
    codes.add(c)
    if r.get("RTL_Layout","")!="REQUIRED": errors.append(f"{s}:RTL_NOT_REQUIRED")
    for x in refs(r.get("Action_IDs")):
        if x not in w2i: errors.append(f"{s}:UNKNOWN_W2:{x}")
    for x in refs(r.get("W1_Contract_IDs")):
        if x not in w1i: errors.append(f"{s}:UNKNOWN_W1:{x}")
    for p in refs(r.get("Permissions")):
        if p not in perms: errors.append(f"{s}:MISSING_PERMISSION:{p}")

for r in w2:
    key=(r.get("HTTP_Verb","").upper(),r.get("Route",""))
    if key in routes: errors.append(f"W2:DUPLICATE_ROUTE:{key[0]} {key[1]}")
    routes.add(key)

for r in uat:
    t=r["Acceptance_ID"]
    for field,pool,label in [("Related_W1",w1i,"W1"),("Related_W2",w2i,"W2"),("Related_W3",w3i,"W3")]:
        for x in refs(r.get(field)):
            if x not in pool: errors.append(f"{t}:UNKNOWN_{label}:{x}")

for r in cov:
    q=r["Rule_ID"]
    for field,pool,label in [("Related_W1",w1i,"W1"),("Related_W2",w2i,"W2"),("Related_W3",w3i,"W3"),("Acceptance_IDs",uati,"UAT")]:
        for x in refs(r.get(field)):
            if x not in pool: errors.append(f"{q}:UNKNOWN_{label}:{x}")

for r in w1:
    if r.get("Physical_Status")!="CONTRACT_ONLY": errors.append(f"{r['W1_Contract_ID']}:PHYSICAL_STATUS_NOT_CONTRACT_ONLY")

print("P2-C01 W0-3 contract validator")
print(f"W1={len(w1)} W2={len(w2)} W3={len(w3)} UAT={len(uat)} RULES={len(cov)} SECURITY={len(sec)}")
if errors:
    for e in errors: print("ERROR:",e)
    print(f"RESULT=FAIL ERROR_COUNT={len(errors)}"); sys.exit(1)
print("RESULT=PASS ERROR_COUNT=0")
