using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Data;
using System.IO;
using Maticsoft.CmConfig;
using Maticsoft.Utility;
namespace Maticsoft.CodeHelper
{
    /// <summary>
    /// 代码协助通用类
    /// </summary>
    public class CodeCommon
    {
        static string datatypefile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\DatatypeMap.cfg";

        #region 缩进间隔
        /// <summary>
        /// 缩进间隔
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string Space(int num)
        {
            StringBuilder str = new StringBuilder();
            for (int n = 0; n < num; n++)
            {
                str.Append("\t");
            }
            return str.ToString();
        }
        #endregion
        
        #region 转换 数据库字段类型 为 c#类型

        /// <summary>
        /// 转换【数据库字段类型】 =》为【c#类型】
        /// </summary>
        /// <param name="dbtype">数据库字段类型</param>
        /// <returns>c#类型</returns>		
        public static string DbTypeToCS(string dbtype)
        {
            string CSType = "string";
            if (File.Exists(datatypefile))
            {                
                string val = DatatypeMap.GetValueFromCfg(datatypefile,"DbToCS", dbtype.ToLower().Trim());
                if (val == "")
                {
                    CSType = dbtype.ToLower().Trim();
                }
                else
                {
                    CSType = val;
                }               
            }
            return CSType;           

        }
        #endregion

        #region 是否c#中的值类型
        /// <summary>
        /// 是否c#中的值类型
        /// </summary>
        public static bool isValueType(string cstype)
        {
            bool isval = false;
            if (File.Exists(datatypefile))
            {                
                string val = DatatypeMap.GetValueFromCfg(datatypefile, "ValueType", cstype.Trim());
                if (val == "true" || val == "是")
                {
                    isval = true;
                }
            }
            return isval;            
        }
        #endregion

        #region （存储过程参数）得到 数据库字段类型 的 长度 (包括是否需要加)

        /// <summary>
        /// （存储过程参数）得到数据库字段类型的长度(包括是否需要加)
        /// </summary>
        /// <param name="dbtype">数据库字段类型</param>
        /// <returns></returns>
        public static string DbTypeLength(string dbtype,string datatype, string Length)
        {
            string strtype = "";
            switch (dbtype)
            {
                case "SQL2000":
                case "SQL2005":
                case "SQL2008":
                case "SQL2012":
                    strtype = DbTypeLengthSQL(dbtype,datatype, Length);
                    break;
                case "Oracle":
                    strtype = DbTypeLengthOra(datatype, Length);
                    break;
                case "MySQL":
                    strtype = DbTypeLengthMySQL(datatype, Length);
                    break;
                case "OleDb":
                    strtype = DbTypeLengthOleDb(datatype, Length);
                    break;
                case "SQLite":
                    strtype = DbTypeLengthSQLite(datatype, Length);
                    break;
            }
            return strtype;
        }
        
