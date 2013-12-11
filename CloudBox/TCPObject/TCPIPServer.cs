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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/*
 *  Design rule:
 *  1. Server only handle one ID of Client.
 *  2. If Client ID duplicate server will release old Client, and handle new client.
 */

namespace CloudBox.TcpObject
{
    /// <summary>
    /// TCP server
    /// </summary>
    public sealed class TCPIPServer
    {
        string    m_sIP;
        int       m_i4Port;
        Socket    m_pServer;
        List<TCPSocket> m_pClientList = new List<TCPSocket>();
        Thread    m_pAcceptThread;
        bool      m_bIsRunning;

        /// <summary>
        /// event for server log
        /// </summary>
        public event TCPIPClient.TraceLogHandler EventTraceLog;
        /// <summary>
        /// event for server receive data from all client.
        /// </summary>
        public event TCPIPClient.MessageContentHandler EventAllClientReceive;
        /// <summary>
        /// event for handshake fail from all client.
        /// </summary>
        public event TCPIPClient.HandshakeHandler EventAllClientHandshakeFail;

        /// <summary>
        /// get client count in server m_pClientList.
        /// </summary>
        public int ClientListCount
        {
            get { return m_pClientList.Count; }
        }

        public List<TCPSocket> Clients
        {
            get { return m_pClientList; }
        }

        /// <summary>
        /// Trace Log
        /// </summary>
        /// <param name="a_i4Level">Log level</param>
        /// <param name="a_sLog">Log content</param>
        private void TraceLog(int a_i4Level,string a_sLog)
        {
            if (EventTraceLog != null)
            {
                try
                {
                    EventTraceLog(a_i4Level, "[Server]:" + a_sLog);
                }catch{}
            }
        }

        /// <summary>
        /// MTCPIPServer construct.
        /// </summary>
        /// <param name="a_sIP">Server IP</param>
        /// <param name="a_i4Port">Server Port</param>
        public TCPIPServer(string a_sIP, int a_i4Port)
        {
            m_sIP = a_sIP;
            m_i4Port = a_i4Port;
        } // end of MTCPIPServer(string a_sIP, int a_i4Port)

        /// <summary>
        /// Destructor
        /// </summary>
        ~TCPIPServer()
        {
            ShutdownServer();
        } // end of ~MTCPIPServer()

