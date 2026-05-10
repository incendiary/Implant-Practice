# 200 — Create Process (VBA)

Spawns a remote process from a VBA macro using `CreateProcessW` declared via `Declare PtrSafe Function`.

## What it does

Declares `CreateProcessW` and the required `STARTUPINFO` / `PROCESS_INFORMATION` types, then calls it to spawn Edge. `VirtualAlloc` is also declared but not used in this module — it is the setup for module 300.

## OPSEC notes

- Spawning processes from an Office macro is a very high-fidelity alert — most EDRs flag this immediately
- The parent process of the spawned process will be `WINWORD.EXE` or `EXCEL.EXE` — extremely unusual in enterprise environments
- `CREATE_SUSPENDED` (not used here but available via `dwCreationFlags`) is needed for APC injection patterns
