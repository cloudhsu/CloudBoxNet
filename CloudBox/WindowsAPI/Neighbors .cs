using System;
using System.Runtime.InteropServices;

namespace CloudBox.Core.APIs
{
    public static class Neighbors
    {
        [DllImport("coredll.dll", EntryPoint = "WNetAddConnection3")]
        static extern int WNetAddConnection3CE(
            IntPtr hwndOwner,
            NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUserName,
            int dwFlags);

        [DllImport("coredll.dll", EntryPoint = "WNetCancelConnection2")]
        static extern int WNetCancelConnection2CE(
            string lpName,
            int dwFlags,
            int fForce);

        [DllImport("mpr.dll", EntryPoint = "WNetAddConnection3")]
        static extern int WNetAddConnection3Win(
            IntPtr hwndOwner,
            NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUserName,
            int dwFlags);

        [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2")]
        static extern int WNetCancelConnection2Win(
            string lpName,
            int dwFlags,
            int fForce);

        public static int WNetAddConnection3(
            IntPtr hwndOwner,
            NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUserName,
            int dwFlags)
        {
            int result = 0;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = WNetAddConnection3CE(hwndOwner,lpNetResource,lpPassword,lpUserName,dwFlags);
            }
            else
            {
                result = WNetAddConnection3Win(hwndOwner, lpNetResource, lpPassword, lpUserName, dwFlags);
            }
            return result;
        }

        public static int WNetCancelConnection2(
            string lpName,
            int dwFlags,
            int fForce)
        {
            int result = 0;
            if (System.Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                result = WNetCancelConnection2CE(lpName,dwFlags,fForce);
            }
            else
            {
                result = WNetCancelConnection2Win(lpName, dwFlags, fForce);
            }
            return result;
        }

        public static int MapDrive(IntPtr hwnd, string netRes, string shareName, string userName, string password)
        {
            NETRESOURCE NetRes = new NETRESOURCE();
            NetRes.dwScope = RESOURCE_GLOBALNET | RESOURCE_REMEMBERED;
            NetRes.dwType = RESOURCETYPE_DISK;
            NetRes.dwDisplayType = RESOURCEDISPLAYTYPE_SHARE;
            NetRes.dwUsage = RESOURCEUSAGE_CONNECTABLE;
            NetRes.lpRemoteName = Marshal.StringToBSTR(netRes);
            NetRes.lpLocalName = Marshal.StringToBSTR(shareName);
            NetRes.lpComment = IntPtr.Zero;
            NetRes.lpProvider = IntPtr.Zero;

            int ret = WNetAddConnection3(hwnd, NetRes, password, userName, 1);

            //if (ret != 0)
            //{
                //throw new System.ComponentModel.Win32Exception(ret, ((NetworkErrors)ret).ToString());
            //}
            return ret;

        }

        public static void Disconnect(string shareName, bool force)
        {
            if ((shareName != null) && (shareName != String.Empty))
            {
                int ret = WNetCancelConnection2(shareName, 1, (force) ? 1 : 0);

                if (ret != 0)
                {
                    throw new System.ComponentModel.Win32Exception(ret, ((NetworkErrors)ret).ToString());
                }
            }

        }

        public enum NetworkErrors
        {
            NoError = 0,
            AccessDenied = 5,
            InvalidHandle = 6,
            NotEnoughMemory = 8,
            NotSupported = 50,
            UnexpectedNetError = 59,
            InvalidPassword = 86,
            InvalidParameter = 87,
            InvalidLevel = 124,
            Busy = 170,
            MoreData = 234,
            InvalidAddress = 487,
            DeviceAlreadyRemembered = 1202,
            ExtentedError = 1208,
            Cancelled = 1223,
            Retry = 1237,
            BadUsername = 2202,
            NoNetwork = 1222

        }

        #region P/Invokes

        const int RESOURCE_CONNECTED = 0x00000001;
        const int RESOURCE_GLOBALNET = 0x00000002;
        const int RESOURCE_REMEMBERED = 0x00000003;

        const int RESOURCETYPE_ANY = 0x00000000;
        const int RESOURCETYPE_DISK = 0x00000001;
        const int RESOURCETYPE_PRINT = 0x00000002;

        const int RESOURCEDISPLAYTYPE_GENERIC = 0x00000000;
        const int RESOURCEDISPLAYTYPE_DOMAIN = 0x00000001;
        const int RESOURCEDISPLAYTYPE_SERVER = 0x00000002;
        const int RESOURCEDISPLAYTYPE_SHARE = 0x00000003;
        const int RESOURCEDISPLAYTYPE_FILE = 0x00000004;
        const int RESOURCEDISPLAYTYPE_GROUP = 0x00000005;

        const int RESOURCEUSAGE_CONNECTABLE = 0x00000001;
        const int RESOURCEUSAGE_CONTAINER = 0x00000002;

        #endregion
    }
}