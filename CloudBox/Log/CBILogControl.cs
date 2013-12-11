
namespace CloudBox.Log
{
    public interface CBILogControl : CBILogger, CBIRelease
    {
        CBILogger Logger { get; set; }
    }

    public class CBLogControl : CBILogControl
    {
        public CBILogger Logger { get; set; }

        public CBLogControl() {}

        public virtual void Log(CBLogInfo logInfo)
        {
            Logger.Log(logInfo);
        }

        #region CBIRelease Members

        public virtual void Release()
        {
            // nothing to do
        }

        #endregion
    }
}
