"""Explicit opt-in delivery: verified candidate -> isolated branch -> draft PR (never merge)."""
import argparse
import hashlib
import json
import pathlib
import re
import shutil
import subprocess
import tempfile
import uuid


def run(*args, cwd=None):
    return subprocess.run(args, cwd=cwd, text=True, check=True, capture_output=True, timeout=120).stdout.strip()


def verify(folder):
    folder = folder.resolve()
    report = json.loads((folder / "report.json").read_text(encoding="utf-8"))
    manifest = json.loads((folder / "manifest.json").read_text(encoding="utf-8"))
    required = {"report.json", "policy.json", "front.json", "build.log", "PR.md", "SPEC.md", "plan.md", "changes.diff", "candidate/app/Program.cs", "candidate/core/Discount.cs"}
    if not required.issubset(manifest):
        raise ValueError("Manifesto incompleto.")
    if report["status"] != "approved" or not report["buildPassed"] or not report["tests"] or not all(t["passed"] for t in report["tests"]):
        raise ValueError("Gate de build/testes não aprovado.")
    reviews = report.get("reviews", [])
    if not reviews or any(f["severity"] in ("critical", "high") for f in reviews[-1]["findings"]):
        raise ValueError("Revisão adversarial pendente.")
    if report["mode"] != "offline" and (not report.get("developerDigest") or not report.get("reviewerDigest") or report["developerDigest"] == report["reviewerDigest"]):
        raise ValueError("Revisão deve usar outro modelo.")
    for name, digest in manifest.items():
        parts = pathlib.PurePosixPath(name)
        if parts.is_absolute() or ".." in parts.parts or "\\" in name or ":" in name:
            raise ValueError("Caminho inválido no manifesto.")
        source = folder / name
        if source.is_symlink() or not source.resolve().is_relative_to(folder) or not source.is_file():
            raise ValueError("Arquivo ausente ou fora da entrega.")
        if hashlib.sha256(source.read_bytes()).hexdigest() != digest:
            raise ValueError("Artefato alterado após validação: " + name)
    return manifest


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--run", required=True, type=pathlib.Path)
    parser.add_argument("--repo", required=True, help="owner/name")
    parser.add_argument("--check-only", action="store_true", help="Validate evidence without publishing")
    args = parser.parse_args()
    if not re.fullmatch(r"[A-Za-z0-9_.-]+/[A-Za-z0-9_.-]+", args.repo):
        raise ValueError("Use owner/repo.")
    manifest = verify(args.run)
    if args.check_only:
        print("Evidências e hashes válidos; nada publicado.")
        return
    if not shutil.which("gh") or not shutil.which("git"):
        raise RuntimeError("Instale git e GitHub CLI (gh); execute gh auth login.")
    run("gh", "auth", "status")
    branch = "codex/lab09-delivery-" + uuid.uuid4().hex[:12]
    with tempfile.TemporaryDirectory(prefix="kiss-squad-") as temp:
        checkout = pathlib.Path(temp) / "repo"
        run("git", "clone", "--depth", "1", "https://github.com/" + args.repo + ".git", str(checkout))
        run("git", "checkout", "-b", branch, cwd=checkout)
        destination = checkout / "09-ai-squad" / "deliveries" / branch.rsplit("-", 1)[-1]
        for name in manifest:
            target = destination / name
            target.parent.mkdir(parents=True, exist_ok=True)
            shutil.copyfile(args.run / name, target)
        shutil.copyfile(args.run / "manifest.json", destination / "manifest.json")
        run("git", "add", "--", str(destination.relative_to(checkout)), cwd=checkout)
        run("git", "commit", "-m", "feat: add validated lab09 candidate for human review", cwd=checkout)
        run("git", "push", "origin", branch, cwd=checkout)
        print(run("gh", "pr", "create", "--repo", args.repo, "--head", branch, "--draft", "--title", "Lab09: candidate delivery for human review", "--body-file", str(destination / "PR.md"), cwd=checkout))


if __name__ == "__main__":
    try:
        main()
    except (ValueError, KeyError, OSError, RuntimeError, subprocess.SubprocessError) as error:
        raise SystemExit(str(error))
