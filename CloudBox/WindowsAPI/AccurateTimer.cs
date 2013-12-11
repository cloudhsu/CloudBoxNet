using System;
using System.Runtime.InteropServices;

namespace CloudBox.Core.APIs
{
    // ----------------- AccurateTimer ---------------------- //
    public class AccurateTimer
    {
        public static bool IsTimeBeginPeriod = false;

        const int PM_REMOVE = 0x0001;

        // ----- ce declare ----- //
        [DllImport("Coredll.dll", EntryPoint = "PeekMessage", SetLastError = true)]
        static extern bool PeekMessageCE(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
           uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport("Coredll.dll", EntryPoint = "TranslateMessage", SetLastError = true)]
        static extern bool TranslateMessageCE(ref MSG lpMsg);

        [DllImport("Coredll.dll", EntryPoint = "DispatchMessage", SetLastError = true)]
        static extern bool DispatchMessageCE(ref MSG lpMsg);

        [DllImport("Coredll.dll", EntryPoint = "QueryPerformanceCounter", SetLastError = true)]
        public static extern bool QueryPerformanceCounterCE(ref Int64 count);

        [DllImport("Coredll.dll", EntryPoint = "QueryPerformanceFrequency", SetLastError = true)]
        public static extern bool QueryPerformanceFrequencyCE(ref Int64 frequency);

        // ----- windows declare ----- //
        [DllImport("user32.dll", EntryPoint = "PeekMessage", SetLastError = true)]
        static extern bool PeekMessageWin(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
           uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport("user32.dll", EntryPoint = "TranslateMessage", SetLastError = true)]
        static extern bool TranslateMessageWin(ref MSG lpMsg);

        [DllImport("user32.dll", EntryPoint = "DispatchMessage", SetLastError = true)]
        static extern bool DispatchMessageWin(ref MSG lpMsg);


        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceCounter", SetLastError = true)]
        public static extern bool QueryPerformanceCounterWin(ref Int64 count);

        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceFrequency", SetLastError = true)]
        public static extern bool QueryPerformanceFrequencyWin(ref Int64 frequency);

        // ----- common declare ----- //
        static bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin,
           uint wMsgFilterMax, uint wRemoveMsg)
        {
            bool result;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = PeekMessageCE(out lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax, wRemoveMsg);
            }
            else
            {
                result = PeekMessageWin(out lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax, wRemoveMsg);
            }
            return result;
        }

        static bool TranslateMessage(ref MSG lpMsg)
        {
            bool result;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = TranslateMessageCE(ref lpMsg);
            }
            else
            {
                result = TranslateMessageWin(ref lpMsg);
            }
            return result;
        }

        static bool DispatchMessage(ref MSG lpMsg)
        {
            bool result;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = DispatchMessageCE(ref lpMsg);
            }
            else
            {
                result = DispatchMessageWin(ref lpMsg);
            }
            return result;
        }
        
        public static bool QueryPerformanceCounter(ref Int64 count)
        {
            bool result;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = QueryPerformanceCounterCE(ref count);
            }
            else
            {
                result = QueryPerformanceCounterWin(ref count);
            }
            return result;
        }

        public static bool QueryPerformanceFrequency(ref Int64 frequency)
        {
            bool result;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = QueryPerformanceFrequencyCE(ref frequency);
            }
            else
            {
                result = QueryPerformanceFrequencyWin(ref frequency);
            }
            return result;
        }

        public static void RetryQueryPerformanceCounter(ref Int64 count)
        {
            bool successed = QueryPerformanceCounter(ref count);
            int retry = 0;
            while (!successed)
            {
                successed = QueryPerformanceCounter(ref count);
                retry++;
                if (retry > 3)
                    break;
            }
        }

        public static void AccurateSleep(int a_i4MSec)
        {
            Int64 t_i8Frequency = 0;
            Int64 t_i8StartTime = 0;
            Int64 t_i8EndTime = 0;
            double t_r8PassedMSec = 0;
            MSG msg;
            AccurateTimer.QueryPerformanceCounter(ref t_i8StartTime);
            AccurateTimer.QueryPerformanceFrequency(ref t_i8Frequency);
            do
            {
                if (AccurateTimer.PeekMessage(out msg, IntPtr.Zero, 0, 0, PM_REMOVE))
                {
                    AccurateTimer.TranslateMessage(ref msg);
                    AccurateTimer.DispatchMessage(ref msg);
                }
                AccurateTimer.QueryPerformanceCounter(ref t_i8EndTime);
                t_r8PassedMSec = ((double)(t_i8EndTime - t_i8StartTime) / (double)t_i8Frequency) * 1000;
            } while (t_r8PassedMSec <= a_i4MSec);
        }
    }
}
