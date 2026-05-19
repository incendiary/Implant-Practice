# 600 — Beacon Object File (BOF) Skeleton

## What is a BOF?

A Beacon Object File is a compiled COFF object (produced by `gcc -c` or `cl /c`) that Cobalt Strike's Beacon loader maps directly into the implant's memory at runtime. Unlike a full DLL or shellcode:

- **No new process or thread is created** — the code runs on a Beacon task thread, giving a very low OPSEC footprint.
- **The C runtime is unavailable** — no `malloc`, `printf`, `strlen`, etc. Use the Beacon API and raw Win32.
- **Execution lifetime is short** — when `go()` returns, Beacon unmaps the sections and frees the allocation.

The technique was popularised by TrustedSec's `COFFLoader` and is now standard in red team operations.

## Files

| File | Purpose |
|------|---------|
| `beacon.h` | Minimal extract of the public CS Beacon Development Kit header — data-parser API, output API, OPSEC helpers |
| `bof_template.c` | Entry-point skeleton with argument parsing, Win32 calls via `LIBRARY$Function` convention, and result output |

## Build

### Cross-compile (Linux / macOS with MinGW-w64)

```bash
x86_64-w64-mingw32-gcc -o bof_template.o -c bof_template.c
```

### Native (Windows — MSVC Developer Command Prompt)

```cmd
cl /c /GS- bof_template.c
```

`/GS-` disables stack-protection cookies, which require CRT initialisation that is unavailable inside a BOF.

## Load from a CNA script

```javascript
# Read the compiled object
$bof_data = bof_read_file(script_resource("bof_template.o"));

# Pack arguments: "z" = null-terminated C string
$args = bof_pack($bid, "z", "world");

# Execute inline inside the Beacon
beacon_inline_execute($bid, $bof_data, "go", $args);
```

## Win32 import convention

BOFs do not link against import libraries. Declare functions with the `LIBRARY$Function` convention — the Beacon loader resolves them via `GetProcAddress` at runtime:

```c
/* Declaration — in your source file or a helper header */
WINBASEAPI HANDLE WINAPI KERNEL32$OpenProcess(DWORD, BOOL, DWORD);

/* Usage */
HANDLE h = KERNEL32$OpenProcess(PROCESS_ALL_ACCESS, FALSE, pid);
KERNEL32$CloseHandle(h);
```

Common library prefixes: `KERNEL32`, `NTDLL`, `ADVAPI32`, `USER32`, `WS2_32`.

## OPSEC notes

- Keep `go()` short; long-running BOFs block Beacon's task queue.
- Free any heap allocations (`VirtualAlloc`, `HeapAlloc`) before returning — Beacon's cleanup only covers its own mapped sections.
- Avoid using CRT functions (`strlen`, `sprintf`, etc.); implement minimal helpers inline or use `msvcrt.dll` via the `MSVCRT$` convention if absolutely necessary.
- Strip debug symbols from the final object with `strip --strip-debug` (MinGW) to reduce size and remove path artefacts.

## Further reading

- Cobalt Strike BOF documentation: https://www.cobaltstrike.com/blog/beacon-object-files-luser-driven-attacks/
- TrustedSec COFFLoader: https://github.com/trustedsec/COFFLoader
- fortra BOF collection: https://github.com/fortra/nanodump (examples of real-world BOFs)
