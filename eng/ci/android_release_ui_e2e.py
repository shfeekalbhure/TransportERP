#!/usr/bin/env python3
"""Drive the ordinary TransportERP Driver launcher activity through stable AutomationIds.

The script intentionally has no test activity, HTTP shortcut, application certificate-validation
callback, database hook, or application-internal hook. It binds the ephemeral CI root only inside
each launched emulator process mount namespace before UI/network actions. Its input file contains
secrets and must be owner-readable only. Output evidence contains fixed phase/result codes only.
"""

from __future__ import annotations

import argparse
import hashlib
import hmac
import json
import os
import re
import ssl
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
CONSCRYPT_ALIAS = re.compile(r"^[0-9a-f]{8}\.[0-9]{1,2}$")
LOWER_SHA256 = re.compile(r"^[0-9a-f]{64}$")
BOUNDS = re.compile(r"^\[(\d+),(\d+)\]\[(\d+),(\d+)\]$")
OPERATION_ID = re.compile(r"^([0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}) \|")
OPERATION_SUFFIX = re.compile(r"^[0-9a-f]{12}$")
SAFE_RESULT = re.compile(r"Result: ([A-Z0-9_]{1,64})")
INITIAL_ACTION_PROMPT = "Sign in and explicitly activate an authorized scope to use synchronization."
SAFE_SIGN_IN_RESULT_CODES = frozenset(
    {
        "AUTHENTICATED_SCOPE_INVALID",
        "AUTHENTICATION_FAILED",
        "AUTHENTICATION_INPUT_INVALID",
        "BUILD_IDENTITY_UNAVAILABLE",
        "BUILD_SIGNER_IDENTITY_UNAVAILABLE",
        "DEVICE_KEY_ALREADY_PROVISIONED",
        "DEVICE_KEY_ENROLLMENT_AUTHORITY_INVALID",
        "DEVICE_KEY_ENROLLMENT_CHALLENGE_INVALID",
        "DEVICE_KEY_ENROLLMENT_CHALLENGE_REJECTED",
        "DEVICE_KEY_ENROLLMENT_NOT_AUTHORIZED",
        "DEVICE_KEY_ENROLLMENT_REJECTED",
        "DEVICE_KEY_ENROLLMENT_RESPONSE_INVALID",
        "DEVICE_KEY_REBIND_REQUIRED",
        "DEVICE_KEY_RECOVERY_REAUTHENTICATION_REQUIRED",
        "DRIVER_OFFLINE_ALREADY_ACTIVE",
        "NATIVE_DEVICE_SIGNATURE_DER_INVALID",
        "NATIVE_DEVICE_SIGNING_INITIALIZATION_FAILED",
        "NATIVE_DEVICE_SIGNING_INPUT_FAILED",
        "NATIVE_DEVICE_SIGNING_KEY_READ_FAILED",
        "NATIVE_DEVICE_SIGNING_KEY_UNAVAILABLE",
        "NATIVE_DEVICE_SIGNING_OPERATION_FAILED",
        "NATIVE_DEVICE_SIGNING_PROVIDER_UNAVAILABLE",
        "NATIVE_SECURE_STORAGE_KEY_INVALID",
        "NATIVE_SECURE_STORAGE_UNAVAILABLE",
        "NETWORK_UNAVAILABLE",
        "OFFLINE_ACTIVATED",
        "OFFLINE_ACTIVATION_AUTHORITY_UNAVAILABLE",
        "OFFLINE_ACTIVATION_DECISION_INVALID",
        "OFFLINE_CLOSED",
        "OPERATION_FAILED",
        "PROOF_KEY_BINDING_REQUIRED",
        "SERVER_ORIGIN_INVALID",
        "AUTH_SESSION_CONNECTION_FAILED",
        "AUTH_SESSION_HTTP_PROTOCOL_FAILED",
        "AUTH_SESSION_NAME_RESOLUTION_FAILED",
        "AUTH_SESSION_TIMEOUT",
        "SERVER_RESPONSE_INVALID",
        "SERVER_RESPONSE_TOO_LARGE",
        "AUTH_SESSION_TLS_FAILED",
        "AUTH_SESSION_TRANSPORT_FAILED",
        "SESSION_TOKEN_UNAVAILABLE",
    }
)
SAFE_SIGN_OUT_ACKNOWLEDGEMENT_CODES = frozenset(
    {"SIGN_OUT_IN_PROGRESS", "SIGNED_OUT"}
)
SIGN_OUT_ACKNOWLEDGEMENT_TIMEOUT_SECONDS = 10
IME_DISMISS_TIMEOUT_SECONDS = 5
IME_DISMISS_POLL_SECONDS = 0.25
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
FIND_EXHAUSTION_DIAGNOSTIC_TARGETS = frozenset({"driver_sign_out"})


