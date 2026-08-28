#!/usr/bin/env python3
"""Drive the ordinary TransportERP Driver launcher activity through stable AutomationIds.

The script intentionally has no test-activity, HTTP, certificate-validation, database, or
application-internal hook. Its input file contains secrets and must be owner-readable only. Output
evidence contains fixed phase/result codes only.
"""

from __future__ import annotations

import argparse
import json
import os
import re
import stat
import subprocess
import sys
import time
import uuid
import xml.etree.ElementTree as ET
from pathlib import Path
from typing import Callable, Iterable


DEFAULT_PACKAGE = "com.transporterp.mobile.driver"
AUTOMATION_ROOT = "driver_main_scroll"
SAFE_INPUT = re.compile(r"^[A-Za-z0-9@._+/:=%-]+$")
BOUNDS = re.compile(r"^\[(\d+),(\d+)\]\[(\d+),(\d+)\]$")
OPERATION_ID = re.compile(r"^([0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}) \|")
FOCUS_OWNER_ALLOWLIST = (
    ("driver_user_name", "USER_NAME"),
    ("driver_password", "PASSWORD"),
    ("driver_company_id", "COMPANY_ID"),
    ("driver_branch_id", "BRANCH_ID"),
    ("driver_device_id", "DEVICE_ID"),
    ("driver_device_credential", "DEVICE_CREDENTIAL"),
)
SCROLL_OBSERVATION_IDS = tuple(candidate for candidate, _ in FOCUS_OWNER_ALLOWLIST) + (
    "driver_mode",
    "driver_reason",
    "driver_evidence",
    "driver_action_result",
    "driver_sign_in",
    "driver_sign_out",
    "driver_party_name",
    "driver_party_mobile",
    "driver_party_address",
    "driver_queue_party",
    "driver_operation_list",
)


class UiE2EFailure(RuntimeError):
    pass


