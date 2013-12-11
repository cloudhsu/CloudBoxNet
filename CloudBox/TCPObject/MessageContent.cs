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
using System.Text;

namespace CloudBox.TcpObject
{
    /// <summary>
    /// Format message content for communication.
    /// </summary>
    public sealed class MessageContent
    {
        MessageHeader m_pHeader;
        byte[] m_bContent;
        DateTime m_dtCheckTime; // using for check response

        private MessageContent() { }

        /// <summary>
        /// get message information
        /// </summary>
        /// <returns>message information</returns>
        public override string ToString()
        {
            return "Message:" + MessageID + " TargetID:" + TargetID;
        }

        /// <summary>
        /// MessageContent Construct,using for AnalyzeData
        /// </summary>
        /// <param name="a_bData">Data</param>
        /// <param name="a_i4Length">Data Length</param>
        public MessageContent(byte[] a_bData, int a_i4Length)
        {
            // using for data receive
            m_pHeader = new MessageHeader();
            MessageType = a_bData[0];
            SourceID = a_bData[1];
            TargetID = a_bData[2];
            CommandID = a_bData[3];
            MessageID = BitConverter.ToInt32(a_bData, 4);
            ContentLength = BitConverter.ToInt32(a_bData, 8);
            if (ContentLength > a_i4Length)
                throw new Exception("Content Error!");
            if (ContentLength == 0)
                throw new Exception("Format Error!");
            Content = new byte[ContentLength];
            Array.Copy(a_bData, MessageHeader.MSG_HEADER_LENGTH, Content, 0, ContentLength);
        }

        /// <summary>
        /// MessageContent Construct,using for return handshake message.
        /// </summary>
        /// <param name="a_i1MessageType">Message Type</param>
        /// <param name="a_i4MessageID">Message ID</param>
        /// <param name="a_i1SourceID">Source ID</param>
        /// <param name="a_i1TargetID">Target ID</param>
        /// <param name="a_bContent">Data</param>
        public MessageContent(byte a_i1MessageType, int a_i4MessageID,
            byte a_i1SourceID, byte a_i1TargetID, byte[] a_bContent)
        {
            // using for send data
            m_pHeader = new MessageHeader();
            MessageType = a_i1MessageType;
            SourceID = a_i1SourceID;
            TargetID = a_i1TargetID;
            CommandID = 0;
            Content = a_bContent;
            MessageID = a_i4MessageID;
            ContentLength = Content.Length;
        }
        /// <summary>
        /// MessageContent Construct
        /// </summary>
        /// <param name="a_i1MessageType">Message Type</param>
        /// <param name="a_i1SourceID">Source ID</param>
        /// <param name="a_i1TargetID">Target ID</param>
        /// <param name="a_i1CommandID">Command ID</param>
        /// <param name="a_bContent">Data</param>
        public MessageContent(byte a_i1MessageType, byte a_i1SourceID, byte a_i1TargetID, byte[] a_bContent, byte a_i1CommandID)
        {
            // using for send data
            m_pHeader = new MessageHeader();
            MessageType = a_i1MessageType;
            SourceID = a_i1SourceID;
            TargetID = a_i1TargetID;
            CommandID = a_i1CommandID;
            Content = a_bContent;
            MessageID = MessageIDManager.GetMessageID();
            ContentLength = Content.Length;
        }
        /// <summary>
        /// MessageContent Construct
        /// </summary>
        /// <param name="a_i1MessageType">Message Type</param>
        /// <param name="a_i1SourceID">Source ID</param>
        /// <param name="a_i1TargetID">Target ID</param>
        /// <param name="a_bContent">Data</param>
        public MessageContent(byte a_i1MessageType, byte a_i1SourceID, byte a_i1TargetID, byte[] a_bContent)
        {
            // using for send data
            m_pHeader = new MessageHeader();
            MessageType = a_i1MessageType;
            SourceID = a_i1SourceID;
            TargetID = a_i1TargetID;
            CommandID = 0;
            Content = a_bContent;
            MessageID = MessageIDManager.GetMessageID();
            ContentLength = Content.Length;
        }
        /// <summary>
        /// Get MessageContent total length
        /// (content length + header length(11 bytes) + EOF length(1 byte))
        /// </summary>
        public int TotalLength
        {
            // content length + header length + EOF length
            get { return ContentLength + MessageHeader.MSG_HEADER_LENGTH + 1; }
        }
        /// <summary>
        /// Convert MessageContent object to byte array
        /// </summary>
        /// <returns>byte array</returns>
        public byte[] GetBytes()
        {
            // total size is header length(11 bytes) + content length + end flag(1 byte)
            byte[] t_bData = new byte[ContentLength + MessageHeader.MSG_HEADER_LENGTH + 1];
            t_bData[0] = MessageType;
            t_bData[1] = SourceID;
            t_bData[2] = TargetID;
            t_bData[3] = CommandID;
            Array.Copy(BitConverter.GetBytes(MessageID), 0, t_bData, 4, 4);
            Debug.WriteLine(BitConverter.ToString(t_bData, 4, 4));
            Debug.WriteLine(BitConverter.ToInt32(t_bData, 4));
            Array.Copy(BitConverter.GetBytes(ContentLength), 0, t_bData, 8, 4);
            Debug.WriteLine(BitConverter.ToString(t_bData, 8, 4));
            Debug.WriteLine(BitConverter.ToInt32(t_bData, 8));
            Array.Copy(Content, 0, t_bData, MessageHeader.MSG_HEADER_LENGTH, ContentLength);
            Debug.WriteLine(BitConverter.ToString(t_bData, MessageHeader.MSG_HEADER_LENGTH, ContentLength));
            Array.Copy(new byte[] { MessageConst.EOF_FLAG }, 0, t_bData, MessageHeader.MSG_HEADER_LENGTH + ContentLength, 1);

            return t_bData;
        }

        /// <summary>
        /// Set Message Type
        /// </summary>
        public byte MessageType
        {
            get { return m_pHeader.MessageType; }
            set { m_pHeader.MessageType = value; }
        }

        /// <summary>
        /// message from source id
        /// </summary>
        public byte SourceID
        {
            get { return m_pHeader.SourceID; }
            set { m_pHeader.SourceID = value; }
        }

        /// <summary>
        /// message send to target id
        /// </summary>
        public byte TargetID
        {
            get { return m_pHeader.TargetID; }
            set { m_pHeader.TargetID = value; }
        }

        /// <summary>
        /// command id
        /// </summary>
        public byte CommandID
        {
            get { return m_pHeader.CommandID; }
            set { m_pHeader.CommandID = value; }
        }

        /// <summary>
        /// message id for handshake message use.
        /// </summary>
        public int MessageID
        {
            get { return m_pHeader.MessageID; }
            set { m_pHeader.MessageID = value; }
        }

        /// <summary>
        /// Content Length
        /// </summary>
        public int ContentLength
        {
            get { return m_pHeader.ContentLength; }
            set { m_pHeader.ContentLength = value; }
        }

        /// <summary>
        /// byte array Content
        /// </summary>
        public byte[] Content
        {
            get { return m_bContent; }
            set { m_bContent = value; }
        }

        /// <summary>
        /// time for handshake timeout check
        /// </summary>
        public DateTime CheckTime
        {
            get { return m_dtCheckTime; }
            set { m_dtCheckTime = value; }
        }
        /// <summary>
        /// Get String Content
        /// </summary>
        public string MsgContent
        {
            get { return ASCIIEncoding.ASCII.GetString(Content, 0, ContentLength); }
        }
    }
}
