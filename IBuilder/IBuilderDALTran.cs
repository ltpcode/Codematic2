using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.IBuilder
{
    /// <summary>
    /// 代码构造器接口(基于事务的父子表处理)
    /// </summary>
    public interface IBuilderDALTran
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
        /// 父表名
        /// </summary>
        string TableNameParent
        {
            set;
            get;
        }
        /// <summary>
        /// 子表名
        /// </summary>
        string TableNameSon
        {
            set;
            get;
        }
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        List<ColumnInfo> FieldlistParent
        {
            set;
            get;
        }
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        List<ColumnInfo> FieldlistSon
        {
            set;
            get;
        }

        /// <summary>
        /// 主键或条件字段列表
        /// </summary>
        List<ColumnInfo> KeysParent
        {
            set;
            get;
        }
        /// <summary>
        /// 主键或条件字段列表
        /// </summary>
        List<ColumnInfo> KeysSon
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
        ///Model类名(父类)
        /// </summary>
        string ModelNameParent
        {
            set;
            get;
        }
        /// <summary>
        /// Model类名(子类)
        /// </summary>
        string ModelNameSon
        {
            set;
            get;
        }
       
        /// <summary>
        /// 数据层的命名空间
        /// </summary>
        string DALpath
        {
            set;
            get;
        }
        /// <summary>
        /// 数据层的类名(父类)
        /// </summary>
        string DALNameParent
        {
            set;
            get;
        }
        /// <summary>
        /// 数据层的类名(子类)
        /// </summary>
        string DALNameSon
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
