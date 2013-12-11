using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CloudBox.Core.APIs
{
    /*
     * HANDLE = IntPtr
     * LPCTSTR = string
     * DWORD = UInt32
    */

    public class WinAPIConst
    {
        public const int ERROR_SUCCESS = 0;
        public const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;
        public const int ERROR_BUFFER_OVERFLOW = 111;
        public const int MAX_ADAPTER_NAME_LENGTH = 256;
        public const int MAX_ADAPTER_ADDRESS_LENGTH = 8;
        public const int MIB_IF_TYPE_OTHER = 1;
        public const int MIB_IF_TYPE_ETHERNET = 6;
        public const int MIB_IF_TYPE_TOKENRING = 9;
        public const int MIB_IF_TYPE_FDDI = 15;
        public const int MIB_IF_TYPE_PPP = 23;
        public const int MIB_IF_TYPE_LOOPBACK = 24;
        public const int MIB_IF_TYPE_SLIP = 28;
        public const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        public const UInt32 SECTION_QUERY = 0x0001;
        public const UInt32 SECTION_MAP_WRITE = 0x0002;
        public const UInt32 SECTION_MAP_READ = 0x0004;
        public const UInt32 SECTION_MAP_EXECUTE = 0x0008;
        public const UInt32 SECTION_EXTEND_SIZE = 0x0010;
        public const UInt32 SECTION_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SECTION_QUERY |
            SECTION_MAP_WRITE |
            SECTION_MAP_READ |
            SECTION_MAP_EXECUTE |
            SECTION_EXTEND_SIZE);
        public const UInt32 FILE_MAP_ALL_ACCESS = SECTION_ALL_ACCESS;

        public const int CREATE_NEW = 1;
        public const int CREATE_ALWAYS = 2;
        public const int OPEN_EXISTING = 3;
        public const int OPEN_ALWAYS = 4;
        public const int TRUNCATE_EXISTING = 5;

        public const UInt32 GENERIC_READ = 0x80000000;
        public const UInt32 GENERIC_WRITE = 0x40000000;
        public const UInt32 GENERIC_EXECUTE = 0x20000000;
        public const UInt32 GENERIC_ALL = 0x10000000;

        public const int FILE_SHARE_READ = 0x00000001;
        public const int FILE_SHARE_WRITE = 0x00000002;

        public const int FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        public const int FILE_ATTRIBUTE_NORMAL = 0x00000080;
        public const int FILE_FLAG_RANDOM_ACCESS = 0x10000000;
        public const int SEC_COMMIT = 0x8000000;

        public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        public const int PAGE_READWRITE = 0x04;
        public const int PAGE_READONLY = 0x02;
        public const int FILE_MAP_READ = 0x0004;
        public const int FILE_MAP_WRITE = 0x0002;
        //public const int FILE_MAP_ALL_ACCESS = 0x001f;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct IP_ADDRESS_STRING
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string Address;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SYSTEMTIME
    {
        public UInt16 Year;
        public UInt16 Month;
        public UInt16 DayOfWeek;
        public UInt16 Day;
        public UInt16 Hour;
        public UInt16 Minute;
        public UInt16 Second;
        public UInt16 Milliseconds;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct IP_ADDR_STRING
    {
        public IntPtr Next;
        public IP_ADDRESS_STRING IpAddress;
        public IP_ADDRESS_STRING IpMask;
        public Int32 Context;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES
    {
        public int nLength;
        public IntPtr lpSecurityDescriptor;
        public int bInheritHandle;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct IP_ADAPTER_INFO
    {
        public IntPtr Next;
        public Int32 ComboIndex;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WinAPIConst.MAX_ADAPTER_NAME_LENGTH + 4)]
        public string AdapterName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WinAPIConst.MAX_ADAPTER_DESCRIPTION_LENGTH + 4)]
        public string AdapterDescription;
        public UInt32 AddressLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = WinAPIConst.MAX_ADAPTER_ADDRESS_LENGTH)]
        public byte[] Address;
        public Int32 Index;
        public UInt32 Type;
        public UInt32 DhcpEnabled;
        public IntPtr CurrentIpAddress;
        public IP_ADDR_STRING IpAddressList;
        public IP_ADDR_STRING GatewayList;
        public IP_ADDR_STRING DhcpServer;
        public bool HaveWins;
        public IP_ADDR_STRING PrimaryWinsServer;
        public IP_ADDR_STRING SecondaryWinsServer;
        public Int32 LeaseObtained;
        public Int32 LeaseExpires;
    }

    [Flags]
    public enum FileMapAccess : uint
    {
        FileMapCopy = 0x0001,
        FileMapWrite = 0x0002,
        FileMapRead = 0x0004,
        FileMapAllAccess = 0x001f,
        fileMapExecute = 0x0020,
    }

    [Flags]
    enum FileMapProtection : uint
    {
        PAGE_READONLY = 0x02,
        PAGE_READWRITE = 0x04,
        PAGE_WRITECOPY = 0x08,       // WinCE not support
        PageExecuteRead = 0x20,
        PageExecuteReadWrite = 0x40,
        SEC_COMMIT = 0x8000000,
        SEC_IMAGE = 0x1000000,       // WinCE not support
        SEC_NOCACHE = 0x10000000,    // WinCE not support
        SEC_RESERVE = 0x4000000,     // WinCE not support
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr handle;
        public uint msg;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }

    // ----------------- EfficientAnalysis ---------------------- //
    public class EfficientAnalysis
    {
        static DateTime m_dtStart;
        static string m_sStartInfo;
        public static void Start(string a_sStartInfo)
        {
            m_dtStart = DateTime.Now;
            m_sStartInfo = "Start " + a_sStartInfo;
        }
        public static void End(string a_sEndInfo)
        {
            TimeSpan t_tsResult = DateTime.Now.Subtract(m_dtStart);
            Debug.WriteLine(m_sStartInfo);
            Debug.WriteLine("This process cost " + t_tsResult.TotalMilliseconds + " ms");
            Debug.WriteLine("End " + a_sEndInfo);
        }
    }

    public class NETRESOURCE
    {
        public int dwScope;
        public int dwType;
        public int dwDisplayType;
        public int dwUsage;
        public IntPtr lpLocalName;
        public IntPtr lpRemoteName;
        public IntPtr lpComment;
        public IntPtr lpProvider;
    }
}
