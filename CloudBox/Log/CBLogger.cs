using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBox.Log
{
    public abstract class CBLogger : CBILog , CBIRelease
    {
        protected CBILogger logger;
        protected CBILogger _logger;

        public CBLogger()
        {
            _logger = new CBDebugLogger();
        }

        ~CBLogger()
        {
            Release();
        }

        #region CBILog Members

        public void LogDebug(string message)
        {
            CBLogInfo logInfo = new CBLogInfo(LogLevel.Debug, message);
            Log(logInfo);
        }

        public void LogDebug(Exception ex)
        {
            CBLogInfo logInfo = new CBLogInfo(LogLevel.Debug, ex);
            Log(logInfo);
        }

        public void LogError(string message)
        {
            CBLogInfo logInfo = new CBLogInfo(LogLevel.Error, message);
            Log(logInfo);
        }

        public void LogError(Exception ex)
        {
            CBLogInfo logInfo = new CBLogInfo(LogLevel.Error, ex);
            Log(logInfo);
        }

        public void LogInfo(string message)
        {
            CBLogInfo logInfo = new CBLogInfo(LogLevel.Info, message);
            Log(logInfo);
        }

        public void LogInfo(Exception ex)
        {
            CBLogInfo logInfo = new CBLogInfo(LogLevel.Info, ex);
            Log(logInfo);
        }

        #endregion

        protected virtual void Log(CBLogInfo logInfo)
        {
            try
            {
                logger.Log(logInfo);
            }
            catch(Exception ex)
            {
                CBLogInfo errorInfo = new CBLogInfo(LogLevel.Error, ex);
                _logger.Log(errorInfo);
            }
        }

        #region CBIRelease Members

        public virtual void Release()
        {
            // nothing to do
        }

        #endregion
    }

    public class CBLogger<TLogger> : CBLogger
        where TLogger : CBILogger, new()
    {
        public CBLogger()
            : base()
        {
            logger = new TLogger();
        }

        ~CBLogger()
        {
            Release();
        }
    }

    public class CBLogger<TLogger, TLogControl> : CBLogger<TLogger>
        where TLogger : CBILogger, new()
        where TLogControl : CBILogControl, new()
    {
        TLogControl controller;

        public CBLogger()
            : base()
        {
            controller = new TLogControl();
            controller.Logger = logger;
        }

        ~CBLogger()
        {
            Release();
        }

        protected override void Log(CBLogInfo logInfo)
        {
            try
            {
                controller.Log(logInfo);
            }
            catch (Exception ex)
            {
                CBLogInfo errorInfo = new CBLogInfo(LogLevel.Error, ex);
                _logger.Log(errorInfo);
            }
        }

        public override void Release()
        {
            controller.Release();
        }
    }
}
