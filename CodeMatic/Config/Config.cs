using System;
using System.IO;
using System.Web;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Windows.Forms;
namespace Codematic
{
	//主程序配置

	#region 配置对象模型类 ModuleSettings

	/// <summary>
	/// 配置的modul类（注意相关属性的类型与[XmlElement]）
	/// use:ModuleSettings settings=ModuleConfig.GetSettings();
	/// </summary>
	public class ModuleSettings
	{        
        private string _procprefix;
        private string _projectname;
		private string _namespace="Maticsoft";
		private string _folder="Folder";
		private string _appframe="f3";//简单三层(s3)，复杂三层(f3)，自定义(custom)
		private string _daltype="sql";//直接写sql语句(sql)，还是调用存储过程(Proc);
        private string _blltype = "sql";//直接写sql语句(sql)，还是调用存储过程(Proc);
		private string _editfont="新宋体";
		private float _editfontsize=9;
        private string _dbHelperName = "DbHelperSQL";
      		
        
        /// <summary>
        /// 存储过程前缀 
        /// </summary>
        [XmlElement]
        public string ProcPrefix
        {
            set { _procprefix = value; }
            get { return _procprefix; }
        }
        /// <summary>
        /// 项目名称 
        /// </summary>
        [XmlElement]
        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }
		/// <summary>
		/// 默认顶级命名空间名
		/// </summary>
		[XmlElement]
		public string Namepace
		{
            set { _namespace = value; }
            get { return _namespace; }
		}
		/// <summary>
		/// 默认业务逻辑所在的文件夹
		/// </summary>
		[XmlElement]
		public string Folder
		{
			set{ _folder=value; }
			get{ return _folder; }
		}
		/// <summary>
		/// 采用的架构
		/// </summary>
		[XmlElement]
		public string AppFrame
		{
			set{ _appframe=value; }
			get{ return _appframe; }
		}
		/// <summary>
		/// 数据层类型 
		/// </summary>
		[XmlElement]
		public string DALType
		{
			set{ _daltype=value; }
			get{ return _daltype; }
		}
        /// <summary>
        /// 业务层类型 
        /// </summary>
        [XmlElement]
        public string BLLType
        {
            set { _blltype = value; }
            get { return _blltype; }
        }
		/// <summary>
		/// 当前编辑器使用的字体名
		/// </summary>
		[XmlElement]
		public string EditFont
		{
			set{ _editfont=value; }
			get{ return _editfont; }
		}
		/// <summary>
		/// 当前编辑器使用的字体的大小
		/// </summary>
		[XmlElement]
		public float EditFontSize
		{
			set{ _editfontsize=value; }
			get{ return _editfontsize; }
		}
        /// <summary>
        /// 数据访问类名 
        /// </summary>
        [XmlElement]
        public string DbHelperName
        {
            set { _dbHelperName = value; }
            get { return _dbHelperName; }
        }
 
               
		
	}
	#endregion 


	#region  配置的操作类 ModuleConfig

	/// <summary>
	/// 配置的操作类ModuleConfig。
	/// </summary>
	public class ModuleConfig
	{        		
		public static ModuleSettings GetSettings()
		{			
			ModuleSettings data = null;
			XmlSerializer serializer = new XmlSerializer(typeof(ModuleSettings));
			try
			{
				string apppath=Application.StartupPath;
				string fileName = apppath+"\\config.xml";
				FileStream fs = new FileStream(fileName, FileMode.Open);					
				data = (ModuleSettings)serializer.Deserialize(fs);
				fs.Close();				
			}
			catch
			{	
				data = new ModuleSettings();
			}			
			return data;
		}
        
		public static void SaveSettings(ModuleSettings data)
		{
			string apppath=Application.StartupPath;
			string fileName = apppath+"\\config.xml";
			XmlSerializer serializer = new XmlSerializer (typeof(ModuleSettings));
        
			// serialize the object
			FileStream fs = new FileStream(fileName, FileMode.Create);
			serializer.Serialize(fs, data);
			fs.Close();
		}
	}

	#endregion



}
