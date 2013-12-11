using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBox.Policy
{
    /// <summary>
    /// base policy for return value policy
    /// </summary>
    public interface CBIRPolicy : CBIPolicy
    {
    }

    /// <summary>
    /// Return TReturn and none parameter policy
    /// </summary>
    /// <typeparam name="TReturn">return value</typeparam>
    public interface CBIRPolicy<TReturn> : CBIRPolicy
    {
        TReturn Policy();
    }

    /// <summary>
    /// Return TReturn and one parameter policy
    /// </summary>
    /// <typeparam name="TReturn">generic return value</typeparam>
    /// <typeparam name="TParam">generic parameter</typeparam>
    public interface CBIRPolicy<TReturn, TParam> : CBIRPolicy
    {
        TReturn Policy(TParam param);
    }

    /// <summary>
    /// Return TReturn and two parameters policy
    /// </summary>
    /// <typeparam name="TReturn">generic return value</typeparam>
    /// <typeparam name="TParam1">generic parameter 1</typeparam>
    /// <typeparam name="TParam2">generic parameter 2</typeparam>
    public interface CBIRPolicy<TReturn, TParam1, TParam2> : CBIRPolicy
    {
        TReturn Policy(TParam1 param1, TParam2 param2);
    }

    /// <summary>
    /// Return TReturn and three parameters policy
    /// </summary>
    /// <typeparam name="TReturn">generic return value</typeparam>
    /// <typeparam name="TParam1">generic parameter 1</typeparam>
    /// <typeparam name="TParam2">generic parameter 2</typeparam>
    /// <typeparam name="TParam3">generic parameter 3</typeparam>
    public interface CBIRPolicy<TReturn, TParam1, TParam2, TParam3> : CBIRPolicy
    {
        TReturn Policy(TParam1 param1, TParam2 param2, TParam3 param3);
    }

    /// <summary>
    /// Return TReturn and four parameters policy
    /// </summary>
    /// <typeparam name="TReturn">generic return value</typeparam>
    /// <typeparam name="TParam1">generic parameter 1</typeparam>
    /// <typeparam name="TParam2">generic parameter 2</typeparam>
    /// <typeparam name="TParam3">generic parameter 3</typeparam>
    /// <typeparam name="TParam4">generic parameter 4</typeparam>
    public interface CBIRPolicy<TReturn, TParam1, TParam2, TParam3, TParam4> : CBIRPolicy
    {
        TReturn Policy(TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4);
    }
}
