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
using System.Drawing;
namespace Maticsoft.Web.Accounts.Admin
{
    public partial class UserType : System.Web.UI.Page
    {
        LTP.Accounts.Bus.UserType ut = new LTP.Accounts.Bus.UserType();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetStyle();                
                BindUserType();                
            }
        }
        private void SetStyle()
        {
            DataGrid1.BorderWidth = Unit.Pixel(1);
            DataGrid1.CellPadding = 4;
            DataGrid1.CellSpacing = 0;
            DataGrid1.BorderColor = ColorTranslator.FromHtml(Application[Session["Style"].ToString() + "xtable_bordercolorlight"].ToString());
            DataGrid1.HeaderStyle.BackColor = ColorTranslator.FromHtml(Application[Session["Style"].ToString() + "xtable_titlebgcolor"].ToString());

        }
        private void BindUserType()
        {
            DataSet typelist = ut.GetAllList();
            this.DataGrid1.DataSource = typelist;
            this.DataGrid1.DataBind();
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
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
            this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);
            this.DataGrid1.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_CancelCommand);
            this.DataGrid1.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_UpdateCommand);
            this.DataGrid1.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_EditCommand);


        }
        #endregion

        #region DataGrid事件
        private void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            switch (e.Item.ItemType)
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
            string c = e.CommandName;
            string UserType = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
            //string Permissions = e.Item.Cells[1].Text.Trim();
            switch (c)
            {
                case "Delete":
                    {
                        ut.Delete(UserType);
                        BindUserType();
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
            string usertype = e.Item.Cells[0].Text;
            TextBox bTextBox = (TextBox)(e.Item.Cells[1].Controls[0]);
            string desc = bTextBox.Text.Trim();
            if ((desc != "") && (usertype != ""))
            {
                ut.Update(usertype, desc);
            }
            //恢复状态
            DataGrid1.EditItemIndex = -1;
            BindUserType();
        }
        private void DataGrid1_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            DataGrid1.EditItemIndex = e.Item.ItemIndex;
            BindUserType();
        }

        private void DataGrid1_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            DataGrid1.EditItemIndex = -1;
            BindUserType();

        }
        #endregion

        protected void BtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            string usertype = txtUserType.Text.Trim();
            string desc = txtDescription.Text.Trim();

            ut.Add(usertype,desc);
            BindUserType();
        }

        
    }
}