        #region DbTypeLength
        /// <summary>
        /// 得到某种类型字段应该的长度
        /// </summary>
        public static string GetDataTypeLenVal(string datatype, string Length)
        {
            string LenVal = "";
            switch (datatype.Trim())
            {
                case "int":
                    if (Length == "")
                    {
                        LenVal = "4";
                    }
                    else
                    {
                        LenVal = Length;
                    }
                    break;                
                case "char":                
                    {
                        if (Length.Trim() == "")
                        {
                            LenVal = "10";
                        }
                        else
                        {
                            LenVal = Length;
                        }
                    }
                    break;                
                case "nchar":
                    {
                        LenVal = Length;
                        if (Length.Trim() == "")
                        {
                            LenVal = "10";
                        }
                        //else
                        //{
                        //    LenVal = (int.Parse(Length.Trim()) / 2).ToString();
                        //}   
                    }
                    break;
                case "varchar":
                case "nvarchar":
                case "varbinary":
                    {
                        LenVal = Length;
                        if (Length.Length == 0 || Length == "0" || Length == "-1")
                        {
                            LenVal = "MAX";
                        }

                        //else
                        //{
                        //    Length = (int.Parse(Length) / 2).ToString();
                        //}
                        //if (Length.Trim() == "")
                        //{
                        //    LenVal = "50";
                        //}
                        //else
                        //{
                        //    if (int.Parse(Length.Trim()) < 1)
                        //    {
                        //        LenVal = "";
                        //    }
                        //}
                    }
                    break;
                
                case "bit":
                    LenVal = "1";
                    break;
                case "float":
                case "numeric":
                case "decimal":
                case "money":
                case "smallmoney":
                case "binary":
                case "smallint":
                case "bigint":
                    LenVal = Length;
                    break;
                case "image":
                case "datetime":
                case "smalldatetime":
                case "ntext":
                case "text":
                    break;
                default:
                    LenVal = Length;
                    break;
            }
            return LenVal;
        }
        private static string DbTypeLengthSQL(string dbtype, string datatype, string Length)
        {
            string LenVal = GetDataTypeLenVal(datatype,Length);
            string lenstr = "";
            if ( LenVal != "" )
            {
                if(LenVal == "MAX")
                {
                    LenVal = "-1";
                }
                lenstr = CSToProcType(dbtype, datatype) + "," + LenVal;
            }
            else
            {
                lenstr = CSToProcType(dbtype, datatype);
            }
            return lenstr;
        }
        private static string DbTypeLengthOra(string datatype, string Length)
        {
            string len = "";
            switch (datatype.Trim().ToLower())
            {
                case "number":
                    if (Length == "")
                    {
                        len = "4";
                    }
                    else
                    {
                        len = Length;
                    }
                    break;
                case "varchar2":
                    {
                        if (Length == "")
                        {
                            len = "50";
                        }
                        else
                        {
                            len = Length;
                        }
                    }
                    break;
                case "char":
                    {
                        if (Length == "")
                        {
                            len = "50";
                        }
                        else
                        {
                            len = Length;
                        }
                    }
                    break;
                case "date":
                case "nchar":
                case "nvarchar2":
                case "long":
                case "long raw":
                case "bfile":
                case "blob":
                    break;
                default:
                    len = Length;
                    break;
            }

            if (len != "")
            {
                len = CSToProcType("Oracle", datatype) + "," + len;
            }
            else
            {
                len = CSToProcType("Oracle", datatype);
            }
            return len;
        }
        private static string DbTypeLengthMySQL(string datatype, string Length)
        {
            string len = "";
            switch (datatype.Trim().ToLower())
            {
                case "number":
                    if (Length == "")
                    {
                        len = "4";
                    }
                    else
                    {
                        len = Length;
                    }
                    break;
                case "varchar2":
                    {
                        if (Length == "")
                        {
                            len = "50";
                        }
                        else
                        {
                            len = Length;
                        }
                    }
                    break;
                case "char":
                    {
                        if (Length == "")
                        {
                            len = "50";
                        }
                        else
                        {
                            len = Length;
                        }
                    }
                    break;
                case "date":
                case "nchar":
                case "nvarchar2":
                case "long":
                case "long raw":
                case "bfile":
                case "blob":
                    //len = Length;
                    break;
                default:
                    len = Length;
                    break;
            }

            if (len != "")
            {
                len = CSToProcType("MySQL", datatype) + "," + len;
            }
            else
            {
                len = CSToProcType("MySQL", datatype);
            }
            return len;
        }
        private static string DbTypeLengthOleDb(string datatype, string Length)
        {
            string len = "";
            switch (datatype.Trim())
            {
                case "int":
                    if (Length == "")
                    {
                        len = "4";
                    }
                    else
                    {
                        len = Length;
                    }
                    break;
                case "varchar":
                    {
                        if (Length == "")
                        {
                            len = "50";
                        }
                        else
                        {
                            len = Length;
                        }
                    }
                    break;
                case "char":
                    {
                        if (Length == "")
                        {
                            len = "50";
                        }
                        else
                        {
                            len = Length;
                        }
                    }
                    break;
                case "bit":
                    len = "1";
                    break;
                case "float":
                case "numeric":
                case "decimal":
                case "money":
                case "smallmoney":
                case "binary":
                case "smallint":
                case "bigint":
                    len = Length;
                    break;
                case "image":
                case "datetime":
                case "smalldatetime":
                case "nchar":
                case "nvarchar":
                case "ntext":
                case "text":
                    break;
                default:
                    len = Length;
                    break;
            }

            if (len != "")
            {
                len = CSToProcType("OleDb", datatype) + "," + len;
            }
            else
            {
                len = CSToProcType("OleDb", datatype);
            }
            return len;
        }
        private static string DbTypeLengthSQLite(string datatype, string Length)
        {
            string len = "";
            switch (datatype.Trim())
            {
                case "int":
                case "integer":
                    if (Length == "")
                    {
                        len = "4";
                    }
                    else
                    {
                        len = Length;
                    }
                    break;
                case "varchar":
                    {
                        if (Length == "")
                        {
                            len = "50";
                        }
                        else
                        {
                            len = Length;
                        }
                    }
                    break;
                case "char":
                    {
                        if (Length == "")
                        {
                            len = "50";
                        }
                        else
                        {
                            len = Length;
                        }
                    }
                    break;
                case "bit":
                    len = "1";
                    break;
                case "float":
                case "numeric":
                case "decimal":
                case "money":
                case "smallmoney":
                case "binary":
                case "smallint":
                case "bigint":
                case "blob":
                    len = Length;
                    break;
                case "image":
                case "datetime":
                case "smalldatetime":
                case "nchar":
                case "nvarchar":
                case "ntext":
                case "text":
                case "time":
                case "date":
                case "boolean":
                    break;
                default:
                    len = Length;
                    break;
            }

            if (len != "")
            {
                len = CSToProcType("SQLite", datatype) + "," + len;
            }
            else
            {
                len = CSToProcType("SQLite", datatype);
            }
            return len;
        }
        #endregion


