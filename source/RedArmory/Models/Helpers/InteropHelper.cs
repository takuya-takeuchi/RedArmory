using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using RedArmory.Interop;

namespace RedArmory.Models.Helpers
{

    internal static class InteropHelper
    {

        public static string GetLongPathName(string shortPath)
        {
            if (string.IsNullOrWhiteSpace(shortPath))
            {
                return shortPath;
            }
			
            var builder = new StringBuilder(255);
            var result = SafeNativeMethods.GetLongPathName(shortPath, builder, builder.Capacity);
            if (result > 0 && result < builder.Capacity)
            {
                return builder.ToString(0, result);
            }

            if (result > 0)
            {
                builder = new StringBuilder(result);
                result = SafeNativeMethods.GetLongPathName(shortPath, builder, builder.Capacity);
                return builder.ToString(0, result);
            }

            return null;
        }

        public static string[] SplitArgs(string unsplitArgumentLine)
        {
            int numberOfArgs;
            var ptrToSplitArgs = SafeNativeMethods.CommandLineToArgvW(unsplitArgumentLine, out numberOfArgs);

            if (ptrToSplitArgs == IntPtr.Zero)
            {
                throw new ArgumentException("文字列の解析に失敗しました。", new Win32Exception());
            }

            try
            {
                var splitArgs = new string[numberOfArgs];

                for (var index = 0; index < numberOfArgs; index++)
                {
                    splitArgs[index] = Marshal.PtrToStringUni(Marshal.ReadIntPtr(ptrToSplitArgs, index * IntPtr.Size));
                }

                return splitArgs;
            }
            finally
            {
                SafeNativeMethods.LocalFree(ptrToSplitArgs);
            }
        }

    }

}
