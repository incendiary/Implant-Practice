using DInvoke.DynamicInvoke;
using System;
using System.Runtime.InteropServices;


/* Example on Windows 10.0.19045 N/a Build 19045 */

namespace Dinvoke_Ordinal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // create startup info
            var startupInfo = new Win32.STARTUPINFO();
            startupInfo.cb = Marshal.SizeOf(startupInfo);

            //Get API pointer

            //var hLibrary = Generic.GetLibraryAddress("kernel32.dll", 254);

            // create params
            object[] parameters =
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
                new Win32.PROCESS_INFORMATION()
            };

            // get API Pointer
            // e9 in hex is 233 dec
            //OS Name:                   Microsoft Windows 11 Pro Insider Preview
            // OS Version:                10.0.25300 N / A Build 25300
            //ARM?
            // looking for CreateProcessW
            // its FD in Hex

            var hLibrary = Generic.GetLibraryAddress("kernel32.dll", 233);

            // Dynamic invoke Via Api
            // I don't think this updates the process info

            var success = (bool)Generic.DynamicFunctionInvoke(
                hLibrary,
                typeof(Win32.CreateProcessDelegate),
                ref parameters);

            // fail or bail

            if (!success)
            {
                Console.WriteLine($"[x] CreateProcessW failed: {Marshal.GetLastWin32Error()}");
                return;
            }

            // Get the process info from the dictionary after the API call
            // Donesnt seem like this works
            var processInfo = (Win32.PROCESS_INFORMATION)parameters[9];

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