using System;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using Maticsoft.Model;
namespace Maticsoft.Web.SysManage
{
	/// <summary>
	/// modify 的摘要说明。
	/// </summary>
	public partial class modify : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DropDownList dropModule;
		protected System.Web.UI.WebControls.DropDownList Dropdepart;
		protected System.Web.UI.WebControls.CheckBox chkPublic;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				//得到现有菜单
				BiudTree();	           
				//得到所有权限
				BiudPermTree();

//				//绑定模块列表
//				BindModule();
////				BindModuledept();
//				 BindDept();

				BindImages();

				if(Request["id"]!=null)
				{
					string id=Request.Params["id"];
					if(id==null || id.Trim()=="")
					{
						Response.Redirect("treelist.aspx");
						Response.End();
					}
					else
					{
						ShowInfo(id);						
					}
				}
			}
			
		}

		private void ShowInfo(string id)
		{
			
			Navigation011.Para_Str="id="+id;
			Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();
			SysNode node=sm.GetNode(int.Parse(id));		
			
			this.lblID.Text=id;
			this.txtOrderid.Text=node.OrderID.ToString();
			this.txtName.Text=node.Text;
			//menu
			if(node.ParentID==0)
			{
				this.listTarget.SelectedIndex=0;
			}
			else
			{
				for(int m=0;m<this.listTarget.Items.Count;m++)
				{
					if(this.listTarget.Items[m].Value==node.ParentID.ToString())
					{
						this.listTarget.Items[m].Selected=true;
					}
				}
			}
			this.txtUrl.Text=node.Url;
//			this.txtImgUrl.Text=node.ImageUrl;
			this.txtDescription.Text=node.Comment;

			//Permission
			for(int n=0;n<this.listPermission.Items.Count;n++)
			{
				if((this.listPermission.Items[n].Value==node.PermissionID.ToString())&&(this.listPermission.Items[n].Value!="-1"))
				{
					this.listPermission.Items[n].Selected=true;
				}
			}

//			//module
//			for(int n=0;n<this.dropModule.Items.Count;n++)
//			{
//				if(this.dropModule.Items[n].Value==node.ModuleID.ToString())
//				{
//					this.dropModule.Items[n].Selected=true;
//				}
//			}
//
//			//module
//			for(int n=0;n<this.Dropdepart.Items.Count;n++)
//			{
//				if(this.Dropdepart.Items[n].Value==node.KeShiDM.ToString())
//				{
//					this.Dropdepart.Items[n].Selected=true;
//				}
//			}

			//image
			for(int n=0;n<this.imgsel.Items.Count;n++)
			{
				if(this.imgsel.Items[n].Value==node.ImageUrl)
				{
					this.imgsel.Items[n].Selected=true;
					this.hideimgurl.Value=node.ImageUrl;
				}
			}
//			if(node.KeshiPublic=="true")
//			{
//				this.chkPublic.Checked=true;
//			}
		}

		#region 
		//菜单
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
//		


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
				//string parentid=r["ParentID"].ToString();
				//string permissionid=r["PermissionID"].ToString();
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
				//string permissionid=r["PermissionID"].ToString();
				text=blank+"『"+text+"』";
				
				this.listTarget.Items.Add(new ListItem(text,nodeid));
				int sonparentid=int.Parse(nodeid);
				string blank2=blank+"─";
				

				BindNode( sonparentid, dt,blank2);
			}
		}
