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

namespace CloudBox.NetworkStatus
{
    public enum NetConnectionStatus
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2,
        Disconnecting = 3,
        HardwareNotPresent = 4,
        HardwareDisabled = 5,
        HardwareMalfunction = 6,
        MediaDisconnected = 7,
        Authenticating = 8,
        AuthenticationSucceeded = 9,
        AuthenticationFailed = 10,
        InvalidAddress = 11,
        CredentialsRequired = 12
    }

    public sealed class NetworkInfo
    {
        string m_DeviceName;
        public string DeviceName
        {
            get { return m_DeviceName; }
            set { m_DeviceName = value; }
        }
        string m_MacAddress;
        public string MacAddress
        {
            get { return m_MacAddress; }
            set { m_MacAddress = value; }
        }
        string m_AdapterType;
        public string AdapterType
        {
            get { return m_AdapterType; }
            set { m_AdapterType = value; }
        }
        string m_IP;
        public string IP
        {
            get { return m_IP; }
            set { m_IP = value; }
        }
        string m_Mask;
        public string Mask
        {
            get { return m_Mask; }
            set { m_Mask = value; }
        }
        string m_DefaultGateway;
        public string DefaultGateway
        {
            get { return m_DefaultGateway; }
            set { m_DefaultGateway = value; }
        }
        string m_ConnectionID;
        public string ConnectionID
        {
            get { return m_ConnectionID; }
            set { m_ConnectionID = value; }
        }
        NetConnectionStatus m_status;
        public NetConnectionStatus Status
        {
            get { return m_status; }
            set { m_status = value; }
        }

        public string GetHelp()
        {
            string t_msg = "Normal Connection.";
            if (m_status == NetConnectionStatus.Connected)
            {
                t_msg = "Connect succeed.";
            }
            else if (m_status == NetConnectionStatus.Disconnected)
            {
                t_msg = "Your connection was disable, please check Network Setting in Console.";
            }
            else if (m_status == NetConnectionStatus.MediaDisconnected)
            {
                t_msg = "Cable had bad contact with Network Card! Please check it.";
            }
            else if (m_status == NetConnectionStatus.InvalidAddress)
            {
                t_msg = "IP address is Invalid, please check DHCP/Router or IP setting.";
            }
            else
            {
                t_msg = string.Format("NetConnectionStatus is {0}", m_status.ToString());
            }

            return t_msg;
        }
    }
}
