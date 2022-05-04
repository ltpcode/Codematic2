using System;
using System.Collections.Generic;
using System.Text;

namespace Maticsoft.CodeHelper
{
    /// <summary>
    /// 字段信息
    /// </summary>
    [Serializable]
    public class ColumnInfo
    {
        private string _colorder;
        private string _columnName;
        private string _typeName = "";
        private string _length = "";
        private string _precision = "";
        private string _scale = "";
        private bool _isIdentity;
        private bool _isprimaryKey;
        private bool _isForeignKey;
        private bool _nullable;
        private string _defaultVal = "";
        private string _description = "";

        /// <summary>
        /// 序号
        /// </summary>
        public string ColumnOrder
        {
            set { _colorder = value; }
            get { return _colorder; }
        }
        /// <summary>
        /// 字段名
        /// </summary>
        public string ColumnName
        {
            set { _columnName = value; }
            get { return _columnName; }
        }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string TypeName
        {
            set { _typeName = value; }
            get { return _typeName; }
        }
        /// <summary>
        /// 长度
        /// </summary>
        public string Length
        {
            set { _length = value; }
            get { return _length; }
        }
        /// <summary>
        /// 精度
        /// </summary>
        public string Precision
        {
            set { _precision = value; }
            get { return _precision; }
        }
        /// <summary>
        /// 小数位数
        /// </summary>
        public string Scale
        {
            set { _scale = value; }
            get { return _scale; }
        }
        /// <summary>
        /// 是否是标识列
        /// </summary>
        public bool IsIdentity
        {
            set { _isIdentity = value; }
            get { return _isIdentity; }
        }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPrimaryKey
        {
            set { _isprimaryKey = value; }
            get { return _isprimaryKey; }
        }
        /// <summary>
        /// 是否是外键
        /// </summary>
        public bool IsForeignKey
        {
            set { _isForeignKey = value; }
            get { return _isForeignKey; }
        }
        /// <summary>
        /// 是否允许空
        /// </summary>
        public bool Nullable
        {
            set { _nullable = value; }
            get { return _nullable; }
        }
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultVal
        {
            set { _defaultVal = value; }
            get { return _defaultVal; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }




    }
}
