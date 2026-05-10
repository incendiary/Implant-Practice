# 600 — Base DInvoke (C#)

Replaces `[DllImport]` with **dynamic API invocation**: resolves function addresses at runtime and invokes them via delegate, leaving no static import entries in the PE.

## What it does

Uses the `DInvoke` NuGet library (`TheWover/DInvoke`) to call `CreateProcessW` from `kernel32.dll` without a `[DllImport]` declaration:

1. Builds a parameter array matching the function signature
2. Calls `Generic.DynamicApiInvoke("kernel32.dll", "CreateProcessW", ...)` which:
   - Walks the PEB's loader data to find the module base
   - Parses the PE export table to find the function RVA
   - Calls the function via a typed delegate

## Why this matters

`[DllImport]` entries are visible in the .NET assembly metadata and the PE import table — trivially detectable by static analysis. DInvoke eliminates both.

## OPSEC notes

- The DInvoke library itself is a known indicator — consider reimplementing the resolution logic inline
- Function names (`"CreateProcessW"`) are still plaintext strings in the binary — see module 700 for hashing
- `Generic.DynamicApiInvoke` still resolves via `GetProcAddress` equivalent — see module 800 for ordinal-based resolution
