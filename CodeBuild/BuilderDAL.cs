using System;
using System.Data;
using System.Text;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using LTP.Utility;
using LTP.IDBO;
using LTP.CodeHelper;
using LTP.DBFactory;
namespace LTP.CodeBuild
{
    /// <summary>
    /// 数据层代码生成
    /// </summary>
    public class BuilderDAL
    {

        #region 私有变量
        IDbObject dbobj;
        private string _dbtype;
        protected string _key = "ID";//默认第一个主键字段		
        protected string _keyType = "int";//默认第一个主键类型        
        #endregion

        #region 公有属性
                      
        private string _dbname;
        private string _tablename;
        private string _modelname; //model类名
        private ArrayList _fieldlist;
        private Hashtable _keys; //主键或条件字段列表        
        private string _namespace ; //顶级命名空间名
        private string _folder; //所在文件夹
        private string _dbhelperName ;//数据库访问类名
        private string _modelpath;
        private string _modelspace;
        private string _dalpath;
        private string _idalpath;
        private string _iclass;
       
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
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        public ArrayList Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        //选择的字段集合字符串
        public string Fields
        {
            get
            {
                StringPlus _fields = new StringPlus();
                foreach (object obj in Fieldlist)
                {
                    _fields.Append("'" + obj.ToString() + "',");
                }
                _fields.DelLastComma();
                return _fields.Value;
            }
        }
        public Hashtable Keys
        {
            set { _keys = value; }
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
        public string DbHelperName
        {
            set { _dbhelperName = value; }
            get{ return _dbhelperName; }
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
                        return "Sql";
                    case "Oracle":
                        return "Oracle";
                    case "OleDb":
                        return "OleDb";
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
                        return "SqlDbType";
                    case "Oracle":
                        return "OracleType";
                    case "OleDb":
                        return "OleDbType";
                    default:
                        return "SqlDbType";
                }
            }
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
        public string ModelSpace
        {
            set { _modelspace = value; }
            get
            {  return _modelspace; }
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
        /// 存储过程参数 调用符号@
        /// </summary>
        public string preParameter
        {
            get
            {
                switch (_dbtype)
                {
                    case "SQL2000":
                    case "SQL2005":
                        return "@";                        
                    case "Oracle":
                        return ":";                        
                    //case "OleDb":
                    //    break;
                    default:
                        return "@";                        
                }                
            }
        }
        #endregion		
                        
        public BuilderDAL(IDbObject idbobj)
        {
            dbobj = idbobj;
            _dbtype = idbobj.DbType;           
        }
        public BuilderDAL(IDbObject idbobj, string dbname, string tablename, string modelname,ArrayList fieldlist, Hashtable keys, string namepace,
            string folder,string dbherlpername,string modelpath,string modelspace,
            string dalpath,string idalpath,string iclass)
        {
            dbobj = idbobj;
            _dbtype = idbobj.DbType;
            _dbname=dbname;
            _tablename = tablename;
            _modelname = modelname;            
            _namespace = namepace;
            _folder = folder;
            _dbhelperName = dbherlpername;
            _modelpath = modelpath;
            _modelspace = modelspace;
            _dalpath = dalpath;
            _idalpath = idalpath;
            _iclass = iclass;
            Fieldlist = fieldlist;
            Keys = keys;
            foreach (DictionaryEntry key in keys)
            {
                _key = key.Key.ToString();
                _keyType = CodeCommon.DbTypeToCS(key.Value.ToString());
                break;
            }    
        }


        /// <summary>
        /// 得到字段列表
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GetFieldslist(DataTable dt)
        {
            StringPlus strclass = new StringPlus();
            foreach (DataRow row in dt.Rows)
            {
                string columnName = row["ColumnName"].ToString();
                strclass.Append("" + columnName + ",");
                //strclass.Append("[" + columnName + "],");
            }
            strclass.DelLastComma();
            return strclass.Value;
        }

        
        #region 数据层(使用SQL语句实现)
        /// <summary>
        /// 得到最大ID的方法代码
        /// </summary>
        /// <param name="TabName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CreatGetMaxIDSQL()
        {
            StringBuilder strclass = new StringBuilder();
            strclass.Append("\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// <summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// 得到最大ID" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// </summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "public int GetMaxId()" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "{" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "return " + DbHelperName + ".GetMaxID(\"" + _key + "\", \"" + _tablename + "\"); " + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "}");

            return strclass.ToString();
        }

        /// <summary>
        /// 得到Exists方法的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CreatExistsSQL()
        {
            StringBuilder strclass = new StringBuilder();
            strclass.Append("\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// <summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// 是否存在该记录" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// </summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "public bool Exists(" + _keyType + " " + _key + ")" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "{" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "StringBuilder strSql=new StringBuilder();" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\"select count(1) from " + _tablename);
            if (CodeCommon.IsAddMark(_keyType))
            {
                strclass.Append(" where " + _key + "='\"+" + _key + "+\"'\");" + "\r\n");
            }
            else
            {
                strclass.Append(" where " + _key + "=\"+" + _key + "+\"\");" + "\r\n");
            }
            strclass.Append(CodeCommon.Space(3) + "return " + DbHelperName + ".Exists(strSql.ToString());" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "}");

            return strclass.ToString();
        }
        /// <summary>
        /// 得到Add()的代码
        /// </summary>
        /// <param name="ExistsMaxId">是否有GetMaxId()生成主健</param>	
        public string CreatAddSQL(bool ExistsMaxId)
        {           
            DataTable dt = dbobj.GetColumnInfoList(DbName, _tablename);
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            bool KeyIsIdentity = false;//是否是自动增长列
            StringBuilder strclass = new StringBuilder();
            StringBuilder strclass1 = new StringBuilder();
            StringBuilder strclass2 = new StringBuilder();
            strclass.Append("\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// <summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// 增加一条数据" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// </summary>" + "\r\n");
            string strretu = "void";
            if (ExistsMaxId)
            {
                strretu = CodeCommon.DbTypeToCS(_keyType);
            }
            //方法定义头
            string strFun = CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpace + " model)";
            strclass.Append("$MaticsoftMethod$" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "{" + "\r\n");
            if (ExistsMaxId)
            {
                strclass.Append(CodeCommon.Space(3) + "//model." + _key + "=GetMaxId();" + "\r\n");
            }
            strclass.Append(CodeCommon.Space(3) + "StringBuilder strSql=new StringBuilder();" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\"insert into " + _tablename + "(\");" + "\r\n");
            strclass1.Append(CodeCommon.Space(3) + "strSql.Append(\"");
            if (dt != null)
            {
                DataRow[] dtrows;
                if (Fieldlist.Count > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                foreach (DataRow row in dtrows)
                {
                    string columnName = row["ColumnName"].ToString();
                    string columnType = row["TypeName"].ToString();
                    string IsIdentity = row["IsIdentity"].ToString();
                    //string isPK = row["isPK"].ToString();
                    //if ((isPK == "√") || (_key == columnName))
                    //{
                        if ((IsIdentity == "√") && (columnType == "int")) //if ((!ExistsMaxId) && (IsIdentity == "√")) 
                        {
                            KeyIsIdentity = true;
                            strretu = CodeCommon.DbTypeToCS(columnType);
                            continue;
                        }
                        
                    //}
                    strclass1.Append(columnName + ",");
                    if (CodeCommon.IsAddMark(columnType.Trim()))
                    {
                        strclass2.Append(CodeCommon.Space(3) + "strSql.Append(\"'\"+model." + columnName + "+\"',\");" + "\r\n");
                    }
                    else
                    {
                        strclass2.Append(CodeCommon.Space(3) + "strSql.Append(\"\"+model." + columnName + "+\",\");" + "\r\n");
                    }
                }
            }                        
            //去掉最后的逗号
            strclass1.Remove(strclass1.Length - 1, 1);
            strclass2.Remove(strclass2.Length - 6, 1);

            strclass1.Append("\");" + "\r\n");
            strclass.Append(strclass1.ToString());

            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\")\");" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\" values (\");" + "\r\n");
            strclass.Append(strclass2.ToString());

            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\")\");" + "\r\n");

            //重新定义方法头
            if (KeyIsIdentity)
            {
                strFun = CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpace + " model)";

                strclass.Append(CodeCommon.Space(3) + "strSql.Append(\";select @@IDENTITY\");" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "object obj = DbHelperSQL.GetSingle(strSql.ToString());" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "if (obj == null)" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "{" + "\r\n");
                strclass.Append(CodeCommon.Space(4) + "return 1;" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "}" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "else" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "{" + "\r\n");
                strclass.Append(CodeCommon.Space(4) + "return Convert.ToInt32(obj);" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "}" + "\r\n");
                
            }
            else
            {
                strclass.Append(CodeCommon.Space(3) + "" + DbHelperName + ".ExecuteSql(strSql.ToString());" + "\r\n");
                if (ExistsMaxId)
                {
                    strclass.Append(CodeCommon.Space(3) + "return model." + _key + ";" + "\r\n");
                }  
            }                       
            strclass.Replace("$MaticsoftMethod$", strFun);
                     
            strclass.Append(CodeCommon.Space(2) + "}");
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
        public string CreatUpdateSQL()
        {
            DataTable dt = dbobj.GetColumnInfoList(DbName, _tablename);
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringBuilder strclass = new StringBuilder();
            strclass.Append("\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// <summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// 更新一条数据" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// </summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "public void Update(" + ModelSpace + " model)" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "{" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "StringBuilder strSql=new StringBuilder();" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\"update " + _tablename + " set \");" + "\r\n");
            if (dt != null)
            {
                DataRow[] dtrows;
                if (Fieldlist.Count > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                foreach (DataRow row in dtrows)
                {
                    string columnName = row["ColumnName"].ToString();
                    string columnType = row["TypeName"].ToString();
                    string Length = row["Length"].ToString();
                    string IsIdentity = row["IsIdentity"].ToString();
                    string isPK = row["isPK"].ToString();
                    
                    if ((IsIdentity == "√") || (isPK == "√") || (_key == columnName))
                    {
                        continue;
                    }  
                    //if (_key == columnName)
                    //{
                    //    continue;
                    //}
                    if (CodeCommon.IsAddMark(columnType.Trim()))
                    {
                        strclass.Append(CodeCommon.Space(3) + "strSql.Append(\"" + columnName + "='\"+model." + columnName + "+\"',\");" + "\r\n");
                    }
                    else
                    {
                        strclass.Append(CodeCommon.Space(3) + "strSql.Append(\"" + columnName + "=\"+model." + columnName + "+\",\");" + "\r\n");
                    }
                }
            }

            //去掉最后的逗号			
            strclass.Remove(strclass.Length - 6, 1);
            if (CodeCommon.IsAddMark(_keyType))
            {
                strclass.Append(CodeCommon.Space(3) + "strSql.Append(\" where " + _key + "='\"+model." + _key + "+\"'\");" + "\r\n");
            }
            else
            {
                strclass.Append(CodeCommon.Space(3) + "strSql.Append(\" where " + _key + "=\"+model." + _key + "+\"\");" + "\r\n");
            }
            strclass.Append(CodeCommon.Space(3) + "" + DbHelperName + ".ExecuteSql(strSql.ToString());" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "}");
            return strclass.ToString();
        }
        /// <summary>
        /// 得到Delete的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatDeleteSQL()
        {
            StringBuilder strclass = new StringBuilder();
            strclass.Append("\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// <summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// 删除一条数据" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// </summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "public void Delete(" + _keyType + " " + _key + ")" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "{" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "	StringBuilder strSql=new StringBuilder();" + "\r\n");
            if (_dbtype != "OleDb")
            {
                strclass.Append(CodeCommon.Space(3) + "	strSql.Append(\"delete " + _tablename + " \");" + "\r\n");
            }
            else
            {
                strclass.Append(CodeCommon.Space(3) + "	strSql.Append(\"delete from " + _tablename + " \");" + "\r\n");
            }
            if (CodeCommon.IsAddMark(_keyType))
            {
                strclass.Append(CodeCommon.Space(3) + "	strSql.Append(\" where " + _key + "='\"+" + _key + "+\"'\");" + "\r\n");
            }
            else
            {
                strclass.Append(CodeCommon.Space(3) + "	strSql.Append(\" where " + _key + "=\"+" + _key + ");" + "\r\n");
            }
            strclass.Append(CodeCommon.Space(3) + "	" + DbHelperName + ".ExecuteSql(strSql.ToString());" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "}");
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
        public string CreatGetModelSQL()
        {            
            DataTable dt = dbobj.GetColumnList(DbName, _tablename);

            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringBuilder strclass = new StringBuilder();
            strclass.Append("\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// <summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// 得到一个对象实体" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// </summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "public " + ModelSpace + " GetModel(" + _keyType + " " + _key + ")" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "{" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "StringBuilder strSql=new StringBuilder();" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\"select * "  + " \");" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\" " + GetFieldslist(dt) + " \");" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\" from " + _tablename + " \");" + "\r\n");
            if (CodeCommon.IsAddMark(_keyType))
            {
                strclass.Append(CodeCommon.Space(3) + "strSql.Append(\" where " + _key + "='\"+" + _key + "+\"'\");" + "\r\n");
            }
            else
            {
                strclass.Append(CodeCommon.Space(3) + "strSql.Append(\" where " + _key + "=\"+" + _key + ");" + "\r\n");
            }
            strclass.Append(CodeCommon.Space(3) + "" + ModelSpace + " model=new " + ModelSpace + "();" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "DataSet ds=" + DbHelperName + ".Query(strSql.ToString());" + "\r\n");
            //strclass.Append(CodeCommon.Space(3) + "model." + _key + "=" + _key + ";" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "if(ds.Tables[0].Rows.Count>0)" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "{" + "\r\n");
            if (dt != null)
            {
                DataRow[] dtrows;
                if (Fieldlist.Count > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                foreach (DataRow row in dtrows)
                {
                    string columnName = row["ColumnName"].ToString(); //dbReader.GetString(0);
                    string columnType = row["TypeName"].ToString();//dbReader.GetString(1);
                    //if (_key == columnName)
                    //{
                    //    continue;
                    //}
                    switch (CodeCommon.DbTypeToCS(columnType))
                    {
                        case "int":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        case "decimal":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        case "DateTime":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        case "string":
                            {
                                strclass.Append(CodeCommon.Space(4) + "model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();" + "\r\n");
                            }
                            break;
                        case "bool":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(6) + "model." + columnName + "=true;" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "}" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "else" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(6) + "model." + columnName + "=false;" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "}" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n" + "\r\n");
                            }
                            break;
                        case "byte[]":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"];" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        case "Guid":
                            {                                
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=new Guid(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        default:
                            strclass.Append(CodeCommon.Space(4) + "model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();" + "\r\n");
                            break;

                    }

                }
            }
            strclass.Append(CodeCommon.Space(4) + "return model;" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "}" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "else" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "{" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "return null;" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "}" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "}");
            return strclass.ToString();
        }

        /// <summary>
        /// 得到GetList()的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatGetListSQL()
        {    

            DataTable dt = dbobj.GetColumnList(DbName, TableName);
            StringPlus strclass = new StringPlus();            
            if (dt != null)
            {
                DataRow[] dtrows;
                if (Fieldlist.Count > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                //foreach (DataRow row in dtrows)
                //{
                //    string columnName = row["ColumnName"].ToString();
                //    strclass1.Append("[" + columnName + "],");
                //}
                //strclass1.DelLastComma();
            }            
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 获得数据列表");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            strclass.AppendLine(GetFieldslist(dt) + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
            strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            strclass.AppendSpaceLine(3, "{" + "\r\n");
            strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Query(strSql.ToString());");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
        }

        #endregion//数据层

        #region 数据层(使用存储过程实现)

        /// <summary>
        /// 得到最大_key的方法代码
        /// </summary>
        /// <param name="TabName"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatGetMaxIDProc()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 得到最大ID");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public int GetMaxId()");
            strclass.AppendSpaceLine(2, "{" + "\r\n");
            strclass.AppendSpaceLine(3, "int rowsAffected;");
            strclass.AppendSpaceLine(3, "return " + DbHelperName + ".RunProcedure(\"" + "UP_" + _tablename + "_GetMaxId" + "\",null,out rowsAffected);");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();
        }

        /// <summary>
        /// 得到Exists方法的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatExistsProc()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 是否存在该记录");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Exists(" + _keyType + " " + _key + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "int rowsAffected;");
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + _key + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(_dbtype, _keyType, "") + ")");
            strclass.AppendSpaceLine(4, "};");
            strclass.AppendSpaceLine(3, "parameters[0].Value = " + _key + ";");
            strclass.AppendSpaceLine(3, "int result= " + DbHelperName + ".RunProcedure(\"UP_" + _tablename + "_Exists\",parameters,out rowsAffected);");
            strclass.AppendSpaceLine(3, "if(result==1)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return true;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return false;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
        }
        /// <summary>
        /// 得到Add()的代码
        /// </summary>
        /// <param name="ExistsMaxId">是否有GetMaxId()生成主健</param>	
        public string CreatAddProc(bool ExistsMaxId)
        {
            
            DataTable dt = dbobj.GetColumnInfoList(DbName, _tablename);
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            bool KeyIsIdentity = false;//是否是自动增长列
            StringPlus strclass = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "///  增加一条数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            string strretu = "void";
            if (ExistsMaxId)
            {
                strretu = CodeCommon.DbTypeToCS(_keyType);
            }
            //方法定义头
            string strFun = CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpace + " model)";
            strclass.Append("$MaticsoftMethod$" + "\r\n");


            //strclass.AppendSpaceLine(2, "public " + _keyType + " Add(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            if (ExistsMaxId)
            {
                strclass.AppendSpaceLine(3, "//model." + _key + "=GetMaxId();");
            }
            strclass.AppendSpaceLine(3, "int rowsAffected;");
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            int nkey = 0;
            if (dt != null)
            {
                int n = 0;
                DataRow[] dtrows;
                if (Fieldlist.Count > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                foreach (DataRow row in dtrows)
                {
                    string columnName = row["ColumnName"].ToString();
                    string columnType = row["TypeName"].ToString();
                    string Length = row["Length"].ToString();
                    string IsIdentity = row["IsIdentity"].ToString();
                    //string isPK = row["isPK"].ToString();
                    strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + columnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(_dbtype, columnType, Length) + "),");
                    //if ((isPK == "√") || (_key == columnName))
                    //{
                    //    if (IsIdentity == "√")
                    //    {
                    //        KeyIsIdentity = true;
                    //    }
                    //    continue;
                    //}
                    //if ((isPK == "√") || (_key == columnName))
                    //{
                        if ((IsIdentity == "√") && (columnType == "int"))  //if ((!ExistsMaxId) && (IsIdentity == "√"))
                        {
                            KeyIsIdentity = true;
                            nkey = n;
                            strclass2.AppendSpaceLine(3, "parameters[" + n + "].Direction = ParameterDirection.Output;");
                            n++;
                            continue;
                        }
                    //}
                    strclass2.AppendSpaceLine(3, "parameters[" + n + "].Value = model." + columnName + ";");
                    n++;
                }
            }
            strclass.DelLastComma();
            strclass.AppendLine("};");
            strclass.AppendLine(strclass2.Value);
            strclass.AppendSpaceLine(3, "" + DbHelperName + ".RunProcedure(\"" + "UP_" + _tablename + "_ADD" + "\",parameters,out rowsAffected);");
            //重新定义方法头
            if (KeyIsIdentity)
            {
                strFun = CodeCommon.Space(2) + "public int Add(" + ModelSpace + " model)";

                strclass.AppendSpaceLine(3, "return (" + _keyType + ")parameters[" + nkey + "].Value;");
            }
            else
            {
                if (ExistsMaxId)
                {
                    strclass.AppendSpaceLine(3, "return model." + _key + ";");
                }                
            }                       
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value.Replace("$MaticsoftMethod$", strFun);

        }

        /// <summary>
        /// 得到Update（）的代码
        /// </summary>
        /// <param name="DbName"></param>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <param name="ModelName"></param>
        /// <returns></returns>
        public string CreatUpdateProc()
        {

            
            DataTable dt = dbobj.GetColumnInfoList(DbName, _tablename);
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "///  更新一条数据");
            strclass.AppendSpaceLine(2, "/// </summary>");

            strclass.AppendSpaceLine(2, "public void Update(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "int rowsAffected;");
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            int n = 0;
            if (dt != null)
            {
                DataRow[] dtrows;
                if (Fieldlist.Count > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                foreach (DataRow row in dtrows)
                {
                    string columnName = row["ColumnName"].ToString();
                    string columnType = row["TypeName"].ToString();
                    string Length = row["Length"].ToString();
                    strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + columnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(_dbtype, columnType, Length) + "),");
                    strclass2.AppendSpaceLine(3, "parameters[" + n + "].Value = model." + columnName + ";");
                    n++;
                }
            }
            strclass.DelLastComma();
            strclass.AppendLine("};");
            strclass.AppendLine(strclass2.Value);
            strclass.AppendSpaceLine(3, "" + DbHelperName + ".RunProcedure(\"" + "UP_" + _tablename + "_Update" + "\",parameters,out rowsAffected);");
            //			strclass.AppendSpaceLine(3,"return (rowsAffected == 1);");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;

        }
        /// <summary>
        /// 得到Delete的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatDeleteProc()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 删除一条数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public void Delete(" + _keyType + " " + _key + ")");
            strclass.AppendSpaceLine(2, "{" + "\r\n");
            strclass.AppendSpaceLine(3, "int rowsAffected;");
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + _key + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(_dbtype, _keyType, "") + ")");
            strclass.AppendSpaceLine(4, "};");
            strclass.AppendSpaceLine(3, "parameters[0].Value = " + _key + ";");
            strclass.AppendSpaceLine(3, "" + DbHelperName + ".RunProcedure(\"" + "UP_" + _tablename + "_Delete" + "\",parameters,out rowsAffected);");
            //			strclass.AppendSpaceLine(3,"return (rowsAffected == 1);");
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
        public string CreatGetModelProc()
        {
            
            DataTable dt = dbobj.GetColumnList(DbName, _tablename);
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 得到一个对象实体");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModel(" + _keyType + " " + _key + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + _key + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(_dbtype, _keyType, "") + ")");
            strclass.AppendSpaceLine(4, "};");
            strclass.AppendSpaceLine(3, "parameters[0].Value = " + _key + ";");
            strclass.AppendSpaceLine(3, "" + ModelSpace + " model=new " + ModelSpace + "();" + "\r\n");
            strclass.AppendSpaceLine(3, "DataSet ds= " + DbHelperName + ".RunProcedure(\"" + "UP_" + _tablename + "_GetModel" + "\",parameters,\"ds\");");

            //strclass.AppendSpaceLine(3, "model." + _key + "=" + _key + ";");
            strclass.AppendSpaceLine(3, "if(ds.Tables[0].Rows.Count>0)");
            strclass.AppendSpaceLine(3, "{");
            if (dt != null)
            {
                DataRow[] dtrows;
                if (Fieldlist.Count > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                foreach (DataRow row in dtrows)
                {
                    string columnName = row["ColumnName"].ToString(); //dbReader.GetString(0);
                    string columnType = row["TypeName"].ToString();//dbReader.GetString(1);
                    //if (_key == columnName)
                    //{
                    //    continue;
                    //}
                    switch (CodeCommon.DbTypeToCS(columnType))
                    {
                        case "int":
                            {
                                strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                                strclass.AppendSpaceLine(4, "{");
                                strclass.AppendSpaceLine(5, "model." + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                                strclass.AppendSpaceLine(4, "}");
                            }
                            break;
                        case "decimal":
                            {
                                strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                                strclass.AppendSpaceLine(4, "{");
                                strclass.AppendSpaceLine(5, "model." + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                                strclass.AppendSpaceLine(4, "}");
                            }
                            break;
                        case "DateTime":
                            {
                                strclass.AppendSpaceLine(4, "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")");
                                strclass.AppendSpaceLine(4, "{" + "\r\n");
                                strclass.AppendSpaceLine(5, "model." + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());");
                                strclass.AppendSpaceLine(4, "}");
                            }
                            break;
                        case "string":
                            {
                                strclass.AppendSpaceLine(4, "model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
                            }
                            break;
                        case "bool":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(6) + "model." + columnName + "=true;" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "}" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "else" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(6) + "model." + columnName + "=false;" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "}" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n" + "\r\n");
                            }
                            break;
                        case "byte[]":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"];" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        case "Guid":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=new Guid(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        default:
                            strclass.AppendSpaceLine(4, "model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();");
                            break;

                    }

                }
            }
            strclass.AppendSpaceLine(4, "return model;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "else");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "return null;");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;

        }

        /// <summary>
        /// 得到GetList()的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatGetListProc()
        {
            //StringPlus strclass = new StringPlus();
            //strclass.AppendSpaceLine(2, "/// <summary>");
            //strclass.AppendSpaceLine(2, "/// 获取数据列表");
            //strclass.AppendSpaceLine(2, "/// </summary>");
            //strclass.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            //strclass.AppendSpaceLine(2, "{");
            ////strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            ////strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + _key + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(_dbtype, _keyType, "") + ")");
            ////strclass.AppendSpaceLine(4, "};");
            ////strclass.AppendSpaceLine(3, "parameters[0].Value = strWhere;");
            //strclass.AppendSpaceLine(3, "return " + DbHelperName + ".RunProcedure(\"" + "UP_" + _tablename + "_GetList" + "\",null,\"ds\");");
            //strclass.AppendSpaceLine(2, "}");
            //return strclass.Value;


            DataTable dt = dbobj.GetColumnList(DbName, TableName);
            StringPlus strclass = new StringPlus();            
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 获得数据列表");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            strclass.AppendLine(GetFieldslist(dt) + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
            strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            strclass.AppendSpaceLine(3, "}");
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
            strclass.AppendSpaceLine(2, "/// 分页获取数据列表");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(int PageSize,int PageIndex,string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "tblName\", " + DbParaDbType + ".VarChar, 255),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "fldName\", " + DbParaDbType + ".VarChar, 255),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "PageSize\", " + DbParaDbType + "." + CodeCommon.CSToProcType(_dbtype, "int") + "),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "PageIndex\", " + DbParaDbType + "." + CodeCommon.CSToProcType(_dbtype, "int") + "),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "IsReCount\", " + DbParaDbType + "." + CodeCommon.CSToProcType(_dbtype, "bit") + "),");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "OrderType\", " + DbParaDbType + "." + CodeCommon.CSToProcType(_dbtype, "bit") + "),");
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

        #endregion//数据层

        #region 数据层(使用Parameter实现)

        /// <summary>
        /// 得到最大ID的方法代码
        /// </summary>
        /// <param name="TabName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CreatGetMaxIDParam()
        {
            StringBuilder strclass = new StringBuilder();
            strclass.Append("\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// <summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// 得到最大ID" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// </summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "public int GetMaxId()" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "{" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "return " + DbHelperName + ".GetMaxID(\"" + _key + "\", \"" + _tablename + "\"); " + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "}");

            return strclass.ToString();
        }

        /// <summary>
        /// 得到Exists方法的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string CreatExistsParam()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 是否存在该记录");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Exists(" + _keyType + " " + _key + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"select count(1) from " + _tablename + "\");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + _key + "= " + preParameter + "" + _key + "\");");

            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + "" + _key + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(_dbtype, _keyType, "") + ")");
            strclass.AppendSpaceLine(4, "};");
            strclass.AppendSpaceLine(3, "parameters[0].Value = " + _key + ";");
            strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Exists(strSql.ToString(),parameters);");
            strclass.AppendSpaceLine(2, "}");

            return strclass.Value;
        }

        /// <summary>
        /// 得到Add()的代码
        /// </summary>
        /// <param name="ExistsMaxId">是否有GetMaxId()生成主健</param>	
        public string CreatAddParam(bool IsGetMaxId)
        {
            
            DataTable dt = dbobj.GetColumnInfoList(DbName, _tablename);
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            bool KeyIsIdentity = false;//是否是自动增长列
            StringBuilder strclass = new StringBuilder();
            StringBuilder strclass1 = new StringBuilder();
            StringBuilder strclass2 = new StringBuilder();
            StringPlus strclass3 = new StringPlus();
            StringPlus strclass4 = new StringPlus();
            strclass.Append("\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// <summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// 增加一条数据" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// </summary>" + "\r\n");
            
            string strretu = "void";
            if (IsGetMaxId)
            {
                strretu = CodeCommon.DbTypeToCS(_keyType);
            }
            //方法定义头
            string strFun = CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpace + " model)";
            strclass.Append("$MaticsoftMethod$" + "\r\n");

            //strclass.Append(CodeCommon.Space(2) + "public " + strretu + " Add(" + ModelSpace + " model)" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "{" + "\r\n");
            if (IsGetMaxId)
            {
                strclass.Append(CodeCommon.Space(3) + "//model." + _key + "=GetMaxId();" + "\r\n");
            }
            strclass.Append(CodeCommon.Space(3) + "StringBuilder strSql=new StringBuilder();" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\"insert into " + _tablename + "(\");" + "\r\n");
            strclass1.Append(CodeCommon.Space(3) + "strSql.Append(\"");
            if (dt != null)
            {
                int n = 0;
                DataRow[] dtrows;
                if (Fieldlist.Count > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                foreach (DataRow row in dtrows)
                {
                    string columnName = row["ColumnName"].ToString();
                    string columnType = row["TypeName"].ToString();
                    string Length = row["Length"].ToString();
                    string IsIdentity = row["IsIdentity"].ToString();
                    //string isPK = row["isPK"].ToString();                                    

                    //if ((isPK == "√") || (_key == columnName))
                    //{
                        if ((IsIdentity == "√") && (columnType == "int")) 
                        {
                            KeyIsIdentity = true;
                            continue;
                        }                        
                    //}
                    strclass3.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\""+preParameter + columnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(_dbtype,columnType, Length) + "),");
                    strclass1.Append(columnName + ",");
                    strclass2.Append(preParameter + columnName + ",");
                    strclass4.AppendSpaceLine(3, "parameters[" + n + "].Value = model." + columnName + ";");
                    n++;
                }
            }
            //去掉最后的逗号
            strclass1.Remove(strclass1.Length - 1, 1);
            strclass2.Remove(strclass2.Length - 1, 1);
            strclass3.DelLastComma();

            strclass1.Append(")\");" + "\r\n");
            strclass.Append(strclass1.ToString());
                        
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\" values (\");" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\"" + strclass2.ToString() + ")\");" + "\r\n");

            if (KeyIsIdentity)
            {
                strclass.Append(CodeCommon.Space(3) + "strSql.Append(\";select @@IDENTITY\");" + "\r\n");
            }

            strclass.Append(CodeCommon.Space(3) + "" + DbParaHead + "Parameter[] parameters = {" + "\r\n");
            strclass.Append(strclass3.Value);
            strclass.Append("};" + "\r\n");
            strclass.Append(strclass4.Value + "\r\n");

            //strclass.Append(CodeCommon.Space(3) + "" + DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);" + "\r\n");
            //if (IsGetMaxId)
            //{
            //    strclass.Append(CodeCommon.Space(3) + "return model." + _key + ";" + "\r\n");
            //}

            //重新定义方法头
            if (KeyIsIdentity)
            {
                strFun = CodeCommon.Space(2) + "public int Add(" + ModelSpace + " model)";
                
                strclass.Append(CodeCommon.Space(3) + "object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "if (obj == null)" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "{" + "\r\n");
                strclass.Append(CodeCommon.Space(4) + "return 1;" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "}" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "else" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "{" + "\r\n");
                strclass.Append(CodeCommon.Space(4) + "return Convert.ToInt32(obj);" + "\r\n");
                strclass.Append(CodeCommon.Space(3) + "}" + "\r\n");

            }
            else
            {
                strclass.Append(CodeCommon.Space(3) + "" + DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);" + "\r\n");
                if (IsGetMaxId)
                {
                    strclass.Append(CodeCommon.Space(3) + "return model." + _key + ";" + "\r\n");
                }
            }
            strclass.Replace("$MaticsoftMethod$", strFun);

            strclass.Append(CodeCommon.Space(2) + "}");
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
        public string CreatUpdateParam()
        {            
            DataTable dt = dbobj.GetColumnInfoList(DbName, _tablename);
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 更新一条数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public void Update(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpaceLine(3, "strSql.Append(\"update " + _tablename + " set \");");
            if (dt != null)
            {
                int n = 0;
                DataRow[] dtrows;
                if (Fieldlist.Count > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                foreach (DataRow row in dtrows)
                {
                    string columnName = row["ColumnName"].ToString();
                    string columnType = row["TypeName"].ToString();
                    string Length = row["Length"].ToString();
                    string IsIdentity = row["IsIdentity"].ToString();
                    string isPK = row["isPK"].ToString();
                    strclass1.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + columnName + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(_dbtype, columnType, Length) + "),");
                    strclass2.AppendSpaceLine(3, "parameters[" + n + "].Value = model." + columnName + ";");
                    n++;

                    if ((IsIdentity == "√") || (isPK == "√") || (_key == columnName))
                    {
                        continue;
                    }
                    strclass.AppendSpaceLine(3, "strSql.Append(\"" + columnName + "=" + preParameter + columnName + ",\");");                    
                }
            }

            //去掉最后的逗号			
            strclass.DelLastComma();
            strclass.AppendLine("\");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + _key + "=" + preParameter + _key + "\");");

            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass1.DelLastComma();
            strclass.Append(strclass1.Value);
            strclass.AppendLine("};");
            strclass.AppendLine(strclass2.Value);
            strclass.AppendSpaceLine(3, "" + DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }
        /// <summary>
        /// 得到Delete的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatDeleteParam()
        {
            StringPlus strclass = new StringPlus();

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 删除一条数据");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public void Delete(" + _keyType + " " + _key + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            if (_dbtype != "OleDb")
            {
                strclass.AppendSpaceLine(3, "strSql.Append(\"delete " + _tablename + " \");");
            }
            else
            {
                strclass.AppendSpaceLine(3, "strSql.Append(\"delete from " + _tablename + " \");");
            }
            strclass.AppendSpaceLine(3, "strSql.Append(\" where " + _key + "=" + preParameter + _key + "\");");

            strclass.AppendSpaceLine(3, "" + DbParaHead + "Parameter[] parameters = {");
            strclass.AppendSpaceLine(5, "new " + DbParaHead + "Parameter(\"" + preParameter + _key + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(_dbtype, _keyType, "") + ")");
            strclass.AppendSpaceLine(4, "};");
            strclass.AppendSpaceLine(3, "parameters[0].Value = " + _key + ";");

            strclass.AppendSpaceLine(3, "" + DbHelperName + ".ExecuteSql(strSql.ToString(),parameters);");
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
        public string CreatGetModelParam()
        {            
            DataTable dt = dbobj.GetColumnList(DbName, _tablename);
            if (ModelSpace == "")
            {
                ModelSpace = "ModelClassName"; ;
            }
            StringBuilder strclass = new StringBuilder();
            strclass.Append("\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// <summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// 得到一个对象实体" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "/// </summary>" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "public " + ModelSpace + " GetModel(" + _keyType + " " + _key + ")" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "{" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "StringBuilder strSql=new StringBuilder();" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\"select " + GetFieldslist(dt) + " from " + _tablename + " \");" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "strSql.Append(\" where " + _key + "=" + preParameter + _key + "\");" + "\r\n");


            strclass.Append(CodeCommon.Space(3) + "" + DbParaHead + "Parameter[] parameters = {" + "\r\n");
            strclass.Append(CodeCommon.Space(5) + "new " + DbParaHead + "Parameter(\"" + preParameter + _key + "\", " + DbParaDbType + "." + CodeCommon.DbTypeLength(_dbtype, _keyType, "") + ")");
            strclass.Append("};" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "parameters[0].Value = " + _key + ";" + "\r\n");

            strclass.Append(CodeCommon.Space(3) + "" + ModelSpace + " model=new " + ModelSpace + "();" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "DataSet ds=" + DbHelperName + ".Query(strSql.ToString(),parameters);" + "\r\n");
            //strclass.Append(CodeCommon.Space(3) + "model." + _key + "=" + _key + ";" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "if(ds.Tables[0].Rows.Count>0)" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "{" + "\r\n");
            if (dt != null)
            {
                DataRow[] dtrows;
                if (Fieldlist.Count > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                foreach (DataRow row in dtrows)
                {
                    string columnName = row["ColumnName"].ToString(); //dbReader.GetString(0);
                    string columnType = row["TypeName"].ToString();//dbReader.GetString(1);
                    //if (_key == columnName)
                    //{
                    //    continue;
                    //}
                    switch (CodeCommon.DbTypeToCS(columnType))
                    {
                        case "int":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=int.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        case "decimal":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=decimal.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        case "DateTime":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=DateTime.Parse(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        case "string":
                            {
                                strclass.Append(CodeCommon.Space(4) + "model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();" + "\r\n");
                            }
                            break;
                        case "bool":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "if((ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()==\"1\")||(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString().ToLower()==\"true\"))" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(6) + "model." + columnName + "=true;" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "}" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "else" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(6) + "model." + columnName + "=false;" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "}" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n" + "\r\n");
                            }
                            break;
                        case "byte[]":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=(byte[])ds.Tables[0].Rows[0][\"" + columnName + "\"];" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        case "Guid":
                            {
                                strclass.Append(CodeCommon.Space(4) + "if(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString()!=\"\")" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "{" + "\r\n");
                                strclass.Append(CodeCommon.Space(5) + "model." + columnName + "=new Guid(ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString());" + "\r\n");
                                strclass.Append(CodeCommon.Space(4) + "}" + "\r\n");
                            }
                            break;
                        default:
                            strclass.Append(CodeCommon.Space(4) + "model." + columnName + "=ds.Tables[0].Rows[0][\"" + columnName + "\"].ToString();" + "\r\n");
                            break;

                    }

                }
            }
            strclass.Append(CodeCommon.Space(4) + "return model;" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "}" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "else" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "{" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "return null;" + "\r\n");
            strclass.Append(CodeCommon.Space(3) + "}" + "\r\n");
            strclass.Append(CodeCommon.Space(2) + "}");
            return strclass.ToString();
        }

        /// <summary>
        /// 得到GetList()的代码
        /// </summary>
        /// <param name="_tablename"></param>
        /// <param name="_key"></param>
        /// <returns></returns>
        public string CreatGetListParam()
        {
            DataTable dt = dbobj.GetColumnList(DbName, TableName);
            StringPlus strclass = new StringPlus();
            //string strfields = GetFieldslist(dt);
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 获得数据列表");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "StringBuilder strSql=new StringBuilder();");
            strclass.AppendSpace(3, "strSql.Append(\"select ");
            strclass.AppendLine(GetFieldslist(dt) + " \");");
            strclass.AppendSpaceLine(3, "strSql.Append(\" FROM " + TableName + " \");");
            strclass.AppendSpaceLine(3, "if(strWhere.Trim()!=\"\")");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "strSql.Append(\" where \"+strWhere);");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return " + DbHelperName + ".Query(strSql.ToString());");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
        }



        #endregion
    }
}
