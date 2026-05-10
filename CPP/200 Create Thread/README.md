# 200 — Create Thread (C++)

Downloads shellcode and executes it in the **current process** using `CreateThread`.

## What it does

1. Downloads shellcode via WinHTTP (see module 100)
2. Calls `VirtualProtect` to mark the shellcode buffer `PAGE_EXECUTE_READWRITE`
3. Casts the buffer pointer to `LPTHREAD_START_ROUTINE` and passes it to `CreateThread`
4. Waits for a keypress to keep the process alive while the shellcode runs

A second variant (`FunctionDelegate.cpp`) uses a function-pointer cast instead of `CreateThread` — simpler but blocks the main thread.

## Files

| File | Purpose |
|------|---------|
| `CreateThread.cpp` | Thread-based execution with `_getch()` keepalive |
| `FunctionDelegate.cpp` | Direct function-pointer invocation |

## OPSEC notes

- `PAGE_EXECUTE_READWRITE` is flagged by most EDRs — prefer `PAGE_EXECUTE_READ` after writing
- Self-injection into the implant process is the noisiest injection class
- `CreateThread` creates a thread with no parent process context — suspicious on its own
