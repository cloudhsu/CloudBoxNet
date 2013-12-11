using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudBox.Log;
using System.Diagnostics;

namespace Test
{
    class OrzLog : CBILogger
    {
        #region CBILogger Members

        public void Log(CBLogInfo logInfo)
        {
            Debug.Write(logInfo.ToString());
        }

        #endregion


        void ShowList(List<int> list)
        {

        }

        void ShowList(List<double> list)
        {

        }

        void ShowList<T>(List<T> list)
        {

        }
    }
}
