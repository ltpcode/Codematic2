using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Maticsoft.CodeHelper;
using System.Data.OracleClient;
using System.Collections;
using Maticsoft.IDBO;
namespace Maticsoft.DbObjects.Oracle
{
	/// <summary>
	/// DbObject 的摘要说明。
	/// </summary>
	public class DbObject:IDbObject
	{		
		
		#region  属性
        public string DbType
        {
            get { return "Oracle"; }
        }
		private string _dbconnectStr;	
		public string DbConnectStr
		{
			set{_dbconnectStr=value;}
			get{return _dbconnectStr;}
		}
		OracleConnection connect = new OracleConnection();
		
		#endregion		

		#region 构造函数，构造基本信息
		public DbObject()
		{			
		}

		/// <summary>
		/// 构造一个数据库连接
		/// </summary>
		/// <param name="connect"></param>
		public DbObject(string DbConnectStr)
		{			
			_dbconnectStr=DbConnectStr;
			connect.ConnectionString=DbConnectStr;
		}		
		/// <summary>
		/// 构造一个连接字符串
		/// </summary>
		/// <param name="SSPI">是否windows集成认证</param>
		/// <param name="Ip">服务器IP</param>
		/// <param name="User">用户名</param>
		/// <param name="Pass">密码</param>
		public DbObject(bool SSPI,string server,string User,string Pass)
		{		
			connect = new OracleConnection();							
			_dbconnectStr="Data Source="+server+"; user id="+User+";password="+Pass;			
			connect.ConnectionString=_dbconnectStr;
		}

		
		#endregion

		#region 打开连接OpenDB()
		/// <summary>
		/// 打开数据库
		/// </summary>
		/// <param name="DbName">要打开的数据库</param>
		/// <returns></returns>
		public void OpenDB()
		{
			try
			{
				if(connect.ConnectionString=="")
				{
					connect.ConnectionString=_dbconnectStr;
				}
				if(connect.ConnectionString!=_dbconnectStr)
				{
					connect.Close();
					connect.ConnectionString=_dbconnectStr;
				}
				if(connect.State==System.Data.ConnectionState.Closed)
				{
					connect.Open();
				}					

			}
			catch//(System.Exception ex)
			{
				//string str=ex.Message;	
				//return null;
			}
			
		}
		#endregion

		#region ADO.NET 操作

		public int ExecuteSql(string DbName,string SQLString)
		{
			OpenDB();
			OracleCommand dbCommand = new OracleCommand(SQLString,connect);
			dbCommand.CommandText=SQLString;
			int rows=dbCommand.ExecuteNonQuery();
			return rows;
		}
		
		public DataSet Query(string DbName,string SQLString)
		{			
			DataSet ds = new DataSet();
			try
			{		
				OpenDB();
				OracleDataAdapter command = new OracleDataAdapter(SQLString,connect);				
				command.Fill(ds,"ds");
			}
			catch(System.Data.OracleClient.OracleException ex)
			{				
				throw new Exception(ex.Message);
			}			
			return ds;				
		}	

		public OracleDataReader ExecuteReader(string strSQL)
		{				
			try
			{
				OpenDB();
				OracleCommand cmd = new OracleCommand(strSQL,connect);
				OracleDataReader myReader = cmd.ExecuteReader();
				return myReader;
			}
			catch(System.Data.OracleClient.OracleException ex)
			{								
				throw new Exception(ex.Message);
			}			
		}
        public object GetSingle(string DbName, string SQLString)
        {
            try
            {
                OpenDB();
                OracleCommand dbCommand = new OracleCommand(SQLString, connect);
                object obj = dbCommand.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
                else
                {
                    return obj;
                }
            }
            catch
            {
                return null;
            }
        }
		#endregion


		#region 得到数据库的名字列表 GetDBList()
		/// <summary>
		/// 得到数据库的名字列表
		/// </summary>
		/// <returns></returns>
        public List<string> GetDBList()
		{
			// TODO:  添加 DbObject.GetDBList 实现
			return null;
		}
		#endregion

