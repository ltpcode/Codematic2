using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Maticsoft.IDBO;
using Maticsoft.Utility;
using Maticsoft.DBFactory;
using Maticsoft.IBuilder;
using Maticsoft.CodeHelper;
namespace Maticsoft.CodeBuild
{
    public class BuilderFrameF3 : BuilderFrame
    {
        #region  私有变量
        IBuilderDAL idal;
        IBuilderBLL ibll;
        IBuilderDALTran idaltran;
        IBuilderIDAL iidal;
 
        #endregion


        #region 构造
        public BuilderFrameF3(IDbObject idbobj, string dbName, string tableName, string tableDescription, string modelName, string bllName, string dalName, 
            List<ColumnInfo> fieldlist, List<ColumnInfo> keys, 
            string nameSpace, string folder, string dbHelperName)
        {
            dbobj = idbobj;
            _dbtype = idbobj.DbType;
            DbName = dbName;
            TableName = tableName;
            TableDescription = tableDescription;
            ModelName = modelName;
            BLLName = bllName;
            DALName = dalName;
            NameSpace = nameSpace;
            DbHelperName = dbHelperName;
            Folder = folder;            
            Fieldlist = fieldlist;
            Keys = keys;
            foreach (ColumnInfo key in keys)
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
        public BuilderFrameF3(IDbObject idbobj, string dbName,string nameSpace, string folder, string dbHelperName)
        {
            dbobj = idbobj;
            _dbtype = idbobj.DbType;
            DbName = dbName;
            NameSpace = nameSpace;
            DbHelperName = dbHelperName;
            Folder = folder;

        }
        #endregion



        #region 生成Model
        /// <summary>
		/// 得到Model类
		/// </summary>		
        public string GetModelCode()
        {            
            //return model.CreatModel();
            Maticsoft.BuilderModel.BuilderModel model = new Maticsoft.BuilderModel.BuilderModel();
            model.ModelName = ModelName;
            model.NameSpace = NameSpace;
            model.Fieldlist = Fieldlist;
            model.Modelpath = Modelpath;
            model.TableDescription = TableDescription;
            return model.CreatModel();
        }

        /// <summary>
        /// 得到父子表Model
        /// </summary>		
        public string GetModelCode(string tableNameParent, string modelNameParent, List<ColumnInfo> FieldlistP,
                       string tableNameSon, string modelNameSon, List<ColumnInfo> FieldlistS)
        {
            if (modelNameParent == "")
            {
                modelNameParent = tableNameParent;
            }
            if (modelNameSon == "")
            {
                modelNameSon = tableNameSon;
            }
            StringPlus strclass = new StringPlus();
            StringPlus strclass1 = new StringPlus();
            StringPlus strclass2 = new StringPlus();
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Collections.Generic;");
            strclass.AppendLine("namespace " + Modelpath);
            strclass.AppendLine("{");

            //父类
            //Maticsoft.BuilderModel.BuilderModelT modelP = new Maticsoft.BuilderModel.BuilderModelT(dbobj, DbName, tableNameParent, modelNameParent, FieldlistP,
            //    tableNameSon, modelNameSon, FieldlistS, NameSpace, Folder, Modelpath);
            Maticsoft.BuilderModel.BuilderModelT modelP = new Maticsoft.BuilderModel.BuilderModelT();
            modelP.ModelName = modelNameParent;
            modelP.NameSpace = NameSpace;
            modelP.Fieldlist = FieldlistP;
            modelP.Modelpath = Modelpath;
            modelP.ModelNameSon = modelNameSon;

            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 实体类" + modelNameParent + " 。(属性说明自动提取数据库字段的描述信息)");
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "[Serializable]");    
            strclass.AppendSpaceLine(1, "public class " + modelNameParent);
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, "public " + modelNameParent + "()");
            strclass.AppendSpaceLine(2, "{}");
            strclass.AppendLine(modelP.CreatModelMethodT());
            strclass.AppendSpaceLine(1, "}");

            //子类
            //Maticsoft.BuilderModel.BuilderModel modelS = new Maticsoft.BuilderModel.BuilderModel(dbobj, DbName, tableNameSon, modelNameSon, NameSpace, Folder, Modelpath, FieldlistS);
            Maticsoft.BuilderModel.BuilderModel modelS = new Maticsoft.BuilderModel.BuilderModel();
            modelS.ModelName = modelNameSon;
            modelS.NameSpace = NameSpace;
            modelS.Fieldlist = FieldlistS;
            modelS.Modelpath = Modelpath;

