# 400 — NtMapViewOfSection (C++)

Injects shellcode using a **shared memory section** mapped into both the local and remote process. Avoids `VirtualAllocEx` and `WriteProcessMemory` entirely.

## What it does

1. Spawns Edge (`msedge.exe`) suspended with `CREATE_SUSPENDED`
2. Downloads shellcode
3. Resolves `NtCreateSection`, `NtMapViewOfSection`, `NtUnmapViewOfSection` directly from `ntdll.dll` via `GetProcAddress`
4. Creates a `SEC_COMMIT` / `PAGE_EXECUTE_READWRITE` section with `NtCreateSection`
5. Maps the section into **local** process memory, copies shellcode into it with `RtlCopyMemory`
6. Maps the **same section** into the remote process — shellcode is now present without any cross-process write
7. Redirects the remote thread's `RCX` register (first argument / entry point on x64) to the mapped address with `GetThreadContext` / `SetThreadContext`
8. Resumes the thread, then unmaps the local view

## Files

| File | Purpose |
|------|---------|
| `NtMapViewOfSection.cpp` | Full implementation |
| `Native.h` | Function pointer typedefs for `NtCreateSection`, `NtMapViewOfSection`, `NtUnmapViewOfSection` |

## Why this matters

`VirtualAllocEx` + `WriteProcessMemory` are heavily monitored. Section mapping achieves the same result:
- No cross-process memory write visible to EDR write callbacks
- No `CreateRemoteThread` call
- Shellcode is delivered via a kernel object (section handle) shared between processes

## OPSEC notes

- Requires `CONTEXT_INTEGER` flag to redirect `RCX` — misuse will crash the target thread
- `GetProcAddress` on `ntdll` functions is still detectable; syscall stubs avoid this (out of scope here)
- Must target a **64-bit** process; `RCX` is x64-specific
- Edge may have additional mitigations depending on version and OS configuration
