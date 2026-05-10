using System;
using System.Collections;
using System.Runtime.InteropServices;
using DInvoke.DynamicInvoke;
using static Base_Dinvoke.Win32;

/* Example on Windows 10.0.19045 N/a Build 19045 */

namespace Base_Dinvoke
{
    internal class Program
    {
        static void Main()
        {
            // create startup info
            var startupInfo = new Win32.Startupinfo();
            startupInfo.cb = Marshal.SizeOf(startupInfo);


            // Define the PROCESS_INFORMATION structure
            ProcessInformation processInfo = new ProcessInformation();

            // create process parameter list/array

            object[] parametersList =
            {
                null,
                @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                IntPtr.Zero,
                IntPtr.Zero,
                false,
                (uint)0,
                IntPtr.Zero,
                @"C:\Program Files (x86)\Microsoft\Edge\Application\",
                startupInfo,
                processInfo
            };

            // Dynamic invoke
            var success = (bool)Generic.DynamicApiInvoke(
                "kernel32.dll",
                "CreateProcessW",
                typeof(Win32.CreateProcessDelegate),
                ref parametersList);

            // fail and bail
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