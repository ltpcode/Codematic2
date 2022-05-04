using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Maticsoft.CodeHelper;
using Maticsoft.Utility;
namespace Maticsoft.BuilderModel
{
    /// <summary>
    /// 父子表的model生成
    /// </summary>
    public class BuilderModelT : BuilderModel
    {
        #region  公有属性
        private string _modelnameson=""; //model类名        
        public string ModelNameSon
        {
            set { _modelnameson = value; }
            get { return _modelnameson; }
        }
        
        #endregion

        public BuilderModelT()
        {
        }     

        #region 生成完整单个Model类

        /// <summary>
        /// 生成完整Model类
        /// </summary>		
        public string CreatModelMethodT()
        {            
            StringPlus strclass = new StringPlus();         
            strclass.AppendLine(CreatModelMethod());
            strclass.AppendSpaceLine(2, "private List<" + ModelNameSon + "> _" + ModelNameSon.ToLower() + "s;");//私有变量
            strclass.AppendSpaceLine(2, "/// <summary>");
            strclass.AppendSpaceLine(2, "/// 子类 ");
            strclass.AppendSpaceLine(2, "/// </summary>");
            //strclass.AppendSpaceLine(2, "[Serializable]");
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
