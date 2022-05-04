using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Data;
using Maticsoft.Utility;
namespace Maticsoft.AddInManager
{
    /// <summary>
    /// 插件配置管理
    /// </summary>
    public class AddIn
    {
        string fileAddin = Application.StartupPath + "\\CodeDAL.addin";
        Cache cache = new Cache();
        #region 属性
        private string _guid;
        private string _name;
        private string _desc;
        private string _assembly;
        private string _classname;
        private string _version;

        public string Guid
        {
            set { _guid = value; }
            get { return _guid; }
        }
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        public string Decription
        {
            set { _desc = value; }
            get { return _desc; }
        }
        public string Assembly
        {
            set { _assembly = value; }
            get { return _assembly; }
        }
        public string Classname
        {
            set { _classname = value; }
            get { return _classname; }
        }
        public string Version
        {
            set { _version = value; }
            get { return _version; }
        }

        #endregion
        public AddIn()
        {
        }
        /// <summary>
        /// 构造一个插件信息
        /// </summary>
        /// <param name="AssemblyGuid"></param>
        public AddIn(string AssemblyGuid)
        {
            object obj = cache.GetObject(AssemblyGuid);
            if (obj == null)
            {
                try
                {
                    obj = GetAddIn(AssemblyGuid);
                    if (obj != null)
                    {
                        cache.SaveCache(AssemblyGuid, obj);// 写入缓存

                        DataRow row = (DataRow)obj;
                        _guid = row["Guid"].ToString();
                        _name = row["Name"].ToString();
                        _desc = row["Decription"].ToString();
                        _assembly = row["Assembly"].ToString();
                        _classname = row["Classname"].ToString();
                        _version = row["Version"].ToString();
                    }

                }
                catch (System.Exception ex)
                {
                    string str = ex.Message;// 记录错误日志
                }
            }
            
        }

        /// <summary>
        /// 得到所有插件
        /// </summary>
        /// <returns></returns>
        public DataSet GetAddInList()
        {
            try
            {
                DataSet dsAddin = new DataSet();
                if (File.Exists(fileAddin))
                {
                    dsAddin.ReadXml(fileAddin);
                    if (dsAddin.Tables.Count > 0)
                    {
                        return dsAddin;
                    }
                }
                return null;
            }
            catch (SystemException ex)
            {
                string err = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 得到某种接口下的所有插件
        /// </summary>
        /// <returns></returns>
        public DataSet GetAddInList(string InterfaceName)
        {
            try
            {
                DataSet dsAddin = new DataSet();
                if (File.Exists(fileAddin))
                {
                    dsAddin.ReadXml(fileAddin);
                    if (dsAddin.Tables.Count > 0)
                    {
                        List<DataRow> rowdels = new List<DataRow>();
                        foreach (DataRow row in dsAddin.Tables[0].Rows)
                        {
                            string assem = row["Assembly"].ToString();
                            bool ret = false;
                            try
                            {
                                Assembly assembly = System.Reflection.Assembly.Load(assem);
                                Type[] types = assembly.GetTypes();                                
                                foreach (Type t in types)
                                {
                                    Type[] interfaces = t.GetInterfaces();
                                    foreach (Type theInterface in interfaces)
                                    {
                                        if (theInterface.FullName == InterfaceName)
                                        {
                                            ret = true;
                                        }
                                    }
                                }                                
                            }
                            catch
                            { }

                            if (!ret)
                            {
                                rowdels.Add(row);
                            }
                        }
                        foreach (DataRow r in rowdels)
                        {
                            dsAddin.Tables[0].Rows.Remove(r);
                        }
                        return dsAddin;
                    }
                }
                return null;
            }
            catch (SystemException ex)
            {
                string err = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 得到一个插件的信息
        /// </summary>
        /// <returns></returns>
        private DataRow GetAddIn(string AssemblyGuid)
        {
            DataSet dsAddin = GetAddInList();
            if (dsAddin.Tables.Count > 0)
            {
                DataRow[] drs = dsAddin.Tables[0].Select("Guid='" + AssemblyGuid + "'");
                if (drs.Length > 0)
                {
                    DataRow row = drs[0];
                    return row;
                }
            }
            return null;
        }
        
        /// <summary>
        /// 得到一个插件信息(缓存)
        /// </summary>
        /// <param name="AssemblyGuid"></param>
        /// <returns></returns>
        public DataRow GetAddInByCache(string AssemblyGuid)
        {
            object obj = cache.GetObject(AssemblyGuid);
            if (obj == null)
            {
                try
                {
                    obj = GetAddIn(AssemblyGuid);
                    cache.SaveCache(AssemblyGuid, obj);// 写入缓存
                }
                catch (System.Exception ex)
                {
                    string str = ex.Message;// 记录错误日志
                }
            }
            return (DataRow)obj;
        }

        /// <summary>
        /// 增加插件信息
        /// </summary>
        public void AddAddIn()
        {
            DataSet dsAddin = new DataSet();
            if (File.Exists(fileAddin))
            {
                dsAddin.ReadXml(fileAddin);
                if (dsAddin.Tables.Count > 0)
                {
                    DataRow rown = dsAddin.Tables[0].NewRow();
                    rown["Guid"] = _guid;
                    rown["Name"] = _name;
                    rown["Decription"] = _desc;
                    rown["Assembly"] = _assembly;
                    rown["Classname"] = _classname;
                    rown["Version"] = _version;
                    dsAddin.Tables[0].Rows.Add(rown);
                    //dsAddin.WriteXml(fileAddin);

                    XmlTextWriter xtw = new XmlTextWriter(fileAddin, Encoding.Default);
                    xtw.WriteStartDocument();
                    dsAddin.WriteXml(xtw);
                    xtw.Close();

                }
            }
        }
        /// <summary>
        /// 删除一个插件
        /// </summary>
        /// <param name="AssemblyGuid"></param>
        public void DeleteAddIn(string AssemblyGuid)
        {
            DataSet dsAddin = GetAddInList();
            if (dsAddin.Tables.Count > 0)
            {
                dsAddin.Tables[0].Select("Guid='" + AssemblyGuid + "'")[0].Delete();
                dsAddin.WriteXml(fileAddin);
            }		
				
        }
        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <returns></returns>
        public string LoadFile()
        {
            StreamReader srFile = new StreamReader(fileAddin, Encoding.Default);
            string strContents = srFile.ReadToEnd();
            srFile.Close();
            return strContents;
        }



    }
}
