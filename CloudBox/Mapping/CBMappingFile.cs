using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CloudBox.Core.APIs;

namespace CloudBox.Mapping
{
    public class CBMappingFile
    {
        string m_FileName;
        IntPtr m_MapFile;
        IntPtr m_Buff;
        uint m_FileSize;

        const int ERROR_ALREADY_EXISTS = 0xB7;

        public CBMappingFile()
        {
            m_FileName = "";
            m_MapFile = IntPtr.Zero;
            m_Buff = IntPtr.Zero;
        }
        ~CBMappingFile()
        {
            CloseMapping();
        }

        public void OpenMapping(string fileName,uint size)
        {
            m_FileName = fileName;
            m_FileSize = size;
            if (m_MapFile == IntPtr.Zero)
            {
                // create map file
                m_MapFile = MappingFile.CreateFileMapping(WinAPIConst.INVALID_HANDLE_VALUE, null,
                    WinAPIConst.PAGE_READWRITE, 0, m_FileSize, m_FileName);
                int t_i4Error = Marshal.GetLastWin32Error();
                if (t_i4Error == ERROR_ALREADY_EXISTS)
                {
                    m_MapFile = MappingFile.OpenFileMapping(WinAPIConst.FILE_MAP_ALL_ACCESS, false, m_FileName);
                }

                if (m_MapFile == IntPtr.Zero)
                {
                    t_i4Error = Marshal.GetLastWin32Error();
                    throw new Exception(string.Format("Create or open a mapping file error, Error code[{0}]", t_i4Error));
                }

                // get map file
                m_Buff = MappingFile.MapViewOfFile(m_MapFile, WinAPIConst.FILE_MAP_ALL_ACCESS, 0, 0, m_FileSize);
                if (m_MapFile == IntPtr.Zero)
                {
                    t_i4Error = Marshal.GetLastWin32Error();
                    throw new Exception(string.Format("Mapping of file error, Error code[{0}]", t_i4Error));
                }
            }
        }

        public void Write(byte[] data)
        {
            Marshal.Copy(data, 0, m_Buff, data.Length);
        }

        public void Write(string data)
        {
            byte[] newData = ASCIIEncoding.ASCII.GetBytes(data);
            Write(newData);
        }

        public byte[] ReadBytes()
        {
            byte[] bytData = new byte[m_FileSize];
            // read data from map file
            Marshal.Copy(m_Buff, bytData, 0, (int)m_FileSize);
            return bytData;
        }

        public string ReadText()
        {
            byte[] newData = ReadBytes();
            return ASCIIEncoding.ASCII.GetString(newData);
        }

        public void CloseMapping()
        {
            if (m_Buff != IntPtr.Zero)
                MappingFile.UnmapViewOfFile(m_Buff);
            if (m_MapFile != IntPtr.Zero)
                MappingFile.CloseHandle(m_MapFile);
        }
    }
}
