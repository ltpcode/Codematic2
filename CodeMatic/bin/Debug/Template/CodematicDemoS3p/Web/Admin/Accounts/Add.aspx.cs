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
	/// Add 的摘要说明。
	/// </summary>
	public partial class Add : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlInputButton btnCancel;
		public string adminname="管理部门";
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{		
                //BindSuppData();				
			}
		}
        //private void BindSuppData()
        //{
        //    Maticsoft.BLL.ADManage.AdSupplier adsupp=new Maticsoft.BLL.ADManage.AdSupplier();
        //    this.Dropdepart.DataSource=adsupp.GetNameList();
        //    this.Dropdepart.DataTextField="SupplierName";
        //    this.Dropdepart.DataValueField="SupplierID";
        //    this.Dropdepart.DataBind();
        //    adminname=Maticsoft.Common.ConfigHelper.GetConfigString("AdManager");
        //    this.Dropdepart.Items.Insert(0,new ListItem(adminname,"-1"));
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
			User newUser = new User();
			string strErr="";
//			if(this.Dropdepart.SelectedIndex==0)
//			{
//				strErr+="请选择部门!";				
//			}
			if(newUser.HasUser(txtUserName.Text))
			{
				strErr+="该用户名已存在！";
			}

			if(strErr!="")
			{
				Maticsoft.Common.MessageBox.Show(this,strErr);
				return;
			}			
			newUser.UserName=txtUserName.Text;
			newUser.Password=AccountsPrincipal.EncryptPassword(txtPassword.Text);
			newUser.TrueName=txtTrueName.Text;
			if(RadioButton1.Checked)
				newUser.Sex="男";
			else
				newUser.Sex="女";

			newUser.Phone=this.txtPhone.Text.Trim();
			newUser.Email=txtEmail.Text;
			newUser.EmployeeID=0;
            //newUser.DepartmentID=this.Dropdepart.SelectedValue;
			newUser.Activity=true;
            newUser.UserType = "AA";
			newUser.Style=1;
			int userid=newUser.Create();		
			if (userid == -100)
			{
				this.lblMsg.Text = "该用户名已存在！";
				this.lblMsg.Visible = true;
			}
			else 
			{
				Response.Redirect("Admin/RoleAssignment.aspx?UserID="+userid);
			}
		
		}

		
	}
}
