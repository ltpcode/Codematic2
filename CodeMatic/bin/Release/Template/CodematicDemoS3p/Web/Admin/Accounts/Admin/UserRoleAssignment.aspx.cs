using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LTP.Accounts.Bus;

namespace Maticsoft.Web.Accounts.Admin
{
    public partial class UserRoleAssignment : System.Web.UI.Page//Maticsoft.Web.Accounts.MoviePage
    {

        #region 初始化
        Maticsoft.BLL.Accounts_Users bll = new Maticsoft.BLL.Accounts_Users();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {                
                InitialDataBind();
                ddlUser_SelectedIndexChanged(sender, e);
            }
        }
        #endregion

        #region 功能函数

        #region 初始化数据绑定
        private void InitialDataBind()
        {
            DataSet ds;

            #region 绑定用户下拉列表
            string[] str = new string[3] { "AA", "AG", "PG" };

            User user = new User();

            this.ddlUser.Items.Clear();
            for (int i = 0; i < str.Length; i++)
            {
                ds = new DataSet();
                ds = user.GetUsersByType(str[i].ToString(), "");

                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    string userID = ds.Tables[0].Rows[j]["UserID"].ToString();
                    string userName = ds.Tables[0].Rows[j]["UserName"].ToString();

                    ListItem li = new ListItem(userName, userID);
                    this.ddlUser.Items.Add(li);
                }

                ds.Dispose();
            }
            #endregion

            #region 绑定角色
            int userid = Convert.ToInt32(this.ddlUser.SelectedValue);
            FillSelectedRoleList(userid);
            FillAllRoleList(userid);
            #endregion
        }
        #endregion

        //填充用户已有的角色
        private void FillSelectedRoleList(int userid)
        {
            this.SelectedRoleList.Items.Clear();

            string strWhere = " UserID=" + userid;

            DataSet ds = new DataSet();
            ds = bll.GetRolesByUser(userid);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string roleid = ds.Tables[0].Rows[i]["Roleid"].ToString();
                string description = ds.Tables[0].Rows[i]["Description"].ToString();

                ListItem li = new ListItem(description,roleid);
                this.SelectedRoleList.Items.Add(li);
            }

            ds.Dispose();

        }

        //填充用户没有的角色
        private void FillAllRoleList(int userid)
        {
            this.AllRoleList.Items.Clear();
           
            DataSet ds = new DataSet();
            ds = bll.GetRolesByNoUser(userid);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string roleid = ds.Tables[0].Rows[i]["Roleid"].ToString();
                string description = ds.Tables[0].Rows[i]["Description"].ToString();

                ListItem li = new ListItem(description, roleid);
                this.AllRoleList.Items.Add(li);
            }

            ds.Dispose();

        }

        #endregion

        protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            int userid = int.Parse(this.ddlUser.SelectedValue);
            FillSelectedRoleList(userid);
            FillAllRoleList(userid);
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.doing.Attributes.Add("display", "none");
            int userid = int.Parse(this.ddlUser.SelectedValue);
            string Idlist = "";
            int num = this.AllRoleList.Items.Count;

            for (int i = 0; i < num; i++)
            {
                if (this.AllRoleList.Items[i].Selected)
                {
                    int roleid = int.Parse(this.AllRoleList.Items[i].Value);
                    string description = this.AllRoleList.Items[i].Text;             

                    bll.Add(userid,roleid);
                    Idlist += roleid + ",";
                 
                }
            }


            #region 添加日志

            //获取当前用户及权限
            AccountsPrincipal user = new AccountsPrincipal(Context.User.Identity.Name);
            //获取当前用户
            User currentUser = new LTP.Accounts.Bus.User(user);
            try
            {

                UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "管理员端  | 系统管理 | 用户角色权函数限设置 |  要关联的用户ID： " + userid + " , 被设置的角色ID： " + Idlist);
            }
            catch
            {
                UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "管理员端  |  系统管理 | 用户角色权函数限设置 | 要关联的用户ID： " + userid + " , 被设置的角色ID " + Idlist + ", 添加日志失败");
            }


            #endregion

            ddlUser_SelectedIndexChanged(sender, e);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            this.doing.Attributes.Add("display", "none");
            int userid = int.Parse(this.ddlUser.SelectedValue);
            string Idlist = "";
            int num = this.SelectedRoleList.Items.Count;

            for (int i = 0; i < num; i++)
            {
                if (this.SelectedRoleList.Items[i].Selected)
                {
                    int roleid = int.Parse(this.SelectedRoleList.Items[i].Value);
                    string description = this.SelectedRoleList.Items[i].Text;
                    bll.Delete(userid, roleid);
                    Idlist += roleid + ",";

                }
            }


            #region 添加日志

            //获取当前用户及权限
            AccountsPrincipal user = new AccountsPrincipal(Context.User.Identity.Name);
            //获取当前用户
            User currentUser = new LTP.Accounts.Bus.User(user);
            try
            {

                UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "管理员端  |  系统管理 | 用户角色权函数限设置 | 要移除的用户ID： " + userid + " , 被移除的角色ID： " + Idlist);
            }
            catch
            {
                UserLog.AddLog(currentUser.UserName, currentUser.UserType, Request.UserHostAddress, Request.Url.AbsoluteUri, "管理员端  |  系统管理 | 用户角色权函数限设置 |  要移除的用户ID： " + userid + " , 被移除的角色ID： " + Idlist + ", 添加日志失败");
            }


            #endregion

            ddlUser_SelectedIndexChanged(sender, e);
        }
            
    }
}
