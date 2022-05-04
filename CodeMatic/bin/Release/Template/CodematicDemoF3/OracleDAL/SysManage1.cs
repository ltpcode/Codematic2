using System;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using <$$namespace$$>.IDAL;
using Maticsoft.DBUtility;
namespace <$$namespace$$>.OracleDAL
{
	/// <summary>
	/// 用参数方式实现数据层示例。
	/// </summary>
    public class SysManage1 : ISysManage//这里必须实现接口，否则工厂类创建接口报错
	{
		public SysManage1()
		{			
		}		
        /// <summary>
        /// 得到最大编号
        /// </summary>
        /// <returns></returns>
		public int GetMaxId()
		{          
			return DbHelperOra.GetMaxID("NodeID", "S_Tree");           
		}
        
        public int AddTreeNode(<$$namespace$$>.Model.SysNode model)
		{
			model.NodeID=GetMaxId();

			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into S_Tree(");
			strSql.Append("NodeID,Text,ParentID,Location,OrderID,comment,Url,PermissionID,ImageUrl)");
			strSql.Append(" values (");
			strSql.Append("@NodeID,@Text,@ParentID,@Location,@OrderID,@comment,@Url,@PermissionID,@ImageUrl)");
				
			//
			OracleParameter[] parameters = {
											new OracleParameter("@NodeID", OracleType.Number,4),
											new OracleParameter("@Text", OracleType.VarChar,100),
											new OracleParameter("@ParentID", OracleType.Int32,4),										
											new OracleParameter("@Location", OracleType.VarChar,50),
											new OracleParameter("@OrderID", OracleType.Int32,4),
											new OracleParameter("@comment", OracleType.VarChar,50),
											new OracleParameter("@Url", OracleType.VarChar,100),
											new OracleParameter("@PermissionID", OracleType.Int32,4),
											new OracleParameter("@ImageUrl", OracleType.VarChar,100)};
			parameters[0].Value = model.NodeID;
			parameters[1].Value = model.Text;
			parameters[2].Value = model.ParentID;		
			parameters[3].Value = model.Location;
			parameters[4].Value = model.OrderID;
			parameters[5].Value = model.Comment;
			parameters[6].Value = model.Url;
			parameters[7].Value = model.PermissionID;
			parameters[8].Value = model.ImageUrl;
		
			DbHelperOra.ExecuteSql(strSql.ToString(),parameters);
			return model.NodeID;
		}


        public void UpdateNode(<$$namespace$$>.Model.SysNode model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update S_Tree set ");
			strSql.Append("Text=@Text,");
			strSql.Append("ParentID=@ParentID,");
			strSql.Append("Location=@Location,");
			strSql.Append("OrderID=@OrderID,");
			strSql.Append("comment=@comment,");
			strSql.Append("Url=@Url,");
			strSql.Append("PermissionID=@PermissionID,");
			strSql.Append("ImageUrl=@ImageUrl");
			strSql.Append(" where NodeID=@NodeID");

			OracleParameter[] parameters = {
											new OracleParameter("@NodeID", OracleType.Int32,4),
											new OracleParameter("@Text", OracleType.VarChar,100),
											new OracleParameter("@ParentID", OracleType.Int32,4),										
											new OracleParameter("@Location", OracleType.VarChar,50),
											new OracleParameter("@OrderID", OracleType.Int32,4),
											new OracleParameter("@comment", OracleType.VarChar,50),
											new OracleParameter("@Url", OracleType.VarChar,100),
											new OracleParameter("@PermissionID", OracleType.Int32,4),
											new OracleParameter("@ImageUrl", OracleType.VarChar,100)};
			parameters[0].Value = model.NodeID;
			parameters[1].Value = model.Text;
			parameters[2].Value = model.ParentID;		
			parameters[3].Value = model.Location;
			parameters[4].Value = model.OrderID;
			parameters[5].Value = model.Comment;
			parameters[6].Value = model.Url;
			parameters[7].Value = model.PermissionID;
			parameters[8].Value = model.ImageUrl;

			DbHelperOra.ExecuteSql(strSql.ToString(),parameters);

		}

		public void DelTreeNode(int NodeID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete S_Tree ");
			strSql.Append(" where NodeID=@NodeID");
			
			OracleParameter[] parameters = {
											new OracleParameter("@NodeID", OracleType.Int32,4)
										};
			parameters[0].Value = NodeID;	

			DbHelperOra.ExecuteSql(strSql.ToString(),parameters);
		}		


		public DataSet GetTreeList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * from S_Tree ");	
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by parentid,orderid ");

			return DbHelperOra.Query(strSql.ToString());
		}

		
		/// <summary>
		/// 得到菜单节点
		/// </summary>
		/// <param name="NodeID"></param>
		/// <returns></returns>
        public <$$namespace$$>.Model.SysNode GetNode(int NodeID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * from S_Tree ");	
			strSql.Append(" where NodeID=@NodeID");
			
			OracleParameter[] parameters = {
											new OracleParameter("@NodeID", OracleType.Int32,4)
										};
			parameters[0].Value = NodeID;

            <$$namespace$$>.Model.SysNode node = new <$$namespace$$>.Model.SysNode();
			DataSet ds=DbHelperOra.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				node.NodeID=int.Parse(ds.Tables[0].Rows[0]["NodeID"].ToString());
				node.Text=ds.Tables[0].Rows[0]["text"].ToString();
				if(ds.Tables[0].Rows[0]["ParentID"].ToString()!="")
				{
					node.ParentID=int.Parse(ds.Tables[0].Rows[0]["ParentID"].ToString());
				}
				node.Location=ds.Tables[0].Rows[0]["Location"].ToString();
				if(ds.Tables[0].Rows[0]["OrderID"].ToString()!="")
				{
					node.OrderID=int.Parse(ds.Tables[0].Rows[0]["OrderID"].ToString());
				}
				node.Comment=ds.Tables[0].Rows[0]["comment"].ToString();
				node.Url=ds.Tables[0].Rows[0]["url"].ToString();
				if(ds.Tables[0].Rows[0]["PermissionID"].ToString()!="")
				{
					node.PermissionID=int.Parse(ds.Tables[0].Rows[0]["PermissionID"].ToString());
				}
				node.ImageUrl=ds.Tables[0].Rows[0]["ImageUrl"].ToString();	
								
				return node;
			}
			else
			{
				return null;
			}

			
		}

