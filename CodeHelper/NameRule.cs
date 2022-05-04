using System;
using System.Collections.Generic;
using System.Text;

namespace Maticsoft.CodeHelper
{
    /// <summary>
    /// 命名规则处理
    /// </summary>
    public class NameRule
    {      

        #region 

        /// <summary>
        /// 组合得到Model类名
        /// </summary>
        /// <param name="TabName">表名</param>
        /// <returns></returns>
        public static string GetModelClass(string TabName, Maticsoft.CmConfig.DbSettings dbset)
        {
            return dbset.ModelPrefix + TabNameRuled(TabName, dbset) + dbset.ModelSuffix;
        }
        /// <summary>
        /// 组合得到BLL类名
        /// </summary>
        /// <param name="TabName"></param>
        /// <returns></returns>
        public static string GetBLLClass(string TabName, Maticsoft.CmConfig.DbSettings dbset)
        {
            return dbset.BLLPrefix + TabNameRuled(TabName, dbset) + dbset.BLLSuffix;
        }

        /// <summary>
        /// 组合得到DAL类名
        /// </summary>
        /// <param name="TabName"></param>
        /// <returns></returns>
        public static string GetDALClass(string TabName, Maticsoft.CmConfig.DbSettings dbset)
        {
            return dbset.DALPrefix + TabNameRuled(TabName, dbset) + dbset.DALSuffix;
        }

        /// <summary>
        /// 表名处理规则
        /// </summary>
        /// <param name="TabName"></param>
        /// <param name="dbset"></param>
        /// <returns></returns>
        private static string TabNameRuled(string TabName, Maticsoft.CmConfig.DbSettings dbset)
        {
            string newTabName = TabName;
            if (dbset.ReplacedOldStr.Length > 0)
            {
                newTabName = newTabName.Replace(dbset.ReplacedOldStr, dbset.ReplacedNewStr);
            }
            switch (dbset.TabNameRule.ToLower())
            {
                case "lower":
                    newTabName = newTabName.ToLower();
                    break;
                case "upper":
                    newTabName = newTabName.ToUpper();
                    break;
                case "firstupper":
                    {
                        string strfir = newTabName.Substring(0, 1).ToUpper();
                        newTabName = strfir + newTabName.Substring(1);
                    }
                    break;
                case "same":
                    break;
                default:
                    break;
            }
            return newTabName;
        }
        #endregion
        

        
    }
}