//
//		private void BindNode(int parentid,DataTable dt)
//		{
//			DataRow [] drs = dt.Select("ParentID= " + parentid );			
//			foreach( DataRow r in drs )
//			{
//				string nodeid=r["NodeID"].ToString();				
//				string text=r["Text"].ToString();					
//				string permissionid=r["PermissionID"].ToString();
//				text="  "+"├『"+text+"』";			
//				this.listTarget.Items.Add(new ListItem(text,nodeid));
//				int sonparentid=int.Parse(nodeid);
//				BindNode( sonparentid, dt);
//			}
//		}

		//权限
		private void BiudPermTree()
		{
			//				DataSet ds=LTP.Accounts.Bus.AccountsTool.GetAllPermissions();
			//				this.listPermission.Items.Clear();
			//				this.listPermission.DataSource=ds.Tables[1];			
			//				this.listPermission.DataTextField="Description";
			//				this.listPermission.DataValueField="PermissionID";
			//				this.listPermission.DataBind();
			//				this.listPermission.Items.Insert(0,"--请选择--");
							

			DataTable tabcategory=LTP.Accounts.Bus.AccountsTool.GetAllCategories().Tables[0];					
			int rc=tabcategory.Rows.Count;
			for(int n=0;n<rc;n++)
			{
				string CategoryID=tabcategory.Rows[n]["CategoryID"].ToString();
				string CategoryName=tabcategory.Rows[n]["Description"].ToString();
				CategoryName="╋"+CategoryName;
				this.listPermission.Items.Add(new ListItem(CategoryName,"-1"));

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


//		private void BindModule()
//		{
//			Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();
//			DataSet ds=sm.GetTypeModules("function");
//			this.dropModule.DataSource=ds;
//			this.dropModule.DataTextField="Description";
//			this.dropModule.DataValueField="ModuleID";
//			this.dropModule.DataBind();
//			this.dropModule.Items.Insert(0,"--请选择--");
//		}

//		private void BindModuledept()
//		{
//			Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();
//			DataSet ds=sm.GetTypeModules("dept");
//			this.dropModuleDept.DataSource=ds;
//			this.dropModuleDept.DataTextField="Description";
//			this.dropModuleDept.DataValueField="ModuleID";
//			this.dropModuleDept.DataBind();
//			this.dropModuleDept.Items.Insert(0,"--请选择--");
//		}

//		private void BindDept()
//		{
//			DataSet ds=Maticsoft.BLL.PubConstant.GetAllKeShi();
//			this.Dropdepart.DataSource=ds.Tables[0].DefaultView;
//			this.Dropdepart.DataTextField="KeShimch";
//			this.Dropdepart.DataValueField="KeShidm";
//			this.Dropdepart.DataBind();
//			this.Dropdepart.Items.Insert(0,"--请选择--");

//		}

		private void BindImages()
		{
			string dirpath=Server.MapPath("../Images/MenuImg");
			DirectoryInfo di = new DirectoryInfo(dirpath);  
			FileInfo[] rgFiles = di.GetFiles("*.gif");  
			this.imgsel.Items.Clear();
			foreach(FileInfo fi in rgFiles) 
			{ 
				ListItem item=new ListItem(fi.Name,"Images/MenuImg/"+fi.Name);
				this.imgsel.Items.Add(item);
			}
			FileInfo[] rgFiles2 = di.GetFiles("*.jpg"); 		
			foreach(FileInfo fi in rgFiles2) 
			{ 
				ListItem item=new ListItem(fi.Name,"Images/MenuImg/"+fi.Name);
				this.imgsel.Items.Add(item);
			}
			this.imgsel.Items.Insert(0,"默认图标");
			this.imgsel.DataBind();
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

		protected void btnAdd_Click(object sender, System.EventArgs e)
		{
			
			string id=Maticsoft.Common.PageValidate.InputText(this.lblID.Text,10);
			string orderid=Maticsoft.Common.PageValidate .InputText(this.txtOrderid.Text,5);
			string name=txtName.Text;
			string url=Maticsoft.Common.PageValidate.InputText(txtUrl.Text,100);
//			string imgUrl=Maticsoft.Common.PageValidate.InputText(txtImgUrl.Text,100);
			string imgUrl=this.hideimgurl.Value;
			string target=this.listTarget.SelectedValue;			
			int parentid=int.Parse(target);

			string strErr="";

			if(orderid.Trim()=="")
			{				
				strErr+="编号不能为空\\n";
				
			}
			try
			{
				int.Parse(orderid);
			}
			catch
			{				
				strErr+="编号格式不正确\\n";
				
			}
			if(name.Trim()=="")
			{				
				strErr+="名称不能为空\\n";			
			}

			if(this.listPermission.SelectedItem.Text.StartsWith("╋"))
			{				
				strErr+="权限类别不能做权限使用\\n";					
			}

			if(strErr!="")
			{
				Maticsoft.Common.MessageBox.Show(this,strErr);
				return;
			}



			int permission_id=-1;
			if(this.listPermission.SelectedIndex>0)
			{
				permission_id=int.Parse(this.listPermission.SelectedValue);
			}
			int moduleid=-1;
//			if(this.dropModule.SelectedIndex>0)
//			{
//				moduleid=int.Parse(this.dropModule.SelectedValue);
//			}
//			int moduledeptid=-1;
//			if(this.dropModuleDept.SelectedIndex>0)
//			{
//				moduledeptid=int.Parse(this.dropModuleDept.SelectedValue);
//			}
			int keshidm=-1;
//			if(this.Dropdepart.SelectedIndex>0)
//			{
//				keshidm=int.Parse(this.Dropdepart.SelectedValue);
//			}
			string keshipublic="false";
//			if(this.chkPublic.Checked)
//			{
//				keshipublic="true";
//			}
			string comment=Maticsoft.Common.PageValidate.InputText(txtDescription.Text,100);
		
			SysNode node=new SysNode();
			node.NodeID=int.Parse(id);
			node.OrderID=int.Parse(orderid);
			node.Text=name;
			node.ParentID=parentid;
			node.Location=parentid+"."+orderid;	
			node.Comment=comment;
			node.Url=url;
			node.PermissionID=permission_id;
			node.ImageUrl=imgUrl;
			node.ModuleID=moduleid;
			node.KeShiDM=keshidm;
			node.KeshiPublic=keshipublic;

			Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();
			sm.UpdateNode(node);
			Response.Redirect("show.aspx?id="+id);

		}


		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.lblID.Text="";
			txtName.Text="";
			txtUrl.Text="";
//			txtImgUrl.Text="";
			txtDescription.Text="";
			
		}


	
	}
}
