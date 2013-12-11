using System;
using System.Runtime.InteropServices;

namespace CloudBox.Core.APIs
{
    public static class MappingFile
    {
        // --- WinCE declare --- //
        [DllImport("Coredll.dll", EntryPoint = "CreateFileMapping", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileMappingCE(
            IntPtr hFile,
            object lpFileMappingAttributes,
            uint flProtect,
            uint dwMaximumSizeHigh,
            uint dwMaximumSizeLow,
            string lpName);

        [DllImport("Coredll.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        public static extern bool CloseHandleCE(IntPtr hObject);

        [DllImport("Coredll.dll", EntryPoint = "MapViewOfFile", SetLastError = true)]
        public static extern IntPtr MapViewOfFileCE(
            IntPtr hFileMappingObject,
            uint dwDesiredAccess,
            uint dwFileOffsetHigh,
            uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap);

        [DllImport("Coredll.dll", EntryPoint = "UnmapViewOfFile", SetLastError = true)]
        public static extern bool UnmapViewOfFileCE(IntPtr lpBaseAddress);

        // --- Windows XP declare --- //
        [DllImport("kernel32.dll", EntryPoint = "CreateFileMapping", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFileMappingWin(
            IntPtr hFile,
            object lpFileMappingAttributes,
            uint flProtect,
            uint dwMaximumSizeHigh,
            uint dwMaximumSizeLow,
            string lpName);

        // WinCE not support OpenFileMapping
        [DllImport("kernel32.dll", EntryPoint = "OpenFileMapping", SetLastError = true)]
        public static extern IntPtr OpenFileMappingWin(uint dwDesiredAccess, bool bInheritHandle, string lpName);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
        public static extern bool CloseHandleWin(IntPtr hObject);

        [DllImport("kernel32.dll", EntryPoint = "MapViewOfFile", SetLastError = true)]
        public static extern IntPtr MapViewOfFileWin(
            IntPtr hFileMappingObject,
            uint dwDesiredAccess,
            uint dwFileOffsetHigh,
            uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap);

        [DllImport("kernel32.dll", EntryPoint = "UnmapViewOfFile", SetLastError = true)]
        public static extern bool UnmapViewOfFileWin(IntPtr lpBaseAddress);

        // --- Common declare --- //
        public static IntPtr CreateFileMapping(
            IntPtr hFile,
            object lpFileMappingAttributes,
            uint flProtect,
            uint dwMaximumSizeHigh,
            uint dwMaximumSizeLow,
            string lpName)
        {
            IntPtr result = IntPtr.Zero;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = CreateFileMappingCE(hFile, lpFileMappingAttributes, flProtect, dwMaximumSizeHigh, dwMaximumSizeLow, lpName);
            }
            else
            {
                result = CreateFileMappingWin(hFile, lpFileMappingAttributes, flProtect, dwMaximumSizeHigh, dwMaximumSizeLow, lpName);
            }
            return result;
        }

        public static IntPtr OpenFileMapping(uint dwDesiredAccess, bool bInheritHandle, string lpName)
        {
            IntPtr t_pHandle = IntPtr.Zero;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                t_pHandle = CreateFileMappingCE(new IntPtr(-1), null,
                        WinAPIConst.PAGE_READWRITE, 0, 0, lpName);
            }
            else
            {
                t_pHandle = OpenFileMappingWin(dwDesiredAccess, bInheritHandle, lpName);
            }
            return t_pHandle;
        }

        public static bool CloseHandle(IntPtr hObject)
        {
            bool result = false;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = CloseHandleCE(hObject);
            }
            else
            {
                result = CloseHandleWin(hObject);
            }
            return result;
        }

        public static IntPtr MapViewOfFile(
            IntPtr hFileMappingObject,
            uint dwDesiredAccess,
            uint dwFileOffsetHigh,
            uint dwFileOffsetLow,
            uint dwNumberOfBytesToMap)
        {
            IntPtr result = IntPtr.Zero;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = MapViewOfFileCE(hFileMappingObject, dwDesiredAccess, dwFileOffsetHigh, dwFileOffsetLow, dwNumberOfBytesToMap);
            }
            else
            {
                result = MapViewOfFileWin(hFileMappingObject, dwDesiredAccess, dwFileOffsetHigh, dwFileOffsetLow, dwNumberOfBytesToMap);
            }
            return result;
        }

        public static bool UnmapViewOfFile(IntPtr lpBaseAddress)
        {
            bool result = false;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = UnmapViewOfFileCE(lpBaseAddress);
            }
            else
            {
                result = UnmapViewOfFileWin(lpBaseAddress);
            }
            return result;
        }
    }
}
