using System;
using LTP.Accounts.Bus;
using System.Web.Security;

namespace Maticsoft.Web.Admin
{
	/// <summary>
	/// Login 的摘要说明。
	/// </summary>
	public partial class Login : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面
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
			this.btnLogin.Click += new System.Web.UI.ImageClickEventHandler(this.btnLogin_Click);

		}
		#endregion

		private void btnLogin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
            if ((Session["PassErrorCountAdmin"] != null) && (Session["PassErrorCountAdmin"].ToString() != ""))
            {
                int PassErroeCount = Convert.ToInt32(Session["PassErrorCountAdmin"]);
                if (PassErroeCount > 3)
                {
                    txtUsername.Disabled = true;
                    txtPass.Disabled = true;
                    btnLogin.Enabled = false;
                    this.lblMsg.Text = "对不起，你错误登录了三次，系统登录锁定！";
                    return;
                }

            }

            if ((Session["CheckCode"] != null) && (Session["CheckCode"].ToString() != ""))
            {
                if (Session["CheckCode"].ToString().ToLower() != this.CheckCode.Value.ToLower())
                {
                    this.lblMsg.Text = "所填写的验证码与所给的不符 !";
                    Session["CheckCode"] = null;
                    return;
                }
                else
                {
                    Session["CheckCode"] = null;
                }
            }
            else
            {
                Response.Redirect("login.aspx");
            }

			string userName=Maticsoft.Common.PageValidate.InputText(txtUsername.Value.Trim(),30);
			string Password=Maticsoft.Common.PageValidate.InputText(txtPass.Value.Trim(),30);			

			AccountsPrincipal newUser = AccountsPrincipal.ValidateLogin(userName,Password);			
			if (newUser == null)
			{				
				this.lblMsg.Text = "登陆失败： " + userName;
                if ((Session["PassErrorCountAdmin"] != null) && (Session["PassErrorCountAdmin"].ToString() != ""))
                {
                    int PassErroeCount = Convert.ToInt32(Session["PassErrorCountAdmin"]);
                    Session["PassErrorCountAdmin"] = PassErroeCount + 1;
                }
                else
                {
                    Session["PassErrorCountAdmin"] = 1;
                }
			}
			else 
			{				
				User currentUser=new LTP.Accounts.Bus.User(newUser);
                //if (currentUser.UserType != "AA")
                //{
                //    this.lblMsg.Text = "你非管理员用户，你没有权限登录后台系统！";
                //    return;
                //}
				Context.User = newUser;
				if(((SiteIdentity)User.Identity).TestPassword( Password) == 0)
				{
					this.lblMsg.Text = "你的密码无效！";
                    if ((Session["PassErrorCountAdmin"] != null) && (Session["PassErrorCountAdmin"].ToString() != ""))
                    {
                        int PassErroeCount = Convert.ToInt32(Session["PassErrorCountAdmin"]);
                        Session["PassErrorCountAdmin"] = PassErroeCount + 1;
                    }
                    else
                    {
                        Session["PassErrorCountAdmin"] = 1;
                    }
				}
				else
				{					
					FormsAuthentication.SetAuthCookie( userName,false );
                    //日志
                    //UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "登录成功");
					
					Session["UserInfo"]=currentUser;
					Session["Style"]=currentUser.Style;
					if(Session["returnPage"]!=null)
					{
						string returnpage=Session["returnPage"].ToString();
						Session["returnPage"]=null;
						Response.Redirect(returnpage);
					}
					else
					{
						Response.Redirect("main.htm");
					}
				}
			}		
		}

       
	}
}
