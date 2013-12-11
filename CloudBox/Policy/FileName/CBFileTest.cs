using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBox.Policy.NamePolicy.FileNamePolicy
{
    public enum CBFilePolicyRole
    {
        NameOnly,
        NameAndExtension,
        NameControl
    }

    public class CBFileNameTest : CBIFileNamePolicy, CBIFileSplitSizePolicy
    {
        //const long DEFAULT_SPLIT_SIZE = 1024 * 1024 * 5;
        const long DEFAULT_SPLIT_SIZE = 100;
        public long MaxSplitSize { get; set; }

        public CBIFileNamePolicy FileNamePolicy { get; set; }
        public CBIFileNameControlPolicy FileNameControlPolicy { get; set; }
        public CBIFilenameExtensionPolicy FileNameExtensionPolicy { get; set; }

        public CBFilePolicyRole PolicyRole { get; set; }

        public CBFileNameTest()
        {
            MaxSplitSize = DEFAULT_SPLIT_SIZE;
            PolicyRole = CBFilePolicyRole.NameOnly;
        }

        public string Name
        {
            get
            {
                if (PolicyRole == CBFilePolicyRole.NameAndExtension)
                    return FileNamePolicy.Name + "." + FileNameExtensionPolicy.Name;
                else if (PolicyRole == CBFilePolicyRole.NameControl)
                    return FileNamePolicy.Name + "_" + FileNameControlPolicy.Name + "." + FileNameExtensionPolicy.Name;
                return FileNamePolicy.Name;
            }
            set
            {
                FileNamePolicy.Name = value;
            }
        }
    }
}