        #endregion

        #region 转换 【c#类型 和 数据类型】 转为 【存储过程的参数】

        /// <summary>
        /// 转换c#类型和数据类型转为存储过程的参数类型
        /// </summary>
        /// <param name="dbtype">数据库字段类型</param>
        /// <returns>c#类型</returns>
        public static string CSToProcType(string DbType, string cstype)
        {
            string strtype = cstype;
            switch (DbType)
            {
                case "SQL2000":
                case "SQL2005":
                case "SQL2008":
                case "SQL2012":
                    strtype = CSToProcTypeSQL(cstype);
                    break;
                case "Oracle":
                    strtype = CSToProcTypeOra(cstype);
                    break;
                case "MySQL":
                    strtype = CSToProcTypeMySQL(cstype);
                    break;
                case "OleDb":
                    strtype = CSToProcTypeOleDb(cstype);
                    break;
                case "SQLite":
                    strtype = CSToProcTypeSQLite(cstype);
                    break;
            }
            return strtype;
        }

        #region CSToProcType

        private static string CSToProcTypeSQL(string cstype)
        {
            string CSType = cstype;
            if (File.Exists(datatypefile))
            {
                //datatype = new Maticsoft.Utility.INIFile(datatypefile);
                //string val = datatype.IniReadValue("ToSQLProc", cstype.ToLower().Trim());
                string val = DatatypeMap.GetValueFromCfg(datatypefile, "SQLDbType", cstype.ToLower().Trim());
                if (val == "")
                {
                    CSType = cstype.ToLower().Trim();
                }
                else
                {
                    CSType = val;
                }
            }
            return CSType;           
        }

        private static string CSToProcTypeOra(string cstype)
        {
            string CSType = cstype;
            if (File.Exists(datatypefile))
            {
                //datatype = new Maticsoft.Utility.INIFile(datatypefile);
                //string val = datatype.IniReadValue("ToOraProc", cstype.ToLower().Trim());
                string val = DatatypeMap.GetValueFromCfg(datatypefile, "OraDbType", cstype.ToLower().Trim());
                if (val == "")
                {
                    CSType = cstype.ToLower().Trim();
                }
                else
                {
                    CSType = val;
                }
            }
            return CSType;  
        }

        private static string CSToProcTypeMySQL(string cstype)
        {
            string CSType = cstype;
            if (File.Exists(datatypefile))
            {
                //datatype = new Maticsoft.Utility.INIFile(datatypefile);
                //string val = datatype.IniReadValue("ToMySQLProc", cstype.ToLower().Trim());
                string val = DatatypeMap.GetValueFromCfg(datatypefile, "MySQLDbType", cstype.ToLower().Trim());
                if (val == "")
                {
                    CSType = cstype.ToLower().Trim();
                }
                else
                {
                    CSType = val;
                }
            }
            return CSType;
        }

        private static string CSToProcTypeOleDb(string cstype)
        {
            string CSType = cstype;
            if (File.Exists(datatypefile))
            {
                //datatype = new Maticsoft.Utility.INIFile(datatypefile);
                //string val = datatype.IniReadValue("ToOleDbProc", cstype.ToLower().Trim());
                string val = DatatypeMap.GetValueFromCfg(datatypefile, "OleDbDbType", cstype.ToLower().Trim());
                if (val == "")
                {
                    CSType = cstype.ToLower().Trim();
                }
                else
                {
                    CSType = val;
                }
            }
            return CSType;  
        }

