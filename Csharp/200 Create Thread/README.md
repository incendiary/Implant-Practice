# 200 — Create Thread (C#)

Downloads shellcode and executes it in the current process. Two variants demonstrate different execution primitives.

## Variants

### CreateThread
Downloads shellcode, marks it `PAGE_EXECUTE_READWRITE` via `VirtualProtect`, then starts a new `Thread` using `Marshal.GetDelegateForFunctionPointer` to wrap the shellcode address as a typed delegate.

### Function Delegate
Same as above but invokes the delegate directly on the calling thread (blocks until shellcode returns).

## Why delegates over CreateThread

`Marshal.GetDelegateForFunctionPointer` lets the CLR call unmanaged code through a typed delegate without exposing a raw `CreateThread` import. The resulting IL still calls `CreateThread` internally — this is a convenience pattern, not an evasion one.

## OPSEC notes

- Requires `AllowUnsafeBlocks` and targeting x64 — see project Properties
- `PAGE_EXECUTE_READWRITE` on a managed heap allocation is a strong EDR signal
- Self-injection is the noisiest class — prefer remote injection (module 300) for real engagements