		#region  得到数据库的 所有用户 表名 GetTables(string DbName)
		/// <summary>
		/// 得到数据库的所有表名
		/// </summary>
		/// <param name="DbName"></param>
		/// <returns></returns>
        public List<string> GetTables(string DbName)
		{
			string strSql="select TNAME name from tab where TABTYPE='TABLE'";
            List<string> tabNames = new List<string>();
            OracleDataReader reader = ExecuteReader(strSql);
            while (reader.Read())
            {
                tabNames.Add(reader.GetString(0));
            }
            reader.Close();
            return tabNames;
		}
		public DataTable GetVIEWs(string DbName)
		{
			string strSql="select TNAME name from tab where TABTYPE='VIEW'";
			return Query("",strSql).Tables[0];
		}
        /// <summary>
        /// 得到数据库的所有表和视图名字
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<string> GetTableViews(string DbName)
        {
            string strSql = "select TNAME name from tab where TABTYPE='TABLE' or TABTYPE='VIEW'";
            List<string> tabNames = new List<string>();
            OracleDataReader reader = ExecuteReader(strSql);
            while (reader.Read())
            {
                tabNames.Add(reader.GetString(0));
            }
            reader.Close();
            return tabNames;

        }
		/// <summary>
		/// 得到数据库的所有表和视图名
		/// </summary>
		/// <param name="DbName">数据库</param>
		/// <returns></returns>
		public DataTable GetTabViews(string DbName)
		{
            string strSql = "select TNAME name,TABTYPE type from tab where TABTYPE='TABLE' or TABTYPE='VIEW'";
			return Query("",strSql).Tables[0];
		}
        /// <summary>
        /// 得到数据库的所有存储过程名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<string> GetProcs(string DbName)
        {
            string strSql = "SELECT * FROM ALL_SOURCE  where TYPE='PROCEDURE'  ";
            //return Query(DbName, strSql).Tables[0];
            DataTable dtp = Query(DbName, strSql).Tables[0];
            List<string> nameList = new List<string>();
            if (dtp != null)
            {
                DataRow[] dRows = dtp.Select("", "name ASC");
                foreach (DataRow row in dRows)//循环每个表
                {
                    nameList.Add(row["name"].ToString());
                }
            }
            return nameList;
        }

		#endregion       

        #region  得到数据库的所有 表的详细信息 GetTablesInfo(string DbName)

        /// <summary>
		/// 得到数据库的所有表的详细信息
		/// </summary>
		/// <param name="DbName"></param>
		/// <returns></returns>
        public List<TableInfo> GetTablesInfo(string DbName)
		{
			string strSql="select TNAME name,'dbo' cuser,TABTYPE type,'' dates from tab where TABTYPE='TABLE'";
            
            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            OracleDataReader reader = ExecuteReader(strSql);
            while (reader.Read())
            {
                tab = new TableInfo();
                tab.TabName = reader.GetString(0);
                tab.TabDate = reader.GetValue(3).ToString();
                tab.TabType = reader.GetString(2);
                tab.TabUser = reader.GetString(1);
                tablist.Add(tab);
            }
            reader.Close();
            return tablist;
		}

