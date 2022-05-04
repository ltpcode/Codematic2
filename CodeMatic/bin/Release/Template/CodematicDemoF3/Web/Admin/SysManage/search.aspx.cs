using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Maticsoft.Web.SysManage
{
	/// <summary>
	/// search 的摘要说明。
	/// </summary>
	public partial class search : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				//得到现有菜单
				BiudTree();	           

				//得到所有权限
				BiudPermTree();
			}
			
		}

		#region 
		
		private void BiudTree()
		{
//			Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();			
//			DataTable dt=sm.GetTreeList("").Tables[0];
//
//
//			this.listTarget.Items.Clear();
//			//加载树
//			this.listTarget.Items.Add(new ListItem("根目录","0"));
//			DataRow [] drs = dt.Select("ParentID= " + 0);			
//			foreach( DataRow r in drs )
//			{
//				string nodeid=r["NodeID"].ToString();				
//				string text=r["Text"].ToString();					
//				string parentid=r["ParentID"].ToString();
//				string permissionid=r["PermissionID"].ToString();
//				text="╋"+text;				
//				this.listTarget.Items.Add(new ListItem(text,nodeid));
//				int sonparentid=int.Parse(nodeid);
//				BindNode( sonparentid, dt);
//
//			}	
//			this.listTarget.DataBind();		
	
			Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();			
			DataTable dt=sm.GetTreeList("").Tables[0];


			this.listTarget.Items.Clear();
			//加载树
			this.listTarget.Items.Add(new ListItem("根目录","0"));
			DataRow [] drs = dt.Select("ParentID= " + 0);	
		

			foreach( DataRow r in drs )
			{
				string nodeid=r["NodeID"].ToString();				
				string text=r["Text"].ToString();					
				string parentid=r["ParentID"].ToString();
				string permissionid=r["PermissionID"].ToString();
				text="╋"+text;				
				this.listTarget.Items.Add(new ListItem(text,nodeid));
				int sonparentid=int.Parse(nodeid);
				string blank="├";
				
				BindNode( sonparentid, dt,blank);

			}	
			this.listTarget.DataBind();			

		}
		private void BindNode(int parentid,DataTable dt,string blank)
		{
			DataRow [] drs = dt.Select("ParentID= " + parentid );
			
			foreach( DataRow r in drs )
			{
				string nodeid=r["NodeID"].ToString();				
				string text=r["Text"].ToString();					
				string permissionid=r["PermissionID"].ToString();
				text=blank+"『"+text+"』";
				
				this.listTarget.Items.Add(new ListItem(text,nodeid));
				int sonparentid=int.Parse(nodeid);
				string blank2=blank+"─";
				

				BindNode( sonparentid, dt,blank2);
			}
		}

//		private void BindNode(int parentid,DataTable dt)
//		{
//			DataRow [] drs = dt.Select("ParentID= " + parentid );			
//			foreach( DataRow r in drs )
//			{
//				string nodeid=r["NodeID"].ToString();				
//				string text=r["Text"].ToString();					
//				string permissionid=r["PermissionID"].ToString();
//				text="  "+"├"+text;				
//				this.listTarget.Items.Add(new ListItem(text,nodeid));
//				int sonparentid=int.Parse(nodeid);
//				BindNode( sonparentid, dt);
//			}
//		}

		private void BiudPermTree()
		{

			DataTable tabcategory=LTP.Accounts.Bus.AccountsTool.GetAllCategories().Tables[0];					
			int rc=tabcategory.Rows.Count;
			for(int n=0;n<rc;n++)
			{
				string CategoryID=tabcategory.Rows[n]["CategoryID"].ToString();
				string CategoryName=tabcategory.Rows[n]["Description"].ToString();
				CategoryName="╋"+CategoryName;
				this.listPermission.Items.Add(new ListItem(CategoryName,CategoryID));

				DataTable tabforums=LTP.Accounts.Bus.AccountsTool.GetPermissionsByCategory(int.Parse(CategoryID)).Tables[0];
				int fc=tabforums.Rows.Count;
				for(int m=0;m<fc;m++)
				{
					string ForumID=tabforums.Rows[m]["PermissionID"].ToString();
					string ForumName=tabforums.Rows[m]["Description"].ToString();
					ForumName="  ├『"+ForumName+"』";
					this.listPermission.Items.Add(new ListItem(ForumName,ForumID));
				}
			}
			this.listPermission.DataBind();	
			this.listPermission.Items.Insert(0,"--请选择--");
		}


		#endregion


		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			string id=Maticsoft.Common.PageValidate.InputText(this.txtID.Text,10);
			string name=Maticsoft.Common.PageValidate.InputText(txtName.Text,10);			
			string comment=Maticsoft.Common.PageValidate.InputText(txtDescription.Text,100);
			string parentid=this.listTarget.SelectedValue;			
			string strsql=" (1=1) ";
			


			if(this.listPermission.SelectedItem.Text.StartsWith("╋"))
			{
				Response.Write("<script defer> window.alert('对不起,权限类别不能做权限使用！');</script>");
				return;
			}
			string permission_id="-1";
			if(this.listPermission.SelectedIndex>0)
			{
				permission_id=this.listPermission.SelectedValue;
			}
			if(id!="")
			{	
				try
				{
					int.Parse(id);
				}
				catch
				{
					Response.Write("<script defer> window.alert('对不起,编号格式不正确！');</script>");
					return;
				}
				strsql+=" and (NodeID="+id+")";
			}
			
			if(name!="")
			{				
				strsql+=" and (Text like'%"+name+"%')";
			}	

			if(parentid!="-1")
			{	
				strsql+=" and (ParentID="+parentid+")";
			}

			if(permission_id!="-1")
			{	
				strsql+=" and (PermissionID="+permission_id+")";
			}

			if(comment!="")
			{
				strsql+=" and (comment like'%"+comment+"%')";
			}

			if(strsql!="")
			{
				Session["strWheresys"]=strsql;
			}
			else
			{
				Session["strWheresys"]="";
			}
			Response.Redirect("treelist.aspx?page=1");
		
		}

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.txtID.Text="";
			this.txtName.Text="";			
			txtDescription.Text="";
			
		}
	}
}
