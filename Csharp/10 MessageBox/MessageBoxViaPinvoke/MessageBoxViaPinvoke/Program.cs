using System.Runtime.InteropServices;

[DllImport("user32.dll", CharSet = CharSet.Unicode)]
static extern int MessageBoxW(IntPtr hWnd, string lpText, string lpCaption, uint uType);

MessageBoxW(IntPtr.Zero, "Hello World", "MS Word", 0);

