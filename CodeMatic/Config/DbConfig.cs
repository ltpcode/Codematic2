using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Data;
using System.IO;
using System.Windows.Forms;
namespace Codematic
{
	#region 数据源登录信息
	/// <summary>
	/// DbInfo 的摘要说明。
	/// </summary>
	public class DbSettings
	{
		public DbSettings()
		{}
                
		#region 

		private string _dbtype;
		private string _server;
        private string _connectstr;        
		private string _dbname;
        private bool _connectSimple=false;
      
		/// <summary>
		/// 数据源类型 
		/// </summary>
		[XmlElement]
		public string DbType
		{
			set{ _dbtype=value; }
			get{ return _dbtype; }
		}

		/// <summary>
		/// 服务器
		/// </summary>
		[XmlElement]
		public string Server
		{
			set{ _server=value; }
			get{ return _server; }
		}
        /// <summary>
        /// 连接字符串
        /// </summary>
        [XmlElement]
        public string ConnectStr
        {
            set { _connectstr = value; }
            get { return _connectstr; }
        }
       
		/// <summary>
        /// 数据库//增加一个是否单库字段。
		/// </summary>
		[XmlElement]
		public string DbName
		{
            set { _dbname = value; }
            get { return _dbname; }
		}

        /// <summary>
        /// 简洁连接模式，只列出表名，无字段信息。
        /// </summary>
        [XmlElement]
        public bool ConnectSimple
        {
            set { _connectSimple = value; }
            get { return _connectSimple; }
        }

		#endregion

	}
	#endregion 


	public class DbConfig
	{        
        static string fileName = Application.StartupPath + "\\DbSetting.config";

		#region 得到 所有登陆过的数据源设置
		/// <summary>
		/// 得到所有登陆过的数据源设置
		/// </summary>
		/// <returns></returns>
		public static DbSettings[] GetSettings()
		{				
			try
			{				
				DataSet ds=new DataSet();
                ArrayList DbList = new ArrayList();
				if(File.Exists(fileName))
				{					
					DbSettings dbobj;
					ds.ReadXml(fileName);//,XmlReadMode.ReadSchema
					if(ds.Tables.Count>0)
					{
						foreach(DataRow dr in ds.Tables[0].Rows)
						{
							dbobj=new DbSettings();
							dbobj.DbType=dr["DbType"].ToString();
							dbobj.Server=dr["Server"].ToString();
                            dbobj.ConnectStr = dr["ConnectStr"].ToString();
                            dbobj.DbName = dr["DbName"].ToString();
                            if (ds.Tables[0].Columns.Contains("ConnectSimple"))
                            {
                                if ((dr["ConnectSimple"] != null) && (dr["ConnectSimple"].ToString().Length > 0))
                                {
                                    dbobj.ConnectSimple = bool.Parse(dr["ConnectSimple"].ToString());
                                }
                            }                            
							DbList.Add(dbobj);
						}						
					}					
				}
                DbSettings[] dbList = (DbSettings[])DbList.ToArray(typeof(DbSettings));
                return dbList;			
			}
			catch
			{
				return null;
			}
            
		
		}
        /// <summary>
        /// 得到当前数据库服务器配置信息
        /// </summary>
        /// <returns></returns>
        public static DataSet GetSettingDs()
        {
            try
            {
                DataSet ds = new DataSet();               
                if (File.Exists(fileName))
                {                   
                    ds.ReadXml(fileName);//,XmlReadMode.ReadSchema                    
                }
                return ds;
            }
            catch
            {
                return null;
            }
        }
		#endregion

        #region 根据 长服务器名 得到服务器配置
        /// <summary>
        /// 根据长服务器名得到服务器配置
        /// </summary>
        /// <param name="servername"></param>
        public static DbSettings GetSetting(string loneServername)
        {
            //127.0.0.1(dbtype)(dbname)            
            string dbtype = "SQL2000";
            int s = loneServername.IndexOf("(");
            string server = loneServername.Substring(0, s);
            int e = loneServername.IndexOf(")",s);
            dbtype = loneServername.Substring(s + 1,e-s-1);
            string dbname = "";
            if (loneServername.Length > e + 1)
            {
                dbname = loneServername.Substring(e + 2).Replace(")","");
            }            
            return GetSetting(dbtype, server, dbname);            
        }
        #endregion

