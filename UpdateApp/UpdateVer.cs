using System;
using System.IO;
using System.Web;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Windows.Forms;
namespace UpdateApp
{
    //主程序配置

    #region 配置对象模型类 UpdateSettings

    /// <summary>
    /// 配置的modul类
    /// </summary>
    public class UpdateSettings
    {
        private string _version="1.0";
        private string _description;
        private string _serverurl;

        /// <summary>
        /// 当前软件版本 
        /// </summary>
        [XmlElement]
        public string Version
        {
            set { _version = value; }
            get { return _version; }
        }
        /// <summary>
        /// 升级描述
        /// </summary>
        [XmlElement]
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 升级服务器地址
        /// </summary>
        [XmlElement]
        public string ServerUrl
        {
            set { _serverurl = value; }
            get { return _serverurl; }
        }

    }
    #endregion


    #region  配置的操作类UpdateConfig
    /// <summary>
    /// 配置的操作类ModuleConfig。
    /// </summary>
    public class UpdateConfig
    {
        public static UpdateSettings GetSettings()
        {
            UpdateSettings data = null;
            XmlSerializer serializer = new XmlSerializer(typeof(UpdateSettings));
            try
            {
                string fileName = Application.StartupPath + @"\UpdateVer.xml";
                FileStream fs = new FileStream(fileName, FileMode.Open);
                data = (UpdateSettings)serializer.Deserialize(fs);
                fs.Close();
            }
            catch
            {
                data = new UpdateSettings();
            }
            return data;
        }
        public static void SaveSettings(UpdateSettings data)
        {
            string fileName = Application.StartupPath + @"\UpdateVer.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(UpdateSettings));

            // serialize the object
            FileStream fs = new FileStream(fileName, FileMode.Create);
            serializer.Serialize(fs, data);
            fs.Close();
        }
    }

    #endregion

}
