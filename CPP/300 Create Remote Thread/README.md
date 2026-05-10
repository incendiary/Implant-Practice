# 300 — Create Remote Thread (C++)

Injects shellcode into a **separate process** using the classic `VirtualAllocEx` → `WriteProcessMemory` → `CreateRemoteThread` pattern.

## What it does

1. Spawns a target process (`notepad.exe`) with `CREATE_NO_WINDOW`
2. Downloads shellcode via WinHTTP
3. Allocates `PAGE_EXECUTE_READWRITE` memory in the remote process with `VirtualAllocEx`
4. Copies the shellcode into remote memory with `WriteProcessMemory`
5. Creates a remote thread pointing at the shellcode with `CreateRemoteThread`

## Files

| File | Purpose |
|------|---------|
| `CreateRemoteThread.cpp` | Full injection into spawned notepad |
| `createThread.cpp` | Process creation only — no shellcode, confirms CreateProcess works |

## OPSEC notes

- `CreateRemoteThread` is one of the most-monitored Win32 calls — logged by Sysmon Event ID 8
- Spawning the target process yourself leaves a clear parent–child relationship in the process tree
- Edge has PPL-style protections; notepad is used here as an unprivileged target
- `VirtualAllocEx` with `PAGE_EXECUTE_READWRITE` will trigger memory-scanning EDRs
- Consider `NtMapViewOfSection` (module 400) to avoid both `VirtualAllocEx` and `WriteProcessMemory`
