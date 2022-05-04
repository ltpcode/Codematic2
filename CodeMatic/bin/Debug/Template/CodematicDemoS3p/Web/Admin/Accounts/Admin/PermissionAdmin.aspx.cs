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

namespace Maticsoft.Web.Accounts.Admin
{
	/// <summary>
	/// PermissionAdmin 的摘要说明。
	/// </summary>
	public partial class PermissionAdmin : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{				
				SetStyle();
				ImageButton btn = (ImageButton)Page.FindControl("BtnDelCategory");
				btn.Attributes.Add("onclick", "return confirm('你是否确定删除这条记录？');");
				CategoriesDatabind();
				if(this.ClassList.SelectedItem!=null)
				{
					PermissionsDatabind();
				}
			}
        }


        #region 初始化
        private void SetStyle()
		{
			DataGrid1.BorderWidth=Unit.Pixel(1);
			DataGrid1.CellPadding=4;
			DataGrid1.CellSpacing=0;
			DataGrid1.BorderColor=ColorTranslator.FromHtml(Application[Session["Style"].ToString()+"xtable_bordercolorlight"].ToString());
			DataGrid1.HeaderStyle.BackColor=ColorTranslator.FromHtml(Application[Session["Style"].ToString()+"xtable_titlebgcolor"].ToString());
				
		}

		private void CategoriesDatabind()
		{
			DataSet CategoriesList=AccountsTool.GetAllCategories();
			this.ClassList.DataSource=CategoriesList;
			this.ClassList.DataTextField="Description";
			this.ClassList.DataValueField="CategoryID";
			this.ClassList.DataBind();
		}

		private void PermissionsDatabind()
		{
			int CategoryId=int.Parse(this.ClassList.SelectedValue);
			DataSet PermissionsList=AccountsTool.GetPermissionsByCategory(CategoryId);
			this.DataGrid1.DataSource=PermissionsList;
			this.DataGrid1.DataBind();
        }

        #endregion

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
			this.BtnAddCategory.Click += new System.Web.UI.ImageClickEventHandler(this.BtnAddCategory_Click);
			this.BtnDelCategory.Click += new System.Web.UI.ImageClickEventHandler(this.BtnDelCategory_Click);
			this.BtnAddPermissions.Click += new System.Web.UI.ImageClickEventHandler(this.BtnAddPermissions_Click);
			this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
			this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);
            this.DataGrid1.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_CancelCommand);
            this.DataGrid1.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_UpdateCommand);
            this.DataGrid1.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_EditCommand);
			

		}
		#endregion

		protected void ClassList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PermissionsDatabind();
        }

        #region 按钮事件
private void BtnAddCategory_Click(object sender, System.Web.UI.ImageClickEventArgs e)
{
	string Category=this.CategoriesName.Text.Trim();
	if(Category!="")
	{
		PermissionCategories c=new PermissionCategories();
		c.Create(Category);
		CategoriesDatabind();
		if(this.ClassList.SelectedItem!=null)
		{
			PermissionsDatabind();
		}
		this.CategoriesName.Text="";
	}
	else
	{
		this.lbltip1.Text="名称不能为空！";
	}
}

		private void BtnDelCategory_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			int CategoryId=int.Parse(this.ClassList.SelectedValue);
			PermissionCategories c=new PermissionCategories();
			c.Delete(CategoryId);
			CategoriesDatabind();
			if(this.ClassList.SelectedItem!=null)
			{
				PermissionsDatabind();
			}
		
		}

		private void BtnAddPermissions_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{		
			string Permissions=this.PermissionsName.Text.Trim();
			if(Permissions!="")
			{
				int CategoryId=int.Parse(this.ClassList.SelectedValue);
				Permissions p=new Permissions();
				p.Create(CategoryId,Permissions);
				if(this.ClassList.SelectedItem!=null)
				{
					PermissionsDatabind();
				}
				this.PermissionsName.Text="";
			}
			else
			{
				this.lbltip2.Text="名称不能为空！";
			}

        }

        #endregion

        #region DataGrid事件
        private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			switch(e.Item.ItemType)
			{
				case ListItemType.Item:
				case ListItemType.AlternatingItem:
				case ListItemType.EditItem:
					ImageButton btn = (ImageButton)e.Item.FindControl("btnDelete");
					btn.Attributes.Add("onclick", "return confirm('你是否确定删除这条记录？');");
					break;
    
			}

		}

		private void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			string c=e.CommandName;
			int PermissionsID =(int)this.DataGrid1.DataKeys[e.Item.ItemIndex];
			string Permissions=e.Item.Cells[1].Text.Trim();
			switch(c)
			{
				case "Delete":
					if(Permissions!="帐户管理")
					{
						Permissions p=new Permissions();
						p.Delete(PermissionsID);
						PermissionsDatabind();
					}
					else
					{
						Response.Write("<script>alert('对不起,不能删除这个权限,否则将无法进行权限管理!');</script>");
					}
					break;
				case "Edit":
                    //this.TabEdit.Visible=true;
                    //this.lblPermId.Text=PermissionsID.ToString();
                    //this.txtNewName.Text=Permissions;
					break;
 
			}

		}

        private void DataGrid1_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string id = e.Item.Cells[0].Text;
            TextBox bTextBox = (TextBox)(e.Item.Cells[1].Controls[0]);
            string Permissions = bTextBox.Text.Trim();
            if ((Permissions != "") && (id != ""))
            {                 
                Permissions p = new Permissions();
                p.Update(int.Parse(id), Permissions);                
            }
            //恢复状态
            DataGrid1.EditItemIndex = -1;
            PermissionsDatabind();
        }
        private void DataGrid1_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            DataGrid1.EditItemIndex = e.Item.ItemIndex;
            PermissionsDatabind();
        }

        private void DataGrid1_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            DataGrid1.EditItemIndex = -1;
            PermissionsDatabind();

        }
        #endregion

    }
}
