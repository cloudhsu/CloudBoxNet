using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBox.Policy.SQLPolicy
{
    public interface CBIConditionPolicy
    {
        string Condition { get; set; }
    }
}
