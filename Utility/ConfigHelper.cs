using System;
using System.Configuration;

namespace Maticsoft.Utility
{
	/// <summary>
	/// web.config操作类
	/// </summary>
	public sealed class ConfigHelper
	{

		#region 得到AppSettings中的配置字符串信息
		/// <summary>
		/// 得到AppSettings中的配置字符串信息
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetConfigString(string key)
		{
            return ConfigurationManager.AppSettings[key];
		}
		#endregion

		#region 得到AppSettings中的配置bool信息
		/// <summary>
		/// 得到AppSettings中的配置bool信息
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool GetConfigBool(string key)
		{
			bool result = false;
			string cfgVal = GetConfigString(key);
			if(null != cfgVal && string.Empty != cfgVal)
			{
				try
				{
					result = bool.Parse(cfgVal);
				}
				catch(FormatException)
				{
					// Ignore format exceptions.
				}
			}

			return result;
		}
		#endregion

		#region 得到AppSettings中的配置decimal信息
		/// <summary>
		/// 得到AppSettings中的配置decimal信息
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static decimal GetConfigDecimal(string key)
		{
			decimal result = 0;
			string cfgVal = GetConfigString(key);
			if(null != cfgVal && string.Empty != cfgVal)
			{
				try
				{
					result = decimal.Parse(cfgVal);
				}
				catch(FormatException)
				{
					// Ignore format exceptions.
				}
			}

			return result;
		}
		#endregion

		#region 得到AppSettings中的配置int信息
		/// <summary>
		/// 得到AppSettings中的配置int信息
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static int GetConfigInt(string key)
		{
			int result = 0;
			string cfgVal = GetConfigString(key);
			if(null != cfgVal && string.Empty != cfgVal)
			{
				try
				{
					result = int.Parse(cfgVal);
				}
				catch(FormatException)
				{
					// Ignore format exceptions.
				}
			}

			return result;
		}
		#endregion

	}
}
