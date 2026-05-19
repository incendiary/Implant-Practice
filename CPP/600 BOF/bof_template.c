/*
 * Beacon Object File (BOF) skeleton template.
 *
 * A BOF is a compiled COFF object (not a full PE) loaded directly into the
 * Beacon process at runtime.  The loader maps the sections, applies
 * relocations, and calls the entry function.  Because the object runs inside
 * the implant's memory space:
 *   - No new process or thread is created — very low OPSEC footprint.
 *   - The C runtime is NOT available; use the Beacon API or raw Win32.
 *   - All memory is managed by Beacon; do not call free() on BeaconDataExtract
 *     results.
 *
 * Build (cross-compile from Linux/macOS with MinGW):
 *   x86_64-w64-mingw32-gcc -o bof_template.o -c bof_template.c
 *
 * Build (MSVC — from a Developer Command Prompt):
 *   cl /c /GS- bof_template.c
 *
 * Load from a CNA script:
 *   beacon_inline_execute($bid, bof_read_file("bof_template.o"), "go",
 *       bof_pack($bid, "z", "world"));
 *
 * Operator output appears in the Cobalt Strike Beacon console.
 */

#include "beacon.h"

/* -------------------------------------------------------------------------
 * Declare the Win32 functions this BOF needs.
 * Do NOT call the standard prototypes — use the LIBRARY$Function convention
 * so the Beacon loader resolves them via GetProcAddress at runtime.
 * ------------------------------------------------------------------------- */

WINBASEAPI HANDLE WINAPI KERNEL32$GetCurrentProcess(void);
WINBASEAPI DWORD  WINAPI KERNEL32$GetCurrentProcessId(void);
WINBASEAPI DWORD  WINAPI KERNEL32$GetCurrentThreadId(void);
WINBASEAPI BOOL   WINAPI KERNEL32$CloseHandle(HANDLE hObject);

/* -------------------------------------------------------------------------
 * go() — BOF entry point.
 *
 * @param args   Pointer to the serialised argument blob produced by bof_pack().
 * @param len    Length of the blob in bytes.
 *
 * Beacon calls go() after mapping the object and resolving imports.
 * When go() returns, Beacon unmaps the sections and frees the allocation.
 * ------------------------------------------------------------------------- */

void go(char *args, int len)
{
    /* ------------------------------------------------------------------
     * 1. Parse arguments from the operator.
     *    The aggressor script should have called:
     *      bof_pack($bid, "z", $some_string);
     *    "z"  = null-terminated wide string (use BeaconDataExtract for raw bytes)
     *    "i"  = 32-bit int  (BeaconDataInt)
     *    "s"  = 16-bit short (BeaconDataShort)
     *    See the CS documentation for the full format string reference.
     * ------------------------------------------------------------------ */
    datap  parser;
    char  *name;

    BeaconDataParse(&parser, args, len);
    name = BeaconDataExtract(&parser, NULL);   /* NULL = we don't need the length */

    if (name == NULL || name[0] == '\0') {
        name = "world";   /* sensible default if no argument provided */
    }

    /* ------------------------------------------------------------------
     * 2. Do the actual work.
     *    Replace this section with your technique.
     * ------------------------------------------------------------------ */

    DWORD pid = KERNEL32$GetCurrentProcessId();
    DWORD tid = KERNEL32$GetCurrentThreadId();

    /* ------------------------------------------------------------------
     * 3. Return results to the operator console.
     *    CALLBACK_OUTPUT  — normal output
     *    CALLBACK_ERROR   — shown in red, signals failure
     * ------------------------------------------------------------------ */

    BeaconPrintf(CALLBACK_OUTPUT,
        "[bof_template] hello, %s!\n"
        "  running in PID %lu, TID %lu\n",
        name, (unsigned long)pid, (unsigned long)tid);

    /*
     * Nothing to clean up in this skeleton — if you allocated memory with
     * VirtualAlloc / HeapAlloc you must free it here before returning,
     * as the Beacon loader will only unmap the BOF's own code sections.
     */
}