        private static string CSToProcTypeSQLite(string cstype)
        {
            string CSType = cstype;
            if (File.Exists(datatypefile))
            {
                //datatype = new Maticsoft.Utility.INIFile(datatypefile);
                //string val = datatype.IniReadValue("ToSQLiteProc", cstype.ToLower().Trim());
                string val = DatatypeMap.GetValueFromCfg(datatypefile, "SQLiteType", cstype.ToLower().Trim());
                if (val == "")
                {
                    CSType = cstype.ToLower().Trim();
                }
                else
                {
                    CSType = val;
                }
            }
            return CSType;
        }

        #endregion

        #endregion


        #region 该数据类型是否加单引号

        /// <summary>
        /// 该数据类型是否加单引号
        /// </summary>
        /// <param name="columnType">数据库类型</param>
        /// <returns></returns>
        public static bool IsAddMark(string columnType)
        {
            bool isadd = false;            
            if (File.Exists(datatypefile))
            {                
                string val = DatatypeMap.GetValueFromCfg(datatypefile, "AddMark", columnType.ToLower().Trim());
                if (val == "true" || val == "是")
                {
                    isadd = true;
                }                
            }
            return isadd;           
        }
        #endregion


        #region byte型数据转16进制 

        static char[] hexDigits = {'0', '1', '2', '3', '4', '5', '6', '7',
									  '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
        public static string ToHexString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            string str = new string(chars);
            return "0x" + str.Substring(0, bytes.Length);
        }

        #endregion

        #region  得到表的字段List对象信息
        /// <summary>
        /// 将【字段信息表数据DataTable】转为 【所有字段对象数组List-ColumnInfo】
        /// </summary>
        public static List<ColumnInfo> GetColumnInfos(DataTable dt)
        {
            List<ColumnInfo> keys = new List<ColumnInfo>();
            if (dt != null)
            {
                ArrayList colexist = new ArrayList();//是否已经存在
                ColumnInfo key;
                foreach (DataRow row in dt.Rows)
                {
                    string Colorder = row["Colorder"].ToString();
                    string ColumnName = row["ColumnName"].ToString();
                    string TypeName = row["TypeName"].ToString();
                    string isIdentity = row["IsIdentity"].ToString();
                    string IsPK = row["IsPK"].ToString();
                    string Length = row["Length"].ToString();
                    string Preci = row["Preci"].ToString();
                    string Scale = row["Scale"].ToString();
                    string cisNull = row["cisNull"].ToString();
                    string DefaultVal = row["DefaultVal"].ToString();
                    string DeText = row["DeText"].ToString();

                    key = new ColumnInfo();
                    key.ColumnOrder = Colorder;
                    key.ColumnName = ColumnName;
                    key.TypeName = TypeName;                                        
                    key.IsIdentity = (isIdentity == "√") ? true : false;
                    key.IsPrimaryKey = (IsPK == "√") ? true : false;
                    key.Length = Length;
                    key.Precision = Preci;
                    key.Scale = Scale;
                    key.Nullable = (cisNull == "√") ? true : false; 
                    key.DefaultVal = DefaultVal;
                    key.Description = DeText;
                    
                    if (!colexist.Contains(ColumnName))
                    {
                        keys.Add(key);
                        colexist.Add(ColumnName);
                    }
                }
                return keys;
            }
            else
            {
                return null;
            }
 
        }

