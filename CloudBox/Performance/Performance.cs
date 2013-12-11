using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CloudBox.Log;

namespace CloudBox.Performance
{
    /// <summary>
    /// Performance Counter to calculate function executing time.
    /// </summary>
    public sealed class Performance
    {
        public delegate void CalculateHanlder();

        /// <summary>
        /// Using Performance.CalculFunction(MethodName)
        /// It will print executing time in Console.
        /// </summary>
        /// <param name="method">A none argument method</param>
        public static void CalculateMethod(CalculateHanlder method)
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            method.Invoke();
            st.Stop();
            string msg = string.Format("Elapsed = {0} in [{1}]", st.Elapsed.ToString(), method.Method.ToString());
            CBLog.LogInfo(msg);
        }
    }
}
