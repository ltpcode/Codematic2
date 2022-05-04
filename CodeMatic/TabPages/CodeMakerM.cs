using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Codematic.UserControls;
using Maticsoft.CodeBuild;
using Maticsoft.Utility;
using Maticsoft.CodeHelper;
using System.Threading;
using Maticsoft.AddInManager;
namespace Codematic
{
    /// <summary>
    /// 父子表代码生成
    /// </summary>
    public partial class CodeMakerM : Form
    {       
        private UcCodeView codeview;        
        Maticsoft.CmConfig.DbSettings dbset;
        Maticsoft.IDBO.IDbObject dbobj;
        Maticsoft.CodeBuild.CodeBuilders cb;//代码生成对象

        string servername;
        string dbname;
        string tablename;
        private Thread thread;
        delegate void SetListCallback();
        DALTypeAddIn cm_daltype;
        DALTypeAddIn cm_blltype;
        DALTypeAddIn cm_webtype;
        
        public CodeMakerM(string Dbname)
        {
            InitializeComponent();
            dbname = Dbname;     
            codeview = new UcCodeView();
            tabPage2.Controls.Add(codeview);
            SetListViewMenu("colum");
            CreatView();
            
            DbView dbviewfrm = (DbView)Application.OpenForms["DbView"];
            if (dbviewfrm != null)
            {
                try
                {
                    this.thread = new Thread(new ThreadStart(Showlistview));
                    this.thread.Start();
                }
                catch
                {
                }
            }
            
        }
        private void CodeMaker_Load(object sender, EventArgs e)
        {
            
        }

        void Showlistview()
        {
            DbView dbviewfrm = (DbView)Application.OpenForms["DbView"];            
            if (dbviewfrm.treeView1.InvokeRequired)
            {
                SetListCallback d = new SetListCallback(Showlistview);
                dbviewfrm.Invoke(d, null);
            }
            else
            {
                SetListView(dbviewfrm);
            }
        }


        #region 初始化窗体设置
        private void SetFormConfig(string servername)
        {
            //radbtn_Type_Click(null, null);
                        
            dbset = Maticsoft.CmConfig.DbConfig.GetSetting(servername);
            txtProjectName.Text = dbset.ProjectName;
            txtNameSpace.Text = dbset.Namepace;
            txtNameSpace2.Text = dbset.Folder;
            txtProcPrefix.Text = dbset.ProcPrefix;
            switch (dbset.AppFrame.ToLower())
            {
                case "one":
                    radbtn_Frame_One.Checked = true;
                    break;
                case "s3":
                    radbtn_Frame_S3.Checked = true;
                    break;
                case "f3":
                    radbtn_Frame_F3.Checked = true;
                    break;
            }
            radbtn_Type_Click(null, null);
            radbtn_Frame_Click(null, null);
            radbtn_F3_Click(null, null);
            
            #region 加载插件
            cm_daltype = new DALTypeAddIn("Maticsoft.IBuilder.IBuilderDALTran");
            cm_daltype.Title = "DAL";
            groupBox_DALType.Controls.Add(cm_daltype);
            cm_daltype.Location = new System.Drawing.Point(30, 18);
            
            cm_blltype = new DALTypeAddIn("Maticsoft.IBuilder.IBuilderBLL");
            cm_blltype.Title = "BLL";
            groupBox_DALType.Controls.Add(cm_blltype);
            cm_blltype.Location = new System.Drawing.Point(30, 18);
            
            cm_webtype = new DALTypeAddIn("Maticsoft.IBuilder.IBuilderWeb");
            cm_webtype.Title = "Web";
            groupBox_DALType.Controls.Add(cm_webtype);
            cm_webtype.Location = new System.Drawing.Point(30, 18);
            
            #endregion
            
            cm_daltype.SetSelectedDALType(dbset.DALType.Trim());
            cm_blltype.SetSelectedDALType(dbset.BLLType.Trim());
            cm_webtype.SetSelectedDALType(dbset.WebType.Trim()); 

            this.tabControl1.SelectedIndex = 0;
        }
                
        #endregion

        #region  选择设置

        #region 类型选择
        private void radbtn_Type_Click(object sender, EventArgs e)
        {
            if (this.radbtn_Type_DB.Checked)
            {
                this.groupBox_DB.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_DALType.Visible = false;
                this.groupBox_Method.Visible = false;
                this.groupBox_FrameSel.Visible = false;
                this.groupBox_F3.Visible = false;
                this.groupBox_Web.Visible = false;
            }
            if (this.radbtn_Type_CS.Checked)
            {
                this.groupBox_DB.Visible = false;
                this.groupBox_AppType.Visible = true;
                this.groupBox_DALType.Visible = true;
                this.groupBox_Method.Visible = true;
                this.groupBox_FrameSel.Visible = true;
                this.groupBox_F3.Visible = true;
                this.groupBox_Web.Visible = false;


                //this.groupBox_FrameSel.Top = 188;
                //this.groupBox_F3.Top = 235;
                //this.groupBox_DALType.Top = 286;
                //this.groupBox_Method.Top = 337;
                //this.groupBox_AppType.Top = 401;


                radbtn_Frame_Click(sender, e);

            }
            if (this.radbtn_Type_Web.Checked)
            {
                this.groupBox_DB.Visible = false;
                this.groupBox_AppType.Visible = false;
                this.groupBox_DALType.Visible = false;
                this.groupBox_Method.Visible = false;
                this.groupBox_FrameSel.Visible = false;
                this.groupBox_F3.Visible = false;
                this.groupBox_Web.Visible = true;
                
                cm_daltype.Visible = false;
                cm_blltype.Visible = false;
                cm_webtype.Visible = true;
                                
            }


        }
        #endregion

