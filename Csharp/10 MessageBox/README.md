# 10 — MessageBox via P/Invoke (C#)

The simplest possible P/Invoke example: call a Win32 API (`MessageBoxW`) directly from managed C# without any C++ interop layer.

## What it does

Declares `MessageBoxW` from `user32.dll` using `[DllImport]` and calls it to display a popup.

## Why it matters

P/Invoke is the foundation of all Win32 interop in C#. Understanding the attribute parameters (`CharSet`, `SetLastError`, `CallingConvention`) is prerequisite knowledge for every module that follows.

## OPSEC notes

- `[DllImport]` declarations appear in the PE's import table and are trivially detectable
- DInvoke (modules 600–800) is the evasion-aware replacement
