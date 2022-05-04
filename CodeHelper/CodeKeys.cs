using System;
using System.Collections.Generic;
using System.Text;
using Maticsoft.Utility;
        
namespace Maticsoft.CodeHelper
{
    /// <summary>
    /// 代码中，关于数据库的主键或条件字段
    /// </summary>
    public class CodeKey
    {
        private string _keyName;
        private string _keyType;
        private bool _isPK;
        private bool _isIdentity;

        /// <summary>
        /// 字段名
        /// </summary>
        public string KeyName
        {
            set { _keyName = value; }
            get { return _keyName; }
        }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string KeyType
        {
            set { _keyType = value; }
            get { return _keyType; }
        }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPK
        {
            set { _isPK = value; }
            get { return _isPK; }
        }
        /// <summary>
        /// 是否是标识列
        /// </summary>
        public bool IsIdentity
        {
            set { _isIdentity = value; }
            get { return _isIdentity; }
        }


        


    }

    /// <summary>
    /// 主键字段和条件字段操作类
    /// </summary>
    public class CodeKeyMaker
    { 

        ///// <summary>
        ///// 得到参数的列表(例如：用于Exists  Delete  GetModel 的参数传入)
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <returns></returns>
        //public static string GetParameter(List<CodeKey> keys)
        //{
        //    StringPlus strclass = new StringPlus();
        //    foreach (CodeKey key in keys)
        //    {
        //        strclass.Append(CodeCommon.DbTypeToCS(key.KeyType) + " " + key.KeyName + ",");                
        //    }
        //    strclass.DelLastComma();
        //    return strclass.Value;
        //}

        ///// <summary>
        ///// 得到Where条件语句-SQL方式 (例如：用于Exists  Delete  GetModel 的where)
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <returns></returns>
        //public static string GetParameter(List<CodeKey> keys)
        //{
        //    StringPlus strclass = new StringPlus();
        //    foreach (CodeKey key in keys)
        //    {
        //        if (CodeCommon.IsAddMark(key.KeyType))
        //        {
        //            strclass.Append(key.KeyName + "='\"+" + key.KeyName + "+\"'\"");
        //        }
        //        else
        //        {
        //            strclass.Append(key.KeyName + "=\"+" + key.KeyName );
        //        }
                
        //    }            
        //    return strclass.Value;
        //}
        ///// <summary>
        ///// 得到Where条件语句-SqlParameter方式 (例如：用于Exists  Delete  GetModel 的where)
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <returns></returns>
        //public static string GetParameter(List<CodeKey> keys)
        //{
        //    StringPlus strclass = new StringPlus();
        //    foreach (CodeKey key in keys)
        //    {
        //        if (CodeCommon.IsAddMark(key.KeyType))
        //        {
        //            strclass.Append(key.KeyName + "='\"+" + key.KeyName + "+\"'\"");
        //        }
        //        else
        //        {
        //            strclass.Append(key.KeyName + "=\"+" + key.KeyName);
        //        }

        //    }
        //    return strclass.Value;
        //}




    }

}
