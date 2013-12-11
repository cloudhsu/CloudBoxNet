using System;

namespace CloudBox.Policy.NamePolicy.FileNamePolicy
{
    /// <summary>
    /// The base policy interface for file extension name.
    /// </summary>
    public interface CBIFilenameExtensionPolicy : CBIFileNamePolicy
    {
    }

    /// <summary>
    /// txt extension policy
    /// </summary>
    public class CBTextExtensionPolicy : CBIFilenameExtensionPolicy
    {

        #region CBIFilenameExtensionPolicy Members

        public string Name
        {
            get
            {
                return "txt";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    /// <summary>
    /// xml extension policy
    /// </summary>
    public class CBXmlExtensionPolicy : CBIFilenameExtensionPolicy
    {

        #region CBIFilenameExtensionPolicy Members

        public string Name
        {
            get
            {
                return "xml";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    /// <summary>
    /// log extension policy
    /// </summary>
    public class CBLogExtensionPolicy : CBIFilenameExtensionPolicy
    {

        #region CBIFilenameExtensionPolicy Members

        public string Name
        {
            get
            {
                return "log";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    /// <summary>
    /// int extension policy
    /// </summary>
    public class CBIniExtensionPolicy : CBIFilenameExtensionPolicy
    {

        #region CBIFilenameExtensionPolicy Members

        public string Name
        {
            get
            {
                return "ini";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