        #region 得到 指定数据源 和 IP 的服务器配置信息
        /// <summary>
		/// 得到指定数据源的配置信息
		/// </summary>
		/// <returns></returns>
        public static DbSettings GetSetting(string DbType, string Serverip,string DbName)
		{				
			try
			{
                DbSettings dbset = null;
				DataSet ds=new DataSet();	
				if(File.Exists(fileName))
				{					
					ds.ReadXml(fileName);//,XmlReadMode.ReadSchema
					if(ds.Tables.Count>0)
					{
                        string strwhere = "DbType='" + DbType + "' and Server='" + Serverip + "'";
                        if (DbName.Trim() != "")
                        {
                            strwhere += " and DbName='" + DbName + "'";
                        }
                        DataRow[] drs = ds.Tables[0].Select(strwhere);
						if(drs.Length>0)
						{
                            dbset = new DbSettings();
                            dbset.DbType = drs[0]["DbType"].ToString();
                            dbset.Server = drs[0]["Server"].ToString();
                            dbset.ConnectStr = drs[0]["ConnectStr"].ToString();
                            dbset.DbName = drs[0]["DbName"].ToString();

                            if (ds.Tables[0].Columns.Contains("ConnectSimple"))
                            {
                                if ((drs[0]["ConnectSimple"] != null) && (drs[0]["ConnectSimple"].ToString().Length > 0))
                                {
                                    dbset.ConnectSimple = bool.Parse(drs[0]["ConnectSimple"].ToString());
                                }
                            }      
						}						
					}					
				}
                return dbset;				
			}
			catch
			{
				return null;
			}			
		}
		#endregion

		#region 保存当前数据源设置

		public static DataTable CreateDataTable()
		{			
			DataTable table=new DataTable("DBServer");
			DataColumn col;
			
			col=new DataColumn();
			col.DataType=Type.GetType("System.String");
			col.ColumnName="DbType";
			table.Columns.Add(col);

			col=new DataColumn();
			col.DataType=Type.GetType("System.String");
			col.ColumnName="Server";
			table.Columns.Add(col);

			col=new DataColumn();
			col.DataType=Type.GetType("System.String");
            col.ColumnName = "ConnectStr";
			table.Columns.Add(col); 

			col=new DataColumn();
			col.DataType=Type.GetType("System.String");
            col.ColumnName = "DbName";
			table.Columns.Add(col);
            
            col = new DataColumn();
            col.DataType = Type.GetType("System.Boolean");
            col.ColumnName = "ConnectSimple";
            table.Columns.Add(col); 	
			
			return table;
		}

        /// <summary>
        /// 增加当前数据源设置
        /// </summary>
        /// <param name="data"></param>
		public static bool AddSettings(DbSettings dbobj)
		{
			try
			{				
				DataSet ds=new DataSet();			
				if(!File.Exists(fileName))
                {
                    #region 第一次添加
                    DataTable dt=CreateDataTable();					
					DataRow rown=dt.NewRow();
					rown["DbType"]=dbobj.DbType;
					rown["Server"]=dbobj.Server;
                    rown["ConnectStr"] = dbobj.ConnectStr;
                    rown["DbName"] = dbobj.DbName;
                    rown["ConnectSimple"] = dbobj.ConnectSimple;			
					dt.Rows.Add(rown);
			
					ds.Tables.Add(dt);
                    #endregion
                }
				else
                {
                    #region 追加

                    ds.ReadXml(fileName);//,XmlReadMode.ReadSchema
					if(ds.Tables.Count>0)
					{
                        DataRow[] drs = ds.Tables[0].Select("DbType='" + dbobj.DbType + "' and Server='" + dbobj.Server + "' and DbName='" + dbobj.DbName + "'");
						if(drs.Length>0)
						{
                            //drs[0]["DbType"] = dbobj.DbType;
                            //drs[0]["Server"] = dbobj.Server;
                            //drs[0]["Uid"] = dbobj.Uid;
                            //drs[0]["Password"] = dbobj.Password;
                            //drs[0]["LoginMode"] = dbobj.LoginMode;
                            return false;
                        }
                        else
                        {
                            DataRow rown = ds.Tables[0].NewRow();
                            rown["DbType"] = dbobj.DbType;
                            rown["Server"] = dbobj.Server;
                            rown["ConnectStr"] = dbobj.ConnectStr;
                            rown["DbName"] = dbobj.DbName;
                            rown["ConnectSimple"] = dbobj.ConnectSimple;		
                            ds.Tables[0].Rows.Add(rown);	
                            
						}
					}
					else
					{
						DataTable dt=CreateDataTable();					
						DataRow rown=dt.NewRow();
						rown["DbType"]=dbobj.DbType;
						rown["Server"]=dbobj.Server;
                        rown["ConnectStr"] = dbobj.ConnectStr;
                        rown["DbName"] = dbobj.DbName;
                        rown["ConnectSimple"] = dbobj.ConnectSimple;		
						dt.Rows.Add(rown);
			
						ds.Tables.Add(dt);
                    }
                    #endregion

                }
				ds.WriteXml(fileName);
                return true;
			}
			catch
			{
				//throw new Exception("保存配置信息失败！");
                return false;
			}
		}

