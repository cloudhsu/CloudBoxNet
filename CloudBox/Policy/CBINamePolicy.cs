using System;
using CloudBox.General;

namespace CloudBox.Policy.NamePolicy
{
    /// <summary>
    /// Name property interface policy
    /// </summary>
    public interface CBINamePolicy : CBIPolicy
    {
        /// <summary>
        /// Name Property
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// Implement empty value for Name policy
    /// </summary>
    public class CBEmptyNamePolicy : CBINamePolicy
    {
        /// <summary>
        /// Name Property
        /// </summary>
        public string Name
        {
            get
            {
                return string.Empty;
            }
            set
            {
                throw new NotSupportedException("CBEmptyNamePolicy.Name is a readonly property.");
            }
        }
    }

    /// <summary>
    /// Implement a normal name property
    /// </summary>
    public class CBNamePolicy : CBINamePolicy
    {
        /// <summary>
        /// Name Property
        /// </summary>
        public string Name { get; set; }
    }
}
