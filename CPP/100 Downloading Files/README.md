# 100 — Downloading Files (C++)

Uses the Windows **WinHTTP** API to fetch a payload over HTTPS from a remote server.

## What it does

1. Opens a WinHTTP session with automatic proxy detection and SSL defaults
2. Connects to the target host on port 443
3. Issues a `GET` request for the payload path
4. Streams the response body into a `std::vector<BYTE>` buffer
5. Returns the buffer for use by the caller (e.g. injection)

## Files

| File | Purpose |
|------|---------|
| `download.cpp` | Standalone download-only example (no execution) |

## Build

Open in Visual Studio on Windows and build as Release x64.

## OPSEC notes

- `WINHTTP_FLAG_SECURE_DEFAULTS` enforces TLS — server must have a valid cert
- The user-agent is `NULL` (no string) — production implants typically spoof a browser UA
- Replace `www.infinity-bank.com` with your listener address before use
- Consider adding `WinHttpSetOption` for custom headers to blend with legitimate traffic
