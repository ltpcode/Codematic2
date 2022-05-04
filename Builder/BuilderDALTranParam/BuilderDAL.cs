using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.BuilderDALTranParam
{
    /// <summary>
    /// 数据访问层代码构造器（Parameter方式）
    /// </summary>
    public class BuilderDAL : Maticsoft.IBuilder.IBuilderDALTran
    {

        #region 私有变量
        protected string _key = "ID";//标识列，或主键字段		
        protected string _keyType = "int";//标识列，或主键字段类型        
        #endregion

        #region 公有属性
        IDbObject dbobj;
        private string _dbname;
        private string _tablenameparent;
        private string _tablenameson;
        private List<ColumnInfo> _fieldlistparent;
        private List<ColumnInfo> _keysparent; // 主键或条件字段列表      
        private List<ColumnInfo> _fieldlistson;
        private List<ColumnInfo> _keysson; // 主键或条件字段列表                
        private string _namespace; //顶级命名空间名
        private string _folder; //所在文件夹               
        private string _modelpath;
        private string _modelnameparent;
        private string _modelnameson;
        private string _dalpath;
        private string _dalnameparent;
        private string _dalnameson;
        private string _idalpath;
        private string _iclass;
        private string _dbhelperName;//数据库访问类名    
        private string _procprefix;

        public IDbObject DbObject
        {
            set { dbobj = value; }
            get { return dbobj; }
        }
        /// <summary>
        /// 库名
        /// </summary>
        public string DbName
        {
            set { _dbname = value; }
            get { return _dbname; }
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableNameParent
        {
            set { _tablenameparent = value; }
            get { return _tablenameparent; }
        }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableNameSon
        {
            set { _tablenameson = value; }
            get { return _tablenameson; }
        }

        /// <summary>
        /// 选择的字段集合
        /// </summary>
        public List<ColumnInfo> FieldlistParent
        {
            set { _fieldlistparent = value; }
            get { return _fieldlistparent; }
        }
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        public List<ColumnInfo> FieldlistSon
        {
            set { _fieldlistson = value; }
            get { return _fieldlistson; }
        }
        /// <summary>
        /// 主键或条件字段的集合
        /// </summary>
        public List<ColumnInfo> KeysParent
        {
            set
            {
                _keysparent = value;
                foreach (ColumnInfo key in _keysparent)
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
            get { return _keysparent; }
        }
        /// <summary>
        /// 主键或条件字段的集合
        /// </summary>
        public List<ColumnInfo> KeysSon
        {
            set { _keysson = value; }
            get { return _keysson; }
        }
        /// <summary>
        /// 顶级命名空间名
        /// </summary>
        public string NameSpace
        {
            set { _namespace = value; }
            get { return _namespace; }
        }
        /// <summary>
        /// 所在文件夹
        /// </summary>
        public string Folder
        {
            set { _folder = value; }
            get { return _folder; }
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
        /// Model类名(父类)
        /// </summary>
        public string ModelNameParent
        {
            set { _modelnameparent = value; }
            get { return _modelnameparent; }
        }
        /// <summary>
        ///  Model类名(子类)
        /// </summary>
        public string ModelNameSon
        {
            set { _modelnameson = value; }
            get { return _modelnameson; }
        }
        /// <summary>
        /// 实体类的整个命名空间 + 类名
        /// </summary>
        public string ModelSpaceParent
        {
            get { return Modelpath + "." + ModelNameParent; }
        }
        /// <summary>
        /// 实体类的整个命名空间 + 类名
        /// </summary>
        public string ModelSpaceSon
        {
            get { return Modelpath + "." + ModelNameSon; }
        }
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
        /// <summary>
        /// 数据层的类名(父类)
        /// </summary>
        public string DALNameParent
        {
            set { _dalnameparent = value; }
            get { return _dalnameparent; }
        }
        /// <summary>
        /// 数据层的类名(子类)
        /// </summary>
        public string DALNameSon
        {
            set { _dalnameson = value; }
            get { return _dalnameson; }
        }

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
        /// <summary>
        /// 数据库访问类名
        /// </summary>
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
                return Maticsoft.CodeHelper.Language.LoadFromCfg("BuilderDALTranParam.lan");
            }
        }
        #endregion

        #region 构造属性

        /// <summary>
        /// 所选字段的 select 列表
        /// </summary>
        public string Fieldstrlist
        {
            get
            {
                StringPlus _fields = new StringPlus();
                foreach (ColumnInfo obj in FieldlistParent)
                {
                    _fields.Append(obj.ColumnName + ",");
                }
                _fields.DelLastComma();
                return _fields.Value;
            }
        }
        /// <summary>
        /// 所选字段的 select 列表
        /// </summary>
        public string FieldstrlistSon
        {
            get
            {
                StringPlus _fields = new StringPlus();
                foreach (ColumnInfo obj in FieldlistSon)
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
                return CodeCommon.DbParaHead(dbobj.DbType);
            }

        }
        /// <summary>
        ///  不同数据库字段类型
        /// </summary>
        public string DbParaDbType
        {
            get
            {
                return CodeCommon.DbParaDbType(dbobj.DbType);
            }
        }

        /// <summary>
        /// 存储过程参数 调用符号@
        /// </summary>
        public string preParameter
        {
            get
            {
                return CodeCommon.preParameter(dbobj.DbType);
            }
        }
        /// <summary>
        /// 父表主键或条件字段中是否有标识列
        /// </summary>
        public bool IsHasIdentity
        {
            get
            {
                return CodeCommon.IsHasIdentity(_keysparent);
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

        public BuilderDAL(IDbObject idbobj, string dbname, string tablename, string modelname,
            List<ColumnInfo> fieldlist, List<ColumnInfo> keys, string namepace,
            string folder, string dbherlpername, string modelpath,
            string dalpath, string idalpath, string iclass)
        {
            dbobj = idbobj;
            _dbname = dbname;
            _tablenameparent = tablename;
            _modelnameparent = modelname;
            _namespace = namepace;
            _folder = folder;
            _dbhelperName = dbherlpername;
            _modelpath = modelpath;
            _dalpath = dalpath;
            _idalpath = idalpath;
            _iclass = iclass;
            FieldlistParent = fieldlist;
            KeysParent = keys;
            foreach (ColumnInfo key in _keysparent)
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


        #region  根据列信息 得到参数的列表

        ///// <summary>
        ///// 得到Where条件语句 - Parameter方式 (例如：用于Exists  Delete  GetModel 的where)
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <returns></returns>
        //public string GetWhereExpression(List<ColumnInfo> keys)
        //{
        //    StringPlus strclass = new StringPlus();
        //    foreach (ColumnInfo key in keys)
        //    {
        //        strclass.Append(key.ColumnName + "=" + preParameter + key.ColumnName + " and ");
        //    }
        //    strclass.DelLastChar("and");
        //    return strclass.Value;
        //}

        ///// <summary>
        ///// 生成sql语句中的参数列表(例如：用于Add  Exists  Update Delete  GetModel 的参数传入)
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <returns></returns>
        //public string GetPreParameter(List<ColumnInfo> keys)
        //{
        //    StringPlus strclass = new StringPlus();
        //    StringPlus strclass2 = new StringPlus();
        //    strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
        //    int n = 0;
        //    foreach (ColumnInfo key in keys)
        //    {
        //        strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "" + key.ColumnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, key.TypeName, "") + "),");
        //        strclass2.AppendSpaceLine(3, "parameters[" + n.ToString() + "].Value = " + key.ColumnName + ";");
        //        n++;
        //    }
        //    strclass.DelLastComma();
        //    strclass.AppendLine("};");
        //    strclass.Append(strclass2.Value);
        //    return strclass.Value;
        //}

        /// <summary>
        /// 生成sql语句中的参数列表(例如：用于Add  Exists  Update Delete  GetModel 的参数传入)
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string GetPreParameter(List<ColumnInfo> keys, string numPara)
        {
            StringPlus strclass = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters" + numPara + " = {");
            int n = 0;
            foreach (ColumnInfo key in keys)
            {
                strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "" + key.ColumnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, key.TypeName, "") + "),");
                strclass2.AppendSpaceLine(3, "parameters" + numPara + "[" + n.ToString() + "].Value = " + key.ColumnName + ";");
                n++;
            }
            strclass.DelLastComma();
            strclass.AppendLine("};");
            strclass.Append(strclass2.Value);
            return strclass.Value;
        }

        #endregion

        #region 数据层(整个类)
        /// <summary>
        /// 得到整个类的代码
        /// </summary>     
        public string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Text;");
            strclass.AppendLine("using System.Collections.Generic;");
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
            strclass.AppendSpaceLine(1, "/// " + Languagelist["summary"].ToString() + ":" + DALNameParent);
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpace(1, "public partial class " + DALNameParent);
            if (IClass != "")
            {
                strclass.Append(":" + IClass);
            }
            strclass.AppendLine("");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, "public " + DALNameParent + "()");
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
                strclass.AppendLine(CreatGetListByPageProc());
            }
            #endregion

            strclass.AppendSpaceLine(2, "#endregion  Method");
            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            strclass.AppendLine("");

            return strclass.ToString();
        }

        #endregion

        #region 数据层(使用Parameter实现)

        /// <summary>
        /// 得到最大ID的方法代码
        /// </summary>
        public string CreatGetMaxID()
        {
            StringPlus strclass = new StringPlus();
            if (_keysparent.Count > 0)
            {
                string keyname = "";
                foreach (ColumnInfo obj in _keysparent)
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
                            strclass.AppendSpaceLine(2, "return " + DbHelperName + ".GetMaxID(\"" + keyname + "\", \"" + _tablenameparent + "\"); ");
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
        public string CreatExists()
        {
            StringPlus strclass = new StringPlus();
            if (_keysparent.Count > 0)
            {
                string strInparam = Maticsoft.CodeHelper.CodeCommon.GetInParameter(_keysparent, false);
                if (!string.IsNullOrEmpty(strInparam))
                {
                    strclass.AppendSpaceLine(2, "/// <summary>");
                    strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryExists"].ToString());
                    strclass.AppendSpaceLine(2, "/// </summary>");
                    strclass.AppendSpaceLine(2, "public bool Exists(" + strInparam + ")");
                    strclass.AppendSpaceLine(2, "{");
                    strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                    strclass.AppendSpaceLine(3, "strSql.Append(\"select count(1) from " + _tablenameparent + "\");");
                    strclass.AppendSpaceLine(3, "strSql.Append(\" where " + Maticsoft.CodeHelper.CodeCommon.GetWhereParameterExpression(KeysParent, false, dbobj.DbType) + "\");");

                    strclass.AppendLine(CodeCommon.GetPreParameter(KeysParent, false, dbobj.DbType));

                    strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Exists(strSql.ToString(),parameters);");
                    strclass.AppendSpaceLine(2, "}");
                }
            }
            return strclass.Value;
        }

        /// <summary>
        /// 得到Add()的代码
        /// </summary>        
        public string CreatAdd()
        {
            
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            StringPlus strclass3 = new StringPlus();
            StringPlus strclass4 = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 增加一条数据,及其子表数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            string strretu = "void";
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005" || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012") && (IsHasIdentity))
            {
                strretu = "int";
            }
            //方法定义头
            string strFun = CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpaceParent + " model)";
            strclass.AppendLine(strFun);
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"insert into " + _tablenameparent + "(\");");
            strclass1.AppendSpace(3, "strSql.Append(\"");
            int n = 0;
            int nkey = 0;
            foreach (ColumnInfo field in FieldlistParent)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                string Length = field.Length;
                if (field.IsIdentity)
                {
                    //nkey = n;
                    continue;
                }
                strclass3.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + columnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, columnType, Length) + "),");
                strclass1.Append(columnName + ",");
                strclass2.Append(preParameter + columnName + ",");
                strclass4.AppendSpaceLine(3, "parameters[" + n + "].Value = model." + columnName + ";");
                n++;
            }
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005"
                || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012") && (IsHasIdentity))
            {
                nkey = n;
                strclass3.AppendSpaceLine(5, "new SqlParameter(\"@ReturnValue\",SqlDbType.Int),");
                strclass4.AppendSpaceLine(3, "parameters[" + nkey.ToString() + "].Direction = ParameterDirection.Output;");
            }

            //去掉最后的逗号
            strclass1.DelLastComma();
            strclass2.DelLastComma();
            strclass3.DelLastComma();
            strclass1.AppendLine(")\");");
            strclass.Append(strclass1.ToString());
            strclass.AppendSpaceLine(3, "strSql.Append(\" values (\");");
            strclass.AppendSpaceLine(3, "strSql.Append(\"" + strclass2.ToString() + ")\");");
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005" || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012") && (IsHasIdentity))
            {
                strclass.AppendSpaceLine(3, "strSql.Append(\";set @ReturnValue= @@IDENTITY\");");
            }
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass.Append(strclass3.Value);
            strclass.AppendLine("};");
            strclass.AppendLine(strclass4.Value);

            #region tran
            strclass.AppendSpaceLine(3, "List<CommandInfo> sqllist = new List<CommandInfo>();");
            strclass.AppendSpaceLine(3, "CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);");
            strclass.AppendSpaceLine(3, "sqllist.Add(cmd);");

            strclass.AppendSpaceLine(3, "StringBuilder strSql2;");
            strclass.AppendSpaceLine(3, "foreach (" + ModelSpaceSon + " models in model." + ModelNameSon + "s)");
            strclass.AppendSpaceLine(3, "{");

            StringPlus strclass11 = new StringPlus();
            StringPlus strclass21 = new StringPlus();
            StringPlus strclass31 = new StringPlus();
            StringPlus strclass41 = new StringPlus();
            //新的增加
            strclass.AppendSpaceLine(4, "strSql2=new StringBuilder();");
            strclass.AppendSpaceLine(4, "strSql2.Append(\"insert into " + _tablenameson + "(\");");
            strclass11.AppendSpace(4, "strSql2.Append(\"");
            int ns = 0;
            foreach (ColumnInfo field in FieldlistSon)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                string Length = field.Length;
                if (field.IsIdentity)
                {
                    continue;
                }
                strclass31.AppendSpaceLine(6, "new " + DbParaHead + "Parameter(\"" + preParameter + columnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, columnType, Length) + "),");
                strclass11.Append(columnName + ",");
                strclass21.Append(preParameter + columnName + ",");
                strclass41.AppendSpaceLine(4, "parameters2[" + ns + "].Value = models." + columnName + ";");
                ns++;
            }
            strclass11.DelLastComma();
            strclass21.DelLastComma();
            strclass31.DelLastComma();
            strclass11.AppendLine(")\");");
            strclass.Append(strclass11.ToString());
            strclass.AppendSpaceLine(4, "strSql2.Append(\" values (\");");
            strclass.AppendSpaceLine(4, "strSql2.Append(\"" + strclass21.ToString() + ")\");");
            //if (IsHasIdentity)
            //{
            //    strclass.AppendSpaceLine(4, "strSql2.Append(\";select @@IDENTITY\");");
            //}
            strclass.AppendSpaceLine(4, "" + DbParaHead + "Parameter[] parameters2 = {");
            strclass.Append(strclass31.Value);
            strclass.AppendLine("};");
            strclass.AppendLine(strclass41.Value);
            //end新的增加

            strclass.AppendSpaceLine(4, "cmd = new CommandInfo(strSql2.ToString(), parameters2);");
            strclass.AppendSpaceLine(4, "sqllist.Add(cmd);");
            strclass.AppendSpaceLine(3, "}");
            #endregion


            //重新定义方法头
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005"
                || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012") && (IsHasIdentity))
            {
                strclass.AppendSpaceLine(3, DbHelperName + ".ExecuteSqlTranWithIndentity(sqllist);");
                strclass.AppendSpaceLine(3, "return (" + _keyType + ")parameters[" + nkey + "].Value;");
            }
            else
            {
                strclass.AppendSpaceLine(3, "" + DbHelperName + ".ExecuteSqlTran(sqllist);");
            }
            strclass.AppendSpace(2, "}");
            return strclass.ToString();
        }

        /// <summary>
        /// 得到Update（）的代码
        /// </summary>     
        public string CreatUpdate()
        {
            //if (ModelSpaceParent == "")
            //{
            //    ModelSpaceParent = "ModelClassName"; ;
            //}
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryUpdate"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Update(" + ModelSpaceParent + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"update " + _tablenameparent + " set \");");
            int n = 0;
            if (FieldlistParent.Count == 0)
            {
                FieldlistParent = KeysParent;
            }
            foreach (ColumnInfo field in FieldlistParent)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string Length = field.Length;
                bool IsIdentity = field.IsIdentity;
                bool isPK = field.IsPrimaryKey;

                strclass1.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + columnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, columnType, Length) + "),");
                strclass2.AppendSpaceLine(3, "parameters[" + n + "].Value = model." + columnName + ";");
                n++;
                if (field.IsIdentity || field.IsPrimaryKey || (KeysParent.Contains(field)))
                {
                    continue;
                }
                strclass.AppendSpaceLine(3, "strSql.Append(\"" + columnName + "=" + preParameter + columnName + ",\");");
            }


            //去掉最后的逗号			
            strclass.DelLastComma();
            strclass.AppendLine("\");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + CodeCommon.GetWhereParameterExpression(KeysParent, true, dbobj.DbType) + "\");");

            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass1.DelLastComma();
            strclass.Append(strclass1.Value);
            strclass.AppendLine("};");
            strclass.AppendLine(strclass2.Value);
            strclass.AppendSpaceLine(3, "int rowsAffected=" + DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");

            strclass.AppendSpaceLine(3, "if (rowsAffected > 0)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return true;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return false;");
            strclass.AppendSpaceLine(3, "}");

            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }
        /// <summary>
        /// 得到Delete的代码
        /// </summary>
        public string CreatDelete()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 删除一条数据，及子表所有相关数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(KeysParent, true) + ")");
            strclass.AppendSpaceLine(2, "{");

            strclass.AppendSpaceLine(3, "List<CommandInfo> sqllist = new List<CommandInfo>();");

            //子
            strclass.AppendSpaceLine(3, "StringBuilder strSql2=new StringBuilder();");
            if (dbobj.DbType != "OleDb")
            {
                strclass.AppendSpaceLine(3, "strSql2.Append(\"delete " + _tablenameson + " \");");
            }
            else
            {
                strclass.AppendSpaceLine(3, "strSql2.Append(\"delete from " + _tablenameson + " \");");
            }
            strclass.AppendSpaceLine(3, "strSql2.Append(\" where " + CodeCommon.GetWhereParameterExpression(KeysSon, true, dbobj.DbType) + "\");");
            strclass.AppendLine(GetPreParameter(KeysSon, "2"));
            strclass.AppendSpaceLine(3, "CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2);");
            strclass.AppendSpaceLine(3, "sqllist.Add(cmd);");


            //父
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            if (dbobj.DbType != "OleDb")
            {
                strclass.AppendSpaceLine(3, "strSql.Append(\"delete " + _tablenameparent + " \");");
            }
            else
            {
                strclass.AppendSpaceLine(3, "strSql.Append(\"delete from " + _tablenameparent + " \");");
            }
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + CodeCommon.GetWhereParameterExpression(KeysParent, true, dbobj.DbType) + "\");");
            
            strclass.AppendLine(CodeCommon.GetPreParameter(KeysParent, true, dbobj.DbType));
            strclass.AppendSpaceLine(3, "cmd = new CommandInfo(strSql.ToString(), parameters);");
            strclass.AppendSpaceLine(3, "sqllist.Add(cmd);");

            

            strclass.AppendSpaceLine(3, "int rowsAffected=" + DbHelperName + ".ExecuteSqlTran(sqllist);");

            strclass.AppendSpaceLine(3, "if (rowsAffected > 0)");
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
            if (KeysParent.Count == 1)
            {
                keyField = KeysParent[0].ColumnName;
            }
            else
            {
                foreach (ColumnInfo field in KeysParent)
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
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool DeleteList(string " + keyField + "list )");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "List<string> sqllist = new List<string>();");

                strclass.AppendSpaceLine(3, "StringBuilder strSql2=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql2.Append(\"delete from " + _tablenameson + " \");");
                strclass.AppendSpaceLine(3, "strSql2.Append(\" where " + KeysSon[0].ColumnName + " in (\"+" + keyField + "list + \")  \");");
                strclass.AppendSpaceLine(3, "sqllist.Add(strSql2.ToString());");

                strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
                strclass.AppendSpaceLine(3, "strSql.Append(\"delete from " + _tablenameparent + " \");");
                strclass.AppendSpaceLine(3, "strSql.Append(\" where " + keyField + " in (\"+" + keyField + "list + \")  \");");
                strclass.AppendSpaceLine(3, "sqllist.Add(strSql.ToString());");
                

                strclass.AppendSpaceLine(3, "int rowsAffected=" + DbHelperName + ".ExecuteSqlTran(sqllist);");
                strclass.AppendSpaceLine(3, "if (rowsAffected > 0)");
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
        public string CreatGetModel()
        {
            //if (ModelSpaceParent == "")
            //{
            //    ModelSpaceParent = "ModelClassName"; ;
            //}
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModel"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpaceParent + " GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(KeysParent, true) + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"select " + Fieldstrlist + " from " + _tablenameparent + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + CodeCommon.GetWhereParameterExpression(KeysParent, true, dbobj.DbType) + "\");");
                        
            strclass.AppendLine(CodeCommon.GetPreParameter(KeysParent, true, dbobj.DbType));

            strclass.AppendSpaceLine(3, "" + ModelSpaceParent + " model=new " + ModelSpaceParent + "();");
            strclass.AppendSpaceLine(3, "DataSet ds=" + DbHelperName + ".Query(strSql.ToString(),parameters);");
            strclass.AppendSpaceLine(3, "if(ds.Tables[0].Rows.Count>0)");
            strclass.AppendSpaceLine(3, "{");

            //父表数据
            strclass.AppendSpaceLine(4, "#region  父表信息");
            foreach (ColumnInfo field in FieldlistParent)
            {
                
                string columnName = field.ColumnName;
                string columnType = field.TypeName;

                strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"]!=null && ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                strclass.AppendSpaceLine(4, "{");
                #region 字段属性
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
                            strclass.AppendSpaceLine(5, "model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
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
            strclass.AppendSpaceLine(4, "#endregion  父表信息end");
            strclass.AppendLine();

            #region 子表数据

            strclass.AppendSpaceLine(4, "#region  子表信息");
            strclass.AppendSpaceLine(4, "StringBuilder strSql2=new StringBuilder();");
            strclass.AppendSpaceLine(4, "strSql2.Append(\"select " + FieldstrlistSon + " from " + _tablenameson + " \");");
            strclass.AppendSpaceLine(4, "strSql2.Append(\" where " +  CodeCommon.GetWhereParameterExpression(KeysSon, true, dbobj.DbType)+ "\");");
            strclass.AppendLine(GetPreParameter(KeysParent, "2"));
            strclass.AppendSpaceLine(4, "DataSet ds2=" + DbHelperName + ".Query(strSql2.ToString(),parameters2);");
            strclass.AppendSpaceLine(4, "if(ds2.Tables[0].Rows.Count>0)");
            strclass.AppendSpaceLine(4, "{");


            strclass.AppendSpaceLine(5, "#region  子表字段信息");
            strclass.AppendSpaceLine(5, "int i = ds2.Tables[0].Rows.Count;");
            strclass.AppendSpaceLine(5, "List<" + ModelSpaceSon + "> models = new List<" + ModelSpaceSon + ">();");
            strclass.AppendSpaceLine(5, ModelSpaceSon + " modelt;");
            strclass.AppendSpaceLine(5, "for (int n = 0; n < i; n++)");
            strclass.AppendSpaceLine(5, "{");
            strclass.AppendSpaceLine(6, "modelt = new " + ModelSpaceSon + "();");
            foreach (ColumnInfo field in FieldlistSon)
            {
                
                string columnName = field.ColumnName;
                string columnType = field.TypeName;

                strclass.AppendSpaceLine(6, "if(ds2.Tables[0].Rows[n][\"" + columnName + "\"]!=null && ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                strclass.AppendSpaceLine(6, "{");
                #region 字段属性
                switch (CodeCommon.DbTypeToCS(columnType))
                {
                    case "int":
                        {
                            //strclass.AppendSpaceLine(6, "if(ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(6, "{");
                            strclass.AppendSpaceLine(7, "modelt." + columnName + "=int.Parse(ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(6, "}");
                        }
                        break;
                    case "decimal":
                        {
                            //strclass.AppendSpaceLine(6, "if(ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(6, "{");
                            strclass.AppendSpaceLine(7, "modelt." + columnName + "=decimal.Parse(ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(6, "}");
                        }
                        break;
                    case "DateTime":
                        {
                            //strclass.AppendSpaceLine(6, "if(ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(6, "{");
                            strclass.AppendSpaceLine(7, "modelt." + columnName + "=DateTime.Parse(ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(6, "}");
                        }
                        break;
                    case "string":
                        {
                            strclass.AppendSpaceLine(7, "modelt." + columnName + "=ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString();");
                        }
                        break;
                    case "bool":
                        {
                            //strclass.AppendSpaceLine(6, "if(ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(6, "{");
                            strclass.AppendSpaceLine(7, "if((ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString()==\"1\")||(ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString().ToLower()==\"true\"))");
                            strclass.AppendSpaceLine(7, "{");
                            strclass.AppendSpaceLine(8, "modelt." + columnName + "=true;");
                            strclass.AppendSpaceLine(7, "}");
                            strclass.AppendSpaceLine(7, "else");
                            strclass.AppendSpaceLine(7, "{");
                            strclass.AppendSpaceLine(8, "modelt." + columnName + "=false;");
                            strclass.AppendSpaceLine(7, "}");
                            //strclass.AppendSpaceLine(6, "}");
                        }
                        break;
                    case "byte[]":
                        {
                            //strclass.AppendSpaceLine(6, "if(ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(6, "{");
                            strclass.AppendSpaceLine(7, "modelt." + columnName + "=(byte[])ds2.Tables[0].Rows[n][\"" + columnName + "\"];");
                            //strclass.AppendSpaceLine(6, "}");
                        }
                        break;
                    case "Guid":
                        {
                            //strclass.AppendSpaceLine(6, "if(ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(6, "{");
                            strclass.AppendSpaceLine(7, "modelt." + columnName + "=new Guid(ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(6, "}");
                        }
                        break;
                    default:
                        strclass.AppendSpaceLine(7, "modelt." + columnName + "=ds2.Tables[0].Rows[n][\"" + columnName + "\"].ToString();");
                        break;
                }
                #endregion

                strclass.AppendSpaceLine(6, "}");
            }
            strclass.AppendSpaceLine(6, "models.Add(modelt);");
            strclass.AppendSpaceLine(5, "}");
            strclass.AppendSpaceLine(5, "model." + ModelNameSon + "s = models;");
            strclass.AppendSpaceLine(5, "#endregion  子表字段信息end");

            strclass.AppendSpaceLine(4, "}");

            strclass.AppendSpaceLine(4, "#endregion  子表信息end");
            #endregion

            strclass.AppendLine();
            strclass.AppendSpaceLine(4, "return model;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return null;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }

        /// <summary>
        /// DataRowToModel的代码
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public string CreatDataRowToModel()
        {
            //if (ModelSpaceParent == "")
            //{
            //    //ModelSpace = "ModelClassName"; ;
            //}
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModel"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpaceParent + " DataRowToModel(DataRow row)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "" + ModelSpaceParent + " model=new " + ModelSpaceParent + "();");

            strclass.AppendSpaceLine(3, "if (row != null)");
            strclass.AppendSpaceLine(3, "{");

            #region 字段赋值
            foreach (ColumnInfo field in FieldlistParent)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;

                strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null && row[\"" + columnName + "\"].ToString()!=\"\")");
                strclass.AppendSpaceLine(4, "{");
                #region
                switch (CodeCommon.DbTypeToCS(columnType))
                {
                    case "int":
                        {
                            //strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=int.Parse(row[\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "long":
                        {
                            //strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=long.Parse(row[\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "decimal":
                        {
                            //strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=decimal.Parse(row[\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "float":
                        {
                            //strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=float.Parse(row[\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "DateTime":
                        {
                            //strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=DateTime.Parse(row[\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "string":
                        {
                            //strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"]!=null)");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=row[\"" + columnName + "\"].ToString();");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "bool":
                        {
                            //strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "if((row[\"" + columnName + "\"].ToString()==\"1\")||(row[\"" + columnName + "\"].ToString().ToLower()==\"true\"))");
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
                            //strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "=(byte[])row[\"" + columnName + "\"];");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    case "uniqueidentifier":
                    case "Guid":
                        {
                            //strclass.AppendSpaceLine(4, "if(row[\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(4, "{");
                            strclass.AppendSpaceLine(5, "model." + columnName + "= new Guid(row[\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(4, "}");
                        }
                        break;
                    default:
                        strclass.AppendSpaceLine(5, "//model." + columnName + "=row[\"" + columnName + "\"].ToString();");
                        break;
                }
                #endregion
                strclass.AppendSpaceLine(4, "}");
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
            strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableNameParent + " \");");
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
                strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableNameParent + " \");");
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
        /// 得到GetList()的代码
        /// </summary>
        public string CreatGetListByPageProc()
        {
            StringPlus strclass = new StringPlus();
            //strclass.AppendSpaceLine(2, "/*");
            //strclass.AppendSpaceLine(2, "/// <summary>");
            //strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
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
            //strclass.AppendSpaceLine(3, "parameters[0].Value = \"" + this.TableNameParent + "\";");
            //strclass.AppendSpaceLine(3, "parameters[1].Value = \"" + this._keysparent + "\";");
            //strclass.AppendSpaceLine(3, "parameters[2].Value = PageSize;");
            //strclass.AppendSpaceLine(3, "parameters[3].Value = PageIndex;");
            //strclass.AppendSpaceLine(3, "parameters[4].Value = 0;");
            //strclass.AppendSpaceLine(3, "parameters[5].Value = 0;");
            //strclass.AppendSpaceLine(3, "parameters[6].Value = strWhere;	");
            //strclass.AppendSpaceLine(3, "return " + DbHelperName + ".RunProcedure(\"UP_GetRecordByPage\",parameters,\"ds\");");
            //strclass.AppendSpaceLine(2, "}*/");
            return strclass.Value;
        }

        #endregion


    }
}
