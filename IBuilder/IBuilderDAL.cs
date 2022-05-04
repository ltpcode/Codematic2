using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.IBuilder
{
    /// <summary>
    /// DAL代码构造器接口
    /// </summary>
    public interface IBuilderDAL
    { 

        #region 公有属性
        IDbObject DbObject
        {
            set;
            get;
        }
        /// <summary>
        /// 库名
        /// </summary>
        string DbName
        {
            set;
            get;
        }
        /// <summary>
        /// 表名
        /// </summary>
        string TableName
        {
            set;
            get;
        }
        //string TableDescription
        //{
        //    set;
        //    get;
        //}
        
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        List<ColumnInfo> Fieldlist
        {
            set;
            get;
        }        
        /// <summary>
        /// 主键或条件字段列表
        /// </summary>
        List<ColumnInfo> Keys
        {
            set;
            get;
        }
        /// <summary>
        /// 顶级命名空间名
        /// </summary>
        string NameSpace
        {
            set;
            get;
        }
        /// <summary>
        /// 所在文件夹
        /// </summary>
        string Folder
        {
            set;
            get;
        }

        /// <summary>
        /// 实体类的命名空间
        /// </summary>
        string Modelpath
        {
            set;
            get;
        }
        /// <summary>
        /// model类名
        /// </summary>
        string ModelName
        {
            set;
            get;
        }
        ///// <summary>
        ///// 实体类的整个命名空间 + 类名
        ///// </summary>
        //string ModelSpace
        //{
        //    set;
        //    get;
        //}
        
        /// <summary>
        /// 数据层的命名空间
        /// </summary>
        string DALpath
        {
            set;
            get;
        }
        /// <summary>
        /// 数据层的类名
        /// </summary>
        string DALName
        {
            set;
            get;
        }

        /// <summary>
        /// 接口的命名空间
        /// </summary>
        string IDALpath
        {
            set;
            get;
        }
        /// <summary>
        /// 接口类名
        /// </summary>
        string IClass
        {
            set;
            get;
        }


        /// <summary>
        /// 数据库访问类名
        /// </summary>
        string DbHelperName
        {
           set;
            get;
        }        
        /// <summary>
        /// 存储过程前缀 
        /// </summary>       
        string ProcPrefix
        {
            set;
            get;
        }

        ///// <summary>
        ///// 多语言资源列表，请将文件放在指定的文件夹
        ///// </summary>
        //Hashtable Languagelist
        //{
        //    get;
        //}

        #endregion

        string GetDALCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel, bool List);

        /// <summary>
        /// 得到GetMaxID()的方法代码
        /// </summary>
        string CreatGetMaxID();

        /// <summary>
        /// 得到Exists()方法的代码
        /// </summary>
        string CreatExists();
                   
        /// <summary>
        /// 得到Add()的代码
        /// </summary>
        string CreatAdd();       

        /// <summary>
        /// 得到Update()的代码
        /// </summary>        
        string CreatUpdate();
        
        /// <summary>
        /// 得到Delete()的代码
        /// </summary>
        string CreatDelete();
        
        /// <summary>
        /// 得到GetModel()的代码
        /// </summary>
        string CreatGetModel();
        
        /// <summary>
        /// 得到GetList()的代码
        /// </summary> 
        string CreatGetList();

    }
}
