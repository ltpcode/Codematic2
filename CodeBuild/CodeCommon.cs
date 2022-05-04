using System;
using System.Collections.Generic;
using System.Text;

namespace LTP.CodeBuild
{
    public class CodeCommon
    {       

        /// <summary>
        /// 得到空格间隔数
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

        #region 转换 数据库字段类型 为 c#类型

        /// <summary>
        /// 转换数据库字段类型为c#类型
        /// </summary>
        /// <param name="dbtype">数据库字段类型</param>
        /// <returns>c#类型</returns>		
        public static string DbTypeToCS(string dbtype)
        {
            string CSType = "string";
            switch (dbtype.ToLower().Trim())
            {                
                case "varchar":
                case "varchar2":
                case "nvarchar":
                case "nvarchar2":
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                case "string":
                    CSType = "string";
                    break;
                case "date":
                case "datetime":
                case "smalldatetime":
                case "DateTime":
                    CSType = "DateTime";
                    break;
                case "smallint":
                case "int":
                case "number":
                case "bigint":
                case "tinyint":                
                    CSType = "int";
                    break;
                case "float":
                case "numeric":
                case "decimal":
                case "money":
                case "smallmoney":
                case "real":
                    CSType = "decimal";
                    break;
                case "bit":
                case "bool":
                    CSType = "bool";
                    break;
                case "binary":
                case "varbinary":
                case "image":
                case "raw":
                case "long":
                case "long raw":
                case "blob":
                case "bfile":
                case "byte[]":
                    CSType = "byte[]";
                    break;
                case "uniqueidentifier":
                case "Guid":
                    CSType = "Guid";
                    break;
                default:
                    CSType = "string";
                    break;
            }
            return CSType;
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
                    strtype = DbTypeLengthSQL(dbtype,datatype, Length);
                    break;
                case "Oracle":
                    strtype = DbTypeLengthOra(datatype, Length);
                    break;
                case "OleDb":
                    strtype = DbTypeLengthOleDb(datatype, Length);
                    break;
            }
            return strtype;
        }


        #region DbTypeLength

