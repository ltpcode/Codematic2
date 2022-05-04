using System;
using System.Collections.Generic;
using System.Text;

namespace Maticsoft.CodeHelper
{
    /// <summary>
    /// 生成的代码信息
    /// </summary>
    [Serializable]
    public class CodeInfo
    {
        private string _code;
        private string _errormsg;
        private string _fileExtensionValue = ".cs";

                
        public string Code
        {
            set { _code = value; }
            get { return _code; }
        }
        public string ErrorMsg
        {
            set { _errormsg = value; }
            get { return _errormsg; }
        }
        /// <summary>
        /// 输出的文件扩展名
        /// </summary>
        public string FileExtension
        {
            set { _fileExtensionValue = value; }
            get { return _fileExtensionValue; }
        }
    }
}
