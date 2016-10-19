using System;
using System.Runtime.InteropServices;
using System.Text;

/*
using HANDLE = IntPtr、UIntPtr; // Visual C++ では void*
using BYTE = Byte; // Visual C++ では unsigned char
using SHORT = Int16; // Visual C++ では short
using WORD = UInt16; // Visual C++ では unsigned short
using INT = Int32; // Visual C++ では int
using UINT = UInt32; // Visual C++ では unsigned int
using LONG = Int32; // Visual C++ では long
using BOOL = Boolean; // Visual C++ では long
using DWORD = UInt32; // Visual C++ では unsigned long
using ULONG = UInt32; // Visual C++ では unsigned long
using CHAR = Char; // Visual C++ では char
using LPCSTR = String ^ [in], StringBuilder ^ [in, out]; // Visual C++ では char*
using LPCSTR = String; // Visual C++ では const char*
using LPWSTR = String ^ [in], StringBuilder ^ [in, out]; // Visual C++ では wchar_t*
using LPCWSTR = String; // Visual C++ では const wchar_t *
using FLOAT = Single; // Visual C++ では float
using DOUBLE = Double; // Visual C++ では double
*/

namespace Ouranos.RedArmory.Interop
{

    /// <summary>
    /// Windows API をカプセル化して提供します。
    /// </summary>
    internal static class SafeNativeMethods
    {

        public const int STD_INPUT_HANDLE = -10;

        public const int STD_OUTPUT_HANDLE = -11;

        public const int STD_ERROR_HANDLE = -12;

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("shell32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.U4)]
        public static extern int GetLongPathName(
            [MarshalAs(UnmanagedType.LPTStr)]
            string lpszShortPath,
            [MarshalAs(UnmanagedType.LPTStr)]
            StringBuilder lpszLongPath,
            [MarshalAs(UnmanagedType.U4)]
            int cchBuffer);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr hMem);


        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AttachConsole(uint dwProcessId);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeConsole();


        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);

    }

}
