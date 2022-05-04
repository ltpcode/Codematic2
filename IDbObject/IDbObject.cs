using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Maticsoft.CodeHelper;
using System.Data.SqlClient;
namespace Maticsoft.IDBO
{
	/// <summary>
	/// 获取数据库信息类的接口定义。
	/// </summary>
	public interface IDbObject
	{
        /// <summary>
        ///  数据库类型
        /// </summary>
        string DbType
        {            
            get;
        }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        string DbConnectStr
		{
			set;get;
		}

		#region db操作

		int ExecuteSql(string DbName,string SQLString);
		DataSet Query(string DbName,string SQLString);

		#endregion
		
		#region 得到数据库的名字列表 GetDBList()
		/// <summary>
		/// 得到数据库的名字列表
		/// </summary>
		/// <returns></returns>
		List<string> GetDBList();
		#endregion

		#region  得到数据库的所有表名 GetTables(string DbName)
		/// <summary>
		/// 得到数据库的所有表名
		/// </summary>
		/// <param name="DbName"></param>
		/// <returns></returns>
        List<string> GetTables(string DbName);
		DataTable GetVIEWs(string DbName);
        /// <summary>
        /// 得到数据库的所有表和视图名字
        /// </summary>
        /// <param name="DbName">数据库</param>
        /// <returns></returns>
        List<string> GetTableViews(string DbName);
		/// <summary>
		/// 得到数据库的所有表和视图名
		/// </summary>
		/// <param name="DbName">数据库</param>
		/// <returns></returns>
		DataTable GetTabViews(string DbName);
        List<string> GetProcs(string DbName);
		#endregion
		
		#region  得到数据库的所有表的详细信息 GetTablesInfo(string DbName)

		/// <summary>
		/// 得到数据库的所有表的详细信息
		/// </summary>
		/// <param name="DbName">数据库</param>
		/// <returns></returns>
        List<TableInfo> GetTablesInfo(string DbName);
        DataTable GetTablesExProperty(string DbName);
        List<TableInfo> GetVIEWsInfo(string DbName);
		/// <summary>
		/// 得到数据库的所有表和视图的详细信息
		/// </summary>
		/// <param name="DbName">数据库</param>
		/// <returns></returns>
        List<TableInfo> GetTabViewsInfo(string DbName);
        List<TableInfo> GetProcInfo(string DbName);        
		#endregion

        #region 得到对象定义语句
        /// <summary>
        /// 得到视图或存储过程的定义语句
        /// </summary>  
        string GetObjectInfo(string DbName, string objName);
        #endregion

        #region 得到(快速)数据库里表的列名和类型 GetColumnList(string DbName,string TableName)
        /// <summary>
		/// 得到数据库里表的列名和类型
		/// </summary>
		/// <param name="DbName"></param>
		/// <param name="TableName"></param>
		/// <returns></returns>
        List<ColumnInfo> GetColumnList(string DbName, string TableName);
		#endregion

		#region 得到数据库里表的列的详细信息 GetColumnInfoList(string DbName,string TableName)
		/// <summary>
		/// 得到数据库里表的列的详细信息
		/// </summary>
		/// <param name="DbName"></param>
		/// <param name="TableName"></param>
		/// <returns></returns>
        List<ColumnInfo> GetColumnInfoList(string DbName, string TableName);
		#endregion

		#region 得到数据库里表的主键 GetKeyName(string DbName,string TableName)
		
        /// <summary>
		/// 得到数据库里表的主键
		/// </summary>
		/// <param name="DbName"></param>
		/// <param name="TableName"></param>
		/// <returns></returns>
		DataTable GetKeyName(string DbName,string TableName);

        List<ColumnInfo> GetKeyList(string DbName, string TableName);
		#endregion


        #region 得到数据库里表的外键 GetFKeyList

        List<ColumnInfo> GetFKeyList(string DbName, string TableName);
        #endregion



		#region 得到数据表里的数据 GetTabData(string DbName,string TableName)

		/// <summary>
		/// 得到数据表里的数据
		/// </summary>
		/// <param name="DbName"></param>
		/// <param name="TableName"></param>
		/// <returns></returns>
		DataTable GetTabData(string DbName,string TableName);

		#endregion

		#region 数据库属性操作

		/// <summary>
		/// 修改数据库名称
		/// </summary>
		/// <param name="OldName"></param>
		/// <param name="NewName"></param>
		/// <returns></returns>
		bool RenameTable(string DbName,string OldName,string NewName);
		/// <summary>
		/// 删除表
		/// </summary>	
		bool DeleteTable(string DbName,string TableName);

		string GetVersion();

		#endregion
	}
}
