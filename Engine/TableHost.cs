using System;
using System.Collections.Generic;
using System.Text;
using Maticsoft.IDBO;
using Maticsoft.CodeHelper;
namespace Maticsoft.CodeEngine
{
    [Serializable]
    public class TableHost : DatabaseHost
    {
        public TableHost()
        {
        }
        public TableHost(string TableName)
        { 
        }
        
        private string _tablename;
        private string _tabledescription = "";
        private List<ColumnInfo> _keys; //主键或条件字段列表
        private List<ColumnInfo> _fkeys; //主键或条件字段列表
        private List<ColumnInfo> _fieldlist;
        private string _folder;//所在文件夹

        
        public string TableName
        {
            set { _tablename = value; }
            get { return _tablename; }
        }
        /// <summary>
        /// 表的描述信息
        /// </summary>
        public string TableDescription
        {
            set { _tabledescription = value; }
            get { return _tabledescription; }
        }
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        public List<ColumnInfo> Fieldlist
        {
            set { _fieldlist = value; }
            get { return _fieldlist; }
        }
        /// <summary>
        /// 主键集合
        /// </summary>
        public List<ColumnInfo> Keys
        {
            set { _keys = value; }
            get { return _keys; }
        }
        /// <summary>
        /// 外键集合
        /// </summary>
        public List<ColumnInfo> FKeys
        {
            set { _fkeys = value; }
            get { return _fkeys; }
        }
        public string Folder
        {
            set { _folder = value; }
            get { return _folder; }
        }

        /// <summary>
        /// 得到标识列
        /// </summary>
        public ColumnInfo IdentityKey
        {
            get
            {
                ColumnInfo idkey = null;
                foreach (ColumnInfo key in _keys)
                {                    
                    if (key.IsIdentity)
                    {
                        idkey= key;                        
                    }
                }
                return idkey;
            }            
        }



    }
}