        /// <summary>
        /// 将【所有字段对象数组List-ColumnInfo】转为 【字段信息表数据DataTable】
        /// </summary>        
        public static DataTable GetColumnInfoDt(List<ColumnInfo> collist)
        {
            DataTable cTable = new DataTable();
            cTable.Columns.Add("colorder");
            cTable.Columns.Add("ColumnName");
            cTable.Columns.Add("TypeName");
            cTable.Columns.Add("Length");
            cTable.Columns.Add("Preci");
            cTable.Columns.Add("Scale");
            cTable.Columns.Add("IsIdentity");
            cTable.Columns.Add("isPK");
            cTable.Columns.Add("cisNull");
            cTable.Columns.Add("defaultVal");
            cTable.Columns.Add("deText");

            foreach(ColumnInfo col in collist)
            {
                DataRow newRow = cTable.NewRow();
                newRow["colorder"] = col.ColumnOrder;
                newRow["ColumnName"] = col.ColumnName;                
                newRow["TypeName"] = col.TypeName;
                newRow["Length"] = col.Length;
                newRow["Preci"] = col.Precision;
                newRow["Scale"] = col.Scale;
                newRow["IsIdentity"] = (col.IsIdentity) ? "√" : "";
                newRow["isPK"] = (col.IsPrimaryKey) ? "√" : "";
                newRow["cisNull"] = (col.Nullable) ? "√" : "";
                newRow["defaultVal"] = col.DefaultVal;
                newRow["deText"] = col.Description;
                cTable.Rows.Add(newRow);
            }
            return cTable;	
        }
        #endregion

        #region 得到标识列字段

        /// <summary>
        /// 得到标识列字段
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static ColumnInfo GetIdentityKey(List<ColumnInfo> keys)
        {           
            foreach (ColumnInfo field in keys)
            {                
                if (field.IsIdentity)
                {
                    return field;
                }
            }
            return null;        
        }

