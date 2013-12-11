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
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CloudBox.TcpObject
{
    /// <summary>
    /// TCP IP Socket
    /// </summary>
    public class TCPSocket
    {
        protected const int HANDSHAKE_TIMEOUT = 10;
        protected const int AUTO_HANDSHAKE_TIME = 20;
        protected const int RETRY_CONNECT_TIME = 10;

        /// <summary>
        /// IPv4 IP string.
        /// </summary>
        protected string m_sIP;

        /// <summary>
        /// port number.
        /// </summary>
        protected int m_i4Port;

        /// <summary>
        /// will connect target name.
        /// </summary>
        protected string m_sTargetName;
        /// <summary>
        /// id by myself
        /// </summary>
        protected byte m_i1ClientID;
        /// <summary>
        /// socket instance
        /// </summary>
        protected Socket m_pClient;
        /// <summary>
        /// thread for receive data.
        /// </summary>
        protected Thread m_pReceiveThread;
        /// <summary>
        /// thread for handshake message and auto connection check handshaking.
        /// </summary>
        protected Thread m_pHandshakeThread;
        /// <summary>
        /// message list for handshake timeout check.
        /// </summary>
        protected List<MessageContent> m_pMsgHandshakeList = new List<MessageContent>();
        /// <summary>
        /// Time tick for auto connection handshaking
        /// </summary>
        protected DateTime m_dtLastHandshakeTime;

        /// <summary>
        /// flag for thread life.
        /// </summary>
        protected bool m_bIsLive;

        /// <summary>
        /// Trace Log Event Delegate.
        /// </summary>
        /// <param name="a_i4Level">Log level</param>
        /// <param name="a_sLog">Log</param>
        public delegate void TraceLogHandler(int a_i4Level, string a_sLog);

        /// <summary>
        /// MessageContent object event delegate.
        /// </summary>
        /// <param name="a_pMsg">MessageContent object</param>
        public delegate void MessageContentHandler(MessageContent a_pMsg);

        /// <summary>
        /// Handler for handshake
        /// </summary>
        /// <param name="a_pClient"></param>
        /// <param name="a_pMsg"></param>
        public delegate void HandshakeHandler(TCPSocket a_pClient, MessageContent a_pMsg);

        /// <summary>
        /// check MTCPSocket exist in server.
        /// </summary>
        /// <param name="a_pClient">this point</param>
        /// <returns>yes:exist, no:not exist</returns>
        public delegate bool IsExistInServerHandler(TCPSocket a_pClient);

        /// <summary>
        /// Client shutdown Event Delegate.
        /// </summary>
        /// <param name="a_i1ClientID">Client ID</param>
        public delegate void ClientShutdownHandler(byte a_i1ClientID);

        /// <summary>
        /// Client shutdown event.
        /// </summary>
        public event ClientShutdownHandler EventClientShutdown;
        /// <summary>
        /// Log event.
        /// </summary>
        public event TraceLogHandler EventTraceLog;
        /// <summary>
        /// To fire this event, when receive data.
        /// </summary>
        public event MessageContentHandler EventDataReceive;
        /// <summary>
        /// To fire this event, when a handshake event timeout 5 sec.
        /// </summary>
        public event HandshakeHandler EventHandshakeFail;

        /// <summary>
        /// event for check client exist in server.
        /// </summary>
        public event IsExistInServerHandler EventIsExistInServer;

        /// <summary>
        /// Client ID
        /// </summary>
        public byte ClientID
        {
            get { return m_i1ClientID; }
        }

        /// <summary>
        /// target name
        /// </summary>
        public string TargetName
        {
            get { return m_sTargetName; }
        }

        /// <summary>
        /// check connection flag
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (m_pClient == null)
                    return false;
                return m_pClient.Connected;
            }
        }

        /// <summary>
        /// Trace Log
        /// </summary>
        /// <param name="a_i4Level">Log level</param>
        /// <param name="a_sLog">Log content</param>
        protected void TraceLog(int a_i4Level, string a_sLog)
        {
            if (EventTraceLog != null)
            {
                try
                {
                    EventTraceLog(a_i4Level, "[Client ID]:" + ClientID + " To [" + TargetName + "]," + a_sLog);
                }
                catch { }
            }
        }

        protected void DoEventShutdown()
        {
            if (EventClientShutdown != null)
                EventClientShutdown(this.ClientID);
        }

        /// <summary>
        /// not use.
        /// </summary>
        protected TCPSocket() { }

        /// <summary>
        /// MTCPSocket Construct
        /// </summary>
        /// <param name="a_i1ClientID">Client ID</param>
        public TCPSocket(byte a_i1ClientID)
        {
            m_i1ClientID = a_i1ClientID;
            m_sTargetName = "No Target";
        } // end of MTCPIPClient(string a_sClientID)

        /// <summary>
        /// MTCPSocket Construct
        /// </summary>
        /// <param name="a_i1ClientID">Client ID</param>
        /// <param name="a_sTargetName">Target Name</param>
        /// <param name="a_sIP">Remote IP</param>
        /// <param name="a_i4Port">Remote Port</param>
        public TCPSocket(byte a_i1ClientID, string a_sTargetName, string a_sIP, int a_i4Port)
        {
            m_i1ClientID = a_i1ClientID;
            m_sTargetName = a_sTargetName;
            m_sIP = a_sIP;
            m_i4Port = a_i4Port;
        } // end of MTCPIPClient(byte a_i1ClientID,string a_sIP,int a_i4Port)

        /// <summary>
        /// MTCPSocket Construct, using for server accept a new client
        /// </summary>
        /// <param name="a_pClient">Socket Client</param>
        /// <param name="a_i1ClientID">Client ID</param>
        public TCPSocket(Socket a_pClient, byte a_i1ClientID)
        {
            // New MTCPClient
            // using by server
            m_pClient = a_pClient;
            m_i1ClientID = a_i1ClientID;
            m_sTargetName = "Client";
        } // end of MTCPIPClient(Socket a_pClient, string a_sClientID)

        /// <summary>
        /// Destructor
        /// </summary>
        ~TCPSocket()
        {
            Destory();
        } // end of ~MTCPSocket()

        /// <summary>
        /// Get MTCPSocket information
        /// </summary>
        /// <returns>RemoteEndPoint + Client ID</returns>
        public override string ToString()
        {
            try
            {
                if (m_pClient != null && m_pClient.RemoteEndPoint != null)
                    return m_pClient.RemoteEndPoint.ToString() + ", Client[" + ClientID + "]";
            }catch{}
            return "Client[" + ClientID + "]";
        } // end of ToString()

        /// <summary>
        /// Send message with MessageContent object.
        /// </summary>
        /// <param name="a_i1MsgType">Message type</param>
        /// <param name="a_sMsgContent">Text data content</param>
        public void SendMessage(byte a_i1MsgType, string a_sMsgContent)
        {
            try
            {
                // send string data to server(FrontEnd or MainFrame)
                MessageContent t_pMsg = new MessageContent(a_i1MsgType,
                    ClientID, MessageConst.SERVER_ID, Encoding.ASCII.GetBytes(a_sMsgContent));

                m_pClient.Send(t_pMsg.GetBytes());
                AddToHandshakeCheck(t_pMsg);
            }
            catch (SocketException ex)
            {
                Shutdown();
                DoEventShutdown();
                Debug.WriteLine(ex.NativeErrorCode + ":" + ex.Message + " In [SendMessage]");
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[ErrorCode]:{0},[SocketException]:{1} In [SendMessage]", ex.NativeErrorCode, ex.Message));
            }
            catch (Exception ex)
            {
                Shutdown();
                DoEventShutdown();
                Debug.WriteLine(ex.Message + " In [SendMessage]");
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [SendMessage]", ex.Message));
            }
        } // end of SendMessage(int a_i4MsgID, string a_sMsg)

        /// <summary>
        /// Send message with MessageContent object.
        /// </summary>
        /// <param name="a_i1MsgType">Message type</param>
        /// <param name="a_sMsgContent">Text data content</param>
        /// <param name="a_i1Command">command</param>
        public void SendMessage(byte a_i1MsgType, string a_sMsgContent, byte a_i1Command)
        {
            try
            {
                // send string data to server(FrontEnd or MainFrame)
                MessageContent t_pMsg = new MessageContent(a_i1MsgType,
                    ClientID, MessageConst.SERVER_ID, Encoding.ASCII.GetBytes(a_sMsgContent), a_i1Command);

                m_pClient.Send(t_pMsg.GetBytes());
                AddToHandshakeCheck(t_pMsg);
            }
            catch (SocketException ex)
            {
                Shutdown();
                DoEventShutdown();
                Debug.WriteLine(ex.NativeErrorCode + ":" + ex.Message + " In [SendMessage]");
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[ErrorCode]:{0},[SocketException]:{1} In [SendMessage]", ex.NativeErrorCode, ex.Message));
            }
            catch (Exception ex)
            {
                Shutdown();
                DoEventShutdown();
                Debug.WriteLine(ex.Message + " In [SendMessage]");
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [SendMessage]", ex.Message));
            }
        } // end of SendMessage(int a_i4MsgID, string a_sMsg)

        /// <summary>
        /// Send message with MessageContent object.
        /// </summary>
        /// <param name="a_i1MsgType">Message type</param>
        /// <param name="a_bContent">Binary data content</param>
        public void SendMessage(byte a_i1MsgType, byte[] a_bContent)
        {
            try
            {
                // send byte array data to server(FrontEnd or MainFrame)
                MessageContent t_pMsg = new MessageContent(a_i1MsgType,
                    ClientID, MessageConst.SERVER_ID, a_bContent);

                m_pClient.Send(t_pMsg.GetBytes());
                AddToHandshakeCheck(t_pMsg);
            }
            catch (SocketException ex)
            {
                Shutdown();
                DoEventShutdown();
                Debug.WriteLine(ex.NativeErrorCode + ":" + ex.Message + " In [SendMessage]");
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[ErrorCode]:{0},[SocketException]:{1} In [SendMessage]", ex.NativeErrorCode, ex.Message));
            }
            catch (Exception ex)
            {
                Shutdown();
                DoEventShutdown();
                Debug.WriteLine(ex.Message + " In [SendMessage]");
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [SendMessage]", ex.Message));
            }
        } // end of SendMessage(int a_i4MsgID, byte[] a_bContent)

        /// <summary>
        /// Send message with MessageContent object.
        /// </summary>
        /// <param name="a_pMsg">MessageContent object</param>
        public void SendMessage(MessageContent a_pMsg)
        {
            try
            {
                // send data with MessageContent
                m_pClient.Send(a_pMsg.GetBytes());
                AddToHandshakeCheck(a_pMsg);
            }
            catch (SocketException ex)
            {
                Shutdown();
                DoEventShutdown();
                Debug.WriteLine(ex.NativeErrorCode + ":" + ex.Message + " In [SendMessage]");
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[ErrorCode]:{0},[SocketException]:{1} In [SendMessage]", ex.NativeErrorCode, ex.Message));
            }
            catch (Exception ex)
            {
                Shutdown();
                DoEventShutdown();
                Debug.WriteLine(ex.Message + " In [SendMessage]");
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [SendMessage]", ex.Message));
            }
        } // end of SendMessage(MessageContent a_pMsg)

        /// <summary>
        /// Add message type MessageConst.TYPE_COMMAND, MessageConst.TYPE_AUTOMATION, MessageConst.TYPE_HANDSHAKE
        /// to handshake checking.
        /// </summary>
        /// <param name="a_pMsg">MessageContent object</param>
        void AddToHandshakeCheck(MessageContent a_pMsg)
        {
            if (a_pMsg.MessageType == MessageConst.TYPE_COMMAND ||
                a_pMsg.MessageType == MessageConst.TYPE_AUTOMATION ||
                a_pMsg.MessageType == MessageConst.TYPE_HANDSHAKE)
            {
                a_pMsg.CheckTime = DateTime.Now;
                lock (m_pMsgHandshakeList)
                {
                    m_pMsgHandshakeList.Add(a_pMsg);
                }
            }
        } // end of AddHandshakeMessage(MessageContent a_pMsg)

        /// <summary>
        /// Check receive message is or not handshake message.
        /// if is handshake message response handshake OK.
        /// </summary>
        /// <param name="a_pMsg">MessageContent object</param>
        void ResponseHandshake(MessageContent a_pMsg)
        {
            if (a_pMsg.MessageType == MessageConst.TYPE_COMMAND ||
                a_pMsg.MessageType == MessageConst.TYPE_AUTOMATION ||
                a_pMsg.MessageType == MessageConst.TYPE_HANDSHAKE)
            {
                // Check client exist in TCPServer or not.
                if(a_pMsg.MessageType == MessageConst.TYPE_HANDSHAKE)
                {
                    if(EventIsExistInServer != null)
                    {
                        bool t_bIsExist = EventIsExistInServer(this);
                        if (!t_bIsExist)
                        {
                            Destory();
                            throw new Exception("Client[" + ClientID + "] dose not exist in server.");
                        }
                    }
                }
                // if receive a COMMAND or AUTOMATION
                // response a OK handshake
                MessageContent t_pMsg = new MessageContent((byte)(a_pMsg.MessageType + 1), a_pMsg.MessageID,
                    a_pMsg.TargetID, a_pMsg.SourceID, ASCIIEncoding.ASCII.GetBytes("OK"));
                SendMessage(t_pMsg);
            }
        } // end of ResponseHandshake(MessageContent a_pMsg)

        /// <summary>
        /// Check receive message is or not handshake ok message.
        /// if is a handshake OK message remove the pair handshake message from m_pMsgHandshakeList.
        /// </summary>
        /// <param name="a_pMsg">MessageContent object</param>
        void CheckHandshakeOKMsg(MessageContent a_pMsg)
        {
            if (m_pMsgHandshakeList.Count == 0)
                return;
            if (a_pMsg.MessageType == MessageConst.TYPE_COMMAND_OK ||
                a_pMsg.MessageType == MessageConst.TYPE_AUTOMATION_OK ||
                a_pMsg.MessageType == MessageConst.TYPE_HANDSHAKE_OK)
            {
                for (int i = 0; i < m_pMsgHandshakeList.Count; i++)
                {
                    MessageContent t_pHandshakeMsg = m_pMsgHandshakeList[i];
                    if (t_pHandshakeMsg.MessageID == a_pMsg.MessageID)
                    {
                        if (a_pMsg.MessageType == MessageConst.TYPE_HANDSHAKE_OK)
                            m_dtLastHandshakeTime = DateTime.Now;
                        try
                        {
                            lock (m_pMsgHandshakeList)
                            {
                                m_pMsgHandshakeList.RemoveAt(i);
                            }
                        }
                        catch{}
                        break;
                    }
                } // end for
            }
        }

        /// <summary>
        /// Shutdown client.
        /// </summary>
        protected void Shutdown()
        {
            try
            {
                if (m_pClient != null)
                {
                    m_pClient.Shutdown(SocketShutdown.Both);
                    m_pClient.Close();
                    m_pClient = null;
                    TraceLog(LogLevel.LOG_LEVEL_NORMAL, this.ToString() + " Socket Connection Shutdown Succeed.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [Shutdown]", ex.Message));
            }
        } // end of Shutdown()

        /// <summary>
        /// shutdown socket and stop threads
        /// </summary>
        public virtual void Destory()
        {
            Shutdown();
            m_bIsLive = false;
            try
            {
                m_pReceiveThread.Abort();
                m_pReceiveThread = null;
            }
            catch (Exception) { }
            try
            {
                m_pHandshakeThread.Abort();
                m_pHandshakeThread = null;
            }
            catch (Exception) { }
            TraceLog(LogLevel.LOG_LEVEL_NORMAL, this.ToString() + " Destory Succeed");
        }

        /// <summary>
        ///Start Receive data and Handshake's thread.
        /// </summary>
        public void StartReceive()
        {
            if(m_bIsLive)
            {
                TraceLog(LogLevel.LOG_LEVEL_NORMAL, this.ToString() + " thread already started.");
            }
            else
            {
                m_bIsLive = true;
                m_pReceiveThread = new Thread(new ThreadStart(this.ReceiveData));
                m_pReceiveThread.Name = m_i1ClientID.ToString();
                m_pReceiveThread.Start();
                Thread.Sleep(1);
                m_pHandshakeThread = new Thread(new ThreadStart(this.HandshakeData));
                m_pHandshakeThread.Name = m_i1ClientID.ToString();
                m_pHandshakeThread.Start();
                TraceLog(LogLevel.LOG_LEVEL_NORMAL, this.ToString() + " Start Receive Succeed.");
            }
        } // end of StartReceive()

        /// <summary>
        /// Analyze Client ID
        /// </summary>
        /// <param name="a_bData">Receive data content</param>
        /// <param name="a_i4Length">Receive data length</param>
        /// <returns>MessageContent object</returns>
        public static MessageContent AnalyzeClientID(byte[] a_bData, int a_i4Length)
        {
            MessageContent t_pMsg = new MessageContent(a_bData, a_i4Length);
            return t_pMsg;
        }

        /// <summary>
        /// Analyze data when receive one or more package.
        /// </summary>
        /// <param name="a_bData">Receive data content</param>
        /// <param name="a_i4Length">Receive data length</param>
        protected void AnalyzeData(byte[] a_bData, int a_i4Length)
        {
            try
            {
                while (true)
                {
                    if (a_i4Length <= MessageHeader.MSG_HEADER_LENGTH)
                        return;
                    MessageContent t_pMsg = new MessageContent(a_bData, a_i4Length);
                    if (t_pMsg.MessageType == MessageConst.TYPE_SHUTDOWN)
                    {
                        DoEventShutdown();
                        Destory();
                        return;
                    } // end if
                    else
                    {
                        // Handshake check
                        ResponseHandshake(t_pMsg);
                        CheckHandshakeOKMsg(t_pMsg);
                        DoDataReceive(t_pMsg);
                    } // end else
                    if (t_pMsg.TotalLength == a_i4Length)
                    {
                        break;
                    }
                    else
                    {
                        // remnant data
                        Debug.WriteLine(BitConverter.ToString(a_bData, 0, a_i4Length));
                        Array.Copy(a_bData, t_pMsg.TotalLength, a_bData, 0, a_i4Length);
                        Debug.WriteLine(BitConverter.ToString(a_bData, 0, a_i4Length));
                        a_i4Length = a_i4Length - t_pMsg.TotalLength;
                    }
                }// end for
            }
            catch (Exception ex)
            {
                TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [AnalyzeData]", ex.Message));
            }
        }// end of AnalyzeData(byte[] a_bData,int a_i4Length)

        /// <summary>
        /// delegate normal message to EventDataReceive.
        /// </summary>
        /// <param name="a_pMsg">MessageContent</param>
        void DoDataReceive(MessageContent a_pMsg)
        {
            // Event Receive data
            if (EventDataReceive != null)
                EventDataReceive(a_pMsg);
        }

        /// <summary>
        /// Using for Receive data with Thread.
        /// </summary>
        protected void ReceiveData()
        {
            while (m_bIsLive)
            {
                try
                {
                    // Receive Loop
                    while (IsConnected)
                    {
                        byte[] t_bData = new byte[409600];
                        int t_i4Length = m_pClient.Receive(t_bData);
                        if (t_i4Length > 0)
                        {
                            AnalyzeData(t_bData, t_i4Length);
                        }
                        if (t_i4Length == 0)
                        {
                            Shutdown();
                            DoEventShutdown();
                            break;
                        }
                        Thread.Sleep(1);
                    }
                }
                catch (SocketException ex)
                {
                    Debug.WriteLine(ex.NativeErrorCode + ":" + ex.Message + " In [ReceiveData]");
                    TraceLog(LogLevel.LOG_LEVEL_NORMAL, String.Format("[ErrorCode]:{0},[SocketException]:{1} In [ReceiveData]", ex.NativeErrorCode, ex.Message));
                    Shutdown();
                    DoEventShutdown();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + " In [ReceiveData]");
                    TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [ReceiveData]", ex.Message));
                    Shutdown();
                    DoEventShutdown();
                }
                Thread.Sleep(1000);
            }
        } // end of ReceiveData()

        /// <summary>
        /// Using for Handshake message checking with Thread.
        /// </summary>
        protected void HandshakeData()
        {
            while(m_bIsLive)
            {
                try
                {
                    // Handshake Loop
                    AutoHandshake();
                    CheckHandshakeTimeout();
                    CheckAutoHandshakeTimeout();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message + " In [HandshakeData]");
                    TraceLog(LogLevel.LOG_LEVEL_DEBUG, String.Format("[Exception]:{0} In [HandshakeData]", ex.Message));
                }
                Thread.Sleep(500);
            }
        } // end of HandshakeData()

        /// <summary>
        /// delegate a message to EventHandshakeFail
        /// </summary>
        /// <param name="a_pHandshakeMsg">message from m_pMsgHandshakeList</param>
        protected virtual void DoHandshakeFail(MessageContent a_pHandshakeMsg)
        {
            TraceLog(LogLevel.LOG_LEVEL_DEBUG, "Message:" + a_pHandshakeMsg.MessageID + " idle!");
            if (EventHandshakeFail != null)
            {
                // Can not using BeginInvoke and EndInvoke in WinCE.
                // Using invoke will get a NotSupportException in XPac.
                EventHandshakeFail(this, a_pHandshakeMsg);
            }
        }

        /// <summary>
        /// check handshake timeout
        /// </summary>
        protected void CheckHandshakeTimeout()
        {
            if ( m_pMsgHandshakeList.Count > 0)
            {
                for (int i = 0; i < m_pMsgHandshakeList.Count; i++)
                {
                    MessageContent t_pHandshakeMsg = (MessageContent)m_pMsgHandshakeList[i];
                    TimeSpan t_IdleTime = DateTime.Now.Subtract(t_pHandshakeMsg.CheckTime);
                    if (t_pHandshakeMsg.MessageType != MessageConst.TYPE_HANDSHAKE &&
                        t_IdleTime.TotalSeconds >= HANDSHAKE_TIMEOUT)
                    {
                        DoHandshakeFail(t_pHandshakeMsg);
                        try
                        {
                            lock (m_pMsgHandshakeList)
                            {
                                m_pMsgHandshakeList.RemoveAt(i);
                            }
                        }
                        catch { }
                        break;
                    }
                } // end for
            }
        }

        /// <summary>
        /// check auto handshake timeout
        /// </summary>
        protected void CheckAutoHandshakeTimeout()
        {
            int t_i4HandshakeFailCount = 0;
            foreach (MessageContent t_pHandshakeMsg in m_pMsgHandshakeList)
            {
                TimeSpan t_IdleTime = DateTime.Now.Subtract(t_pHandshakeMsg.CheckTime);
                if (t_pHandshakeMsg.MessageType == MessageConst.TYPE_HANDSHAKE &&
                    t_IdleTime.TotalSeconds >= HANDSHAKE_TIMEOUT)
                {
                    t_i4HandshakeFailCount++;
                    if (t_i4HandshakeFailCount >= 3)
                    {
                        DoHandshakeFail(t_pHandshakeMsg);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// nothing to do at server.
        /// </summary>
        protected virtual void AutoHandshake()
        {
            // nothing to do.
        }
    }
}
