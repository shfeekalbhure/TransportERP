# Kimi Team Workspace — TransportERP

This directory is the repository-side landing zone for the Kimi engineering team.

## Hosted branch
`kimi/team-transport-20260829`

## Roles
| Agent | Responsibility |
|---|---|
| KIMI-00 | Coordinate tasks, enforce scope, collect handoff evidence |
| KIMI-01 | Explore repository and governing evidence; no implementation by default |
| KIMI-02 | Produce implementation plans and architecture checks |
| KIMI-03 | Implement approved code changes |
| KIMI-04 | Run restore/build/tests/migrations/CI checks as authorized |
| KIMI-05 | Independent review of KIMI-03 output |
| KIMI-06 | Governance, traceability, final task handoff |

## Standard task flow
`OWNER TASK -> KIMI-00 -> KIMI-01/02 -> KIMI-03 -> KIMI-04 -> KIMI-05 -> KIMI-06 -> OWNER REVIEW`

## No-merge rule
Kimi may prepare commits and pull requests on its workspace branch, but may not merge into `master` or governance branches unless the owner explicitly changes this rule.

## First local launch
After cloning the repository and installing/authenticating Kimi Code:

```powershell
git fetch origin
git switch kimi/team-transport-20260829
kimi
```

Kimi should read `/AGENTS.md` before accepting implementation tasks.

## Suggested first command to KIMI-00

> You are KIMI-00, coordinator for the hosted TransportERP Kimi team. Read AGENTS.md and this KIMI_TEAM/README.md first. Do not modify master, do not merge, and do not begin implementation until you have identified the exact task scope and governing evidence. Delegate repository discovery, planning, implementation, testing, independent review, and governance handoff to the defined roles as appropriate. Every delivery must include branch, commit SHA, changed files, tests, blockers, and PR information.
