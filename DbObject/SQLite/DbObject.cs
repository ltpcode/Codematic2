using System;
using System.Data;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Maticsoft.CodeHelper;
using System.Data.SQLite;
using Maticsoft.IDBO;
namespace Maticsoft.DbObjects.SQLite
{
    /// <summary>
    /// SQLite数据库信息类。
    /// </summary>
    public class DbObject : IDbObject
    {

        #region  属性
        public string DbType
        {
            get { return "SQLite"; }
        }
        private string _dbconnectStr;
        public string DbConnectStr
        {
            set { _dbconnectStr = value; }
            get { return _dbconnectStr; }
        }
        SQLiteConnection connect = new SQLiteConnection ();

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
            _dbconnectStr = DbConnectStr;
            connect.ConnectionString = DbConnectStr;
        }
        /// <summary>
        /// 构造一个连接字符串
        /// </summary>
        /// <param name="SSPI">是否windows集成认证</param>
        /// <param name="Ip">服务器IP</param>
        /// <param name="User">用户名</param>
        /// <param name="Pass">密码</param>
        public DbObject(bool SSPI, string Ip, string User, string Pass)
        {
            connect = new SQLiteConnection ();
            if (SSPI)
            {                
                _dbconnectStr = String.Format("Data Source={0}; Password={1}", Ip, Pass);
            }
            else
            {
                _dbconnectStr = String.Format("Data Source={0};Password={1}", Ip, Pass);

            }
            connect.ConnectionString = _dbconnectStr;

        }
        #endregion
            

        #region 打开数据库 OpenDB(string DbName)

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="DbName">要打开的数据库</param>
        /// <returns></returns>
        private SQLiteCommand OpenDB()
        {
            try
            {
                if (connect.ConnectionString == "")
                {
                    connect.ConnectionString = _dbconnectStr;
                }
                if (connect.ConnectionString != _dbconnectStr)
                {
                    connect.Close();
                    connect.ConnectionString = _dbconnectStr;
                }
                SQLiteCommand dbCommand = new SQLiteCommand();
                dbCommand.Connection = connect;
                if (connect.State == System.Data.ConnectionState.Closed)
                {
                    connect.Open();
                }                
                return dbCommand;

            }
            catch (System.Exception ex)
            {
                string str = ex.Message;
                return null;
            }

        }
        #endregion

        #region ADO.NET 操作