        #region 架构选择
        private void radbtn_Frame_Click(object sender, EventArgs e)
        {
            if (this.radbtn_Frame_One.Checked)
            {
                //this.groupBox_DALType.Top = 342;
                //this.groupBox_Method.Top = 393;

                this.groupBox_DB.Visible = false;
                this.groupBox_F3.Visible = false;
                this.groupBox_DALType.Visible = true;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;

            }
            if (this.radbtn_Frame_S3.Checked)
            {
                //this.groupBox_F3.Top = 343;
                //this.groupBox_DALType.Top = 393;
                //this.groupBox_Method.Top = 444;

                this.groupBox_DB.Visible = false;
                this.groupBox_F3.Visible = true;
                this.groupBox_DALType.Visible = true;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;

                this.radbtn_F3_IDAL.Visible = false;
                this.radbtn_F3_DALFactory.Visible = false;

                radbtn_F3_Click(sender, e);

            }
            if (this.radbtn_Frame_F3.Checked)
            {
                this.groupBox_DB.Visible = false;
                this.groupBox_F3.Visible = true;
                this.groupBox_DALType.Visible = true;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = true;
                this.groupBox_Web.Visible = false;

                this.radbtn_F3_IDAL.Visible = true;
                this.radbtn_F3_DALFactory.Visible = true;

                //this.groupBox_F3.Top = 343;
                //this.groupBox_DALType.Top = 393;
                //this.groupBox_Method.Top = 444;
                //this.groupBox_AppType.Top = 499;

                radbtn_F3_Click(sender, e);
            }

        }
        #endregion

        #region 层选择(工厂模式的)
        private void radbtn_F3_Click(object sender, EventArgs e)
        {
            if (this.radbtn_F3_Model.Checked)
            {
                this.groupBox_DALType.Visible = false;
                this.groupBox_Method.Visible = false;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;
            }
            if (this.radbtn_F3_DAL.Checked)
            {                
                this.groupBox_DALType.Visible = true;
                cm_daltype.Visible = true;
                cm_blltype.Visible = false;
                cm_webtype.Visible = false;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;
            }
            if (this.radbtn_F3_IDAL.Checked)
            {
                this.groupBox_DALType.Visible = false;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;
            }
            if (this.radbtn_F3_DALFactory.Checked)
            {
                this.groupBox_DALType.Visible = false;
                this.groupBox_Method.Visible = false;
                this.groupBox_AppType.Visible = true;
                this.groupBox_Web.Visible = false;
            }
            if (this.radbtn_F3_BLL.Checked)
            {
                this.groupBox_DALType.Visible = true;
                cm_daltype.Visible = false;
                cm_blltype.Visible = true;
                cm_webtype.Visible = false;
                this.groupBox_Method.Visible = true;
                this.groupBox_AppType.Visible = false;
                this.groupBox_Web.Visible = false;
            }
        }
        #endregion

        private void radbtn_DBSel_Click(object sender, EventArgs e)
        {
            if (this.radbtn_DB_Proc.Checked)
            {
                chk_DB_GetMaxID.Visible = true;
                chk_DB_Exists.Visible = true;
                chk_DB_Add.Visible = true;
                chk_DB_Update.Visible = true;
                chk_DB_Delete.Visible = true;
                chk_DB_GetModel.Visible = true;
                chk_DB_GetList.Visible = true;
                txtProcPrefix.Visible=true;
                txtTabname.Visible=true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                
            }
            else
            {
                chk_DB_GetMaxID.Visible = false;
                chk_DB_Exists.Visible = false;
                chk_DB_Add.Visible = false;
                chk_DB_Update.Visible = false;
                chk_DB_Delete.Visible = false;
                chk_DB_GetModel.Visible = false;
                chk_DB_GetList.Visible = false;
                txtProcPrefix.Visible = false;
                txtTabname.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label7.Visible = false;
            }
        }
        #endregion

