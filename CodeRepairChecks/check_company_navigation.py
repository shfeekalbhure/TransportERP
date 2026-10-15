#!/usr/bin/env python3
"""Fail-closed, read-only contract checks for SCR-SET-004 (company management).

This is a static regression guard, NOT a WinForms runtime or database integration test.
Run: python CodeRepairChecks/check_company_navigation.py
"""
from pathlib import Path
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
NAV = ROOT / "TransportERP.Desktop" / "FrmMain.PhaseOneNavigation.cs"
MAIN = ROOT / "TransportERP.Desktop" / "FrmMain.cs"
SCREEN = ROOT / "TransportERP.Desktop" / "Forms" / "SystemSettings" / "General"
DESIGNER = SCREEN / "UcCompanyManagement.Designer.cs"
CONTROL = SCREEN / "UcCompanyManagement.cs"
HOST = SCREEN / "FrmCompanyManagement.cs"
HOST_DESIGNER = SCREEN / "FrmCompanyManagement.Designer.cs"

errors: list[str] = []
def verify(condition: bool, message: str) -> None:
    if not condition:
        errors.append(message)

nav = NAV.read_text(encoding="utf-8-sig")
main = MAIN.read_text(encoding="utf-8-sig")
control = CONTROL.read_text(encoding="utf-8-sig")
designer = DESIGNER.read_text(encoding="utf-8-sig")
host = HOST.read_text(encoding="utf-8-sig")
host_designer = HOST_DESIGNER.read_text(encoding="utf-8-sig")

verify('"02.01.01" => new FrmCompanyManagement()' in nav,
       "Company form navigation is not mapped to FrmCompanyManagement.")
verify('"02.01.01" => new UcCompanyManagement()' in nav,
       "Company workspace navigation is not mapped to UcCompanyManagement.")
verify("case UcCompanyManagement screen0: screen0.CloseRequested += closeRequested" in main,
       "Company workspace close event is not wired to tab close.")
verify("public event EventHandler? CloseRequested" in control,
       "Company control must expose its close event.")
verify("btnClose.Click += BtnClose_Click" in designer and "CloseRequested?.Invoke" in control,
       "Company Close button does not raise the close event.")
verify("companyManagement.CloseRequested += CompanyManagement_CloseRequested" in host_designer
       and "void CompanyManagement_CloseRequested" in host,
       "Company form host does not handle close.")
disabled = re.search(r"foreach\s*\(var button in new\[\]\s*\{([^}]+)\}", control)
verify(disabled is not None and "btnClose" not in disabled.group(1),
       "Company Close must remain enabled independently of unavailable CRUD actions.")
verify("ConfigureUnavailableActions();" in control and "button.Enabled = false" in control,
       "Unimplemented company actions must remain disabled.")
for name in ("txtCompanyId", "txtCompanyCode", "txtCompanyNameAr", "txtCompanyNameEn",
             "txtTaxNumber", "cmbCompanyGroup", "cmbActivityType", "cmbCountry",
             "cmbBaseCurrency", "tabCompanies", "tabBasicData", "tabStatus"):
    verify(re.search(r"\b" + name + r" = new ", designer) is not None,
           f"Missing expected Designer control: {name}")
tags = re.findall(r"\b(\w+)\.Tag\s*=\s*\"(FLD-COMPANY-NAME-(?:AR|EN))\"", designer)
expected = {
    "txtCompanyNameAr": "FLD-COMPANY-NAME-AR",
    "txtCompanyNameEn": "FLD-COMPANY-NAME-EN",
}
verify(len(tags) == len(expected) and dict(tags) == expected,
       "Company name field identifiers are missing, duplicated or assigned to other fields.")
if errors:
    for e in errors:
        print("FAIL:", e, file=sys.stderr)
    sys.exit(1)
print("PASS: SCR-SET-004 static navigation, close event, command availability and field-ID checks")
