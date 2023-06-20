#include <iostream>
#include <Windows.h>

int main()
{
    // create startup info struct
    LPSTARTUPINFOW startup_info = new STARTUPINFOW();
    startup_info->cb = sizeof(STARTUPINFOW);

    // create process info struct
    PPROCESS_INFORMATION process_info = new PROCESS_INFORMATION();

    // null terminated command line
    wchar_t cmd[] = L"C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe\0";

    wchar_t current_dir[] = L"C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\\0";

    // create process
    BOOL success = CreateProcess(
        NULL,
        cmd,
        NULL,
        NULL,
        FALSE,
        0,
        NULL,
        current_dir,
        startup_info,
        process_info);

    // bail if call failed
    if (!success) {
        printf("[x] CreateProcess failed: %d\n", GetLastError());
        return 1;
    }

    // print process information
    printf("dwProcessId : %d\n", process_info->dwProcessId);
    printf("dwThreadId  : %d\n", process_info->dwThreadId);
    printf("hProcess    : %p\n", process_info->hProcess);
    printf("hThread     : %p\n", process_info->hThread);

    // close the handles
    CloseHandle(process_info->hThread);
    CloseHandle(process_info->hProcess);
}