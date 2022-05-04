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
using System.Configuration;

namespace Maticsoft.Web.Accounts
{
	/// <summary>
	/// userupdate 的摘要说明。
	/// </summary>
	public partial class userupdate : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlInputText txtDate;
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack) 
			{
                //BindSuppData();								
				User currentUser;
				if(Request["userid"]!=null)
				{
					int userid=int.Parse(Request["userid"]);
					currentUser=new User(userid);					
					if(currentUser==null)
					{
						Response.Write("<script language=javascript>window.alert('该用户不存在！\\');history.back();</script>");
						return;
					}

					this.lblName.Text=currentUser.UserName;
					txtTrueName.Text=currentUser.TrueName;
					if(currentUser.Sex=="男")
						RadioButton1.Checked=true;
					else
						RadioButton2.Checked=true;
					this.txtPhone.Text=currentUser.Phone;
					txtEmail.Text=currentUser.Email;

					
                    //for(int i=0;i<this.Dropdepart.Items.Count;i++)
                    //{
                    //    if(this.Dropdepart.Items[i].Value==currentUser.DepartmentID)
                    //    {
                    //        this.Dropdepart.Items[i].Selected=true;
                    //    }
                    //}
					

					this.dropStyle.SelectedIndex=currentUser.Style-1;

					AccountsPrincipal user=new AccountsPrincipal(userid);
					BindRoles(user);

					
				}
			}
		}

		private void BindRoles(AccountsPrincipal user)
		{
			if(user.Permissions.Count>0)
			{
				RoleList.Visible = true;
				ArrayList Permissions = user.Permissions;
				RoleList.Text = "权限列表：<ul>";
				for(int i=0;i<Permissions.Count;i++)
				{
					RoleList.Text+="<li>" + Permissions[i] + "</li>";
				}
				RoleList.Text += "</ul>";
			}
		}
	
        //private void BindSuppData()
        //{
        //    Maticsoft.BLL.ADManage.AdSupplier adsupp=new Maticsoft.BLL.ADManage.AdSupplier();
        //    this.Dropdepart.DataSource=adsupp.GetNameList();
        //    this.Dropdepart.DataTextField="SupplierName";
        //    this.Dropdepart.DataValueField="SupplierID";
        //    this.Dropdepart.DataBind();
        //    string herosoftmana=Maticsoft.Common.ConfigHelper.GetConfigString("AdManager");
        //    this.Dropdepart.Items.Insert(0,new ListItem(herosoftmana,"-1"));
			
        //}

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
			string username=this.lblName.Text.Trim();				
			AccountsPrincipal user=new AccountsPrincipal(username);
			User currentUser=new LTP.Accounts.Bus.User(user);

			currentUser.UserName=username;
			currentUser.TrueName=txtTrueName.Text.Trim();
			currentUser.Password=AccountsPrincipal.EncryptPassword(txtPassword.Text);
			if(RadioButton1.Checked)
				currentUser.Sex="男";
			else
				currentUser.Sex="女";
			currentUser.Phone=this.txtPhone.Text.Trim();
			currentUser.Email=txtEmail.Text.Trim();
            //currentUser.EmployeeID=0;
            //currentUser.DepartmentID=this.Dropdepart.SelectedValue;			          
			int style=int.Parse(this.dropStyle.SelectedValue);
			currentUser.Style=style;
			if (!currentUser.Update())
			{
				this.lblMsg.ForeColor=Color.Red;
				this.lblMsg.Text = "更新用户信息发生错误！";
			}
			else 
			{
				Response.Redirect("Admin/useradmin.aspx");
			}			
		}





	}
}
