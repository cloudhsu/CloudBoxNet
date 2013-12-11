using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBox.Policy.SQLPolicy
{
    public class CBSQL : CBIStatementPolicy
    {
        public virtual string Statement { get; set; }
    }

    public class CBSQL<TStatementPolicy> : CBSQL
        where TStatementPolicy : CBIStatementPolicy, new()
    {
        TStatementPolicy statement;
        public CBSQL()
        {
            statement = new TStatementPolicy();
        }
        public override string Statement
        {
            get { return statement.Statement; }
            set { throw new NotSupportedException("CBSQL.Statement is readonly"); }
        }
    }

    public class CBSQL<TSqlCmdPolicy, TStatementPolicy> : CBSQL
        where TSqlCmdPolicy : CBISqlCmdPolicy, new()
        where TStatementPolicy : CBIStatementPolicy, new()
    {
        TSqlCmdPolicy cmd;
        TStatementPolicy statement;
        public CBSQL()
        {
            cmd = new TSqlCmdPolicy();
            statement = new TStatementPolicy();
        }
        public override string Statement
        {
            get { return cmd.SqlCmd + statement.Statement; }
            set { throw new NotSupportedException("CBSQL.Statement is readonly"); }
        }
    }

    public class CBSQL<TSqlCmdPolicy, TStatementPolicy, TConditionPolicy> : CBSQL
        where TSqlCmdPolicy : CBISqlCmdPolicy, new()
        where TStatementPolicy : CBIStatementPolicy, new()
        where TConditionPolicy : CBIConditionPolicy, new()
    {
        TSqlCmdPolicy cmd;
        TStatementPolicy statement;
        TConditionPolicy condition;
        public CBSQL()
        {
            cmd = new TSqlCmdPolicy();
            statement = new TStatementPolicy();
            condition = new TConditionPolicy();
        }
        public override string Statement
        {
            get
            {
                return cmd.SqlCmd + statement.Statement + condition.Condition;
            }
            set { throw new NotSupportedException("CBSQL.Statement is readonly"); }
        }
    }
}
