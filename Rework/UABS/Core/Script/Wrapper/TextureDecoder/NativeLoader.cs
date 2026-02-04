using System;
using System.Runtime.InteropServices;

namespace UABS.Wrapper
{
    internal static class NativeLoader
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        public static void Load(string path)
        {
            IntPtr handle = LoadLibrary(path);
            if (handle == IntPtr.Zero)
                throw new DllNotFoundException($"Failed to load DLL from {path}");
        }
    }
}