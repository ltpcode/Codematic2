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

namespace Maticsoft.Web.Accounts
{
	/// <summary>
	/// userPass 的摘要说明。
	/// </summary>
	public partial class userPass : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{
				if (Context.User.Identity.IsAuthenticated)
				{					
					AccountsPrincipal user=new AccountsPrincipal(Context.User.Identity.Name);
					User currentUser=new LTP.Accounts.Bus.User(user);
					this.lblName.Text=currentUser.UserName;					
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

		}
		#endregion

		protected void btnAdd_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid) 
			{			
				SiteIdentity SID=new SiteIdentity(User.Identity.Name);
				if(SID.TestPassword(txtOldPassword.Text)==0)					
				{			
					this.lblMsg.ForeColor=Color.Red;
					this.lblMsg.Text = "原密码输入错误！";
				}
				else
					if(this.txtPassword.Text.Trim()!=this.txtPassword1.Text.Trim())
				{
					this.lblMsg.ForeColor=Color.Red;
					this.lblMsg.Text="密码输入的不一致！请重试！";
				}
				else
				{
					AccountsPrincipal user=new AccountsPrincipal(Context.User.Identity.Name);
					User currentUser=new LTP.Accounts.Bus.User(user);
				
					currentUser.Password=AccountsPrincipal.EncryptPassword(txtPassword.Text);					

					if (!currentUser.Update())
					{
						this.lblMsg.ForeColor=Color.Red;
						this.lblMsg.Text = "更新用户信息发生错误！";
                        //日志
                        //UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "用户密码更新失败");
					}
					else 
					{
						this.lblMsg.ForeColor=Color.Blue;
						this.lblMsg.Text = "用户信息更新成功！";
                        //日志
                        //UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "用户密码更新成功");
					}
                    
				}
			}

		
		}

		protected void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.txtOldPassword.Text="";
			this.txtPassword.Text="";
			this.txtPassword1.Text="";
		}
	}
}
