# 100 — Downloading Files (C#)

Uses `System.Net.Http.HttpClient` to fetch a payload over HTTPS.

## What it does

Sends an async `GET` request and reads the response body into a `byte[]` for use by subsequent injection modules.

## OPSEC notes

- `HttpClient` uses `WinHTTP` under the hood on Windows — the same network stack as the C++ module 100
- The default user-agent includes the .NET runtime version string — spoof it with `client.DefaultRequestHeaders.UserAgent`
- Replace `www.infinity-bank.com` with your listener before use