            strclass.AppendSpaceLine(1, "/// <summary>");
            strclass.AppendSpaceLine(1, "/// 实体类" + modelNameSon + " 。(属性说明自动提取数据库字段的描述信息)");
            strclass.AppendSpaceLine(1, "/// </summary>");
            strclass.AppendSpaceLine(1, "[Serializable]");            
            strclass.AppendSpaceLine(1, "public class " + modelNameSon);
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, "public " + modelNameSon + "()");
            strclass.AppendSpaceLine(2, "{}");
            strclass.AppendLine(modelS.CreatModelMethod());
            strclass.AppendSpaceLine(1, "}");

            strclass.AppendLine("}");
            strclass.AppendLine("");

            return strclass.ToString();
        }

        #endregion

        #region 数据访问层代码

        public string GetDALCode(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List, string procPrefix)
        {
            idal = BuilderFactory.CreateDALObj(AssemblyGuid);
            if (idal == null)
            {
                return "//请选择有效的数据层代码组件类型！";
            }
            idal.DbObject = dbobj;
            idal.DbName = DbName;
            idal.TableName = TableName;
            idal.Fieldlist = Fieldlist;
            idal.Keys = Keys;
            idal.NameSpace = NameSpace;            
            idal.Folder = Folder;            
            idal.Modelpath = Modelpath;
            idal.ModelName = ModelName;            
            idal.DALpath = DALpath;
            idal.DALName = DALName;
            idal.IDALpath = IDALpath;
            idal.IClass = IClass;
            idal.DbHelperName = DbHelperName;
            idal.ProcPrefix = procPrefix;
            return idal.GetDALCode(Maxid, Exists, Add, Update, Delete, GetModel, List);

        }
        /// <summary>
        /// 生成父子表，事务代码
        /// </summary>
        public string GetDALCodeTran(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete,
            bool GetModel, bool List, string procPrefix, string tableNameParent, string tableNameSon, string modelNameParent, string modelNameSon,
            List<ColumnInfo> fieldlistParent, List<ColumnInfo> fieldlistSon, List<ColumnInfo> keysParent, List<ColumnInfo> keysSon,
            string DALNameParent, string DALNameSon)
        {
            idaltran = BuilderFactory.CreateDALTranObj(AssemblyGuid);
            if (idaltran == null)
            {
                return "//请选择有效的数据层代码组件类型！";
            }
            idaltran.DbObject = dbobj;
            idaltran.DbName = DbName;
            idaltran.TableNameParent = tableNameParent;
            idaltran.TableNameSon = tableNameSon;
            idaltran.FieldlistParent = fieldlistParent;
            idaltran.FieldlistSon = fieldlistSon;
            idaltran.KeysParent = keysParent;
            idaltran.KeysSon = keysSon;
                        
            idaltran.NameSpace = NameSpace;            
            idaltran.Folder = Folder;            
            idaltran.Modelpath = Modelpath;
            idaltran.ModelNameParent = modelNameParent;
            idaltran.ModelNameSon = modelNameSon;
            idaltran.DALpath = DALpath;
            idaltran.DALNameParent = DALNameParent;
            idaltran.DALNameSon = DALNameSon;

            idaltran.IDALpath = IDALpath;
            idaltran.IClass = IClass;
            idaltran.DbHelperName = DbHelperName;
            idaltran.ProcPrefix = procPrefix;

            return idaltran.GetDALCode(Maxid, Exists, Add, Update, Delete, GetModel, List);

        }
        #endregion

        #region  接口层代码

        /// <summary>
        /// 得到接口层代码
        /// </summary>
        /// <param name="ID">主键</param>
        /// <param name="ModelName">类名</param>
        /// <returns></returns>
        public string GetIDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List, bool ListProc)
        {
            iidal = BuilderFactory.CreateIDALObj();
            if (iidal == null)
            {
                return "//请选择有效的接口层代码组件类型！";
            }
            iidal.Fieldlist = Fieldlist;
            iidal.Keys = Keys;
            iidal.NameSpace = NameSpace;
            iidal.Folder = Folder;
            iidal.Modelpath = Modelpath;
            iidal.ModelName = ModelName;
            iidal.TableDescription = Maticsoft.CodeHelper.CodeCommon.CutDescText(TableDescription, 10, BLLName);            
            iidal.IDALpath = IDALpath;
            iidal.IClass = IClass;            
            iidal.IsHasIdentity = IsHasIdentity;
            iidal.DbType = dbobj.DbType;
            return iidal.GetIDALCode(Maxid, Exists, Add, Update, Delete, GetModel, List);


            #region 
            /*
            StringBuilder strclass = new StringBuilder();
            strclass.Append("using System;\r\n");
            strclass.Append("using System.Data;\r\n");

            strclass.Append("namespace " + IDALpath + "\r\n");
            strclass.Append("{" + "\r\n");
            strclass.Append("	/// <summary>" + "\r\n");            
            strclass.Append("	/// 接口层" + IClass +" "+ Maticsoft.CodeHelper.CodeCommon.CutDescText(TableDescription, 10, "") + "\r\n");
            strclass.Append("	/// </summary>" + "\r\n");
            strclass.Append("	public interface " + IClass + "\r\n");
            strclass.Append("	{\r\n");
            strclass.Append(Space(2) + "#region  成员方法" + "\r\n");

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
                                strclass.Append(Space(2) + "/// <summary>" + "\r\n");
                                strclass.Append(Space(2) + "/// 得到最大ID" + "\r\n");
                                strclass.Append(Space(2) + "/// </summary>" + "\r\n");
                                strclass.Append("		int GetMaxId();" + "\r\n");
                                break;
                            }
                        }
                    }
                }
            }
            if (Exists)
            {
                if (Keys.Count > 0)
                {
                    strclass.Append(Space(2) + "/// <summary>" + "\r\n");
                    strclass.Append(Space(2) + "/// 是否存在该记录" + "\r\n");
                    strclass.Append(Space(2) + "/// </summary>" + "\r\n");
                    strclass.Append(Space(2) + "bool Exists(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys,false) + ");" + "\r\n");
                }
            }
            if (Add)
            {
                strclass.Append(Space(2) + "/// <summary>" + "\r\n");
                strclass.Append(Space(2) + "/// 增加一条数据" + "\r\n");
                strclass.Append(Space(2) + "/// </summary>" + "\r\n");
                //string strretu = "void";
                //if (IsHasIdentity)
                //{
                //    strretu = "int";
                //}
                string strretu = "void";
                if ((dbobj.DbType == "SQL2000" || dbobj.DbType == "SQL2005" || dbobj.DbType == "SQL2008") && (IsHasIdentity))
                {
                    strretu = "int";  //CodeCommon.DbTypeToCS(obj.TypeName)
                    if (_keyType != "int")
                    {
                        strretu = _keyType;
                    }
                }
                strclass.Append(Space(2)  + strretu + " Add(" + ModelSpace + " model);" + "\r\n");
                
            }
            if (Update)
            {
                strclass.Append(Space(2) + "/// <summary>" + "\r\n");
                strclass.Append(Space(2) + "/// 更新一条数据" + "\r\n");
                strclass.Append(Space(2) + "/// </summary>" + "\r\n");
                strclass.Append(Space(2) + "bool Update(" + ModelSpace + " model);" + "\r\n");
            }
            if (Delete)
            {
                strclass.Append(Space(2) + "/// <summary>" + "\r\n");
                strclass.Append(Space(2) + "/// 删除一条数据" + "\r\n");
                strclass.Append(Space(2) + "/// </summary>" + "\r\n");
                strclass.Append(Space(2) + "bool Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys,true) + ");" + "\r\n");

                #region 联合主键优先的删除(既有标识字段，又有非标识主键字段)

                if ((Maticsoft.CodeHelper.CodeCommon.HasNoIdentityKey(Keys)) && (Maticsoft.CodeHelper.CodeCommon.GetIdentityKey(Keys) != null))
                {
                    strclass.Append(Space(2) + "/// <summary>" + "\r\n");
                    strclass.Append(Space(2) + "/// 删除一条数据" + "\r\n");
                    strclass.Append(Space(2) + "/// </summary>" + "\r\n");
                    strclass.Append(Space(2) + "bool Delete(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys, false) + ");" + "\r\n");
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
                    strclass.Append(Space(2) + "bool DeleteList(string " + keyField + "list );" + "\r\n");
                }

#endregion


            }
            if (GetModel)
            {
                strclass.Append(Space(2) + "/// <summary>" + "\r\n");
                strclass.Append(Space(2) + "/// 得到一个对象实体" + "\r\n");
                strclass.Append(Space(2) + "/// </summary>" + "\r\n");
                strclass.Append(Space(2) + ModelSpace + " GetModel(" + Maticsoft.CodeHelper.CodeCommon.GetInParameter(Keys,true) + ");" + "\r\n");
            }
            if (List)
            {
                strclass.Append(Space(2) + "/// <summary>" + "\r\n");
                strclass.Append(Space(2) + "/// 获得数据列表" + "\r\n");
                strclass.Append(Space(2) + "/// </summary>" + "\r\n");
                strclass.Append(Space(2) + "DataSet GetList(string strWhere);" + "\r\n");

                if ((dbobj.DbType == "SQL2000") ||
                (dbobj.DbType == "SQL2005") ||
                (dbobj.DbType == "SQL2008"))
                {
                    strclass.Append(Space(2) + "/// <summary>" + "\r\n");
                    strclass.Append(Space(2) + "/// 获得前几行数据" + "\r\n");
                    strclass.Append(Space(2) + "/// </summary>" + "\r\n");
                    strclass.Append(Space(2) + "DataSet GetList(int Top,string strWhere,string filedOrder);" + "\r\n");
                }
            }
            if (ListProc)
            {
                //strclass.Append(Space(2) + "/// <summary>" + "\r\n");
                //strclass.Append(Space(2) + "/// 获得数据列表" + "\r\n");
                //strclass.Append(Space(2) + "/// </summary>" + "\r\n");
                //strclass.Append("		DataSet GetList();" + "\r\n");

                strclass.Append(Space(2) + "/// <summary>" + "\r\n");
                strclass.Append(Space(2) + "/// 根据分页获得数据列表" + "\r\n");
                strclass.Append(Space(2) + "/// </summary>" + "\r\n");
                strclass.Append(Space(2) + "//DataSet GetList(int PageSize,int PageIndex,string strWhere);" + "\r\n");
            }
            strclass.Append(Space(2) + "#endregion  成员方法" + "\r\n");
            strclass.Append("	}\r\n");
            strclass.Append("}" + "\r\n");
            return strclass.ToString();
            */
            #endregion


        }
        #endregion

        #region 数据工厂

        public string GetDALFactoryCode()
        {
            StringPlus strclass = new StringPlus();            
            strclass.AppendLine("using System;");
            strclass.AppendLine("using System.Reflection;");
            strclass.AppendLine("using System.Configuration;");
            strclass.AppendLine("using " + IDALpath + ";");
            strclass.AppendLine("namespace " + Factorypath );
            strclass.AppendLine("{" );
            strclass.AppendSpaceLine(1, "/// <summary>" );
            strclass.AppendSpaceLine(1, "/// 抽象工厂模式创建DAL。");
            strclass.AppendSpaceLine(1, "/// web.config 需要加入配置：(利用工厂模式+反射机制+缓存机制,实现动态创建不同的数据层对象接口) ");
            strclass.AppendSpaceLine(1, "/// DataCache类在导出代码的文件夹里");
            strclass.AppendSpaceLine(1, "/// <appSettings> ");
            strclass.AppendSpaceLine(1, "/// <add key=\"DAL\" value=\"" + DALpath + "\" /> (这里的命名空间根据实际情况更改为自己项目的命名空间)");
            strclass.AppendSpaceLine(1, "/// </appSettings> ");
            strclass.AppendSpaceLine(1, "/// </summary>" );
            strclass.AppendSpaceLine(1, "public sealed class DataAccess//<t>");
            strclass.AppendSpaceLine(1, "{");
            strclass.AppendSpaceLine(2, "private static readonly string AssemblyPath = ConfigurationManager.AppSettings[\"DAL\"];");

            //CreateObject
            strclass.AppendSpaceLine(2, "/// <summary>" );
            strclass.AppendSpaceLine(2, "/// 创建对象或从缓存获取" );
            strclass.AppendSpaceLine(2, "/// </summary>" );
            strclass.AppendSpaceLine(2, "public static object CreateObject(string AssemblyPath,string ClassNamespace)" );
            strclass.AppendSpaceLine(2, "{" );
            strclass.AppendSpaceLine(3, "object objType = DataCache.GetCache(ClassNamespace);//从缓存读取" );
            strclass.AppendSpaceLine(3, "if (objType == null)" );
            strclass.AppendSpaceLine(3, "{" );
            strclass.AppendSpaceLine(4, "try" );
            strclass.AppendSpaceLine(4, "{" );
            strclass.AppendSpaceLine(5, "objType = Assembly.Load(AssemblyPath).CreateInstance(ClassNamespace);//反射创建");
            strclass.AppendSpaceLine(5, "DataCache.SetCache(ClassNamespace, objType);// 写入缓存" );
            strclass.AppendSpaceLine(4, "}" );
            strclass.AppendSpaceLine(4, "catch" );
            strclass.AppendSpaceLine(4, "{}" );
            strclass.AppendSpaceLine(3, "}" );
            strclass.AppendSpaceLine(3, "return objType;" );
            strclass.AppendSpaceLine(2, "}" );


            //Create
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 创建数据层接口");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "//public static t Create(string ClassName)");
            strclass.AppendSpaceLine(2, "//{");
            strclass.AppendSpaceLine(3, "//string ClassNamespace = AssemblyPath +\".\"+ ClassName;");           
            strclass.AppendSpaceLine(3, "//object objType = CreateObject(AssemblyPath, ClassNamespace);");
            strclass.AppendSpaceLine(3, "//return (t)objType;");
            strclass.AppendSpaceLine(2, "//}");


            strclass.AppendLine(GetDALFactoryMethodCode());

            strclass.AppendSpaceLine(1, "}" );
            strclass.AppendLine("}" );            
            return strclass.ToString();
        }

        /// <summary>
        /// 得到工厂中，具体接口创建方法代码
        /// </summary>
        /// <returns></returns>
        public string GetDALFactoryMethodCode()
        {
            StringPlus strclass = new StringPlus();
            strclass.AppendSpaceLine(2, "/// <summary>" );
            strclass.AppendSpaceLine(2, "/// 创建" + DALName + "数据层接口。" +Maticsoft.CodeHelper.CodeCommon.CutDescText(TableDescription,10,""));
            strclass.AppendSpaceLine(2, "/// </summary>" );
            strclass.AppendSpaceLine(2, "public static " + IDALpath + "." + IClass + " Create" + DALName + "()");
            strclass.AppendSpaceLine(2, "{\r\n");
            if (Folder != "")
            {
                strclass.AppendSpaceLine(3, "string ClassNamespace = AssemblyPath +\"." + Folder + "." + DALName + "\";");
            }
            else
            {
                strclass.AppendSpaceLine(3, "string ClassNamespace = AssemblyPath +\"" + "." + DALName + "\";");
            }
            strclass.AppendSpaceLine(3, "object objType=CreateObject(AssemblyPath,ClassNamespace);");
            strclass.AppendSpaceLine(3, "return (" + IDALpath + "." + IClass + ")objType;");
            strclass.AppendSpaceLine(2, "}");
            return strclass.Value;
        }

        #endregion

        #region 业务层 
        public string GetBLLCode(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List, bool ListProc)
        {
            ibll = BuilderFactory.CreateBLLObj(AssemblyGuid);
            if (ibll == null)
            {
                return "//请选择有效的业务层代码组件类型！";
            }
            ibll.Fieldlist = Fieldlist;
            ibll.Keys = Keys;
            ibll.NameSpace = NameSpace;
            ibll.Folder = Folder;
            ibll.Modelpath = Modelpath;
            ibll.ModelName = ModelName;
            ibll.TableDescription = Maticsoft.CodeHelper.CodeCommon.CutDescText(TableDescription, 10, BLLName);
            ibll.BLLpath = BLLpath;
            ibll.BLLName = BLLName;
            ibll.Factorypath = Factorypath;
            ibll.IDALpath = IDALpath;
            ibll.IClass = IClass;
            ibll.DALpath = DALpath;
            ibll.DALName = DALName;
            ibll.IsHasIdentity = IsHasIdentity;
            ibll.DbType = dbobj.DbType;

            return ibll.GetBLLCode(Maxid, Exists, Add, Update, Delete, GetModel, GetModelByCache, List);

           

        }
        #endregion


    }
}