class Driver:
    def __init__(self, adb: str, package: str, timeout_seconds: int) -> None:
        self.adb = adb
        self.package = package
        self.timeout_seconds = timeout_seconds
        self._last_scroll_hierarchy: ET.Element | None = None

    def run(self, *arguments: str, timeout: int = 30) -> str:
        try:
            completed = subprocess.run(
                [self.adb, *arguments],
                check=False,
                capture_output=True,
                text=True,
                timeout=timeout,
            )
        except (OSError, subprocess.TimeoutExpired) as error:
            raise UiE2EFailure("ADB_UNAVAILABLE") from error
        if completed.returncode != 0:
            raise UiE2EFailure("ADB_COMMAND_FAILED")
        return completed.stdout.replace("\r", "")

    def launch_ordinary_activity(self) -> None:
        package_dump = self.run("shell", "dumpsys", "package", self.package)
        flag_sets = re.findall(r"(?m)^\s*(?:pkgFlags|flags)=\[([^]]*)]", package_dump)
        if not flag_sets:
            raise UiE2EFailure("RELEASE_FLAG_EVIDENCE_MISSING")
        if any(re.search(r"\bDEBUGGABLE\b", flags) for flags in flag_sets):
            raise UiE2EFailure("INSTALLED_PACKAGE_IS_DEBUGGABLE")
        self.run("shell", "am", "force-stop", self.package)
        component = self.run(
            "shell",
            "cmd",
            "package",
            "resolve-activity",
            "--components",
            "-a",
            "android.intent.action.MAIN",
            "-c",
            "android.intent.category.LAUNCHER",
            self.package,
        ).strip().splitlines()
        if len(component) != 1 or not component[0].startswith(self.package + "/"):
            raise UiE2EFailure("ORDINARY_LAUNCHER_ACTIVITY_NOT_RESOLVED")
        self.run("shell", "am", "start", "-W", "-n", component[0])
        self.wait_for(AUTOMATION_ROOT)

    def dump(self) -> ET.Element:
        remote = "/data/local/tmp/transporterp-driver-release-ui.xml"
        self.run("shell", "uiautomator", "dump", remote)
        payload = self.run("exec-out", "cat", remote)
        self.run("shell", "rm", "-f", remote)
        start = payload.find("<?xml")
        if start < 0:
            raise UiE2EFailure("UI_HIERARCHY_INVALID")
        try:
            return ET.fromstring(payload[start:])
        except ET.ParseError as error:
            raise UiE2EFailure("UI_HIERARCHY_INVALID") from error

    @staticmethod
    def _matches(node: ET.Element, automation_id: str) -> bool:
        resource_id = node.attrib.get("resource-id", "")
        return (
            node.attrib.get("content-desc") == automation_id
            or resource_id == automation_id
            or resource_id.rsplit("/", 1)[-1] == automation_id
        )

    def nodes(self, automation_id: str, root: ET.Element | None = None) -> list[ET.Element]:
        hierarchy = root if root is not None else self.dump()
        return [
            node
            for node in hierarchy.iter("node")
            if self._matches(node, automation_id) and node.attrib.get("visible-to-user", "true") == "true"
        ]

    @staticmethod
    def _rectangle(node: ET.Element) -> tuple[int, int, int, int]:
        match = BOUNDS.fullmatch(node.attrib.get("bounds", ""))
        if match is None:
            raise UiE2EFailure("UI_ELEMENT_BOUNDS_INVALID")
        left, top, right, bottom = (int(value) for value in match.groups())
        if right <= left or bottom <= top:
            raise UiE2EFailure("UI_ELEMENT_BOUNDS_INVALID")
        return left, top, right, bottom

    def _scroll(self, toward_bottom: bool) -> str:
        before = self.dump()
        roots = self.nodes(AUTOMATION_ROOT, before)
        if len(roots) != 1:
            raise UiE2EFailure("UI_SCROLL_ROOT_INVALID")
        before_signature = self._safe_scroll_signature(before)
        left, top, right, bottom = self._rectangle(roots[0])
        x = (left + right) // 2
        upper = top + max(20, (bottom - top) // 4)
        lower = bottom - max(20, (bottom - top) // 4)
        start, end = (lower, upper) if toward_bottom else (upper, lower)
        self.run("shell", "input", "swipe", str(x), str(start), str(x), str(end), "250")
        time.sleep(0.25)
        self._last_scroll_hierarchy = None
        after = self.dump()
        self._last_scroll_hierarchy = after
        after_signature = self._safe_scroll_signature(after)
        if before_signature is None or after_signature is None:
            return "UNKNOWN"
        return "UNCHANGED" if before_signature == after_signature else "MOVED"

    def _safe_scroll_signature(
        self,
        hierarchy: ET.Element,
    ) -> tuple[tuple[str, tuple[int, int, int, int]], ...] | None:
        signature: list[tuple[str, tuple[int, int, int, int]]] = []
        try:
            for automation_id in SCROLL_OBSERVATION_IDS:
                found = self.nodes(automation_id, hierarchy)
                if len(found) > 1:
                    return None
                if found:
                    signature.append((automation_id, self._rectangle(found[0])))
        except UiE2EFailure:
            return None
        return tuple(signature) if signature else None

    @staticmethod
    def _aggregate_scroll_movement(movements: list[str]) -> str:
        if "MOVED" in movements:
            return "MOVED"
        if movements and all(movement == "UNCHANGED" for movement in movements):
            return "UNCHANGED"
        return "UNKNOWN"

    def find(self, automation_id: str) -> ET.Element:
        found = self.nodes(automation_id)
        if found:
            return found[0]
        movements: list[str] = []
        for _ in range(6):
            movements.append(self._scroll(toward_bottom=False))
            found = self.nodes(automation_id, self._last_scroll_hierarchy)
            if found:
                return found[0]
        for _ in range(14):
            movements.append(self._scroll(toward_bottom=True))
            found = self.nodes(automation_id, self._last_scroll_hierarchy)
            if found:
                return found[0]
        movement = self._aggregate_scroll_movement(movements)
        raise UiE2EFailure(f"UI_AUTOMATION_ID_NOT_FOUND:SCROLL_{movement}")

    def wait_for(
        self,
        automation_id: str,
        predicate: Callable[[ET.Element], bool] | None = None,
        timeout_seconds: int | None = None,
    ) -> ET.Element:
        deadline = time.monotonic() + (timeout_seconds or self.timeout_seconds)
        while time.monotonic() < deadline:
            found = self.nodes(automation_id)
            if found:
                matching = next((node for node in found if predicate is None or predicate(node)), None)
                if matching is not None:
                    return matching
            time.sleep(0.5)
        raise UiE2EFailure("UI_WAIT_TIMEOUT")

    def click(self, automation_id: str, require_enabled: bool = True) -> None:
        node = self.find(automation_id)
        if require_enabled and node.attrib.get("enabled") != "true":
            raise UiE2EFailure("UI_ELEMENT_DISABLED")
        left, top, right, bottom = self._rectangle(node)
        self.run("shell", "input", "tap", str((left + right) // 2), str((top + bottom) // 2))
        time.sleep(0.2)

    def set_text(self, automation_id: str, value: str, verify_plaintext: bool = True) -> None:
        if not value or not SAFE_INPUT.fullmatch(value):
            raise UiE2EFailure("INPUT_CHARACTER_SET_UNSUPPORTED")
        self.focus_input(automation_id)
        # CI credentials are generated from hexadecimal/base64 alphabets. Reject rather than guess
        # when an input cannot be typed deterministically by Android's standard input command.
        self.run("shell", "input", "text", value, timeout=30)
        if verify_plaintext:
            try:
                self.wait_for(automation_id, lambda node: node.attrib.get("text") == value, 10)
            except UiE2EFailure as error:
                if str(error) != "UI_WAIT_TIMEOUT":
                    raise
                found = self.nodes(automation_id)
                if len(found) != 1:
                    raise UiE2EFailure("UI_TEXT_VERIFY_ELEMENT_COUNT_INVALID") from error
                node = found[0]
                observed = node.attrib.get("text", "")
                text_state = "EXACT" if observed == value else "EMPTY" if not observed else "MISMATCH"
                focus_state = "FOCUSED" if node.attrib.get("focused") == "true" else "NOT_FOCUSED"
                raise UiE2EFailure(f"UI_TEXT_VERIFY_{text_state}_{focus_state}") from error

    def focus_input(self, automation_id: str) -> None:
        movements: list[str] = []
        for attempt in range(5):
            self.click(automation_id)
            try:
                self.wait_for(
                    automation_id,
                    lambda node: node.attrib.get("focused") == "true",
                    2,
                )
                return
            except UiE2EFailure as error:
                if str(error) != "UI_WAIT_TIMEOUT":
                    raise
                if attempt == 4:
                    movement = self._aggregate_scroll_movement(movements)
                    observation = self.safe_focus_observation(automation_id, movement)
                    raise UiE2EFailure(f"UI_INPUT_FOCUS_FAILED:{observation}") from error
                movements.append(self._scroll(toward_bottom=True))
        raise UiE2EFailure("UI_INPUT_FOCUS_FAILED")

    def safe_focus_observation(self, automation_id: str, scroll_movement: str) -> str:
        ime = self.safe_ime_state()
        unavailable = (
            "COUNT_UNKNOWN:VISIBLE_UNKNOWN:FOCUSABLE_UNKNOWN:CLICKABLE_UNKNOWN:"
            f"OWNER_UNKNOWN:ZONE_UNKNOWN:SCROLL_{scroll_movement}:IME_{ime}"
        )
        try:
            hierarchy = self.dump()
            targets = [node for node in hierarchy.iter("node") if self._matches(node, automation_id)]
            count = "ZERO" if not targets else "ONE" if len(targets) == 1 else "MULTIPLE"
            if len(targets) != 1:
                return (
                    f"COUNT_{count}:VISIBLE_UNKNOWN:FOCUSABLE_UNKNOWN:CLICKABLE_UNKNOWN:"
                    f"OWNER_{self._safe_focus_owner(hierarchy)}:ZONE_UNKNOWN:"
                    f"SCROLL_{scroll_movement}:IME_{ime}"
                )
            target = targets[0]
            visible = self._safe_boolean(target.attrib.get("visible-to-user"))
            focusable = self._safe_boolean(target.attrib.get("focusable"))
            clickable = self._safe_boolean(target.attrib.get("clickable"))
            zone = self._safe_vertical_zone(hierarchy, target)
            return (
                f"COUNT_ONE:VISIBLE_{visible}:FOCUSABLE_{focusable}:CLICKABLE_{clickable}:"
                f"OWNER_{self._safe_focus_owner(hierarchy)}:ZONE_{zone}:"
                f"SCROLL_{scroll_movement}:IME_{ime}"
            )
        except (UiE2EFailure, ValueError, TypeError):
            return unavailable

    @staticmethod
    def _safe_boolean(value: str | None) -> str:
        return "TRUE" if value == "true" else "FALSE" if value == "false" else "UNKNOWN"

    def _safe_focus_owner(self, hierarchy: ET.Element) -> str:
        focused = [node for node in hierarchy.iter("node") if node.attrib.get("focused") == "true"]
        if not focused:
            return "NONE"
        if len(focused) != 1:
            return "MULTIPLE"
        matches = [
            label
            for candidate, label in FOCUS_OWNER_ALLOWLIST
            if self._matches(focused[0], candidate)
        ]
        return matches[0] if len(matches) == 1 else "OTHER"

    def _safe_vertical_zone(self, hierarchy: ET.Element, target: ET.Element) -> str:
        roots = [node for node in hierarchy.iter("node") if self._matches(node, AUTOMATION_ROOT)]
        if len(roots) != 1:
            return "UNKNOWN"
        _, target_top, _, target_bottom = self._rectangle(target)
        _, root_top, _, root_bottom = self._rectangle(roots[0])
        target_center = (target_top + target_bottom) // 2
        if target_center < root_top or target_center > root_bottom:
            return "OUTSIDE"
        relative = target_center - root_top
        height = root_bottom - root_top
        if relative * 3 < height:
            return "UPPER"
        if relative * 3 < height * 2:
            return "MIDDLE"
        return "LOWER"

    def safe_ime_state(self) -> str:
        try:
            payload = self.run("shell", "dumpsys", "input_method")
        except UiE2EFailure:
            return "UNKNOWN"
        return self._parse_ime_state(payload)

    @staticmethod
    def _parse_ime_state(payload: str) -> str:
        matches = re.findall(
            r"(?m)^\s*(?:mShowRequested=(?:true|false)\s+"
            r"mShowExplicitlyRequested=(?:true|false)\s+"
            r"mShowForced=(?:true|false)\s+)?mInputShown=(true|false)\s*$",
            payload,
        )
        if len(matches) != 1:
            return "UNKNOWN"
        return "SHOWN" if matches[0] == "true" else "HIDDEN"

    def hide_keyboard(self) -> None:
        self.run("shell", "input", "keyevent", "KEYCODE_BACK")
        time.sleep(0.25)

    def text(self, automation_id: str) -> str:
        return self.find(automation_id).attrib.get("text", "")

    def wait_text(self, automation_id: str, expected: str, timeout_seconds: int | None = None) -> None:
        deadline = time.monotonic() + (timeout_seconds or self.timeout_seconds)
        while time.monotonic() < deadline:
            try:
                node = self.find(automation_id)
            except UiE2EFailure as error:
                if not str(error).startswith("UI_AUTOMATION_ID_NOT_FOUND:SCROLL_"):
                    raise
            else:
                if node.attrib.get("text") == expected:
                    return
            time.sleep(0.5)
        raise UiE2EFailure("UI_WAIT_TIMEOUT")

    def operation_summaries(self) -> list[str]:
        self.find("driver_operation_list")
        return [node.attrib.get("text", "") for node in self.nodes("driver_operation_summary")]

    def wait_for_new_operation(self, existing_ids: set[str]) -> str:
        deadline = time.monotonic() + self.timeout_seconds
        while time.monotonic() < deadline:
            for summary in self.operation_summaries():
                match = OPERATION_ID.match(summary)
                if match is not None and match.group(1) not in existing_ids:
                    return match.group(1)
            self.click("driver_refresh")
            time.sleep(0.75)
        raise UiE2EFailure("NEW_OPERATION_NOT_VISIBLE")

    def wait_for_operation_success(self, operation_id: str) -> None:
        deadline = time.monotonic() + self.timeout_seconds
        while time.monotonic() < deadline:
            summaries = self.operation_summaries()
            matching = next((value for value in summaries if value.startswith(operation_id + " |")), None)
            if matching is not None and "| Succeeded |" in matching and "result=SUCCEEDED" in matching:
                return
            if matching is not None and ("| Rejected |" in matching or "| Failed |" in matching):
                raise UiE2EFailure("OPERATION_TERMINAL_FAILURE")
            self.click("driver_refresh")
            time.sleep(1)
        raise UiE2EFailure("OPERATION_SUCCESS_TIMEOUT")


def read_input(path: Path) -> dict[str, str]:
    if not path.is_file():
        raise UiE2EFailure("INPUT_FILE_MISSING")
    if os.name == "posix" and stat.S_IMODE(path.stat().st_mode) & 0o077:
        raise UiE2EFailure("INPUT_FILE_PERMISSIONS_UNSAFE")
    try:
        value = json.loads(path.read_text(encoding="utf-8"))
    except (OSError, UnicodeError, json.JSONDecodeError) as error:
        raise UiE2EFailure("INPUT_FILE_INVALID") from error
    required = ("userName", "password", "companyId", "branchId", "deviceId", "deviceCredential")
    if not isinstance(value, dict) or set(value) != set(required) or any(
        not isinstance(value.get(name), str) or not value[name] for name in required
    ):
        raise UiE2EFailure("INPUT_FILE_INVALID")
    try:
        if uuid.UUID(value["companyId"]).int == 0 or uuid.UUID(value["branchId"]).int == 0:
            raise ValueError
    except ValueError as error:
        raise UiE2EFailure("INPUT_SCOPE_INVALID") from error
    return value


def operation_ids(summaries: Iterable[str]) -> set[str]:
    values: set[str] = set()
    for summary in summaries:
        match = OPERATION_ID.match(summary)
        if match is not None:
            values.add(match.group(1))
    return values


def safe_sign_in_observation(driver: Driver, phase: str) -> str:
    try:
        if phase == "ACTION_RESULT":
            value = driver.text("driver_action_result")
            match = re.fullmatch(r"Result: ([A-Z0-9_]{1,64})", value)
            return "OBSERVED_RESULT_" + match.group(1) if match is not None else "OBSERVATION_UNAVAILABLE"
        if phase == "MODE_READY":
            value = driver.text("driver_mode")
            match = re.fullmatch(r"Offline runtime: (CLOSED|READY)", value)
            return "OBSERVED_MODE_" + match.group(1) if match is not None else "OBSERVATION_UNAVAILABLE"
    except UiE2EFailure:
        pass
    return "OBSERVATION_UNAVAILABLE"


def sign_in_and_activate(driver: Driver, secret_input: dict[str, str]) -> None:
    phase = "USER_NAME"
    try:
        driver.set_text("driver_user_name", secret_input["userName"])
        phase = "PASSWORD"
        driver.set_text("driver_password", secret_input["password"], verify_plaintext=False)
        phase = "COMPANY_ID"
        driver.set_text("driver_company_id", secret_input["companyId"])
        phase = "BRANCH_ID"
        driver.set_text("driver_branch_id", secret_input["branchId"])
        phase = "DEVICE_ID"
        driver.set_text("driver_device_id", secret_input["deviceId"])
        phase = "DEVICE_CREDENTIAL"
        driver.set_text(
            "driver_device_credential",
            secret_input["deviceCredential"],
            verify_plaintext=False,
        )
        phase = "HIDE_KEYBOARD"
        driver.hide_keyboard()
        phase = "SUBMIT"
        driver.click("driver_sign_in")
        phase = "ACTION_RESULT"
        driver.wait_text("driver_action_result", "Result: OFFLINE_ACTIVATED")
        phase = "MODE_READY"
        driver.wait_text("driver_mode", "Offline runtime: READY")
    except UiE2EFailure as error:
        observation = safe_sign_in_observation(driver, phase)
        raise UiE2EFailure(f"{phase}:{observation}:{error}") from error


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--input", required=True, type=Path)
    parser.add_argument("--evidence", required=True, type=Path)
    parser.add_argument("--adb", default=os.environ.get("ADB", "adb"))
    parser.add_argument("--package", default=DEFAULT_PACKAGE)
    parser.add_argument("--timeout-seconds", type=int, default=180)
    arguments = parser.parse_args()
    if arguments.timeout_seconds < 30 or arguments.timeout_seconds > 900:
        raise UiE2EFailure("TIMEOUT_INVALID")

    secret_input = read_input(arguments.input)
    driver = Driver(arguments.adb, arguments.package, arguments.timeout_seconds)
    evidence = {
        "schemaVersion": 1,
        "activity": "ordinary-launcher",
        "nonDebuggableRelease": False,
        "closedDefault": False,
        "authenticatedActivation": False,
        "businessOperationQueued": False,
        "businessOperationSucceeded": False,
        "persistedAfterReleaseRestart": False,
        "signedOutClosed": False,
    }
    phase = "INITIAL_LAUNCH"
    try:
        driver.launch_ordinary_activity()
        evidence["nonDebuggableRelease"] = True
        phase = "INITIAL_CLOSED_MODE"
        driver.wait_text("driver_mode", "Offline runtime: CLOSED")
        phase = "INITIAL_CLOSED_REASON"
        driver.wait_text("driver_reason", "Reason: OFFLINE_CLOSED")
        phase = "INITIAL_CLOSED_WRITE_CONTROL"
        if driver.find("driver_queue_party").attrib.get("enabled") != "false":
            raise UiE2EFailure("CLOSED_DEFAULT_WRITE_ENABLED")
        evidence["closedDefault"] = True

        phase = "INITIAL_SIGN_IN"
        sign_in_and_activate(driver, secret_input)
        evidence["authenticatedActivation"] = True

        phase = "INITIAL_OPERATION_LIST"
        existing = operation_ids(driver.operation_summaries())
        suffix = uuid.uuid4().hex[:12]
        phase = "QUEUE_PARTY_NAME"
        driver.set_text("driver_party_name", "UIE2E-" + suffix)
        phase = "QUEUE_PARTY_MOBILE"
        driver.set_text("driver_party_mobile", "700" + suffix[:6])
        phase = "QUEUE_PARTY_ADDRESS"
        driver.set_text("driver_party_address", "UIE2E-Address-" + suffix)
        driver.hide_keyboard()
        phase = "QUEUE_PARTY_ACTION"
        driver.click("driver_queue_party")
        phase = "QUEUE_PARTY_RESULT"
        driver.wait_text("driver_action_result", "Result: BUSINESS_OPERATION_QUEUED")
        evidence["businessOperationQueued"] = True

        phase = "NEW_OPERATION_VISIBLE"
        operation_id = driver.wait_for_new_operation(existing)
        phase = "INITIAL_OPERATION_SUCCESS"
        driver.wait_for_operation_success(operation_id)
        evidence["businessOperationSucceeded"] = True

        # Android's ordinary force-stop is the crash/restart boundary. Relaunch the same installed
        # non-debuggable package, reauthenticate through the normal UI, and prove the exact local
        # operation/result survived encrypted storage without a test activity or internal hook.
        phase = "RESTART_FORCE_STOP"
        driver.run("shell", "am", "force-stop", arguments.package)
        phase = "RESTART_LAUNCH"
        driver.launch_ordinary_activity()
        phase = "RESTART_CLOSED_MODE"
        driver.wait_text("driver_mode", "Offline runtime: CLOSED")
        phase = "RESTART_SIGN_IN"
        sign_in_and_activate(driver, secret_input)
        phase = "PERSISTED_OPERATION_SUCCESS"
        driver.wait_for_operation_success(operation_id)
        evidence["persistedAfterReleaseRestart"] = True

        phase = "SIGN_OUT_ACTION"
        driver.click("driver_sign_out")
        phase = "SIGN_OUT_CLOSED_MODE"
        driver.wait_text("driver_mode", "Offline runtime: CLOSED")
        phase = "SIGN_OUT_WRITE_CONTROL"
        if driver.find("driver_queue_party").attrib.get("enabled") != "false":
            raise UiE2EFailure("SIGNED_OUT_WRITE_ENABLED")
        evidence["signedOutClosed"] = True
        arguments.evidence.parent.mkdir(parents=True, exist_ok=True)
        arguments.evidence.write_text(
            json.dumps(evidence, sort_keys=True, separators=(",", ":")) + "\n",
            encoding="utf-8",
        )
        return 0
    except UiE2EFailure as error:
        raise UiE2EFailure(f"{phase}:{error}") from error
    finally:
        # A process stop is not a success substitute. It only guarantees volatile bearer teardown
        # when a UI assertion fails before the normal sign-out path.
        try:
            driver.run("shell", "am", "force-stop", arguments.package)
        except UiE2EFailure:
            pass


if __name__ == "__main__":
    try:
        raise SystemExit(main())
    except UiE2EFailure as error:
        print(f"ANDROID_RELEASE_UI_E2E_FAILED:{error}", file=sys.stderr)
        raise SystemExit(1)
