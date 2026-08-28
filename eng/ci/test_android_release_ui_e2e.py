#!/usr/bin/env python3
"""Fail-closed unit tests for the ordinary Android Release UI harness."""

from __future__ import annotations

import hashlib
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


class ProcessConscryptTrustTests(unittest.TestCase):
    certificate_der = b"fixed-test-root-der"
    root_sha256 = hashlib.sha256(certificate_der).hexdigest()

    def driver(self) -> object:
        return HARNESS.Driver("adb", "pkg", 30, "abcdef12.0", self.root_sha256)

    def test_exact_root_is_bound_and_verified_for_each_process_launch(self) -> None:
        driver = self.driver()
        events: list[str] = []
        commands: list[tuple[str, ...]] = []

        def run(*arguments: str, **_: object) -> str:
            commands.append(arguments)
            if arguments[:3] == ("shell", "pidof", "pkg"):
                events.append("PID")
                return "321\n"
            if "mount" in arguments:
                events.append("BIND")
                return ""
            raise AssertionError(arguments)

        def run_binary(*_: str, **__: object) -> bytes:
            commands.append(_)
            events.append("READ")
            return HARNESS.ssl.DER_cert_to_PEM_cert(self.certificate_der).encode("ascii")

        driver.run = run
        driver.run_binary = run_binary
        driver.bind_conscrypt_trust_for_current_process()
        driver.bind_conscrypt_trust_for_current_process()
        self.assertEqual(["PID", "BIND", "READ", "PID", "BIND", "READ"], events)
        self.assertEqual(2, driver.conscrypt_process_bind_count)
        self.assertEqual(
            2,
            commands.count(
                (
                    "shell", "nsenter", "-t", "321", "-m", "--", "mount", "--bind",
                    "/system/etc/security/cacerts", "/apex/com.android.conscrypt/cacerts",
                )
            ),
        )
        self.assertEqual(
            2,
            commands.count(
                (
                    "exec-out", "nsenter", "-t", "321", "-m", "--", "cat",
                    "/apex/com.android.conscrypt/cacerts/abcdef12.0",
                )
            ),
        )

    def test_bind_failure_stops_before_certificate_read(self) -> None:
        driver = self.driver()
        driver.run = mock.Mock(
            side_effect=("321\n", HARNESS.UiE2EFailure("secret-bearing adb failure"))
        )
        driver.run_binary = mock.Mock(side_effect=AssertionError("must not read"))
        with self.assertRaisesRegex(
            HARNESS.UiE2EFailure,
            "^RELEASE_CONSCRYPT_BIND_FAILED$",
        ) as raised:
            driver.bind_conscrypt_trust_for_current_process()
        self.assertNotIn("321", str(raised.exception))
        self.assertNotIn("secret", str(raised.exception))
        driver.run_binary.assert_not_called()

    def test_unavailable_invalid_and_mismatched_roots_fail_with_fixed_codes(self) -> None:
        cases = (
            (
                HARNESS.UiE2EFailure("secret-bearing certificate read"),
                None,
                "RELEASE_CONSCRYPT_ROOT_UNAVAILABLE",
            ),
            (b"not-a-certificate", ValueError("secret-bearing parse"), "RELEASE_CONSCRYPT_ROOT_INVALID"),
            (b"other-certificate", b"different-der", "RELEASE_CONSCRYPT_ROOT_MISMATCH"),
        )
        for certificate, parsed, expected in cases:
            with self.subTest(expected=expected):
                driver = self.driver()
                driver.run = mock.Mock(side_effect=("321\n", ""))
                if isinstance(certificate, BaseException):
                    driver.run_binary = mock.Mock(side_effect=certificate)
                else:
                    driver.run_binary = mock.Mock(return_value=certificate)
                parse = (
                    mock.patch.object(HARNESS.ssl, "PEM_cert_to_DER_cert", side_effect=parsed)
                    if isinstance(parsed, BaseException)
                    else mock.patch.object(HARNESS.ssl, "PEM_cert_to_DER_cert", return_value=parsed)
                )
                with parse:
                    with self.assertRaisesRegex(HARNESS.UiE2EFailure, f"^{expected}$") as raised:
                        driver.bind_conscrypt_trust_for_current_process()
                self.assertNotIn("secret", str(raised.exception))
                self.assertEqual(0, driver.conscrypt_process_bind_count)

    def test_invalid_configuration_and_process_identity_fail_closed(self) -> None:
        invalid = HARNESS.Driver("adb", "pkg", 30, "unsafe-alias", "not-a-sha")
        invalid.run = mock.Mock(side_effect=AssertionError("must not call adb"))
        with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^RELEASE_CONSCRYPT_INPUT_INVALID$"):
            invalid.bind_conscrypt_trust_for_current_process()
        invalid.run.assert_not_called()

        driver = self.driver()
        driver.run = mock.Mock(return_value="321 654\n")
        with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^RELEASE_PROCESS_PID_INVALID$"):
            driver.bind_conscrypt_trust_for_current_process()

        unavailable = self.driver()
        unavailable.run = mock.Mock(
            side_effect=HARNESS.UiE2EFailure("secret-bearing pid lookup")
        )
        with self.assertRaisesRegex(
            HARNESS.UiE2EFailure,
            "^RELEASE_PROCESS_PID_UNAVAILABLE$",
        ) as raised:
            unavailable.bind_conscrypt_trust_for_current_process()
        self.assertNotIn("secret", str(raised.exception))

    def test_launcher_binds_before_any_ui_observation_on_initial_and_restart_launches(self) -> None:
        driver = self.driver()
        events: list[str] = []

        def run(*arguments: str, **_: object) -> str:
            if arguments[:3] == ("shell", "dumpsys", "package"):
                return "pkgFlags=[ HAS_CODE ]\n"
            if "resolve-activity" in arguments:
                return "pkg/MainActivity\n"
            if arguments[:3] == ("shell", "am", "start"):
                events.append("START")
            return ""

        def bind() -> None:
            events.append("BIND")
            driver.conscrypt_process_bind_count += 1

        driver.run = run
        driver.bind_conscrypt_trust_for_current_process = bind
        driver.wait_for = lambda _: events.append("UI")
        driver.launch_ordinary_activity()
        driver.launch_ordinary_activity()
        self.assertEqual(["START", "BIND", "UI", "START", "BIND", "UI"], events)
        self.assertEqual(2, driver.conscrypt_process_bind_count)


