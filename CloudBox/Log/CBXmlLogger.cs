using System.IO;
using System.Xml;
using CloudBox.Policy.NamePolicy.FileNamePolicy;
using CloudBox.Serialize;

namespace CloudBox.Log
{
    class CBXmlLogger : CBTextLogger
    {
        const string ROOT = "LogRoot";

        public CBXmlLogger()
            : this(LOG_PERNAME)
        {
        }

        protected CBXmlLogger(string fileName)
        {
            m_FileName = 
                new CBFileName<CBFullPreDateNowNamePolicy, CBFileTextSplitPolicy, CBXmlExtensionPolicy>();
            m_FileName.Name = fileName;
        }

        public override void Log(CBLogInfo logInfo)
        {
            string log = SerializeManager.SerializeToXml(logInfo);
            XmlDocument doc = new XmlDocument();
            XmlNode root = null;
            if(!File.Exists(m_FileName.Name))
            {
                root = doc.CreateNode(XmlNodeType.Element, ROOT, "");
                doc.AppendChild(root);
            }
            else
            {
                doc.Load(m_FileName.Name);
                root = doc.SelectSingleNode(ROOT);
            }
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(log));
            XmlReader reader = XmlReader.Create(new StringReader(log));
            XmlNode logNode = doc.ReadNode(reader);
            root.AppendChild(logNode);
            doc.Save(m_FileName.Name);
        }
    }
}
