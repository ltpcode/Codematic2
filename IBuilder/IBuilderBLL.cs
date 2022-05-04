using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.IBuilder
{
    /// <summary>
    /// BLL代码构造器接口
    /// </summary>
    public interface IBuilderBLL
    {
        #region 公有属性
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
        /// 所在文件夹，二级命名空间名
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
        /// Model类名
        /// </summary>
        string ModelName
        {
            set;
            get;
        }
        string TableDescription
        {
            set;
            get;
        }
        /// <summary>
        /// 业务逻辑层的命名空间
        /// </summary>
        string BLLpath
        {
            set;
            get;
        }
        /// <summary>
        /// BLL类名
        /// </summary>
        string BLLName
        {
            set;
            get;
        }

        /// <summary>
        /// 工厂类的命名空间
        /// </summary>
        string Factorypath
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
        /// 接口名
        /// </summary>
        string IClass
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
        /// DAL类名
        /// </summary>
        string DALName
        {
            set;
            get;
        }
        /// <summary>
        /// 是否有自动增长标识列
        /// </summary>
        bool IsHasIdentity
        {
            set;
            get;
        }
        /// <summary>
        /// 数据库类型
        /// </summary>
        string DbType
        {
            set;
            get;
        }
        
        #endregion

        string GetBLLCode(bool Maxid, bool Exists, bool Add, bool Update, bool Delete, bool GetModel,bool GetModelByCache, bool List);

        /// <summary>
        /// 得到GetMaxID()的方法代码
        /// </summary>
        string CreatBLLGetMaxID();

        /// <summary>
        /// 得到Exists()方法的代码
        /// </summary>
        string CreatBLLExists();

        /// <summary>
        /// 得到Add()的代码
        /// </summary>
        string CreatBLLADD();

        /// <summary>
        /// 得到Update()的代码
        /// </summary>        
        string CreatBLLUpdate();

        /// <summary>
        /// 得到Delete()的代码
        /// </summary>
        string CreatBLLDelete();

        /// <summary>
        /// 得到GetModel()的代码
        /// </summary>
        string CreatBLLGetModel();

        /// <summary>
        /// 得到GetList()的代码
        /// </summary> 
        string CreatBLLGetList();
    }
}
