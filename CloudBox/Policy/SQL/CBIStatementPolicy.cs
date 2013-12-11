using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBox.Policy.SQLPolicy
{
    public interface CBIStatementPolicy
    {
        string Statement { get; set; }
    }

    public class CBFormaterStatement<TObject,TFormatter> : CBIStatementPolicy
        where TObject : new()
        where TFormatter : CBIStatementFormatter<TObject>, new()
    {
        TObject obj;
        TFormatter formatter;
        public CBFormaterStatement()
        {
            obj = new TObject();
            formatter = new TFormatter();
            Formatter();
        }
        #region CBIStatementPolicy Members

        public string Statement { get; set; }

        #endregion

        #region CBIFormatStatement<TObject,TFormatter> Members

        void Formatter()
        {
            Statement = formatter.Formatter(obj);
        }

        #endregion
    }
}
