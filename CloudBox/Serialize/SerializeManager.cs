/*
* Copyright (c) 2011, Cloud Hsu
* All rights reserved.
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither the name of the Cloud Hsu nor the
*       names of its contributors may be used to endorse or promote products
*       derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY CLOUD HSU "AS IS" AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL THE REGENTS AND CONTRIBUTORS BE LIABLE FOR ANY
* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using CloudBox.DesignPatterns;
using CloudBox.Log;

namespace CloudBox.Serialize
{

    using SerializePool = TDictionaryPool<_Serializable>;
    using System.Xml;
    
    /// <summary>
    /// Serialize manager.
    /// The requirement of serializable object.
    /// 1. Serializable Class which need [Serializable] attribute or implement ISerializable.
    /// 2. Serializable Class must be declared public level.
    /// 3. Only public read/write property can serialize.
    /// 4. Need a default constructor.
    /// 5. You can do List.Add in default contruator.
    /// </summary>
    public sealed class SerializeManager
    {
        /// <summary>
        /// Initialize SerializeManager once
        /// Using reflection factory to create object
        /// </summary>
        static SerializeManager()
        {
            SerializePool.RegisterFactory(new TReflectionFactory<_Serializable>());
        }

        /// <summary>
        /// not use, disable new
        /// </summary>
        private SerializeManager() { }

        /// <summary>
        /// Object serialize to XML string.
        /// </summary>
        /// <param name="a_pObject">Object which need [Serializable] attribute or implement ISerializable</param>
        /// <returns>XML string</returns>
        public static String SerializeToXml(Object a_pObject)
        {
            Type type = a_pObject.GetType();
            _Serializable serialize = SerializePool.New(type.FullName, type);
            //MemoryStream ms = new MemoryStream();
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.OmitXmlDeclaration = true;
            StringWriter stringWriter = new StringWriter();
            //using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter,
            //writerSettings))
            //{
            //    serializer.Serialize(xmlWriter, request);
            //}
            XmlSerializerNamespaces myNameSpc = new XmlSerializerNamespaces();
            myNameSpc.Add("", "");
            try
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter,
            writerSettings))
                {
                    serialize.Fomatter.Serialize(xmlWriter, a_pObject, myNameSpc);
                    //ms.Seek(0, SeekOrigin.Begin);
                }
            }
            catch (Exception ex)
            {
                CBLog.LogDebug(ex);
            }
            finally
            {
                SerializePool.Delete(type.FullName, serialize);
            }
            //ASCIIEncoding t_pEncoding = new ASCIIEncoding();
            //String t_sContect = t_pEncoding.GetString(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            string t_sContect = stringWriter.ToString();
            return t_sContect;
        }

        /// <summary>
        /// Deserialize XML data to object
        /// </summary>
        /// <param name="a_pType">object type</param>
        /// <param name="a_sContent">XML data</param>
        /// <returns>A type of object cast to object</returns>
        public static Object XmlDeserialize(Type a_pType, String a_sXMLContent)
        {
            ASCIIEncoding t_pEncoding = new ASCIIEncoding();
            MemoryStream ms = new MemoryStream(t_pEncoding.GetBytes(a_sXMLContent));
            _Serializable serialize = SerializePool.New(a_pType.FullName, a_pType);
            Object t_pData = null;
            try
            {
                t_pData = serialize.Fomatter.Deserialize(ms);
            }
            catch (Exception ex)
            {
                CBLog.LogDebug(ex);
            }
            finally
            {
                SerializePool.Delete(a_pType.FullName, serialize);
            }
            return t_pData;
        }
    }

    /// <summary>
    /// Using for pool, for SerializeManager only.
    /// </summary>
    internal sealed class _Serializable : IPoolable
    {
        XmlSerializer fomatter;
        public XmlSerializer Fomatter
        {
            get { return fomatter; }
            set { fomatter = value; }
        }
        public _Serializable() { }

        #region IPoolable Members

        public void Create()
        {
            // nothing to do
        }

        public void Create(params object[] args)
        {
            fomatter = new XmlSerializer(args[0] as Type);
        }

        public void Initialize()
        {
            // nothing to do
        }

        public void Release()
        {
            // nothing to do
        }

        #endregion
    }
}
