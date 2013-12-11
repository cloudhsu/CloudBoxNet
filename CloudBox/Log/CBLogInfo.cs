using System;
using System.Text;

namespace CloudBox.Log
{
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Error = 2
    }
    [Serializable]
    public class CBLogInfo// : ISerializable
    {
        LogLevel m_Level;
        string m_Message;
        string m_StackTrace;
        DateTime m_LogTime;

        public const string CRLF = "\r\n";

        private CBLogInfo()
        {
            m_StackTrace = "";
            m_LogTime = DateTime.Now;
        }

        public CBLogInfo(LogLevel level, string message) : this()
        {
            m_Level = level;
            m_Message = message;
        }

        public CBLogInfo(LogLevel level, Exception ex) : this()
        {
            m_Level = level;
            m_Message = string.Format("{0}:{1}",ex.GetType().FullName,ex.Message);
            m_StackTrace = ex.StackTrace;
        }
        
        public String LogTime
        {
            get { return m_LogTime.ToString("HH:mm:ss:FFF"); }
            set { m_LogTime = DateTime.Now; }
        }

        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }
        public string StackTrace
        {
            get { return m_StackTrace; }
            set { m_StackTrace = value; }
        }
        public LogLevel Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(m_LogTime.ToString("HH:mm:ss"));
            sb.Append(string.Format(":{0:###000}",m_LogTime.Millisecond));
            sb.Append("|");
            sb.Append(string.Format("{0,-5}", m_Level.ToString()));
            sb.Append("|");
            sb.Append(m_Message);
            sb.Append(CRLF);
            if (!string.IsNullOrEmpty(m_StackTrace))
            {
                sb.Append("Stack Trace:");
                sb.Append(m_StackTrace);
                sb.Append(CRLF);
            }
            return sb.ToString();
        }
    }
}
