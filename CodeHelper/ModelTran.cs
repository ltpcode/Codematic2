using System;
using System.Collections.Generic;
using System.Text;

namespace Maticsoft.CodeHelper
{
    /// <summary>
    /// 事务操作对象
    /// </summary>
    public class ModelTran
    {
        private string dbName;
        private string tableName;
        private string modelName;
        private string action;        
        private List<ColumnInfo> _fieldlist;
        private List<ColumnInfo> _keys; // 主键或条件字段列表  


        /// <summary>
        /// 库名
        /// </summary>
        public string DbName
        {
            set { dbName = value; }
            get { return dbName; }
        }
        
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            set { tableName = value; }
            get { return tableName; }
        }

        /// <summary>
        /// 实体类名
        /// </summary>
        public string ModelName
        {
            set { modelName = value; }
            get { return modelName; }
        }

        /// <summary>
        /// 操作
        /// </summary>
        public string Action
        {
            set { action = value; }
            get { return action; } 
        }

        /// <summary>
        /// 选择要生成的字段集合
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }

        /// <summary>
        /// 主键或条件字段的集合
        /// </summary>
        public List<ColumnInfo> Keys
        {
            set { _keys = value; }
            get { return _keys; }
        }


    }
}
