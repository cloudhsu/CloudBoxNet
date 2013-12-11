using System;
using System.IO;

namespace CloudBox.Policy.NamePolicy.FileNamePolicy
{
    /// <summary>
    /// The base policy interface for file name control.
    /// </summary>
    public interface CBIFileNameControlPolicy : CBIFileNamePolicy
    {
        void Control();
    }

    /// <summary>
    /// File split size
    /// </summary>
    public interface CBIFileSplitSizePolicy
    {
        long MaxSplitSize { get; set; }
    }

    /// <summary>
    /// Use for large file split
    /// </summary>
    public interface CBILargeFileSplitPolicy
    {
        int Index { get; set; }
    }

    /// <summary>
    /// File split policy
    /// If file exist and file size large more than setting it will split file.
    /// EX: 01, 02 or 03
    /// </summary>
    public class CBFileNumSplitPolicy : CBILargeFileSplitPolicy, CBIFileNameControlPolicy
    {
        public CBFileNumSplitPolicy()
        {
            Index = 1;
        }
        public int Index { get; set; }

        public virtual string Name
        {
            get
            {
                return Index.ToString("00");
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Control()
        {
            Index++;
        }

    }

    /// <summary>
    /// File split policy
    /// If file exist and file size large more than setting it will split file.
    /// EX: AA, AB or AC
    /// </summary>
    public class CBFileTextSplitPolicy : CBFileNumSplitPolicy
    {
        public CBFileTextSplitPolicy() : base()
        {
            Index = 0;
        }

        public override string Name
        {
            get
            {
                int ch1 = (Index / 26) + 0x41;
                int ch2 = (Index % 26) + 0x41;
                return string.Format("{0}{1}",Convert.ToChar(ch1), Convert.ToChar(ch2));
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
