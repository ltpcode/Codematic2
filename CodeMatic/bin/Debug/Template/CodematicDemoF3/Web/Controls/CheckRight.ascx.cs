namespace <$$namespace$$>.Web.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using LTP.Accounts.Bus;
	using System.Configuration;
	using System.Web.Security;

	/// <summary>
	///	CheckRight 的摘要说明。
	/// </summary>
	public partial class CheckRight : System.Web.UI.UserControl
	{
		public int PermissionID=-1;
		protected void Page_Load(object sender, System.EventArgs e)
		{			
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
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            if (!Page.IsPostBack)
            {
                string virtualPath = ConfigurationManager.AppSettings.Get("VirtualPath");
                if (Context.User.Identity.IsAuthenticated)
                {
                    AccountsPrincipal user = new AccountsPrincipal(Context.User.Identity.Name);
                    if (Session["UserInfo"] == null)
                    {
                        LTP.Accounts.Bus.User currentUser = new LTP.Accounts.Bus.User(user);
                        Session["UserInfo"] = currentUser;
                        Session["Style"] = currentUser.Style;
                        Response.Write("<script defer>location.reload();</script>");
                    }
                    if ((PermissionID != -1) && (!user.HasPermissionID(PermissionID)))
                    {
                        Response.Clear();
                        Response.Write("<script defer>window.alert('您没有权限进入本页！\\n请重新登录或与管理员联系');history.back();</script>");
                        Response.End();
                    }

                }
                else
                {
                    FormsAuthentication.SignOut();
                    Session.Clear();
                    Session.Abandon();
                    Response.Clear();
                    Response.Write("<script defer>window.alert('您没有权限进入本页或当前登录用户已过期！\\n请重新登录或与管理员联系！');parent.location='" + virtualPath + "/Login.aspx';</script>");
                    Response.End();
                }

            }
		}
		#endregion
	}
}
