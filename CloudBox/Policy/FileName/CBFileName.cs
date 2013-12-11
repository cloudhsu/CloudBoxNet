using System.IO;

namespace CloudBox.Policy.NamePolicy.FileNamePolicy
{
    /// <summary>
    /// Abstract base class for file name policy host.
    /// How to use:
    /// CBFileName filename = new CBFileName<CBIFileNamePolicy>();
    /// CBFileName filename = new CBFileName<CBIFileNamePolicy,CBIFilenameExtensionPolicy>();
    /// CBFileName filename = new CBFileName<CBIFileNamePolicy,CBIFileNameControlPolicy,CBIFilenameExtensionPolicy>();
    /// Just call fliename.Name to get file name
    /// filename.MaxSplitSize will use for CBIFileNameControlPolicy
    /// </summary>
    public abstract class CBFileName : CBIFileNamePolicy, CBIFileSplitSizePolicy
    {
        const long DEFAULT_SPLIT_SIZE = 1024 * 1024 * 5;
        //const long DEFAULT_SPLIT_SIZE = 100;
        public virtual string Name { get; set; }
        public long MaxSplitSize { get; set; }

        public CBFileName()
        {
            MaxSplitSize = DEFAULT_SPLIT_SIZE;
        }
    }

    /// <summary>
    /// This policy will get name from CBIFileNamePolicy
    /// </summary>
    /// <typeparam name="TNamePolicy">Must be CBIFileNamePolicy</typeparam>
    public sealed class CBFileName<TNamePolicy> : CBFileName
        where TNamePolicy : CBIFileNamePolicy,new()
    {
        TNamePolicy _policy;
        public CBFileName()
            : base()
        {
            _policy = new TNamePolicy();
        }
        public override string Name
        {
            get
            {
                return _policy.Name;
            }
            set
            {
                _policy.Name = value;
            }
        }
    }

    /// <summary>
    /// This policy will get name from CBIFileNamePolicy.Name + "." + CBIFilenameExtensionPolicy.Name
    /// </summary>
    /// <typeparam name="TNamePolicy">Must be CBIFileNamePolicy</typeparam>
    /// <typeparam name="TNameExtensionPolicy">Must be CBIFilenameExtensionPolicy</typeparam>
    public sealed class CBFileName<TNamePolicy, TNameExtensionPolicy> : CBFileName
        where TNamePolicy : CBIFileNamePolicy, new()
        where TNameExtensionPolicy : CBIFilenameExtensionPolicy, new()
    {
        TNamePolicy _policy1;
        TNameExtensionPolicy _policy2;
        public CBFileName()
            : base()
        {
            _policy1 = new TNamePolicy();
            _policy2 = new TNameExtensionPolicy();
        }
        public override string Name
        {
            get
            {
                return _policy1.Name + "." + _policy2.Name;
            }
            set
            {
                _policy1.Name = value;
            }
        }
    }

    /// <summary>
    /// This policy will get name from
    /// CBIFileNamePolicy.Name + "_" + CBIFileNameControlPolicy.Name + "." + CBIFilenameExtensionPolicy.Name
    /// </summary>
    /// <typeparam name="TNamePolicy">Must be CBIFileNamePolicy</typeparam>
    /// <typeparam name="TNameControlPolicy">Must be CBIFileNameControlPolicy</typeparam>
    /// <typeparam name="TNameExtensionPolicy">Must be CBIFilenameExtensionPolicy</typeparam>
    public sealed class CBFileName<TNamePolicy, TNameControlPolicy, TNameExtensionPolicy> : CBFileName, CBIFileNameControlPolicy
        where TNamePolicy : CBIFileNamePolicy, new()
        where TNameControlPolicy : CBIFileNameControlPolicy, new()
        where TNameExtensionPolicy : CBIFilenameExtensionPolicy, new()
    {
        TNamePolicy _policy1;
        TNameControlPolicy _policy2;
        TNameExtensionPolicy _policy3;

        public CBFileName()
            : base()
        {
            _policy1 = new TNamePolicy();
            _policy2 = new TNameControlPolicy();
            _policy3 = new TNameExtensionPolicy();
        }

        string FileName
        {
            get
            {
                return _policy1.Name + "_" + _policy2.Name + "." + _policy3.Name;
            }
        }

        public override string Name
        {
            get
            {
                Control();
                return FileName;
            }
            set
            {
                _policy1.Name = value;
            }
        }

        public void Control()
        {
            while (true)
            {
                FileInfo info = new FileInfo(FileName);
                if (info.Exists && info.Length > MaxSplitSize)
                {
                    _policy2.Control();
                }
                else
                {
                    break;
                }
            }
        }
    }
}