        /// <summary>
        /// 得到所有表达扩展属性
        /// </summary>
        /// <param name="DbName"></param>
        /// <returns></returns>
        public DataTable GetTablesExProperty(string DbName)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(" select t.TNAME objname,c.TABLE_NAME name,c.comments value");
                strSql.Append(" from tab t,user_tab_comments c  ");
                strSql.Append(" where t.TABTYPE='TABLE' and t.TNAME =c.TABLE_NAME ");
                return Query(DbName, strSql.ToString()).Tables[0];
            }
            catch
            {
                return new DataTable();
            }
        }

        public List<TableInfo> GetVIEWsInfo(string DbName)
		{
			string strSql="select TNAME name,'dbo' cuser,TABTYPE type,'' dates from tab where TABTYPE='VIEW'";
            //return Query("",strSql).Tables[0];

            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            OracleDataReader reader = ExecuteReader(strSql);
            while (reader.Read())
            {
                tab = new TableInfo();
                tab.TabName = reader.GetString(0);
                tab.TabDate = reader.GetValue(3).ToString();
                tab.TabType = reader.GetString(2);
                tab.TabUser = reader.GetString(1);
                tablist.Add(tab);
            }
            reader.Close();
            return tablist;
		}
		/// <summary>
		/// 得到数据库的所有表和视图的详细信息
		/// </summary>
		/// <param name="DbName">数据库</param>
		/// <returns></returns>
        public List<TableInfo> GetTabViewsInfo(string DbName)
		{
			string strSql="select TNAME name,'dbo' cuser,TABTYPE type,'' dates from tab ";
            //return Query("",strSql).Tables[0];

            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            OracleDataReader reader = ExecuteReader(strSql);
            while (reader.Read())
            {
                tab = new TableInfo();
                tab.TabName = reader.GetString(0);
                tab.TabDate = reader.GetValue(3).ToString();
                tab.TabType = reader.GetString(2);
                tab.TabUser = reader.GetString(1);
                tablist.Add(tab);
            }
            reader.Close();
            return tablist;
		}
        /// <summary>
        /// 得到数据库的所有存储过程的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<TableInfo> GetProcInfo(string DbName)
        {
            string strSql = "SELECT * FROM ALL_SOURCE  where TYPE='PROCEDURE'  ";

            //return Query(DbName, strSql).Tables[0];
           
            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            OracleDataReader reader = ExecuteReader(strSql);
            while (reader.Read())
            {
                tab = new TableInfo();
                tab.TabName = reader.GetString(0);
                tab.TabDate = reader.GetValue(3).ToString();
                tab.TabType = reader.GetString(2);
                tab.TabUser = reader.GetString(1);
                tablist.Add(tab);
            }
            reader.Close();
            return tablist;

            //return null;
        }
		#endregion

        #region 得到对象定义语句
        /// <summary>
        /// 得到视图或存储过程的定义语句
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public string GetObjectInfo(string DbName, string objName)
        {
            //string strSql = "select DBMS_METADATA.GET_DDL('PROCEDURE',u.object_name) from user_objects u where object_type = 'PROCEDURE'";
            //string strSql = String.Format("select dbms_metadata.get_ddl('PROCEDURE','{0}','{1}') from dual;", objName,);            
            //return GetSingle(DbName, strSql.ToString()).ToString();
            return "";
        }
        #endregion

		#region 得到(快速)数据库里表的列名和类型 GetColumnList(string DbName,string TableName)

		/// <summary>
		/// 得到数据库里表的列名和类型
		/// </summary>
		/// <param name="DbName"></param>
		/// <param name="TableName"></param>
		/// <returns></returns>
        public List<ColumnInfo> GetColumnList(string DbName, string TableName)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
            strSql.Append("COLUMN_ID as colorder,");
			strSql.Append("COLUMN_NAME as ColumnName,");
			strSql.Append("DATA_TYPE as TypeName, ");
            strSql.Append("DECODE(DATA_TYPE, 'NUMBER',DATA_PRECISION, DATA_LENGTH) as Length ");
            
			strSql.Append(" from USER_TAB_COLUMNS ");
			strSql.Append(" where TABLE_NAME='"+TableName+"'");
			strSql.Append(" order by COLUMN_ID");					
            
            // select    A.column_name 字段名,A.data_type 数据类型,A.data_length 长度,A.data_precision 
            // 整数位,    A.Data_Scale 小数位,A.nullable 允许空值,A.Data_default 缺省值,B.comments 备注,
            // c.comments 表名 
            // from  user_tab_columns A,user_col_comments B,user_tab_comments C 
            // where a.COLUMN_NAME=b.column_name and
            // A.Table_Name = B.Table_Name and A.Table_Name='B_CITY' and a.TABLE_NAME=c.TABLE_NAME   

            List<ColumnInfo> collist = new List<ColumnInfo>();
            ColumnInfo col;
            OracleDataReader reader = ExecuteReader(strSql.ToString());
            while (reader.Read())
            {
                col = new ColumnInfo();
                col.ColumnOrder = reader.GetValue(0).ToString();
                col.ColumnName = reader.GetString(1);
                col.TypeName = reader.GetString(2);
                col.Length = reader.GetValue(3).ToString();
                col.Precision = "";
                col.Scale = "";
                col.IsPrimaryKey = false;
                col.Nullable = false;
                col.DefaultVal = "";
                col.IsIdentity = false                    ;
                col.Description = "";
                collist.Add(col);
            }
            reader.Close();
            return collist;
		}

		#endregion

		#region 得到数据库里表的 列的详细信息 GetColumnInfoList(string DbName,string TableName)

		/// <summary>
		/// 得到数据库里表的列的详细信息
		/// </summary>
		/// <param name="DbName"></param>
		/// <param name="TableName"></param>
		/// <returns></returns>
        public List<ColumnInfo> GetColumnInfoList(string DbName, string TableName)
		{
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ");
                strSql.Append("COL.COLUMN_ID as colorder,");
                strSql.Append("COL.COLUMN_NAME as ColumnName,");
                strSql.Append("COL.DATA_TYPE as TypeName, ");
                //strSql.Append("COL.DATA_LENGTH as Length,");
                strSql.Append("DECODE(COL.DATA_TYPE, 'NUMBER',COL.DATA_PRECISION, COL.DATA_LENGTH) as Length,");
                strSql.Append("COL.DATA_PRECISION as Preci,");
                strSql.Append("COL.DATA_SCALE as Scale,");
                strSql.Append("'' as IsIdentity,");
                //strSql.Append("PKCOL.COLUMN_POSITION as isPK,");//就是这儿，如果有多个主键，就会以1,2,3。。。这样的顺序下去
                strSql.Append("case when PKCOL.COLUMN_POSITION >0  then '√'else '' end as isPK,");                
                strSql.Append("case when COL.NULLABLE='Y'  then '√'else '' end as cisNull, ");
                strSql.Append("COL.DATA_DEFAULT as defaultVal, ");
                strSql.Append("CCOM.COMMENTS as deText,");
                strSql.Append("COL.NUM_DISTINCT as NUM_DISTINCT ");
                strSql.Append(" FROM USER_TAB_COLUMNS COL,USER_COL_COMMENTS CCOM, ");
                strSql.Append(" ( SELECT AA.TABLE_NAME, AA.INDEX_NAME, AA.COLUMN_NAME, AA.COLUMN_POSITION");
                strSql.Append(" FROM USER_IND_COLUMNS AA, USER_CONSTRAINTS BB");
                strSql.Append(" WHERE BB.CONSTRAINT_TYPE = 'P'");
                strSql.Append(" AND AA.TABLE_NAME = BB.TABLE_NAME");
                strSql.Append(" AND AA.INDEX_NAME = BB.CONSTRAINT_NAME");
                strSql.Append(" AND AA.TABLE_NAME IN ('" + TableName + "')");
                strSql.Append(") PKCOL");
                strSql.Append(" WHERE COL.TABLE_NAME = CCOM.TABLE_NAME");
                strSql.Append(" AND COL.COLUMN_NAME = CCOM.COLUMN_NAME");
                strSql.Append(" AND COL.TABLE_NAME ='" + TableName + "'");
                strSql.Append(" AND COL.COLUMN_NAME = PKCOL.COLUMN_NAME(+)");
                strSql.Append(" AND COL.TABLE_NAME = PKCOL.TABLE_NAME(+)");
                strSql.Append(" ORDER BY COL.COLUMN_ID ");

                //return Query("",strSql.ToString()).Tables[0];
                Hashtable fklist = GetForeignKey(DbName, TableName);

                List<ColumnInfo> collist = new List<ColumnInfo>();
                ColumnInfo col;
                OracleDataReader reader = ExecuteReader(strSql.ToString());
                while (reader.Read())
                {
                    col = new ColumnInfo();
                    col.ColumnOrder = reader.GetValue(0).ToString();
                    col.ColumnName = reader.GetValue(1).ToString();
                    col.TypeName = reader.GetValue(2).ToString();
                    col.Length = reader.GetValue(3).ToString();
                    col.Precision = reader.GetValue(4).ToString();
                    col.Scale = reader.GetValue(5).ToString();
                    col.IsIdentity = (reader.GetValue(6).ToString() == "√") ? true : false;
                    col.IsPrimaryKey = (reader.GetValue(7).ToString() == "√") ? true : false;
                    col.IsForeignKey = false;
                    if (fklist[TableName] != null && fklist[TableName].ToString() == col.ColumnName)
                    {
                        col.IsForeignKey = true;
                    }
                    col.Nullable = (reader.GetValue(8).ToString() == "√") ? true : false;
                    col.DefaultVal = reader.GetValue(9).ToString();
                    col.Description = reader.GetValue(10).ToString();
                    collist.Add(col);
                }
                reader.Close();
                return collist;

            }
            catch (System.Exception ex)
            {
                throw new Exception("获取列数据失败" + ex.Message);
            }

            #region
            //SELECT COL.TABLE_NAME as TNAME, 
            //TCOM.COMMENTS as TCMT,
            //COL.COLUMN_NAME as COL_NM, 
            //CCOM.COMMENTS as COL_CMT,
            //COL.COLUMN_ID ID,
            //PKCOL.COLUMN_POSITION AS PK,  --就是这儿，如果有多个主键，就会以1,2,3。。。这样的顺序下去
            //COL.DATA_TYPE as TYPE_CD, 
            //DECODE(COL.DATA_TYPE, 'NUMBER',COL.DATA_PRECISION ||'.'||COL.DATA_SCALE, COL.DATA_LENGTH) as LENGTH,
            //COL.NULLABLE as NULL_YN,
            //COL.DATA_DEFAULT as D_DEFAULT,
            //COL.NUM_DISTINCT as NUM_DISTINCT
            //FROM USER_TAB_COLUMNS COL,
            //USER_TAB_COMMENTS TCOM,
            //USER_COL_COMMENTS CCOM,
            //( SELECT AA.TABLE_NAME, AA.INDEX_NAME, AA.COLUMN_NAME, AA.COLUMN_POSITION
            //FROM USER_IND_COLUMNS AA, USER_CONSTRAINTS BB
            //WHERE BB.CONSTRAINT_TYPE = 'P'
            //AND AA.TABLE_NAME = BB.TABLE_NAME
            //AND AA.INDEX_NAME = BB.CONSTRAINT_NAME
            //-- AND AA.TABLE_NAME IN ('ACCOUNTMONEY') 
            //) PKCOL
            //WHERE COL.TABLE_NAME = TCOM.TABLE_NAME 
            //AND COL.TABLE_NAME = CCOM.TABLE_NAME 
            //AND COL.COLUMN_NAME = CCOM.COLUMN_NAME
            //AND COL.TABLE_NAME = 'ACCOUNTMONEY' 
            //AND COL.COLUMN_NAME = PKCOL.COLUMN_NAME(+)
            //AND COL.TABLE_NAME = PKCOL.TABLE_NAME(+)
            //ORDER BY COL.TABLE_NAME ,
            //COL.COLUMN_ID 
            #endregion

        }

		#endregion

        
		#region 得到数据库里表的主键 GetKeyName(string DbName,string TableName)

		public DataTable GetKeyName(string DbName, string TableName)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append("COL.COLUMN_ID as colorder,");
            strSql.Append("COL.COLUMN_NAME as ColumnName,");
            strSql.Append("COL.DATA_TYPE as TypeName, ");
            strSql.Append("COL.DATA_LENGTH as Length,");
            strSql.Append("COL.DATA_PRECISION as Preci,");
            strSql.Append("COL.DATA_SCALE as Scale,");
            strSql.Append("'' as IsIdentity,");
            //strSql.Append("PKCOL.COLUMN_POSITION as isPK,");//就是这儿，如果有多个主键，就会以1,2,3。。。这样的顺序下去
            strSql.Append("case when PKCOL.COLUMN_POSITION >0  then '√'else '' end as isPK,");

            //strSql.Append("COL.NULLABLE as cisNull,");
            strSql.Append("case when COL.NULLABLE='Y'  then '√'else '' end as cisNull, ");

            strSql.Append("COL.DATA_DEFAULT as defaultVal, ");
            strSql.Append("CCOM.COMMENTS as deText,");
            strSql.Append("COL.NUM_DISTINCT as NUM_DISTINCT ");
            strSql.Append(" FROM USER_TAB_COLUMNS COL,USER_COL_COMMENTS CCOM, ");
            strSql.Append(" ( SELECT AA.TABLE_NAME, AA.INDEX_NAME, AA.COLUMN_NAME, AA.COLUMN_POSITION");
            strSql.Append(" FROM USER_IND_COLUMNS AA, USER_CONSTRAINTS BB");
            strSql.Append(" WHERE BB.CONSTRAINT_TYPE = 'P'");
            strSql.Append(" AND AA.TABLE_NAME = BB.TABLE_NAME");
            strSql.Append(" AND AA.INDEX_NAME = BB.CONSTRAINT_NAME");
            strSql.Append(" AND AA.TABLE_NAME IN ('" + TableName + "')");
            strSql.Append(") PKCOL");
            strSql.Append(" WHERE COL.TABLE_NAME = CCOM.TABLE_NAME");
            strSql.Append(" AND PKCOL.COLUMN_POSITION >0");
            strSql.Append(" AND COL.COLUMN_NAME = CCOM.COLUMN_NAME");
            strSql.Append(" AND COL.TABLE_NAME ='" + TableName + "'");
            strSql.Append(" AND COL.COLUMN_NAME = PKCOL.COLUMN_NAME(+)");
            strSql.Append(" AND COL.TABLE_NAME = PKCOL.TABLE_NAME(+)");
            strSql.Append(" ORDER BY COL.COLUMN_ID ");	

            return Query("", strSql.ToString()).Tables[0];
		}

        /// <summary>
        /// 得到数据库里表的主键数组
        /// </summary>
        /// <param name="DbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public List<ColumnInfo> GetKeyList(string DbName, string TableName)
        {
            List<ColumnInfo> collist = GetColumnInfoList(DbName, TableName);
            List<ColumnInfo> keylist = new List<ColumnInfo>();
            foreach (ColumnInfo col in collist)
            {
                if (col.IsPrimaryKey || col.IsIdentity)
                {
                    keylist.Add(col);
                }
            }
            return keylist;
        }
        #endregion

        #region 得到数据库里表的外键 GetFKeyList

        private Hashtable GetForeignKey(string DbName, string TableName)
        {
            Hashtable fklist = new Hashtable();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select distinct(col.column_name),r.table_name,r.column_name  ");
                strSql.Append("from user_constraints con,user_cons_columns col,  ");
                strSql.Append("(select t2.table_name,t2.column_name,t1.r_constraint_name  ");
                strSql.Append(" from user_constraints t1,user_cons_columns t2  ");
                strSql.Append(" where t1.r_constraint_name=t2.constraint_name  ");
                strSql.Append(" and t1.table_name='" + TableName + "' ");
                strSql.Append(" ) r  ");
                strSql.Append("where con.constraint_name=col.constraint_name  ");
                strSql.Append("and con.r_constraint_name=r.r_constraint_name  ");
                strSql.Append("and con.table_name='" + TableName + "'  ");

                OracleDataReader reader = ExecuteReader(strSql.ToString());
                while (reader.Read())
                {
                    string colname = reader.GetValue(0).ToString();
                    string tabname = reader.GetValue(1).ToString();
                    if (!fklist.Contains(tabname))
                    {
                        fklist.Add(tabname, colname);
                    }
                }
                reader.Close();

            }
            catch //(System.Exception ex)
            {
            }
            return fklist;
        }

        /// <summary>
        /// 得到数据库里表的外键数组
        /// </summary>
        /// <param name="DbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public List<ColumnInfo> GetFKeyList(string DbName, string TableName)
        {
            List<ColumnInfo> collist = GetColumnInfoList(DbName, TableName);
            List<ColumnInfo> keylist = new List<ColumnInfo>();
            foreach (ColumnInfo col in collist)
            {
                if (col.IsForeignKey)
                {
                    keylist.Add(col);
                }
            }
            return keylist;
        }

        #endregion


		#region 得到数据表里的数据 GetTabData(string DbName,string TableName)

		/// <summary>
		/// 得到数据表里的数据
		/// </summary>
		/// <param name="DbName"></param>
		/// <param name="TableName"></param>
		/// <returns></returns>
		public DataTable GetTabData(string DbName, string TableName)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select * from " + TableName + "");
			return Query("",strSql.ToString()).Tables[0];
		}
        /// <summary>
        /// 根据SQL查询得到数据表里的数据
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public DataTable GetTabDataBySQL(string DbName, string strSQL)
        {
            return Query("", strSQL).Tables[0];
        }
		#endregion

		#region 数据库属性操作

		public bool RenameTable(string DbName, string OldName, string NewName)
		{
            try
            {
                string strsql = "RENAME " + OldName + " TO " + NewName + "";
                ExecuteSql(DbName, strsql);
                return true;
            }
            catch//(System.Exception ex)
            {
                //string str=ex.Message;	
                return false;
            }		
		}

		public bool DeleteTable(string DbName, string TableName)
		{
			try
			{				
				string strsql="DROP TABLE "+TableName+"";
                ExecuteSql(DbName, strsql);
				return true;
			}
			catch//(System.Exception ex)
			{
				//string str=ex.Message;	
				return false;
			}			
		}
		/// <summary>
		/// 得到版本号
		/// </summary>
		/// <returns></returns>
		public string GetVersion()
		{			
			return "";				
		}
		#endregion

		
	}
}
