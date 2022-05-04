using System.Text;
using System.Data;
using <$$namespace$$>.IDAL;
using Maticsoft.DBUtility;
namespace <$$namespace$$>.OracleDAL
{
	/// <summary>
	/// 系统菜单管理类。(普通SQL实现方式)
	/// </summary>
    public class SysManage : ISysManage
	{
        //在这里可以更换数据库,支持多数据库，支持采用加密方式实现
        //DbHelperSQLP DbHelperOra = new DbHelperSQLP(PubConstant.GetConnectionString("ConnectionString2"));
		public SysManage()
		{            
		}
        
		public int GetMaxId()
		{
			string strsql="select max(NodeID)+1 from S_Tree";
			object obj=DbHelperOra.GetSingle(strsql);
			if(obj==null)
			{
				return 1;
			}
			else
			{
				return int.Parse(obj.ToString());
			}
		}

        public int AddTreeNode(<$$namespace$$>.Model.SysNode node)
		{
			node.NodeID=GetMaxId();

			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into S_Tree(");
			strSql.Append("NodeID,Text,ParentID,Location,OrderID,comment,Url,PermissionID,ImageUrl)");
			strSql.Append(" values (");
			strSql.Append("'"+node.NodeID+"',");
			strSql.Append("'"+node.Text+"',");
			strSql.Append(""+node.ParentID+",");			
			strSql.Append("'"+node.Location+"',");
			strSql.Append(""+node.OrderID+",");		
			strSql.Append("'"+node.Comment+"',");
			strSql.Append("'"+node.Url+"',");
			strSql.Append(""+node.PermissionID+",");
			strSql.Append("'"+node.ImageUrl+"'");
			strSql.Append(")");						
			DbHelperOra.ExecuteSql(strSql.ToString());
			return node.NodeID;

		}

        public void UpdateNode(<$$namespace$$>.Model.SysNode node)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update S_Tree set ");
			strSql.Append("Text='"+node.Text+"',");
			strSql.Append("ParentID="+node.ParentID.ToString()+",");
			strSql.Append("Location='"+node.Location+"',");
			strSql.Append("OrderID="+node.OrderID+",");
			strSql.Append("comment='"+node.Comment+"',");
			strSql.Append("Url='"+node.Url+"',");
			strSql.Append("PermissionID="+node.PermissionID+",");
			strSql.Append("ImageUrl='"+node.ImageUrl+"'");
			strSql.Append(" where NodeID="+node.NodeID);
			DbHelperOra.ExecuteSql(strSql.ToString());

		}

		public void DelTreeNode(int nodeid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete S_Tree ");
			strSql.Append(" where NodeID="+nodeid);					
			DbHelperOra.ExecuteSql(strSql.ToString());
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
			strSql.Append(" where NodeID="+NodeID);
            <$$namespace$$>.Model.SysNode node = new <$$namespace$$>.Model.SysNode();
            DataSet ds=DbHelperOra.Query(strSql.ToString());
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
