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

/*
 *  Message Const rule:
 *  1. Message Type ID: 0-10 system message.
 *  2. 11~100: normal message.
 *  3. 101~255: handshake message.
 *  4. Handshake message: if you want to declare a handshake message, must like this sample.
 *    Sample:
 *     TYPE_COMMAND        = 14;
 *     TYPE_COMMAND_OK     = 15;
 *    Rule:
 *     handshake message = ID;
 *     handshake return OK = ID + 1;
 *  Testing report:
 *  1. disconnection testing.
 *      1-1. If client was shutdown server can handle it.
 *      1-2. if server shutdown, client must call connect again.
 *  2. Long data testing.
 *      Client send 200000 bytes data to server to test.
 */

namespace CloudBox.TcpObject
{
    /// <summary>
    /// Const definition
    /// </summary>
    public sealed class MessageConst
    {
        /// <summary>
        /// EOF for message content object.
        /// </summary>
        public const byte EOF_FLAG = 0xFF;

        /// <summary>
        /// server id
        /// </summary>
        public const byte SERVER_ID = 100;

        /// <summary>
        /// Message type for client id
        /// </summary>
        public const byte TYPE_CLIENT_ID      = 0;  // byte array

        /// <summary>
        /// Message type for disconnect
        /// </summary>
        public const byte TYPE_SHUTDOWN       = 1;  // byte array

        /// <summary>
        /// Message type for update. (OnStateUpdate in ModuleDaemon/FrontEnd Daemon)
        /// </summary>
        public const byte TYPE_UPDATE         = 11; // byte array (daemon)

        /// <summary>
        /// Message type for After FrontEnd send command to chamber/mainframe then get response message.
        /// </summary>
        public const byte TYPE_MESSAGE_BOX    = 12; // string (mainframe/chambers to frontend ; receiver is message dialog)

        /// <summary>
        /// Message type for update Event to FrontEnd Event log bar.
        /// </summary>
        public const byte TYPE_EVENT_LOG      = 13; // string (mainframe/chambers to frontend ; receiver is event log bar)

        /// <summary>
        /// Message type for chamber update PM to FrontEnd.
        /// </summary>
        public const byte TYPE_PM_UPDATE      = 14; // using for chamber to update PM

        /// <summary>
        /// Message type for get connection status from mainframe/frontend daemon
        /// </summary>
        public const byte TYPE_CONNECT_STATUS = 15; // using for mainframe/frontend daemon to update connect status
        
        // ------------------------------------------------------------------------------------ //
        // --- Pair handshake message start from ID 101. --- //
        
        /// <summary>
        /// Message type for frontend command
        /// </summary>
        public const byte TYPE_COMMAND        = 101; // string (frontend to mainframe/chambers , mainframe to chambers)

        /// <summary>
        /// Message type for frontend command handshake message response.
        /// </summary>
        public const byte TYPE_COMMAND_OK     = 102; // handshaking

        /// <summary>
        /// Message type for Automation
        /// </summary>
        public const byte TYPE_AUTOMATION     = 103; // string (frontend to mainframe , mainframe to frontend)
        /// <summary>
        /// Message type for Automation handshake message response.
        /// </summary>
        public const byte TYPE_AUTOMATION_OK  = 104; // handshaking

        /// <summary>
        /// Message type for Auto connection handshake message
        /// </summary>
        public const byte TYPE_HANDSHAKE      = 105; // auto handshaking check

        /// <summary>
        /// Message type for Auto connection handshake message response.
        /// </summary>
        public const byte TYPE_HANDSHAKE_OK   = 106; // handshaking check OK
    }

    /// <summary>
    /// get message id
    /// </summary>
    sealed class MessageIDManager
    {
        private MessageIDManager(){}
        static int MessageID = 1000;
        /// <summary>
        /// Get a serial number for message id.
        /// </summary>
        /// <returns>Message ID</returns>
        public static int GetMessageID()
        {
            if (MessageID >= 500000000)
                MessageID = 255;
            return MessageID++;
        }
    }
}
