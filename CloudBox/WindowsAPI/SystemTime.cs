using System;
using System.Runtime.InteropServices;

namespace CloudBox.Core.APIs
{
    public static class SystemTime
    {
        [DllImport("Coredll.dll", EntryPoint = "SetSystemTime", CharSet = CharSet.Auto)]
        static extern bool SetSystemTimeCE(ref SYSTEMTIME lpSystemTime);

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime", CharSet = CharSet.Auto)]
        static extern bool SetSystemTimeWin(ref SYSTEMTIME lpSystemTime);


        public static bool SetSystemTime(ref SYSTEMTIME lpSystemTime)
        {
            bool result = false;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = SetSystemTimeCE(ref lpSystemTime);
            }
            else
            {
                result = SetSystemTimeWin(ref lpSystemTime);
            }
            return result;
        }
    }
}