        /// <summary>
        /// Check Client is connected
        /// </summary>
        /// <param name="a_i4ClientID">Client ID</param>
        /// <returns>true is connected</returns>
        public bool CheckClientConnected(int a_i4ClientID)
        {
            if (m_pClientList.Count == 0)
                return false;
            for (int t_i4ClientIndex = 0; t_i4ClientIndex < m_pClientList.Count; t_i4ClientIndex++)
            {
                TCPSocket t_pTcpClient = m_pClientList[t_i4ClientIndex];
                if (t_pTcpClient.ClientID == (byte)a_i4ClientID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Start server
        /// </summary>
        public void StartServer()
        {
            try
            {
                m_pServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // connect to server
                m_pServer.Bind(new IPEndPoint(IPAddress.Parse(m_sIP), m_i4Port));
                m_pServer.Listen(100);
                TraceLog(LogLevel.LOG_LEVEL_NORMAL, "Bind to" + m_sIP + ":" + m_i4Port + " success.");
                if (m_pAcceptThread != null)
                {
                    m_pAcceptThread.Abort();
                    m_pAcceptThread = null;
                }
                m_pAcceptThread = new Thread(new ThreadStart(this.AcceptClient));
                m_pAcceptThread.Name = "ServerAccept";
                m_bIsRunning = true;
                m_pAcceptThread.Start();
                TraceLog(LogLevel.LOG_LEVEL_NORMAL, "Start Accept.");
            }
            catch (SocketException ex)
            {
                Debug.WriteLine(ex.NativeErrorCode + ":" + ex.Message);
                TraceLog(LogLevel.LOG_LEVEL_WARRING, String.Format("[ErrorCode]:{0},[SocketException]:{1} In [StartServer]", ex.NativeErrorCode, ex.Message));
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                TraceLog(LogLevel.LOG_LEVEL_WARRING, String.Format("[Exception]:{0} In [StartServer]", ex.Message));
                throw;
            }
        } // end of  StartServer()

        /// <summary>
        /// Shutdown server.
        /// </summary>
        public void ShutdownServer()
        {
            try
            {
                // Shutdown server
                if (m_pServer != null)
                {
                    try
                    {
                        m_pServer.Close();
                        m_pServer = null;
                    }
                    catch (Exception) { }
                }
                for (int i = m_pClientList.Count-1; i >=0 ; i--)
                {
                    TCPSocket t_pShutDownTcpClient = m_pClientList[i];
                    try
                    {
                        t_pShutDownTcpClient.Destory();
                    }
                    catch (Exception) { }
                }
            }
            catch (SocketException ex)
            {
                Debug.WriteLine(ex.NativeErrorCode + ":" + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            try
            {
                m_bIsRunning = false;
                if (m_pAcceptThread != null)
                {
                    m_pAcceptThread.Abort();
                    m_pAcceptThread = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            TraceLog(LogLevel.LOG_LEVEL_TRACE, "Shutdown success");
        } // end of ShutdownServer()

        /// <summary>
        /// Accept client with Thread.
        /// </summary>
        void AcceptClient()
        {
            while (m_bIsRunning)
            {
                Socket t_pNewClient = null;
                try
                {
                    t_pNewClient = m_pServer.Accept();
                    t_pNewClient.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 409600);
                    byte[] t_bData = new byte[1024];
                    int t_i4Length = t_pNewClient.Receive(t_bData);
                    if (t_i4Length > 0)
                    {
                        MessageContent t_pMsg = TCPSocket.AnalyzeClientID(t_bData, t_i4Length);
                        byte t_i1ClientID = t_pMsg.Content[0];
                        TCPSocket t_pNewTcpClient = new TCPSocket(t_pNewClient, t_i1ClientID);
                        TraceLog(LogLevel.LOG_LEVEL_NORMAL, t_pNewTcpClient.ToString() + " create connection succeed.");
                        t_pNewTcpClient.EventDataReceive += new TCPSocket.MessageContentHandler(TcpClient_EventDataReceive);
                        t_pNewTcpClient.EventHandshakeFail += new TCPSocket.HandshakeHandler(TcpClient_EventHandshakeFail);
                        t_pNewTcpClient.EventClientShutdown += new TCPSocket.ClientShutdownHandler(TcpClient_EventClientShutdown);
                        t_pNewTcpClient.EventTraceLog += new TCPSocket.TraceLogHandler(TraceLog);
                        t_pNewTcpClient.EventIsExistInServer += new TCPSocket.IsExistInServerHandler(TcpClient_EventIsExistInServer);
                        t_pNewTcpClient.StartReceive();
                        AddNewClient(t_pNewTcpClient);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                Thread.Sleep(1);
            }
        }

        bool TcpClient_EventIsExistInServer(TCPSocket a_pClient)
        {
            foreach(TCPSocket t_pExistClient in m_pClientList)
            {
                if (a_pClient.ClientID == t_pExistClient.ClientID)
                    return true;
            }
            TraceLog(LogLevel.LOG_LEVEL_WARRING, a_pClient.ToString() + " dose not exist in server. Please restart client.");
            return false;
        } // end of AcceptClient()

        /// <summary>
        /// Add new client for list when accept a client.
        /// If client already exist,to shutdown the old exist client and to handle the new client.
        /// </summary>
        /// <param name="t_pNewClient">New Client</param>
        void AddNewClient(TCPSocket t_pNewClient)
        {
            for (int t_i4ClientIndex = 0; t_i4ClientIndex < m_pClientList.Count; t_i4ClientIndex++)
            {
                TCPSocket t_pExistTcpClient = m_pClientList[t_i4ClientIndex];
                if (t_pExistTcpClient.ClientID.Equals(t_pNewClient.ClientID))
                {
                    m_pClientList.Remove(t_pExistTcpClient);
                    t_pExistTcpClient.Destory();
                    TraceLog(LogLevel.LOG_LEVEL_NORMAL, t_pExistTcpClient.ToString() + " already Exist and will remove it.");
                    t_pExistTcpClient = null;
                    break;
                }
            }
            m_pClientList.Add(t_pNewClient);
            TraceLog(LogLevel.LOG_LEVEL_NORMAL, t_pNewClient.ToString() + " Added to server to manage.");
        }

        /// <summary>
        /// delegate function, using for MTCPIPClient.EventClientShutdown
        /// </summary>
        /// <param name="a_i1ClientID">Shutdown client Id</param>
        void TcpClient_EventClientShutdown(byte a_i1ClientID)
        {
            TCPSocket t_pShutDownTcpClient = null;
            try
            {
                TraceLog(LogLevel.LOG_LEVEL_NORMAL, "Client[" + a_i1ClientID + "] will shutdown in server.");
                for (int t_i4ClientIndex = 0; t_i4ClientIndex < m_pClientList.Count; t_i4ClientIndex++)
                {
                    t_pShutDownTcpClient = m_pClientList[t_i4ClientIndex];
                    if (t_pShutDownTcpClient.ClientID == a_i1ClientID)
                    {
                        m_pClientList.Remove(t_pShutDownTcpClient);
                        t_pShutDownTcpClient.Destory();
                        TraceLog(LogLevel.LOG_LEVEL_NORMAL, t_pShutDownTcpClient.ToString() + " disconnected");
                        t_pShutDownTcpClient = null;
                        break;
                    }
                }
            }
            catch (SocketException ex)
            {
                Debug.WriteLine(ex.NativeErrorCode + ":" + ex.Message);
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[ErrorCode]:{0},[SocketException]:{1} In [TcpClient_EventClientShutdown]", ex.NativeErrorCode, ex.Message));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [TcpClient_EventClientShutdown]", ex.Message));
            }
        } // end of TcpClient_EventClientShutdown()

        /// <summary>
        /// delegate function, using for MTCPIPClient.EventHandshakeFail
        /// </summary>
        /// <param name="a_pClient">MTCPSocket.</param>
        /// <param name="a_pMsg">MessageContent object.</param>
        void TcpClient_EventHandshakeFail(TCPSocket a_pClient, MessageContent a_pMsg)
        {
            if (EventAllClientHandshakeFail != null)
            {
                EventAllClientHandshakeFail(a_pClient, a_pMsg);
            }
        } // end of TcpClient_EventDataReceive()

        /// <summary>
        /// delegate function, using for MTCPIPClient.EventDataReceive
        /// </summary>
        /// <param name="a_pMsg">MessageContent object.</param>
        void TcpClient_EventDataReceive(MessageContent a_pMsg)
        {
            if (EventAllClientReceive != null)
            {
                EventAllClientReceive(a_pMsg);
            }
        } // end of TcpClient_EventDataReceive()

        /// <summary>
        /// Send text data to appoint client.
        /// </summary>
        /// <param name="a_i1ClientID">Appoint client</param>
        /// <param name="a_i1MsgType">Message Type</param>
        /// <param name="a_sMsgContent">Text data content</param>
        public void SendDataToClient(byte a_i1ClientID, byte a_i1MsgType, string a_sMsgContent)
        {
            TCPSocket t_pTargetTcpClient = null;
            try
            {
                bool t_bIsFoundTarget = false;
                int retry = 0;
                while (retry < 5)
                {
                    for (int t_i4ClientIndex = 0; t_i4ClientIndex < m_pClientList.Count; t_i4ClientIndex++)
                    {
                        t_pTargetTcpClient = m_pClientList[t_i4ClientIndex];
                        if (t_pTargetTcpClient.ClientID == a_i1ClientID)
                        {
                            t_bIsFoundTarget = true;
                            break;
                        }
                    }
                    if (t_bIsFoundTarget)
                        break;
                    retry++;
                    Thread.Sleep(200);
                }
                if (!t_bIsFoundTarget)
                {
                    throw new Exception("Client is not exist");
                }
                MessageContent t_pMsg = new MessageContent(a_i1MsgType,
                    MessageConst.SERVER_ID, a_i1ClientID, Encoding.ASCII.GetBytes(a_sMsgContent));
                t_pTargetTcpClient.SendMessage(t_pMsg);
            }
            catch (SocketException ex)
            {
                Debug.WriteLine(ex.NativeErrorCode + ":" + ex.Message);
                Debug.WriteLine("Remove " + t_pTargetTcpClient.ToString());
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[ErrorCode]:{0},[SocketException]:{1} In [SendDataToClient]", ex.NativeErrorCode, ex.Message));
                // if got socket exception, release client
                m_pClientList.Remove(t_pTargetTcpClient);
                t_pTargetTcpClient.Destory();
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [SendDataToClient]", ex.Message));
                throw;
            }
        } // end of SendDataToClient(string a_sClientID,int a_i4MsgID, string a_sMsg,int a_i4CmdID)
        /// <summary>
        /// Send text data to appoint client.
        /// </summary>
        /// <param name="a_i1ClientID">Appoint client</param>
        /// <param name="a_i1MsgType">Message Type</param>
        /// <param name="a_i4CmdID">Command ID</param>
        /// <param name="a_sMsgContent">Text data content</param>
        public void SendDataToClient(byte a_i1ClientID, byte a_i1MsgType, byte a_i4CmdID, string a_sMsgContent)
        {
            TCPSocket t_pTargetTcpClient = null;
            try
            {
                bool t_bIsFoundTarget = false;
                int retry = 0;
                while (retry < 5)
                {
                    for (int t_i4ClientIndex = 0; t_i4ClientIndex < m_pClientList.Count; t_i4ClientIndex++)
                    {
                        t_pTargetTcpClient = m_pClientList[t_i4ClientIndex];
                        if (t_pTargetTcpClient.ClientID == a_i1ClientID)
                        {
                            t_bIsFoundTarget = true;
                            break;
                        }
                    }
                    if (t_bIsFoundTarget)
                        break;
                    retry++;
                    Thread.Sleep(200);
                }
                if (!t_bIsFoundTarget)
                {
                    throw new Exception("Client is not exist");
                }
                MessageContent t_pMsg = new MessageContent(a_i1MsgType,
                    MessageConst.SERVER_ID, a_i1ClientID, Encoding.ASCII.GetBytes(a_sMsgContent), a_i4CmdID);
                t_pTargetTcpClient.SendMessage(t_pMsg);
            }
            catch (SocketException ex)
            {
                Debug.WriteLine(ex.NativeErrorCode + ":" + ex.Message);
                Debug.WriteLine("Remove " + t_pTargetTcpClient.ToString());
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[ErrorCode]:{0},[SocketException]:{1} In [SendDataToClient]", ex.NativeErrorCode, ex.Message));
                // if got socket exception, release client
                m_pClientList.Remove(t_pTargetTcpClient);
                t_pTargetTcpClient.Destory();
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [SendDataToClient]", ex.Message));
                throw;
            }
        } // end of SendDataToClient(string a_sClientID,int a_i4MsgID, string a_sMsg)

        public ClientStatus GetClientStatus(int a_i4ClientID,string a_sClientName)
        {
            ClientStatus t_pStatus = new ClientStatus(a_i4ClientID, a_sClientName);
            foreach (TCPSocket t_pShutDownTcpClient in m_pClientList)
            {
                if(t_pShutDownTcpClient.ClientID == t_pStatus.ClientID)
                {
                    t_pStatus.IsConnect = t_pShutDownTcpClient.IsConnected;
                    break;
                }
            }
            return t_pStatus;
        }
    }
}
