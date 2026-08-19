#!/usr/bin/env python3
"""Fail if anything under Assets/ is missing its .meta sibling, or a .meta is orphaned.

Reads the git index, which under pre-commit is exactly what is about to be committed.
"""

import subprocess
import sys

ROOT = "Assets/"


def ignored(path):
    # Unity skips dotfiles and backup files; they never get a .meta.
    name = path.rsplit("/", 1)[-1]
    return name.startswith(".") or name.endswith("~")


def check(tracked):
    """tracked: iterable of repo-relative paths in the index. Returns (missing, orphan)."""
    in_assets = {p for p in tracked if p.startswith(ROOT)}
    files = {p for p in in_assets if not ignored(p)}
    # Parents come from every path: a folder holding only dotfiles still needs its .meta.
    dirs = {d for p in in_assets for d in _parents(p)} - {ROOT.rstrip("/")}
    assets = (files | dirs) - {p for p in files if p.endswith(".meta")}

    missing = sorted(p for p in assets if p + ".meta" not in files)
    orphan = sorted(p for p in files if p.endswith(".meta") and p[:-5] not in assets)
    return missing, orphan


def _parents(path):
    parts = path.split("/")[:-1]
    return ["/".join(parts[: i + 1]) for i in range(len(parts))]


def demo():
    m, o = check(["Assets/A/B.cs", "Assets/A/B.cs.meta", "Assets/A.meta"])
    assert (m, o) == ([], []), (m, o)

    m, o = check(["Assets/A/B.cs", "Assets/A.meta"])
    assert m == ["Assets/A/B.cs"] and o == [], (m, o)  # missing file meta

    m, o = check(["Assets/A/B.cs", "Assets/A/B.cs.meta"])
    assert m == ["Assets/A"] and o == [], (m, o)  # missing directory meta

    m, o = check(["Assets/A/B.cs.meta", "Assets/A.meta"])
    assert m == [] and o == ["Assets/A/B.cs.meta"], (m, o)  # deleted file, meta left

    m, o = check(["Assets/A/.gitkeep", "Assets/A.meta", "README.md"])
    assert (m, o) == ([], []), (m, o)  # dotfiles and non-Assets paths ignored
    print("self-test ok")


def main():
    if "--self-test" in sys.argv:
        return demo()

    tracked = subprocess.run(
        ["git", "ls-files"], capture_output=True, text=True, check=True
    ).stdout.splitlines()
    missing, orphan = check(tracked)

    for p in missing:
        print(f"missing .meta: {p}")
    for p in orphan:
        print(f"orphaned .meta: {p} (counterpart not tracked)")
    if missing or orphan:
        print("\nUnity needs every asset and folder paired with its .meta file.")
        return 1


if __name__ == "__main__":
    sys.exit(main() or 0)