        /// <summary>
        /// 更新当前数据源设置
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateSettings(DbSettings dbobj)
        {
            try
            {                
                DataSet ds = new DataSet();
                if (!File.Exists(fileName))
                {
                    DataTable dt = CreateDataTable();
                    DataRow rown = dt.NewRow();
                    rown["DbType"] = dbobj.DbType;
                    rown["Server"] = dbobj.Server;
                    rown["ConnectStr"] = dbobj.ConnectStr;
                    rown["DbName"] = dbobj.DbName;
                    rown["ConnectSimple"] = dbobj.ConnectSimple;		
                    dt.Rows.Add(rown);

                    ds.Tables.Add(dt);
                }
                else
                {
                    ds.ReadXml(fileName);//,XmlReadMode.ReadSchema
                    if (ds.Tables.Count > 0)
                    {
                        DataRow[] drs = ds.Tables[0].Select("DbType='" + dbobj.DbType + "' and Server='" + dbobj.Server + "' and DbName='" + dbobj.DbName + "'");
                        if (drs.Length > 0)
                        {
                            drs[0]["DbType"] = dbobj.DbType;
                            drs[0]["Server"] = dbobj.Server;
                            drs[0]["ConnectStr"] = dbobj.ConnectStr;
                            drs[0]["DbName"] = dbobj.DbName;
                            drs[0]["ConnectSimple"] = dbobj.ConnectSimple;		
                            
                        }
                        else
                        {
                            DataRow rown = ds.Tables[0].NewRow();
                            rown["DbType"] = dbobj.DbType;
                            rown["Server"] = dbobj.Server;
                            rown["ConnectStr"] = dbobj.ConnectStr;
                            rown["DbName"] = dbobj.DbName;
                            rown["ConnectSimple"] = dbobj.ConnectSimple;		
                            ds.Tables[0].Rows.Add(rown);

                        }
                    }
                    else
                    {
                        DataTable dt = CreateDataTable();
                        DataRow rown = dt.NewRow();
                        rown["DbType"] = dbobj.DbType;
                        rown["Server"] = dbobj.Server;
                        rown["ConnectStr"] = dbobj.ConnectStr;
                        rown["DbName"] = dbobj.DbName;
                        rown["ConnectSimple"] = dbobj.ConnectSimple;		
                        dt.Rows.Add(rown);

                        ds.Tables.Add(dt);
                    }

                }
                ds.WriteXml(fileName);
            }
            catch
            {
                throw new Exception("保存配置信息失败！");
            }
        }	


		#endregion

        #region 删除 指定数据源的配置信息
        /// <summary>
        /// 删除指定数据源的配置信息
        /// </summary>
        /// <returns></returns>
        public static void DelSetting(string DbType, string Serverip, string DbName)
        {
            try
            {             
                DataSet ds = new DataSet();
                if (File.Exists(fileName))
                {
                    ds.ReadXml(fileName);//,XmlReadMode.ReadSchema
                    if (ds.Tables.Count > 0)
                    {
                        string strwhere = "DbType='" + DbType + "' and Server='" + Serverip + "'";
                        if ((DbName.Trim() != "") && (DbName.Trim() != "master"))
                        {
                            strwhere += " and DbName='" + DbName + "'";
                        }
                        DataRow[] drs = ds.Tables[0].Select(strwhere);
                        if (drs.Length > 0)
                        {
                            ds.Tables[0].Rows.Remove(drs[0]);                           
                        }
                        ds.Tables[0].AcceptChanges();
                    }
                }
                ds.WriteXml(fileName);
            }
            catch
            {
                //return null;
            }
        }
        #endregion
	
	}
}
