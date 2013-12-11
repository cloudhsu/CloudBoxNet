using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBox.DesignPatterns
{
    public abstract class _IndexCache
    {
        protected enum ObjectState
        {
            NO_USE,
            USING
        }

        protected List<ObjectState> m_objState = new List<ObjectState>();

        protected _IndexCache()
        {
        }
    }
}
