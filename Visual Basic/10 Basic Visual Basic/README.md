# 10 — MessageBox via VBA Declare (VBA)

The simplest VBA Win32 interop example: call `MessageBoxW` from `user32.dll` using a `Declare PtrSafe Function` statement.

## What it does

Declares `MessageBoxW` in the VBA module header, then calls it from a macro that triggers on `Document_Open` or `AutoOpen`.

## Why it matters

`Declare PtrSafe Function` is VBA's equivalent of P/Invoke. Understanding it is prerequisite for all subsequent VBA modules. The `PtrSafe` keyword is required for 64-bit Office.

## OPSEC notes

- `AutoOpen` and `Document_Open` are the two standard macro auto-execution triggers — both are monitored by email gateways and AV
- `StrConv(str, vbUnicode)` is needed because VBA strings are internally Unicode but `Declare` functions expect wide-char pointers
