/*
* Copyright (c) 2011, Cloud Hsu
* All rights reserved.
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither the name of the Cloud Hsu nor the
*       names of its contributors may be used to endorse or promote products
*       derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY CLOUD HSU "AS IS" AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL THE REGENTS AND CONTRIBUTORS BE LIABLE FOR ANY
* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
#if !WindowsCE
using System.Management;

namespace CloudBox.NetworkStatus
{
    public sealed class NetworkManager
    {
        static readonly NetworkManager m_instance = new NetworkManager();
        static Dictionary<string,NetworkInfo> m_Informations = new Dictionary<string,NetworkInfo>();
        Thread _thread;
        bool m_IsAlive;

        public static NetworkManager Instance
        {
            get { return m_instance; }
        }
        public Dictionary<string, NetworkInfo> Informations
        {
            get { return m_Informations; }
        }
        private NetworkManager() { }
        static NetworkManager()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionID IS NOT NULL");
            foreach (ManagementObject mo in searcher.Get())
            {
                NetworkInfo info = new NetworkInfo();
                info.DeviceName = ParseProperty(mo["Description"]);
                info.AdapterType = ParseProperty(mo["AdapterType"]);
                info.MacAddress = ParseProperty(mo["MACAddress"]);
                info.ConnectionID = ParseProperty(mo["NetConnectionID"]);
                info.Status = (NetConnectionStatus)Convert.ToInt32(mo["NetConnectionStatus"]);
                SetIP(info);
                m_Informations.Add(info.ConnectionID, info);
            }
        }
        ~NetworkManager()
        {
            m_IsAlive = false;
        }

        static string ParseProperty(object data)
        {
            if (data != null)
                return data.ToString();
            return "";
        }


        public void StartMonitor()
        {
            m_IsAlive = true;
            _thread = new Thread(new ThreadStart(Monitor));
            _thread.Start();
        }

        public void Destory()
        {
            m_IsAlive = false;
            try
            {
                _thread.Abort();
                _thread = null;
            }
            catch{}
        }

        void Monitor()
        {
            while (m_IsAlive)
            {
                try
                {
                    Update();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("[Update]:" + ex.Message);
                }
                Thread.Sleep(100);
            }
        }

        void Update()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionID IS NOT NULL");
            foreach (ManagementObject mo in searcher.Get())
            {
                try
                {
                    if (m_Informations.ContainsKey(ParseProperty(mo["NetConnectionID"])))
                    {
                        NetConnectionStatus status = (NetConnectionStatus)Convert.ToInt32(mo["NetConnectionStatus"]);
                        NetworkInfo info = m_Informations[ParseProperty(mo["NetConnectionID"])];
                        info.DeviceName = ParseProperty(mo["Description"]);
                        info.AdapterType = ParseProperty(mo["AdapterType"]);
                        info.MacAddress = ParseProperty(mo["MACAddress"]);
                        info.ConnectionID = ParseProperty(mo["NetConnectionID"]);
                        info.Status = status;
                        if (info.Status != NetConnectionStatus.Connected)
                        {
                            info.IP = "0.0.0.0";
                            info.Mask = "0.0.0.0";
                            info.DefaultGateway = "0.0.0.0";
                        }
                        else
                        {
                            SetIP(info);
                        }
                        //m_Informations[ParseProperty(mo["NetConnectionID"])] = info;
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("[Update]:" + ex.Message);
                }
            }
        }
        static void SetIP(NetworkInfo info)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject mo in objMC.GetInstances())
            {
                try
                {
                    if (!(bool)mo["ipEnabled"])
                        continue;
                    if (mo["MACAddress"].ToString().Equals(info.MacAddress))
                    {
                        string[] ip = (string[])mo["IPAddress"];
                        info.IP = ip[0];
                        string[] mask = (string[])mo["IPSubnet"];
                        info.Mask = mask[0];
                        string[] gateway = (string[])mo["DefaultIPGateway"];
                        info.DefaultGateway = gateway[0];
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[SetIP]:" + ex.Message);
                }
            }
        }

        public void ConsoleWriteAllStatus()
        {
            foreach (NetworkInfo info in m_Informations.Values)
            {
                Console.WriteLine("=========================================");
                Console.WriteLine("Device Name:" + info.DeviceName);
                Console.WriteLine("Adapter Type:" + info.AdapterType);
                Console.WriteLine("MAC ID:" + info.MacAddress);
                Console.WriteLine("Connection Name:" + info.ConnectionID);
                Console.WriteLine("IP Address:" + info.IP);
                Console.WriteLine("Connection Status:" + info.Status.ToString());
                Console.WriteLine("=========================================");
            }
        }

        public static void ConsoleWriteStatus(NetworkInfo info)
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("Device Name:" + info.DeviceName);
            Console.WriteLine("Adapter Type:" + info.AdapterType);
            Console.WriteLine("MAC ID:" + info.MacAddress);
            Console.WriteLine("Connection Name:" + info.ConnectionID);
            Console.WriteLine("IP Address:" + info.IP);
            Console.WriteLine("Connection Status:" + info.Status.ToString());
            Console.WriteLine("=========================================");
        }

    }
}
#endif