Declare PtrSafe Function MessageBoxW Lib "user32.dll" (ByVal hWnd As LongPtr, ByVal lpText As String, ByVal lpCaption As String, ByVal uType As Integer) As Integer

Sub Document_Open() 
    TargetMacro
End Sub
Sub AutoOpen() 
    TargetMacro
End Sub

Sub TargetMacro()
    Dim result As Integer
    result = MessageBoxW(0, StrConv("Hello World", vbUnicode), StrConv("MS Word", vbUnicode), 0)
End Sub

