using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.BuilderBLLComm
{
    /// <summary>
    /// 业务层代码组件
    /// </summary>
    public class BuilderBLL : IBuilder.IBuilderBLL
    {
        #region 私有变量
        protected string _key = "ID";//默认第一个主键字段		
        protected string _keyType = "int";//默认第一个主键类型        
        #endregion

        #region 公有属性
        private List<ColumnInfo> _fieldlist;
        private List<ColumnInfo> _keys;
        private string _namespace; //顶级命名空间名
        private string _folder;//所在文件夹
        private string _modelspace;
        private string _modelname;//model类名 
        protected string _tabledescription = "";
        private string _bllname;//bll类名    
        private string _dalname;//dal类名    
        private string _modelpath;
        private string _bllpath;
        private string _factorypath;
        private string _idalpath;
        private string _iclass;
        private string _dalpath;
        private string _dalspace;
        private bool isHasIdentity;
        private string dbType;

        /// <summary>
        /// 选择的字段集合
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        /// <summary>
        /// 主键或条件字段列表 
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
            //set { _keys = value; }
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
        /// 所在文件夹，二级命名空间名
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
        /// Model类名
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
        /// 表的描述信息
        /// </summary>
        public string TableDescription
        {
            set { _tabledescription = value; }
            get { return _tabledescription; }
        }
        /*============================*/

        /// <summary>
        /// 业务逻辑层的命名空间
        /// </summary>
        public string BLLpath
        {
            set { _bllpath = value; }
            get { return _bllpath; }
        }
        /// <summary>
        /// BLL类名
        /// </summary>
        public string BLLName
        {
            set { _bllname = value; }
            get { return _bllname; }
        }

        /*============================*/

        /// <summary>
        /// 数据层的命名空间
        /// </summary>
        public string DALpath
        {
            set { _dalpath = value; }
            get { return _dalpath; }
        }
        /// <summary>
        /// DAL类名
        /// </summary>
        public string DALName
        {
            set { _dalname = value; }
            get { return _dalname; }
        }

        /// <summary>
        /// 数据层的命名空间+ 类名，即等于 DALpath + DALName
        /// </summary>
        public string DALSpace
        {
            get { return DALpath + "." + DALName; }
        }

        /*============================*/
        /// <summary>
        /// 工厂类的命名空间
        /// </summary>
        public string Factorypath
        {
            set { _factorypath = value; }
            get { return _factorypath; }
        }
        /// <summary>
        /// 接口的命名空间
        /// </summary>
        public string IDALpath
        {
            set { _idalpath = value; }
            get { return _idalpath; }
        }
        /// <summary>
        /// 接口名
        /// </summary>
        public string IClass
        {
            set { _iclass = value; }
            get { return _iclass; }
        }

        /*============================*/

        /// <summary>
        /// 是否有自动增长标识列
        /// </summary>
        public bool IsHasIdentity
        {
            set { isHasIdentity = value; }
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
        public string DbType
        {
            set { dbType = value; }
            get { return dbType; }
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
                return Maticsoft.CodeHelper.Language.LoadFromCfg("BuilderBLLComm.lan");
            }
        }
        #endregion

        #region  构造函数
        public BuilderBLL()
        {
        }
        public BuilderBLL(List<ColumnInfo> keys, string modelspace)
        {
            _modelspace = modelspace;
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

        #region 业务层方法
        /// <summary>
        /// 得到整个类的代码
        /// </summary>      
        public string GetBLLCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Data;");
            strclass.AppendLine("using System.Collections.Generic;");
            if (GetModelByCache)
            {
                strclass.AppendLine("using Maticsoft.Common;");
            }
            strclass.AppendLine("using " + Modelpath + ";");
            if ((Factorypath != "") && (Factorypath != null))
            {
                strclass.AppendLine("using " + Factorypath + ";");
            }
            if ((IDALpath != "") && (IDALpath != null))
            {
                strclass.AppendLine("using " + IDALpath + ";");
            }
            strclass.AppendLine("namespace " + BLLpath);
            strclass.AppendLine("{");
            strclass.AppendSpaceLine(1, "/// <summary>");
            if (TableDescription.Length > 0)
            {
                strclass.AppendSpaceLine(1, "/// " + TableDescription.Replace("\r\n", "\r\n\t///"));
            }
            else
            {
                strclass.AppendSpaceLine(1, "/// " + BLLName + ":" + Languagelist["summary"].ToString());
            }
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "public partial class " + BLLName);
            strclass.AppendSpaceLine(1, "{");

            if ((IClass != "") && (IClass != null))
            {
                strclass.AppendSpaceLine(2, "private readonly " + IClass + " dal=" + "DataAccess.Create" + DALName + "();");
                //if (Folder != "")
                //{
                //    strclass.AppendSpaceLine(2, "private readonly " + IClass + " dal=" + "DataAccess<" + IClass + ">.Create(\"" + Folder + "." + DALName + "\");");
                //}
                //else
                //{                    
                //    strclass.AppendSpaceLine(2, "private readonly " + IClass + " dal=" + "DataAccess<" + IClass + ">.Create(\"" + DALName + "\");");
                //}                
            }
            else
            {
                strclass.AppendSpaceLine(2, "private readonly " + DALSpace + " dal=" + "new " + DALSpace + "();");
            }
            strclass.AppendSpaceLine(2, "public " + BLLName + "()");
            strclass.AppendSpaceLine(2, "{}");
            strclass.AppendSpaceLine(2, "#region  BasicMethod");

            #region  方法代码
            if (Maxid)
            {
                if (Keys.Count > 0)
                {
                    foreach (ColumnInfo obj in Keys)
                    {
                        if (CodeCommon.DbTypeToCS(obj.TypeName) == "int")
                        {
                            if (obj.IsPrimaryKey)
                            {
                                strclass.AppendLine(CreatBLLGetMaxID());
                                break;
                            }
                        }
                    }
                }
            }
            if (Exists)
            {
                strclass.AppendLine(CreatBLLExists());
            }
            if (Add)
            {
                strclass.AppendLine(CreatBLLADD());
            }
            if (Update)
            {
                strclass.AppendLine(CreatBLLUpdate());
            }
            if (Delete)
            {
                strclass.AppendLine(CreatBLLDelete());
            }
            if (GetModel)
            {
                strclass.AppendLine(CreatBLLGetModel());
            }
            if (GetModelByCache)
            {
                strclass.AppendLine(CreatBLLGetModelByCache(ModelName));
            }
            if (List)
            {
                strclass.AppendLine(CreatBLLGetList());
                strclass.AppendLine(CreatBLLGetAllList());
                strclass.AppendLine(CreatBLLGetListByPage());
            }

            #endregion
            strclass.AppendSpaceLine(2, "#endregion  BasicMethod");

            strclass.AppendSpaceLine(2, "#region  ExtensionMethod");
            strclass.AppendLine("");
            strclass.AppendSpaceLine(2, "#endregion  ExtensionMethod");


            strclass.AppendSpaceLine(1, "}");
            strclass.AppendLine("}");
            strclass.AppendLine("");

            return strclass.ToString();
        }

        #endregion

        #region 具体方法代码

        public string CreatBLLGetMaxID()
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
                            strclass.AppendSpaceLine(3, "return dal.GetMaxId();");
                            strclass.AppendSpaceLine(2, "}");
                            break;
                        }
                    }
                }
            }
            return strclass.ToString();

        }
        public string CreatBLLExists()
        {
            StringPlus strclass = new StringPlus();
            if (_keys.Count > 0)
            {
                string strInparam = Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false);
                if (!string.IsNullOrEmpty(strInparam))
                {
                    strclass.AppendSpaceLine(2, "/// <summary>");
                    strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryExists"].ToString());
                    strclass.AppendSpaceLine(2, "/// </summary>");
                    strclass.AppendSpaceLine(2, "public bool Exists(" + strInparam + ")");
                    strclass.AppendSpaceLine(2, "{");
                    strclass.AppendSpaceLine(3, "return dal.Exists(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, false) + ");");
                    strclass.AppendSpaceLine(2, "}");
                }
                
            }
            return strclass.ToString();
        }
        public string CreatBLLADD()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryadd"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            string strretu = "bool";
            if ((DbType == "SQL2000" || DbType == "SQL2005" || DbType == "SQL2008" || DbType == "SQL2012" || DbType == "SQLite") && (IsHasIdentity))    
            {
                strretu = "int ";
                if (_keyType != "int")
                {
                    strretu = _keyType;
                }  
            }
            strclass.AppendSpaceLine(2, "public " + strretu + " Add(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            if (strretu == "void")
            {
                strclass.AppendSpaceLine(3, "dal.Add(model);");
            }
            else
            {
                strclass.AppendSpaceLine(3, "return dal.Add(model);");
            }
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }
        public string CreatBLLUpdate()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryUpdate"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Update(" + ModelSpace + " model)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return dal.Update(model);");
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }
        public string CreatBLLDelete()
        {
            StringPlus strclass = new StringPlus();

            #region 标识字段优先的删除
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public bool Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            strclass.AppendSpaceLine(3, "return dal.Delete(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
            strclass.AppendSpaceLine(2, "}");

            #endregion

            #region 联合主键优先的删除(既有标识字段，又有非标识主键字段)

            if ((Maticsoft.CodeHelper.CodeCommon.HasNoIdentityKey(Keys)) && (Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(Keys) != null))
            {
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false) + ")");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, KeysNullTip);
                strclass.AppendSpaceLine(3, "return dal.Delete(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, false) + ");");
                strclass.AppendSpaceLine(2, "}");
            }

            #endregion


            #region 批量删除

            //批量删除方法
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
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryDelete"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public bool DeleteList(string " + keyField + "list )");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "return dal.DeleteList(Maticsoft.Common.PageValidate.SafeLongFilter(" + keyField + "list,0) );");
                strclass.AppendSpaceLine(2, "}");
            }
            #endregion

            return strclass.ToString();
        }
        public string CreatBLLGetModel()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModel"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            strclass.AppendSpaceLine(3, "return dal.GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
            strclass.AppendSpaceLine(2, "}");

            return strclass.ToString();

        }
        public string CreatBLLGetModelByCache(string ModelName)
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetModelByCache"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public " + ModelSpace + " GetModelByCache(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, true) + ")");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, KeysNullTip);
            string para = "";
            if (Keys.Count > 0)
            {
                para = "+ " + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlistAdd(Keys, true);
            }
            strclass.AppendSpaceLine(3, "string CacheKey = \"" + ModelName + "Model-\" " + para + ";");
            strclass.AppendSpaceLine(3, "object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);");
            strclass.AppendSpaceLine(3, "if (objModel == null)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, "try");
            strclass.AppendSpaceLine(4, "{");
            strclass.AppendSpaceLine(5, "objModel = dal.GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetFieldstrlist(Keys, true) + ");");
            strclass.AppendSpaceLine(5, "if (objModel != null)");
            strclass.AppendSpaceLine(5, "{");
            //strclass.AppendSpaceLine(6, "int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt(\"ModelCache\");");
            strclass.AppendSpaceLine(6, "int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache(\"CacheTime\"), 30);");
            strclass.AppendSpaceLine(6, "Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);");
            strclass.AppendSpaceLine(5, "}");
            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(4, "catch{}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return (" + ModelSpace + ")objModel;");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;

        }
        public string CreatBLLGetList()
        {
            StringPlus strclass = new StringPlus();
            //返回DataSet
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetList(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return dal.GetList(strWhere);");
            strclass.AppendSpaceLine(2, "}");



            //根据权限返回DataSet
            string param = "";            
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;
                string deText = field.Description;
                bool ispk = field.IsPrimaryKey;
                bool IsIdentity = field.IsIdentity;
                               
            }
                       

            if (param.Length > 1)//至少有其中一个字段
            {

                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public DataSet GetList(string strWhere" + param + ")");
                strclass.AppendSpaceLine(2, "{");

                //个性定制-特殊字段处理
                //if (fieldValid.Length > 0)
                //{
                //    strclass.AppendSpaceLine(3, "if(!ShowInvalid)");
                //    strclass.AppendSpaceLine(3, "{");
                //    strclass.AppendSpaceLine(4, "if (strWhere.Length > 1)");
                //    strclass.AppendSpaceLine(4, "{");
                //    strclass.AppendSpaceLine(5, "strWhere += \" and \";");
                //    strclass.AppendSpaceLine(4, "}");
                //    strclass.AppendSpaceLine(4, "strWhere += \" " + fieldValid + "=1 \";");
                //    strclass.AppendSpaceLine(3, "}");
                //}
                //if (fieldAuthority.Length > 0)
                //{
                //    strclass.AppendSpaceLine(3, "if (UserPermissions.Count > 0)");
                //    strclass.AppendSpaceLine(3, "{");
                //    strclass.AppendSpaceLine(4, "if (strWhere.Length > 1)");
                //    strclass.AppendSpaceLine(4, "{");
                //    strclass.AppendSpaceLine(5, "strWhere += \" and \";");
                //    strclass.AppendSpaceLine(4, "}");
                //    strclass.AppendSpaceLine(4, "strWhere += \" " + fieldAuthority + " in (\" + StringPlus.GetArrayStr(UserPermissions) + \")\";");
                //    strclass.AppendSpaceLine(3, "}");
                //}


                strclass.AppendSpaceLine(3, "return dal.GetList(strWhere);");
                strclass.AppendSpaceLine(2, "}");
            }


            if ((DbType == "SQL2000") ||
                (DbType == "SQL2005") ||
                (DbType == "SQL2008") || (DbType == "SQL2012"))
            {
                //返回DataSet
                strclass.AppendSpaceLine(2, "/// <summary>");
                strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList2"].ToString());
                strclass.AppendSpaceLine(2, "/// </summary>");
                strclass.AppendSpaceLine(2, "public DataSet GetList(int Top,string strWhere,string filedOrder)");
                strclass.AppendSpaceLine(2, "{");
                strclass.AppendSpaceLine(3, "return dal.GetList(Top,strWhere,filedOrder);");
                strclass.AppendSpaceLine(2, "}");
            }


            //返回List<>
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> GetModelList(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "DataSet ds = dal.GetList(strWhere);");
            strclass.AppendSpaceLine(3, "return DataTableToList(ds.Tables[0]);");
            strclass.AppendSpaceLine(2, "}");


            //返回List<>
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public List<" + ModelSpace + "> DataTableToList(DataTable dt)");
            strclass.AppendSpaceLine(2, "{");            
            strclass.AppendSpaceLine(3, "List<" + ModelSpace + "> modelList = new List<" + ModelSpace + ">();");
            strclass.AppendSpaceLine(3, "int rowsCount = dt.Rows.Count;");
            strclass.AppendSpaceLine(3, "if (rowsCount > 0)");
            strclass.AppendSpaceLine(3, "{");
            strclass.AppendSpaceLine(4, ModelSpace + " model;");
            strclass.AppendSpaceLine(4, "for (int n = 0; n < rowsCount; n++)");
            strclass.AppendSpaceLine(4, "{");

            #region 字段赋值
            /*
            foreach (ColumnInfo field in Fieldlist)
            {
                string columnName = field.ColumnName;
                string columnType = field.TypeName;

                strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"]!=null && dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                strclass.AppendSpaceLine(5, "{");
                #region
                switch (CodeCommon.DbTypeToCS(columnType))
                {
                    case "int":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=int.Parse(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "long":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=long.Parse(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "decimal":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=decimal.Parse(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "float":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=float.Parse(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "DateTime":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=DateTime.Parse(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "string":
                        {
                            strclass.AppendSpaceLine(5, "model." + columnName + "=dt.Rows[n][\"" + columnName + "\"].ToString();");
                        }
                        break;
                    case "bool":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "if((dt.Rows[n][\"" + columnName + "\"].ToString()==\"1\")||(dt.Rows[n][\"" + columnName + "\"].ToString().ToLower()==\"true\"))");
                            strclass.AppendSpaceLine(6, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=true;");
                            strclass.AppendSpaceLine(6, "}");
                            strclass.AppendSpaceLine(6, "else");
                            strclass.AppendSpaceLine(6, "{");
                            strclass.AppendSpaceLine(7, "model." + columnName + "=false;");
                            strclass.AppendSpaceLine(6, "}");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "byte[]":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=(byte[])dt.Rows[n][\"" + columnName + "\"];");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    case "uniqueIdentifier":
                    case "Guid":
                        {
                            //strclass.AppendSpaceLine(5, "if(dt.Rows[n][\"" + columnName + "\"].ToString()!=\"\")");
                            //strclass.AppendSpaceLine(5, "{");
                            strclass.AppendSpaceLine(6, "model." + columnName + "=new Guid(dt.Rows[n][\"" + columnName + "\"].ToString());");
                            //strclass.AppendSpaceLine(5, "}");
                        }
                        break;
                    default:
                        strclass.AppendSpaceLine(5, "//model." + columnName + "=dt.Rows[n][\"" + columnName + "\"].ToString();");
                        break;
                }
                #endregion

                strclass.AppendSpaceLine(5, "}");


            }
*/
            #endregion

            strclass.AppendSpaceLine(5, "model = dal.DataRowToModel(dt.Rows[n]);");
            strclass.AppendSpaceLine(5, "if (model != null)");
            strclass.AppendSpaceLine(5, "{");
            strclass.AppendSpaceLine(6, "modelList.Add(model);");
            strclass.AppendSpaceLine(5, "}");

            strclass.AppendSpaceLine(4, "}");
            strclass.AppendSpaceLine(3, "}");
            strclass.AppendSpaceLine(3, "return modelList;");
            strclass.AppendSpaceLine(2, "}");



            return strclass.ToString();

        }
        public string CreatBLLGetAllList()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetAllList()");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return GetList(\"\");");
            strclass.AppendSpaceLine(2, "}");
            return strclass.ToString();
        }
        public string CreatBLLGetListByPage()
        {
            StringPlus strclass = new StringPlus();
            
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public int GetRecordCount(string strWhere)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return dal.GetRecordCount(strWhere);");
            strclass.AppendSpaceLine(2, "}");

            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)");
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);");
            strclass.AppendSpaceLine(2, "}");


            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// " + Languagelist["summaryGetList3"].ToString());
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "//public DataSet GetList(int PageSize,int PageIndex,string strWhere)");
            strclass.AppendSpaceLine(2, "//{");
            strclass.AppendSpaceLine(3, "//return dal.GetList(PageSize,PageIndex,strWhere);");
            strclass.AppendSpaceLine(2, "//}");
            return strclass.ToString();
        }

        #endregion


    }
}
