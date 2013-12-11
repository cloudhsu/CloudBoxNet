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
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CloudBox.TcpObject
{
    /// <summary>
    /// TCP IP Client
    /// </summary>
    public sealed class TCPIPClient : TCPSocket
    {
        /// <summary>
        /// not use
        /// </summary>
        private TCPIPClient() { }

        /// <summary>
        /// MTCPIPClient construct
        /// </summary>
        /// <param name="a_i1ClientID">Client ID</param>
        public TCPIPClient(byte a_i1ClientID)
            : base(a_i1ClientID)
        {
        } // end of MTCPIPClient(string a_sClientID)

        /// <summary>
        /// MTCPIPClient Construct
        /// </summary>
        /// <param name="a_i1ClientID">Client ID</param>
        /// <param name="a_sTargetName">Target Name</param>
        /// <param name="a_sIP">Remote IP</param>
        /// <param name="a_i4Port">Remote Port</param>
        public TCPIPClient(byte a_i1ClientID,string a_sTargetName, string a_sIP, int a_i4Port)
            : base(a_i1ClientID,a_sTargetName,a_sIP,a_i4Port)
        {
        } // end of MTCPIPClient(byte a_i1ClientID,string a_sIP,int a_i4Port)

        /// <summary>
        /// MTCPIPClient Construct, using for server accept a new client
        /// </summary>
        /// <param name="a_pClient">Socket Client</param>
        /// <param name="a_i1ClientID">Client ID</param>
        public TCPIPClient(Socket a_pClient, byte a_i1ClientID)
            : base(a_pClient, a_i1ClientID)
        {
        } // end of MTCPIPClient(Socket a_pClient, string a_sClientID)

        /// <summary>
        /// Destructor
        /// </summary>
        ~TCPIPClient()
        {
            Destory();
        } // end of ~MTCPIPClient()

        /// <summary>
        /// Connect to server and set Client ID to server and start receive.
        /// </summary>
        public void Connect()
        {
            try
            {
                if (!IsConnected)
                {
                    // new TCP/IP Socket
                    m_pClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_pClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 409600);
                    // connect to server
                    m_pClient.Connect(new IPEndPoint(IPAddress.Parse(m_sIP), m_i4Port));
                    TraceLog(LogLevel.LOG_LEVEL_NORMAL, "Connect to " + m_sIP + ":" + m_i4Port + " succeed.");
                    // create ID Message
                    MessageContent t_pMsg = new MessageContent(MessageConst.TYPE_CLIENT_ID, ClientID, MessageConst.SERVER_ID, new byte[] { ClientID });
                    // send ID to server
                    m_pClient.Send(t_pMsg.GetBytes());
                    TraceLog(LogLevel.LOG_LEVEL_NORMAL, "Send Client ID:" + ClientID + " succeed");
                    m_dtLastHandshakeTime = DateTime.Now;
                    m_pMsgHandshakeList.Clear();
                    // Start Receive Data
                    StartReceive();
                } // end if(!m_bIsConnected)
            }
            catch (SocketException ex)
            {
                Debug.WriteLine(ex.NativeErrorCode + ":" + ex.Message + " In [Connect]");
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[ErrorCode]:{0},[SocketException]:{1} In [Connect]", ex.NativeErrorCode, ex.Message));
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " In [Connect]");
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [Connect]", ex.Message));
                throw;
            }
        } // end of Connect()

        /// <summary>
        /// Retry connect to server and set Client ID to server and start receive.
        /// </summary>
        void RetryConnect()
        {
            try
            {
                if (!IsConnected)
                {
                    // new TCP/IP Socket
                    m_pClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_pClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 409600);
                    // connect to server
                    m_pClient.Connect(new IPEndPoint(IPAddress.Parse(m_sIP), m_i4Port));
                    // create ID Message
                    MessageContent t_pMsg = new MessageContent(MessageConst.TYPE_CLIENT_ID, ClientID, MessageConst.SERVER_ID, new byte[] { ClientID });
                    // send ID to server
                    m_pClient.Send(t_pMsg.GetBytes());
                    m_dtLastHandshakeTime = DateTime.Now;
                    m_pMsgHandshakeList.Clear();
                    // Start Receive Data
                    StartReceive();
                } // end if(!m_bIsConnected)
            }
            catch (SocketException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        } // end of RetryConnect()

        /// <summary>
        /// Connect to server and set Client ID to server and start receive.
        /// If using no parameter construct, can call this function.
        /// </summary>
        /// <param name="a_sIP">Remote IP</param>
        /// <param name="a_i4Port">Remote Port</param>
        public void Connect(string a_sIP, int a_i4Port)
        {
            m_sIP = a_sIP;
            m_i4Port = a_i4Port;
            Connect();
        } // end of Connect(string a_sIP, int a_i4Port)

        /// <summary>
        /// Disconnect the connection.
        /// </summary>
        void Disconnect()
        {
            try
            {
                if (m_pClient != null)
                {
                    MessageContent t_pMsg = new MessageContent(MessageConst.TYPE_SHUTDOWN,
                        ClientID, MessageConst.SERVER_ID, ASCIIEncoding.ASCII.GetBytes("ByeBye"));
                    SendMessage(t_pMsg);
                    Thread.Sleep(100);
                    TraceLog(LogLevel.LOG_LEVEL_NORMAL, this.ToString() + " Disconnect Succeed.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " In [Disconnect]");
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [Disconnect]", ex.Message));
            }
        } // end of Disconnect()

        /// <summary>
        /// if auto handshake fail, reconnect(MsgType == TYPE_HANDSHAKE).
        /// if message type is other message type do normal handshake fail
        /// </summary>
        /// <param name="a_pHandshakeMsg"></param>
        protected override void DoHandshakeFail(MessageContent a_pHandshakeMsg)
        {
            if (a_pHandshakeMsg.MessageType == MessageConst.TYPE_HANDSHAKE)
            {
                TraceLog(LogLevel.LOG_LEVEL_WARRING, "Handshaking fail, it will shutdown socket.");
                Disconnect();
                Shutdown();
                TraceLog(LogLevel.LOG_LEVEL_WARRING, "Handshaking fail, shutdown socket succeed.");
                Thread.Sleep(500);
                RetryConnect();
                TraceLog(LogLevel.LOG_LEVEL_WARRING, "Handshaking fail, then retry connect succeed.");
            }
            else
            {
                base.DoHandshakeFail(a_pHandshakeMsg);
            }
        }

        /// <summary>
        /// Before shutdown, client need to send disconnect message to server.
        /// </summary>
        public override void Destory()
        {
            Disconnect();
            base.Destory();
        }

        /// <summary>
        /// do auto handshake for client(chamber/mainframe)
        /// </summary>
        protected override void AutoHandshake()
        {
            TimeSpan t_IdleTime = DateTime.Now.Subtract(m_dtLastHandshakeTime);
            if (t_IdleTime.TotalSeconds >= AUTO_HANDSHAKE_TIME && IsConnected)
            {
                MessageContent t_pMsg = new MessageContent(MessageConst.TYPE_HANDSHAKE,
                    ClientID, MessageConst.SERVER_ID, ASCIIEncoding.ASCII.GetBytes("Handshaking Check"));
                SendMessage(t_pMsg);
            }
            else if (t_IdleTime.TotalSeconds >= RETRY_CONNECT_TIME && !IsConnected)
            {
                m_dtLastHandshakeTime = DateTime.Now;
                RetryConnect();
            }
        }

    }
}
