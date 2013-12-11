using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace CloudBox.Core.APIs
{
    public class NetAdapter
    {
        [DllImport("iphlpapi.dll", CharSet = CharSet.Auto)]
        static extern int GetAdaptersInfo(IntPtr pAdapterInfo, ref int pBufOutLen);

        ArrayList m_pMACAddressArrayList;

        public NetAdapter()
        {
            m_pMACAddressArrayList = new ArrayList();
            FindMACAddress();
        }
        public String ConvertMACAddress(byte[] MACData)
        {
            Object[] t_pParamAry = new Object[6];
            t_pParamAry[0] = (MACData[0]);
            t_pParamAry[1] = (MACData[1]);
            t_pParamAry[2] = (MACData[2]);
            t_pParamAry[3] = (MACData[3]);
            t_pParamAry[4] = (MACData[4]);
            t_pParamAry[5] = (MACData[5]);

            return String.Format( "{0:X2}-{1:X2}-{2:X2}-{3:X2}-{4:X2}-{5:X2}",t_pParamAry );
        }
        public void FindMACAddress()
        {
            int structSize = Marshal.SizeOf(typeof(IP_ADAPTER_INFO));
            IntPtr pArray = Marshal.AllocHGlobal(structSize);

            int ret = GetAdaptersInfo(pArray, ref structSize);

            if (ret == WinAPIConst.ERROR_BUFFER_OVERFLOW) // ERROR_BUFFER_OVERFLOW == 111
            {
                // Buffer was too small, reallocate the correct size for the buffer.
                pArray = Marshal.ReAllocHGlobal(pArray, new IntPtr(structSize));

                ret = GetAdaptersInfo(pArray, ref structSize);
            } // if

            if (ret == 0)
            {
                // Call Succeeded
                IntPtr pEntry = pArray;

                do
                {
                    // Retrieve the adapter info from the memory address
                    IP_ADAPTER_INFO entry = (IP_ADAPTER_INFO)Marshal.PtrToStructure(pEntry, typeof(IP_ADAPTER_INFO));

                    // MAC Address (data is in a byte[])
                    string tmpString = string.Empty;
                    for (int i = 0; i < entry.Address.Length - 1; i++)
                    {
                        tmpString += string.Format("{0:X2}-", entry.Address[i]);
                    }
                    m_pMACAddressArrayList.Add(ConvertMACAddress(entry.Address));
                    // Get next adapter (if any)
                    pEntry = entry.Next;

                }
                while (pEntry != IntPtr.Zero);

                Marshal.FreeHGlobal(pArray);

            } // if
        }
        public ArrayList GetMACAddress() { return m_pMACAddressArrayList; }
    }
}
