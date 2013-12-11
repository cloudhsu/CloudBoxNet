using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudBox.Policy.SQLPolicy
{
    public enum Sex
    {
        Male,
        Female
    }

    public class TestObject : CBIConditionPolicy, CBIStatementPolicy
    {
        public TestObject()
        {
            age = 31;
            name = "Cloud";
            sex = Sex.Male;
        }
        int age;
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        Sex sex;
        public Sex Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        #region CBIConditionPolicy Members

        public string Condition
        {
            get
            {
                return string.Format("where Age={0},Name={1},sex={2}",age,name,sex.ToString());
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region CBIStatementPolicy Members

        public string Statement
        {
            get
            {
                return " from XDTable ";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }

    public class TestFormatter : CBIStatementFormatter<TestObject>
    {

        #region CBIStatementFormatter<TestObject> Members

        public string Formatter(TestObject obj)
        {
            return string.Format(" Age={0},Name={1} from XDTable ", obj.Age + 1, obj.Name);
        }

        #endregion
    }

    public class TestQueryFormatter : CBIStatementFormatter<TestObject>
    {

        #region CBIStatementFormatter<TestObject> Members

        public string Formatter(TestObject obj)
        {
            return string.Format(" Name,Age,Sex ");
        }

        #endregion
    }
}