        /// <summary>
        /// 是否有主键字段，且主键字段并不是标识字段。
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static bool HasNoIdentityKey(List<ColumnInfo> keys)
        {
            foreach (ColumnInfo field in keys)
            {
                if ((field.IsPrimaryKey)&&(!field.IsIdentity))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region  根据列信息 得到参数的列表(基于Parameter方式)

        /// <summary>
        /// 不同数据库类的前缀
        /// </summary>
        public static string DbParaHead(string DbType)
        {
            switch (DbType)
            {
                case "SQL2000":
                case "SQL2005":
                case "SQL2008":
                case "SQL2012":
                    return "Sql";
                case "Oracle":
                    return "Oracle";
                case "MySQL":
                    return "MySql";
                case "OleDb":
                    return "OleDb";
                case "SQLite":
                    return "SQLite";
                default:
                    return "Sql";
            }

        }
        /// <summary>
        ///  不同数据库字段类型
        /// </summary>
        public static string DbParaDbType(string DbType)
        {
            switch (DbType)
            {
                case "SQL2000":
                case "SQL2005":
                case "SQL2008":
                case "SQL2012":
                    return "SqlDbType";
                case "Oracle":
                    return "OracleType";
                case "OleDb":
                    return "OleDbType";
                case "MySQL":
                    return "MySqlDbType";
                case "SQLite":
                    return "DbType";
                default:
                    return "SqlDbType";
            }
        }

        /// <summary>
        /// 存储过程参数 调用符号@
        /// </summary>
        public static string preParameter(string DbType)
        {           
            string Prefix = "@";
            if (File.Exists(datatypefile))
            {
                string val = DatatypeMap.GetValueFromCfg(datatypefile, "ParamePrefix", DbType.ToUpper().Trim());
                if (val == "")
                {
                    Prefix = DbType.ToUpper().Trim();
                }
                else
                {
                    Prefix = val;
                }
            }
            return Prefix;
        }
                
        /// <summary>
        /// 主键或条件字段中是否有标识列
        /// </summary>
        public static bool IsHasIdentity(List<ColumnInfo> Keys)
        {
            bool isid = false;
            if (Keys.Count > 0)
            {
                foreach (ColumnInfo key in Keys)
                {
                    if (key.IsIdentity)
                    {
                        isid = true;
                    }
                }
            }
            return isid;
        }
        
        /// <summary>
        /// 得到Where条件语句 - Parameter方式 (例如：用于Exists  Delete  GetModel 的where)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string GetWhereParameterExpression(List<ColumnInfo> keys, bool IdentityisPrior, string DbType)
        {
            StringPlus strClass = new StringPlus();
            ColumnInfo field = GetIdentityKey(keys);
            bool hasPK = HasNoIdentityKey(keys);

            if ((IdentityisPrior && field != null) || (!hasPK && field != null)) //有标识字段
            {
                strClass.Append(field.ColumnName + "=" + preParameter(DbType) + field.ColumnName);
            }
            else
            {
                foreach (ColumnInfo key in keys)
                {
                    if (key.IsPrimaryKey || !key.IsIdentity)// if (key.IsPK)
                    {
                        strClass.Append(key.ColumnName + "=" + preParameter(DbType) + key.ColumnName + " and ");
                    }
                }
                strClass.DelLastChar("and");
            }
            return strClass.Value;
        }

        /// <summary>
        /// 生成sql语句中的参数列表(例如：用于 Exists  Delete  GetModel 的where参数赋值)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static string GetPreParameter(List<ColumnInfo> keys, bool IdentityisPrior, string DbType)
        {
            StringPlus strclass = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendSpaceLine(3, "" + DbParaHead(DbType) + "Parameter[] parameters = {");

            ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(keys);
            bool hasPK=HasNoIdentityKey(keys);

            if ((IdentityisPrior && field != null) || (!hasPK && field != null)) //有标识字段
            {
                strclass.AppendSpaceLine(5, "new " + DbParaHead(DbType) + "Parameter(\"" + preParameter(DbType) + "" + field.ColumnName + "\", " + DbParaDbType(DbType) + "." + CodeCommon.DbTypeLength(DbType, field.TypeName, "") + ")");
                strclass2.AppendSpaceLine(3, "parameters[0].Value = " + field.ColumnName + ";");
            }
            else
            {
                int n = 0;
                foreach (ColumnInfo key in keys)
                {
                    if (key.IsPrimaryKey || !key.IsIdentity) //if (key.IsPK)
                    {
                        strclass.AppendSpaceLine(5, "new " + DbParaHead(DbType) + "Parameter(\"" + preParameter(DbType) + "" + key.ColumnName + "\", " + DbParaDbType(DbType) + "." + CodeCommon.DbTypeLength(DbType, key.TypeName, key.Length) + "),");
                        strclass2.AppendSpaceLine(3, "parameters[" + n.ToString() + "].Value = " + key.ColumnName + ";");
                        n++;
                    }
                }
                strclass.DelLastComma();
            }
            strclass.AppendSpaceLine(3,"};");
            strclass.Append(strclass2.Value);
            return strclass.Value;

        }

        #endregion

        #region  根据列信息 得到参数的列表(基于SQL方式)

        /// <summary>
        /// 得到方法输入参数定义的列表 (例如：用于Exists  Delete  GetModel 的参数传入)
        /// </summary>
        /// <param name="keys">主键和标识列集</param>
        /// <param name="IdentityisPrior">是否优先使用标识列,true 优先标识列，false 优先主键，null 包括全部</param>
        /// <returns></returns>
        public static string GetInParameter(List<ColumnInfo> keys, bool IdentityisPrior)
        {
            StringPlus strClass = new StringPlus();           
            ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(keys);           
            if ((IdentityisPrior) &&(field != null)) //有标识字段
            {
                strClass.Append(CodeCommon.DbTypeToCS(field.TypeName) + " " + field.ColumnName);
            }
            else
            {
                foreach (ColumnInfo key in keys)
                {
                    if (key.IsPrimaryKey || !key.IsIdentity) //if (key.IsPK)
                    {
                        strClass.Append(CodeCommon.DbTypeToCS(key.TypeName) + " " + key.ColumnName + ",");
                    }
                }
                strClass.DelLastComma();
            }
            return strClass.Value;
        }
        /// <summary>
        /// 字段的 select 列表，和方法传递的参数值
        /// </summary>
        public static string GetFieldstrlist(List<ColumnInfo> keys, bool IdentityisPrior)
        {           
            StringPlus strFields = new StringPlus();            
            ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(keys);
            if ((IdentityisPrior) && (field != null)) //有标识字段
            {
                strFields.Append(field.ColumnName);
            }
            else
            {
                foreach (ColumnInfo key in keys)
                {
                    if (key.IsPrimaryKey || !key.IsIdentity) //if (key.IsPK)
                    {
                        strFields.Append(key.ColumnName + ",");
                    }
                }
                strFields.DelLastComma();
            }            
            
            return strFields.Value;            
        }
        /// <summary>
        /// 字段的 select 列表
        /// </summary>
        public static string GetFieldstrlistAdd(List<ColumnInfo> keys, bool IdentityisPrior)
        {
            StringPlus fields = new StringPlus();
            ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(keys);
            if ((IdentityisPrior) && (field != null)) //有标识字段
            {
                fields.Append(field.ColumnName);
            }
            else
            {
                foreach (ColumnInfo key in keys)
                {
                    if (key.IsPrimaryKey || !key.IsIdentity) //if (key.IsPK)
                    {
                        fields.Append(key.ColumnName + "+");
                    }
                }
                fields.DelLastChar("+");
            }           
            return fields.Value;

        }
        /// <summary>
        /// 得到Where条件语句 - SQL方式 (例如：用于Exists  Delete  GetModel 的where)
        /// </summary>
        public static string GetWhereExpression(List<ColumnInfo> keys, bool IdentityisPrior)
        {
            StringPlus strclass = new StringPlus();
            ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(keys);
            if ((IdentityisPrior) && (field != null)) //有标识字段
            {
                if (CodeCommon.IsAddMark(field.TypeName))
                {
                    strclass.Append(field.ColumnName + "='\"+" + field.ColumnName + "+\"'");
                }
                else
                {
                    strclass.Append(field.ColumnName + "=\"+" + field.ColumnName + "+\"");
                }
            }
            else
            {
                foreach (ColumnInfo key in keys)
                {
                    if (key.IsPrimaryKey||!key.IsIdentity) //if (key.IsPK)
                    {
                        if (CodeCommon.IsAddMark(key.TypeName))
                        {
                            strclass.Append(key.ColumnName + "='\"+" + key.ColumnName + "+\"' and ");
                        }
                        else
                        {
                            strclass.Append(key.ColumnName + "=\"+" + key.ColumnName + "+\" and ");
                        }
                    }
                }
                strclass.DelLastChar("and");
            }            
            return strclass.Value;
        }

        /// <summary>
        /// 得到Where条件语句 - SQL方式 (例如：用于Exists  Delete  GetModel 的where)
        /// </summary>
        public static string GetModelWhereExpression(List<ColumnInfo> keys, bool IdentityisPrior)
        {
            StringPlus strclass = new StringPlus();
            ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(keys);
            if ((IdentityisPrior) && (field != null)) //有标识字段
            {
                if (CodeCommon.IsAddMark(field.TypeName))
                {
                    strclass.Append(field.ColumnName + "='\"+ model." + field.ColumnName + "+\"'");
                }
                else
                {
                    strclass.Append(field.ColumnName + "=\"+ model." + field.ColumnName + "+\"");
                }
            }
            else
            {
                foreach (ColumnInfo key in keys)
                {
                    if (key.IsPrimaryKey || !key.IsIdentity) //if (key.IsPK)
                    {
                        if (CodeCommon.IsAddMark(key.TypeName))
                        {
                            strclass.Append(key.ColumnName + "='\"+ model." + key.ColumnName + "+\"' and ");
                        }
                        else
                        {
                            strclass.Append(key.ColumnName + "=\"+ model." + key.ColumnName + "+\" and ");
                        }
                    }
                }
                strclass.DelLastChar("and");
            }            
            return strclass.Value;
        }           

        #endregion


        #region  截取表的描述和字段描述信息
        /// <summary>
        /// 截取表的描述和字段描述信息
        /// </summary>        
        /// <param name="descText">要截取处理的文字信息</param>
        /// <param name="cutLen">如果无分隔符时，固定截取长度</param>
        /// <param name="ReplaceText">如果descText为空时的替代文字</param>
        /// <returns>返回截取处理后的文字</returns>
        public static string CutDescText( string descText,int cutLen,string ReplaceText)
        {
            string newDeText = "";
            if (descText.Trim().Length > 0)
            {
                int n = 0;
                int n1 = descText.IndexOf(";");
                int n2 = descText.IndexOf("，");
                int n3 = descText.IndexOf(",");

                n = Math.Min(n1, n2);
                if (n < 0)
                {
                    n = Math.Max(n1, n2);
                }
                n = Math.Min(n, n3);
                if (n < 0)
                {
                    n = Math.Max(n1, n2);
                }

                if (n > 0)
                {
                    newDeText = descText.Trim().Substring(0, n);
                }
                else
                {
                    if (descText.Trim().Length > cutLen)
                    {
                        newDeText = descText.Trim().Substring(0, cutLen);
                    }
                    else
                    {
                        newDeText = descText.Trim();
                    }
                }
            }
            else
            {
                newDeText = ReplaceText;
            }
            return newDeText;
        }
        #endregion


        #region 字段排序
        /// <summary>
        /// 根据字符串转int排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>0代表相等，-1代表y大于x，1代表x大于y</returns>
        public static int CompareByintOrder(ColumnInfo x, ColumnInfo y)
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
        public static int CompareByOrder(ColumnInfo x, ColumnInfo y)
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

    }
}
