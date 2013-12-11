using System;

namespace CloudBox.Log
{
    public interface CBILog
    {
        void LogDebug(string message);
        void LogDebug(Exception ex);

        void LogError(string message);
        void LogError(Exception ex);

        void LogInfo(string message);
        void LogInfo(Exception ex);
    }
}