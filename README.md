# Implant Practice

A structured collection of red-team implant development techniques implemented in C++, C#, and VBA. Each numbered module builds on the previous, progressing from basic Win32 API calls through to advanced dynamic invocation and OPSEC-aware injection methods.

This repository is intended as a learning reference and portfolio showcase of low-level Windows offensive security concepts.

> **Note:** This code was written for educational and authorised testing purposes only. All shellcode placeholders point to `www.infinity-bank.com`, a fictitious domain used during training. Do not use any technique here without written authorisation.

> **Assisted uplift notice:** Claude (Anthropic) was used to help refactor, document, and harden these projects for public release. The techniques and original code are the author's own work; Claude assisted with tooling configuration, README authorship, and pre-commit pipeline setup. Things should work, but in some cases verification on a live Windows target has not been re-run post-refactor. PRs and fixes are welcome.

---

## Techniques Covered

| # | Language | Technique |
|---|----------|-----------|
| 10 | C# / VBA | MessageBox via P/Invoke |
| 100 | C++ / C# / VBA | Downloading files over HTTPS (WinHTTP / HttpClient / XMLHTTP) |
| 200 | C++ / C# / VBA | Local shellcode execution via CreateThread and function delegates |
| 300 | C++ / C# / VBA | Remote process injection via CreateRemoteThread |
| 400 | C++ | Shellcode injection via NtMapViewOfSection (section-based, avoids VirtualAllocEx) |
| 500 | C++ | Early-bird APC injection via QueueUserAPC |
| 600 | C++ | Beacon Object File (BOF) skeleton — inline COFF execution inside Beacon |
| 600 | C# | Base DInvoke — dynamic API invocation to avoid static imports |
| 700 | C# | DInvoke with API hashing (avoids function name strings in binary) |
| 800 | C# | DInvoke by ordinal (avoids export name resolution entirely) |

---

## Setup

### Prerequisites

| Tool | Purpose |
|------|---------|
| Visual Studio 2022 (Windows) | Build C++ and C# projects |
| .NET 6+ SDK | Required for C# projects (`dotnet format`, builds) |
| clang-format | C++ auto-formatting |
| pre-commit | Git hook runner |
| gitleaks | Secret scanning |

### Building

Each subdirectory contains its own `.sln` file. Open in Visual Studio on Windows and build normally.

> C# projects require **64-bit** and **unsafe code** enabled:  
> `Project Properties → Build → Allow unsafe code` ✓ and uncheck `Prefer 32-bit`.

**DInvoke modules (600, 700, 800)** require `DInvoke.Data.dll` and `DInvoke.DynamicInvoke.dll` built from source:

```
git clone https://github.com/TheWover/DInvoke
# Build DInvoke.ManualMap in Release / netstandard2.0
# Copy DInvoke.Data.dll and DInvoke.DynamicInvoke.dll into each project's lib/ folder
```

### Installing the pre-commit hooks

```bash
pip install pre-commit
pre-commit install
pre-commit run --all-files   # first-time check
```

To auto-format C# before committing:
```bash
dotnet format <path/to/project.sln>
```

To auto-format C++:
```bash
clang-format -i --style=file <file.cpp>
```

---

## Roadmap

| Issue | Status | Description |
|-------|--------|-------------|
| #1 | ✅ Done | Add root `.gitignore` covering `.vs/`, `bin/`, `obj/` |
| #2 | ✅ Done | Remove tracked `.vs/` IDE cache files from git |
| #3 | ✅ Done | Add `.editorconfig` enforcing C# and C++ style conventions |
| #4 | ✅ Done | Add `.clang-format` (Microsoft style, 4-space indent) |
| #5 | ✅ Done | Add `.pre-commit-config.yaml` with gitleaks, clang-format, dotnet format |
| #6 | ✅ Done | Sanitise shellcode bytes in `Visual Basic/300` — replaced lab IP with zeroed placeholder |
| #7 | ✅ Done | Complete stub implementations: `Csharp/10 MessageBox` and `Csharp/300 CreateRemoteThread` |
| #8 | ✅ Done | Remove commented-out dead code blocks from CPP/300 and CPP/500 |
| #9 | ✅ Done | Add GitHub Actions CI: gitleaks scan + dotnet build on push/PR |
| #10 | ✅ Done | Add per-module README files explaining the technique and OPSEC notes |
| #11 | ✅ Done | Remove hardcoded OneDrive paths from DInvoke `.csproj` files (privacy) |
| #12 | ✅ Done | Enable branch protection on `main`: no force push, no deletion, gitleaks required |
| #13 | ✅ Done | Add C++ BOF skeleton (`CPP/600 BOF/`) — beacon.h header + commented template + build/load docs |

---

## Repository Structure

```
Implant-Practice/
├── CPP/
│   ├── 100 Downloading Files/      # WinHTTP HTTPS download
│   ├── 200 Create Thread/          # Local shellcode via CreateThread + function delegate
│   ├── 300 Create Remote Thread/   # CreateRemoteThread into spawned process
│   ├── 400 NTMapViewOfSection/     # Section-based injection via Nt* APIs
│   ├── 500 QueueUserAPC/           # Early-bird APC injection
│   └── 600 BOF/                   # Beacon Object File skeleton (beacon.h + bof_template.c)
├── Csharp/
│   ├── 10 MessageBox/              # P/Invoke MessageBoxW
│   ├── 100 Downloading Files/      # HttpClient download
│   ├── 200 Create Thread/          # VirtualProtect + Thread delegate / function pointer
│   ├── 300 Create Remote Thread/   # CreateProcessW + CreateRemoteThread
│   ├── 600 DInvoke/                # Base dynamic invocation
│   ├── 700 Dinvoke API Hashing/    # DInvoke with MD5-hashed function names
│   └── 800 Dinvoke Ordinal/        # DInvoke by export ordinal
└── Visual Basic/
    ├── 10 Basic Visual Basic/      # MessageBoxW via Declare
    ├── 100 Downloading Files/      # XMLHTTP download macro
    ├── 200 Create Thread/          # CreateProcessW macro
    └── 300 Create Remote Thread/   # VirtualAlloc + CreateThread shellcode runner
```
