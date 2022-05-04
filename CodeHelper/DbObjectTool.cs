using System;
using System.Collections.Generic;
using System.Text;

namespace Maticsoft.CodeHelper
{
    /// <summary>
    /// 数据库对象工具
    /// </summary>
    public class DbObjectTool
    {
        /// <summary>
        /// 字段的默认值处理为c#的格式和方法
        /// </summary>
        /// <param name="DefaultVal"></param>
        /// <returns></returns>
        public static string DefaultValToCS(string DefaultVal)
        {
            string newDefaultVal = DefaultVal;
            if (DefaultVal.Substring(0, 2) == "N'")
            { 
            }
            if (DefaultVal == "N'")
            {
            }
            return newDefaultVal;
        }
    }
}
