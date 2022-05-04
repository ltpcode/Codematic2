using System;
using System.Runtime.InteropServices;
using System.Text;
namespace Maticsoft.Utility
{
    /// <summary>
    /// INIFile 的摘要说明。
    /// </summary>
    public class INIFile
    {
        public string path;

        public INIFile(string INIPath)
        {
            path = INIPath;
        }

        #region 声明

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);
        #endregion


        #region  写INI

        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }
        #endregion

        #region 删除ini配置

        /// <summary>
        /// 删除ini文件下所有段落
        /// </summary>
        public void ClearAllSection()
        {
            IniWriteValue(null, null, null);
        }
        /// <summary>
        /// 删除ini文件下personal段落下的所有键
        /// </summary>
        /// <param name="Section"></param>
        public void ClearSection(string Section)
        {
            IniWriteValue(Section, null, null);
        }
        #endregion

        #region 读取INI
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
            return temp.ToString();
        }
        public byte[] IniReadValues(string section, string key)
        {
            byte[] temp = new byte[255];
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.path);
            return temp;
        }
        /// <summary>
        /// 读取ini文件的所有段落名
        /// </summary>    
        public string[] IniReadValues()
        {
            byte[] allSection = IniReadValues(null, null);
            return ByteToString(allSection);

        }
        /// <summary>
        /// 转换byte[]类型为string[]数组类型 
        /// </summary>
        /// <param name="sectionByte"></param>
        /// <returns></returns>
        private string[] ByteToString(byte[] sectionByte)
        {                  
            ASCIIEncoding ascii = new ASCIIEncoding();           
            //编码所有key的string类型
            string sections = ascii.GetString(sectionByte);
            //获取key的数组
            string[] sectionList = sections.Split(new char[1] { '\0' });
            return sectionList;
        }

        /// <summary>
        /// 读取ini文件的某段落下所有键名
        /// </summary>    
        public string[] IniReadValues(string Section)
        {
            byte[] sectionByte = IniReadValues(Section, null);
            return ByteToString(sectionByte);
        }

        #endregion

    }


}
