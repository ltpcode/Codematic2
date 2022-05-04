using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Codematic.UserControls;
using Maticsoft.CodeBuild;
using Maticsoft.Utility;
using Maticsoft.CodeHelper;
namespace Codematic
{
    /// <summary>
    /// 事务代码生成
    /// </summary>
    public partial class CodeMakerTran : Form
    {
        private UcCodeView codeview;
        Maticsoft.CmConfig.DbSettings dbset;
        Maticsoft.IDBO.IDbObject dbobj;
        Maticsoft.CodeBuild.CodeBuilders cb;//代码生成对象

        string servername;
        string dbname;
        string tablename;
        DALTypeAddIn cm_daltype;
        private Thread thread;
        delegate void SetListCallback();


        public CodeMakerTran(string Dbname)
        {
            InitializeComponent();
            dbname = Dbname;
            codeview = new UcCodeView();
            tabPage2.Controls.Add(codeview);                        
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

            //BindTablist(Dbname);
        }

        #region 初始化窗体设置
        private void SetFormConfig(string servername)
        {
            dbset = Maticsoft.CmConfig.DbConfig.GetSetting(servername);            
            txtNameSpace.Text = dbset.Namepace;
            txtNameSpace2.Text = dbset.Folder;
            
            #region 加载插件

            cm_daltype = new DALTypeAddIn("Maticsoft.IBuilder.IBuilderDALMTran");
            cm_daltype.Title = "DAL";
            groupBox3.Controls.Add(cm_daltype);
            cm_daltype.Location = new System.Drawing.Point(30, 70);

            cm_daltype.SetSelectedDALType(dbset.DALType.Trim());

            #endregion
            
            this.tabControl1.SelectedIndex = 0;
        }
        
        #endregion
        
        #region 设置listview
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

        #region  绑定表信息
        private void BindTablist(string Dbname)
        {
            List<TableInfo> tablist = dbobj.GetTablesInfo(Dbname);
            if ((tablist != null) && (tablist.Count > 0))
            {
                this.listTable.Items.Clear();                
                foreach (TableInfo tab in tablist)
                {
                    string name = tab.TabName;
                    listTable.Items.Add(name);                    
                }
            }
        }

        private void listTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((listTable.SelectedItem != null) && (listTable.Text != "System.Data.DataRowView"))
            {
                string tabname = listTable.SelectedItem.ToString();
                BindlistViewCol(dbname, tabname);
            }
        }
              
        #endregion

        #region  为listView邦定 列 数据

        private void CreatView()
        {
            //创建列表
            this.listView1.Columns.Clear();
            this.listView1.Items.Clear();
            //this.listView1.LargeImageList = imglistView;
            //this.listView1.SmallImageList = imglistView;
            this.listView1.View = View.Details;
            this.listView1.GridLines = true;
            //this.listView1.CheckBoxes = true;
            this.listView1.FullRowSelect = true;

            listView1.Columns.Add("表", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("类名", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("操作", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("条件字段", 150, HorizontalAlignment.Left);
            
            
        }

        private void BindlistViewCol(string Dbname, string TableName)
        {
            List<ColumnInfo> collist = dbobj.GetColumnInfoList(Dbname, TableName);
            if ((collist != null) && (collist.Count > 0))
            {               
                cmbox_Field.Items.Clear();
                foreach (ColumnInfo col in collist)
                {
                    string order = col.ColumnOrder;
                    string columnName = col.ColumnName;
                    string typename = col.TypeName;
                    this.cmbox_Field.Items.Add(columnName);
                    
                }
                if (cmbox_Field.Items.Count > 0)
                {
                    cmbox_Field.SelectedIndex = 0;
                }
            }
        }

        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                listView1.Items.Remove(item);
            }
        }


        #endregion


        #region 添加操作内容

        private void radbtn_Click(object sender, EventArgs e)
        {
            if (radbtn_Insert.Checked)
            {
                label3.Visible = false;
                cmbox_Field.Visible = false;
            }
            else
            {
                label3.Visible = true;
                cmbox_Field.Visible = true;
            }
        }

        List<string> selTab = new List<string>();
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (listTable.SelectedItem == null)
                return;

            string tabname = listTable.SelectedItem.ToString();

            string action = radbtn_Insert.Text;
            string whereField = "";
            if (radbtn_Update.Checked)
            {
                action = radbtn_Update.Text;
                whereField = cmbox_Field.Text;
            }
            if (radbtn_Delete.Checked)
            {
                action = radbtn_Delete.Text;
                whereField = cmbox_Field.Text;
            }           

            //如果已经存在则不添加
            if (selTab.Contains(tabname + action))
            {
                return;
            }

            ListViewItem item1 = new ListViewItem(tabname, 0);
            item1.Checked = true;
            item1.ImageIndex = -1;
            item1.SubItems.Add(tabname);
            item1.SubItems.Add(action);
            item1.SubItems.Add(whereField);
            selTab.Add(tabname + action);
            
            listView1.Items.AddRange(new ListViewItem[] { item1 });
        }
        
        #endregion 


        #region 公共方法

        /// <summary>
        /// 得到表和操作的集合
        /// </summary>
        /// <returns></returns>
        private List<ModelTran> GetModelTranlist()
        {
            List<ModelTran> modelTrans = new List<ModelTran>();
            ModelTran mt;
            foreach (ListViewItem item in listView1.Items)
            {
                mt = new ModelTran();
                mt.DbName = dbname;
                mt.TableName=item.SubItems[0].Text;//表
                mt.ModelName= item.SubItems[1].Text;//类名                
                mt.Action= item.SubItems[2].Text;//操作
                string keyField = item.SubItems[3].Text;//条件字段
                
                List<ColumnInfo> collist = dbobj.GetColumnInfoList(dbname, mt.TableName);
                mt.Fieldlist = collist;
                mt.Keys = new List<ColumnInfo>();
                foreach(ColumnInfo key in collist)
                {
                    if (key.ColumnName == keyField)
                    {
                        mt.Keys.Add(key);
                    }
                }

                modelTrans.Add(mt);
            }

            return modelTrans;
        }

        /// <summary>
        /// 设置代码控件内容
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="strContent"></param>
        private void SettxtContent(string Type, string strContent)
        {
            codeview.SettxtContent(Type, strContent);
            this.tabControl1.SelectedIndex = 1;            
        }

        /// <summary>
        /// 得到数据层类型
        /// </summary>        
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


        #endregion

        
        private void btn_Create_Click(object sender, EventArgs e)
        {
            string namepace = this.txtNameSpace.Text.Trim();
            string namepace2 = this.txtNameSpace2.Text.Trim();
            List<ModelTran> modelTranlist = GetModelTranlist();

            if (modelTranlist.Count == 0)
                return;

            BuilderFrameS3 s3 = new BuilderFrameS3(dbobj, dbname, namepace, namepace2, dbset.DbHelperName);

            string dALtype = GetDALType();
            string strCode = s3.GetDALCodeMTran(dALtype, modelTranlist);
            SettxtContent("CS", strCode);

        }

       

        
    }
}