        public int ExecuteSql(string DbName, string SQLString)
        {
            SQLiteCommand dbCommand = OpenDB();
            dbCommand.CommandText = SQLString;
            int rows = dbCommand.ExecuteNonQuery();
            return rows;
        }
        public DataSet Query(string DbName, string SQLString)
        {
            DataSet ds = new DataSet();
            try
            {
                OpenDB();
                SQLiteDataAdapter command = new SQLiteDataAdapter(SQLString, connect);
                command.Fill(ds, "ds");
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
        public SQLiteDataReader ExecuteReader(string DbName, string strSQL)
        {
            try
            {
                OpenDB();
                SQLiteCommand cmd = new SQLiteCommand(strSQL, connect);
                SQLiteDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                throw ex;
            }
        }
        public object GetSingle(string DbName, string SQLString)
        {
            try
            {
                SQLiteCommand dbCommand = OpenDB();
                dbCommand.CommandText = SQLString;
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

        #region 公共方法

        /// <summary>
        /// List根据字符串排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0代表相等，-1代表y大于x，1代表x大于y</returns>
        private int CompareStrByOrder(string x, string y)
        {
            if (x == "")
            {
                if (y == "")
                {
                    return 0;// If x is null and y is null, they're equal. 
                }
                else
                {
                    return -1;// If x is null and y is not null, y is greater. 
                }
            }
            else
            {
                if (y == "")  // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    int retval = x.CompareTo(y);
                    return retval;
                }
            }
        }
        /// <summary>
        /// 根据字符串转int排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0代表相等，-1代表y大于x，1代表x大于y</returns>
        private int CompareDinosByintOrder(ColumnInfo x, ColumnInfo y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y is greater. 
                    return -1;
                }
            }
            else
            {
                if (y == null)  // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the lengths of the two strings.
                    //int retval = x.Colorder.CompareTo(y.Colorder);
                    //return retval;
                    int n = 0;
                    int m = 0;
                    try
                    {
                        n = Convert.ToInt32(x.ColumnOrder);
                    }
                    catch
                    {
                        return -1;
                    }
                    try
                    {
                        m = Convert.ToInt32(y.ColumnOrder);
                    }
                    catch
                    {
                        return 1;
                    }

                    if (n < m)
                    {
                        return -1;
                    }
                    else
                        if (x == y)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }

                }
            }
        }

        /// <summary>
        /// 根据字符串排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0代表相等，-1代表y大于x，1代表x大于y</returns>
        private int CompareDinosByOrder(ColumnInfo x, ColumnInfo y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y is greater. 
                    return -1;
                }
            }
            else
            {
                if (y == null)  // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the lengths of the two strings.     

                    int retval = x.ColumnOrder.CompareTo(y.ColumnOrder);
                    return retval;

                    //if (retval != 0)
                    //{
                    //    // If the strings are not of equal length,
                    //    // the longer string is greater.
                    //    return retval;
                    //}
                    //else
                    //{
                    //    // If the strings are of equal length,
                    //    // sort them with ordinary string comparison.
                    //    return x.CompareTo(y);
                    //}
                }
            }
        }
               
        #endregion


        #region 得到数据库的名字列表 GetDBList()
        public List<string> GetDBList()
        {            
            return null;
        }
        #endregion

        #region  得到数据库的所有表和视图 的名字

        private DataTable Tab2Tab(DataTable sTable)
        {
            DataTable tTable = new DataTable();
            tTable.Columns.Add("name");
            tTable.Columns.Add("cuser");
            tTable.Columns.Add("type");
            tTable.Columns.Add("dates");
            foreach (DataRow row in sTable.Rows)
            {
                DataRow newRow = tTable.NewRow();
                newRow["name"] = row[2].ToString();
                newRow["cuser"] = "dbo";
                newRow["type"] = row[3].ToString();
                newRow["dates"] = row[6].ToString();
                tTable.Rows.Add(newRow);
            }
            return tTable;
        }
        
        /// <summary>
        /// 得到数据库的所有表名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<string> GetTables(string DbName)
        {
            string strSql = "select name from sqlite_master where type='table' AND name NOT LIKE 'sqlite_%' order by name";
            List<string> tabNames = new List<string>();
            SQLiteDataReader reader = ExecuteReader(DbName, strSql);
            while (reader.Read())
            {
                tabNames.Add(reader.GetString(0));
            }
            reader.Close();
            //tabNames.Sort(CompareStrByOrder);
            return tabNames;
        }
        
        /// <summary>
        /// 得到数据库的所有视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetVIEWs(string DbName)
        {
            string strSql = "select name from sqlite_master WHERE type='view' AND name NOT LIKE 'sqlite_%' order by name";//order by id
            DataTable dt = Query(DbName, strSql).Tables[0];
            dt.Columns[0].ColumnName = "name";
            return dt;
        }

        /// <summary>
        /// 得到数据库的所有表和视图名字
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<string> GetTableViews(string DbName)
        {
            string strSql = "select name from sqlite_master WHERE type IN ('table','view') AND name NOT LIKE 'sqlite_%' order by name";//order by id
            List<string> tabNames = new List<string>();
            SQLiteDataReader reader = ExecuteReader(DbName, strSql);
            while (reader.Read())
            {
                tabNames.Add(reader.GetString(0));
            }
            reader.Close();
            //tabNames.Sort(CompareStrByOrder);
            return tabNames;
        }

        /// <summary>
        /// 得到数据库的所有表和视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetTabViews(string DbName)
        {
            string strSql = "select name from sqlite_master WHERE type IN ('table','view') AND name NOT LIKE 'sqlite_%' order by name";//order by id
            DataTable dt = Query(DbName, strSql).Tables[0];
            dt.Columns[0].ColumnName = "name";
            return dt;
        }
        
        /// <summary>
        /// 得到数据库的所有存储过程名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<string> GetProcs(string DbName)
        {
            return new List<string>();
        }
        #endregion

        #region  得到数据库的所有表的详细信息 GetTablesInfo(string DbName)
        /// <summary>
        /// 得到数据库的所有表的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<TableInfo> GetTablesInfo(string DbName)
        {
            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            string strSql = "select * from sqlite_master where type='table' AND name NOT LIKE 'sqlite_%' order by name";
            SQLiteDataReader reader = ExecuteReader(DbName, strSql);
            while (reader.Read())
            {

                tab = new TableInfo();
                tab.TabName = reader["Name"].ToString();
                //try
                //{
                //    if (reader["Create_time"] != null)
                //    {
                //        tab.TabDate = reader["Create_time"].ToString();
                //    }
                //}
                //catch
                //{ }
                tab.TabType = "U";
                tab.TabUser = "";
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
            return null;
        }

        /// <summary>
        /// 得到数据库的所有视图的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<TableInfo> GetVIEWsInfo(string DbName)
        {
            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            string strSql = "select * from sqlite_master where type='view' AND name NOT LIKE 'sqlite_%' order by name";
            SQLiteDataReader reader = ExecuteReader(DbName, strSql);
            while (reader.Read())
            {

                tab = new TableInfo();
                tab.TabName = reader["Name"].ToString();
                //try
                //{
                //    if (reader["Create_time"] != null)
                //    {
                //        tab.TabDate = reader["Create_time"].ToString();
                //    }
                //}
                //catch
                //{ }
                tab.TabType = "U";
                tab.TabUser = "";
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
            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            string strSql = "select * from sqlite_master WHERE type IN ('table','view') AND name NOT LIKE 'sqlite_%' order by name";
            SQLiteDataReader reader = ExecuteReader(DbName, strSql);
            while (reader.Read())
            {
                tab = new TableInfo();
                tab.TabName = reader["Name"].ToString();
                //try
                //{
                //    if (reader["Create_time"] != null)
                //    {
                //        tab.TabDate = reader["Create_time"].ToString();
                //    }
                //}
                //catch
                //{ }               
                tab.TabType = "U";
                tab.TabUser = "";
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
            return null;
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sql ");
            strSql.Append("from sqlite_master   ");
            strSql.Append("where name= '" + objName+"'");    
            //return GetSingle(DbName, strSql.ToString()).ToString();
            object obj = GetSingle(DbName, strSql.ToString());
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 得到(快速)数据库里表的列名和类型 GetColumnList(string DbName,string TableName)

        
        private DataTable Tab2Colum(DataTable sTable)
        {
            DataTable tTable = new DataTable();
            tTable.Columns.Add("colorder");
            tTable.Columns.Add("ColumnName");
            tTable.Columns.Add("TypeName");
            tTable.Columns.Add("Length");
            tTable.Columns.Add("Preci");
            tTable.Columns.Add("Scale");
            tTable.Columns.Add("IsIdentity");
            tTable.Columns.Add("isPK");
            tTable.Columns.Add("cisNull");
            tTable.Columns.Add("defaultVal");
            tTable.Columns.Add("deText");

            int n = 0;
            foreach (DataRow row in sTable.Select("", "ORDINAL_POSITION asc"))
            {
                DataRow newRow = tTable.NewRow();
                newRow["colorder"] = row[6].ToString();
                newRow["ColumnName"] = row[3].ToString();
                //string type = GetColumnType(row[11].ToString());

                newRow["TypeName"] = row[11].ToString();
                newRow["Length"] = row[13].ToString();
                newRow["Preci"] = row[15].ToString();
                newRow["Scale"] = row[16].ToString();
                newRow["IsIdentity"] = "";
                newRow["isPK"] = "";
                if (row[10].ToString().ToLower() == "true")
                {
                    newRow["cisNull"] = "";
                }
                else
                {
                    newRow["cisNull"] = "√";
                }
                newRow["defaultVal"] = row[8].ToString();
                newRow["deText"] = "";
                tTable.Rows.Add(newRow);
                n++;
            }

            return tTable;
        }


        /// <summary>
        /// 得到数据库里表或视图的列名和类型
        /// </summary>
        /// <param name="DbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public List<ColumnInfo> GetColumnList(string DbName, string TableName)
        {
            return GetColumnInfoList(DbName, TableName);

        }        
        #endregion


        #region 得到表的列的详细信息 GetColumnInfoList(string DbName,string TableName)
        /// <summary>
        /// 得到数据库里表或视图的列的详细信息
        /// </summary>
        /// <param name="DbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public List<ColumnInfo> GetColumnInfoList(string DbName, string TableName)
        {
            OpenDB();
            List<ColumnInfo> collist = new List<ColumnInfo>();
            try
            {

                //DataTable data = connect.GetSchema("TABLES");                
                DataTable schemaTable = connect.GetSchema("Columns", new string[] { null, null, TableName });

                //Hashtable pklist = GetPrimaryKey(DbName, TableName);

                ColumnInfo col;
                #region
                //foreach (DataColumn colu in schemaTable.Columns)
                //{
                //    col = new ColumnInfo();

                //    #region 根据列名取值
                //    try
                //    {
                //        col.Colorder = colu.Ordinal.ToString();
                //        col.ColumnName = colu.ColumnName;
                //        col.TypeName = GetColumnType(colu.DataType.Name);
                //        col.Length = colu.MaxLength.ToString();
                //        //col.Preci = colu.DataType.
                //        //col.Scale = colu.DataType.sc
                //        if (colu.AllowDBNull)
                //        {
                //            col.cisNull = true;
                //        }
                //        else
                //        {
                //            col.cisNull = false;
                //        }
                                                
                //        if (colu.DefaultValue!=null)
                //        {
                //            col.DefaultVal = colu.DefaultValue.ToString();
                //        }
                //        col.DeText = colu.Caption;
                //        col.IsPK = false;
                //        if (pklist[TableName] != null && pklist[TableName].ToString() == col.ColumnName)
                //        {
                //            col.IsPK = true;
                //        }                        
                //        col.IsIdentity = colu.AutoIncrement;
                        
                //    }
                //    catch
                //    {
                //    }
                //    #endregion

                //    collist.Add(col);

                //}
                #endregion

                #region
                foreach (DataRow row in schemaTable.Rows)
                {
                    col = new ColumnInfo();
                                        
                    #region 根据列名取值
                    try
                    {
                        col.ColumnOrder = row["ORDINAL_POSITION"].ToString();
                        col.ColumnName = row["COLUMN_NAME"].ToString();
                        //string type = GetColumnType(row["DATA_TYPE"].ToString());

                        col.TypeName = row["DATA_TYPE"].ToString();
                        col.Length = row["CHARACTER_MAXIMUM_LENGTH"].ToString();
                        col.Precision = row["NUMERIC_PRECISION"].ToString();
                        col.Scale = row["NUMERIC_SCALE"].ToString();
                        if (row["IS_NULLABLE"].ToString().ToLower() == "true")
                        {
                            col.Nullable = true;
                        }
                        else
                        {
                            col.Nullable = false;
                        }
                                                
                        if (row["COLUMN_HASDEFAULT"].ToString().ToLower() == "true")
                        {
                            col.DefaultVal = row["COLUMN_DEFAULT"].ToString();
                        }
                        col.Description = row["DESCRIPTION"].ToString();
                        col.IsPrimaryKey = false;
                        if (row["PRIMARY_KEY"].ToString().ToLower() == "true")
                        {
                            col.IsPrimaryKey = true;
                        }
                        string flags = row["COLUMN_FLAGS"].ToString().Trim();
                        col.IsIdentity = false;
                        //if (row["DATA_TYPE"].ToString().Trim() == "3" && flags == "90")
                        if (row["AUTOINCREMENT"].ToString().ToLower() == "true")
                        {
                            col.IsIdentity = true;
                        }
                    }
                    catch
                    {
                    }
                    #endregion

                    collist.Add(col);
                }
                #endregion

                #region 排序
                collist.Sort(CompareDinosByintOrder);
                #endregion
            }
            catch
            {
            }
            return collist;
        }
        
        public DataTable GetColumnInfoListSP(string DbName, string TableName)
        {
            return null;
        }
        #endregion


        #region 得到数据库里表的主键 GetKeyName(string DbName,string TableName)

        /// <summary>
        /// 得到数据库里表的主键
        /// </summary>
        /// <param name="DbName">库</param>
        /// <param name="TableName">表</param>
        /// <returns></returns>
        public DataTable GetKeyName(string DbName, string TableName)
        {
            DataTable tTable = new DataTable();
            try
            {
                OpenDB();
                DataTable schemaTable = connect.GetSchema("Columns", new string[] { null, null, TableName });                                
                tTable.Columns.Add("colorder");
                tTable.Columns.Add("ColumnName");
                tTable.Columns.Add("TypeName");
                tTable.Columns.Add("Length");
                tTable.Columns.Add("Preci");
                tTable.Columns.Add("Scale");
                tTable.Columns.Add("IsIdentity");
                tTable.Columns.Add("isPK");
                tTable.Columns.Add("cisNull");
                tTable.Columns.Add("defaultVal");
                tTable.Columns.Add("deText");

                DataRow newRow;
                foreach (DataRow row in schemaTable.Rows)
                {
                    if (row["PRIMARY_KEY"]!=null && row["PRIMARY_KEY"].ToString().ToLower() == "true")
                    {
                        newRow = tTable.NewRow();

                        #region 根据列名取值
                        newRow["colorder"] = row["ORDINAL_POSITION"].ToString();
                        newRow["ColumnName"] = row["COLUMN_NAME"].ToString();
                        newRow["TypeName"] = row["DATA_TYPE"].ToString();// GetColumnType(row["DATA_TYPE"].ToString());
                        newRow["Length"] = row["CHARACTER_MAXIMUM_LENGTH"].ToString();
                        newRow["Preci"] = row["NUMERIC_PRECISION"].ToString();
                        newRow["Scale"] = row["NUMERIC_SCALE"].ToString();
                        newRow["IsIdentity"] = row["AUTOINCREMENT"].ToString().ToLower() == "true" ? "√" : "";
                        newRow["isPK"] = "√";
                        newRow["cisNull"] = row["IS_NULLABLE"].ToString().ToLower() == "true" ? "√" : "";
                        newRow["deText"] = row["DESCRIPTION"].ToString();
                                               
                        #endregion

                        tTable.Rows.Add(newRow);
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                string str = ex.Message;
                
            }
            return tTable;
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

        #region 得到数据库里表的外键 GetFKeyName

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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from " + TableName + "");
            return Query(DbName, strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 根据SQL查询得到数据表里的数据
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public DataTable GetTabDataBySQL(string DbName, string strSQL)
        {
            return Query(DbName, strSQL).Tables[0];
        }

        #endregion


        #region 数据库属性操作

        /// <summary>
        /// 修改表名称
        /// </summary>
        /// <param name="OldName"></param>
        /// <param name="NewName"></param>
        /// <returns></returns>
        public bool RenameTable(string DbName, string OldName, string NewName)
        {
            try
            {
                SQLiteCommand dbCommand = OpenDB();
                dbCommand.CommandText = "RENAME TABLE " + OldName + " TO " + NewName + "";
                dbCommand.ExecuteNonQuery();
                return true;
            }
            catch//(System.Exception ex)
            {
                //string str=ex.Message;	
                return false;
            }
        }

        /// <summary>
        /// 删除表
        /// </summary>	
        public bool DeleteTable(string DbName, string TableName)
        {
            try
            {
                SQLiteCommand dbCommand = OpenDB();
                dbCommand.CommandText = "DROP TABLE " + TableName + "";
                dbCommand.ExecuteNonQuery();
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


        ///// <summary>
        ///// 得到创建表 的脚本
        ///// </summary>
        ///// <returns></returns>
        //public string GetTableScript(string DbName, string TableName)
        //{
        //    string strScript = "";
        //    string strSql = "SHOW CREATE TABLE " + TableName;
        //    SQLiteDataReader reader = ExecuteReader(DbName, strSql);
        //    while (reader.Read())
        //    {
        //        strScript = reader.GetString(1);
        //    }
        //    reader.Close();
        //    return strScript;

        //}
        #endregion



    }
}
