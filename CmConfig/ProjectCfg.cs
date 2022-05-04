using System;
using System.IO;
using System.Web;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
namespace Maticsoft.CmConfig
{
	//项目发布配置

	#region 配置对象模型类 ProSettings

	/// <summary>
	/// 项目发布配置
	/// </summary>
	public class ProSettings
	{		
		private string _mode="";
		private string _fileext="";
		private string _fileextdel="";
		private string _sourceDirectory;
		private string _targetDirectory;
				

		/// <summary>
		/// 发布方式
		/// </summary>
		[XmlElement]
		public string Mode
		{
			set{ _mode=value; }
			get{ return _mode; }
		}
		/// <summary>
		/// 筛选法
		/// </summary>
		[XmlElement]
		public string FileExt
		{
			set{ _fileext=value; }
			get{ return _fileext; }
		}
		/// <summary>
		/// 过滤法
		/// </summary>
		[XmlElement]
		public string FileExtDel
		{
			set{ _fileextdel=value; }
			get{ return _fileextdel; }
		}
		/// <summary>
		/// 上次的源路径
		/// </summary>
		[XmlElement]
		public string SourceDirectory
		{
			set{ _sourceDirectory=value; }
			get{ return _sourceDirectory; }
		}
		/// <summary>
		/// 上次的目标路径
		/// </summary>
		[XmlElement]
		public string TargetDirectory
		{
			set{ _targetDirectory=value; }
			get{ return _targetDirectory; }
		}

	}
	#endregion 


	#region  配置的操作类 ProConfig
	/// <summary>
	/// 配置的操作类ModuleConfig。
	/// </summary>
	public class ProConfig
	{

		public static ProSettings GetSettings()
		{			
			ProSettings data = null;
			XmlSerializer serializer = new XmlSerializer(typeof(ProSettings));
			try
			{
				string fileName = "ProConfig.config";				
				FileStream fs = new FileStream(fileName, FileMode.Open);					
				data = (ProSettings)serializer.Deserialize(fs);
				fs.Close();				
			}
			catch
			{	
				data = new ProSettings();
			}
		
			
			return data;
		}


		public static void SaveSettings(ProSettings data)
		{
			string fileName = "ProConfig.config";
			XmlSerializer serializer = new XmlSerializer (typeof(ProSettings));
        
			// serialize the object
			FileStream fs = new FileStream(fileName, FileMode.Create);
			serializer.Serialize(fs, data);
			fs.Close();
		}

		}
	
	#endregion


}
