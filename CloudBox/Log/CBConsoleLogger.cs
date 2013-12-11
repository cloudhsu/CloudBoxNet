using System;

namespace CloudBox.Log
{
    class CBConsoleLogger : CBILogger
    {
        #region _CBILog Members

        public void Log(CBLogInfo logInfo)
        {
            Console.Write(logInfo.ToString());
        }

        #endregion
    }
}
