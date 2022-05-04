using System;
using System.Text;

namespace Maticsoft.Utility
{
	/// <summary>
	///string操作类。
	/// </summary>
    public class StringPlus
	{
		StringBuilder str;
		public string Value
		{			
			get
            {
                return str.ToString();
            }
		}
		public StringPlus()
		{
			str=new StringBuilder();
		}

		#region 增加Tab空格缩进或间隔
		/// <summary>
		/// 增加Tab空格缩进或间隔
		/// </summary>
		/// <param name="num">间隔数</param>
		/// <returns></returns>
		public string Space(int SpaceNum)
		{			
			StringBuilder strspace=new StringBuilder();
			for(int n=0;n<SpaceNum;n++)
			{
				strspace.Append("\t");
			}
			return strspace.ToString();
		}
		#endregion

		#region 增加文本
		/// <summary>
		/// 增加文本
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public string Append(string Text)
		{			
			str.Append(Text);
			return str.ToString();
		}
		#endregion
	
		#region 追加一行文本，带回车换行。
		/// <summary>
		/// 追加回车换行。
		/// </summary>		
		public string AppendLine()
		{			
			str.Append("\r\n");
			return str.ToString();
		}
        /// <summary>
        /// 追加一行文本，带回车换行。
        /// </summary>
        /// <param name="Text">文本</param>
        /// <returns></returns>
        public string AppendLine(string Text)
        {
            str.Append(Text + "\r\n");
            return str.ToString();
        }
		#endregion

		#region 追加一行文本，前面加空格缩进，后面带回车换行。

        /// <summary>
        /// 追加一行文本，前面加空格缩进
        /// </summary>
        /// <param name="SpaceNum">空格缩进数目</param>
        /// <param name="Text">文本</param>
        /// <returns></returns>
        public string AppendSpace(int SpaceNum, string Text)
        {
            str.Append(Space(SpaceNum));
            str.Append(Text);           
            return str.ToString();
        }
		/// <summary>
		/// 追加一行文本，前面加空格缩进，后面带回车换行。
		/// </summary>
		/// <param name="SpaceNum">空格缩进数目</param>
		/// <param name="Text">文本</param>
		/// <returns></returns>
		public string AppendSpaceLine(int SpaceNum,string Text)
		{			
			str.Append(Space(SpaceNum));
			str.Append(Text);
			str.Append("\r\n");
			return str.ToString();
		}
		public override string ToString()
		{
			return str.ToString();
		}
		
		#endregion

        #region 删除字符
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public void DelLastComma()
        {
            string strtemp = str.ToString();            
            int n=strtemp.LastIndexOf(",");
            if (n > 0)
            {
                str = new StringBuilder();
                str.Append(strtemp.Substring(0, n));
            }            
        }

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public void DelLastChar(string strchar)
        {
            string strtemp = str.ToString();
            int n = strtemp.LastIndexOf(strchar);
            if (n > 0)
            {
                str = new StringBuilder();
                str.Append(strtemp.Substring(0, n));
            }
        }

    
        /// <summary>
        /// 删除指定位置的字符
        /// </summary>
        /// <param name="Start">开始索引</param>
        /// <param name="Num">删除个数</param>
        public void Remove(int Start, int Num)
        {
            //string strtemp = str.ToString();
            //str = new StringBuilder();
            //str.Append(strtemp.Substring(0, strtemp.LastIndexOf(strchar)));
            str.Remove(Start, Num);
        }

        #endregion

    }
}
