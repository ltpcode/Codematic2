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
	/// Index 的摘要说明。
	/// </summary>
	public partial class Index :System.Web.UI.Page// Maticsoft.Web.Accounts.MoviePage
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{            			
			if (!Context.User.Identity.IsAuthenticated )
			{
				Session["message"]="你没有通过权限审核！";
				Session["returnPage"]=Request.RawUrl;
				Response.Redirect("../Login.aspx",true);
			}
            
            AccountsPrincipal user=new AccountsPrincipal(Context.User.Identity.Name);			
			if(!user.HasPermission("帐户管理"))
			{
				Session["message"]="你没有帐户管理的权限！";
				Session["returnPage"]=Request.RawUrl;
				Response.Redirect("../Login.aspx",true);
			}

//			int i=user.Roles.Count;
//			string s=user.Roles[0].ToString();
//			bool b=user.Roles.Contains("管理员");
//			i=user.Permissions.Count;
//			s=user.Permissions[0].ToString();
//			b=user.Permissions.Contains("帐户管理");



/*
			Context.User = new AccountsPrincipal(Context.User.Identity.Name);
			if(!((AccountsPrincipal)Context.User).HasPermission("帐户管理"))
			{
				Session["message"]="你没有帐户管理的权限！";
				Session["returnPage"]=Request.RawUrl;
				Response.Redirect("../Login.aspx",true);
			}
*/



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
