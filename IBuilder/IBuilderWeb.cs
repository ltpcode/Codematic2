using System;
using System.Collections.Generic;
using System.Text;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.IBuilder
{
    /// <summary>
    ///Web层代码构造器接口
    /// </summary>
    public interface IBuilderWeb
    {

        #region 公有属性
        
        /// <summary>
        /// 顶级命名空间名 
        /// </summary>
        string NameSpace
        {
            set;
            get;
        }
        string Folder
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
        string BLLName
        {
            set;
            get;
        }
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
        #endregion

        #region Aspx页面html

        /// <summary>
        /// 得到表示层增加窗体的html代码
        /// </summary>      
        string GetAddAspx();        
        /// <summary>
        /// 得到表示层增加窗体的html代码
        /// </summary>      
        string GetUpdateAspx();
        /// <summary>
        /// 得到表示层显示窗体的html代码
        /// </summary>     
        string GetShowAspx();
        /// <summary>
        /// 得到表示层列表窗体的html代码
        /// </summary>     
        string GetListAspx();
        /// <summary>
        /// 增删改3个页面代码
        /// </summary>      
        string GetWebHtmlCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm);
       
        #endregion

        #region 表示层 CS
        /// <summary>
        /// 得到表示层增加窗体的代码
        /// </summary>      
        string GetAddAspxCs();
        /// <summary>
        /// 得到修改窗体的代码
        /// </summary>      
        string GetUpdateAspxCs();        
        /// <summary>
        /// 得到修改窗体的代码
        /// </summary>       
        string GetUpdateShowAspxCs();
        /// <summary>
        /// 得到表示层显示窗体的代码
        /// </summary>       
        string GetShowAspxCs();
        /// <summary>
        /// 得到表示层删除页面的代码
        /// </summary>       
        string GetDeleteAspxCs();
        /// <summary>
        /// 得到表示层显示窗体的代码
        /// </summary>       
        string GetListAspxCs();
        /// <summary>
        /// 删除页面
        /// </summary>
        /// <returns></returns>
        //string CreatDeleteForm();        
        string CreatSearchForm();        
        string GetWebCode(bool ExistsKey, bool AddForm, bool UpdateForm, bool ShowForm, bool SearchForm);
       
        #endregion//表示层

        #region  生成aspx.designer.cs
        /// <summary>
        /// 得到表示层增加窗体的html代码
        /// </summary>      
        string GetAddDesigner(); 
        /// <summary>
        /// 得到表示层增加窗体的html代码
        /// </summary>      
        string GetUpdateDesigner(); 
        /// <summary>
        /// 得到表示层显示窗体的html代码
        /// </summary>     
        string GetShowDesigner();
        /// <summary>
        /// 得到表示层列表窗体的html代码
        /// </summary>     
        string GetListDesigner();   
        #endregion
    }
}
