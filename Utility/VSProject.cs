using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
namespace Maticsoft.Utility
{
    /// <summary>
    /// vs项目文件修改
    /// </summary>
    public class VSProject
    {

        #region 修改项目文件

        /// <summary>
        /// 自动选择类型进行添加
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="classname"></param>
        public void AddClass(string filename, string classname)
        {
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            string name = doc.DocumentElement.FirstChild.Name;
            switch (name)            
            {
                case "CSHARP":
                    AddClass2003(filename, classname);
                    break;
                case "PropertyGroup":
                    AddClass2005(filename, classname);
                    break;         
                default:
                    break;
            }
        }

        /// <summary>
        /// vs2003 格式项目追加文件
        /// </summary>
        /// <param name="filename">项目文件名</param>
        /// <param name="classname">类文件名</param>
        public void AddClass2003(string filename, string classname)
        {
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            foreach (XmlElement xmlFirstElement in doc.DocumentElement.ChildNodes)//检索根结点
            {
                foreach (XmlElement xmlSecondElement in xmlFirstElement)
                {
                    if (xmlSecondElement.Name == "Files")
                    {
                        foreach (XmlElement xmlThreeElement in xmlSecondElement)
                        {
                            if (xmlThreeElement.Name == "Include")
                            {
                                XmlElement elem = doc.CreateElement("File", doc.DocumentElement.NamespaceURI);
                                elem.SetAttribute("RelPath", classname);
                                elem.SetAttribute("SubType", "Code");
                                elem.SetAttribute("BuildAction", "Compile");
                                xmlThreeElement.AppendChild(elem);
                                break;
                            }
                        }
                    }
                }
            }
            doc.Save(filename);
        }


        /// <summary>
        /// vs2005格式项目追加文件
        /// </summary>
        /// <param name="filename">项目文件名</param>
        /// <param name="classname">类文件名</param>
        public void AddClass2005(string filename, string classname)
        {
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            foreach (XmlElement xmlFirstElement in doc.DocumentElement.ChildNodes)//检索根结点
            {
                if (xmlFirstElement.Name == "ItemGroup")
                {
                    string code = xmlFirstElement.ChildNodes[0].InnerText; //xmlFirstElement.InnerText;
                    string type = xmlFirstElement.ChildNodes[0].Name;
                    if (type == "Compile") 
                    {
                        XmlElement elem = doc.CreateElement("Compile", doc.DocumentElement.NamespaceURI);
                        elem.SetAttribute("Include", classname);
                        xmlFirstElement.AppendChild(elem);
                        break;
                    }
                }
            }
            doc.Save(filename);
        }

        /// <summary>
        /// vs2008格式项目追加文件
        /// </summary>
        /// <param name="filename">项目文件名</param>
        /// <param name="classname">类文件名</param>
        public void AddClass2008(string filename, string classname)
        {
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            foreach (XmlElement xmlFirstElement in doc.DocumentElement.ChildNodes)//检索根结点
            {
                if (xmlFirstElement.Name == "ItemGroup")
                {
                    string code = xmlFirstElement.ChildNodes[0].InnerText; //xmlFirstElement.InnerText;
                    string type = xmlFirstElement.ChildNodes[0].Name;
                    if (type == "Compile")
                    {
                        XmlElement elem = doc.CreateElement("Compile", doc.DocumentElement.NamespaceURI);
                        elem.SetAttribute("Include", classname);
                        xmlFirstElement.AppendChild(elem);
                        break;
                    }
                }
            }
            doc.Save(filename);
        }

        /// <summary>
        /// vs2005格式项目追加文件
        /// </summary>
        /// <param name="filename">项目文件名</param>
        /// <param name="classname">类文件名</param>
        public void AddClass2005Aspx(string filename, string aspxname)
        {
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            foreach (XmlElement xmlFirstElement in doc.DocumentElement.ChildNodes)//检索根结点
            {
                if (xmlFirstElement.Name == "ItemGroup")
                {
                    string code = xmlFirstElement.ChildNodes[0].InnerText; //xmlFirstElement.InnerText;
                    string type = xmlFirstElement.ChildNodes[0].Name;
                    if (type == "Compile")
                    {
                        XmlElement elem = doc.CreateElement("Compile", doc.DocumentElement.NamespaceURI);
                        elem.SetAttribute("Include", aspxname);
                        xmlFirstElement.AppendChild(elem);
                        break;
                    }
                }
            }
            doc.Save(filename);
        }
        #endregion

        #region 向类文件增加方法

        /// <summary>
        /// 向类文件增加方法
        /// </summary>
        /// <param name="ClassFile">类文件</param>
        /// <param name="strContent">方法内容</param>
        public void AddMethodToClass(string ClassFile, string strContent)
        {
            if (File.Exists(ClassFile))
            {
                string strcontent = File.ReadAllText(ClassFile, Encoding.Default);
                if (strcontent.IndexOf(" class ") > 0)
                {
                    int n1 = strcontent.LastIndexOf("}");
                    string temp=strcontent.Substring(0,n1-1);
                    int n2 = temp.LastIndexOf("}");

                    string sss = strcontent.Substring(0, n2 - 1);

                    string lastStr = sss + "\r\n" + strContent + "\r\n" + "}" + "\r\n" + "}";
                    
                    StreamWriter sw = new StreamWriter(ClassFile, false, Encoding.Default);
                    sw.Write(lastStr);
                    sw.Flush();
                    sw.Close();
                }
            }
        }
        #endregion



    }
}
