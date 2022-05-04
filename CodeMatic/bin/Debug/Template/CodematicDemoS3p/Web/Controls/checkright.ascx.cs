using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LTP.Accounts.Bus;
using System.Configuration;
using System.Web.Security;

namespace Maticsoft.Web.Controls
{
    public partial class CheckRight : System.Web.UI.UserControl
    {
        public int PermissionID = -1;
        protected void Page_Load(object sender, System.EventArgs e)
        {
        }

        #region Web 窗体设计器生成的代码
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }
        private void InitializeComponent()
        {
            if (!Page.IsPostBack)
            {
                string virtualPath = ConfigurationManager.AppSettings.Get("VirtualPath");
                string loginPage = ConfigurationManager.AppSettings.Get("LoginPage");
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
                    Response.Write("<script defer>window.alert('您没有权限进入本页或当前登录用户已过期！\\n请重新登录或与管理员联系！');parent.location='" + virtualPath + "/" + loginPage + "';</script>");
                    Response.End();
                }

            }
        }
        #endregion
    }
}