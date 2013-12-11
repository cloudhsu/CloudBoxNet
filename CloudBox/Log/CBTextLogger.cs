using System.IO;
using CloudBox.Policy.NamePolicy.FileNamePolicy;

namespace CloudBox.Log
{
    class CBTextLogger : CBILogger
    {
        protected const string LOG_PERNAME = "CBLog_";

        protected CBFileName m_FileName;

        public CBTextLogger()
            : this(LOG_PERNAME)
        {
        }

        protected CBTextLogger(string fileName)
        {
            m_FileName = 
                new CBFileName<CBFullPreDateNowNamePolicy, CBFileNumSplitPolicy, CBLogExtensionPolicy>();
            m_FileName.Name = fileName;
        }

        #region _CBILog Members

        public virtual void Log(CBLogInfo logInfo)
        {
            File.AppendAllText(m_FileName.Name,logInfo.ToString());
        }

        #endregion
    }
}
