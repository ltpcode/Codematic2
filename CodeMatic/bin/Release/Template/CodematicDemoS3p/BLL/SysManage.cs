using System.Data;
using Maticsoft.Model;
namespace Maticsoft.BLL
{
	/// <summary>
	/// 系统菜单管理。
	/// </summary>
	public class SysManage
	{
		Maticsoft.DAL.SysManage dal=new Maticsoft.DAL.SysManage();

        
		public SysManage()
		{			
		}
		
		public int AddTreeNode(SysNode node)
		{			
			return dal.AddTreeNode(node);
		}
		public void UpdateNode(SysNode node)
		{			
			dal.UpdateNode(node);
		}
		public void DelTreeNode(int nodeid)
		{			
			dal.DelTreeNode(nodeid);
		}

		public DataSet GetTreeList(string strWhere)
		{			
			return dal.GetTreeList(strWhere);
		}

		public SysNode GetNode(int NodeID)
		{			
			return dal.GetNode(NodeID);
		}

        public int GetPermissionCatalogID(int permissionID)
        {
            return dal.GetPermissionCatalogID(permissionID);
        }

        #region 日志管理
        public void AddLog(string time,string loginfo,string Particular)
		{			
			dal.AddLog(time,loginfo,Particular);
		}
		public void DelOverdueLog(int days)
		{						
			dal.DelOverdueLog(days);
		}
		public void DeleteLog(string Idlist)
		{			
			string str="";
			if(Idlist.Trim()!="")
			{
				str=" ID in ("+Idlist+")";
			}
			dal.DeleteLog(str);
		}
		public void DeleteLog(string timestart,string timeend)
		{			
			string str=" datetime>'"+timestart+"' and datetime<'"+timeend+"'";
			dal.DeleteLog(str);
		}
		public DataSet GetLogs(string strWhere)
		{			
			return dal.GetLogs(strWhere);
		}
		public DataRow GetLog(string ID)
		{			
			return dal.GetLog(ID);
        }

        #endregion


    }
}
