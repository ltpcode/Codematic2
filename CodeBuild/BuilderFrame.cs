using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Maticsoft.IDBO;
using Maticsoft.Utility;
using Maticsoft.CodeHelper;
namespace Maticsoft.CodeBuild
{
    /// <summary>
    /// 框架代码基类
    /// </summary>
    public class BuilderFrame
    {
        #region  私有变量
        protected IDbObject dbobj;
        protected string _dbtype;     
        protected string _key = "ID";//默认第一个主键字段		
        protected string _keyType = "int";//默认第一个主键类型

        #endregion

        #region  公有属性
        private string _dbname;
        private string _tablename;
        private string _tabledescription = "";
        private string _modelname;//model类名 
        private string _bllname;//bll类名    
        private string _dalname;//dal类名    
        private string _namespace = "Maticsoft";//顶级命名空间名
        private string _folder;//所在文件夹
        private string _dbhelperName;//数据库访问类名
        private List<ColumnInfo> _keys; //主键或条件字段列表
        private List<ColumnInfo> _fieldlist;

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
        /// 表的描述信息
        /// </summary>
        public string TableDescription
        {
            set { _tabledescription = value; }
            get { return _tabledescription; }
        }
        public List<ColumnInfo> Keys
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


        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        public string BLLName
        {
            set { _bllname = value; }
            get { return _bllname; }
        }
        public string DALName
        {
            set { _dalname = value; }
            get { return _dalname; }
        }  
       
        
        public string DbHelperName
        {
            set { _dbhelperName = value; }
            get { return _dbhelperName; }
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
        /// 选择的字段集合字符串
        /// </summary>
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

        #endregion

        #region 构造属性
        private string _modelpath;        
        private string _dalpath;           
        private string _idalpath;       
        private string _bllpath;        
        //private string _factoryclass;

        /// <summary>
        /// 实体类的命名空间
        /// </summary>
        public string Modelpath
        {
            set { _modelpath = value; }
            get
            {
                _modelpath = _namespace + "." + "Model";
                if (_folder.Trim() != "")
                {
                    _modelpath += "." + _folder;
                }
                return _modelpath;
            }
        }
        /// <summary>
        /// 实体类的整个命名空间+类名
        /// </summary>
        public string ModelSpace
        {            
            get
            {
                return Modelpath + "." + ModelName; 
            }
        }
        /// <summary>
        /// 数据层的命名空间
        /// </summary>
        public string DALpath
        {            
            get
            {
                string strdbtype = _dbtype;
                if (_dbtype == "SQL2000" || _dbtype == "SQL2005"
                    || _dbtype == "SQL2008" || _dbtype == "SQL2012")
                {
                    strdbtype = "SQLServer";
                }
                _dalpath = _namespace + "." + strdbtype + "DAL";
                if (_folder.Trim() != "")
                {
                    _dalpath += "." + _folder;
                }
                return _dalpath;
            }
            set { _dalpath = value; }
        }        
        /// <summary>
        /// 数据层的操作类名称定义
        /// </summary>
        public string DALSpace
        {            
            get
            {
                return DALpath + "." + DALName;
            }
        }    

        /// <summary>
        /// 接口的命名空间
        /// </summary>
        public string IDALpath
        {
            get
            {
                _idalpath = _namespace + "." + "IDAL";
                if (_folder.Trim() != "")
                {
                    _idalpath += "." + _folder;
                }
                return _idalpath;
            }
        }
        /// <summary>
        /// 接口名
        /// </summary>
        public string IClass
        {
            get
            {
                return "I" + DALName;
            }
        }
        /// <summary>
        /// 业务逻辑层的操作类名称定义
        /// </summary>
        public string BLLpath
        {
            set { _bllpath = value; }
            get
            {
                string bllpath = _namespace + "." + "BLL";
                if (_folder.Trim() != "")
                {
                    bllpath += "." + _folder;
                }
                return bllpath;
            }
        }
        /// <summary>
        /// 业务逻辑层的操作类名称定义
        /// </summary>
        public string BLLSpace
        {
            get
            {
                return BLLpath + "." + BLLName;
            }
        }
        /// <summary>
        /// 工厂类的命名空间
        /// </summary>
        public string Factorypath
        {
            get
            {
                string factorypath = _namespace + "." + "DALFactory";
                //if (_folder.Trim() != "")
                //{
                //    factorypath += "." + _folder;
                //}
                return factorypath;
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
                    default:
                        return "SqlDbType";
                }
            }
        }
        /// <summary>
        /// 字段的 select 列表
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
        /// 列中是否有标识列
        /// </summary>
        public bool IsHasIdentity
        {
            get
            {
                bool isid = false;
                foreach (ColumnInfo key in Keys)
                {
                    _key = key.ColumnName;
                    _keyType = key.TypeName;
                    if (key.IsIdentity)
                    {
                        _key = key.ColumnName;
                        _keyType = CodeCommon.DbTypeToCS(key.TypeName);
                        isid = true;
                        break;
                    }
                }
                return isid;
            }
        }
        /// <summary>
        /// 主键标识字段
        /// </summary>
        public string Key
        {
            get
            {
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
                return _key;
            }
        }
        #endregion	

        /// <summary>
        /// 得到主键条件字段参数定义列表
        /// </summary>
        /// <param name="Keys"></param>
        /// <returns></returns>
        public string GetkeyParalist(Hashtable Keys)
        {
            StringPlus strlist = new StringPlus();
            foreach (DictionaryEntry key in Keys)
            {
                strlist.Append(CodeCommon.DbTypeToCS(key.Value.ToString()) + " " + key.Key.ToString() + ",");
            }
            if (strlist.Value.IndexOf(",") > 0)
            {
                strlist.DelLastComma();
            }            
            return strlist.Value;
        }
        
        /// <summary>
        /// 得到主键的条件表达式
        /// </summary>
        /// <param name="Keys"></param>
        /// <returns></returns>
        public string GetkeyWherelist(Hashtable Keys)
        {
            StringPlus strlist = new StringPlus();
            int n = 0;
            foreach (DictionaryEntry key in Keys)
            {
                n++;
                if (CodeCommon.IsAddMark(key.Value.ToString()))
                {
                    strlist.Append(key.Key.ToString() + "='\"+" + key.Key.ToString() + "+\"'\"");
                }
                else
                {
                    strlist.Append(key.Key.ToString() + "=\"+" + key.Key.ToString() + "+\"");
                    if (n == Keys.Count)
                    {
                        strlist.Append("\"");
                    }
                }                
                strlist.Append(" and ");
            }

            if (strlist.Value.IndexOf("and") > 0)
            {
                strlist.DelLastChar("and");
            }            
            return strlist.Value;
        }
        
        /// <summary>
        /// 得到主键的条件表达式(存储过程参数 )
        /// </summary>
        /// <param name="Keys"></param>
        /// <returns></returns>
        public string GetkeyWherelistProc(Hashtable Keys)
        {
            StringPlus strlist = new StringPlus();
            foreach (DictionaryEntry key in Keys)
            {               
                strlist.Append(key.Key.ToString() + "=@" + key.Key.ToString() );               
                strlist.Append(" and ");
            }
            if (strlist.Value.IndexOf("and") > 0)
            {
                strlist.DelLastChar("and");
            }
            return strlist.Value;
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
                strclass.Append("[" + columnName + "],");
            }
            strclass.DelLastComma();
            return strclass.Value;
        }

        public string Space(int num)
        {
            StringBuilder str = new StringBuilder();
            for (int n = 0; n < num; n++)
            {
                str.Append("\t");
            }
            return str.ToString();
        }
    }
}
