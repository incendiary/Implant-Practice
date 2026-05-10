using System;
using System.Runtime.InteropServices;
using Create_Process;

namespace Create_Process
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // create startup info
            var startupInfo = new Win32.STARTUPINFO();
            startupInfo.cb = Marshal.SizeOf(startupInfo);

            // create process
            var success = Win32.CreateProcessW(
                null,
                @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                IntPtr.Zero,
                IntPtr.Zero,
                false,
                0,
                IntPtr.Zero,
                @"C:\Program Files (x86)\Microsoft\Edge\Application\",
                ref startupInfo,
                out var processInfo);

            // bail if it failed
            if (!success)
            {
                Console.WriteLine($"[x] CreateProcessW failed: {Marshal.GetLastWin32Error()}");
                return;
            }

            // print process info
            Console.WriteLine("dwProcessId : {0}", processInfo.dwProcessId);
            Console.WriteLine("dwThreadId  : {0}", processInfo.dwThreadId);
            Console.WriteLine("hProcess    : 0x{0:X}", processInfo.hProcess);
            Console.WriteLine("hThread     : 0x{0:X}", processInfo.hThread);

            // close handles
            Win32.CloseHandle(processInfo.hThread);
            Win32.CloseHandle(processInfo.hProcess);
        }
    }
}