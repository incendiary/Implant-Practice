# 700 — DInvoke API Hashing (C#)

Extends base DInvoke (module 600) by replacing plaintext function and module names with **pre-computed MD5 hashes**. Static analysis cannot determine which APIs are called.

## What it does

Instead of passing `"kernel32.dll"` and `"CreateProcessW"` as strings, passes their MD5 hashes to DInvoke's hash-aware resolution overloads:

```csharp
var hKernel = Generic.GetLoadedModuleAddress("56CC05AB6D069D47DF2539FE99937D46", 0xdeadbeef);
var hCreateProcess = Generic.GetExportAddress(hKernel, "B9825E679978ACD8B8C4873B1A5751C9", 0xdeadbeef);
```

DInvoke walks the export table and hashes each export name on the fly, returning the address when a match is found.

## Generating hashes

```csharp
// kernel32.dll → 56CC05AB6D069D47DF2539FE99937D46
// CreateProcessW → B9825E679978ACD8B8C4873B1A5751C9
using System.Security.Cryptography;
using System.Text;
BitConverter.ToString(MD5.HashData(Encoding.Unicode.GetBytes("kernel32.dll"))).Replace("-","");
```

## OPSEC notes

- The hash values themselves become a fingerprint if defenders know the algorithm — rotating the algorithm or salt removes this
- The `0xdeadbeef` seed in this example is the default DInvoke hash key — change it per engagement
- Hash resolution is slower than direct resolution; on a loaded system this is negligible
