using System.Collections.Generic;
using System.Threading;

namespace CloudBox.Log
{
    class CBLogQueue : CBLogControl
    {
        Queue<CBLogInfo> _queue;
        Thread _thread;
        bool _alive;

        public CBLogQueue() : base()
        {
            _queue = new Queue<CBLogInfo>();
            _alive = true;
            _thread = new Thread(new ThreadStart(this.PollingQueue));
            _thread.Start();
        }

        ~CBLogQueue()
        {
            Release();
        }

        public override void Log(CBLogInfo logInfo)
        {
            lock (_queue)
            {
                _queue.Enqueue(logInfo);
            }
        }

        void PurgeQueue()
        {
            while(_queue.Count > 0)
            {
                CBLogInfo logInfo = null;
                lock (_queue)
                {
                    logInfo = _queue.Dequeue();
                }
                Logger.Log(logInfo);
            }
        }

        void PollingQueue()
        {
            while(_alive)
            {
                try
                {
                    if (_queue.Count > 0)
                    {
                        CBLogInfo logInfo = null;
                        lock (_queue)
                        {
                            logInfo = _queue.Dequeue();
                        }
                        Logger.Log(logInfo);
                    }
                }
                catch {}
                Thread.Sleep(100);
            }
        }

        #region CBILogger Members


        public override void Release()
        {
            // nothing to do
            _alive = false;
            try
            {
                _thread.Abort();
            }
            catch { }
            Thread.Sleep(300);
            PurgeQueue();
        }

        #endregion
    }
}
