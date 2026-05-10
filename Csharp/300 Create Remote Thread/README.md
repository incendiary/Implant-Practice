# 300 — Create Remote Thread (C#)

Remote process injection in C#: spawn a process, allocate memory in it, write shellcode, and execute via `CreateRemoteThread`.

## Variants

### Create_Process
Demonstrates `CreateProcessW` via P/Invoke in isolation — confirms the process-spawning primitive works before combining it with injection.

### CreateRemoteThread
Full remote injection:
1. Downloads shellcode over HTTPS
2. Spawns `notepad.exe` suspended with `CreateProcessW`
3. Allocates `PAGE_EXECUTE_READWRITE` memory in the remote process with `VirtualAllocEx`
4. Writes shellcode with `WriteProcessMemory`
5. Executes with `CreateRemoteThread` then resumes

## OPSEC notes

- All three API calls (`VirtualAllocEx`, `WriteProcessMemory`, `CreateRemoteThread`) are monitored — Sysmon Event IDs 8 and 10
- P/Invoke declarations appear in the .NET assembly's import metadata
- DInvoke (modules 600–800) removes the static import signatures
- Edge has PPL-adjacent protections; `notepad.exe` is used as an unprotected target
