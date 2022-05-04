using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.BuilderDALSQL
{
    /// <summary>
    /// 数据访问层代码构造器（SQL方式）
    /// </summary>
    public class BuilderDAL : Maticsoft.IBuilder.IBuilderDAL
    {
        #region 私有变量

        /// <summary>
        /// 标识列，或主键字段	
        /// </summary>
        protected string _IdentityKey = "";
        /// <summary>
        /// 标识列，或主键字段类型 
        /// </summary>
        protected string _IdentityKeyType = "int";

        #endregion

        #region 公有属性
        IDbObject dbobj;
        private string _dbname;
        private string _tablename;
        private string _modelname; //model类名
        private string _dalname;//dal类名    
        private List<ColumnInfo> _fieldlist;
        private List<ColumnInfo> _keys; //主键或条件字段列表    

        private string _namespace; //顶级命名空间名
        private string _folder; //所在文件夹
        private string _dbhelperName;//数据库访问类名
        private string _modelpath;
        private string _dalpath;
        private string _idalpath;
        private string _iclass;
        private string _procprefix;

        public IDbObject DbObject
        {
            set { dbobj = value; }
            get { return dbobj; }
        }
        public string DbName
        {
            set { _dbname = value; }
            get { return _dbname; }
        }
        public string TableName
        {
            set { _tablename = value; }
            get { return _tablename; }
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
            set
            {
                _keys = value;
                foreach (ColumnInfo key in _keys)
                {
                    _IdentityKey = key.ColumnName;
                    _IdentityKeyType = key.TypeName;
                    if (key.IsIdentity)
                    {
                        _IdentityKey = key.ColumnName;
                        _IdentityKeyType = CodeCommon.DbTypeToCS(key.TypeName);
                        break;
                    }
                }
            }
            get { return _keys; }
        }
        public string NameSpace
        {
            set { _namespace = value; }
            get { return _namespace; }
        }
        public string Folder
        {
            set { _folder = value; }
            get { return _folder; }
        }

        /*============================*/
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// 实体类的命名空间
        /// </summary>
        public string Modelpath
        {
            set { _modelpath = value; }
            get { return _modelpath; }
        }
        /// <summary>
        /// 实体类的整个命名空间 + 类名
        /// </summary>
        /// <summary>
        /// 实体类的整个命名空间 + 类名，即等于 Modelpath+ModelName
        /// </summary>
        public string ModelSpace
        {
            get { return Modelpath + "." + ModelName; }
        }
        /*============================*/

        /// <summary>
        /// 数据层的命名空间
        /// </summary>
        public string DALpath
        {
            set { _dalpath = value; }
            get
            {
                return _dalpath;
            }
        }
        public string DALName
        {
            set { _dalname = value; }
            get { return _dalname; }
        }
        /*============================*/

        /// <summary>
        /// 接口的命名空间
        /// </summary>
        public string IDALpath
        {
            set { _idalpath = value; }
            get
            {
                return _idalpath;
            }
        }
        /// <summary>
        /// 接口类名
        /// </summary>
        public string IClass
        {
            set { _iclass = value; }
            get
            {
                return _iclass;
            }
        }

        public string DbHelperName
        {
            set { _dbhelperName = value; }
            get { return _dbhelperName; }
        }
        /// <summary>
        /// 存储过程前缀 
        /// </summary>       
        public string ProcPrefix
        {
            set { _procprefix = value; }
            get { return _procprefix; }
        }
        #endregion

        #region 构造属性

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
        /// 不同数据库类的前缀
        /// </summary>
        public string DbParaHead
        {
            get
            {
                switch (dbobj.DbType)
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

        }
        /// <summary>
        ///  不同数据库字段类型
        /// </summary>
        public string DbParaDbType
        {
            get
            {
                switch (dbobj.DbType)
                {
                    case "SQL2000":
                    case "SQL2005":
                    case "SQL2008":
                    case "SQL2012":
                        return "SqlDbType";
                    case "Oracle":
                        return "OracleType";
                    case "MySQL":
                        return "MySqlDbType";
                    case "OleDb":
                        return "OleDbType";
                    case "SQLite":
                        return "DbType";
                    default:
                        return "SqlDbType";
                }
            }
        }

        /// <summary>
        /// 存储过程参数 调用符号@
        /// </summary>
        public string preParameter
        {
            get
            {
                switch (dbobj.DbType)
                {
                    case "SQL2000":
                    case "SQL2005":
                    case "SQL2008":
                    case "SQL2012":
                        return "@";
                    case "Oracle":
                        return ":";
                    //case "OleDb":
                    // break;
                    default:
                        return "@";

                }
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
        private string KeysNullTip
        {
            get
            {
                if (_keys.Count == 0)
                {
                    return "//该表无主键信息，请自定义主键/条件字段";
                }
                else
                {
                    return "";
                }
            }
        }

        //语言文件
        public Hashtable Languagelist
        {
            get
            {
                return Maticsoft.CodeHelper.Language.LoadFromCfg("BuilderDALSQL.lan");
            }
        }
        #endregion

        #region 构造函数

        public BuilderDAL()
        {
        }
        public BuilderDAL(IDbObject idbobj)
        {
            dbobj = idbobj;
        }
        public BuilderDAL(IDbObject idbobj, string dbname, string tablename, string modelname, string dalName,
            List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string namepace,
            string folder, string dbherlpername, string modelpath, string modelspace,
            string dalpath, string idalpath, string iclass)
        {
            dbobj = idbobj;
            _dbname = dbname;
            _tablename = tablename;
            _modelname = modelname;
            _namespace = namepace;
            _folder = folder;
            _dbhelperName = dbherlpername;
            _modelpath = modelpath;
            _dalpath = dalpath;
            _idalpath = idalpath;
            _iclass = iclass;
            Fieldlist = fieldlist;
            Keys = keys;
            foreach (ColumnInfo key in _keys)
            {
                _IdentityKey = key.ColumnName;
                _IdentityKeyType = key.TypeName;
                if (key.IsIdentity)
                {
                    _IdentityKey = key.ColumnName;
                    _IdentityKeyType = CodeCommon.DbTypeToCS(key.TypeName);
                    break;
                }
            }
        }

        #endregion


        #region  根据列信息 得到参数的列表

        /// <summary>
        /// 得到Where条件语句 - Parameter方式 (例如：用于Exists  Delete  GetModel 的where)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string GetWhereExpression(List<ColumnInfo> keys, bool IdentityisPrior)
        {
            StringPlus strClass = new StringPlus();
            ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(keys);
            if ((IdentityisPrior) && (field != null)) //有标识字段
            {
                strClass.Append(field.ColumnName + "=" + preParameter + field.ColumnName);
            }
            else
            {
                foreach (ColumnInfo key in keys)
                {
                    if (key.IsPrimaryKey)
                    {
                        strClass.Append(key.ColumnName + "=" + preParameter + key.ColumnName + " and ");
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
        public string GetPreParameter(List<ColumnInfo> keys, bool IdentityisPrior)
        {
            StringPlus strclass = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");

            ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(keys);
            if ((IdentityisPrior) && (field != null)) //有标识字段
            {
                strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "" + field.ColumnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, field.TypeName, "") + ")");
                strclass2.AppendSpaceLine(3, "parameters[0].Value = " + field.ColumnName + ";");
            }
            else
            {
                int n = 0;
                foreach (ColumnInfo key in keys)
                {
                    if (key.IsPrimaryKey)
                    {
                        strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "" + key.ColumnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, key.TypeName, "") + "),");
                        strclass2.AppendSpaceLine(3, "parameters[" + n.ToString() + "].Value = " + key.ColumnName + ";");
                        n++;
                    }
                }
                strclass.DelLastComma();
            }
            strclass.AppendLine("};");
            strclass.Append(strclass2.Value);
            return strclass.Value;

        }

        #endregion

        #region 数据层(整个类)

        public string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Text;");
            switch (dbobj.DbType)
            {
                case "SQL2005":
                case "SQL2008":
                case "SQL2012":
                    strclass.AppendLine("using System.Data.SqlClient;");
                    break;
                case "SQL2000":
                    strclass.AppendLine("using System.Data.SqlClient;");
                    break;
                case "Oracle":
                    strclass.AppendLine("using System.Data.OracleClient;");
                    break;
                case "MySQL":
                    strclass.AppendLine("using MySql.Data.MySqlClient;");
                    break;
                case "OleDb":
                    strclass.AppendLine("using System.Data.OleDb;");
                    break;
                case "SQLite":
                    strclass.AppendLine("using System.Data.SQLite;");
                    break;
            }
            if (IDALpath != "")
            {
                strclass.AppendLine("using " + IDALpath + ";");
            }
            strclass.AppendLine("using Maticsoft.DBUtility;//Please add references");
            strclass.AppendLine("namespace " + DALpath);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// " + Languagelist["summary"].ToString() + ":" + DALName);
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpace(1, "public partial class " + DALName);
            if (IClass != "")
            {
                strclass.Append(":" + IClass);
            }
            strclass.AppendLine("");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, "public " + DALName + "()");
            strclass.AppendSpaceLine(2, "{}");
            strclass.AppendSpaceLine(2, "#region  Method");


            #region  方法代码
            if (Maxid)
            {
                strclass.AppendLine(CreatGetMaxID());
            }
            if (Exists)
            {
                strclass.AppendLine(CreatExists());
            }
            if (Add)
            {
                strclass.AppendLine(CreatAdd());
            }
            if (Update)
            {
                strclass.AppendLine(CreatUpdate());
            }
            if (Delete)
            {
                strclass.AppendLine(CreatDelete());
            }
            if (GetModel)
            {
                strclass.AppendLine(CreatGetModel());
                strclass.AppendLine(CreatDataRowToModel());
            }
            if (List)
            {
                strclass.AppendLine(CreatGetList());
                strclass.AppendLine(CreatGetListByPage());
                strclass.AppendLine(CreatGetListByPageProc());
            }
            #endregion

            strclass.AppendSpaceLine(2, "#endregion  Method");

            strclass.AppendSpaceLine(2, "#region  MethodEx");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "#endregion  MethodEx");


            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            strclass.AppendLine("");

            return strclass.ToString();
        }

        #endregion


        #region 数据层(方法)

        /// <summary>
        /// 得到某字段最大值的方法代码(只有主键是int型的情况下生成)
        /// </summary>
        /// <param name="TabName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CreatGetMaxID()
        {
            StringPlus strclass = new StringPlus();
            if (_keys.Count > 0)
            {
                string keyname = "";
                foreach (ColumnInfo obj in _keys)
                {
                    if (CodeCommon.DbTypeToCS(obj.TypeName) == "int")
                    {
                        keyname = obj.ColumnName;
                        if (obj.IsPrimaryKey)
                        {
                            strclass.AppendLine("");
                            strclass.AppendSpaceLine(2, "/// <summary>");
                            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetMaxId"].ToString());
                            strclass.AppendSpaceLine(2, "/// </summary>");
                            strclass.AppendSpaceLine(2, "public int GetMaxId()");
                            strclass.AppendSpaceLine(2, "{");
                            strclass.AppendSpaceLine(2, "return " + DbHelperName + ".GetMaxID(\"" + keyname + "\", \"" + _tablename + "\"); ");
                            strclass.AppendSpaceLine(2, "}");
                            break;
                        }
                    }
                }
            }
            return strclass.ToString();
        }

        /// <summary>
        /// 得到Exists方法的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CreatExists()
        {
            StringPlus strclass = new StringPlus();
            if (_keys.Count > 0)
            {
                string strInparam = Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false);
                if (!string.IsNullOrEmpty(strInparam))
                {
                    strclass.AppendLine("");
                    strclass.AppendSpaceLine(2, "/// <summary>");
                    strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryExists"].ToString());
                    strclass.AppendSpaceLine(2, "/// </summary>");
                    strclass.AppendSpaceLine(2, "public bool Exists(" + strInparam + ")");
                    strclass.AppendSpaceLine(2, "{");
                    strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                    strclass.AppendSpaceLine(3, "strSql.Append(\"select count(1) from " + _tablename + "\");");
                    strclass.AppendSpaceLine(3, "strSql.Append(\" where " + Maticsoft.CodeHelper.CodeCommon.GetWhereExpression(Keys, false) + "\");");
                    strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Exists(strSql.ToString());");
                    strclass.AppendSpace(2, "}");
                }
            }
            return strclass.ToString();
        }
        /// <summary>
        /// 得到Add()的代码
        /// </summary>        
        public string CreatAdd()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryadd"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            string strretu = "bool";
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005" ||
                dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012" || dbobj.DbType == "SQLite") && (IsHasIdentity))
            {
                strretu = "int";
                if (_IdentityKeyType != "int")
                {
                    strretu = _IdentityKeyType;
                }
            }
            //if (dbobj.DbType == "OleDb" && IsHasIdentity)
            //{
            //    strretu = "bool";
            //}

            //方法定义头
            string strFun = CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpace + " model)";
            strclass.AppendLine(strFun);
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "StringBuilder strSql1=new StringBuilder();");
            strclass.AppendSpaceLine(3, "StringBuilder strSql2=new StringBuilder();");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;

                if (IsIdentity)
                {
                    continue;
                }
                strclass.AppendSpaceLine(3, "if (model." + columnName + " != null)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql1.Append(\"" + columnName + ",\");");
                if ((dbobj.DbType == "Oracle") && (columnType.ToLower() == "date" || columnType.ToLower() == "datetime"))
                {
                    strclass.AppendSpaceLine(4, "strSql2.Append(\"to_date('\" + model." + columnName + ".ToString() + \"','YYYY-MM-DD HH24:MI:SS'),\");");
                }
                else
                    if (columnType.ToLower() == "bit")
                    {
                        strclass.AppendSpaceLine(4, "strSql2.Append(\"\"+(model." + columnName + "? 1 : 0) +\",\");");
                    }
                    else
                        if ("uniqueidentifier" == columnType.ToLower())
                        {
                            strclass.AppendSpaceLine(4, "strSql2.Append(\"'\"+ Guid.NewGuid().ToString() +\"',\");");
                        }
                        else
                            if (CodeCommon.IsAddMark(columnType.Trim()))
                            {
                                strclass.AppendSpaceLine(4, "strSql2.Append(\"'\"+model." + columnName + "+\"',\");");
                            }
                            else
                            {
                                strclass.AppendSpaceLine(4, "strSql2.Append(\"\"+model." + columnName + "+\",\");");
                            }

                strclass.AppendSpaceLine(3, "}");
            }
            strclass.AppendSpaceLine(3, "strSql.Append(\"insert into " + TableName + "(\");");
            strclass.AppendSpaceLine(3, "strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));");
            strclass.AppendSpaceLine(3, "strSql.Append(\")\");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" values (\");");
            strclass.AppendSpaceLine(3, "strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));");
            strclass.AppendSpaceLine(3, "strSql.Append(\")\");");

            if (strretu == "void")
            {
                strclass.AppendSpaceLine(3, "" + DbHelperName + ".ExecuteSql(strSql.ToString());");
            }
            else
                if (strretu == "bool")
                {
                    strclass.AppendSpaceLine(3, "int rows=" + DbHelperName + ".ExecuteSql(strSql.ToString());");
                    strclass.AppendSpaceLine(3, "if (rows > 0)");
                    strclass.AppendSpaceLine(3, "{");
                    strclass.AppendSpaceLine(4, "return true;");
                    strclass.AppendSpaceLine(3, "}");
                    strclass.AppendSpaceLine(3, "else");
                    strclass.AppendSpaceLine(3, "{");
                    strclass.AppendSpaceLine(4, "return false;");
                    strclass.AppendSpaceLine(3, "}");
                }
                else//返回自动增长列值
                {
                    if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005"
                        || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012") && (IsHasIdentity))
                    {
                        strclass.AppendSpaceLine(3, "strSql.Append(\";select @@IDENTITY\");");
                    }
                    if ((dbobj.DbType == "SQLite") && (IsHasIdentity))
                    {
                        strclass.AppendSpaceLine(3, "strSql.Append(\";select LAST_INSERT_ROWID()\");");
                    }
                    strclass.AppendSpaceLine(3, "object obj = " + DbHelperName + ".GetSingle(strSql.ToString());");
                    strclass.AppendSpaceLine(3, "if (obj == null)");
                    strclass.AppendSpaceLine(3, "{");
                    strclass.AppendSpaceLine(4, "return 0;");
                    strclass.AppendSpaceLine(3, "}");
                    strclass.AppendSpaceLine(3, "else");
                    strclass.AppendSpaceLine(3, "{");
                    switch (strretu)
                    {
                        case "int":
                            strclass.AppendSpaceLine(4, "return Convert.ToInt32(obj);");
                            break;
                        case "long":
                            strclass.AppendSpaceLine(4, "return Convert.ToInt64(obj);");
                            break;
                        case "decimal":
                            strclass.AppendSpaceLine(4, "return Convert.ToDecimal(obj);");
                            break;
                    }
                    strclass.AppendSpaceLine(3, "}");
                }
            strclass.AppendSpace(2, "}");
            return strclass.ToString();
        }

        /// <summary>
        /// 得到Update（）的代码
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public string CreatUpdate()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryUpdate"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Update(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"update " + _tablename + " set \");");
            if (Fieldlist.Count == 0)
            {
                Fieldlist = Keys;
            }
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string Length = field.Length;
                bool IsIdentity = field.IsIdentity;
                bool isPK = field.IsPrimaryKey;
                bool nullable = field.Nullable;

                if (field.IsIdentity || field.IsPrimaryKey || (Keys.Contains(field)))
                {
                    continue;
                }
                if (columnType.ToLower() == "timestamp")
                {
                    continue;
                }
                strclass.AppendSpaceLine(3, "if (model." + columnName + " != null)");
                strclass.AppendSpaceLine(3, "{");

                if ((dbobj.DbType == "Oracle") && (columnType.ToLower() == "date" || columnType.ToLower() == "datetime"))
                {
                    strclass.AppendSpaceLine(4, "strSql.Append(\"" + columnName + "=to_date('\" + model." + columnName + ".ToString() + \"','YYYY-MM-DD HH24:MI:SS'),\");");
                }
                else
                    if (columnType.ToLower() == "bit")
                    {
                        strclass.AppendSpaceLine(4, "strSql.Append(\"" + columnName + "=\"+ (model." + columnName + "? 1 : 0) +\",\");");
                    }
                    else
                        if (CodeCommon.IsAddMark(columnType.Trim()))
                        {
                            strclass.AppendSpaceLine(4, "strSql.Append(\"" + columnName + "='\"+model." + columnName + "+\"',\");");
                        }
                        else
                        {
                            strclass.AppendSpaceLine(4, "strSql.Append(\"" + columnName + "=\"+model." + columnName + "+\",\");");
                        }
                strclass.AppendSpaceLine(3, "}");
                if (nullable)
                {
                    strclass.AppendSpaceLine(3, "else");//是null的情况
                    strclass.AppendSpaceLine(3, "{");
                    strclass.AppendSpaceLine(4, "strSql.Append(\"" + columnName + "= null ,\");");
                    strclass.AppendSpaceLine(3, "}");
                }

            }

            //去掉最后的逗号		

            strclass.AppendSpaceLine(3, "int n = strSql.ToString().LastIndexOf(\",\");");
            strclass.AppendSpaceLine(3, "strSql.Remove(n, 1);");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + Maticsoft.CodeHelper.CodeCommon.GetModelWhereExpression(Keys, true) + "\");");
            strclass.AppendSpaceLine(3, "int rowsAffected=" + DbHelperName + ".ExecuteSql(strSql.ToString());");

            strclass.AppendSpaceLine(3, "if (rowsAffected > 0)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return true;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return false;");
            strclass.AppendSpaceLine(3, "}");

            strclass.AppendSpace(2, "}");
            return strclass.ToString();
        }
        /// <summary>
        /// 得到Delete的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatDelete()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            //if (dbobj.DbType != "OleDb")
            //{
            //    strclass.AppendSpaceLine(3, "strSql.Append(\"delete " + _tablename + " \");" );
            //}
            //else
            //{
            strclass.AppendSpaceLine(3, "strSql.Append(\"delete from " + _tablename + " \");");
            //}
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + Maticsoft.CodeHelper.CodeCommon.GetWhereExpression(Keys, true) + "\" );");

            strclass.AppendSpaceLine(3, "int rowsAffected=" + DbHelperName + ".ExecuteSql(strSql.ToString());");

            strclass.AppendSpaceLine(3, "if (rowsAffected > 0)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return true;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return false;");
            strclass.AppendSpaceLine(3, "}");


            strclass.AppendSpace(2, "}");



            #region 联合主键优先的删除(既有标识字段，又有非标识主键字段)

            if ((Maticsoft.CodeHelper.CodeCommon.HasNoIdentityKey(Keys)) && (Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(Keys) != null))
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false) + ")");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, KeysNullTip);
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"delete from " + _tablename + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" where " + GetWhereExpression(Keys, false) + "\");");
                strclass.AppendLine(GetPreParameter(Keys, false));

                strclass.AppendSpaceLine(3, "int rows=" + DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");
                strclass.AppendSpaceLine(3, "if (rows > 0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return true;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return false;");
                strclass.AppendSpaceLine(3, "}");

                strclass.AppendSpaceLine(2, "}");
            }

            #endregion

            #region 批量删除方法

            string keyField = "";
            if (Keys.Count == 1)
            {
                keyField = Keys[0].ColumnName;
            }
            else
            {
                foreach (ColumnInfo field in Keys)
                {
                    if (field.IsIdentity)
                    {
                        keyField = field.ColumnName;
                        break;
                    }
                }
            }
            if (keyField.Trim().Length > 0)
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDeletelist"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool DeleteList(string " + keyField + "list )");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"delete from " + _tablename + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" where " + keyField + " in (\"+" + keyField + "list + \")  \");");
                strclass.AppendSpaceLine(3, "int rows=" + DbHelperName + ".ExecuteSql(strSql.ToString());");
                strclass.AppendSpaceLine(3, "if (rows > 0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return true;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return false;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(2, "}");
            }
            #endregion



            return strclass.ToString();
        }

        /// <summary>
        /// 得到GetModel()的代码
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public string CreatGetModel()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModel"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            if (dbobj.DbType == "SQL2005" || dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012")
            {
                strclass.Append(" top 1 ");
            }
            strclass.AppendLine(" \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" " + Fieldstrlist + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" from " + _tablename + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + Maticsoft.CodeHelper.CodeCommon.GetWhereExpression(Keys, true) + "\" );");
            strclass.AppendSpaceLine(3, ModelSpace + " model=new " + ModelSpace + "();");
            strclass.AppendSpaceLine(3, "DataSet ds=" + DbHelperName + ".Query(strSql.ToString());");
            strclass.AppendSpaceLine(3, "if(ds.Tables[0].Rows.Count>0)");
            strclass.AppendSpaceLine(3, "{");
            

            #region 
            /*
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;

                strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"]!=null && ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                strclass.AppendSpaceLine(4, "{");
                #region
                switch (CodeCommon.DbTypeToCS(columnType))
                {
                    case "int":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "long":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=long.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "decimal":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "float":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=float.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "DateTime":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "string":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"]!=null)");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "bool":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))");
                            strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=true;");
                            strclass.AppendSpaceLine(5, "}");
                            strclass.AppendSpaceLine(5, "else");
                            strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=false;");
                            strclass.AppendSpaceLine(5, "}");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "byte[]":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"];");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "uniqueidentifier":
                    case "Guid":
                        {
                            //strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=new Guid(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    default:
                        strclass.AppendSpaceLine(5, "//model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
                        break;
                }
                #endregion
                strclass.AppendSpaceLine(4, "}");

            }
*/
            #endregion

            strclass.AppendSpaceLine(4, "return DataRowToModel(ds.Tables[0].Rows[0]);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return null;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpace(2, "}");
            return strclass.ToString();
        }

        /// <summary>
        /// DataRowToModel的代码
        /// </summary>      
        public string CreatDataRowToModel()
        {
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModel"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " DataRowToModel(DataRow row)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "" + ModelSpace + " model=new " + ModelSpace + "();");

            strclass.AppendSpaceLine(3, "if (row != null)");
            strclass.AppendSpaceLine(3, "{");

            #region 字段赋值
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;

                //strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                //strclass.AppendSpaceLine(4, "{");
                #region
                switch (CodeCommon.DbTypeToCS(columnType))
                {
                    case "int":
                        {
                            strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=int.Parse(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "long":
                        {
                            strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=long.Parse(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "decimal":
                        {
                            strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=decimal.Parse(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "float":
                        {
                            strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=float.Parse(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "DateTime":
                        {
                            strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=DateTime.Parse(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "string":
                        {
                            strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null)");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=row[\"" + columnName + "\"].ToString();");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "bool":
                        {
                            strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "if((row[\"" + columnName + "\"].ToString()==\"1\")||(row[\"" + columnName + "\"].ToString().ToLower()==\"true\"))");
                            strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=true;");
                            strclass.AppendSpaceLine(5, "}");
                            strclass.AppendSpaceLine(5, "else");
                            strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=false;");
                            strclass.AppendSpaceLine(5, "}");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "byte[]":
                        {
                            strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=(byte[])row[\"" + columnName + "\"];");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "uniqueidentifier":
                    case "Guid":
                        {
                            strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                            strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "= new Guid(row[\"" + columnName + "\"].ToString());");
                            strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    default:
                        strclass.AppendSpaceLine(5, "//model." + columnName + "=row[\"" + columnName + "\"].ToString();");
                        break;
                }
                #endregion
                //strclass.AppendSpaceLine(4, "}");
            }
            #endregion

            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return model;");
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }


        /// <summary>
        /// 得到GetList()的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatGetList()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            strclass.AppendLine(Fieldstrlist + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
            strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Query(strSql.ToString());");
            strclass.AppendSpaceLine(2, "}");

            if ((dbobj.DbType == "SQL2000") ||
                (dbobj.DbType == "SQL2005") ||
                (dbobj.DbType == "SQL2008") ||
                (dbobj.DbType == "SQL2012"))
            {
                strclass.AppendLine();
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList2"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public DataSet GetList(int Top,string strWhere,string filedOrder)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"select \");");
                strclass.AppendSpaceLine(3, "if(Top>0)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" top \"+Top.ToString());");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "strSql.Append(\" " + Fieldstrlist + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
                strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "strSql.Append(\" order by \" + filedOrder);");
                strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Query(strSql.ToString());");
                strclass.AppendSpaceLine(2, "}");
            }

            return strclass.Value;
        }

        /// <summary>
        /// 得到分页方法的代码
        /// </summary>        
        public string CreatGetListByPage()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["GetRecordCount"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public int GetRecordCount(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"select count(1) FROM " + TableName + " \");");
            strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "object obj = DbHelperSQL.GetSingle(strSql.ToString());");
            strclass.AppendSpaceLine(3, "if (obj == null)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return 0;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return Convert.ToInt32(obj);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(2, "}");



            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"SELECT * FROM ( \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" SELECT ROW_NUMBER() OVER (\");");
            strclass.AppendSpaceLine(3, "if (!string.IsNullOrEmpty(orderby.Trim()))");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\"order by T.\" + orderby );");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\"order by T." + _IdentityKey + " desc\");");
            strclass.AppendSpaceLine(3, "}");

            strclass.AppendSpaceLine(3, "strSql.Append(\")AS Row, T.*  from " + TableName + " T \");");
            strclass.AppendSpaceLine(3, "if (!string.IsNullOrEmpty(strWhere.Trim()))");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\" WHERE \" + strWhere);");
            strclass.AppendSpaceLine(3, "}");

            strclass.AppendSpaceLine(3, "strSql.Append(\" ) TT\");");
            strclass.AppendSpaceLine(3, "strSql.AppendFormat(\" WHERE TT.Row between {0} and {1}\", startIndex, endIndex);");

            strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Query(strSql.ToString());");
            strclass.AppendSpaceLine(2, "}");

            return strclass.Value;
        }




        /// <summary>
        /// 得到GetList()的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatGetListByPageProc()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/*");
            //strclass.AppendSpaceLine(2, "/// <summary>");
            //strclass.AppendSpaceLine(2, "/// "+Languagelist["summaryGetList3"].ToString());
            //strclass.AppendSpaceLine(2, "/// </summary>");
            //strclass.AppendSpaceLine(2, "public DataSet GetList(int PageSize,int PageIndex,string strWhere)");
            //strclass.AppendSpaceLine(2, "{");
            //strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            //strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "tblName\", " + DbParaDbType + ".VarChar, 255),");
            //strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "fldName\", " + DbParaDbType + ".VarChar, 255),");
            //strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "PageSize\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "int") + "),");
            //strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "PageIndex\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "int") + "),");
            //strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "IsReCount\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "bit") + "),");
            //strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "OrderType\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "bit") + "),");
            //strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "strWhere\", " + DbParaDbType + ".VarChar,1000),");
            //strclass.AppendSpaceLine(5, "};");
            //strclass.AppendSpaceLine(3, "parameters[0].Value = \"" + TableName + "\";");
            //strclass.AppendSpaceLine(3, "parameters[1].Value = \"" + _key + "\";");
            //strclass.AppendSpaceLine(3, "parameters[2].Value = PageSize;");
            //strclass.AppendSpaceLine(3, "parameters[3].Value = PageIndex;");
            //strclass.AppendSpaceLine(3, "parameters[4].Value = 0;");
            //strclass.AppendSpaceLine(3, "parameters[5].Value = 0;");
            //strclass.AppendSpaceLine(3, "parameters[6].Value = strWhere;	");
            //strclass.AppendSpaceLine(3, "return " + DbHelperName + ".RunProcedure(\"UP_GetRecordByPage\",parameters,\"ds\");");
            //strclass.AppendSpaceLine(2, "}");
            strclass.AppendSpaceLine(2, "*/");
            return strclass.Value;
        }
        #endregion//数据层


    }
}
