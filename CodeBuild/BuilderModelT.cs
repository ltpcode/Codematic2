using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LTP.IDBO;
using LTP.CodeHelper;
using LTP.Utility;

namespace LTP.CodeBuild
{
    /// <summary>
    /// 父子表的model生成
    /// </summary>
    public class BuilderModelT : BuilderModel
    {
        #region  公有属性
        private string _tablenameson;
        private string _modelnameson; //model类名       
        private string _namespaceson = "Maticsoft"; //顶级命名空间名 
        private string _folderson; //所在文件夹
        private string _modelpathson;
        private List<ColumnInfo> _fieldlistson;
               
        public string TableNameSon
        {
            set { _tablenameson = value; }
            get { return _tablenameson; }
        }
        public string ModelNameSon
        {
            set { _modelnameson = value; }
            get { return _modelnameson; }
        }
        public string NameSpaceSon
        {
            set { _namespaceson = value; }
            get { return _namespaceson; }
        }
        public string FolderSon
        {
            set { _folderson = value; }
            get { return _folderson; }
        }
        /// <summary>
        /// 实体类的命名空间
        /// </summary>
        public string ModelpathSon
        {
            set { _modelpathson = value; }
            get { return _modelpathson; }
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
        /// 选择的字段集合的-字符串
        /// </summary>
        public string FieldsSon
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
        #endregion
               
        public BuilderModelT(IDbObject idbobj, string DbName, string tableNameParent, string modelNameParent,List<ColumnInfo> FieldlistP,
                       string tableNameSon,string modelNameSon,List<ColumnInfo> FieldlistS,string NameSpace, string Folder, string Modelpath)
        {
            dbobj = idbobj;
            _dbname = DbName;
            _tablename = TableName;
            _modelname = ModelName;
            _tablenameson = tableNameSon;
            _modelnameson = modelNameSon;
            _namespace = NameSpace;
            _folder = Folder;
            _modelpath = Modelpath;
            Fieldlist = FieldlistP;
            _fieldlistson = FieldlistS;

        }

        #region 生成完整单个Model类

        /// <summary>
        /// 生成完整单个Model类
        /// </summary>		
        public string CreatModelMethodT()
        {
            
            StringPlus strclass = new StringPlus();
         
            strclass.AppendLine(CreatModelMethod());

            strclass.AppendSpaceLine(2, "private List<" + ModelNameSon + "> _" + ModelNameSon.ToLower() + "s;");//私有变量
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 子类 ");
            strclass.AppendSpaceLine(2, "/// </summary>");
            strclass.AppendSpaceLine(2, "public List<" + ModelNameSon + "> " + ModelNameSon + "s");//属性
            strclass.AppendSpaceLine(2, "{");
            strclass.AppendSpaceLine(3, "set{" + " _" + ModelNameSon.ToLower() + "s=value;}");
            strclass.AppendSpaceLine(3, "get{return " + "_" + ModelNameSon.ToLower() + "s;}");
            strclass.AppendSpaceLine(2, "}");
                        
            return strclass.ToString();
        }
        #endregion
                
    }
}
