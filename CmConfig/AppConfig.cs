using System;
using System.IO;
using System.Web;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Windows.Forms;
namespace Maticsoft.CmConfig
{
    //主程序配置

    #region 配置对象模型类 ModuleSettings

    /// <summary>
    /// 配置的modul类（注意相关属性的类型与[XmlElement]）
    /// use:AppSettings settings=AppConfig.GetSettings();
    /// </summary>
    public class AppSettings
    {
        private string _appstart;
        private string _startuppage;
        private string _homepage;
        private string _templatefolder = "Template";
        private bool _setup=false;
        private string _language = "zh-cn";
        
        /// <summary>
        /// 应用程序启动时 startuppage   blank   homepage
        /// </summary>
        [XmlElement]
        public string AppStart
        {
            set { _appstart = value; }
            get { return _appstart; }
        }
        /// <summary>
        /// 起始页rss地址
        /// </summary>
        [XmlElement]
        public string StartUpPage
        {
            set { _startuppage = value; }
            get { return _startuppage; }
        }
        /// <summary>
        /// 首页url地址
        /// </summary>
        [XmlElement]
        public string HomePage
        {
            set { _homepage = value; }
            get { return _homepage; }
        } 
        /// <summary>
        /// 代码模版存放目录
        /// </summary>
        [XmlElement]
        public string TemplateFolder
        {
            set { _templatefolder = value; }
            get { return _templatefolder; }
        }
        /// <summary>
        /// 是否发送安装信息
        /// </summary>
        [XmlElement]
        public bool Setup
        {
            set { _setup = value; }
            get { return _setup; }
        }
        /// <summary>
        /// Language
        /// </summary>
        [XmlElement]
        public string Language
        {
            set { _language = value; }
            get { return _language; }
        }
    }
    #endregion


    #region  配置的操作类 ModuleConfig

    /// <summary>
    /// 配置的操作类ModuleConfig。
    /// </summary>
    public class AppConfig
    {
        public static AppSettings GetSettings()
        {
            AppSettings data = null;
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            try
            {
                string apppath = Application.StartupPath;
                string fileName = apppath + "\\appconfig.config";
                FileStream fs = new FileStream(fileName, FileMode.Open);
                data = (AppSettings)serializer.Deserialize(fs);
                fs.Close();
            }
            catch
            {
                data = new AppSettings();
            }
            return data;
        }

        public static void SaveSettings(AppSettings data)
        {
            try
            {
                string apppath = Application.StartupPath;
                string fileName = apppath + "\\appconfig.config";
                XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));

                // serialize the object
                FileStream fs = new FileStream(fileName, FileMode.Create);
                serializer.Serialize(fs, data);
                fs.Close();
            }
            catch
            { }
        }
    }

    #endregion



}
