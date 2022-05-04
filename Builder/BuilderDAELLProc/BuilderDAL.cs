using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.BuilderDALELProc
{
    /// <summary>
    /// 数据访问层代码构造器（存储过程方式）
    /// </summary>
    public class BuilderDAL : Maticsoft.IBuilder.IBuilderDAL
    {
        #region 私有变量
        protected string _key = "ID";//标识列，或主键字段		
        protected string _keyType = "int";//标识列，或主键字段类型        
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
        /// <summary>
        /// 主键或条件字段
        /// </summary>
        public List<ColumnInfo> Keys
        {
            set
            {
                _keys = value;

                foreach (ColumnInfo key in _keys)
                {
                    _key = key.ColumnName;
                    _keyType = key.TypeName;
                    if (key.IsIdentity)
                    {
                        _key = key.ColumnName;
                        _keyType = CodeCommon.DbTypeToCS(key.TypeName);
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
        /// <summary>
        /// 实体类的命名空间
        /// </summary>
        public string Modelpath
        {
            set { _modelpath = value; }
            get { return _modelpath; }
        }
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
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
        /*============================*/

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

        //语言文件
        public Hashtable Languagelist
        {
            get
            {
                return Maticsoft.CodeHelper.Language.LoadFromCfg("BuilderDALELProc.lan");
            }
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
        ///  不同数据库字段类型
        /// </summary>
        public string DbParaDbType
        {
            get
            {
                return "DbType";
            }
        }


        /// <summary>
        /// 存储过程参数 调用符号@
        /// </summary>
        public string preParameter
        {
            get
            {
                return "@";
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

        #region  根据列信息 得到参数的列表

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
                strclass.Append(key.ColumnName + "=" + preParameter + key.ColumnName + " and ");
            }
            strclass.DelLastChar("and");
            return strclass.Value;
        }

        /// <summary>
        /// 生成sql语句中的参数列表(例如：用于Add  Exists  Update Delete  GetModel 的参数传入)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string GetPreParameter(List<ColumnInfo> keys)
        {
            StringPlus strclass = new StringPlus();
            foreach (ColumnInfo key in keys)
            {
                strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"" + key.ColumnName + "\", DbType." + CSToProcType(key.TypeName) + "," + key.ColumnName + ");");
            }
            return strclass.Value;
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
            _dalname = dalName;
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
                _key = key.ColumnName;
                _keyType = key.TypeName;
                if (key.IsIdentity)
                {
                    _key = key.ColumnName;
                    _keyType = CodeCommon.DbTypeToCS(key.TypeName);
                    break;
                }
            }
        }

        #endregion


        #region 数据层(整个类)

        public string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Text;");
            strclass.AppendLine("using System.Collections.Generic;");
            strclass.AppendLine("using Microsoft.Practices.EnterpriseLibrary.Data;");
            strclass.AppendLine("using Microsoft.Practices.EnterpriseLibrary.Data.Sql;");
            strclass.AppendLine("using System.Data.Common;");
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
                strclass.AppendLine(CreatGetListArray());
                strclass.AppendLine(CreatReaderBind());
            }
            #endregion

            strclass.AppendSpaceLine(2, "#endregion  Method");
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            strclass.AppendLine("");

            return strclass.ToString();

        }

        #endregion


        #region 数据层(使用存储过程实现)

        /// <summary>
        /// 得到最大ID的方法代码
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
                            strclass.AppendSpaceLine(3, "string strsql = \"select max(" + keyname + ")+1 from " + _tablename + "\";");
                            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
                            strclass.AppendSpaceLine(3, "object obj = db.ExecuteScalar(CommandType.Text, strsql);");
                            strclass.AppendSpaceLine(3, "if (obj != null && obj != DBNull.Value)");
                            strclass.AppendSpaceLine(3, "{");
                            strclass.AppendSpaceLine(4, "return int.Parse(obj.ToString());");
                            strclass.AppendSpaceLine(3, "}");
                            strclass.AppendSpaceLine(3, "return 1;");
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
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatExists()
        {
            StringPlus strclass = new StringPlus();
            if (_keys.Count > 0)
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryExists"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool Exists(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false) + ")");
                strclass.AppendSpaceLine(2, "{");

                strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
                strclass.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"" + ProcPrefix + _tablename + "_Exists\");");
                strclass.Append(GetPreParameter(Keys));
                strclass.AppendSpaceLine(3, "int result;");
                strclass.AppendSpaceLine(3, "object obj = db.ExecuteScalar(dbCommand);");
                strclass.AppendSpaceLine(3, "int.TryParse(obj.ToString(),out result);");
                strclass.AppendSpaceLine(3, "if(result==1)");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return true;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(3, "else");
                strclass.AppendSpaceLine(3, "{");
                strclass.AppendSpaceLine(4, "return false;");
                strclass.AppendSpaceLine(3, "}");
                strclass.AppendSpaceLine(2, "}");
            }
            return strclass.Value;
        }

        /// <summary>
        /// 得到Add()的代码
        /// </summary>
        /// <param name="ExistsMaxId">是否有GetMaxId()生成主健</param>	
        public string CreatAdd()
        {                        
            StringPlus strclass = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "///  " + Languagelist["summaryadd"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            string strretu = "bool";
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005"
                || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012") && (IsHasIdentity))
            {
                strretu = "int";
                if (_keyType != "int")
                {
                    strretu = _keyType;
                }
            }

            //方法定义头
            string strFun = CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpace + " model)";
            strclass.AppendLine(strFun);
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"" + ProcPrefix + _tablename + "_ADD\");");

            string nkey = string.Empty; ;

            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                string Length = field.Length;

                if (field.IsIdentity)
                {
                    strclass.AppendSpaceLine(3, "db.AddOutParameter(dbCommand, \"" + columnName + "\", DbType." + CSToProcType(columnType) + ", " + Length + ");");
                    nkey = columnName;
                }
                else
                {
                    strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"" + columnName + "\", DbType." + CSToProcType(columnType) + ", model." + columnName + ");");
                }
            }

            strclass.AppendSpaceLine(3, "db.ExecuteNonQuery(dbCommand);");


            //重新定义方法头
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005"
                || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012") && (IsHasIdentity))
            {
                strclass.AppendSpaceLine(3, "return (" + _keyType + ")db.GetParameterValue(dbCommand, \"" + nkey + "\");");
            }
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;

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
            //if (ModelSpace == "")
            //{
            //    ModelSpace = "ModelClassName"; ;
            //}
            StringPlus strclass = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "///  " + Languagelist["summaryUpdate"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");

            strclass.AppendSpaceLine(2, "public void Update(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");

            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"" + ProcPrefix + _tablename + "_Update\");");

            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string Length = field.Length;

                strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"" + columnName + "\", DbType." + CSToProcType(columnType) + ", model." + columnName + ");");
            }
            strclass.AppendSpaceLine(3, "db.ExecuteNonQuery(dbCommand);");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;

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
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public void Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(2, "{");

            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"" + ProcPrefix + _tablename + "_Delete\");");

            strclass.AppendLine(GetPreParameter(Keys));            
            strclass.AppendSpaceLine(3, "int rows=db.ExecuteNonQuery(dbCommand);");
            strclass.AppendSpaceLine(3, "if (rows > 0)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return true;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return false;");
            strclass.AppendSpaceLine(3, "}");

            strclass.AppendSpaceLine(2, "}");


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


            return strclass.Value;

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
            //if (ModelSpace == "")
            //{
            //    ModelSpace = "ModelClassName"; ;
            //}
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModel"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(2, "{");

            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"" + ProcPrefix + _tablename + "_GetModel\");");

            strclass.AppendLine(GetPreParameter(Keys));
            strclass.AppendSpaceLine(3, "" + ModelSpace + " model=null;");
            strclass.AppendSpaceLine(3, "using (IDataReader dataReader = db.ExecuteReader(dbCommand))");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "if(dataReader.Read())");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "model=ReaderBind(dataReader);");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return model;");
            strclass.AppendSpaceLine(2, "}");

            return strclass.Value;
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
            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "return db.ExecuteDataSet(CommandType.Text, strSql.ToString());");
            strclass.AppendSpaceLine(2, "}");


            if ((dbobj.DbType == "SQL2000") ||
               (dbobj.DbType == "SQL2005") ||
               (dbobj.DbType == "SQL2008") ||
               (dbobj.DbType == "SQL2012")
                )
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
                strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
                strclass.AppendSpaceLine(3, "return db.ExecuteDataSet(CommandType.Text, strSql.ToString());");
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
            strclass.AppendSpaceLine(4, "strSql.Append(\"order by T." + _key + " desc\");");
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
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(int PageSize,int PageIndex,string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "DbCommand dbCommand = db.GetStoredProcCommand(\"UP_GetRecordByPage\");");
            strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"tblName\", DbType.AnsiString, \"" + TableName + "\");");
            strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"fldName\", DbType.AnsiString, \"" + _key + "\");");
            strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"PageSize\", DbType.Int32, PageSize);");
            strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"PageIndex\", DbType.Int32, PageIndex);");
            strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"IsReCount\", DbType.Boolean, 0);");
            strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"OrderType\", DbType.Boolean, 0);");
            strclass.AppendSpaceLine(3, "db.AddInParameter(dbCommand, \"strWhere\", DbType.AnsiString, strWhere);");
            strclass.AppendSpaceLine(3, "return db.ExecuteDataSet(dbCommand);");
            strclass.AppendSpaceLine(2, "}*/");
            return strclass.Value;
        }


        #region  生成对象实体绑定数据

        /// <summary>
        /// 生成对象实体绑定数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string CreatReaderBind()
        {
            //if (ModelSpace == "")
            //{
            //    ModelSpace = "ModelClassName"; ;
            //}
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 对象实体绑定数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " ReaderBind(IDataReader dataReader)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, ModelSpace + " model=new " + ModelSpace + "();");

            bool isobj = false;

            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                string Length = field.Length;

                switch (CodeCommon.DbTypeToCS(columnType))
                {
                    case "int":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(int)ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "long":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(long)ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "decimal":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");

                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(decimal)ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "DateTime":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(DateTime)ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "string":
                        {
                            strclass1.AppendSpaceLine(3, "model." + columnName + "=dataReader[\"" + columnName + "\"].ToString();");
                        }
                        break;
                    case "bool":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");

                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(bool)ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "byte[]":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");

                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "=(byte[])ojb;");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    case "Guid":
                        {
                            isobj = true;
                            strclass1.AppendSpaceLine(3, "ojb = dataReader[\"" + columnName + "\"];");
                            strclass1.AppendSpaceLine(3, "if(ojb != null && ojb != DBNull.Value)");
                            strclass1.AppendSpaceLine(3, "{");
                            strclass1.AppendSpaceLine(4, "model." + columnName + "= new Guid(ojb.ToString());");
                            strclass1.AppendSpaceLine(3, "}");
                        }
                        break;
                    default:
                        strclass1.AppendSpaceLine(3, "model." + columnName + "=dataReader[\"" + columnName + "\"].ToString();\r\n");
                        break;
                }
            }
            if (isobj)
            {
                strclass.AppendSpaceLine(3, "object ojb; ");
            }
            strclass.Append(strclass1.ToString());
            strclass.AppendSpaceLine(3, "return model;");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
        }


        public string CreatGetListArray()
        {
            string strList = "List<" + ModelSpace + ">";
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 获得数据列表（比DataSet效率高，推荐使用）");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + strList + " GetListArray(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            strclass.AppendLine(Fieldstrlist + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
            strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, strList + " list = new " + strList + "();");
            strclass.AppendSpaceLine(3, "Database db = DatabaseFactory.CreateDatabase();");
            strclass.AppendSpaceLine(3, "using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, strSql.ToString()))");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "while (dataReader.Read())");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "list.Add(ReaderBind(dataReader));");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return list;");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
        }


        #endregion

        #endregion//数据层


        #region CSToProcType
        /// <summary>
        /// 企业库数据库字段对应
        /// </summary>
        /// <param name="cstype"></param>
        /// <returns></returns>
        private static string CSToProcType(string cstype)
        {
            string ProcType = cstype;
            switch (cstype.Trim().ToLower())
            {

                case "string":
                case "nvarchar":
                case "nchar":
                case "ntext":
                    ProcType = "String";
                    break;
                case "text":
                case "char":
                case "varchar":
                    ProcType = "AnsiString";
                    break;
                case "datetime":
                case "smalldatetime":
                    ProcType = "DateTime";
                    break;
                case "smallint":
                    ProcType = "Int16";
                    break;
                case "tinyint":
                    ProcType = "Byte";
                    break;
                case "int":
                    ProcType = "Int32";
                    break;
                case "bigint":
                case "long":
                    ProcType = "Int64";
                    break;
                case "float":
                    ProcType = "Double";
                    break;
                case "real":
                case "numeric":
                case "decimal":
                    ProcType = "Decimal";
                    break;
                case "money":
                case "smallmoney":
                    ProcType = "Currency";
                    break;
                case "bool":
                case "bit":
                    ProcType = "Boolean";
                    break;
                case "binary":
                case "varbinary":
                    ProcType = "Binary";
                    break;
                case "image":
                    ProcType = "Image";
                    break;
                case "uniqueidentifier":
                    ProcType = "Guid";
                    break;
                case "timestamp":
                    ProcType = "String";
                    break;
                default:
                    ProcType = "String";
                    break;
            }
            return ProcType;
        }

        #endregion



    }
}