class UiE2EFailure(RuntimeError):
    pass


def mobile_for_operation_suffix(suffix: str) -> str:
    if not OPERATION_SUFFIX.fullmatch(suffix):
        raise UiE2EFailure("OPERATION_SUFFIX_INVALID")
    return f"700{int(suffix, 16) % 1_000_000:06d}"


class Driver:
    def __init__(
        self,
        adb: str,
        package: str,
        timeout_seconds: int,
        conscrypt_alias: str | None = None,
        conscrypt_root_sha256: str | None = None,
    ) -> None:
        self.adb = adb
        self.package = package
        self.timeout_seconds = timeout_seconds
        self.conscrypt_alias = conscrypt_alias
        self.conscrypt_root_sha256 = conscrypt_root_sha256
        self.conscrypt_process_bind_count = 0
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

    def run_binary(self, *arguments: str, timeout: int = 30) -> bytes:
        try:
            completed = subprocess.run(
                [self.adb, *arguments],
                check=False,
                capture_output=True,
                timeout=timeout,
            )
        except (OSError, subprocess.TimeoutExpired) as error:
            raise UiE2EFailure("ADB_UNAVAILABLE") from error
        if completed.returncode != 0:
            raise UiE2EFailure("ADB_COMMAND_FAILED")
        return completed.stdout

    def bind_conscrypt_trust_for_current_process(self) -> None:
        if (
            self.conscrypt_alias is None
            or CONSCRYPT_ALIAS.fullmatch(self.conscrypt_alias) is None
            or self.conscrypt_root_sha256 is None
            or LOWER_SHA256.fullmatch(self.conscrypt_root_sha256) is None
        ):
            raise UiE2EFailure("RELEASE_CONSCRYPT_INPUT_INVALID")
        try:
            process_pid = self.run("shell", "pidof", self.package).strip()
        except UiE2EFailure as error:
            raise UiE2EFailure("RELEASE_PROCESS_PID_UNAVAILABLE") from error
        if re.fullmatch(r"[0-9]+", process_pid) is None:
            raise UiE2EFailure("RELEASE_PROCESS_PID_INVALID")
        system_directory = "/system/etc/security/cacerts"
        conscrypt_directory = "/apex/com.android.conscrypt/cacerts"
        certificate_path = f"{conscrypt_directory}/{self.conscrypt_alias}"
        try:
            self.run(
                "shell",
                "nsenter",
                "-t",
                process_pid,
                "-m",
                "--",
                "mount",
                "--bind",
                system_directory,
                conscrypt_directory,
            )
        except UiE2EFailure as error:
            raise UiE2EFailure("RELEASE_CONSCRYPT_BIND_FAILED") from error
        try:
            certificate = self.run_binary(
                "exec-out",
                "nsenter",
                "-t",
                process_pid,
                "-m",
                "--",
                "cat",
                certificate_path,
            )
        except UiE2EFailure as error:
            raise UiE2EFailure("RELEASE_CONSCRYPT_ROOT_UNAVAILABLE") from error
        try:
            certificate_der = ssl.PEM_cert_to_DER_cert(certificate.decode("ascii"))
        except (UnicodeDecodeError, ValueError) as error:
            raise UiE2EFailure("RELEASE_CONSCRYPT_ROOT_INVALID") from error
        actual_sha256 = hashlib.sha256(certificate_der).hexdigest()
        if not hmac.compare_digest(actual_sha256, self.conscrypt_root_sha256):
            raise UiE2EFailure("RELEASE_CONSCRYPT_ROOT_MISMATCH")
        self.conscrypt_process_bind_count += 1

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
        self.bind_conscrypt_trust_for_current_process()
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

    @staticmethod
    def _safe_axis_relation(
        target_start: int,
        target_end: int,
        root_start: int,
        root_end: int,
        before: str,
        after: str,
    ) -> str:
        if target_end <= root_start:
            return before
        if target_start >= root_end:
            return after
        if target_start >= root_start and target_end <= root_end:
            return "INSIDE"
        return "OVERLAP"

    def safe_find_exhaustion_observation(
        self,
        automation_id: str,
        up_movements: list[str],
        down_movements: list[str],
        top_anchor_seen: bool,
    ) -> str:
        if automation_id not in FIND_EXHAUSTION_DIAGNOSTIC_TARGETS:
            return "OBSERVATION_UNAVAILABLE"
        up = up_movements[-1] if up_movements and up_movements[-1] in {
            "MOVED", "UNCHANGED", "UNKNOWN"
        } else "UNKNOWN"
        down = down_movements[-1] if down_movements and down_movements[-1] in {
            "MOVED", "UNCHANGED", "UNKNOWN"
        } else "UNKNOWN"
        top_anchor = "TRUE" if top_anchor_seen is True else "FALSE"
        unavailable = (
            "COUNT_UNKNOWN:VISIBLE_UNKNOWN:ENABLED_UNKNOWN:"
            f"X_UNKNOWN:Y_UNKNOWN:UP_{up}:DOWN_{down}:TOP_ANCHOR_SEEN_{top_anchor}"
        )
        try:
            hierarchy = self.dump()
            targets = [
                node for node in hierarchy.iter("node") if self._matches(node, automation_id)
            ]
            count = "ZERO" if not targets else "ONE" if len(targets) == 1 else "MULTIPLE"
            if len(targets) != 1:
                return (
                    f"COUNT_{count}:VISIBLE_UNKNOWN:ENABLED_UNKNOWN:"
                    f"X_UNKNOWN:Y_UNKNOWN:UP_{up}:DOWN_{down}:"
                    f"TOP_ANCHOR_SEEN_{top_anchor}"
                )
            target = targets[0]
            visible = self._safe_boolean(target.attrib.get("visible-to-user"))
            enabled = self._safe_boolean(target.attrib.get("enabled"))
            roots = [
                node for node in hierarchy.iter("node") if self._matches(node, AUTOMATION_ROOT)
            ]
            if len(roots) != 1:
                return (
                    f"COUNT_ONE:VISIBLE_{visible}:ENABLED_{enabled}:"
                    f"X_UNKNOWN:Y_UNKNOWN:UP_{up}:DOWN_{down}:"
                    f"TOP_ANCHOR_SEEN_{top_anchor}"
                )
            target_left, target_top, target_right, target_bottom = self._rectangle(target)
            root_left, root_top, root_right, root_bottom = self._rectangle(roots[0])
            x_relation = self._safe_axis_relation(
                target_left, target_right, root_left, root_right, "LEFT", "RIGHT"
            )
            y_relation = self._safe_axis_relation(
                target_top, target_bottom, root_top, root_bottom, "ABOVE", "BELOW"
            )
            return (
                f"COUNT_ONE:VISIBLE_{visible}:ENABLED_{enabled}:"
                f"X_{x_relation}:Y_{y_relation}:UP_{up}:DOWN_{down}:"
                f"TOP_ANCHOR_SEEN_{top_anchor}"
            )
        except (UiE2EFailure, ValueError, TypeError):
            return unavailable

    def find(self, automation_id: str) -> ET.Element:
        found = self.nodes(automation_id)
        if found:
            return found[0]
        up_movements: list[str] = []
        top_anchor_seen = False
        for _ in range(6):
            up_movements.append(self._scroll(toward_bottom=False))
            found = self.nodes(automation_id, self._last_scroll_hierarchy)
            if found:
                return found[0]
            if automation_id == "driver_sign_out" and self.nodes(
                "driver_sign_in", self._last_scroll_hierarchy
            ):
                top_anchor_seen = True
        down_movements: list[str] = []
        for _ in range(14):
            down_movements.append(self._scroll(toward_bottom=True))
            found = self.nodes(automation_id, self._last_scroll_hierarchy)
            if found:
                return found[0]
        movement = self._aggregate_scroll_movement(up_movements + down_movements)
        observation = self.safe_find_exhaustion_observation(
            automation_id, up_movements, down_movements, top_anchor_seen
        )
        suffix = "" if observation == "OBSERVATION_UNAVAILABLE" else f":{observation}"
        raise UiE2EFailure(f"UI_AUTOMATION_ID_NOT_FOUND:SCROLL_{movement}{suffix}")

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
        self.dismiss_ime_if_shown()
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
            return self._read_ime_state()
        except UiE2EFailure:
            return "UNKNOWN"

    def _read_ime_state(self) -> str:
        payload = self.run("shell", "dumpsys", "input_method")
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

    def dismiss_ime_if_shown(self) -> None:
        if self._read_ime_state() != "SHOWN":
            return
        self.run("shell", "input", "keyevent", "KEYCODE_BACK")
        deadline = time.monotonic() + IME_DISMISS_TIMEOUT_SECONDS
        while time.monotonic() < deadline:
            if self._read_ime_state() == "HIDDEN":
                roots = self.nodes(AUTOMATION_ROOT)
                if len(roots) != 1:
                    raise UiE2EFailure("UI_IME_DISMISS_ROOT_INVALID")
                return
            time.sleep(IME_DISMISS_POLL_SECONDS)
        raise UiE2EFailure("UI_IME_DISMISS_TIMEOUT")

    def hide_keyboard(self) -> None:
        self.dismiss_ime_if_shown()

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

    def wait_enabled(self, automation_id: str, timeout_seconds: int | None = None) -> None:
        deadline = time.monotonic() + (timeout_seconds or self.timeout_seconds)
        while time.monotonic() < deadline:
            try:
                node = self.find(automation_id)
            except UiE2EFailure as error:
                if not str(error).startswith("UI_AUTOMATION_ID_NOT_FOUND:SCROLL_"):
                    raise
            else:
                if node.attrib.get("enabled") == "true":
                    return
            time.sleep(0.5)
        raise UiE2EFailure("UI_WAIT_TIMEOUT")

    def wait_result_code(self, automation_id: str, expected_code: str) -> None:
        if expected_code not in SAFE_SIGN_IN_RESULT_CODES:
            raise UiE2EFailure("EXPECTED_RESULT_CODE_INVALID")
        expected = f"Result: {expected_code}"
        deadline = time.monotonic() + self.timeout_seconds
        while time.monotonic() < deadline:
            try:
                node = self.find(automation_id)
            except UiE2EFailure as error:
                if not str(error).startswith("UI_AUTOMATION_ID_NOT_FOUND:SCROLL_"):
                    raise
            else:
                value = node.attrib.get("text", "")
                if value == expected:
                    return
                match = SAFE_RESULT.fullmatch(value)
                if match is not None:
                    code = match.group(1)
                    raise UiE2EFailure(
                        f"UI_RESULT_{code}" if code in SAFE_SIGN_IN_RESULT_CODES else "UI_RESULT_OTHER"
                    )
            time.sleep(0.5)
        raise UiE2EFailure("UI_WAIT_TIMEOUT")

    def wait_sign_out_acknowledgement(self, automation_id: str) -> None:
        deadline = time.monotonic() + SIGN_OUT_ACKNOWLEDGEMENT_TIMEOUT_SECONDS
        while time.monotonic() < deadline:
            try:
                node = self.find(automation_id)
            except UiE2EFailure as error:
                if not str(error).startswith("UI_AUTOMATION_ID_NOT_FOUND:SCROLL_"):
                    raise
            else:
                match = SAFE_RESULT.fullmatch(node.attrib.get("text", ""))
                if match is not None:
                    code = match.group(1)
                    if code in SAFE_SIGN_OUT_ACKNOWLEDGEMENT_CODES:
                        return
                    if code == "SIGN_OUT_BUSY":
                        raise UiE2EFailure("SIGN_OUT_BUSY")
            time.sleep(0.5)
        raise UiE2EFailure("SIGN_OUT_ACTION_NOT_ACCEPTED")

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


