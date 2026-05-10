# 800 — DInvoke by Ordinal (C#)

Resolves Win32 functions by **export ordinal number** rather than name or hash. No strings or hashes referencing function names exist in the binary.

## What it does

```csharp
var hLibrary = Generic.GetLibraryAddress("kernel32.dll", 233);
```

`GetLibraryAddress` with an integer second argument resolves the export at ordinal 233, which corresponds to `CreateProcessW` on the tested build.

## Ordinal mapping (version-specific)

Ordinals are **not stable across OS versions** — they change between Windows builds. The ordinal 233 was verified on:

> Windows 10.0.19045 (19H2 / 21H2 era)

Always verify the ordinal on your target OS before use:

```powershell
# PowerShell — list exports with ordinals
[System.Reflection.Assembly]::LoadWithPartialName('System') | Out-Null
$dll = [System.Runtime.InteropServices.Marshal]::LoadLibrary("kernel32.dll")
# or use dumpbin /exports C:\Windows\System32\kernel32.dll
```

## OPSEC notes

- No function name strings or hashes in the binary at all — strongest static evasion of the three DInvoke patterns
- Process info is not correctly returned via `out` parameters through `DynamicFunctionInvoke` in this implementation — known limitation noted in code comments
- Must be compiled as **64-bit** — ordinals differ between 32-bit and 64-bit kernel32
