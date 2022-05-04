using System;
using Maticsoft.Model;

namespace Maticsoft.Web.SysManage
{
	/// <summary>
	/// show 的摘要说明。
	/// </summary>
	public partial class show : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label lblModuledept;
		protected System.Web.UI.WebControls.Label lblModule;
		protected System.Web.UI.WebControls.Label lblKeshiPublic;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				string id=Request.Params["id"];
				if(id==null || id.Trim()=="")
				{
					Response.Redirect("treelist.aspx");
					Response.End();
				}

				Navigation011.Para_Str="id="+id;
				Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();				
				SysNode node=sm.GetNode(int.Parse(id));
				lblID.Text=id;
				this.lblOrderid.Text=node.OrderID.ToString();
				lblName.Text=node.Text;			
				if(node.ParentID==0)
				{
					this.lblTarget.Text="根目录";
				}
				else
				{
					lblTarget.Text=sm.GetNode(node.ParentID).Text;			
				}
				lblUrl.Text=node.Url;
				lblImgUrl.Text=node.ImageUrl;
				LTP.Accounts.Bus.Permissions perm=new LTP.Accounts.Bus.Permissions();
				if(node.PermissionID==-1)
				{
					this.lblPermission.Text="没有权限限制";
				}
				else
				{
					this.lblPermission.Text=perm.GetPermissionName(node.PermissionID);
				}
				
				lblDescription.Text=node.Comment;
//				if(node.ModuleID!=-1)
//				{
//					this.lblModule.Text=sm.GetModuleName(node.ModuleID);
//				}
//				else
//				{
//					this.lblModule.Text="未归属任何模块";
//				}
//
//				if(node.KeShiDM!=-1)
//				{
//					this.lblModuledept.Text=Maticsoft.BLL.PubConstant.GetKeshiName(node.KeShiDM);
//				}
//				else
//				{
//					this.lblModuledept.Text="未归属任何部门";
//				}
//				if(node.KeshiPublic=="true")
//				{
//					this.lblKeshiPublic.Text="作为部门内部公有部分出现";
//				}

			}
		}

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
	}
}
