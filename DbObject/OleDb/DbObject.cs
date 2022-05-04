using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Maticsoft.CodeHelper;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using Maticsoft.IDBO;
namespace Maticsoft.DbObjects.OleDb
{
    /// <summary>
    /// OleDb 数据库信息类。
    /// </summary>
    public class DbObject : IDbObject
    {
        //string datatypefile = Application.StartupPath + @"\datatype.ini";
        //Maticsoft.Utility.INIFile dtfile;
        string datatypefile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\DatatypeMap.cfg";
        

        #region  属性
        public string DbType
        {
            get { return "OleDb"; }
        }
        private string _dbconnectStr;
        public string DbConnectStr
        {
            set { _dbconnectStr = value; }
            get { return _dbconnectStr; }
        }
        OleDbConnection connect = new OleDbConnection();

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
        public DbObject(bool SSPI, string server, string User, string Pass)
        {
            connect = new OleDbConnection();            
            _dbconnectStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + server + ";Persist Security Info=False";
            connect.ConnectionString = _dbconnectStr;
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
                if (connect.ConnectionString == "")
                {
                    connect.ConnectionString = _dbconnectStr;
                }
                if (connect.ConnectionString != _dbconnectStr)
                {
                    connect.Close();
                    connect.ConnectionString = _dbconnectStr;
                }
                if (connect.State == System.Data.ConnectionState.Closed)
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

        public int ExecuteSql(string DbName, string SQLString)
        {
            OpenDB();
            OleDbCommand dbCommand = new OleDbCommand(SQLString, connect);
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
                OleDbDataAdapter command = new OleDbDataAdapter(SQLString, connect);
                command.Fill(ds, "ds");
            }
            catch (System.Data.OleDb.OleDbException ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }

        public OleDbDataReader ExecuteReader(string strSQL)
        {
            try
            {
                OpenDB();
                OleDbCommand cmd = new OleDbCommand(strSQL, connect);
                OleDbDataReader myReader = cmd.ExecuteReader();
                return myReader;
            }
            catch (System.Data.OleDb.OleDbException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public object GetSingle(string DbName, string SQLString)
        {
            try
            {
                OpenDB();
                OleDbCommand dbCommand = new OleDbCommand(SQLString, connect);
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
        /// 根据字符串排序
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

        #endregion

        #region 得到数据库的名字列表 GetDBList()
        /// <summary>
        /// 得到数据库的名字列表
        /// </summary>
        /// <returns></returns>
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
        /// <param name="DbName"></param>
        /// <returns></returns>
        public List<string> GetTables(string DbName)
        {
            OpenDB();
            DataTable schemaTable = connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                new object[] { null, null, null, "TABLE" });           
            List<string> tabNames = new List<string>();            
            foreach (DataRow row in schemaTable.Rows)
            {
                tabNames.Add(row[2].ToString());
            }
            tabNames.Sort(CompareStrByOrder);
            return tabNames;
        }
                
        public DataTable GetVIEWs(string DbName)
        {
            OpenDB();
            DataTable schemaTable = connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                new object[] { null, null, null, "VIEW" });
            return Tab2Tab(schemaTable);
        }
        /// <summary>
        /// 得到数据库的所有表和视图名字
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<string> GetTableViews(string DbName)
        {
            return GetTables(DbName);

        }
        /// <summary>
        /// 得到数据库的所有表和视图名
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public DataTable GetTabViews(string DbName)
        {
            OpenDB();
            DataTable schemaTable = connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            return Tab2Tab(schemaTable);
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
        /// <param name="DbName"></param>
        /// <returns></returns>
        public List<TableInfo> GetTablesInfo(string DbName)
        {
            OpenDB();
            DataTable schemaTable = connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                new object[] { null, null, null, "TABLE" });
            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            foreach (DataRow row in schemaTable.Rows)
            {
                tab = new TableInfo();
                tab.TabName = row[2].ToString();
                tab.TabUser = "dbo";
                tab.TabType = row[3].ToString();
                tab.TabDate = row[6].ToString();
                tablist.Add(tab);
            }
            #region 排序
            tablist.Sort(CompareTabByOrder);
            #endregion
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


        public List<TableInfo> GetVIEWsInfo(string DbName)
        {
            OpenDB();
            DataTable schemaTable = connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                new object[] { null, null, null, "VIEW" });
            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            foreach (DataRow row in schemaTable.Rows)
            {
                tab = new TableInfo();
                tab.TabName = row[2].ToString();
                tab.TabUser = "dbo";
                tab.TabType = row[3].ToString();
                tab.TabDate = row[6].ToString();
                tablist.Add(tab);
            }
            #region 排序
            tablist.Sort(CompareTabByOrder);
            #endregion
            return tablist;
        }

        /// <summary>
        /// 得到数据库的所有表和视图的详细信息
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        public List<TableInfo> GetTabViewsInfo(string DbName)
        {
            OpenDB();
            DataTable schemaTable = connect.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);            
            List<TableInfo> tablist = new List<TableInfo>();
            TableInfo tab;
            foreach (DataRow row in schemaTable.Rows)
            {
                tab = new TableInfo();
                tab.TabName = row[2].ToString();
                tab.TabUser = "dbo";
                tab.TabType = row[3].ToString();
                tab.TabDate = row[6].ToString();
                tablist.Add(tab);
            }
            
            #region 排序
            tablist.Sort(CompareTabByOrder);
            #endregion

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

        /// <summary>
        /// 根据字符串排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0代表相等，-1代表y大于x，1代表x大于y</returns>
        private int CompareTabByOrder(TableInfo x, TableInfo y)
        {
            if (x == null)
            {
                if (y == null)
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
                if (y == null)  // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    int retval = x.TabName.CompareTo(y.TabName);
                    return retval;                   
                }
            }
        }

        #endregion

        #region 得到对象定义语句
        public string GetObjectInfo(string DbName, string objName)
        {
            return null;
        }
        #endregion

        #region 得到(快速)数据库里表的列名和类型 GetColumnList(string DbName,string TableName)
        
        /// <summary>
        /// 得到字段类型字符串
        /// </summary>
        /// <param name="typeNum"></param>
        /// <returns></returns>
        private string GetColumnType(string typeNum)
        {
            string type = typeNum;
            if (File.Exists(datatypefile))
            {
                //dtfile = new Maticsoft.Utility.INIFile(datatypefile);
                //type = dtfile.IniReadValue("AccessDbTypeMap", typeNum);

                type = Maticsoft.CmConfig.DatatypeMap.GetValueFromCfg(datatypefile, "AccessTypeMap", typeNum);
            }                       
            //switch (typeNum)
            //{
            //    case "3":
            //        type = "int";
            //        break;
            //    case "5":
            //        type = "float";
            //        break;
            //    case "6":
            //        type = "money";
            //        break;
            //    case "7":
            //        type = "datetime";
            //        break;
            //    case "11":
            //        type = "bool";
            //        break;
            //    case "130":
            //        type = "varchar";
            //        break;
            //}
            return type;
        }

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
                string type = GetColumnType(row[11].ToString());
                
                newRow["TypeName"] = type;
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
        /// 得到数据库里表的列名和类型
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public List<ColumnInfo> GetColumnList(string DbName, string TableName)
        {
            return GetColumnInfoList(DbName, TableName);
        }

        #endregion

        #region 得到数据库里表的列的详细信息 GetColumnInfoList(string DbName,string TableName)

        /// <summary>
        /// 得到数据库里表的列的详细信息
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public List<ColumnInfo> GetColumnInfoList(string DbName, string TableName)
        {
            OpenDB();
            List<ColumnInfo> collist = new List<ColumnInfo>();
            try
            {
                DataTable schemaTable = connect.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
                    new Object[] { null, null, TableName, null });

                Hashtable pklist = GetPrimaryKey(DbName, TableName);
                Hashtable fklist = GetForeignKey(DbName, TableName);

                ColumnInfo col;
                foreach (DataRow row in schemaTable.Rows)
                {
                    col = new ColumnInfo();


                    #region 根据序号取值
                    //col.Colorder = row[6].ToString();
                    //col.ColumnName = row[3].ToString();
                    //string type = GetColumnType(row[11].ToString());

                    //col.TypeName = type;
                    //col.Length = row[13].ToString();
                    //col.Preci = row[15].ToString();
                    //col.Scale = row[16].ToString();
                    //if (row[10].ToString().ToLower() == "true")
                    //{
                    //    col.cisNull = false;
                    //}
                    //else
                    //{
                    //    col.cisNull = true;
                    //}

                    //int IsHasDefault = Convert.ToInt16(row["COLUMN_HASDEFAULT"]);
                    //if (IsHasDefault == 1)
                    //{
                    //    col.DefaultVal = row[8].ToString();
                    //}
                    //col.DeText = "";
                    //col.IsPK = false;
                    //if (pklist.Contains(col.ColumnName))
                    //{
                    //    col.IsPK = true;
                    //}
                    //col.IsIdentity = false;

                    #endregion

                    #region 根据列名取值
                    try
                    {
                        col.ColumnOrder = row["ORDINAL_POSITION"].ToString();
                        col.ColumnName = row["COLUMN_NAME"].ToString();
                        string type = GetColumnType(row["DATA_TYPE"].ToString());

                        col.TypeName = type;
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

                        //int IsHasDefault = Convert.ToInt16(row["COLUMN_HASDEFAULT"]);
                        //if (IsHasDefault == 1)
                        if (row["COLUMN_HASDEFAULT"].ToString().ToLower() == "true")
                        {
                            col.DefaultVal = row["COLUMN_DEFAULT"].ToString();
                        }
                        col.Description = row["DESCRIPTION"] == null ? col.ColumnName : row["DESCRIPTION"].ToString();
                        col.IsPrimaryKey = false;
                        if (pklist[TableName]!=null && pklist[TableName].ToString() == col.ColumnName)
                        {
                            col.IsPrimaryKey = true;
                        }
                        col.IsForeignKey = false;
                        if (fklist[TableName] != null && fklist[TableName].ToString() == col.ColumnName)
                        {
                            col.IsForeignKey = true;
                        }
                        string flags = row["COLUMN_FLAGS"].ToString().Trim();
                        col.IsIdentity = false;
                        if (row["DATA_TYPE"].ToString().Trim() == "3" && flags == "90")
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

                #region 排序
                collist.Sort(CompareDinosByintOrder);
                #endregion
            }
            catch
            { 
            }
            return collist;
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


        public DataTable GetColumnInfoListSP(string DbName, string ViewName)
        {            
            return null;
        }

        #endregion


        #region 得到数据库里表的主键 GetKeyName(string DbName,string TableName)
              
        public DataTable GetKeyName(string DbName, string TableName)
        {
            DataTable tTable = new DataTable();
            try
            {
                OpenDB();
                DataTable schemaTable = connect.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
                    new Object[] { null, null, TableName, null });

                Hashtable pklist = GetPrimaryKey(DbName, TableName);

                
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

                DataRow newRow ;
                foreach (DataRow row in schemaTable.Rows)
                {
                    string tabname = row["TABLE_NAME"].ToString();
                    string colname = row["COLUMN_NAME"].ToString();

                    if (pklist[TableName]!=null && tabname == TableName && pklist[TableName].ToString() == colname)
                    {
                        newRow = tTable.NewRow();

                        #region 根据列名取值
                        newRow["colorder"] = row["ORDINAL_POSITION"].ToString();
                        newRow["ColumnName"] = row["COLUMN_NAME"].ToString();
                        newRow["TypeName"] = GetColumnType(row["DATA_TYPE"].ToString());
                        newRow["Length"] = row["CHARACTER_MAXIMUM_LENGTH"].ToString();
                        newRow["Preci"] = row["NUMERIC_PRECISION"].ToString();
                        newRow["Scale"] = row["NUMERIC_SCALE"].ToString();
                        newRow["IsIdentity"] = "";
                        newRow["isPK"] = "√";
                        newRow["cisNull"] = "";                        
                        newRow["deText"] = "";
                        
                        if (row["IS_NULLABLE"].ToString().ToLower() == "true")
                        {
                            newRow["cisNull"] = "√";
                        }                        
                        if (row["COLUMN_HASDEFAULT"].ToString().ToLower() == "true")
                        {
                            newRow["defaultVal"] = row["COLUMN_DEFAULT"].ToString();
                        }                                                
                        string flags = row["COLUMN_FLAGS"].ToString().Trim();                        
                        if (row["DATA_TYPE"].ToString().Trim() == "3" && flags == "90")
                        {
                            newRow["IsIdentity"] = "√";
                        }
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


        public List<ColumnInfo> GetKeyNamelist(string DbName, string TableName)
        {
            OpenDB();
            DataTable schemaTable = connect.GetOleDbSchemaTable(OleDbSchemaGuid.Columns,
                new Object[] { null, null, TableName, null });

            Hashtable pklist = GetPrimaryKey(DbName, TableName);
            List<ColumnInfo> collist = new List<ColumnInfo>();
            ColumnInfo col;
            foreach (DataRow row in schemaTable.Rows)
            {
                string columnName = row["COLUMN_NAME"].ToString();
                if (pklist[TableName]!=null && pklist[TableName].ToString() == columnName)
                {
                    col = new ColumnInfo();

                    #region 根据列名取值
                    col.ColumnOrder = row["ORDINAL_POSITION"].ToString();
                    col.ColumnName = row["COLUMN_NAME"].ToString();
                    string type = GetColumnType(row["DATA_TYPE"].ToString());

                    col.TypeName = type;
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
                    col.Description = row["DESCRIPTION"] == null ? col.ColumnName : row["DESCRIPTION"].ToString();
                    col.IsPrimaryKey = true;
                    string flags = row["COLUMN_FLAGS"].ToString().Trim();
                    col.IsIdentity = false;
                    if (row["DATA_TYPE"].ToString().Trim() == "3" && flags == "90")
                    {
                        col.IsIdentity = true;
                    }
                    #endregion

                    collist.Add(col);

                }
            }

            #region 排序
            //collist.Sort(CompareDinosByintOrder);
            #endregion
            return collist;
        }


        private Hashtable GetPrimaryKey(string DbName, string TableName)
        {
            Hashtable pklist = new Hashtable();
            try
            {
                OpenDB();
                DataTable schemaTable = connect.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, null);
                foreach (DataRow dr in schemaTable.Rows)
                {
                    string tabname = dr["TABLE_NAME"].ToString();
                    string colname = dr["COLUMN_NAME"].ToString();
                    if (!pklist.Contains(tabname))
                    {
                        pklist.Add(tabname,colname);
                    }
                }                
            }
            catch //(System.Exception ex)
            {                
            }
            return pklist;
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
            Hashtable pklist = new Hashtable();
            try
            {
                OpenDB();
                DataTable schemaTable = connect.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, null);
                foreach (DataRow dr in schemaTable.Rows)
                {
                    string tabname = dr["TABLE_NAME"].ToString();
                    string colname = dr["COLUMN_NAME"].ToString();
                    if (!pklist.Contains(tabname))
                    {
                        pklist.Add(tabname, colname);
                    }
                }
            }
            catch //(System.Exception ex)
            {
            }
            return pklist;
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [" + TableName + "]");
            return Query("", strSql.ToString()).Tables[0];
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
            return false;
        }

        public bool DeleteTable(string DbName, string TableName)
        {
            try
            {
                string strsql = "DROP TABLE " + TableName + "";
                ExecuteSql(DbName, TableName);
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
