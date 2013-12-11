using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBox.Policy
{
    /// <summary>
    /// Base interface for policy
    /// </summary>
    public interface CBIPolicy
    {
    }

    /// <summary>
    /// A void and none parameter policy
    /// </summary>
    public interface CBNIPolicy : CBIPolicy
    {
        /// <summary>
        /// A void and none parameter policy function.
        /// </summary>
        void Policy();
    }

    /// <summary>
    /// A void and one parameter policy
    /// </summary>
    /// <typeparam name="TParam">generic parameter</typeparam>
    public interface CBIPolicy<TParam> : CBIPolicy
    {
        /// <summary>
        /// A void and one parameter policy function.
        /// </summary>
        /// <param name="param">generic parameter</param>
        void Policy(TParam param);
    }

    /// <summary>
    /// A void and two parameters policy
    /// </summary>
    /// <typeparam name="TParam1">generic parameter 1</typeparam>
    /// <typeparam name="TParam2">generic parameter 2</typeparam>
    public interface CBIPolicy<TParam1, TParam2> : CBIPolicy
    {
        /// <summary>
        /// A void and two parameters policy function.
        /// </summary>
        /// <param name="param1">generic parameter 1</param>
        /// <param name="param2">generic parameter 2</param>
        void Policy(TParam1 param1, TParam2 param2);
    }

    /// <summary>
    /// A void and three parameters policy
    /// </summary>
    /// <typeparam name="TParam1">generic parameter 1</typeparam>
    /// <typeparam name="TParam2">generic parameter 2</typeparam>
    /// <typeparam name="TParam3">generic parameter 3</typeparam>
    public interface CBIPolicy<TParam1, TParam2, TParam3> : CBIPolicy
    {
        /// <summary>
        /// A void and three parameters policy function.
        /// </summary>
        /// <param name="param1">generic parameter 1</param>
        /// <param name="param2">generic parameter 2</param>
        /// <param name="param3">generic parameter 3</param>
        void Policy(TParam1 param1, TParam2 param2, TParam3 param3);
    }

    /// <summary>
    /// A void and four parameters policy
    /// </summary>
    /// <typeparam name="TParam1">generic parameter 1</typeparam>
    /// <typeparam name="TParam2">generic parameter 2</typeparam>
    /// <typeparam name="TParam3">generic parameter 3</typeparam>
    /// <typeparam name="TParam4">generic parameter 4</typeparam>
    public interface CBIPolicy<TParam1, TParam2, TParam3, TParam4> : CBIPolicy
    {
        /// <summary>
        /// A void and four parameters policy function.
        /// </summary>
        /// <param name="param1">generic parameter 1</param>
        /// <param name="param2">generic parameter 2</param>
        /// <param name="param3">generic parameter 3</param>
        /// <param name="param4">generic parameter 4</param>
        void Policy(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }
}
