# 500 — QueueUserAPC (C++)

Injects shellcode using the **APC (Asynchronous Procedure Call)** queue of a suspended thread. This is the "early-bird" injection pattern.

## What it does

1. Spawns Edge (`msedge.exe`) with `CREATE_SUSPENDED | CREATE_NO_WINDOW` — the main thread starts suspended
2. Downloads shellcode
3. Allocates `PAGE_EXECUTE_READWRITE` memory in the remote process with `VirtualAllocEx`
4. Writes shellcode with `WriteProcessMemory`
5. Queues the shellcode address onto the main thread's APC queue with `QueueUserAPC`
6. Resumes the thread with `ResumeThread` — the APC fires before any user code runs (early-bird)

## Files

| File | Purpose |
|------|---------|
| `QueueUserAPC.cpp` | Full early-bird APC injection |

## Why this matters

When a thread enters an **alertable wait** state (or resumes from `CREATE_SUSPENDED` before user code runs), Windows drains its APC queue. By queuing shellcode before resuming, execution happens inside the legitimate process before it has initialised its own defences or hooks.

## OPSEC notes

- `QueueUserAPC` is visible to Sysmon and most EDRs
- `VirtualAllocEx` + `WriteProcessMemory` are still used here — see module 400 for avoiding those
- The suspended-then-resumed pattern is a strong signal for sandboxes; consider using an existing alertable thread rather than spawning your own process
- Edge has additional protections; in practice use an unprotected target for testing
