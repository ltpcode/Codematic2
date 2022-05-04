using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using LTP.Accounts.Bus;

namespace Maticsoft.Web.Accounts.Admin
{
	/// <summary>
	/// Index 的摘要说明。
	/// </summary>
	public partial class RoleAdmin : System.Web.UI.Page//Maticsoft.Web.Accounts.MoviePage
	{

		private DataSet roles;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
//			AccountsPrincipal currentPrincipal=new AccountsPrincipal( Context.User.Identity.Name );

//			AccountsPrincipal currentPrincipal = (AccountsPrincipal)Context.User;
//			if (!currentPrincipal.HasPermission((int)AccountsPermissions.CreateRoles))
//			{
//				NewRoleButton.Visible = false;
//				NewRoleDescription.Visible = false;
//			}
//			else 
//			{
//				NewRoleButton.Visible = true;
//				NewRoleDescription.Visible = true;
//			}
			dataBind();
		}
		private void dataBind()
		{
			roles = AccountsTool.GetRoleList();
			RoleList.DataSource = roles.Tables["Roles"];
			RoleList.DataBind();
		}

		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			base.OnInit(e);
			InitializeComponent();
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    
			this.BtnAdd.Click += new System.Web.UI.ImageClickEventHandler(this.BtnAdd_Click);

		}
		#endregion


		private void BtnAdd_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Role role=new Role();
			role.Description=TextBox1.Text;
			try
			{
				role.Create();
			}
			catch{}
			TextBox1.Text="";
			dataBind();
		
		}

	}
}
