using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using Maticsoft.CodeHelper;
namespace Maticsoft.IDBO
{
	/// <summary>
	/// IDbScriptBuilder 的摘要说明。
	/// </summary>
	public interface IDbScriptBuilder
	{

		#region 属性
		string DbConnectStr
		{
			get;
			set;
		}

		string DbName
		{
			get;
			set;
		}

		string TableName
		{
			get;
			set;
		}
        	

        string ProcPrefix
        {
            set;
            get;
        }       
        string ProjectName
        {
            set;
            get;
        }
        /// <summary>
        /// 选择的字段集合
        /// </summary>
        List<ColumnInfo> Fieldlist
        {
            set;
            get;
        }
        /// <summary>
        /// 选择的字段集合字符串
        /// </summary>
        string Fields
        {
            get;
        }
        /// <summary>
        /// 主键或条件字段
        /// </summary>
        List<ColumnInfo> Keys
        {
            set;
            get;
        }
		#endregion
	

		#region 生成数据库表创建脚本
        /// <summary>
        /// 生成数据库所有表的创建脚本
        /// </summary>
        /// <returns></returns>
        string CreateDBTabScript(string dbname);
		/// <summary>
		/// 生成数据库表创建脚本
		/// </summary>
		/// <returns></returns>
		string CreateTabScript(string dbname,string tablename);

        /// <summary>
        /// 根据SQL查询结果 生成数据创建脚本
        /// </summary>
        /// <returns></returns>
        string CreateTabScriptBySQL(string dbname, string strSQL);
        /// <summary>
        /// 生成数据库表创建脚本到文件
        /// </summary>
        /// <param name="dbname"></param>
        /// <param name="tablename"></param>
        /// <param name="filename"></param>
        /// <param name="progressBar"></param>
		void CreateTabScript(string dbname,string tablename,string filename,System.Windows.Forms.ProgressBar progressBar);
        		
		#endregion

		#region 创建存储过程

		string CreatPROCGetMaxID();
        string CreatPROCIsHas();
		string CreatPROCADD();
		string CreatPROCUpdate();
        string CreatPROCDelete();
        string CreatPROCGetModel();
		string CreatPROCGetList();
		
		/// <summary>
		/// 得到某个表的存储过程（选择生成的方法）
		/// </summary>
		/// <param name="Maxid"></param>
		/// <param name="Ishas"></param>
		/// <param name="Add"></param>
		/// <param name="Update"></param>
		/// <param name="Delete"></param>
		/// <param name="GetModel"></param>
		/// <param name="List"></param>
		/// <param name="dtColumn">表的所有列信息</param>
		/// <returns></returns>
        string GetPROCCode(bool Maxid, bool Ishas, bool Add, bool Update, bool Delete, bool GetModel, bool List);
		
		/// <summary>
		/// 得到某个表的存储过程
		/// </summary>
		/// <param name="dbname">库名</param>
		/// <param name="tablename">表名</param>
		/// <returns></returns>
		string GetPROCCode(string dbname,string tablename);
		/// <summary>
		/// 得到一个库下所有表的存储过程
		/// </summary>
		/// <param name="DbName"></param>
		/// <returns></returns>
		string GetPROCCode(string dbname);
		#endregion

        #region 生成SQL查询语句

        /// <summary>
        /// 生成Select查询语句
        /// </summary>
        /// <param name="dbname">库名</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        string GetSQLSelect(string dbname, string tablename);
       

        /// <summary>
        /// 生成update查询语句
        /// </summary>
        /// <param name="dbname">库名</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        string GetSQLUpdate(string dbname, string tablename);
        
        /// <summary>
        /// 生成update查询语句
        /// </summary>
        /// <param name="dbname">库名</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        string GetSQLDelete(string dbname, string tablename);

        /// <summary>
        /// 生成INSERT查询语句
        /// </summary>
        /// <param name="dbname">库名</param>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        string GetSQLInsert(string dbname, string tablename);
        #endregion
	}
}
