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
	/// userinfo 的摘要说明。
	/// </summary>
	public partial class userinfo : System.Web.UI.Page
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
					this.lblTruename.Text=currentUser.TrueName;
					this.lblSex.Text=currentUser.Sex;
					this.lblPhone.Text=currentUser.Phone;
					this.lblEmail.Text=currentUser.Email;

                    lblUserIP.Text = Request.UserHostAddress;

                    //if(currentUser.DepartmentID=="-1")
                    //{
                    //    string herosoftmana=Maticsoft.Common.ConfigHelper.GetConfigString("AdManager");
                    //    this.lblDepart.Text=herosoftmana;
                    //}
                    //else
                    //{
						
                    //        if(Maticsoft.Common.PageValidate.IsNumber(currentUser.DepartmentID))
                    //        {
                    //            Maticsoft.BLL.ADManage.AdSupplier supp=new Maticsoft.BLL.ADManage.AdSupplier();
                    //            Maticsoft.Model.ADManage.AdSupplier suppmodel=supp.GetModel(int.Parse(currentUser.DepartmentID));
                    //            this.lblDepart.Text=suppmodel.SupplierName;
                    //            this.lblModeys.Text=suppmodel.Moneys.ToString();
                    //        }
						
						
                    //}
					switch(currentUser.Style)
					{
						case 1:
							this.lblStyle.Text="默认蓝";
							break;
						case 2:
							this.lblStyle.Text="橄榄绿";
							break;
						case 3:
							this.lblStyle.Text="深红";
							break;
						case 4:
							this.lblStyle.Text="深绿";
							break;
					}
					


//					if(user.Roles.Count>0)
//					{
//						RoleList.Visible = true;
//						ArrayList roles = user.Roles;
//						RoleList.Text = "角色列表：<ul>";
//						for(int i=0;i<roles.Count;i++)
//						{
//							RoleList.Text+="<li>" + roles[i] + "</li>";
//						}
//						RoleList.Text += "</ul>";
//					}



//					if(user.Permissions.Count>0)
//					{
//						RoleList.Visible = true;
//						ArrayList Permissions = user.Permissions;
//						RoleList.Text = "权限列表：<ul>";
//						for(int i=0;i<Permissions.Count;i++)
//						{
//							RoleList.Text+="<li>" + Permissions[i] + "</li>";
//						}
//						RoleList.Text += "</ul>";
//					}




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
	}
}
