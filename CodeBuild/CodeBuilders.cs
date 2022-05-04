using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.DBFactory;
using Maticsoft.CodeHelper;
namespace Maticsoft.CodeBuild
{
    /// <summary>
    /// C# 代码生成类。
    /// </summary>
    public class CodeBuilders
    {
        #region  私有变量
        IDbObject dbobj;
        IBuilder.IBuilderWeb ibw;
        #endregion

        #region  公有属性

        private string _dbtype;
        private string _dbconnectStr;
        private string _dbname;
        private string _tablename;
        private string _tabledescription = "";
        private string _modelname;//model类名    
        private string _bllname;//bll类名    
        private string _dalname;//dal类名    
        private string _namespace = "Maticsoft";//顶级命名空间名
        private string _folder;//所在文件夹
        private string _dbhelperName = "DbHelperSQL";//数据库访问类名
        private List<ColumnInfo> _keys; //主键或条件字段列表
        private List<ColumnInfo> _fieldlist;
        private string _procprefix;

        public string DbType
        {
            set { _dbtype = value; }
            get { return _dbtype; }
        }
        public string DbConnectStr
        {
            set { _dbconnectStr = value; }
            get { return _dbconnectStr; }
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
        /// 表的描述信息
        /// </summary>
        public string TableDescription
        {
            set { _tabledescription = value; }
            get { return _tabledescription; }
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
        public List<ColumnInfo> Keys
        {
            set { _keys = value; }
            get { return _keys; }
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

        private string _modelpath;
        private string _dalpath;
        private string _idalpath;
        private string _bllpath;
        private string _factoryclass;


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
            set { _dalpath = value; }
            get
            {
                string dalpath = _namespace + "." + _dbtype + "DAL";
                if (_folder.Trim() != "")
                {
                    dalpath += "." + _folder;
                }
                return dalpath;
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
                if (_folder.Trim() != "")
                {
                    factorypath += "." + _folder;
                }
                return factorypath;
            }
        }
        /// <summary>
        /// 工厂类的名称
        /// </summary>
        public string FactoryClass
        {
            get
            {
                _factoryclass = _namespace + "." + "DALFactory";
                if (_folder.Trim() != "")
                {
                    _factoryclass += "." + _folder;
                }
                _factoryclass += "." + _modelname;
                return _factoryclass;
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
        }
        #endregion


        public CodeBuilders(IDbObject idbobj)
        {
            dbobj = idbobj;
            DbType = idbobj.DbType;
            if (_dbhelperName == "")
            {
                switch (DbType)
                {
                    case "SQL2000":
                    case "SQL2005":
                    case "SQL2008":
                    case "SQL2012":
                        _dbhelperName = "DbHelperSQL";
                        break;
                    case "Oracle":
                        _dbhelperName = "DbHelperOra";
                        break;
                    case "MySQL":
                        _dbhelperName = "DbHelperMySQL";
                        break;
                    case "OleDb":
                        _dbhelperName = "DbHelperOleDb";
                        break;
                }
            }
        }

        #region  生成单类结构代码
        /// <summary>
        /// 生成单类结构代码
        /// </summary>     
        public string GetCodeFrameOne(string DALtype, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            BuilderFrameOne cfo = new BuilderFrameOne(dbobj, DbName, TableName, ModelName, Fieldlist, Keys, NameSpace, Folder, DbHelperName);
            string strcode = cfo.GetCode(DALtype, Maxid, Exists, Add, Update, Delete, GetModel, List, ProcPrefix);
            return strcode;
        }
        #endregion

        #region 生成简单三层结构代码

        /// <summary>
        /// 生成简单三层结构代码——Model
        /// </summary>
        /// <returns></returns>
        public string GetCodeFrameS3Model()
        {
            BuilderFrameS3 s3 = new BuilderFrameS3(dbobj, DbName, TableName, TableDescription, ModelName, BLLName, DALName, Fieldlist, Keys, NameSpace, Folder, DbHelperName);
            string strcode = s3.GetModelCode();
            return strcode;
        }

        /// <summary>
        /// 生成简单三层结构代码——DAL数据访问层
        /// </summary>        
        public string GetCodeFrameS3DAL(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            BuilderFrameS3 s3 = new BuilderFrameS3(dbobj, DbName, TableName, TableDescription, ModelName, BLLName, DALName, Fieldlist, Keys, NameSpace, Folder, DbHelperName);
            return s3.GetDALCode(AssemblyGuid, Maxid, Exists, Add, Update, Delete, GetModel, List, ProcPrefix);
        }

        /// <summary>
        /// 生成简单三层结构代码——BLL数据访问层
        /// </summary>     
        public string GetCodeFrameS3BLL(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List)
        {
            BuilderFrameS3 s3 = new BuilderFrameS3(dbobj, DbName, TableName, TableDescription, ModelName, BLLName, DALName, Fieldlist, Keys, NameSpace, Folder, DbHelperName);
            string strcode = s3.GetBLLCode(AssemblyGuid, Maxid, Exists, Add, Update, Delete, GetModel, GetModelByCache, List);
            return strcode;
        }

        #endregion

        #region 工厂模式三层
        /// <summary>
        /// 生成工厂模式三层结构代码——Model
        /// </summary>
        /// <returns></returns>
        public string GetCodeFrameF3Model()
        {
            BuilderFrameF3 f3 = new BuilderFrameF3(dbobj, DbName, TableName, TableDescription, ModelName, BLLName, DALName, Fieldlist, Keys, NameSpace, Folder, DbHelperName);
            string strcode = f3.GetModelCode();
            return strcode;
        }

        /// <summary>
        /// 生成工厂模式三层结构代码——DAL数据访问层
        /// </summary>        
        public string GetCodeFrameF3DAL(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List)
        {
            BuilderFrameF3 f3 = new BuilderFrameF3(dbobj, DbName, TableName, TableDescription, ModelName, BLLName, DALName, Fieldlist, Keys, NameSpace, Folder, DbHelperName);
            return f3.GetDALCode(AssemblyGuid, Maxid, Exists, Add, Update, Delete, GetModel, List, ProcPrefix);
        }

        /// <summary>
        /// 生成工厂模式三层结构代码——IDAL
        /// </summary>
        /// <returns></returns>
        public string GetCodeFrameF3IDAL(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List, bool ListProc)
        {
            BuilderFrameF3 f3 = new BuilderFrameF3(dbobj, DbName, TableName, TableDescription, ModelName, BLLName, DALName, Fieldlist, Keys, NameSpace, Folder, DbHelperName);
            string strcode = f3.GetIDALCode(Maxid, Exists, Add, Update, Delete, GetModel, List, ListProc);
            return strcode;
        }

        /// <summary>
        /// 生成工厂模式三层结构代码——DALFactory
        /// </summary>
        /// <returns></returns>
        public string GetCodeFrameF3DALFactory()
        {
            BuilderFrameF3 f3 = new BuilderFrameF3(dbobj, DbName, TableName, TableDescription, ModelName, BLLName, DALName, Fieldlist, Keys, NameSpace, Folder, DbHelperName);
            string strcode = f3.GetDALFactoryCode();
            return strcode;
        }
        /// <summary>
        /// 生成工厂模式三层结构代码——DALFactory中的方法代码
        /// </summary>
        /// <returns></returns>
        public string GetCodeFrameF3DALFactoryMethod()
        {
            BuilderFrameF3 f3 = new BuilderFrameF3(dbobj, DbName, TableName, TableDescription, ModelName, BLLName, DALName, Fieldlist, Keys, NameSpace, Folder, DbHelperName);
            string strcode = f3.GetDALFactoryMethodCode();
            return strcode;
        }

        /// <summary>
        /// 生成工厂模式三层结构代码——BLL数据访问层
        /// </summary>     
        public string GetCodeFrameF3BLL(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List)
        {
            BuilderFrameF3 f3 = new BuilderFrameF3(dbobj, DbName, TableName, TableDescription, ModelName, BLLName, DALName, Fieldlist, Keys, NameSpace, Folder, DbHelperName);
            string strcode = f3.GetBLLCode(AssemblyGuid, Maxid, Exists, Add, Update, Delete, GetModel, GetModelByCache, List, List);
            return strcode;
        }

        #endregion

        #region 生成web
        /// <summary>
        /// 构建BuilderWeb接口对象
        /// </summary>
        /// <param name="AssemblyGuid"></param>
        /// <returns></returns>
        public IBuilder.IBuilderWeb CreatBuilderWeb(string AssemblyGuid)
        {
            ibw = BuilderFactory.CreateWebObj(AssemblyGuid);
            ibw.NameSpace = NameSpace;
            ibw.Fieldlist = Fieldlist;
            ibw.Keys = Keys;
            ibw.ModelName = ModelName;
            ibw.Folder = Folder;
            ibw.BLLName = BLLName;
            return ibw;
        }

        // Add---------------------------------------------------------------
        public string GetAddAspx()
        {
            //IBuilder.IBuilderWeb bw = CreatBuilderWeb();
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            return ibw.GetAddAspx();
        }
        public string GetAddAspxCs()
        {
            //IBuilder.IBuilderWeb bw = CreatBuilderWeb();
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            string cs = ibw.GetAddAspxCs();
            StringPlus strcode = new StringPlus();
            //strcode.AppendSpaceLine(2, "protected void Page_LoadComplete(object sender, EventArgs e)");
            //strcode.AppendSpaceLine(2, "{");
            //strcode.AppendSpaceLine(3, "(Master.FindControl(\"lblTitle\") as Label).Text = \"信息添加\";");
            //strcode.AppendSpaceLine(2, "}");
            strcode.AppendSpaceLine(2, "protected void btnSave_Click(object sender, EventArgs e)");
            strcode.AppendSpaceLine(2, "{");
            strcode.AppendSpaceLine(3, cs);
            strcode.AppendSpaceLine(2, "}");
            return strcode.ToString();

        }
        public string GetAddDesigner()
        {            
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            return ibw.GetAddDesigner();
        }

        // Update--------------------------------------------------------------

        public string GetUpdateAspx()
        {            
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            return ibw.GetUpdateAspx();
        }
        public string GetUpdateAspxCs()
        {            
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            string cs = ibw.GetUpdateAspxCs();
            string cs2 = ibw.GetUpdateShowAspxCs();
            StringPlus strcode = new StringPlus();
            
            strcode.AppendSpaceLine(2, "protected void Page_Load(object sender, EventArgs e)");
            strcode.AppendSpaceLine(2, "{");
            strcode.AppendSpaceLine(3, "if (!Page.IsPostBack)");
            strcode.AppendSpaceLine(3, "{");
                        
            //主键传递字段，处理多主键的情况
            string keyField = "ID";            
            if (_keys.Count == 1)
            {
                #region 
                keyField = _keys[0].ColumnName;
                //keyFieldParams = "id={0}";
                strcode.AppendSpaceLine(4, "if (Request.Params[\"id\"] != null && Request.Params[\"id\"].Trim() != \"\")");
                strcode.AppendSpaceLine(4, "{");
                switch (Maticsoft.CodeHelper.CodeCommon.DbTypeToCS(_keys[0].TypeName).ToLower())
                {
                    case "int":
                        strcode.AppendSpaceLine(5, "int " + keyField + "=(Convert.ToInt32(Request.Params[\"id\"]));");
                        break;
                    case "long":
                        strcode.AppendSpaceLine(5, "long " + keyField + "=(Convert.ToInt64(Request.Params[\"id\"]));");
                        break;
                    case "decimal":
                        strcode.AppendSpaceLine(5, "decimal " + keyField + "=(Convert.ToDecimal(Request.Params[\"id\"]));");
                        break;
                    case "bool":                        
                        strcode.AppendSpaceLine(5, "bool " + keyField + "=(Convert.ToBoolean(Request.Params[\"id\"]));");
                        break;
                    case "guid":      
                        strcode.AppendSpaceLine(5, "Guid " + keyField + "=new Guid(Request.Params[\"id\"]);");
                        break;
                    default:
                        strcode.AppendSpaceLine(5, "string " + keyField + "= Request.Params[\"id\"];");
                        break;
                }
                strcode.AppendSpaceLine(5, "ShowInfo(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys,true) + ");");
                strcode.AppendSpaceLine(4, "}");
                #endregion
            }
            else
            {
                ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(_keys);                
                //有标识字段
                if (field != null)
                {
                    keyField = field.ColumnName;
                    strcode.AppendSpaceLine(4, "if (Request.Params[\"id\"] != null && Request.Params[\"id\"].Trim() != \"\")");
                    strcode.AppendSpaceLine(4, "{");
                    switch (Maticsoft.CodeHelper.CodeCommon.DbTypeToCS(field.TypeName).ToLower())
                    {
                        case "int":
                            strcode.AppendSpaceLine(5, "int " + keyField + "=(Convert.ToInt32(Request.Params[\"id\"]));");
                            break;
                        case "long":
                            strcode.AppendSpaceLine(5, "long " + keyField + "=(Convert.ToInt64(Request.Params[\"id\"]));");
                            break;
                        case "decimal":
                            strcode.AppendSpaceLine(5, "decimal " + keyField + "=(Convert.ToDecimal(Request.Params[\"id\"]));");
                            break;
                        case "guid":
                            strcode.AppendSpaceLine(5, "Guid " + keyField + "=new Guid(Request.Params[\"id\"]);");
                            break;
                        default:
                            strcode.AppendSpaceLine(5, "string " + keyField + "= Request.Params[\"id\"];");
                            break;
                    }
                    strcode.AppendSpaceLine(5, "ShowInfo(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
                    strcode.AppendSpaceLine(4, "}");                    
                }
                else  //无标识列多字段
                {
                    #region 
                    for (int n = 0; n < _keys.Count; n++)
                    {                        
                        //多个主键循环
                        keyField = _keys[n].ColumnName;
                        string keyCStype = Maticsoft.CodeHelper.CodeCommon.DbTypeToCS(_keys[n].TypeName);
                        switch (keyCStype.ToLower())
                        {
                            case "int":
                            case "long":
                            case "decimal":
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + " = -1;");
                                break;
                            case "bool":
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + " = false;");
                                break;
                            case "guid":
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + ";");
                                break;
                            default:
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + " = \"\";");
                                break;
                        }
                        strcode.AppendSpaceLine(4, "if (Request.Params[\"id" + n.ToString() + "\"] != null && Request.Params[\"id" + n.ToString() + "\"].Trim() != \"\")");
                        strcode.AppendSpaceLine(4, "{");
                        switch (keyCStype)
                        {
                            case "int":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToInt32(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "long":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToInt64(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "decimal":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToDecimal(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "bool":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToBoolean(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "guid":
                                strcode.AppendSpaceLine(5, keyField + "=new Guid(Request.Params[\"id" + n.ToString() + "\"]);");
                                break;
                            default:
                                strcode.AppendSpaceLine(5, keyField + "= Request.Params[\"id" + n.ToString() + "\"];");
                                break;
                        }
                        strcode.AppendSpaceLine(4, "}");

                    }//循环结束
                    strcode.AppendSpaceLine(4, "#warning 代码生成提示：显示页面,请检查确认该语句是否正确");
                    strcode.AppendSpaceLine(4, "ShowInfo(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
                    #endregion

                }                
            }            
            strcode.AppendSpaceLine(3, "}");
            strcode.AppendSpaceLine(2, "}");
            strcode.AppendSpaceLine(3, cs2);
            strcode.AppendSpaceLine(2, "public void btnSave_Click(object sender, EventArgs e)");
            strcode.AppendSpaceLine(2, "{");
            strcode.AppendSpaceLine(3, cs);
            strcode.AppendSpaceLine(2, "}");
            return strcode.ToString();
        }

        public string GetUpdateDesigner()
        {
            //IBuilder.IBuilderWeb bw = CreatBuilderWeb();
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            return ibw.GetUpdateDesigner();
        }
        /// <summary>
        /// 得到修改窗体的代码
        /// </summary>       
        public string GetUpdateShowAspxCs()
        {
            return ibw.GetUpdateShowAspxCs();
        }

        //Show --------------------------------------------------------------
        public string GetShowAspx()
        {
            //Maticsoft.BuilderWeb.BuilderWeb bw = CreatBuilderWeb();
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            return ibw.GetShowAspx();
        }
        public string GetShowAspxCs()
        {            
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            string cs = ibw.GetShowAspxCs();
            StringPlus strcode = new StringPlus();
            strcode.AppendSpaceLine(2, "public string strid=\"\"; ");
            strcode.AppendSpaceLine(2, "protected void Page_Load(object sender, EventArgs e)");
            strcode.AppendSpaceLine(2, "{");
            strcode.AppendSpaceLine(3, "if (!Page.IsPostBack)");
            strcode.AppendSpaceLine(3, "{");

            //主键传递字段，处理多主键的情况
            string keyField = "ID";            
            if (_keys.Count == 1)
            {
                #region  只有一个主键
                keyField = _keys[0].ColumnName;                
                strcode.AppendSpaceLine(4, "if (Request.Params[\"id\"] != null && Request.Params[\"id\"].Trim() != \"\")");
                strcode.AppendSpaceLine(4, "{");
                strcode.AppendSpaceLine(5, "strid = Request.Params[\"id\"];");//页面的公共变量
                switch (Maticsoft.CodeHelper.CodeCommon.DbTypeToCS(_keys[0].TypeName).ToLower())
                {
                    case "int":
                        strcode.AppendSpaceLine(5, "int " + keyField + "=(Convert.ToInt32(strid));");                        
                        break;
                    case "long":
                        strcode.AppendSpaceLine(5, "long " + keyField + "=(Convert.ToInt64(strid));");
                        break;
                    case "decimal":
                        strcode.AppendSpaceLine(5, "decimal " + keyField + "=(Convert.ToDecimal(strid));");
                        break;
                    case "bool":
                        strcode.AppendSpaceLine(5, "bool " + keyField + "=(Convert.ToBoolean(strid));");
                        break;
                    case "guid":
                        strcode.AppendSpaceLine(5, "Guid " + keyField + "=new Guid(strid);");
                        break;
                    default:
                        strcode.AppendSpaceLine(5, "string " + keyField + "= strid;");
                        break;
                }
                strcode.AppendSpaceLine(5, "ShowInfo(" + keyField + ");");
                strcode.AppendSpaceLine(4, "}");
                #endregion
            }
            else //有多个主键（或ID）
            {
                ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(_keys);
                //有标识字段
                if (field != null)
                {
                    #region 有标识列
                    keyField = field.ColumnName;
                    strcode.AppendSpaceLine(4, "if (Request.Params[\"id\"] != null && Request.Params[\"id\"].Trim() != \"\")");
                    strcode.AppendSpaceLine(4, "{");
                    strcode.AppendSpaceLine(5, "strid = Request.Params[\"id\"];");//页面的公共变量
                    switch (Maticsoft.CodeHelper.CodeCommon.DbTypeToCS(field.TypeName).ToLower())
                    {
                        case "int":
                            strcode.AppendSpaceLine(5, "int " + keyField + "=(Convert.ToInt32(strid));");
                            break;
                        case "long":
                            strcode.AppendSpaceLine(5, "long " + keyField + "=(Convert.ToInt64(strid));");
                            break;
                        case "decimal":
                            strcode.AppendSpaceLine(5, "decimal " + keyField + "=(Convert.ToDecimal(strid));");
                            break;
                        case "bool":
                            strcode.AppendSpaceLine(5, "bool " + keyField + "=(Convert.ToBoolean(strid));");
                            break;
                        case "guid":
                            strcode.AppendSpaceLine(5, "Guid " + keyField + "=new Guid(strid);");
                            break;
                        default:
                            strcode.AppendSpaceLine(5, "string " + keyField + "= strid;");
                            break;
                    }
                    strcode.AppendSpaceLine(5, "ShowInfo(" + keyField + ");");
                    strcode.AppendSpaceLine(4, "}");
                    #endregion
                }
                else
                {                   
                    for (int n = 0; n < _keys.Count; n++)
                    {                        
                        //多个主键循环
                        keyField = _keys[n].ColumnName;
                        string keyCStype = Maticsoft.CodeHelper.CodeCommon.DbTypeToCS(_keys[n].TypeName);
                        switch (keyCStype.ToLower())
                        {
                            case "int":
                            case "long":
                            case "decimal":
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + " = -1;");
                                break;
                            case "bool":
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + " = false;");
                                break;
                            case "guid":
                            case "Guid":
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + " ;");
                                break;
                            default:
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + " = \"\";");
                                break;
                        }
                        strcode.AppendSpaceLine(4, "if (Request.Params[\"id" + n.ToString() + "\"] != null && Request.Params[\"id" + n.ToString() + "\"].Trim() != \"\")");
                        strcode.AppendSpaceLine(4, "{");
                        switch (keyCStype.ToLower())
                        {
                            case "int":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToInt32(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "long":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToInt64(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "decimal":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToDecimal(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "bool":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToBoolean(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "guid":
                                strcode.AppendSpaceLine(5, keyField + "=new Guid(Request.Params[\"id\"]);");
                                break;
                            default:
                                strcode.AppendSpaceLine(5, keyField + "= Request.Params[\"id" + n.ToString() + "\"];");
                                break;
                        }
                        strcode.AppendSpaceLine(4, "}");

                    }//循环结束
                    strcode.AppendSpaceLine(4, "#warning 代码生成提示：显示页面,请检查确认该语句是否正确");
                    strcode.AppendSpaceLine(4, "ShowInfo(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
                }                                               
            }            
            strcode.AppendSpaceLine(3, "}");
            strcode.AppendSpaceLine(2, "}");
            strcode.AppendSpaceLine(2, cs);
            return strcode.ToString();
        }
        public string GetShowDesigner()
        {
            //Maticsoft.BuilderWeb.BuilderWeb bw = CreatBuilderWeb();
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            return ibw.GetShowDesigner();
        }

        //Delete---------------------
        public string GetDeleteAspxCs()
        {
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
                        
            StringPlus strcode = new StringPlus();            
            strcode.AppendSpaceLine(3, "if (!Page.IsPostBack)");
            strcode.AppendSpaceLine(3, "{");
            strcode.AppendSpaceLine(4, BLLSpace + " bll=new " + BLLSpace + "();");
            
            //主键传递字段，处理多主键的情况
            string keyField = "ID";
            if (_keys.Count == 1)
            {
                keyField = _keys[0].ColumnName;

                strcode.AppendSpaceLine(4, "if (Request.Params[\"id\"] != null && Request.Params[\"id\"].Trim() != \"\")");
                strcode.AppendSpaceLine(4, "{");
                switch (Maticsoft.CodeHelper.CodeCommon.DbTypeToCS(_keys[0].TypeName).ToLower())
                {
                    case "int":
                        strcode.AppendSpaceLine(5, "int " + keyField + "=(Convert.ToInt32(Request.Params[\"id\"]));");
                        break;
                    case "long":
                        strcode.AppendSpaceLine(5, "long " + keyField + "=(Convert.ToInt64(Request.Params[\"id\"]));");
                        break;
                    case "decimal":
                        strcode.AppendSpaceLine(5, "decimal " + keyField + "=(Convert.ToDecimal(Request.Params[\"id\"]));");
                        break;
                    case "bool":
                        strcode.AppendSpaceLine(5, "bool " + keyField + "=(Convert.ToBoolean(Request.Params[\"id\"]));");
                        break;
                    case "guid":
                        strcode.AppendSpaceLine(5, "Guid " + keyField + "=new Guid(Request.Params[\"id\"]);");
                        break;
                    default:
                        strcode.AppendSpaceLine(5, "string " + keyField + "= Request.Params[\"id\"];");
                        break;
                }
                strcode.AppendSpaceLine(5, "bll.Delete(" + keyField + ");");
                strcode.AppendSpaceLine(5, "Response.Redirect(\"list.aspx\");");
                strcode.AppendSpaceLine(4, "}");
            }
            else //多个主键
            {
                ColumnInfo field = Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(_keys);
                //有标识字段
                if (field != null)
                {
                    keyField = field.ColumnName;
                    strcode.AppendSpaceLine(4, "if (Request.Params[\"id\"] != null && Request.Params[\"id\"].Trim() != \"\")");
                    strcode.AppendSpaceLine(4, "{");
                    switch (Maticsoft.CodeHelper.CodeCommon.DbTypeToCS(field.TypeName).ToLower())
                    {
                        case "int":
                            strcode.AppendSpaceLine(5, "int " + keyField + "=(Convert.ToInt32(Request.Params[\"id\"]));");
                            break;
                        case "long":
                            strcode.AppendSpaceLine(5, "long " + keyField + "=(Convert.ToInt64(Request.Params[\"id\"]));");
                            break;
                        case "decimal":
                            strcode.AppendSpaceLine(5, "decimal " + keyField + "=(Convert.ToDecimal(Request.Params[\"id\"]));");
                            break;
                        case "bool":
                            strcode.AppendSpaceLine(5, "bool " + keyField + "=(Convert.ToBoolean(Request.Params[\"id\"]));");
                            break;
                        case "guid":
                            strcode.AppendSpaceLine(5, "Guid " + keyField + "=new Guid(Request.Params[\"id\"]);");
                            break;
                        default:
                            strcode.AppendSpaceLine(5, "string " + keyField + "= Request.Params[\"id\"];");
                            break;
                    }
                    strcode.AppendSpaceLine(4, "bll.Delete(" + keyField + ");");
                    strcode.AppendSpaceLine(4, "}");                    
                }
                else
                {
                    for (int n = 0; n < _keys.Count; n++)
                    {
                        //多个主键循环
                        keyField = _keys[n].ColumnName;                        
                        string keyCStype = Maticsoft.CodeHelper.CodeCommon.DbTypeToCS(_keys[n].TypeName);
                        switch (keyCStype)
                        {
                            case "int":
                            case "long":
                            case "decimal":
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + " = -1;");
                                break;
                            case "bool":
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + " = false;");
                                break;
                            case "guid":
                            case "Guid":
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + " ;");
                                break;
                            default:
                                strcode.AppendSpaceLine(4, keyCStype + " " + keyField + " = \"\";");
                                break;
                        }
                        strcode.AppendSpaceLine(4, "if (Request.Params[\"id" + n.ToString() + "\"] != null && Request.Params[\"id" + n.ToString() + "\"].Trim() != \"\")");
                        strcode.AppendSpaceLine(4, "{");
                        switch (keyCStype.ToLower())
                        {
                            case "int":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToInt32(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "long":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToInt64(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "decimal":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToDecimal(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "bool":
                                strcode.AppendSpaceLine(5, keyField + "=(Convert.ToBoolean(Request.Params[\"id" + n.ToString() + "\"]));");
                                break;
                            case "guid":
                                strcode.AppendSpaceLine(5, keyField + "=new Guid(Request.Params[\"id\"]);");
                                break;
                            default:
                                strcode.AppendSpaceLine(5, keyField + "= Request.Params[\"id" + n.ToString() + "\"];");
                                break;
                        }
                        strcode.AppendSpaceLine(4, "}");

                    }//循环结束
                    strcode.AppendSpaceLine(4, "#warning 代码生成提示：删除页面,请检查确认传递过来的参数是否正确");
                    strcode.AppendSpaceLine(4, "// bll.Delete(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
                } 
            }

            strcode.AppendSpaceLine(3, "}");            
            return strcode.ToString();
            
        }


        //List --------------------------------------------------------------
        public string GetListAspx()
        {
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            return ibw.GetListAspx();
        }
        public string GetListAspxCs()
        {
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            string cs = ibw.GetListAspxCs();
            return cs;
        }
        public string GetListDesigner()
        {
            if (ibw == null)
            {
                return "//请选择有效的表示层代码组件！";
            }
            return ibw.GetListDesigner();
        }

        /// <summary>
        /// 增删改3个页面代码
        /// </summary>      
        public string GetWebHtmlCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm)
        {
            StringPlus strclass = new StringPlus();
            if (AddForm)
            {
                strclass.AppendLine(" <!--******************************增加页面代码********************************-->");
                strclass.AppendLine(GetAddAspx());
            }
            if (UpdateForm)
            {
                strclass.AppendLine(" <!--******************************修改页面代码********************************-->");
                strclass.AppendLine(GetUpdateAspx());
            }
            if (ShowForm)
            {
                strclass.AppendLine("  <!--******************************显示页面代码********************************-->");
                strclass.AppendLine(GetShowAspx());
            }

            if (SearchForm)
            {
                strclass.AppendLine("  <!--******************************列表页面代码********************************-->");
                strclass.AppendLine(GetListAspx());
            }
            return strclass.ToString();
        }

        /// <summary>
        /// 生成表示层页面的CS代码
        /// </summary>
        /// <param name="ExistsKey"></param>
        /// <param name="AddForm">是否生成增加窗体的代码</param>
        /// <param name="UpdateForm">是否生成修改窗体的代码</param>
        /// <param name="ShowForm">是否生成显示窗体的代码</param>
        /// <param name="SearchForm">是否生成查询窗体的代码</param>
        /// <returns></returns>
        public string GetWebCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm)
        {
            StringPlus strclass = new StringPlus();
            if (AddForm)
            {
                strclass.AppendLine("  /******************************增加窗体代码********************************/");
                strclass.AppendLine(GetAddAspxCs());
            }
            if (UpdateForm)
            {
                strclass.AppendLine("  /******************************修改窗体代码********************************/");
                strclass.AppendLine("  /*修改代码-提交更新 */");
                strclass.AppendLine(GetUpdateAspxCs());
            }
            if (ShowForm)
            {
                strclass.AppendLine("  /******************************显示窗体代码********************************/");
                strclass.AppendLine(GetShowAspxCs());
            }
            if (SearchForm)
            {
                strclass.AppendLine("  /******************************列表窗体代码********************************/");
                strclass.AppendLine(GetListAspxCs());
            }
            return strclass.Value;
        }
        #endregion

        #region MapXMLs
        //public string GetMapXMLs()
        //{
        //    Maticsoft.BuilderWeb.BuilderWeb bw = CreatBuilderWeb();
        //    return bw.GetMapXMLs();
        //}

        #endregion



    }
}
