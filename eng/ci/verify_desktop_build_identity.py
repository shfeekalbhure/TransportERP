#!/usr/bin/env python3
"""Independently verify the canonical Desktop deployment-tree digest emitted by the WinExe."""

from __future__ import annotations

import argparse
import hashlib
import hmac
import json
import re
import struct
from pathlib import Path


LOWER_SHA256 = re.compile(r"^[0-9a-f]{64}$")


def deployment_digest(root: Path) -> str:
    if not root.is_dir():
        raise SystemExit("Desktop deployment root is missing.")
    files = sorted(
        (path.relative_to(root).as_posix(), path)
        for path in root.rglob("*")
        if path.is_file()
    )
    if not files:
        raise SystemExit("Desktop deployment root is empty.")
    digest = hashlib.sha256()
    for relative, path in files:
        encoded_path = relative.encode("utf-8")
        digest.update(struct.pack(">i", len(encoded_path)))
        digest.update(encoded_path)
        digest.update(struct.pack(">q", path.stat().st_size))
        with path.open("rb") as stream:
            while chunk := stream.read(64 * 1024):
                digest.update(chunk)
    return digest.hexdigest()


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--root", required=True, type=Path)
    parser.add_argument("--identity", required=True, type=Path)
    arguments = parser.parse_args()

    identity = json.loads(arguments.identity.read_text(encoding="utf-8-sig"))
    if set(identity) != {"platform", "artifactSha256", "signerCertificateSha256"}:
        raise SystemExit("Desktop build identity schema is invalid.")
    claimed = identity["artifactSha256"]
    signer = identity["signerCertificateSha256"]
    if identity["platform"] != "desktop-windows" or not isinstance(claimed, str):
        raise SystemExit("Desktop build identity platform or artifact digest is invalid.")
    if not LOWER_SHA256.fullmatch(claimed):
        raise SystemExit("Desktop build identity artifact digest is invalid.")
    if signer is not None and (not isinstance(signer, str) or not LOWER_SHA256.fullmatch(signer)):
        raise SystemExit("Desktop build identity signer digest is invalid.")

    independently_measured = deployment_digest(arguments.root.resolve())
    if not hmac.compare_digest(independently_measured, claimed):
        raise SystemExit("Desktop WinExe build identity does not match the deployment tree.")
    print(independently_measured)


if __name__ == "__main__":
    main()