        private static string DbTypeLengthSQL(string dbtype, string datatype, string Length)
        {
            string len = "";
            switch (datatype.Trim())
            {
                case "int":
                    len = "4";
                    break;
                case "varchar":
                    {
                        if (Length.Trim() == "")
                        {
                            len = "50";
                        }
                        else
                        {
                            len = Length;
                        }
                    }
                    break;
                case "nvarchar":
                    {
                        if (Length.Trim() == "")
                        {
                            len = "50";
                        }
                        else
                        {
                            len = (int.Parse(Length)/2).ToString();
                        }
                    }
                    break;
                case "char":
                    {
                        if (Length.Trim() == "")
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
                case "image ":
                case "datetime":
                case "smalldatetime":
                case "nchar":                
                case "ntext":
                case "text":
                    break;
                default:
                    len = Length;
                    break;
            }

            if (len != "")
            {
                len = CSToProcType(dbtype, datatype) + "," + len;
            }
            else
            {
                len = CSToProcType(dbtype, datatype);
            }
            return len;
        }
        private static string DbTypeLengthOra(string datatype, string Length)
        {
            string len = "";
            switch (datatype.Trim().ToLower())
            {
                case "number":
                    len = "4";
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
        private static string DbTypeLengthOleDb(string datatype, string Length)
        {
            string len = "";
            switch (datatype.Trim())
            {
                case "int":
                    len = "4";
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
                case "image ":
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

        #endregion

        #endregion

        #region 转换 c#类型 和 数据类型 转为 存储过程的参数

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
                    strtype = CSToProcTypeSQL(cstype);
                    break;
                case "Oracle":
                    strtype = CSToProcTypeOra(cstype);
                    break;
                case "OleDb":
                    strtype = CSToProcTypeOleDb(cstype);
                    break;
            }
            return strtype;
        }

        #region CSToProcType

        private static string CSToProcTypeSQL(string cstype)
        {
            string ProcType = cstype;
            switch (cstype.Trim().ToLower())
            {
                case "varchar":
                case "string":
                    ProcType = "VarChar";
                    break;
                case "nvarchar":
                    ProcType = "NVarChar";
                    break;
                case "char":
                    ProcType = "Char";
                    break;
                case "nchar":
                    ProcType = "NChar";
                    break;
                case "text":
                    ProcType = "Text";
                    break;
                case "ntext":
                    ProcType = "NText";
                    break;
                case "datetime":
                    ProcType = "DateTime";
                    break;
                case "smalldatetime":
                    ProcType = "SmallDateTime";
                    break;
                case "smallint":
                    ProcType = "SmallInt";
                    break;
                case "tinyint":
                    ProcType = "TinyInt";
                    break;
                case "int":
                    ProcType = "Int";
                    break;
                case "bigint":
                    ProcType = "BigInt";
                    break;
                case "float":
                    ProcType = "Float";
                    break;
                case "real":
                    ProcType = "Real";
                    break;
                case "numeric":
                case "decimal":
                    ProcType = "Decimal";
                    break;
                case "money":
                    ProcType = "Money";
                    break;
                case "smallmoney":
                    ProcType = "SmallMoney";
                    break;
                case "bool":
                case "bit":
                    ProcType = "Bit";
                    break;
                case "binary":
                    ProcType = "Binary";
                    break;
                case "varbinary":
                    ProcType = "VarBinary";
                    break;
                case "image":
                    ProcType = "Image";
                    break;
                case "uniqueidentifier":
                    ProcType = "UniqueIdentifier";
                    break;
                case "timestamp":
                    ProcType = "Timestamp";
                    break;
                default:
                    ProcType = "VarChar";
                    break;
            }
            return ProcType;
        }

        private static string CSToProcTypeOra(string cstype)
        {
            string ProcType = cstype;
            switch (cstype.Trim().ToLower())
            {
                case "char":
                    ProcType = "Char";
                    break;
                case "varchar2":
                case "string":
                    ProcType = "VarChar";
                    break;
                case "nvarchar2":
                    ProcType = "NVarChar";
                    break;
                case "nchar":
                    ProcType = "NChar";
                    break;
                case "long":
                    ProcType = "LongVarChar";
                    break;
                case "number":
                case "int":
                    ProcType = "Number";
                    break;
                case "date":
                    ProcType = "DateTime";
                    break;
                case "raw":
                    ProcType = "Raw";
                    break;
                case "long raw":
                    ProcType = "LongRaw";
                    break;
                case "blob":
                    ProcType = "Blob";
                    break;
                case "clob":
                case "bit":
                    ProcType = "Clob";
                    break;
                case "nclob":
                    ProcType = "NClob";
                    break;
                case "bfile":
                    ProcType = "BFile";
                    break;
                default:
                    ProcType = "VarChar";
                    break;
            }
            return ProcType;
        }

        private static string CSToProcTypeOleDb(string cstype)
        {
            string ProcType = cstype;
            switch (cstype.Trim().ToLower())
            {
                case "varchar":
                case "string":
                    ProcType = "VarChar";
                    break;
                case "nvarchar":
                    ProcType = "LongVarChar";
                    break;
                case "char":
                    ProcType = "Char";
                    break;
                case "nchar":
                    ProcType = "NChar";
                    break;
                case "text":
                    ProcType = "LongVarChar";
                    break;
                case "ntext":
                    ProcType = "LongVarChar";
                    break;
                case "datetime":
                    ProcType = "Date";
                    break;
                case "smalldatetime":
                    ProcType = "Date";
                    break;
                case "smallint":
                    ProcType = "SmallInt";
                    break;
                case "int":
                    ProcType = "Integer";
                    break;
                case "bigint":
                    ProcType = "BigInt";
                    break;
                case "money":
                case "smallmoney":
                case "float":
                case "numeric":
                case "decimal":
                    ProcType = "Decimal";
                    break;
                case "bool":
                    ProcType = "Boolean";
                    break;
                case "bit":
                    ProcType = "Bit";
                    break;
                case "binary":
                    ProcType = "Binary";
                    break;
                default:
                    ProcType = "VarChar";
                    break;
            }
            return ProcType;
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
            switch (columnType.Trim())
            {
                case "nvarchar":
                case "nchar":
                case "ntext":
                case "varchar":
                case "varchar2":
                case "nvarchar2":
                case "char":
                case "text":
                case "date":
                case "datetime":
                case "smalldatetime":
                case "string":
                case "uniqueidentifier":
                    isadd = true;
                    break;
            }
            return isadd;
        }
        #endregion


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

       
    }
}
