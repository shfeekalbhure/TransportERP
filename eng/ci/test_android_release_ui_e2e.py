#!/usr/bin/env python3
"""Fail-closed unit tests for the ordinary Android Release UI harness."""

from __future__ import annotations

import importlib.util
import unittest
from pathlib import Path
from unittest import mock


MODULE_PATH = Path(__file__).with_name("android_release_ui_e2e.py")
SPEC = importlib.util.spec_from_file_location("android_release_ui_e2e_under_test", MODULE_PATH)
if SPEC is None or SPEC.loader is None:
    raise RuntimeError("ANDROID_UI_E2E_MODULE_LOAD_FAILED")
HARNESS = importlib.util.module_from_spec(SPEC)
SPEC.loader.exec_module(HARNESS)


class ConditionalImeDismissalTests(unittest.TestCase):
    def driver(
        self,
        states: list[str | BaseException],
        *,
        root_count: int = 1,
        fail_back: bool = False,
        fail_root: bool = False,
    ) -> tuple[object, list[str]]:
        driver = HARNESS.Driver("adb", "pkg", 30)
        events: list[str] = []
        remaining = iter(states)

        def read_state() -> str:
            events.append("READ_IME")
            state = next(remaining)
            if isinstance(state, BaseException):
                raise state
            return state

        def run(*arguments: str, **_: object) -> str:
            if arguments[-1] == "KEYCODE_BACK":
                events.append("BACK")
                if fail_back:
                    raise HARNESS.UiE2EFailure("ADB_COMMAND_FAILED")
            else:
                events.append("TYPE")
            return ""

        def nodes(_: str) -> list[object]:
            events.append("ROOT")
            if fail_root:
                raise HARNESS.UiE2EFailure("UI_HIERARCHY_INVALID")
            return [object() for _ in range(root_count)]

        driver._read_ime_state = read_state
        driver.run = run
        driver.nodes = nodes
        driver.focus_input = lambda _: events.append("FOCUS")
        return driver, events

    def test_shown_is_hidden_before_focus_and_type(self) -> None:
        driver, events = self.driver(["SHOWN", "UNKNOWN", "HIDDEN"])
        with mock.patch.object(HARNESS.time, "sleep", return_value=None):
            driver.set_text("driver_company_id", "SAFE", verify_plaintext=False)
        self.assertEqual(
            ["READ_IME", "BACK", "READ_IME", "READ_IME", "ROOT", "FOCUS", "TYPE"],
            events,
        )

    def test_hidden_and_unknown_never_send_back(self) -> None:
        for state in ("HIDDEN", "UNKNOWN"):
            with self.subTest(state=state):
                driver, events = self.driver([state])
                driver.set_text("driver_company_id", "SAFE", verify_plaintext=False)
                self.assertEqual(["READ_IME", "FOCUS", "TYPE"], events)

    def test_never_hidden_times_out_before_focus_or_type(self) -> None:
        driver, events = self.driver(["SHOWN"])
        with mock.patch.object(HARNESS, "IME_DISMISS_TIMEOUT_SECONDS", 0):
            with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^UI_IME_DISMISS_TIMEOUT$"):
                driver.set_text("driver_company_id", "SAFE", verify_plaintext=False)
        self.assertEqual(["READ_IME", "BACK"], events)

    def test_back_failure_stops_before_focus_or_type(self) -> None:
        driver, events = self.driver(["SHOWN"], fail_back=True)
        with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^ADB_COMMAND_FAILED$"):
            driver.set_text("driver_company_id", "SAFE", verify_plaintext=False)
        self.assertEqual(["READ_IME", "BACK"], events)

    def test_state_read_failure_stops_before_focus_or_type(self) -> None:
        failure = HARNESS.UiE2EFailure("ADB_UNAVAILABLE")
        driver, events = self.driver([failure])
        with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^ADB_UNAVAILABLE$"):
            driver.set_text("driver_company_id", "SAFE", verify_plaintext=False)
        self.assertEqual(["READ_IME"], events)

    def test_state_read_failure_after_back_stops_before_focus_or_type(self) -> None:
        failure = HARNESS.UiE2EFailure("ADB_COMMAND_FAILED")
        driver, events = self.driver(["SHOWN", failure])
        with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^ADB_COMMAND_FAILED$"):
            driver.set_text("driver_company_id", "SAFE", verify_plaintext=False)
        self.assertEqual(["READ_IME", "BACK", "READ_IME"], events)

    def test_hidden_requires_exactly_one_visible_root(self) -> None:
        for root_count in (0, 2):
            with self.subTest(root_count=root_count):
                driver, events = self.driver(["SHOWN", "HIDDEN"], root_count=root_count)
                with self.assertRaisesRegex(
                    HARNESS.UiE2EFailure,
                    "^UI_IME_DISMISS_ROOT_INVALID$",
                ):
                    driver.set_text("driver_company_id", "SAFE", verify_plaintext=False)
                self.assertEqual(["READ_IME", "BACK", "READ_IME", "ROOT"], events)

    def test_root_dump_failure_remains_fail_closed(self) -> None:
        driver, events = self.driver(["SHOWN", "HIDDEN"], fail_root=True)
        with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^UI_HIERARCHY_INVALID$"):
            driver.set_text("driver_company_id", "SAFE", verify_plaintext=False)
        self.assertEqual(["READ_IME", "BACK", "READ_IME", "ROOT"], events)


if __name__ == "__main__":
    unittest.main(verbosity=2)
