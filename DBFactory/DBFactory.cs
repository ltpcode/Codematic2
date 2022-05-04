using System;
using System.Reflection;
using Maticsoft.Utility;
using Maticsoft.IDBO;
namespace Maticsoft.DBFactory
{
	/// <summary>
	/// 数据库信息类实例工厂,利用反射动态创建对象。
	/// </summary>
    public class DBOMaker
	{
        private static Cache cache = new Cache();

        #region 同一程序集内反射

        //#region Cache-CreateObject
        //public static object CreateObject(string TypeName)
        //{
        //    object obj = cache.GetObject(TypeName);
        //    if (obj == null)
        //    {
        //        try
        //        {
        //            Type objType = Type.GetType(TypeName, true);
        //            obj = Activator.CreateInstance(objType);
        //            cache.SaveCaech(TypeName, obj);// 写入缓存
        //        }
        //        catch//(System.Exception ex)
        //        {
        //            //string str=ex.Message;// 记录错误日志
        //        }
        //    }
        //    return obj;
        //}
        //#endregion

        ///// <summary>
        ///// 创建数据库信息类接口
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static IDbObject CreateDbObj(string obj)
        //{		
        //    //从程序集创建对象实例
        //    //string objpath = ConfigHelper.GetConfigString("DbObject");
        //    //return (IDbObject)Assembly.Load(objpath).CreateInstance(objpath+".DbObject");

        //    // 使用与指定参数匹配程度最高的构造函数来创建指定类型的实例
        //    ////string obj = ConfigHelper.GetConfigString("DbObject");
        //    //string TypeName="Maticsoft.CodeBuild."+obj+".DbObject";
        //    //Type objType = Type.GetType(TypeName,true);
        //    //return (IDbObject)Activator.CreateInstance(objType);

        //    string TypeName = "Maticsoft.CodeBuild." + obj + ".DbObject";
        //    object objType = CreateObject(TypeName);
        //    return (IDbObject)objType;
			
        //}

        ///// <summary>
        ///// 创建数据库脚本生成类接口
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static IDbScriptBuilder CreateScript(string obj)
        //{			
        //    //string TypeName="Maticsoft.CodeBuild."+obj+".DbScriptBuilder";
        //    //Type objType = Type.GetType(TypeName,true);
        //    //return (IDbScriptBuilder)Activator.CreateInstance(objType);

        //    string TypeName = "Maticsoft.CodeBuild." + obj + ".DbScriptBuilder";
        //    object objType = CreateObject(TypeName);
        //    return (IDbScriptBuilder)objType;
        //}

        #endregion


        #region 不同程序集反射

        private static object CreateObject(string path, string TypeName)
        {
            object obj = cache.GetObject(TypeName);
            if (obj == null)
            {
                try
                {
                    obj = Assembly.Load(path).CreateInstance(TypeName);                    
                    cache.SaveCache(TypeName, obj);// 写入缓存
                }
                catch(System.Exception ex)
                {
                    string str=ex.Message;// 记录错误日志
                }
            }
            return obj;
        }
        /// <summary>
        /// 创建数据库信息类接口
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDbObject CreateDbObj(string dbTypename)
        {
            //从程序集创建对象实例
            //string objpath = ConfigHelper.GetConfigString("DbObject");
            //return (IDbObject)Assembly.Load(objpath).CreateInstance(objpath+".DbObject");

            string TypeName = "Maticsoft.DbObjects." + dbTypename + ".DbObject";
            object objType = CreateObject("Maticsoft.DbObjects", TypeName);
            return (IDbObject)objType;
        }

        /// <summary>
        /// 创建数据库脚本生成类接口
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDbScriptBuilder CreateScript(string dbTypename)
        {
            string TypeName = "Maticsoft.DbObjects." + dbTypename + ".DbScriptBuilder";                       
            object objType = CreateObject("Maticsoft.DbObjects", TypeName);
            return (IDbScriptBuilder)objType;
        }

        #endregion


    }
}
