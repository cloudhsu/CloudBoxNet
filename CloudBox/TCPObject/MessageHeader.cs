namespace CloudBox.TcpObject
{
    /// <summary>
    /// Format message header for communication.
    /// </summary>
    sealed class MessageHeader
    {
        public const int MSG_HEADER_LENGTH = 12;
        // Header Length is 11 byte
        byte m_i1MessageType;  // byte [0]
        byte m_i1SourceID;     // byte [1]
        byte m_i1TargetID;     // byte [2]
        byte m_i1CommandID;    // byte [3]
        int m_i4MessageID;     // byte [4,5,6,7]
        int m_i4ContentLength; // byte [8,9,10,11]

        // Properties
        public byte MessageType
        {
            get { return m_i1MessageType; }
            set { m_i1MessageType = value; }
        }

        public byte SourceID
        {
            get { return m_i1SourceID; }
            set { m_i1SourceID = value; }
        }

        public byte TargetID
        {
            get { return m_i1TargetID; }
            set { m_i1TargetID = value; }
        }

        public byte CommandID
        {
            get { return m_i1CommandID; }
            set { m_i1CommandID = value; }
        }

        public int MessageID
        {
            get { return m_i4MessageID; }
            set { m_i4MessageID = value; }
        }

        public int ContentLength
        {
            get { return m_i4ContentLength; }
            set { m_i4ContentLength = value; }
        }
    }
}
