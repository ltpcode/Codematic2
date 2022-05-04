using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace VSProject
{
    public class SolutionManager
    {
        private string XMLCsproj;//用来存放类库文件

        public SolutionManager(string InXMlCsprojPathth) //外部传入的工程文件的路径存起来
        {
            this.XMLCsproj = InXMlCsprojPathth;
        }

        /// <summary>
        /// 转换项目文件为xml
        /// </summary>
        public void SaveXml()
        {
            System.IO.StreamReader obj_strRead = System.IO.File.OpenText(this.XMLCsproj);
            System.IO.StreamWriter obj_strWrite = System.IO.File.CreateText("C://aaa.xml");
            obj_strWrite.Write(obj_strRead.ReadToEnd());
            obj_strRead.Close();
            obj_strWrite.Close();

        }
        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <returns></returns>
        public VisualStudioProject ExcludeCsproj()
        {
            VisualStudioProject obj_visualStudioProject = new VisualStudioProject();
            System.Xml.XmlDocument doc = new XmlDocument();
            this.SaveXml();
            doc.Load("c://aaa.xml");
            foreach (XmlElement xmlFirstElement in doc.DocumentElement.ChildNodes)//检索根结点
            {
                obj_visualStudioProject.LastOpenVersion = xmlFirstElement.Attributes["LastOpenVersion"].InnerXml.ToString();//把根节点的第一个子节点的属性LastOpenVersion存起来
                foreach (XmlElement xmlSecondElement in xmlFirstElement)//检索第一个子节点的所有子节点
                {
                    if (xmlSecondElement.Name == "Build")//如果该节点的名字为Build，
                    {
                        foreach (XmlElement xmlThirdElement in xmlSecondElement) //检索Build下面的子节点
                        {
                            if (xmlThirdElement.Name == "Settings")//如果节点名为settings
                            {
                                obj_visualStudioProject.ReferencePath = xmlThirdElement.Attributes["ReferencePath"].InnerXml.ToString();//把相应的属性存起来
                                int i = xmlThirdElement.ChildNodes.Count; //把settings节点的子节点总数存给i，因为这里有多个config，我们要把它存到一个类组里面，所以得先知道他几个节点数
                                Config[] obj_config = new Config[i];
                                int j = 0;
                                foreach (XmlElement xmlFourElement in xmlThirdElement) //检索settings的所有子节点
                                {
                                    if (xmlFourElement.Name == "Config")
                                    {
                                        obj_config[j] = new Config(); //分配内存。把相应的属性值存起来
                                        obj_config[j].Name = xmlFourElement.Attributes["Name"].InnerXml.ToString();
                                        obj_config[j].EnableASPDebugging = xmlFourElement.Attributes["EnableASPDebugging"].InnerXml.ToString();
                                        obj_config[j].EnableASPXDebugging = xmlFourElement.Attributes["EnableASPXDebugging"].InnerXml.ToString();
                                        obj_config[j].EnableSQLServerDebugging = xmlFourElement.Attributes["EnableSQLServerDebugging"].InnerXml.ToString();
                                        obj_config[j].EnableUnmanagedDebugging = xmlFourElement.Attributes["EnableUnmanagedDebugging"].InnerXml.ToString();
                                        obj_config[j].RemoteDebugEnabled = xmlFourElement.Attributes["RemoteDebugEnabled"].InnerXml.ToString();
                                        obj_config[j].RemoteDebugMachine = xmlFourElement.Attributes["RemoteDebugMachine"].InnerXml.ToString();
                                        obj_config[j].StartAction = xmlFourElement.Attributes["StartAction"].InnerXml.ToString();
                                        obj_config[j].StartArguments = xmlFourElement.Attributes["StartArguments"].InnerXml.ToString();
                                        obj_config[j].StartPage = xmlFourElement.Attributes["StartPage"].InnerXml.ToString();
                                        obj_config[j].StartProgram = xmlFourElement.Attributes["StartProgram"].InnerXml.ToString();
                                        obj_config[j].StartURL = xmlFourElement.Attributes["StartURL"].InnerXml.ToString();
                                        obj_config[j].StartWorkingDirectory = xmlFourElement.Attributes["StartWorkingDirectory"].InnerXml.ToString();
                                        obj_config[j].StartWithIE = xmlFourElement.Attributes["StartWithIE"].InnerXml.ToString();
                                        j++;
                                    }
                                }
                                obj_visualStudioProject.ConfigObject = obj_config;
                            }
                        }
                    }
                    else if (xmlSecondElement.Name == "OtherProjectSettings")  //判断节点是否是制定节点
                    {
                        OtherProjectSettings obj_OtherProjectSettings = new OtherProjectSettings();
                        obj_OtherProjectSettings.CopyProjectDestinationFolder = xmlSecondElement.Attributes["CopyProjectDestinationFolder"].InnerXml.ToString();
                        obj_OtherProjectSettings.CopyProjectOption = xmlSecondElement.Attributes["CopyProjectOption"].InnerXml.ToString();
                        obj_OtherProjectSettings.CopyProjectUncPath = xmlSecondElement.Attributes["CopyProjectUncPath"].InnerXml.ToString();
                        obj_OtherProjectSettings.ProjectTrust = xmlSecondElement.Attributes["ProjectTrust"].InnerXml.ToString();
                        obj_OtherProjectSettings.ProjectView = xmlSecondElement.Attributes["ProjectView"].InnerXml.ToString();

                        obj_visualStudioProject.OtherProjectSettingsObject = obj_OtherProjectSettings;

                    }
                }
            }

            return obj_visualStudioProject;
        }

        /// <summary>
        /// 保存项目文件
        /// </summary>
        /// <param name="obj_VisualStudioProject"></param>
        public void SaveCsproj(VisualStudioProject obj_VisualStudioProject)
        {

            System.Xml.XmlWriter myXMLWrite = new XmlTextWriter(this.XMLCsproj, System.Text.Encoding.UTF8);
            myXMLWrite.WriteStartDocument(true);
            myXMLWrite.WriteStartElement("VisualStudioProject");
            myXMLWrite.WriteStartElement("CSHARP");
            myXMLWrite.WriteAttributeString("LastOpenVersion", obj_VisualStudioProject.LastOpenVersion);
            myXMLWrite.WriteStartElement("Build");
            myXMLWrite.WriteStartElement("Settings");
            myXMLWrite.WriteAttributeString("ReferencePath", obj_VisualStudioProject.ReferencePath);
            foreach (Config obj_config in obj_VisualStudioProject.ConfigObject)
            {
                myXMLWrite.WriteStartElement("Config");
                myXMLWrite.WriteAttributeString("Name", obj_config.Name);
                myXMLWrite.WriteAttributeString("EnableASPDebugging", obj_config.EnableASPDebugging);
                myXMLWrite.WriteAttributeString("EnableASPXDebugging", obj_config.EnableASPXDebugging);
                myXMLWrite.WriteAttributeString("EnableUnmanagedDebugging", obj_config.EnableUnmanagedDebugging);
                myXMLWrite.WriteAttributeString("EnableSQLServerDebugging", obj_config.EnableSQLServerDebugging);
                myXMLWrite.WriteAttributeString("RemoteDebugEnabled", obj_config.RemoteDebugEnabled);
                myXMLWrite.WriteAttributeString("RemoteDebugMachine", obj_config.RemoteDebugMachine);
                myXMLWrite.WriteAttributeString("StartAction", obj_config.StartAction);
                myXMLWrite.WriteAttributeString("StartArguments", obj_config.StartArguments);
                myXMLWrite.WriteAttributeString("StartPage", obj_config.StartPage);
                myXMLWrite.WriteAttributeString("StartProgram", obj_config.StartProgram);
                myXMLWrite.WriteAttributeString("StartURL", obj_config.StartURL);
                myXMLWrite.WriteAttributeString("StartWorkingDirectory", obj_config.StartWorkingDirectory);
                myXMLWrite.WriteAttributeString("StartWithIE", obj_config.StartWithIE);
                myXMLWrite.WriteEndElement();
            }
            myXMLWrite.WriteEndElement();
            myXMLWrite.WriteEndElement();
            myXMLWrite.WriteStartElement("OtherProjectSettings");
            myXMLWrite.WriteAttributeString("CopyProjectDestinationFolder", obj_VisualStudioProject.OtherProjectSettingsObject.CopyProjectDestinationFolder);
            myXMLWrite.WriteAttributeString("CopyProjectOption", obj_VisualStudioProject.OtherProjectSettingsObject.CopyProjectOption);
            myXMLWrite.WriteAttributeString("CopyProjectUncPath", obj_VisualStudioProject.OtherProjectSettingsObject.CopyProjectUncPath);
            myXMLWrite.WriteAttributeString("ProjectView", obj_VisualStudioProject.OtherProjectSettingsObject.ProjectView);
            myXMLWrite.WriteAttributeString("ProjectTrust", obj_VisualStudioProject.OtherProjectSettingsObject.ProjectTrust);
            myXMLWrite.WriteEndElement();
            myXMLWrite.WriteEndElement();
            myXMLWrite.WriteEndElement();
            myXMLWrite.WriteEndDocument();
            myXMLWrite.Close();

        }





    }
}
