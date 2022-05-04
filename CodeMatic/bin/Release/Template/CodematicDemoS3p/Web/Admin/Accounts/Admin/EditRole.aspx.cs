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
    /// 编辑角色
    /// </summary>
    public partial class EditRole : System.Web.UI.Page
    {
        protected System.Web.UI.WebControls.Button Button1;
        private Role currentRole;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Button btn = (Button)Page.FindControl("RemoveRoleButton");
                btn.Attributes.Add("onclick", "return confirm('你是否确定删除该角色？');");
                DoInitialDataBind();
                CategoryDownList_SelectedIndexChanged(sender, e);

                DataGrid1.BorderWidth = Unit.Pixel(1);
                DataGrid1.CellPadding = 4;
                DataGrid1.CellSpacing = 0;
                DataGrid1.BorderColor = ColorTranslator.FromHtml(Application[Session["Style"].ToString() + "xtable_bordercolorlight"].ToString());
                DataGrid1.HeaderStyle.BackColor = ColorTranslator.FromHtml(Application[Session["Style"].ToString() + "xtable_titlebgcolor"].ToString());

                dataBind();
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
            this.BtnUpName.Click += new System.Web.UI.ImageClickEventHandler(this.BtnUpName_Click);
            this.DataGrid1.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemCreated);
            this.DataGrid1.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGrid1_ItemCommand);
            this.DataGrid1.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGrid1_PageIndexChanged);
            this.DataGrid1.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid1_ItemDataBound);


        }
        #endregion

        #region 角色列表
        //绑定类别列表
        private void DoInitialDataBind()
        {
            currentRole = new Role(Convert.ToInt32(Request["RoleID"]));
            RoleLabel.Text = currentRole.Description;
            this.TxtNewname.Text = currentRole.Description;

            DataSet allCategories = AccountsTool.GetAllCategories();
            CategoryDownList.DataSource = allCategories.Tables[0];
            CategoryDownList.DataTextField = "Description";
            CategoryDownList.DataValueField = "CategoryID";
            CategoryDownList.DataBind();
        }

        //选择类别，填充2个listbox
        protected void CategoryDownList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int categoryID = Convert.ToInt32(CategoryDownList.SelectedItem.Value);
            FillCategoryList(categoryID);
            SelectCategory(categoryID, false);
        }


        //填充非权限列表
        private void FillCategoryList(int categoryId)
        {
            currentRole = new Role(Convert.ToInt32(Request["RoleID"]));
            DataTable categories = currentRole.NoPermissions.Tables["Categories"];
            DataRow currentCategory = categories.Rows.Find(categoryId);
            if (currentCategory != null)
            {
                DataRow[] permissions = currentCategory.GetChildRows("PermissionCategories");
                CategoryList.Items.Clear();
                foreach (DataRow currentRow in permissions)
                {
                    CategoryList.Items.Add(
                        new ListItem((string)currentRow["Description"], Convert.ToString(currentRow["PermissionID"])));
                }
            }
        }

        //填充已有权限listbox
        private void SelectCategory(int categoryId, bool forceSelection)
        {
            currentRole = new Role(Convert.ToInt32(Request["RoleID"]));
            DataTable categories = currentRole.Permissions.Tables["Categories"];
            DataRow currentCategory = categories.Rows.Find(categoryId);

            if (currentCategory != null)
            {
                DataRow[] permissions = currentCategory.GetChildRows("PermissionCategories");

                PermissionList.Items.Clear();
                foreach (DataRow currentRow in permissions)
                {
                    PermissionList.Items.Add(
                        new ListItem((string)currentRow["Description"], Convert.ToString(currentRow["PermissionID"])));
                }
            }


        }
        #endregion

        #region 角色操作

        //增加权限
        protected void AddPermissionButton_Click(object sender, System.EventArgs e)
        {
            int[] items = CategoryList.GetSelectedIndices();
            if (items.Length > 0)
            {
                int currentRole = Convert.ToInt32(Request["RoleID"]);
                Role bizRole = new Role(currentRole);
                foreach (int i in items)
                {
                    int permid = Convert.ToInt32(this.CategoryList.Items[i].Value);
                    bizRole.AddPermission(permid);
                }
            }
            CategoryDownList_SelectedIndexChanged(sender, e);
        }

        //移除权限
        protected void RemovePermissionButton_Click(object sender, System.EventArgs e)
        {
            //if(this.PermissionList.SelectedIndex>-1)
            //{
            //    int currentRole = Convert.ToInt32(Request["RoleID"]);
            //    Role bizRole = new Role(currentRole);
            //    bizRole.RemovePermission( Convert.ToInt32(this.PermissionList.SelectedValue) );
            //    CategoryDownList_SelectedIndexChanged(sender,e);
            //}

            int[] items = PermissionList.GetSelectedIndices();
            if (items.Length > 0)
            {
                int currentRole = Convert.ToInt32(Request["RoleID"]);
                Role bizRole = new Role(currentRole);
                foreach (int i in items)
                {
                    int permid = Convert.ToInt32(this.PermissionList.Items[i].Value);
                    bizRole.RemovePermission(permid);
                }
            }
            CategoryDownList_SelectedIndexChanged(sender, e);
        }

        #endregion

        #region 角色修改

        protected void RemoveRoleButton_Click(object sender, System.EventArgs e)
        {
            int currentRole = Convert.ToInt32(Request["RoleID"]);
            Role bizRole = new Role(currentRole);
            bizRole.Delete();
            Server.Transfer("RoleAdmin.aspx");
        }

        private void BtnUpName_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string newname = this.TxtNewname.Text.Trim();
            currentRole = new Role(Convert.ToInt32(Request["RoleID"]));
            currentRole.Description = newname;
            currentRole.Update();
            DoInitialDataBind();
        }
        #endregion

        #region 绑定用户

        protected void dataBind()
        {
            currentRole = new Role(Convert.ToInt32(Request["RoleID"]));
            DataSet ds = new DataSet();
            ds = currentRole.Users;
            int pageIndex = this.DataGrid1.CurrentPageIndex;
            DataGrid1.DataSource = ds.Tables[0].DefaultView;
            int record_Count = ds.Tables[0].Rows.Count;
            int page_Size = DataGrid1.PageSize;
            int totalPages = int.Parse(Math.Ceiling((double)record_Count / page_Size).ToString());
            if (totalPages > 0)
            {
                if (pageIndex > totalPages - 1)
                    pageIndex = totalPages - 1;
            }
            else
            {
                pageIndex = 0;
            }
            DataGrid1.CurrentPageIndex = pageIndex;
            DataGrid1.DataBind();

            //显示数量
            if (this.DataGrid1.CurrentPageIndex == 0)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                if (this.DataGrid1.PageCount == 1)
                {
                    btnLast.Enabled = false;
                    btnNext.Enabled = false;
                }
            }
            else if (this.DataGrid1.CurrentPageIndex == this.DataGrid1.PageCount - 1)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
            this.lblpagesum.Text = totalPages.ToString();
            this.lblpage.Text = (pageIndex + 1).ToString();
            this.lblrowscount.Text = record_Count.ToString();
        }

        #endregion


        #region NavigateToPage
        protected string FormatString(string str)
        {
            if (str.Length > 10)
            {
                str = str.Substring(0, 9) + "...";
            }
            return str;
        }
        //导航按钮事件
        public void NavigateToPage(object sender, CommandEventArgs e)
        {
            btnFirst.Enabled = true;
            btnPrev.Enabled = true;
            btnNext.Enabled = true;
            btnLast.Enabled = true;
            string pageinfo = e.CommandArgument.ToString();
            switch (pageinfo)
            {
                case "Prev":
                    if (this.DataGrid1.CurrentPageIndex > 0)
                    {
                        this.DataGrid1.CurrentPageIndex -= 1;

                    }
                    break;
                case "Next":
                    if (this.DataGrid1.CurrentPageIndex < (this.DataGrid1.PageCount - 1))
                    {
                        this.DataGrid1.CurrentPageIndex += 1;

                    }
                    break;
                case "First":
                    this.DataGrid1.CurrentPageIndex = 0;
                    break;
                case "Last":
                    this.DataGrid1.CurrentPageIndex = this.DataGrid1.PageCount - 1;
                    break;
            }
            if (this.DataGrid1.CurrentPageIndex == 0)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                if (this.DataGrid1.PageCount == 1)
                {
                    btnLast.Enabled = false;
                    btnNext.Enabled = false;
                }
            }
            else if (this.DataGrid1.CurrentPageIndex == this.DataGrid1.PageCount - 1)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
            dataBind();
        }

        #endregion

        #region DataGrid1事件

        protected void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {

            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    ImageButton btn = (ImageButton)e.Item.FindControl("BtnDel");
                    btn.Attributes.Add("onclick", "return confirm('你是否确定删除这条记录？');");
                    string DepartmentID = (string)DataBinder.Eval(e.Item.DataItem, "DepartmentID");
                    string UserType = (string)DataBinder.Eval(e.Item.DataItem, "UserType");
                    if (DepartmentID == "-1")
                    {
                        string managename = "管理员";
                        e.Item.Cells[6].Text = managename;
                    }
                    else
                    {
                        switch (UserType)
                        {
                            case "AA":
                                break;
                            case "SU":
                                {
                                    //if (Maticsoft.Common.PageValidate.IsNumber(DepartmentID))
                                    //{
                                    //    e.Item.Cells[6].Text = supp.GetName(int.Parse(DepartmentID));
                                    //}
                                }
                                break;
                            default:
                                UserType = "其他";
                                break;
                        }

                    }
                    break;
            }

        }

        protected void DataGrid1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string btn = e.CommandName;
            switch (btn)
            {
                case "BtnEdit":
                    int userID1 = int.Parse(e.Item.Cells[9].Text.Trim());
                    Response.Redirect("../userupdate.aspx?userid=" + userID1);
                    break;
                case "BtnDel":
                    int userID2 = int.Parse(e.Item.Cells[9].Text.Trim());
                    User currentUser2 = new User(userID2);
                    currentUser2.Delete();
                    break;
            }
            dataBind();
        }


        protected void DataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
            dataBind();

        }

        protected void DataGrid1_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Pager)
            {
                foreach (Control c in e.Item.Cells[0].Controls)
                {
                    if (c is Label)
                    {
                        Label lblpage = (Label)c;
                        lblpage.ForeColor = System.Drawing.ColorTranslator.FromHtml("#FF0000");
                        lblpage.Font.Bold = true;
                        break;
                    }
                }
            }
        }

        #endregion

        protected void Button2_ServerClick(object sender, System.EventArgs e)
        {
            Response.Redirect("RoleAdmin.aspx");
        }





    }
}
