using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Maticsoft.AddInManager;
using Maticsoft.Utility;
using System.Data;
namespace Maticsoft.CodeBuild
{
    /// <summary>
    /// 代码生成对象 工厂
    /// </summary>
    public class BuilderFactory
    {
        private static Cache cache = new Cache();

        #region 程序集反射

        private static object CreateObject(string path, string TypeName)
        {
            object obj = cache.GetObject(TypeName);
            if (obj == null)
            {
                try
                {
                    obj = Assembly.Load(path).CreateInstance(TypeName,true);
                    cache.SaveCache(TypeName, obj);// 写入缓存
                }
                catch (System.Exception ex)
                {
                    string str = ex.Message;// 记录错误日志
                }
            }
            return obj;
        }
        #endregion

        #region 加载数据访问层 代码生成对象

        /// <summary>
        /// 创建数据访问层 代码生成对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Maticsoft.IBuilder.IBuilderDAL CreateDALObj(string AssemblyGuid)
        {
            try
            {
                if (AssemblyGuid == "")
                {
                    return null;
                }
                AddIn addin = new AddIn(AssemblyGuid);                
                string Assembly = addin.Assembly;
                string Classname = addin.Classname;

                object objType = CreateObject(Assembly, Classname);
                return (Maticsoft.IBuilder.IBuilderDAL)objType;
            }
            catch (SystemException ex)
            {
                string err = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 创建数据访问层 代码生成对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Maticsoft.IBuilder.IBuilderDALTran CreateDALTranObj(string AssemblyGuid)
        {
            try
            {
                if (AssemblyGuid == "")
                {
                    return null;
                }
                AddIn addin = new AddIn(AssemblyGuid);
                string Assembly = addin.Assembly;
                string Classname = addin.Classname;

                object objType = CreateObject(Assembly, Classname);
                return (Maticsoft.IBuilder.IBuilderDALTran)objType;
            }
            catch (SystemException ex)
            {
                string err = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 创建数据访问层 代码生成对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Maticsoft.IBuilder.IBuilderDALMTran CreateDALMTranObj(string AssemblyGuid)
        {
            try
            {
                if (AssemblyGuid == "")
                {
                    return null;
                }
                AddIn addin = new AddIn(AssemblyGuid);
                string Assembly = addin.Assembly;
                string Classname = addin.Classname;

                object objType = CreateObject(Assembly, Classname);
                return (Maticsoft.IBuilder.IBuilderDALMTran)objType;
            }
            catch (SystemException ex)
            {
                string err = ex.Message;
                return null;
            }
        }
        #endregion

        #region 加载接口层 代码生成对象

        /// <summary>
        /// 创建接口层 代码生成对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Maticsoft.IBuilder.IBuilderIDAL CreateIDALObj()
        {
            try
            {
                object objType = CreateObject("Maticsoft.BuilderIDAL", "Maticsoft.BuilderIDAL.BuilderIDAL");
                return (Maticsoft.IBuilder.IBuilderIDAL)objType;
            }
            catch (SystemException ex)
            {
                string err = ex.Message;
                return null;
            }
        }


        #endregion

        #region 加载业务层 代码生成对象

        /// <summary>
        /// 创建业务层 代码生成对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Maticsoft.IBuilder.IBuilderBLL CreateBLLObj(string AssemblyGuid)
        {
            try
            {
                if (AssemblyGuid == "")
                {
                    return null;
                }
                AddIn addin = new AddIn(AssemblyGuid);
                string Assembly = addin.Assembly;
                string Classname = addin.Classname;

                object objType = CreateObject(Assembly, Classname);
                return (Maticsoft.IBuilder.IBuilderBLL)objType;
            }
            catch (SystemException ex)
            {
                string err = ex.Message;
                return null;
            }
        }
  

        #endregion

        #region 加载Model层 代码生成对象

        /// <summary>
        /// 创建业务层 代码生成对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Maticsoft.IBuilder.IBuilderModel CreateModelObj(string AssemblyGuid)
        {
            try
            {
                if (AssemblyGuid == "")
                {
                    return null;
                }
                AddIn addin = new AddIn(AssemblyGuid);
                string Assembly = addin.Assembly;
                string Classname = addin.Classname;

                object objType = CreateObject(Assembly, Classname);
                return (Maticsoft.IBuilder.IBuilderModel)objType;
            }
            catch (SystemException ex)
            {
                string err = ex.Message;
                return null;
            }
        }


        #endregion

        #region 加载WEB层 代码生成对象

        /// <summary>
        /// 创建业务层 代码生成对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Maticsoft.IBuilder.IBuilderWeb CreateWebObj(string AssemblyGuid)
        {
            try
            {
                if (AssemblyGuid == "")
                {
                    return null;
                }
                AddIn addin = new AddIn(AssemblyGuid);
                string Assembly = addin.Assembly;
                string Classname = addin.Classname;

                object objType = CreateObject(Assembly, Classname);
                return (Maticsoft.IBuilder.IBuilderWeb)objType;
            }
            catch (SystemException ex)
            {
                string err = ex.Message;
                return null;
            }
        }


        #endregion


    }
}
