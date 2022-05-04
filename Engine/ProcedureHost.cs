using System;
using System.Collections.Generic;
using System.Text;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.CodeEngine
{
    [Serializable]
    public class ProcedureHost : TableHost
    {
        
        public string ProcedureName
        {
            get { return TableName; }
        }
        
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        public List<ColumnInfo> Parameterlist
        {
            get { return Fieldlist; }
        }       
       
        /// <summary>
        /// 输出参数
        /// </summary>
        public ColumnInfo OutParameter
        {
            get
            {
                ColumnInfo outparameter = null;
                foreach (ColumnInfo parameter in Parameterlist)
                {
                    if (parameter.Description == "isoutparam")
                    {
                        outparameter = parameter;
                    }
                }
                return outparameter;
            }
        }


        /// <summary>
        /// 组合得到BLL类名
        /// </summary>
        /// <param name="TabName"></param>
        /// <returns></returns>
        public string GetMethodName(string ProcedureName)
        {
            return ProcedureName;
        }


    }
}
