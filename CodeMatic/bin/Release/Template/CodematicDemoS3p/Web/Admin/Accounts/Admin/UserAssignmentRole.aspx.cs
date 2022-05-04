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
    public partial class UserAssignmentRole : System.Web.UI.Page
    {
        //private int userID;
        User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadUsers();
                BindRoles();
            }
        }
        
        private void LoadUsers()
        {
            string usertype = "AA";
            User userAdmin = new User();
            DataSet ds = userAdmin.GetUsersByType(usertype, "");
            DropUserlist.DataSource = ds.Tables[0];
            DropUserlist.DataTextField = "UserName";
            DropUserlist.DataValueField = "UserID";
            DropUserlist.DataBind();
        }
        protected void DropUserlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropUserlist.SelectedItem != null)
            {
                BindRoles();
            }
        }
        private void BindRoles()
        {
            if (DropUserlist.SelectedItem == null)
            {
                return;
            }

            string UserName = DropUserlist.SelectedItem.Text;
            currentUser = new User(UserName);
            AccountsPrincipal newUser = new AccountsPrincipal(UserName);
            
            DataSet dsRole = AccountsTool.GetRoleList();
            chkboxRolelist.DataSource = dsRole.Tables[0].DefaultView;
            chkboxRolelist.DataTextField = "Description";
            chkboxRolelist.DataValueField = "RoleID";
            chkboxRolelist.DataBind();           

            if (newUser.Roles.Count > 0)
            {
                ArrayList roles = newUser.Roles;
                for (int i = 0; i < roles.Count; i++)
                {
                    //RoleList.Text += "<li>" + roles[i] + "</li>";
                    foreach (ListItem item in chkboxRolelist.Items)
                    {
                        if (item.Text == roles[i].ToString()) item.Selected = true;
                    }
                }
            }
 
        }

        public void BtnOk_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string UserName = DropUserlist.SelectedItem.Text;
            currentUser = new User(UserName);
            foreach (ListItem item in chkboxRolelist.Items)
            {
                if (item.Selected == true)
                {
                    currentUser.AddToRole(Convert.ToInt32(item.Value));
                }
                else
                {
                    currentUser.RemoveRole(Convert.ToInt32(item.Value));
                }
            }
            lblTip.Text = "±£´æ³É¹¦£¡";

        }


   
    }
}
