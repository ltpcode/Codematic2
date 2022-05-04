using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.IBuilder
{
    /// <summary>
    /// 代码构造器接口(多表事务代码生成)
    /// </summary>
    public interface IBuilderDALMTran
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
        
        List<ModelTran> ModelTranList
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
        /// 数据层的命名空间
        /// </summary>
        string DALpath
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
       
        #endregion

        string GetDALCode();
              

    }
}
