using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Xml;
//引用
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;

namespace Maticsoft.BuilderDALIBatisNet
{
    /// <summary>
    /// 数据访问层代码构造器（IBatisNet方式）
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
        private List<ColumnInfo> _keys; // 主键或条件字段列表        
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
        public string TableName
        {
            set { _tablename = value; }
            get { return _tablename; }
        }

        /// <summary>
        /// 选择要生成的字段集合
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        /// <summary>
        /// 主键或条件字段的集合
        /// </summary>
        public List<ColumnInfo> Keys
        {
            set { _keys = value; }
            get { return _keys; }
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

        /*============================*/

        /// <summary>
        /// 实体类的命名空间
        /// </summary>
        public string Modelpath
        {
            set { _modelpath = value; }
            get { return _modelpath; }
        }
        /// <summary>
        /// 实体类名
        /// </summary>
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
        /// <summary>
        /// 实体类的程序集
        /// </summary>
        public string ModelAssembly
        {
            get
            {
                string _modelspace = _namespace + "." + "Model";
                return _modelspace;
            }
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
            get { return _iclass; }
        }
        /*============================*/

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
                return Maticsoft.CodeHelper.Language.LoadFromCfg("BuilderDALIBatisNet.lan");
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
        /// 主键或条件字段中是否有标识列
        /// </summary>
        public bool IsHasIdentity
        {
            get
            {
                return CodeCommon.IsHasIdentity(_keys);
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
            string folder, string dbherlpername, string modelpath,
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
            StringPlus strclass2 = new StringPlus();
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            int n = 0;
            foreach (ColumnInfo key in keys)
            {
                strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "" + key.ColumnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(dbobj.DbType, key.TypeName, "") + "),");
                strclass2.AppendSpaceLine(3, "parameters[" + n.ToString() + "].Value = " + key.ColumnName + ";");
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
            }
            if (IDALpath != "")
            {
                strclass.AppendLine("using " + IDALpath + ";");
            }
            strclass.AppendLine("using Maticsoft.DBUtility;//Please add references");
            strclass.AppendLine("namespace " + DALpath);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// " + Languagelist["summary"].ToString() + ":" + DALName );
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

            //strclass.AppendLine("/*   IBatisNet映射文件 ");
            //strclass.AppendLine(GetMapXMLs());
            //strclass.AppendLine("*/");

            return strclass.ToString();
        }

        #endregion

        #region 数据层(使用IBatisNet实现)

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
                            strclass.AppendSpaceLine(2, "public int GetMaxID()");
                            strclass.AppendSpaceLine(2, "{");
                            strclass.AppendSpaceLine(2, "return ExecuteGetMaxID(\"GetMaxID\"); ");
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
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryExists"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool Exists(object Id)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "return ExecuteExists(\"Exists\", Id);");               
                strclass.AppendSpaceLine(2, "}");
            }
            return strclass.Value;
        }

        /// <summary>
        /// 得到增加Add()的代码
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
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryadd"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            string strretu = "void";
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005"
                || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012") && (IsHasIdentity))
            {
                strretu = "int";
            }
            //方法定义头
            string strFun = CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpace + " model)";
            strclass.AppendLine(strFun);
            strclass.AppendSpaceLine(2, "{");            
            //重新定义方法头
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005"
                || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012") && (IsHasIdentity))
            {
                strclass.AppendSpaceLine(3, "return ExecuteInsert(\"Insert" + ModelName + "\", model);");

            }
            else
            {
                strclass.AppendSpaceLine(3, "ExecuteInsert(\"Insert" + ModelName + "\", model);");
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
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryUpdate"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public void Update(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "ExecuteUpdate(\"Update"+ModelName+"\", model);");
            strclass.AppendSpaceLine(2, "}");
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
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public void Delete(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "ExecuteDelete(\"Delete"+ModelName+"\", model);");
            strclass.AppendSpaceLine(2, "}");
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
            if (ModelSpace == "")
            {
                //ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.AppendLine();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModel"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModel(object Id)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, ModelSpace + " model = ExecuteQueryForObject<" + ModelSpace + ">(\"SelectById\", Id);");
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
            strclass.AppendSpaceLine(2, "public DataSet GetList(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "IList<" + ModelSpace + "> list = null; ");
            strclass.AppendSpaceLine(3, "list = ExecuteQueryForList<" + ModelSpace + ">(\"Select" + ModelName + "\", model); ");
            strclass.AppendSpaceLine(3, "return list; ");
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
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "tblName\", " + DbParaDbType + ".VarChar, 255),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "fldName\", " + DbParaDbType + ".VarChar, 255),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "PageSize\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "int") + "),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "PageIndex\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "int") + "),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "IsReCount\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "bit") + "),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "OrderType\", " + DbParaDbType + "." + CodeCommon.CSToProcType(dbobj.DbType, "bit") + "),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "strWhere\", " + DbParaDbType + ".VarChar,1000),");
            strclass.AppendSpaceLine(5, "};");
            strclass.AppendSpaceLine(3, "parameters[0].Value = \"" + this.TableName + "\";");
            strclass.AppendSpaceLine(3, "parameters[1].Value = \"" + this._key + "\";");
            strclass.AppendSpaceLine(3, "parameters[2].Value = PageSize;");
            strclass.AppendSpaceLine(3, "parameters[3].Value = PageIndex;");
            strclass.AppendSpaceLine(3, "parameters[4].Value = 0;");
            strclass.AppendSpaceLine(3, "parameters[5].Value = 0;");
            strclass.AppendSpaceLine(3, "parameters[6].Value = strWhere;	");
            strclass.AppendSpaceLine(3, "return " + DbHelperName + ".RunProcedure(\"UP_GetRecordByPage\",parameters,\"ds\");");
            strclass.AppendSpaceLine(2, "}*/");
            return strclass.Value;
        }

        #endregion


        #region  IBatisNet映射文件
        /// <summary>
        /// 得到IBatisNet映射文件
        /// </summary>
        /// <returns></returns>
        public string GetMapXMLs()
        {
            //1首先要创建一个空的XML文档
            XmlDocument xmldoc = new XmlDocument();

            //2在XML的文档的最头部加入XML的声明段落
            XmlNode xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmldoc.AppendChild(xmlnode);

            #region 加入一个根元素
            XmlElement xmlelem = xmldoc.CreateElement("", "sqlMap", "");
            XmlAttribute xmlAttr = xmldoc.CreateAttribute("xmlns");
            xmlAttr.Value = "http://ibatis.apache.org/mapping";
            xmlelem.Attributes.Append(xmlAttr);

            xmlAttr = xmldoc.CreateAttribute("xmlns:xsi");
            xmlAttr.Value = "http://www.w3.org/2001/XMLSchema-instance";
            xmlelem.Attributes.Append(xmlAttr);

            xmlAttr = xmldoc.CreateAttribute("namespace");
            xmlAttr.Value = ModelName;
            xmlelem.Attributes.Append(xmlAttr);

            xmldoc.AppendChild(xmlelem);

            #endregion


            #region  增加子元素 alias

            XmlElement xmlelem2 = xmldoc.CreateElement("alias");
            XmlElement xmlelem3 = xmldoc.CreateElement("typeAlias");
            XmlAttribute xmlAttr3 = xmldoc.CreateAttribute("alias");
            xmlAttr3.Value = ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("type");
            xmlAttr3.Value = ModelSpace + "," + ModelAssembly;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlelem2.AppendChild(xmlelem3);
            xmlelem.AppendChild(xmlelem2);

            #endregion


            #region 增加子元素 resultMaps

            xmlelem2 = xmldoc.CreateElement("resultMaps");
            xmlelem3 = xmldoc.CreateElement("resultMap");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "SelectAllResult";
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("class");
            xmlAttr3.Value = ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);
            XmlElement xmlelem4;
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                xmlelem4 = xmldoc.CreateElement("result");
                XmlAttribute xmlAttr4 = xmldoc.CreateAttribute("property");
                xmlAttr4.Value = columnName;
                xmlelem4.Attributes.Append(xmlAttr4);

                xmlAttr4 = xmldoc.CreateAttribute("column");
                xmlAttr4.Value = columnName;
                xmlelem4.Attributes.Append(xmlAttr4);
                xmlelem3.AppendChild(xmlelem4);
            }

            xmlelem2.AppendChild(xmlelem3);
            xmlelem.AppendChild(xmlelem2);

            #endregion


            #region  增加子元素 statements

            xmlelem2 = xmldoc.CreateElement("statements");

            #region GetMaxID
            xmlelem3 = xmldoc.CreateElement("select");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "GetMaxID";
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("resultClass");
            xmlAttr3.Value = "int";
            xmlelem3.Attributes.Append(xmlAttr3);

            XmlText xmltext = xmldoc.CreateTextNode("select max(" + _key + ") from " + TableName);
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion

            #region Exists
            xmlelem3 = xmldoc.CreateElement("select");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "Exists";
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("resultClass");
            xmlAttr3.Value = "int";
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("parameterclass");
            xmlAttr3.Value = _keyType;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmltext = xmldoc.CreateTextNode("select count(1) from  " + TableName + " where " + _key + " = #value#");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion

            #region Insert
            xmlelem3 = xmldoc.CreateElement("insert");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "Insert" + ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            //xmlAttr3 = xmldoc.CreateAttribute("resultClass");
            //xmlAttr3.Value = "int";
            //xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("parameterclass");
            xmlAttr3.Value = ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            StringBuilder sqlinsert1 = new StringBuilder();
            StringBuilder sqlinsert2 = new StringBuilder();
            
            #region 主键标识
            if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005"
                || dbobj.DbType == "SQL2008" || dbobj.DbType == "SQL2012") && (IsHasIdentity))
            {
                xmlelem4 = xmldoc.CreateElement("selectKey");
                XmlAttribute xmlAttr4 = xmldoc.CreateAttribute("property");
                xmlAttr4.Value = _key;
                xmlelem4.Attributes.Append(xmlAttr4);

                xmlAttr4 = xmldoc.CreateAttribute("type");
                xmlAttr4.Value = "post";
                xmlelem4.Attributes.Append(xmlAttr4);

                xmlAttr4 = xmldoc.CreateAttribute("resultClass");
                xmlAttr4.Value = "int";
                xmlelem4.Attributes.Append(xmlAttr4);

                xmltext = xmldoc.CreateTextNode("${selectKey}");
                xmlelem4.AppendChild(xmltext);

                xmlelem3.AppendChild(xmlelem4);
            }
           
            #endregion

            StringBuilder sqlInsert = new StringBuilder();
            StringPlus sql1 = new StringPlus();
            StringPlus sql2 = new StringPlus();
            sqlInsert.Append("insert into " + TableName + "(");
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                bool IsIdentity = field.IsIdentity;
                string Length = field.Length;
                if (field.IsIdentity)
                {
                    continue;
                }
                sql1.Append(columnName + ",");
                sql2.Append("#" + columnName + "#,");                                
            }
            sql1.DelLastComma();
            sql2.DelLastComma();
            sqlInsert.Append(sql1.Value);
            sqlInsert.Append(") values (");
            sqlInsert.Append(sql2.Value+")");
            xmltext = xmldoc.CreateTextNode(sqlInsert.ToString());
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion


            #region  update

            #endregion

            #region  delete
            xmlelem3 = xmldoc.CreateElement("delete");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "Delete"+ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            //xmlAttr3 = xmldoc.CreateAttribute("resultClass");
            //xmlAttr3.Value = "int";
            //xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("parameterclass");
            xmlAttr3.Value = _keyType;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmltext = xmldoc.CreateTextNode("delete from  " + TableName + " where " + _key + " = #value#");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion

            #region  SelectAll
            xmlelem3 = xmldoc.CreateElement("select");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "SelectAll" + ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("resultMap");
            xmlAttr3.Value = "SelectAllResult";
            xmlelem3.Attributes.Append(xmlAttr3);

            xmltext = xmldoc.CreateTextNode("select * from  " + TableName );
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion

            #region  SelectByID
            xmlelem3 = xmldoc.CreateElement("select");

            xmlAttr3 = xmldoc.CreateAttribute("id");
            xmlAttr3.Value = "SelectBy" + _key;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("resultMap");
            xmlAttr3.Value = "SelectAllResult";
            xmlelem3.Attributes.Append(xmlAttr3);


            xmlAttr3 = xmldoc.CreateAttribute("resultClass");
            xmlAttr3.Value = ModelName;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmlAttr3 = xmldoc.CreateAttribute("parameterclass");
            xmlAttr3.Value = _keyType;
            xmlelem3.Attributes.Append(xmlAttr3);

            xmltext = xmldoc.CreateTextNode("select * from " + TableName + " where " + _key + " = #value#");
            xmlelem3.AppendChild(xmltext);
            xmlelem2.AppendChild(xmlelem3);
            #endregion


            xmlelem.AppendChild(xmlelem2);
            #endregion
                       
            return xmldoc.OuterXml;


        }
        #endregion
    }
}
