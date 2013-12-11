using System;

namespace CloudBox.Log
{
    public static class CBLog
    {
        static _CBLog _log = new _CBLog();

        public static void ReleaseLog()
        {
            _log.ReleaseLog();
        }
        
        public static Log.CBLogType LogType
        {
            get { return _log.LogType; }
            set
            {
                _log.LogType = value;
            }
        }

        public static void CustomLog<TLogger>()
            where TLogger : CBILogger, new()
        {
            _log.CustomLog<TLogger>();
        }

        public static void CustomLog<TLogger,TControl>()
            where TLogger : CBILogger, new()
            where TControl : CBILogControl, new()
        {
            _log.CustomLog<TLogger, TControl>();
        }

        public static void CustomLog(CBLogger logger)
        {
            _log.CustomLog(logger);
        }

        public static void LogDebug(string message)
        {
            _log.LogDebug(message);
        }
        public static void LogDebug(Exception ex)
        {
            _log.LogDebug(ex);
        }

        public static void LogError(string message)
        {
            _log.LogError(message);
        }
        public static void LogError(Exception ex)
        {
            _log.LogError(ex);
        }

        public static void LogInfo(string message)
        {
            _log.LogInfo(message);
        }
        public static void LogInfo(Exception ex)
        {
            _log.LogInfo(ex);
        }
    }

    class _CBLog
    {
        CBLogger _logger;
        CBLogType _logType;

        public _CBLog()
        {
#if DEBUG
            _logger = new CBLogger<CBConsoleLogger>();
#else
            _logger = new CBLogger<CBTextLogger>();
#endif
            _logType = CBLogType.LogDefault;
        }

        ~_CBLog()
        {
            ReleaseLog();
        }

        public void ReleaseLog()
        {
            if (_logger != null)
                _logger.Release();
        }

        public Log.CBLogType LogType
        {
            get { return _logType; }
            set
            {
                _logType = value;
                SetLogType(_logType);
            }
        }
        void SetLogType(CBLogType type)
        {
            ReleaseLog();
            if (type == CBLogType.LogConsole)
            {
                _logger = new CBLogger<CBConsoleLogger>();
            }
            else if (type == CBLogType.LogTextFile)
            {
                _logger = new CBLogger<CBTextLogger>();
                //_logger = new CBLogger<CBTextLogger,CBLogQueue>();
            }
            else if (type == CBLogType.LogXMLFile)
            {
                _logger = new CBLogger<CBXmlLogger>();
            }
            else if (type == CBLogType.LogOtherCustom)
            {
                _logger = null;
            }
            else
            {
#if DEBUG
                _logger = new CBLogger<CBDebugLogger>();
#else
                _logger = new CBLogger<CBTextLogger>();
#endif
            }
        }

        public void Check()
        {
            if (_logType == CBLogType.LogOtherCustom && _logger == null)
            {
                throw new NotImplementedException("You need to implement a CBILogger");
            }
        }

        public void CustomLog<TLogger>()
            where TLogger : CBILogger, new()
        {
            ReleaseLog();
            _logType = CBLogType.LogOtherCustom;
            _logger = new CBLogger<TLogger>();
        }

        public void CustomLog<TLogger, TControl>()
            where TLogger : CBILogger, new()
            where TControl : CBILogControl, new()
        {
            ReleaseLog();
            _logType = CBLogType.LogOtherCustom;
            _logger = new CBLogger<TLogger, TControl>();
        }

        public void CustomLog(CBLogger logger)
        {
            ReleaseLog();
            _logType = CBLogType.LogOtherCustom;
            _logger = logger;
        }

        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }
        public void LogDebug(Exception ex)
        {
            _logger.LogDebug(ex);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }
        public void LogError(Exception ex)
        {
            _logger.LogError(ex);
        }

        public void LogInfo(string message)
        {
            _logger.LogInfo(message);
        }
        public void LogInfo(Exception ex)
        {
            _logger.LogInfo(ex);
        }
    }
}
