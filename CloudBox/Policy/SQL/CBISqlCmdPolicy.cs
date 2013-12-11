using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBox.Policy.SQLPolicy
{
    public interface CBISqlCmdPolicy
    {
        string SqlCmd { get; set; }
    }

    public class CBISqlSelect : CBISqlCmdPolicy
    {
        public string SqlCmd
        {
            get { return "select "; }
            set { throw new NotSupportedException("CBISqlSelect.SqlCmd is readonly"); }
        }
    }

    public class CBISqlSelectAll : CBISqlCmdPolicy
    {
        public string SqlCmd
        {
            get { return "select * "; }
            set { throw new NotSupportedException("CBISqlSelect.SqlCmd is readonly"); }
        }
    }

    public class CBISqlSelectFormat<TObject, TFormatter> : CBFormaterStatement<TObject, TFormatter>, CBISqlCmdPolicy
        where TObject : new()
        where TFormatter : CBIStatementFormatter<TObject>, new()
    {
        public string SqlCmd
        {
            get { return "select " + Statement; }
            set { throw new NotSupportedException("CBISqlSelect.SqlCmd is readonly"); }
        }
    }

    public class CBISqlInsert : CBISqlCmdPolicy
    {
        public string SqlCmd
        {
            get { return "insert "; }
            set { throw new NotSupportedException("CBISqlSelect.CBISqlInsert is readonly"); }
        }
    }

    public class CBISqlUpdate : CBISqlCmdPolicy
    {
        public string SqlCmd
        {
            get { return "update "; }
            set { throw new NotSupportedException("CBISqlSelect.CBISqlUpdate is readonly"); }
        }
    }

    public class CBISqlDelete : CBISqlCmdPolicy
    {
        public string SqlCmd
        {
            get { return "delete "; }
            set { throw new NotSupportedException("CBISqlSelect.CBISqlDelete is readonly"); }
        }
    }
}
