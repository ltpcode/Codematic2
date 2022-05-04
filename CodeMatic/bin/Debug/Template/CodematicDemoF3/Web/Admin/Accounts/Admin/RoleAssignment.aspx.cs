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
	/// RoleAssignment 的摘要说明。
	/// </summary>
	public partial class RoleAssignment : System.Web.UI.Page  //Maticsoft.Web.Accounts.MoviePage
	{
		private int userID;
		User currentUser;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{			
			string s=Request.Params["UserID"];
			userID=int.Parse(Request.Params["UserID"]);
			currentUser = new User(userID);

			Label1.Text="用户: "+currentUser.UserName+" 角色分配";
			if(!Page.IsPostBack)
			{
				DataSet dsRole=AccountsTool.GetRoleList();
				CheckBoxList1.DataSource=dsRole.Tables[0].DefaultView;
				CheckBoxList1.DataTextField="Description";
				CheckBoxList1.DataValueField="RoleID";
				CheckBoxList1.DataBind();

				AccountsPrincipal newUser = new AccountsPrincipal( currentUser.UserName );

				if ( newUser.Roles.Count > 0 )
				{
					ArrayList roles = newUser.Roles;
					for(int i=0; i<roles.Count; i++)
					{
//						RoleList.Text += "<li>" + roles[i] + "</li>";
						foreach(ListItem item in CheckBoxList1.Items)
						{
							if(item.Text==roles[i].ToString())item.Selected=true;
						}
					}
				}
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
			this.BtnOk.Click += new System.Web.UI.ImageClickEventHandler(this.BtnOk_Click);
			this.Btnback.Click += new System.Web.UI.ImageClickEventHandler(this.Btnback_Click);

		}
		#endregion

	

		private void BtnOk_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			foreach(ListItem item in CheckBoxList1.Items)
			{
				if(item.Selected==true)
				{
					currentUser.AddToRole(Convert.ToInt32(item.Value));
				}
				else
				{
					currentUser.RemoveRole(Convert.ToInt32(item.Value));
				}
			}
			Response.Redirect("UserAdmin.aspx?PageIndex="+Request.Params["PageIndex"]);
		
		}

		private void Btnback_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			Response.Redirect("UserAdmin.aspx?PageIndex="+Request.Params["PageIndex"]);
		
		}


		

	}
}
