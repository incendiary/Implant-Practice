/*
 * Beacon Object File (BOF) internal API header.
 *
 * This is a minimal extract of the public Cobalt Strike Beacon Development Kit
 * header.  The full version ships with a licensed CS installation and is also
 * widely reproduced in public BOF repositories (TrustedSec, fortra, etc.).
 *
 * Cobalt Strike and Beacon are trademarks of Fortra, LLC.
 * Include this header when compiling a BOF with MinGW or MSVC; it is NOT
 * linked at compile time — the symbols are resolved at runtime by the Beacon
 * loader inside the implant.
 */

#pragma once

#include <windows.h>

/* -------------------------------------------------------------------------
 * Data-parser API — unpack arguments passed from the aggressor/CNA script.
 * The blobs are serialised by CS using bof_pack() on the operator side.
 * ------------------------------------------------------------------------- */

typedef struct {
    char *original;   /* pointer to the start of the argument blob  */
    char *buffer;     /* current read position                       */
    int   length;     /* total length of the blob                    */
    int   size;       /* bytes remaining                             */
} datap;

DECLSPEC_IMPORT void   BeaconDataParse(datap *parser, char *buffer, int size);
DECLSPEC_IMPORT char  *BeaconDataExtract(datap *parser, int *size);
DECLSPEC_IMPORT int    BeaconDataInt(datap *parser);
DECLSPEC_IMPORT short  BeaconDataShort(datap *parser);

/* -------------------------------------------------------------------------
 * Output API — write results back to the Beacon operator console.
 * ------------------------------------------------------------------------- */

#define CALLBACK_OUTPUT      0x0
#define CALLBACK_OUTPUT_OEM  0x1e
#define CALLBACK_ERROR       0x0d
#define CALLBACK_OUTPUT_UTF8 0x20

DECLSPEC_IMPORT void BeaconOutput(int type, char *data, int len);
DECLSPEC_IMPORT void BeaconPrintf(int type, const char *fmt, ...);

/* -------------------------------------------------------------------------
 * Utility helpers
 * ------------------------------------------------------------------------- */

DECLSPEC_IMPORT BOOL  BeaconIsAdmin(void);

/* -------------------------------------------------------------------------
 * Win32 import pattern for BOFs.
 *
 * BOFs do not link against import libraries; instead, the Beacon loader
 * resolves symbols at load time using the __imp_ prefix convention.
 * Declare Win32 functions you need like this (do NOT use #include <windows.h>
 * function prototypes directly, as those may resolve via the import lib):
 *
 *   WINBASEAPI HANDLE WINAPI KERNEL32$OpenProcess(DWORD, BOOL, DWORD);
 *
 * Then call them as:
 *
 *   HANDLE h = KERNEL32$OpenProcess(PROCESS_ALL_ACCESS, FALSE, pid);
 *
 * The $ separator is a MinGW convention; MSVC uses the same symbol naming.
 * ------------------------------------------------------------------------- */
