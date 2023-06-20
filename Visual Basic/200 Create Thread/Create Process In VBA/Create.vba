Declare PtrSafe Function CreateProcessW Lib "kernel32.dll" (ByVal lpApplicationName As String, ByVal lpCommandLine As String, ByVal lpProcessAttributes As LongPtr, ByVal lpThreadAttributes As LongPtr, ByVal bInheritHandles As Boolean, ByVal dwCreationFlags As Long, ByVal lpEnvironment As LongPtr, ByVal lpCurrentDirectory As String, lpStartupInfo As STARTUPINFO, lpProcessInformation As PROCESS_INFORMATION) As Boolean
Private Declare PtrSafe Function VirtualAlloc Lib "KERNEL32" (ByVal lpAddress As LongPtr, ByVal dwSize As Long, ByVal flAllocationType As Long, ByVal flProtect As Long) As LongPtr



Type STARTUPINFO
    cb As Long
    lpReserved As String
    lpDesktop As String
    lpTitle As String
    dwX As Long
    dwY As Long
    dwXSize As Long
    dwYSize As Long
    dwXCountChars As Long
    dwYCountChars As Long
    dwFillAttribute As Long
    dwFlags As Long
    wShowWindow As Integer
    cbReserved2 As Integer
    lpReserved2 As LongPtr
    hStdInput As LongPtr
    hStdOutput As LongPtr
    hStdError As LongPtr
End Type

Type PROCESS_INFORMATION
    hProcess As LongPtr
    hThread As LongPtr
    dwProcessId As Long
    dwThreadId As Long
End Type


Sub Document_Open() 
    TargetMacro
End Sub
Sub AutoOpen() 
    TargetMacro
End Sub

Sub TargetMacro()
    Dim startup_info As STARTUPINFO
    Dim process_info As PROCESS_INFORMATION
    
    Dim nullStr As String
    
    Dim success As Boolean
    success = CreateProcessW(nullStr, StrConv("C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe", vbUnicode), 0&, 0&, False, 0, 0&, nullStr, startup_info, process_info)
End Sub
