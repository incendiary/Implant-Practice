# 100 — Downloading Files (VBA)

Downloads a file over HTTP using the `MSXML2.XMLHTTP` COM object — no Win32 API declarations required.

## What it does

Creates an `MSXML2.XMLHTTP` object via `CreateObject`, sends a synchronous `GET` request, and stores the binary response in a `Variant` array via `responseBody`.

## OPSEC notes

- `MSXML2.XMLHTTP` is a legitimate Office/Windows COM object — less suspicious than `WinHTTP` declarations
- The download is synchronous (`False` third argument to `Open`) — the Office process will appear frozen until the download completes
- Replace `www.infinity-bank.com` with your listener address before use
- Consider `MSXML2.ServerXMLHTTP` for proxy-aware requests