def safe_visible_element(driver: Driver, automation_id: str) -> tuple[str, ET.Element | None]:
    try:
        driver.find(automation_id)
        hierarchy = driver.dump()
        nodes = driver.nodes(automation_id, hierarchy)
    except UiE2EFailure:
        return "UNKNOWN", None
    if not nodes:
        return "ZERO", None
    if len(nodes) != 1:
        return "MULTIPLE", None
    return "ONE", nodes[0]


def safe_sign_in_observation(driver: Driver, phase: str) -> str:
    if phase not in ("ACTION_RESULT", "MODE_READY"):
        return "OBSERVATION_UNAVAILABLE"

    result_count, result_node = safe_visible_element(driver, "driver_action_result")
    if result_node is None:
        result_state = "UNKNOWN"
    else:
        value = result_node.attrib.get("text", "")
        match = SAFE_RESULT.fullmatch(value)
        result_state = (
            "CODE_" + match.group(1)
            if match is not None and match.group(1) in SAFE_SIGN_IN_RESULT_CODES
            else "OTHER"
            if match is not None
            else "INITIAL_PROMPT"
            if value == INITIAL_ACTION_PROMPT
            else "EMPTY"
            if not value
            else "OTHER"
        )

    mode_count, mode_node = safe_visible_element(driver, "driver_mode")
    if mode_node is None:
        mode_state = "UNKNOWN"
    else:
        match = re.fullmatch(r"Offline runtime: (CLOSED|READY)", mode_node.attrib.get("text", ""))
        mode_state = match.group(1) if match is not None else "OTHER"

    sign_in_count, sign_in_node = safe_visible_element(driver, "driver_sign_in")
    if sign_in_node is None:
        sign_in_state = "UNKNOWN"
    else:
        enabled = sign_in_node.attrib.get("enabled")
        sign_in_state = "TRUE" if enabled == "true" else "FALSE" if enabled == "false" else "UNKNOWN"

    return (
        f"RESULT_COUNT_{result_count}:RESULT_{result_state}:"
        f"MODE_COUNT_{mode_count}:MODE_{mode_state}:"
        f"SIGN_IN_COUNT_{sign_in_count}:SIGN_IN_ENABLED_{sign_in_state}"
    )


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
        driver.wait_result_code("driver_action_result", "OFFLINE_ACTIVATED")
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
    parser.add_argument("--conscrypt-alias", required=True)
    parser.add_argument("--conscrypt-root-sha256", required=True)
    arguments = parser.parse_args()
    if arguments.timeout_seconds < 30 or arguments.timeout_seconds > 900:
        raise UiE2EFailure("TIMEOUT_INVALID")

    secret_input = read_input(arguments.input)
    driver = Driver(
        arguments.adb,
        arguments.package,
        arguments.timeout_seconds,
        arguments.conscrypt_alias,
        arguments.conscrypt_root_sha256,
    )
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
        "conscryptTrustProcessLaunchCount": 0,
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
        driver.set_text("driver_party_mobile", mobile_for_operation_suffix(suffix))
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
        driver.wait_enabled("driver_sign_out")
        driver.click("driver_sign_out")
        phase = "SIGN_OUT_ACKNOWLEDGEMENT"
        driver.wait_sign_out_acknowledgement("driver_action_result")
        phase = "SIGN_OUT_CLOSED_MODE"
        driver.wait_text("driver_mode", "Offline runtime: CLOSED")
        phase = "SIGN_OUT_WRITE_CONTROL"
        if driver.find("driver_queue_party").attrib.get("enabled") != "false":
            raise UiE2EFailure("SIGNED_OUT_WRITE_ENABLED")
        evidence["signedOutClosed"] = True
        if driver.conscrypt_process_bind_count != 2:
            raise UiE2EFailure("RELEASE_CONSCRYPT_LAUNCH_COUNT_INVALID")
        evidence["conscryptTrustProcessLaunchCount"] = driver.conscrypt_process_bind_count
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
