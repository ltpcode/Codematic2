using System;
using System.Data;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.DbObjects.OleDb
{
	/// <summary>
	/// 数据库脚本生成类。Script
	/// </summary>
	public class DbScriptBuilder:IDbScriptBuilder
	{
        #region 私有变量
        protected string _key = "ID";//标识列，或主键字段		
        protected string _keyType = "int";//标识列，或主键字段类型        
        #endregion

		#region 属性
		private string _dbconnectStr;
		private string _dbname;
		private string _tablename;				
        private string _procprefix;
        private string _projectname;
        private List<ColumnInfo> _fieldlist;
        private List<ColumnInfo> _keys; //主键或条件字段列表 

		public string DbConnectStr
		{
			set{_dbconnectStr=value;}
			get{return _dbconnectStr;}
		}
		public string DbName
		{
			set{ _dbname=value;}
			get{return _dbname;}
		}
		public string TableName
		{
			set{ _tablename=value;}
			get{return _tablename;}
		}		
        /// <summary>
        /// 存储过程前缀 
        /// </summary>       
        public string ProcPrefix
        {
            set { _procprefix = value; }
            get { return _procprefix; }
        }
        /// <summary>
        /// 项目名称 
        /// </summary>        
        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }       
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }       
        public List<ColumnInfo> Keys
        {
            set { _keys = value; }
            get { return _keys; }
        }

		#endregion

        #region 构造属性
        /// <summary>
        /// 选择的字段集合的-字符串
        /// </summary>
        public string Fields
        {
            get
            {
                StringPlus _fields = new StringPlus();
                foreach (ColumnInfo obj in Fieldlist)
                {
                    _fields.Append("'" + obj.ColumnName + "',");
                }
                _fields.DelLastComma();
                return _fields.Value;
            }
        }
        /// <summary>
        /// 字段的 select * 列表
        /// </summary>
        public string Fieldstrlist
        {
            get
            {
                StringPlus _fields = new StringPlus();
                foreach (ColumnInfo obj in Fieldlist)
                {
                    _fields.Append(obj.ColumnName + ",");
                }
                _fields.DelLastComma();
                return _fields.Value;
            }
        }
        /// <summary>
        /// 列中是否有标识列
        /// </summary>
        public bool IsHasIdentity
        {
            get
            {
                bool isid = false;
                if (_keys.Count > 0)
                {
                    foreach (ColumnInfo key in _keys)
                    {
                        if (key.IsIdentity)
                        {
                            isid = true;
                        }
                    }
                }
                return isid;
            }
        }
        #endregion


		DbObject dbobj=new DbObject();

		public DbScriptBuilder()
		{			
		}

        #region 公共方法

        /// <summary>
        /// 得到方法输入参数的列表 (例如：用于Exists  Delete  GetModel 的参数传入)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string GetInParameter(List<ColumnInfo> keys, bool output)
        {
            StringPlus strclass = new StringPlus();

            foreach (ColumnInfo field in Keys)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                bool IsPK = field.IsPrimaryKey;
                string Length = field.Length;
                string Preci = field.Precision;
                string Scale = field.Scale;

                switch (columnType.ToLower())
                {
                    case "decimal":
                    case "numeric":
                        strclass.AppendLine("@" + columnName + " " + columnType + "(" + Preci + "," + Scale + ")");
                        break;
                    case "varchar":
                    case "nvarchar":
                    case "char":
                    case "nchar":
                        strclass.AppendLine("@" + columnName + " " + columnType + "(" + Length + ")");
                        break;
                    default:
                        strclass.AppendLine("@" + columnName + " " + columnType);
                        break;
                }
                if ((IsIdentity) && (output))
                {
                    strclass.AppendLine(" output,");
                    continue;
                }
                strclass.AppendLine(",");
            }
            strclass.DelLastComma();
            return strclass.Value;
        }

        /// <summary>
        /// 得到Where条件语句 - Parameter方式 (例如：用于Exists  Delete  GetModel 的where)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string GetWhereExpression(List<ColumnInfo> keys)
        {
            StringPlus strclass = new StringPlus();
            foreach (ColumnInfo key in keys)
            {
                strclass.Append(key.ColumnName + "=@" + key.ColumnName + " and ");
            }
            strclass.DelLastChar("and");
            return strclass.Value;
        }


        #endregion

		#region 生成数据库表创建脚本

        /// <summary>
        /// 生成数据库所有表的创建脚本
        /// </summary>
        /// <returns></returns>
        public string CreateDBTabScript(string dbname)
        {
            dbobj.DbConnectStr = this.DbConnectStr;
            StringPlus strclass = new StringPlus();
            List<string> tabNames = dbobj.GetTables(dbname);
            if (tabNames.Count > 0)
            {
                foreach (string tabname in tabNames)
                {
                    strclass.AppendLine(CreateTabScript(dbname, tabname));
                }
            }
            return strclass.Value;
        }


		/// <summary>
		/// 生成数据库表创建脚本
		/// </summary>
		/// <returns></returns>
		public string CreateTabScript(string dbname,string tablename)
		{
			dbobj.DbConnectStr=_dbconnectStr;
            List<ColumnInfo> collist = dbobj.GetColumnInfoList(dbname, tablename);
            DataTable dt = Maticsoft.CodeHelper.CodeCommon.GetColumnInfoDt(collist);
			StringPlus strclass=new StringPlus();
			
			string PKfild="";//主键字段
			StringPlus ColdefaVal=new StringPlus();//字段的默认值列表			
			
			Hashtable FildtabList=new Hashtable();//字段列表
			StringPlus FildList=new StringPlus();//字段列表
			//开始创建表
			strclass.AppendLine("");
			strclass.AppendLine("CREATE TABLE ["+tablename+"] (");
			if(dt!=null)
			{				
				foreach(DataRow row in dt.Rows)
				{					
					string columnName=row["ColumnName"].ToString();	
					string columnType=row["TypeName"].ToString();				
					string Length=row["Length"].ToString();
					string Preci=row["Preci"].ToString();
					string Scale=row["Scale"].ToString();
					string ispk=row["isPK"].ToString();	
					string isnull=row["cisNull"].ToString();
					string defaultVal=row["defaultVal"].ToString();

					strclass.Append("["+columnName+"] ["+columnType+"] ");
					switch(columnType.Trim())
					{	
						case "CHAR":					
						case "VARCHAR2":
						case "NCHAR":
						case "NVARCHAR2":						
							strclass.Append(" ("+Length+")");
							break;
						case "NUMBER":		
							strclass.Append(" ("+Preci+","+Scale+")");
							break;						
					}				
					if(isnull=="√")
					{
						strclass.Append(" NULL");
					}
					else
					{
						strclass.Append(" NOT NULL");
					}
					if(defaultVal!="")
					{
						strclass.Append(" DEFAULT "+defaultVal);
					}
					strclass.AppendLine(",");

					FildtabList.Add(columnName,columnType);
					FildList.Append("["+columnName+"],");
					
//					if(defaultVal!="")
//					{
//						ColdefaVal.Append("CONSTRAINT [DF_"+tablename+"_"+columnName+"] DEFAULT "+defaultVal+" FOR ["+columnName+"],");
//					}

					if(PKfild=="")
					{						
						PKfild=columnName;//得到主键
					}
				}
			}
			strclass.DelLastComma();
			FildList.DelLastComma();
			strclass.AppendLine(")");
			strclass.AppendLine("");

			//获取数据
			DataTable dtdata=dbobj.GetTabData(dbname,tablename);
			if(dtdata!=null)
			{				
				foreach(DataRow row in dtdata.Rows)//循环表数据
				{	
					StringPlus strfild=new StringPlus();
					StringPlus strdata=new StringPlus();
					string [] split= FildList.Value.Split(new Char [] { ','});

					foreach(string fild in split)//循环一行数据的各个字段
					{
						string colname=fild.Substring(1,fild.Length-2);
						string coltype="";
						foreach (DictionaryEntry myDE in FildtabList)
						{
							if(myDE.Key.ToString()==colname)
							{
								coltype=myDE.Value.ToString();
							}
						}	
						string strval="";
						switch(coltype)
						{
							case "BLOB":
							{
								byte[] bys=(byte[])row[colname];
                                strval = Maticsoft.CodeHelper.CodeCommon.ToHexString(bys);
							}
								break;
							case "bit":
							{
								strval=(row[colname].ToString().ToLower()=="true")?"1":"0";
							}
								break;
							default:
								strval=row[colname].ToString().Trim();
								break;
						}
						if(strval!="")
						{
                            if (Maticsoft.CodeHelper.CodeCommon.IsAddMark(coltype))
							{
                                //strdata.Append("'"+strval+"',");
                                strdata.Append("'" + strval.Replace("'", "''") + "',");
							}
							else
							{
								strdata.Append(strval+",");
							}	
							strfild.Append("["+colname+"],");
						}

					}					
					strfild.DelLastComma();
					strdata.DelLastComma();
					//导出数据INSERT语句
					strclass.Append("INSERT ["+tablename+"] (");
					strclass.Append(strfild.Value);
					strclass.Append(") VALUES ( ");
					strclass.Append(strdata.Value);//数据值
					strclass.AppendLine(")");
				}
			}


			return strclass.Value;

		}

        /// <summary>
        /// 根据SQL查询结果 生成数据创建脚本
        /// </summary>
        /// <returns></returns>
        public string CreateTabScriptBySQL(string dbname, string strSQL)
        {
            dbobj.DbConnectStr = _dbconnectStr;
            //string PKfild = "";//主键字段
            bool IsIden = false;//是否是标识字段
            string tablename = "TableName";
            StringPlus strclass = new StringPlus();

            #region 查询表名
            int ns = strSQL.IndexOf(" from ");
            if (ns > 0)
            {
                string sqltemp = strSQL.Substring(ns + 5).Trim();
                int ns2 = sqltemp.IndexOf(" ");
                if (sqltemp.Length > 0)
                {
                    if (ns2 > 0)
                    {
                        tablename = sqltemp.Substring(0, ns2).Trim();
                    }
                    else
                    {
                        tablename = sqltemp.Substring(0).Trim();
                    }
                }
            }
            tablename = tablename.Replace("[", "").Replace("]", "");

            #endregion

            #region 数据脚本

            //获取数据
            DataTable dtdata = dbobj.GetTabDataBySQL(dbname, strSQL);
            if (dtdata != null)
            {
                DataColumnCollection dtcols = dtdata.Columns;
                foreach (DataRow row in dtdata.Rows)//循环表数据
                {
                    StringPlus strfild = new StringPlus();
                    StringPlus strdata = new StringPlus();

                    foreach (DataColumn col in dtcols)//循环一行数据的各个字段
                    {
                        string colname = col.ColumnName;
                        string coltype = col.DataType.Name;
                        if (col.AutoIncrement)
                        {
                            IsIden = true;
                        }
                        string strval = "";
                        switch (coltype.ToLower())
                        {
                            case "binary":
                            case "byte[]":
                            case "blob":
                                {
                                    byte[] bys = (byte[])row[colname];
                                    strval = Maticsoft.CodeHelper.CodeCommon.ToHexString(bys);
                                }
                                break;
                            case "bit":
                            case "boolean":
                                {
                                    strval = (row[colname].ToString().ToLower() == "true") ? "1" : "0";
                                }
                                break;
                            default:
                                strval = row[colname].ToString().Trim();
                                break;
                        }
                        if (strval != "")
                        {
                            if (Maticsoft.CodeHelper.CodeCommon.IsAddMark(coltype))
                            {
                                //strdata.Append("'" + strval + "',");
                                strdata.Append("'" + strval.Replace("'", "''") + "',");
                            }
                            else
                            {
                                strdata.Append(strval + ",");
                            }
                            strfild.Append("" + colname + ",");
                        }

                    }
                    strfild.DelLastComma();
                    strdata.DelLastComma();
                    //导出数据INSERT语句
                    strclass.Append("INSERT " + tablename + " (");
                    strclass.Append(strfild.Value);
                    strclass.Append(") VALUES ( ");
                    strclass.Append(strdata.Value);//数据值
                    strclass.AppendLine(")");
                }
            }
            //StringPlus strclass0 = new StringPlus();
            //if (IsIden)
            //{
            //    strclass0.AppendLine("SET IDENTITY_INSERT [" + tablename + "] ON");
            //    strclass0.AppendLine("");
            //}
            //strclass0.AppendLine(strclass.Value);
            //if (IsIden)
            //{
            //    strclass0.AppendLine("");
            //    strclass0.AppendLine("SET IDENTITY_INSERT [" + tablename + "] OFF");
            //}
            #endregion
            return strclass.Value;

        }

	
		/// <summary>
		/// 生成数据库表创建脚本到文件
		/// </summary>
		/// <returns></returns>
		public void CreateTabScript(string dbname,string tablename,string filename,System.Windows.Forms.ProgressBar progressBar)
		{
			StreamWriter sw=new StreamWriter(filename,true,Encoding.Default);//,false);

			dbobj.DbConnectStr=_dbconnectStr;
            List<ColumnInfo> collist = dbobj.GetColumnInfoList(dbname, tablename);
			StringPlus strclass=new StringPlus();
			
			string PKfild="";//主键字段
			StringPlus ColdefaVal=new StringPlus();//字段的默认值列表
			
			
			Hashtable FildtabList=new Hashtable();//字段列表
			StringPlus FildList=new StringPlus();//字段列表
			
			#region 创建的脚本

			//开始创建表
			strclass.AppendLine("");
			strclass.AppendLine("CREATE TABLE ["+tablename+"] (");
            if ((collist!=null)&&(collist.Count > 0))
            {
                foreach (ColumnInfo col in collist)
                {
                    string columnName = col.ColumnName;
                    string columnType = col.TypeName;
                    bool IsIdentity = col.IsIdentity;
                    string Length = col.Length;
                    string Preci = col.Precision;
                    string Scale = col.Scale;
                    bool ispk = col.IsPrimaryKey;
                    bool isnull = col.Nullable;
                    string defaultVal = col.DefaultVal;


					strclass.Append("["+columnName+"] ["+columnType+"] ");					
					switch(columnType.Trim())
					{	
						case "char":					
						case "varchar":
						case "nchar":
						case "nvarchar":						
							strclass.Append(" ("+Length+")");
							break;
						case "float":		
							strclass.Append(" ("+Preci+","+Scale+")");
							break;							
					}				
					if(isnull)
					{
						strclass.Append(" NULL");
					}
					else
					{
						strclass.Append(" NOT NULL");
					}
					if(defaultVal!="")
					{
						strclass.Append(" DEFAULT "+defaultVal);
					}
					strclass.AppendLine(",");

					FildtabList.Add(columnName,columnType);
					FildList.Append("["+columnName+"],");
					
//					if(defaultVal!="")
//					{
//						ColdefaVal.Append("CONSTRAINT [DF_"+tablename+"_"+columnName+"] DEFAULT "+defaultVal+" FOR ["+columnName+"],");
//					}

					if(PKfild=="")
					{						
						PKfild=columnName;//得到主键
					}
				}
			}
			strclass.DelLastComma();
			FildList.DelLastComma();
			strclass.AppendLine(")");
			strclass.AppendLine("");
			
			if(PKfild!="")
			{
				strclass.Append("ALTER TABLE ["+tablename+"] WITH NOCHECK ADD  CONSTRAINT [PK_"+tablename+"] PRIMARY KEY  NONCLUSTERED ( ["+PKfild+"] )");
			}

			#endregion			

			#region
			//设置主键
			//			if((PKfild!="")||(ColdefaVal.Value!=""))
			//			{				
			//				strclass.AppendLine("ALTER TABLE ["+tablename+"] WITH NOCHECK ADD ");
			//				if(ColdefaVal.Value!="")
			//				{
			//					strclass.Append(ColdefaVal.Value);
			//				}
			//				if(PKfild!="")
			//				{
			//					strclass.Append(" CONSTRAINT [PK_"+tablename+"] PRIMARY KEY  NONCLUSTERED ( ["+PKfild+"] )");
			//				}
			//				else
			//				{
			//					strclass.DelLastComma();
			//				}
			//			}
			#endregion

			sw.Write(strclass.Value);

			#region 生成数据脚本

			//获取数据
			DataTable dtdata=dbobj.GetTabData(dbname,tablename);
			if(dtdata!=null)
			{		
				int i=0;				
				progressBar.Maximum=dtdata.Rows.Count;				
				foreach(DataRow row in dtdata.Rows)//循环表数据
				{						
					progressBar.Value=i;
					i++;
					StringPlus rowdata=new StringPlus();

					StringPlus strfild=new StringPlus();
					StringPlus strdata=new StringPlus();
					string [] split= FildList.Value.Split(new Char [] { ','});

					foreach(string fild in split)//循环一行数据的各个字段
					{
						string colname=fild.Substring(1,fild.Length-2);
						string coltype="";
						foreach (DictionaryEntry myDE in FildtabList)
						{
							if(myDE.Key.ToString()==colname)
							{
								coltype=myDE.Value.ToString();
							}
						}	
						string strval="";
						switch(coltype)
						{
							case "binary":
							{
								byte[] bys=(byte[])row[colname];
                                strval = Maticsoft.CodeHelper.CodeCommon.ToHexString(bys);
							}
								break;
							case "bit":
							case "bool":
							{
								strval=(row[colname].ToString().ToLower()=="true")?"1":"0";
							}
								break;
							default:
								strval=row[colname].ToString().Trim();
								break;
						}
						if(strval!="")
						{
                            if (Maticsoft.CodeHelper.CodeCommon.IsAddMark(coltype))
							{
                                //strdata.Append("'"+strval+"',");
                                strdata.Append("'" + strval.Replace("'", "''") + "',");
							}
							else
							{
								strdata.Append(strval+",");
							}	
							strfild.Append("["+colname+"],");
						}

					}					
					strfild.DelLastComma();
					strdata.DelLastComma();

					//导出数据INSERT语句
					rowdata.Append("INSERT ["+tablename+"] (");
					rowdata.Append(strfild.Value);
					rowdata.Append(") VALUES ( ");
					rowdata.Append(strdata.Value);//数据值
					rowdata.AppendLine(")");

					sw.Write(rowdata.Value);
				}
			}

			#endregion

			sw.Flush();
			sw.Close();

		}


		
		#endregion

		#region 创建存储过程
        /// <summary>
        /// 得到一个库下所有表的存储过程
        /// </summary>
        /// <param name="DbName"></param>
        /// <returns></returns>
        public string GetPROCCode(string dbname)
        {
            return "";
        }

        /// <summary>
        /// 得到某个表的存储过程
        /// </summary>
        /// <param name="dbname">库名</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public string GetPROCCode(string dbname, string tablename)
        {
            return "";
        }
        /// <summary>
        /// 得到某个表的存储过程（选择生成的方法）
        /// </summary>   
        public string GetPROCCode(bool Maxid, bool Ishas, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            return "";
        }

        public string CreatPROCGetMaxID()
        {
            return "";
        }
        public string CreatPROCIsHas()
        {
            return "";
        }
        public string CreatPROCADD()
        {
            return "";
        }
        public string CreatPROCUpdate()
        {
            return "";
        }
        public string CreatPROCDelete()
        {
            return "";

        }
        public string CreatPROCGetModel()
        {
            return "";

        }
        public string CreatPROCGetList()
        {
            return "";

        }		
		
		#endregion


        #region 生成SQL查询语句

        /// <summary>
        /// 生成Select查询语句
        /// </summary>
        /// <param name="dbname">库名</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public string GetSQLSelect(string dbname, string tablename)
        {
            dbobj.DbConnectStr = _dbconnectStr;
            List<ColumnInfo> collist = dbobj.GetColumnList(dbname, tablename);
            this.DbName = dbname;
            this.TableName = tablename;
            StringPlus strsql = new StringPlus();
            strsql.Append("select ");
            if ((collist!=null)&&(collist.Count > 0))
            {
                foreach (ColumnInfo col in collist)
                {
                    string columnName = col.ColumnName;
                    strsql.Append("[" + columnName + "],");
                }
                strsql.DelLastComma();
            }
            strsql.Append(" from " + tablename);
            return strsql.Value;
        }

        /// <summary>
        /// 生成update查询语句
        /// </summary>
        /// <param name="dbname">库名</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public string GetSQLUpdate(string dbname, string tablename)
        {
            dbobj.DbConnectStr = _dbconnectStr;
            List<ColumnInfo> collist = dbobj.GetColumnList(dbname, tablename);
            this.DbName = dbname;
            this.TableName = tablename;
            StringPlus strsql = new StringPlus();
            strsql.AppendLine("update " + tablename + " set ");
            if ((collist!=null)&&(collist.Count > 0))
            {
                foreach (ColumnInfo col in collist)
                {
                    string columnName = col.ColumnName;
                    strsql.AppendLine("[" + columnName + "] = <" + columnName + ">,");
                }
                strsql.DelLastComma();
            }
            strsql.Append(" where <搜索条件>");
            return strsql.Value;
        }
        /// <summary>
        /// 生成update查询语句
        /// </summary>
        /// <param name="dbname">库名</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public string GetSQLDelete(string dbname, string tablename)
        {
            dbobj.DbConnectStr = _dbconnectStr;
            //DataTable dt = dbobj.GetColumnList(dbname, tablename);
            this.DbName = dbname;
            this.TableName = tablename;
            StringPlus strsql = new StringPlus();
            strsql.AppendLine("delete from " + tablename);
            strsql.Append(" where  <搜索条件>");
            return strsql.Value;
        }
        /// <summary>
        /// 生成INSERT查询语句
        /// </summary>
        /// <param name="dbname">库名</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public string GetSQLInsert(string dbname, string tablename)
        {
            dbobj.DbConnectStr = _dbconnectStr;
            List<ColumnInfo> collist = dbobj.GetColumnList(dbname, tablename);
            this.DbName = dbname;
            this.TableName = tablename;
            StringPlus strsql = new StringPlus();
            StringPlus strsql2 = new StringPlus();
            strsql.AppendLine("INSERT INTO " + tablename + " ( ");

            if ((collist!=null)&&(collist.Count > 0))
            {
                foreach (ColumnInfo col in collist)
                {
                    string columnName = col.ColumnName;
                    string columnType = col.TypeName;

                    strsql.AppendLine("[" + columnName + "] ,");
                    if (Maticsoft.CodeHelper.CodeCommon.IsAddMark(columnType))
                    {
                        strsql2.Append("'" + columnName + "',");
                    }
                    else
                    {
                        strsql2.Append(columnName + ",");
                    }

                }
                strsql.DelLastComma();
                strsql2.DelLastComma();
            }
            strsql.Append(") VALUES (" + strsql2.Value + ")");
            return strsql.Value;
        }
        #endregion

        
    }
}
