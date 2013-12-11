using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBox.MemoryDr
{
    public class MemoryInfo
    {
        string m_name;
        long m_currentUsage;
        long m_maximumUsage;

        public long MaximumUsage
        {
            get { return m_maximumUsage; }
            set { m_maximumUsage = value; }
        }
        public long CurrentUsage
        {
            get { return m_currentUsage; }
            set { m_currentUsage = value; }
        }
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string GetCurrentKB()
        {
            return Convert.ToString(m_currentUsage / 1024.0);
        }

        public string GetMaximumKB()
        {
            return Convert.ToString(m_maximumUsage / 1024.0);
        }

        public void update(long current)
        {
            m_currentUsage = current;
            if (m_currentUsage > m_maximumUsage)
                m_maximumUsage = m_currentUsage;
        }
    }
}
