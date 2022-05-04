using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;

namespace Maticsoft.CmConfig
{
    /// <summary>
    /// 数据字段类型映射
    /// </summary>
    public static class DatatypeMap
    {
        #region LoadFromCfg

        public static Hashtable LoadFromCfg(XmlDocument doc, string TypeName)
        {
            try
            {
                Hashtable list = new Hashtable();
                XmlNode xmldata = doc.SelectSingleNode("Map/" + TypeName);
                if (xmldata != null)
                {
                    foreach (XmlNode node in xmldata.ChildNodes)
                    {
                        list.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                    }
                }
                return list;
            }
            catch (System.Exception ex)
            {
                throw new Exception("Load DatatypeMap file fail:" + ex.Message);
            }
        }
        public static Hashtable LoadFromCfg(string filename,string xpathTypeName)
        {
            try
            {                
                Hashtable list = new Hashtable();
                XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(filename);
                XmlNode xmldata = doc.SelectSingleNode("Map/" + xpathTypeName);
                if (xmldata != null)
                {
                    foreach (XmlNode node in xmldata.ChildNodes)
                    {
                        //list.Add(xmldata.Item(n).Attributes["key"].Value, xmldata.Item(n).Attributes["value"].Value);
                        list.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                    }
                }
                return list;
            }
            catch (System.Exception ex)
            {
                throw new Exception("Load DatatypeMap file fail:" + ex.Message);
            }
        }

        public static string GetValueFromCfg(string filename, string xpathTypeName,string Key)
        {
            try
            {
                Hashtable list = new Hashtable();
                XmlDocument doc = new System.Xml.XmlDocument();
                doc.Load(filename);
                XmlNode xmldata = doc.SelectSingleNode("Map/" + xpathTypeName);
                if (xmldata != null)
                {
                    foreach (XmlNode node in xmldata.ChildNodes)
                    {                        
                        list.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                    }
                }
                object objkey=list[Key];
                if(objkey!=null)
                {
                    return objkey.ToString();
                }
                else
                {
                    return "";
                }
                
            }
            catch (System.Exception ex)
            {
                throw new Exception("Load DatatypeMap file fail:" + ex.Message);
            }
        }

        #endregion


        #region SaveCfg

        public static bool SaveCfg(string filename,string NodeText,Hashtable list)
        {
            try
            {
               
                XmlDocument xmldoc = new XmlDocument();
                XmlNode xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                xmldoc.AppendChild(xmlnode);

                XmlElement root = xmldoc.CreateElement("", NodeText, "");
                xmldoc.AppendChild(root);
                foreach (DictionaryEntry de in list)
                {
                    XmlElement xml = xmldoc.CreateElement("", NodeText, "");

                    XmlAttribute xmlKey = xmldoc.CreateAttribute("key");
                    xmlKey.Value = de.Key.ToString();
                    xml.Attributes.Append(xmlKey);

                    XmlAttribute xmlValue = xmldoc.CreateAttribute("value");
                    xmlValue.Value = de.Value.ToString();
                    xml.Attributes.Append(xmlValue);
                    root.AppendChild(xml);        
                }
                
                xmldoc.Save(filename);
                return true;
            }
            catch (System.Exception ex)
            {
                throw new Exception("Save DatatypeMap file fail:" + ex.Message);                
            }
        }

        #endregion
    }
}
