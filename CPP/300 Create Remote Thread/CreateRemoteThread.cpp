#include <iostream>
#include <Windows.h>

#pragma comment(lib, "winhttp.lib")

std::vector<BYTE> Download(LPCWSTR baseAddress, LPCWSTR filename);

int main()
{
    // create startup info struct
    LPSTARTUPINFOW startup_info = new STARTUPINFOW();
    startup_info->cb = sizeof(STARTUPINFOW);
    startup_info->dwFlags = STARTF_USESHOWWINDOW;

    // create process info struct
    PPROCESS_INFORMATION process_info = new PROCESS_INFORMATION();

    // null terminated command line
    wchar_t cmd[] = L"C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe\0";

    wchar_t current_dir[] = L"C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\\0";

    // create process 
    // This is cheating, create a branch which finds a target.
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



std::vector<BYTE> Download(LPCWSTR baseAddress, LPCWSTR filename) {

    // initialise session
    HINTERNET hSession = WinHttpOpen(
        NULL,
        WINHTTP_ACCESS_TYPE_AUTOMATIC_PROXY,    // proxy aware
        WINHTTP_NO_PROXY_NAME,
        WINHTTP_NO_PROXY_BYPASS,
        WINHTTP_FLAG_SECURE_DEFAULTS);          // enable ssl

    // create session for target
    HINTERNET hConnect = WinHttpConnect(
        hSession,
        baseAddress,
        INTERNET_DEFAULT_HTTPS_PORT,            // port 443
        0);

    // create request handle
    HINTERNET hRequest = WinHttpOpenRequest(
        hConnect,
        L"GET",
        filename,
        NULL,
        WINHTTP_NO_REFERER,
        WINHTTP_DEFAULT_ACCEPT_TYPES,
        WINHTTP_FLAG_SECURE);                   // ssl

    // send the request
    WinHttpSendRequest(
        hRequest,
        WINHTTP_NO_ADDITIONAL_HEADERS,
        0,
        WINHTTP_NO_REQUEST_DATA,
        0,
        0,
        0);

    // receive response
    WinHttpReceiveResponse(
        hRequest,
        NULL);

    // read the data
    std::vector<BYTE> buffer;
    DWORD bytesRead = 0;

    do {

        BYTE temp[4096]{};
        WinHttpReadData(hRequest, temp, sizeof(temp), &bytesRead);

        if (bytesRead > 0) {
            buffer.insert(buffer.end(), temp, temp + bytesRead);
        }

    } while (bytesRead > 0);

    // close all the handles
    WinHttpCloseHandle(hRequest);
    WinHttpCloseHandle(hConnect);
    WinHttpCloseHandle(hSession);

    return buffer;
}