		#region 日志
		/// <summary>
		/// 增加日志
		/// </summary>
		/// <param name="time"></param>
		/// <param name="loginfo"></param>
		public void AddLog(string time,string loginfo,string Particular)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into S_Log(");
			strSql.Append("datetime,loginfo,Particular)");
			strSql.Append(" values (");
			strSql.Append("'"+time+"',");
			strSql.Append("'"+loginfo+"',");	
			strSql.Append("'"+Particular+"'");	
			strSql.Append(")");						
			DbHelperOra.ExecuteSql(strSql.ToString());			
		}
		public void DeleteLog(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete S_Log ");	
			strSql.Append(" where ID= "+ID);
			DbHelperOra.ExecuteSql(strSql.ToString());
		}
		public void DelOverdueLog(int days)
		{			
			string str=" DATEDIFF(day,[datetime],getdate())>"+days;
			DeleteLog(str);
		}
		public void DeleteLog(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete S_Log ");	
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			DbHelperOra.ExecuteSql(strSql.ToString());
		}
		public DataSet GetLogs(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * from S_Log ");	
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by ID DESC");
			return DbHelperOra.Query(strSql.ToString());
		}
		public DataRow GetLog(string ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * from S_Log ");				
			strSql.Append(" where ID= "+ID);
			return DbHelperOra.Query(strSql.ToString()).Tables[0].Rows[0];
		}
		#endregion


	}
}
