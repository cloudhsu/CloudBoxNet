using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudBox.Policy.NamePolicy;
using CloudBox.General;

namespace CloudBox.Policy.NamePolicy.FileNamePolicy
{
    /// <summary>
    /// The base policy interface for file name
    /// </summary>
    public interface CBIFileNamePolicy : CBINamePolicy
    {
    }

    /// <summary>
    /// Basic implement for CBIFileNamePolicy
    /// </summary>
    public class CBFileNamePolicy : CBIFileNamePolicy
    {
        /// <summary>
        /// File Name
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Get full path file name.
    /// EX: C:/Test/MyFileName
    /// </summary>
    public class CBFullFileNamePolicy : CBIFileNamePolicy
    {
        /// <summary>
        /// File Name
        /// </summary>
        string m_name;

        /// <summary>
        /// Setter: set value to name
        /// Getter: get full path with name
        /// </summary>
        public string Name
        {
            get
            {
                return CBGeneral.GetFullPath(m_name);
            }
            set
            {
                m_name = value;
            }
        }
    }

    /// <summary>
    /// Readonly policy, to get yyyyMMdd file name
    /// EX: 20110923
    /// </summary>
    public class CBDateNowNamePolicy : CBIFileNamePolicy
    {

        #region CBINamePolicy Members

        public string Name
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMdd");
            }
            set
            {
                throw new NotSupportedException("CBDateNowNamePolicy.Name is a readonly property.");
            }
        }

        #endregion
    }

    /// <summary>
    /// File name with date yyyyMMdd
    /// EX: MyFileName20110923
    /// </summary>
    public class CBPreDateNowNamePolicy : CBIFileNamePolicy
    {
        string m_name;

        #region CBINamePolicy Members

        public string Name
        {
            get
            {
                return m_name + DateTime.Now.ToString("yyyyMMdd");
            }
            set
            {
                m_name = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// Full path with date yyyyMMdd
    /// EX: C:/Test/20110923
    /// </summary>
    public class CBFullDateNowNamePolicy : CBIFileNamePolicy
    {

        #region CBINamePolicy Members

        public string Name
        {
            get
            {
                return CBGeneral.GetFullPath(DateTime.Now.ToString("yyyyMMdd"));
            }
            set
            {
                throw new NotSupportedException("CBFullPathDateNowNamePolicy.Name is a readonly property.");
            }
        }

        #endregion
    }

    /// <summary>
    /// Full file name with date yyyyMMdd
    /// EX: C:/Test/MyFileName20110923
    /// </summary>
    public class CBFullPreDateNowNamePolicy : CBIFileNamePolicy
    {
        string m_name;

        #region CBINamePolicy Members

        public string Name
        {
            get
            {
                return CBGeneral.GetFullPath(m_name + DateTime.Now.ToString("yyyyMMdd"));
            }
            set
            {
                m_name = value;
            }
        }

        #endregion
    }
}
