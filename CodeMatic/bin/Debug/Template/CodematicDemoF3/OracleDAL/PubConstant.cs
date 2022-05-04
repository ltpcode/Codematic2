using System;
using System.Configuration;
namespace <$$namespace$$>.OracleDAL
{

    public class PubConstant
    {
        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            string connectionString = ConfigurationManager.AppSettings[configName];
            string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
            if (ConStringEncrypt == "true")
            {
                connectionString = Maticsoft.Common.DEncrypt.DESEncrypt.Decrypt(connectionString);
            }
            return connectionString;
        }

    }
}
