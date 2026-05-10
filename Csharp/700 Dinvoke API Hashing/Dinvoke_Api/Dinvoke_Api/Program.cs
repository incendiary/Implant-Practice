using System;
using System.Collections;
using System.Runtime.InteropServices;
using DInvoke.DynamicInvoke;
using System.Linq;
using DinvokeApi;

/* Example on Windows 10.0.19045 N/a Build 19045 */

namespace DinvokeApi
{
    internal class Program
    {
        static void Main()
        {
            // create startup info
            var startupInfo = new Win32.Startupinfo();
            startupInfo.cb = Marshal.SizeOf(startupInfo);


            // Define the PROCESS_INFORMATION structure
            Win32.ProcessInformation processInfo = new Win32.ProcessInformation();

            // create process parameter list/array two ways of doing,

            object[] parametersListA =
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


            List<Tuple<string, dynamic>> parametersListB = new List<Tuple<string, dynamic>>()
            {
                new Tuple<string, dynamic>("lpApplicationName", null),
                new Tuple<string, dynamic>("lpCommandLine", @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"),
                new Tuple<string, dynamic>("lpProcessAttributes", IntPtr.Zero),
                new Tuple<string, dynamic>("lpThreadAttributes", IntPtr.Zero),
                new Tuple<string, dynamic>("bInheritHandles", false),
                new Tuple<string, dynamic>("dwCreationFlags", (uint)0),
                new Tuple<string, dynamic>("lpEnvironment", IntPtr.Zero),
                new Tuple<string, dynamic>("lpCurrentDirectory", @"C:\Program Files (x86)\Microsoft\Edge\Application\"),
                new Tuple<string, dynamic>("lpStartupInfo", startupInfo),
                new Tuple<string, dynamic>("lpProcessInformation", processInfo)
            };

            object[] parametersArray = parametersListB.Select(tuple => tuple.Item2).ToArray();

            // get kernel32
            var hKernel = Generic.GetLoadedModuleAddress("56CC05AB6D069D47DF2539FE99937D46", 0xdeadbeef);
            // get createprocessw
            var hCreateProcess = Generic.GetExportAddress(
                hKernel,
                "B9825E679978ACD8B8C4873B1A5751C9",
                0xdeadbeef);

            // Dynamic invoke Via Api

            var success = (bool)Generic.DynamicFunctionInvoke(
                hCreateProcess,
                typeof(Win32.CreateProcessDelegate),
                ref parametersArray);

            // fail and bail

            if (!success)
            {
                Console.WriteLine($"[x] CreateProcessW failed: {Marshal.GetLastWin32Error()}");
                return;
            }

            // print process info
            /* Doesnt work
            Console.WriteLine("dwProcessId : {0}", processInfo.dwProcessId);
            Console.WriteLine("dwThreadId  : {0}", processInfo.dwThreadId);
            Console.WriteLine("hProcess    : 0x{0:X}", processInfo.hProcess);
            Console.WriteLine("hThread     : 0x{0:X}", processInfo.hThread);

            Console.ReadLine();

            */

            // close handles
            Win32.CloseHandle(processInfo.hThread);
            Win32.CloseHandle(processInfo.hProcess);

            
        }
    }
}