        #region  绑定表信息
        private void BindTablist(string Dbname)
        {
            List<TableInfo> tablist = dbobj.GetTablesInfo(Dbname);
            if ((tablist!=null)&&(tablist.Count > 0))
            {
                cmbox_PTab.Items.Clear();
                cmbox_STab.Items.Clear();
                foreach (TableInfo tab in tablist)
                {
                    string name = tab.TabName;
                    cmbox_PTab.Items.Add(name);
                    cmbox_STab.Items.Add(name);
                }
            }            
        }
        private void cmbox_PTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cmbox_PTab.SelectedItem != null) && (cmbox_PTab.Text != "System.Data.DataRowView"))
            {
                string tabname = cmbox_PTab.Text;
                BindlistViewCol1(dbname, tabname);
            }

        }

        private void cmbox_STab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cmbox_STab.SelectedItem != null) && (cmbox_STab.Text != "System.Data.DataRowView"))
            {
                string tabname = cmbox_STab.Text;
                BindlistViewCol2(dbname, tabname);
            }
        }
        #endregion

        #region 设置listview

        public void SetListView(DbView dbviewfrm)
        {
            #region 得到类型对象

            TreeNode SelNode = dbviewfrm.treeView1.SelectedNode;
            if (SelNode == null)
                return;
            switch (SelNode.Tag.ToString())
            {
                case "server":
                    {
                        servername = SelNode.Text;
                    }
                    break;               
                case "table":
                    {
                        servername = SelNode.Parent.Parent.Parent.Text;
                        dbname = SelNode.Parent.Parent.Text;
                        tablename = SelNode.Text;
                        dbobj = ObjHelper.CreatDbObj(servername);                        
                        BindTablist(dbname);
                    }
                    break;
                case "view":
                    {
                        servername = SelNode.Parent.Parent.Parent.Text;
                        dbname = SelNode.Parent.Parent.Text;
                        tablename = SelNode.Text;
                        dbobj = ObjHelper.CreatDbObj(servername);                        
                        BindTablist(dbname);
                    }
                    break;
                case "column":
                    {
                        servername = SelNode.Parent.Parent.Parent.Parent.Text;
                        dbname = SelNode.Parent.Parent.Parent.Text;
                        dbobj = ObjHelper.CreatDbObj(servername);                        
                        BindTablist(dbname);

                    }
                    break;
                case "db":
                    {
                        servername = SelNode.Parent.Text;
                        dbname = SelNode.Text;
                        dbobj = ObjHelper.CreatDbObj(servername);                        
                        BindTablist(dbname);
                    }
                    break;
                default:
                    {
                        this.listView1.Items.Clear();
                        this.listView2.Items.Clear();
                    }
                    break;
            }
            
            if (dbobj != null)
            {
                cb = new CodeBuilders(dbobj);
                DataTable dtEx = dbobj.GetTablesExProperty(dbname);
                if (dtEx != null)
                {
                    try
                    {
                        DataRow[] drs = dtEx.Select("objname='" + tablename + "'");
                        if (drs.Length > 0)
                        {
                            if (drs[0]["value"] != null)
                            {
                                cb.TableDescription = drs[0]["value"].ToString();
                            }
                        }
                    }
                    catch
                    { }
                }
            }
            #endregion

            SetFormConfig(servername);
        }
        #endregion
        
        #region  为listView邦定 列 数据

        private void CreatView()
        {
            //创建列表
            this.listView1.Columns.Clear();
            this.listView1.Items.Clear();
            this.listView1.LargeImageList = imglistView;
            this.listView1.SmallImageList = imglistView;
            this.listView1.View = View.Details;
            this.listView1.GridLines = true;
            this.listView1.CheckBoxes = true;
            this.listView1.FullRowSelect = true;

            listView1.Columns.Add("选", 28, HorizontalAlignment.Left);
            listView1.Columns.Add("列名", 105, HorizontalAlignment.Left);
            listView1.Columns.Add("数据类型", 80, HorizontalAlignment.Left);

            //创建列表
            this.listView2.Columns.Clear();
            this.listView2.Items.Clear();
            this.listView2.LargeImageList = imglistView;
            this.listView2.SmallImageList = imglistView;
            this.listView2.View = View.Details;
            this.listView2.GridLines = true;
            this.listView2.CheckBoxes = true;
            this.listView2.FullRowSelect = true;

            listView2.Columns.Add("选", 28, HorizontalAlignment.Left);
            listView2.Columns.Add("列名", 105, HorizontalAlignment.Left);
            listView2.Columns.Add("数据类型", 80, HorizontalAlignment.Left);            
        }

        private void BindlistViewCol1(string Dbname, string TableName)
        {
            List<ColumnInfo> collist = dbobj.GetColumnInfoList(Dbname, TableName);

            if ((collist!=null)&&(collist.Count > 0))
            {
                this.listView1.Items.Clear();
                cmbox_PField.Items.Clear();
                foreach (ColumnInfo col in collist)
                {
                    string order = col.ColumnOrder;
                    string columnName = col.ColumnName;
                    string typename = col.TypeName;
                    this.cmbox_PField.Items.Add(columnName);

                    ListViewItem item1 = new ListViewItem(order, 0);
                    item1.Checked = true;
                    item1.ImageIndex = -1;
                    item1.SubItems.Add(columnName);
                    item1.SubItems.Add(typename);                    
                    listView1.Items.AddRange(new ListViewItem[] { item1 });

                }
                if (cmbox_PField.Items.Count > 0)
                {
                    cmbox_PField.SelectedIndex = 0;
                }              
            }

            txtTabname.Text = TableName;
            txtClassName.Text = TableName;

        }

        private void BindlistViewCol2(string Dbname, string TableName)
        {
            List<ColumnInfo> collist = dbobj.GetColumnInfoList(Dbname, TableName);
            if ((collist!=null)&&(collist.Count > 0))
            {
                listView2.Items.Clear();                
                cmbox_SField.Items.Clear();
                foreach (ColumnInfo col in collist)
                {
                    string order = col.ColumnOrder;
                    string columnName = col.ColumnName;
                    string columnType = col.TypeName;

                    this.cmbox_SField.Items.Add(columnName);

                    ListViewItem item1 = new ListViewItem(order, 0);
                    item1.Checked = true;
                    item1.ImageIndex = -1;
                    item1.SubItems.Add(columnName);
                    item1.SubItems.Add(columnType);
                    
                    listView2.Items.AddRange(new ListViewItem[] { item1 });

                }               
                if (cmbox_SField.Items.Count > 0)
                {
                    cmbox_SField.SelectedIndex = 0;
                }
            }

            txtTabname.Text = TableName;
            txtClassName2.Text = TableName;

        }


        #endregion

        #region 设定listview右键菜单
        private void SetListViewMenu(string itemType)
        {
            switch (itemType.ToLower())
            {
                case "server":
                    {
                    }
                    break;
                case "db":
                    {
                    }
                    break;
                case "table":
                    {
                    }
                    break;
                case "column":
                    {
                    }
                    break;
                default:
                    {
                    }
                    break;
            }
        }
        #endregion


        #region 选择字段按钮
        private void btn_SelAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (!item.Checked)
                {
                    item.Checked = true;
                }
            }
        }

        private void btn_SelI_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = !item.Checked;
            }
        }
        private void btn_SelAll2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.Items)
            {
                if (!item.Checked)
                {
                    item.Checked = true;
                }
            }
        }

        private void btn_SelI2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.Items)
            {
                item.Checked = !item.Checked;
            }
        }
      
        #endregion

        

        #region 公共方法

        /// <summary>
        /// 设置代码控件内容
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="strContent"></param>
        private void SettxtContent(string Type, string strContent)
        {
            codeview.SettxtContent(Type, strContent);
            this.tabControl1.SelectedIndex = 2;

        }
        /// <summary>
        /// 得到父表主键的对象信息
        /// </summary>
        /// <returns></returns>
        private List<ColumnInfo> GetKeyFieldsP()
        {
            string tableNameParent = cmbox_PTab.Text;
            List<ColumnInfo> collist = dbobj.GetColumnInfoList(dbname, tableNameParent);
            DataTable dt = Maticsoft.CodeHelper.CodeCommon.GetColumnInfoDt(collist);
            StringPlus Fields = new StringPlus();           
            Fields.Append("'" + cmbox_PField.Text + "'");

            if ((collist!=null)&&(collist.Count>0))
            {
                DataRow[] dtrows;
                if (Fields.Value.Length > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields.Value + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                List<ColumnInfo> keys = new List<ColumnInfo>();
                ColumnInfo key;
                foreach (DataRow row in dtrows)
                {
                    string Colorder = row["Colorder"].ToString();
                    string ColumnName = row["ColumnName"].ToString();
                    string TypeName = row["TypeName"].ToString();
                    string isIdentity = row["IsIdentity"].ToString();
                    string IsPK = row["IsPK"].ToString();
                    string Length = row["Length"].ToString();
                    string Preci = row["Preci"].ToString();
                    string Scale = row["Scale"].ToString();
                    string cisNull = row["cisNull"].ToString();
                    string DefaultVal = row["DefaultVal"].ToString();
                    string DeText = row["DeText"].ToString();

                    key = new ColumnInfo();
                    key.ColumnOrder = Colorder;
                    key.ColumnName = ColumnName;
                    key.TypeName = TypeName;
                    key.IsIdentity = (isIdentity == "√") ? true : false;
                    key.IsPrimaryKey = (IsPK == "√") ? true : false;
                    key.Length = Length;
                    key.Precision = Preci;
                    key.Scale = Scale;
                    key.Nullable = (cisNull == "√") ? true : false;
                    key.DefaultVal = DefaultVal;
                    key.Description = DeText;
                    keys.Add(key);

                }
                return keys;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到子表选择的字段集合
        /// </summary>
        /// <returns></returns>
        private List<ColumnInfo> GetFieldlistP()
        {
            string tableNameParent = cmbox_PTab.Text;

            List<ColumnInfo> collist = dbobj.GetColumnInfoList(dbname, tableNameParent);
            DataTable dt = Maticsoft.CodeHelper.CodeCommon.GetColumnInfoDt(collist);
            StringPlus Fields = new StringPlus();
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                {
                    Fields.Append("'" + item.SubItems[1].Text + "',");
                }
            }
            Fields.DelLastComma();
            if (dt != null)
            {
                DataRow[] dtrows;
                if (Fields.Value.Length > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields.Value + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                List<ColumnInfo> keys = new List<ColumnInfo>();
                ColumnInfo key;
                foreach (DataRow row in dtrows)
                {
                    string Colorder = row["Colorder"].ToString();
                    string ColumnName = row["ColumnName"].ToString();
                    string TypeName = row["TypeName"].ToString();
                    string isIdentity = row["IsIdentity"].ToString();
                    string IsPK = row["IsPK"].ToString();
                    string Length = row["Length"].ToString();
                    string Preci = row["Preci"].ToString();
                    string Scale = row["Scale"].ToString();
                    string cisNull = row["cisNull"].ToString();
                    string DefaultVal = row["DefaultVal"].ToString();
                    string DeText = row["DeText"].ToString();

                    key = new ColumnInfo();
                    key.ColumnOrder = Colorder;
                    key.ColumnName = ColumnName;
                    key.TypeName = TypeName;
                    key.IsIdentity = (isIdentity == "√") ? true : false;
                    key.IsPrimaryKey = (IsPK == "√") ? true : false;
                    key.Length = Length;
                    key.Precision = Preci;
                    key.Scale = Scale;
                    key.Nullable = (cisNull == "√") ? true : false;
                    key.DefaultVal = DefaultVal;
                    key.Description = DeText;
                    keys.Add(key);

                }
                return keys;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到子表主键的对象信息
        /// </summary>
        /// <returns></returns>
        private List<ColumnInfo> GetKeyFieldsS()
        {            
            string tableNameSon = cmbox_STab.Text;            
            List<ColumnInfo> collist = dbobj.GetColumnInfoList(dbname, tableNameSon);
            DataTable dt = Maticsoft.CodeHelper.CodeCommon.GetColumnInfoDt(collist);
            StringPlus Fields = new StringPlus();
          
            Fields.Append("'" + cmbox_SField.Text + "'");
            if (dt != null)
            {
                DataRow[] dtrows;
                if (Fields.Value.Length > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields.Value + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                List<ColumnInfo> keys = new List<ColumnInfo>();
                ColumnInfo key;
                foreach (DataRow row in dtrows)
                {
                    string Colorder = row["Colorder"].ToString();
                    string ColumnName = row["ColumnName"].ToString();
                    string TypeName = row["TypeName"].ToString();
                    string isIdentity = row["IsIdentity"].ToString();
                    string IsPK = row["IsPK"].ToString();
                    string Length = row["Length"].ToString();
                    string Preci = row["Preci"].ToString();
                    string Scale = row["Scale"].ToString();
                    string cisNull = row["cisNull"].ToString();
                    string DefaultVal = row["DefaultVal"].ToString();
                    string DeText = row["DeText"].ToString();

                    key = new ColumnInfo();
                    key.ColumnOrder = Colorder;
                    key.ColumnName = ColumnName;
                    key.TypeName = TypeName;
                    key.IsIdentity = (isIdentity == "√") ? true : false;
                    key.IsPrimaryKey = (IsPK == "√") ? true : false;
                    key.Length = Length;
                    key.Precision = Preci;
                    key.Scale = Scale;
                    key.Nullable = (cisNull == "√") ? true : false;
                    key.DefaultVal = DefaultVal;
                    key.Description = DeText;
                    keys.Add(key);

                }
                return keys;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到子表选择的字段集合
        /// </summary>
        /// <returns></returns>
        private List<ColumnInfo> GetFieldlistS()
        {            
            string tableNameSon = cmbox_STab.Text;
 
            List<ColumnInfo> collist = dbobj.GetColumnInfoList(dbname, tableNameSon);
            DataTable dt = Maticsoft.CodeHelper.CodeCommon.GetColumnInfoDt(collist);

            StringPlus Fields = new StringPlus();
            foreach (ListViewItem item in this.listView2.Items)
            {
                if (item.Checked)
                {
                    Fields.Append("'" + item.SubItems[1].Text + "',");
                }
            }
            Fields.DelLastComma();
            if (dt != null)
            {
                DataRow[] dtrows;
                if (Fields.Value.Length > 0)
                {
                    dtrows = dt.Select("ColumnName in (" + Fields.Value + ")", "colorder asc");
                }
                else
                {
                    dtrows = dt.Select();
                }
                List<ColumnInfo> keys = new List<ColumnInfo>();
                ColumnInfo key;
                foreach (DataRow row in dtrows)
                {
                    string Colorder = row["Colorder"].ToString();
                    string ColumnName = row["ColumnName"].ToString();
                    string TypeName = row["TypeName"].ToString();
                    string isIdentity = row["IsIdentity"].ToString();
                    string IsPK = row["IsPK"].ToString();
                    string Length = row["Length"].ToString();
                    string Preci = row["Preci"].ToString();
                    string Scale = row["Scale"].ToString();
                    string cisNull = row["cisNull"].ToString();
                    string DefaultVal = row["DefaultVal"].ToString();
                    string DeText = row["DeText"].ToString();

                    key = new ColumnInfo();
                    key.ColumnOrder = Colorder;
                    key.ColumnName = ColumnName;
                    key.TypeName = TypeName;
                    key.IsIdentity = (isIdentity == "√") ? true : false;
                    key.IsPrimaryKey = (IsPK == "√") ? true : false;
                    key.Length = Length;
                    key.Precision = Preci;
                    key.Scale = Scale;
                    key.Nullable = (cisNull == "√") ? true : false;
                    key.DefaultVal = DefaultVal;
                    key.Description = DeText;
                    keys.Add(key);

                }
                return keys;
            }
            else
            {
                return null;
            }
        }

        //得到数据层类型
        private string GetDALType()
        {
            string daltype = "";
            daltype = cm_daltype.AppGuid;
            if ((daltype == "") || (daltype == "System.Data.DataRowView"))
            {
                MessageBox.Show("选择的数据层类型有误，请关闭后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return ""; 
            }
            return daltype;
        }

        //得到bll层类型
        private string GetBLLType()
        {
            string blltype = "";
            blltype = cm_blltype.AppGuid;
            if ((blltype == "") && (blltype == "System.Data.DataRowView"))
            {
                MessageBox.Show("选择的数据层类型有误，请关闭后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
            return blltype;
        }
        //得到web层类型
        public string GetWebType()
        {
            string webtype = "";
            webtype = cm_webtype.AppGuid;
            if ((webtype == "") && (webtype == "System.Data.DataRowView"))
            {
                MessageBox.Show("选择的表示层类型有误，请关闭后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
            return webtype;
        }
        #endregion

        #region 代码生成 按钮

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            if (listView1.CheckedItems.Count < 1)
            {
                MessageBox.Show("没有任何可以生成的项！", "请选择", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                if (this.radbtn_Type_DB.Checked)
                {
                    CreatDB();
                }
                if (this.radbtn_Type_CS.Checked)
                {
                    CreatCS();
                }
                if (this.radbtn_Type_Web.Checked)
                {
                    CreatWeb();
                }
            }
            catch
            {
                MessageBox.Show("代码生成失败，请关闭后重新打开再试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            #region 保存配置

            if (this.radbtn_Frame_One.Checked)
            {
                dbset.AppFrame = "One";
            }
            if (this.radbtn_Frame_S3.Checked)
            {
                dbset.AppFrame = "S3";
            }
            if (this.radbtn_Frame_F3.Checked)
            {
                dbset.AppFrame = "F3";
            }

            dbset.DALType = GetDALType();
            dbset.BLLType = GetBLLType();
            dbset.ProjectName = txtProjectName.Text;
            dbset.Namepace = txtNameSpace.Text;
            dbset.Folder = txtNameSpace2.Text;
            dbset.ProcPrefix = txtProcPrefix.Text;
            //Maticsoft.CmConfig.ModuleConfig.SaveSettings(setting);
            Maticsoft.CmConfig.DbConfig.UpdateSettings(dbset);
            #endregion
        }

        #endregion

        #region 生成 数据库脚本
        private void CreatDB()
        {
            if (this.radbtn_DB_Proc.Checked)
            {
                CreatDBProc();
            }
            else
            {
                CreatDBScript();
            }
        }

        //创建存储过程
        private void CreatDBProc()
        {
            string projectname = this.txtProjectName.Text;
            string snamespace = this.txtNameSpace.Text;
            string snamespace2 = this.txtNameSpace2.Text;
            string classname = this.txtClassName.Text;
            string procPrefix = this.txtProcPrefix.Text;
            string procTabname = this.txtTabname.Text;

            bool Maxid = this.chk_DB_GetMaxID.Checked;
            bool Exists = this.chk_DB_Exists.Checked;
            bool Add = this.chk_DB_Add.Checked;
            bool Update = this.chk_DB_Update.Checked;
            bool Delete = this.chk_DB_Delete.Checked;
            bool GetModel = this.chk_DB_GetModel.Checked;
            bool List = this.chk_DB_GetList.Checked;

            Maticsoft.IDBO.IDbScriptBuilder dsb = ObjHelper.CreatDsb(servername);
            dsb.DbName = dbname;
            dsb.TableName = procTabname;// tablename;
            dsb.ProjectName = projectname;
            dsb.ProcPrefix = procPrefix;            
            dsb.Keys = GetKeyFieldsP();
            dsb.Fieldlist = GetFieldlistP();
            string strProc = dsb.GetPROCCode(Maxid, Exists, Add, Update, Delete, GetModel, List);
            SettxtContent("SQL", strProc);
        }
        //数据库脚本
        private void CreatDBScript()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            Maticsoft.IDBO.IDbScriptBuilder dsb = ObjHelper.CreatDsb(servername);
            dsb.Fieldlist = GetFieldlistP();
            string strScript = dsb.CreateTabScript(dbname, tablename);
            SettxtContent("SQL", strScript);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        #region 生成 C#代码
        //架构选择
        private void CreatCS()
        {
            if (this.radbtn_Frame_One.Checked)
            {
                CreatCsOne();
            }
            if (this.radbtn_Frame_S3.Checked)
            {
                CreatCsS3();
            }
            if (this.radbtn_Frame_F3.Checked)
            {
                CreatCsF3();
            }
        }

        #region 单类结构
        private void CreatCsOne()
        {
            string procPrefix = this.txtProcPrefix.Text;
            string namepace = this.txtNameSpace.Text.Trim();
            string namepace2 = this.txtNameSpace2.Text.Trim();
            if (namepace2.Trim() != "")
            {
                namepace = namepace + "." + namepace2;
            }
            string classname = this.txtClassName.Text;
            if (classname == "")
            {
                classname = tablename;
            }

            BuilderFrameOne cfo = new BuilderFrameOne(dbobj, dbname, tablename, classname, GetFieldlistP(), GetKeyFieldsP(), namepace,namepace2, dbset.DbHelperName);
            string dALtype = GetDALType();
            string strCode = cfo.GetCode(dALtype,false, chk_CS_Exists.Checked, chk_CS_Add.Checked,
                chk_CS_Update.Checked, chk_CS_Delete.Checked, chk_CS_GetModel.Checked, chk_CS_GetList.Checked, procPrefix);
            SettxtContent("CS", strCode);
        }
        #endregion

        #region 简单三层

        private void CreatCsS3()
        {
            if (radbtn_F3_Model.Checked)
            {
                CreatCsS3Model();
            }
            if (radbtn_F3_DAL.Checked)
            {
                CreatCsS3DAL();
            }
            if (radbtn_F3_BLL.Checked)
            {
                CreatCsS3BLL();
            }
        }
        private void CreatCsS3Model()
        {
            string procPrefix = this.txtProcPrefix.Text;
            string namepace = this.txtNameSpace.Text.Trim();
            string namepace2 = this.txtNameSpace2.Text.Trim();
            string tableNameParent = cmbox_PTab.Text;
            string tableNameSon = cmbox_STab.Text;
            string modelNameParent = txtClassName.Text;
            string modelNameSon = txtClassName2.Text;
            //命名规则处理
            modelNameParent = NameRule.GetModelClass(modelNameParent, dbset);
            modelNameSon = NameRule.GetModelClass(modelNameSon, dbset);

            BuilderFrameS3 s3 = new BuilderFrameS3(dbobj, dbname, namepace, namepace2, dbset.DbHelperName);
            string strCode = s3.GetModelCode(tableNameParent, modelNameParent, GetFieldlistP(), tableNameSon, modelNameSon, GetFieldlistS());

            SettxtContent("CS", strCode);
        }
        
        private void CreatCsS3DAL()
        {
            string procPrefix = this.txtProcPrefix.Text;
            string namepace = this.txtNameSpace.Text.Trim();
            string namepace2 = this.txtNameSpace2.Text.Trim();            
            string tableNameParent=cmbox_PTab.Text;
            string tableNameSon = cmbox_STab.Text;

            string modelNameParent = txtClassName.Text;
            string modelNameSon = txtClassName2.Text;
            if (modelNameParent == "")
            {
                modelNameParent = tableNameParent;
            }
            if (modelNameSon == "")
            {
                modelNameSon = tableNameSon;
            }
            string bllname = modelNameParent;
            string dalnameparent = modelNameParent;
            string dalnameson = modelNameSon;

            //命名规则处理
            modelNameParent = NameRule.GetModelClass(modelNameParent, dbset);
            modelNameSon = NameRule.GetModelClass(modelNameSon, dbset);
            bllname = NameRule.GetBLLClass(bllname, dbset);
            dalnameparent = NameRule.GetDALClass(dalnameparent, dbset);
            dalnameson = NameRule.GetDALClass(dalnameson, dbset);

            BuilderFrameS3 s3 = new BuilderFrameS3(dbobj, dbname, tablename, cb.TableDescription, modelNameParent, bllname, dalnameparent, GetFieldlistP(), GetKeyFieldsP(), namepace, namepace2, dbset.DbHelperName);

            string dALtype = GetDALType();
            string strCode = s3.GetDALCodeTran(dALtype, false, chk_CS_Exists.Checked, chk_CS_Add.Checked, 
                chk_CS_Update.Checked, chk_CS_Delete.Checked, chk_CS_GetModel.Checked, chk_CS_GetList.Checked, procPrefix,
                tableNameParent, tableNameSon, modelNameParent, modelNameSon, GetFieldlistP(), GetFieldlistS(),
              GetKeyFieldsP(), GetKeyFieldsS(), dalnameparent, dalnameson);
            SettxtContent("CS", strCode);
        }
        private void CreatCsS3BLL()
        {
            string procPrefix = this.txtProcPrefix.Text;
            string namepace = this.txtNameSpace.Text.Trim();
            string namepace2 = this.txtNameSpace2.Text.Trim();
            string tableNameParent = cmbox_PTab.Text;
            string tableNameSon = cmbox_STab.Text;

            string modelNameParent = txtClassName.Text;
            string modelNameSon = txtClassName2.Text;
            if (modelNameParent == "")
            {
                modelNameParent = tableNameParent;
            }
            if (modelNameSon == "")
            {
                modelNameSon = tableNameSon;
            }
            string bllname = modelNameParent;
            string dalnameparent = modelNameParent;
            string dalnameson = modelNameSon;
            //命名规则处理
            modelNameParent = NameRule.GetModelClass(modelNameParent, dbset);
            modelNameSon = NameRule.GetModelClass(modelNameSon, dbset);
            bllname = NameRule.GetBLLClass(bllname, dbset);
            dalnameparent = NameRule.GetDALClass(dalnameparent, dbset);
            dalnameson = NameRule.GetDALClass(dalnameson, dbset);

            BuilderFrameS3 s3 = new BuilderFrameS3(dbobj, dbname, tablename, cb.TableDescription, modelNameParent, bllname, dalnameparent, GetFieldlistP(), GetKeyFieldsP(), namepace, namepace2, dbset.DbHelperName);
            string blltype = GetBLLType();
            string strCode = s3.GetBLLCode(blltype, false, chk_CS_Exists.Checked, chk_CS_Add.Checked, chk_CS_Update.Checked, chk_CS_Delete.Checked, chk_CS_GetModel.Checked, chk_CS_GetModelByCache.Checked, chk_CS_GetList.Checked);
            SettxtContent("CS", strCode);
        }
        #endregion

        #region 工厂模式三层
        private void CreatCsF3()
        {
            if (radbtn_F3_Model.Checked)
            {
                CreatCsF3Model();
            }
            if (radbtn_F3_DAL.Checked)
            {
                CreatCsF3DAL();
            }
            if (radbtn_F3_IDAL.Checked)
            {
                CreatCsF3IDAL();
            }
            if (radbtn_F3_DALFactory.Checked)
            {
                CreatCsF3DALFactory();
            }
            if (radbtn_F3_BLL.Checked)
            {
                CreatCsF3BLL();
            }
        }
        private void CreatCsF3Model()
        {
            string procPrefix = this.txtProcPrefix.Text;
            string namepace = this.txtNameSpace.Text.Trim();
            string namepace2 = this.txtNameSpace2.Text.Trim();
            string tableNameParent = cmbox_PTab.Text;
            string tableNameSon = cmbox_STab.Text;
            string modelNameParent = txtClassName.Text;
            string modelNameSon = txtClassName2.Text;
            //命名规则处理
            modelNameParent = NameRule.GetModelClass(modelNameParent, dbset);
            modelNameSon = NameRule.GetModelClass(modelNameSon, dbset);

            BuilderFrameF3 f3 = new BuilderFrameF3(dbobj, dbname, namepace, namepace2, dbset.DbHelperName);
            string strCode = f3.GetModelCode(tableNameParent, modelNameParent, GetFieldlistP(), tableNameSon, modelNameSon, GetFieldlistS());

            SettxtContent("CS", strCode);

        }
        private void CreatCsF3DAL()
        {
            string procPrefix = this.txtProcPrefix.Text;
            string namepace = this.txtNameSpace.Text.Trim();
            string namepace2 = this.txtNameSpace2.Text.Trim();
            string tableNameParent = cmbox_PTab.Text;
            string tableNameSon = cmbox_STab.Text;

            string modelNameParent = txtClassName.Text;
            string modelNameSon = txtClassName2.Text;
            if (modelNameParent == "")
            {
                modelNameParent = tableNameParent;
            }
            if (modelNameSon == "")
            {
                modelNameSon = tableNameSon;
            }
            string bllname = modelNameParent;
            string dalnameparent = modelNameParent;
            string dalnameson = modelNameSon;

            BuilderFrameF3 f3 = new BuilderFrameF3(dbobj, dbname, tablename, cb.TableDescription, modelNameParent, bllname, dalnameparent, GetFieldlistP(), GetKeyFieldsP(), namepace, namepace2, dbset.DbHelperName);

            string dALtype = GetDALType();
            string strCode = f3.GetDALCodeTran(dALtype, false, chk_CS_Exists.Checked, chk_CS_Add.Checked,
                chk_CS_Update.Checked, chk_CS_Delete.Checked, chk_CS_GetModel.Checked, chk_CS_GetList.Checked, procPrefix,
                tableNameParent, tableNameSon, modelNameParent, modelNameSon, GetFieldlistP(), GetFieldlistS(),
              GetKeyFieldsP(), GetKeyFieldsS(), dalnameparent, dalnameson);

            SettxtContent("CS", strCode);

        }
        private void CreatCsF3IDAL()
        {            
            string namepace = this.txtNameSpace.Text.Trim();
            string namepace2 = this.txtNameSpace2.Text.Trim();
            string tableNameParent = cmbox_PTab.Text;
            string tableNameSon = cmbox_STab.Text;
            string modelNameParent = txtClassName.Text;
            if (modelNameParent == "")
            {
                modelNameParent = tableNameParent;
            }
            string bllname = modelNameParent;
            string dalnameparent = modelNameParent;


            BuilderFrameF3 f3 = new BuilderFrameF3(dbobj, dbname, tablename, cb.TableDescription, modelNameParent, bllname, dalnameparent, GetFieldlistP(), GetKeyFieldsP(), namepace, namepace2, dbset.DbHelperName);
            string strCode = f3.GetIDALCode(false, chk_CS_Exists.Checked, chk_CS_Add.Checked, chk_CS_Update.Checked, chk_CS_Delete.Checked, chk_CS_GetModel.Checked, chk_CS_GetList.Checked, chk_CS_GetList.Checked);
            SettxtContent("CS", strCode);
        }
        private void CreatCsF3DALFactory()
        {            
            string namepace = this.txtNameSpace.Text.Trim();
            string namepace2 = this.txtNameSpace2.Text.Trim();
            string tableNameParent = cmbox_PTab.Text;
            string tableNameSon = cmbox_STab.Text;
            string modelNameParent = txtClassName.Text;
            if (modelNameParent == "")
            {
                modelNameParent = tableNameParent;
            }
            string bllname = modelNameParent;
            string dalnameparent = modelNameParent;

            BuilderFrameF3 f3 = new BuilderFrameF3(dbobj, dbname, tablename, cb.TableDescription, modelNameParent, bllname, dalnameparent, GetFieldlistP(), GetKeyFieldsP(), namepace, namepace2, dbset.DbHelperName);
            string strCode = f3.GetDALFactoryCode();
            SettxtContent("CS", strCode);
        }
        private void CreatCsF3BLL()
        {            
            string namepace = this.txtNameSpace.Text.Trim();
            string namepace2 = this.txtNameSpace2.Text.Trim();
            string tableNameParent = cmbox_PTab.Text;
            string tableNameSon = cmbox_STab.Text;
            string modelNameParent = txtClassName.Text;
            if (modelNameParent == "")
            {
                modelNameParent = tableNameParent;
            }
            string bllname = modelNameParent;
            string dalnameparent = modelNameParent;

            BuilderFrameF3 f3 = new BuilderFrameF3(dbobj, dbname, tablename, cb.TableDescription, modelNameParent, bllname, dalnameparent, GetFieldlistP(), GetKeyFieldsP(), namepace, namepace2, dbset.DbHelperName);
            string blltype = GetBLLType();
            string strCode = f3.GetBLLCode(blltype, false, chk_CS_Exists.Checked, chk_CS_Add.Checked, chk_CS_Update.Checked, chk_CS_Delete.Checked, chk_CS_GetModel.Checked, chk_CS_GetModelByCache.Checked, chk_CS_GetList.Checked, chk_CS_GetList.Checked);
            SettxtContent("CS", strCode);
        }
        #endregion

        #endregion

        #region 生成 Web页面
        private void CreatWeb()
        {
            string namepace = this.txtNameSpace.Text.Trim();
            string folder = this.txtNameSpace2.Text.Trim();
            string modelname = this.txtClassName.Text;
            if (modelname == "")
            {
                modelname = tablename;
            }
            string bllname = modelname;     
            //命名规则处理
            modelname = NameRule.GetModelClass(modelname, dbset);
            bllname = NameRule.GetBLLClass(bllname, dbset);

            //BuilderWeb bw = new BuilderWeb();            
            //bw.NameSpace = namepace;
            //bw.Fieldlist = GetFieldlistP();
            //bw.Keys = GetKeyFieldsP();
            //bw.ModelName = ModelName;
            //bw.Folder = folder;

            cb.BLLName = bllname;
            cb.NameSpace = namepace;
            cb.Fieldlist = GetFieldlistP();
            cb.Keys = GetKeyFieldsP();
            cb.ModelName = modelname;
            cb.Folder = folder;

            string webtype = GetWebType();
            cb.CreatBuilderWeb(webtype);


            if (radbtn_Web_Aspx.Checked)
            {
                string strCode = cb.GetWebHtmlCode(chk_Web_HasKey.Checked, chk_Web_Add.Checked, chk_Web_Update.Checked, chk_Web_Show.Checked, true);
                SettxtContent("Aspx", strCode);
            }
            else
            {
                string strCode = cb.GetWebCode(chk_Web_HasKey.Checked, chk_Web_Add.Checked, chk_Web_Update.Checked, chk_Web_Show.Checked, true);
                SettxtContent("CS", strCode);
            }
        }
        #endregion

        private void btn_Next_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }



    }
}