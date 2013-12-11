using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudBox.Log;
using CloudBox.IO;
using CloudBox.Policy.NamePolicy;
using CloudBox.Policy.NamePolicy.FileNamePolicy;
using CloudBox.Policy.SQLPolicy;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] a = {1,2,3};

            try
            {
                CBLog.LogDebug("123");
                CBLog.LogInfo("123");
                CBLog.LogError("123");
                a[5] = 1;
            }
            catch (System.Exception ex)
            {
                CBLog.LogDebug(ex);
                CBLog.LogInfo(ex);
                CBLog.LogError(ex);
            }

            CBLog.LogType = CBLogType.LogTextFile;

            try
            {
                CBLog.LogDebug("123");
                CBLog.LogInfo("123");
                CBLog.LogError("123");
                a[5] = 1;
            }
            catch (System.Exception ex)
            {
                CBLog.LogDebug(ex);
                CBLog.LogInfo(ex);
                CBLog.LogError(ex);
            }

            //CBLog.LogType = CBLogType.LogXMLFile;

            try
            {
                CBLog.LogDebug("123");
                CBLog.LogInfo("123");
                CBLog.LogError("123");
                a[5] = 1;
            }
            catch (System.Exception ex)
            {
                CBLog.LogDebug(ex);
                CBLog.LogInfo(ex);
                CBLog.LogError(ex);
            }

            CBLog.LogType = CBLogType.LogConsole;

            CBFileName name1 =
                new CBFileName<CBFileNamePolicy, CBTextExtensionPolicy>();
            name1.Name = "Test1";
            CBLog.LogInfo(name1.Name);

            CBFileName name2 =
                new CBFileName<CBFullFileNamePolicy, CBXmlExtensionPolicy>();
            name2.Name = "Test2";
            CBLog.LogInfo(name2.Name);

            CBFileName nameNormalOld =
                new CBFileName<CBPreDateNowNamePolicy, CBLogExtensionPolicy>();
            nameNormalOld.Name = "Log\\Event_Normal_";
            CBLog.LogInfo(nameNormalOld.Name);

            CBFileName nameDebugOld =
                new CBFileName<CBPreDateNowNamePolicy, CBLogExtensionPolicy>();
            nameDebugOld.Name = "Log\\Event_Debug_";
            CBLog.LogInfo(nameDebugOld.Name);

            CBFileName nameNormal =
                new CBFileName<CBFullPreDateNowNamePolicy, CBFileNumSplitPolicy, CBLogExtensionPolicy>();
            nameNormal.Name = "Log\\Event_Normal_";
            CBLog.LogInfo(nameNormal.Name);

            CBFileName nameDebug =
                new CBFileName<CBPreDateNowNamePolicy, CBFileNumSplitPolicy, CBLogExtensionPolicy>();
            nameDebug.Name = "Log\\Event_Debug_";
            CBLog.LogInfo(nameDebug.Name);

            CBFileName name4 =
                new CBFileName<CBFullDateNowNamePolicy, CBFileNumSplitPolicy, CBXmlExtensionPolicy>();
            CBLog.LogInfo(name4.Name);

            CBFileNameTest nametest1 = new CBFileNameTest();
            nametest1.PolicyRole = CBFilePolicyRole.NameAndExtension;
            nametest1.FileNamePolicy = new CBFullDateNowNamePolicy();
            nametest1.FileNameControlPolicy = new CBFileNumSplitPolicy();
            nametest1.FileNameExtensionPolicy = new CBXmlExtensionPolicy();

            //Apple apple1 = new Apple();
            //Apple apple2 = new Apple(apple1);
            //Banana banana1 = new Banana();
            //Banana banana2 = new Banana(apple2);

            //CBLog.LogInfo(apple1.getCost().ToString());
            //CBLog.LogInfo(apple2.getCost().ToString());
            //CBLog.LogInfo(banana1.getCost().ToString());
            //CBLog.LogInfo(banana2.getCost().ToString());
                    
            CBSQL<CBISqlInsert, TestObject> sql1
                = new CBSQL<CBISqlInsert, TestObject>();
            CBLog.LogInfo(sql1.Statement);
            CBSQL<CBISqlDelete, TestObject> sql2
                = new CBSQL<CBISqlDelete, TestObject>();
            CBLog.LogInfo(sql2.Statement);
            CBSQL<CBISqlUpdate, CBFormaterStatement<TestObject, TestFormatter>, TestObject> sql3
                = new CBSQL<CBISqlUpdate, CBFormaterStatement<TestObject, TestFormatter>, TestObject>();
            CBLog.LogInfo(sql3.Statement);

            CBSQL<CBISqlSelectAll, TestObject> sql4
                = new CBSQL<CBISqlSelectAll, TestObject>();
            CBLog.LogInfo(sql4.Statement);

            CBSQL<CBISqlSelectFormat<TestObject, TestQueryFormatter>, TestObject> sql5
                = new CBSQL<CBISqlSelectFormat<TestObject, TestQueryFormatter>, TestObject>();
            CBLog.LogInfo(sql5.Statement);

            CBLog.LogInfo(nametest1.Name);

            Console.ReadLine();
            //CBLog.ReleaseLog();
        }
    }
}
