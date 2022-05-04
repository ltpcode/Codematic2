using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;
using Maticsoft.Utility;
using Maticsoft.IDBO;
using Maticsoft.DBFactory;
using Maticsoft.IBuilder;
using Maticsoft.CodeHelper;
namespace Maticsoft.CodeBuild
{
    /// <summary>
    /// 简单三层
    /// </summary>
    public class BuilderFrameS3 : BuilderFrame
    {

        #region  私有变量
        IBuilderDAL idal;
        IBuilderDALTran idaltran;
        IBuilderDALMTran idalmtran;
        IBuilderBLL ibll;
                
        /// <summary>
        /// 数据层的命名空间
        /// </summary>
        public new string DALpath
        {
            get
            {
                string _dalpath = NameSpace + "." + "DAL";
                if (Folder.Trim() != "")
                {
                    _dalpath += "." + Folder;
                }
                return _dalpath;
            }            
        }
        public new string DALSpace
        {            
            get
            {
                return DALpath + "." + DALName;
            }
        }    

        #endregion


        #region 构造
        public BuilderFrameS3(IDbObject idbobj, string dbName, string tableName,string tableDescription, string modelName, string bllName, string dalName, 
            List<ColumnInfo> fieldlist, List<ColumnInfo> keys, 
            string nameSpace, string folder,string dbHelperName)
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
            //model = new BuilderModel(dbobj, DbName, TableName, ModelName, NameSpace, Folder, Modelpath, fieldlist);   
         
        }

        public BuilderFrameS3(IDbObject idbobj, string dbName, string nameSpace, string folder, string dbHelperName)
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
        public string GetModelCode()
        {
            Maticsoft.BuilderModel.BuilderModel model = new Maticsoft.BuilderModel.BuilderModel();
            model.ModelName = ModelName;
            model.NameSpace = NameSpace;
            model.Fieldlist = Fieldlist;
            model.Modelpath = Modelpath;
            model.TableDescription = TableDescription;
            return model.CreatModel();
        }
        /// <summary>
		/// 得到Model类
		/// </summary>		
        //public string GetModelCode(string AssemblyGuid)
        //{
        //    //return model.CreatModel();
        //    imodel = BuilderFactory.CreateModelObj(AssemblyGuid);
        //    if (imodel == null)
        //    {
        //        return "请选择有效的Model层代码组件类型！";
        //    }
        //    imodel.ModelName = ModelName;
        //    imodel.NameSpace = NameSpace;
        //    imodel.Fieldlist = Fieldlist;
        //    //imodel.Keys = Keys;
        //    imodel.Modelpath = Modelpath;
        //    //imodel.ModelSpace = ModelSpace;
        //    return imodel.CreatModel();
        //}

        /// <summary>
        /// 得到父子表Model
        /// </summary>		
        public string GetModelCode(string tableNameParent, string modelNameParent,List<ColumnInfo> FieldlistP,
                       string tableNameSon,string modelNameSon,List<ColumnInfo> FieldlistS)
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
            //    tableNameSon, modelNameSon, FieldlistS,NameSpace, Folder, Modelpath);

            Maticsoft.BuilderModel.BuilderModelT modelP = new Maticsoft.BuilderModel.BuilderModelT();
            modelP.ModelName = modelNameParent;
            modelP.NameSpace = NameSpace;
            modelP.Fieldlist = FieldlistP;
            modelP.Modelpath = Modelpath;
            modelP.ModelNameSon = modelNameSon;
            //modelP.FieldlistSon = FieldlistS;

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
            idal.IDALpath = "";
            idal.IClass = "";
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
            
            idaltran.IDALpath = "";
            idaltran.IClass = "";
            idaltran.DbHelperName = DbHelperName;
            idaltran.ProcPrefix = procPrefix;

            return idaltran.GetDALCode(Maxid, Exists, Add, Update, Delete, GetModel, List);

        }


        /// <summary>
        /// 多表事务代码生成
        /// </summary>     
        public string GetDALCodeMTran(string AssemblyGuid, List<ModelTran> modelTranlist)
        {
            idalmtran = BuilderFactory.CreateDALMTranObj(AssemblyGuid);
            if (idalmtran == null)
            {
                return "//请选择有效的数据层代码组件类型！";
            }
            idalmtran.DbObject = dbobj;
            idalmtran.DbName = DbName;
            idalmtran.NameSpace = NameSpace;
            idalmtran.Folder = Folder;
            idalmtran.DbHelperName = DbHelperName;


            idalmtran.Modelpath = Modelpath;
            idalmtran.DALpath = DALpath;
            idalmtran.ModelTranList = modelTranlist;
            
            idalmtran.IDALpath = "";
            idalmtran.IClass = "";
            
            
            return idalmtran.GetDALCode();
        
        }

        #endregion
                
        #region 业务层
        public string GetBLLCode(string AssemblyGuid, bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool GetModelByCache, bool List)
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
            ibll.BLLpath = BLLpath;
            ibll.BLLName = BLLName;
            ibll.TableDescription = Maticsoft.CodeHelper.CodeCommon.CutDescText(TableDescription, 10, BLLName);
            ibll.Factorypath = "";
            ibll.IDALpath = "";
            ibll.IClass = "";
            ibll.DALpath = DALpath;
            ibll.DALName = DALName;
            ibll.IsHasIdentity = IsHasIdentity;
            ibll.DbType = dbobj.DbType;

            return ibll.GetBLLCode(Maxid, Exists, Add, Update, Delete, GetModel,GetModelByCache, List);

           

        }
        
        #endregion


    }
}
