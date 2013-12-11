using System.Diagnostics;

namespace CloudBox.Log
{
    class CBDebugLogger : CBILogger
    {
        #region CBILogger Members

        public void Log(CBLogInfo logInfo)
        {
            Debug.Write(logInfo.ToString());
            Trace.Write(logInfo.ToString());
        }

        #endregion
    }
}