class SafeActionResultTests(unittest.TestCase):
    @staticmethod
    def node(text: str = "", enabled: str = "true") -> object:
        return mock.Mock(attrib={"text": text, "enabled": enabled})

    def test_expected_result_returns_without_exposing_values(self) -> None:
        driver = HARNESS.Driver("adb", "pkg", 30)
        driver.find = lambda _: self.node("Result: OFFLINE_ACTIVATED")
        driver.wait_result_code("driver_action_result", "OFFLINE_ACTIVATED")

    def test_unexpected_safe_result_fails_immediately_with_only_the_code(self) -> None:
        driver = HARNESS.Driver("adb", "pkg", 30)
        driver.find = lambda _: self.node("Result: AUTHENTICATION_FAILED")
        with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^UI_RESULT_AUTHENTICATION_FAILED$"):
            driver.wait_result_code("driver_action_result", "OFFLINE_ACTIVATED")

    def test_allowlisted_transport_results_remain_fixed_and_message_free(self) -> None:
        for code in (
            "AUTH_SESSION_CONNECTION_FAILED",
            "AUTH_SESSION_HTTP_PROTOCOL_FAILED",
            "AUTH_SESSION_NAME_RESOLUTION_FAILED",
            "AUTH_SESSION_TIMEOUT",
            "AUTH_SESSION_TLS_FAILED",
            "AUTH_SESSION_TRANSPORT_FAILED",
        ):
            with self.subTest(code=code):
                driver = HARNESS.Driver("adb", "pkg", 30)
                driver.find = lambda _, result=code: self.node(f"Result: {result}")
                with self.assertRaisesRegex(HARNESS.UiE2EFailure, f"^UI_RESULT_{code}$") as raised:
                    driver.wait_result_code("driver_action_result", "OFFLINE_ACTIVATED")
                self.assertEqual(f"UI_RESULT_{code}", str(raised.exception))

    def test_invalid_expected_result_is_rejected_before_ui_access(self) -> None:
        driver = HARNESS.Driver("adb", "pkg", 30)
        driver.find = mock.Mock(side_effect=AssertionError("UI must not be accessed"))
        for expected in ("unsafe-result", "UNKNOWN_BUT_SAFE_SHAPED"):
            with self.subTest(expected=expected):
                with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^EXPECTED_RESULT_CODE_INVALID$"):
                    driver.wait_result_code("driver_action_result", expected)
        driver.find.assert_not_called()

    def test_unknown_safe_shaped_result_is_not_emitted(self) -> None:
        driver = HARNESS.Driver("adb", "pkg", 30)
        driver.find = lambda _: self.node("Result: SECRET_SHAPED_IDENTIFIER")
        with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^UI_RESULT_OTHER$") as raised:
            driver.wait_result_code("driver_action_result", "OFFLINE_ACTIVATED")
        self.assertNotIn("SECRET", str(raised.exception))

    def test_prompt_and_unrecognized_text_wait_for_timeout_without_emission(self) -> None:
        for value in (HARNESS.INITIAL_ACTION_PROMPT, "secret-bearing unexpected text"):
            with self.subTest(value_type="prompt" if value == HARNESS.INITIAL_ACTION_PROMPT else "other"):
                driver = HARNESS.Driver("adb", "pkg", 30)
                driver.find = mock.Mock(return_value=self.node(value))
                with mock.patch.object(HARNESS.time, "monotonic", side_effect=(0, 1, 31)):
                    with mock.patch.object(HARNESS.time, "sleep", return_value=None):
                        with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^UI_WAIT_TIMEOUT$") as raised:
                            driver.wait_result_code("driver_action_result", "OFFLINE_ACTIVATED")
                self.assertNotIn("secret", str(raised.exception))

    def test_missing_element_retries_but_non_scroll_failure_stops(self) -> None:
        driver = HARNESS.Driver("adb", "pkg", 30)
        driver.find = mock.Mock(
            side_effect=HARNESS.UiE2EFailure("UI_AUTOMATION_ID_NOT_FOUND:SCROLL_UNCHANGED")
        )
        with mock.patch.object(HARNESS.time, "monotonic", side_effect=(0, 1, 31)):
            with mock.patch.object(HARNESS.time, "sleep", return_value=None):
                with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^UI_WAIT_TIMEOUT$"):
                    driver.wait_result_code("driver_action_result", "OFFLINE_ACTIVATED")
        driver.find = mock.Mock(side_effect=HARNESS.UiE2EFailure("UI_HIERARCHY_INVALID"))
        with self.assertRaisesRegex(HARNESS.UiE2EFailure, "^UI_HIERARCHY_INVALID$"):
            driver.wait_result_code("driver_action_result", "OFFLINE_ACTIVATED")

    def test_observation_classifies_only_allowlisted_states(self) -> None:
        driver = mock.Mock()
        nodes = {
            "driver_action_result": self.node(HARNESS.INITIAL_ACTION_PROMPT),
            "driver_mode": self.node("Offline runtime: CLOSED"),
            "driver_sign_in": self.node(enabled="true"),
        }
        with mock.patch.object(
            HARNESS,
            "safe_visible_element",
            side_effect=lambda _, automation_id: ("ONE", nodes[automation_id]),
        ):
            actual = HARNESS.safe_sign_in_observation(driver, "ACTION_RESULT")
        self.assertEqual(
            "RESULT_COUNT_ONE:RESULT_INITIAL_PROMPT:MODE_COUNT_ONE:MODE_CLOSED:"
            "SIGN_IN_COUNT_ONE:SIGN_IN_ENABLED_TRUE",
            actual,
        )

    def test_observation_never_emits_unrecognized_text(self) -> None:
        driver = mock.Mock()
        nodes = {
            "driver_action_result": self.node("secret-bearing unexpected text"),
            "driver_mode": self.node("unexpected mode text"),
            "driver_sign_in": self.node(enabled="unknown"),
        }
        with mock.patch.object(
            HARNESS,
            "safe_visible_element",
            side_effect=lambda _, automation_id: ("ONE", nodes[automation_id]),
        ):
            actual = HARNESS.safe_sign_in_observation(driver, "ACTION_RESULT")
        self.assertEqual(
            "RESULT_COUNT_ONE:RESULT_OTHER:MODE_COUNT_ONE:MODE_OTHER:"
            "SIGN_IN_COUNT_ONE:SIGN_IN_ENABLED_UNKNOWN",
            actual,
        )
        self.assertNotIn("secret", actual)

    def test_observation_does_not_emit_unknown_safe_shaped_result(self) -> None:
        driver = mock.Mock()
        nodes = {
            "driver_action_result": self.node("Result: SECRET_SHAPED_IDENTIFIER"),
            "driver_mode": self.node("Offline runtime: CLOSED"),
            "driver_sign_in": self.node(enabled="true"),
        }
        with mock.patch.object(
            HARNESS,
            "safe_visible_element",
            side_effect=lambda _, automation_id: ("ONE", nodes[automation_id]),
        ):
            actual = HARNESS.safe_sign_in_observation(driver, "ACTION_RESULT")
        self.assertIn("RESULT_OTHER", actual)
        self.assertNotIn("SECRET", actual)

    def test_element_count_failures_remain_fixed_and_value_free(self) -> None:
        driver = mock.Mock()
        states = iter((("ZERO", None), ("MULTIPLE", None), ("UNKNOWN", None)))
        with mock.patch.object(HARNESS, "safe_visible_element", side_effect=lambda *_: next(states)):
            actual = HARNESS.safe_sign_in_observation(driver, "MODE_READY")
        self.assertEqual(
            "RESULT_COUNT_ZERO:RESULT_UNKNOWN:MODE_COUNT_MULTIPLE:MODE_UNKNOWN:"
            "SIGN_IN_COUNT_UNKNOWN:SIGN_IN_ENABLED_UNKNOWN",
            actual,
        )


if __name__ == "__main__":
    unittest.main(verbosity=2)
