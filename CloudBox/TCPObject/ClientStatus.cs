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

namespace CloudBox.TcpObject
{
    [Serializable]
    public class ClientStatus
    {
        int m_i4ClientID;
        string m_sClientName;
        bool m_bIsConnect;
        bool m_isEnable;

        public int ClientID
        {
            get { return m_i4ClientID; }
            set { m_i4ClientID = value; }
        }

        public string ClientName
        {
            get { return m_sClientName; }
            set { m_sClientName = value; }
        }
        public bool IsConnect
        {
            get { return m_bIsConnect; }
            set { m_bIsConnect = value; }
        }
        public bool Enable
        {
            get { return m_isEnable; }
            set { m_isEnable = value; }
        }

        private ClientStatus(){}

        public ClientStatus(int a_i4ClientID, string a_sClientName)
        {
            ClientID = a_i4ClientID;
            ClientName = a_sClientName;
            Enable = true;
        }

        public ClientStatus(int a_i4ClientID, string a_sClientName,bool a_bEnable)
        {
            ClientID = a_i4ClientID;
            ClientName = a_sClientName;
            Enable = a_bEnable;
        }
    }
}
