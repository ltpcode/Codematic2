using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.Text;
using System.IO;
using Maticsoft.CodeHelper;
using System.Windows.Forms;
namespace Codematic
{
    //数据库管理器
    public partial class DbView : Form
    {
        #region 系统变量

        MainForm mainfrm;
        public static bool isMdb = false;//是否是mdb数据库        
        Maticsoft.IDBO.IDbObject dbobj;
        string path = Application.StartupPath;
        LoginForm logo = new LoginForm();
        LoginOra logoOra = new LoginOra();
        LoginOledb logoOledb = new LoginOledb();
        LoginMySQL loginMysql = new LoginMySQL();
        LoginSQLite loginSQLite = new LoginSQLite();
        DbTypeSel dbsel = new DbTypeSel();

        TreeNode TreeClickNode;//右键菜单单击的节点
        TreeNode serverlistNode;
        private bool m_bLayoutCalled = false;        
        Maticsoft.CmConfig.DbSettings dbset;
        #endregion

        delegate void AddTreeNodeCallback(TreeNode ParentNode, TreeNode Node);
        delegate void SetTreeNodeFontCallback(TreeNode Node, Font nodeFont);
                
        public DbView(Form mdiParentForm)
        {
            mainfrm = (MainForm)mdiParentForm;
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            backgroundWorkerCon.DoWork += new DoWorkEventHandler(DoConnect);
            backgroundWorkerCon.ProgressChanged += new ProgressChangedEventHandler(ProgessChangedCon);
            backgroundWorkerCon.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompleteWorkCon);

            backgroundWorkerReg.DoWork += new DoWorkEventHandler(RegServer);
            backgroundWorkerReg.ProgressChanged += new ProgressChangedEventHandler(ProgessChangedReg);
            backgroundWorkerReg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompleteWorkReg);

            treeView1.HideSelection = false;
            this.treeView1.ExpandAll();
        }

        #region FormLoad

        private void DbView_Load(object sender, EventArgs e)
        {
            LoadServer();
            //setting = Maticsoft.CmConfig.ModuleConfig.GetSettings();
            mainfrm = (MainForm)Application.OpenForms["MainForm"];
        }

        #endregion

        #region 公共方法

        // 增加TabPage
        private void AddTabPage(string pageTitle, Control ctrForm)
        {
            if (mainfrm.tabControlMain.Visible == false)
            {
                mainfrm.tabControlMain.Visible = true;
            }
            Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage();
            page.Title = pageTitle;
            page.Control = ctrForm;
            //MainForm mainfrm = (MainForm)Application.OpenForms["MainForm"];
            mainfrm.tabControlMain.TabPages.Add(page);
            mainfrm.tabControlMain.SelectedTab = page;
        }

        // 增加TabPage
        private void AddTabPage(string pageTitle, Control ctrForm, MainForm mainfrm)
        {
            if (mainfrm.tabControlMain.Visible == false)
            {
                mainfrm.tabControlMain.Visible = true;
            }
            Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage();
            page.Title = pageTitle;
            page.Control = ctrForm;
            mainfrm.tabControlMain.TabPages.Add(page);
            mainfrm.tabControlMain.SelectedTab = page;
        }

        // 创建新的唯一窗体页（不允许重复的）
        private void AddSinglePage(Control control, string Title)
        {
            if (mainfrm.tabControlMain.Visible == false)
            {
                mainfrm.tabControlMain.Visible = true;
            }
            bool showed = false;
            Crownwood.Magic.Controls.TabPage currPage = null;
            foreach (Crownwood.Magic.Controls.TabPage page in mainfrm.tabControlMain.TabPages)
            {
                if (page.Control.Name == control.Name)
                {
                    showed = true;
                    currPage = page;
                }
            }
            if (!showed)//不存在
            {
                AddTabPage(Title, control);
            }
            else
            {
                mainfrm.tabControlMain.SelectedTab = currPage;
            }
        }

        /// <summary>
        /// 异步线程中为树增加节点
        /// </summary>
        /// <param name="ParentNode"></param>
        /// <param name="Node"></param>
        public void AddTreeNode(TreeNode ParentNode, TreeNode Node)
        {
            if (this.treeView1.InvokeRequired)
            {
                AddTreeNodeCallback d = new AddTreeNodeCallback(AddTreeNode);
                this.Invoke(d, new object[] { ParentNode, Node });
            }
            else
            {
                ParentNode.Nodes.Add(Node);
            }
        }
        
        /// <summary>
        /// 异步线程中设置节点字体
        /// </summary>
        /// <param name="ParentNode"></param>
        /// <param name="Node"></param>
        public void SetTreeNodeFont(TreeNode Node,Font nodeFont)
        {
            if (this.treeView1.InvokeRequired)
            {
                SetTreeNodeFontCallback d = new SetTreeNodeFontCallback(SetTreeNodeFont);
                this.Invoke(d, new object[] { Node, nodeFont });
            }
            else
            {
                Node.NodeFont = nodeFont;
            }
        }

      
        /// <summary>
        /// 得到选中的服务器名字信息
        /// </summary>
        /// <returns></returns>
        public string GetLongServername()
        {
            TreeNode SelNode = treeView1.SelectedNode;
            if (SelNode == null)
                return "";
            string loneServername = "";
            switch (SelNode.Tag.ToString())
            {
                case "serverlist":
                    return "";
                case "server":
                    {
                        loneServername = SelNode.Text;
                    }
                    break;
                case "db":
                    {
                        loneServername = SelNode.Parent.Text;

                    }
                    break;
                case "tableroot":
                case "viewroot":
                    {
                        loneServername = SelNode.Parent.Parent.Text;

                    }
                    break;
                case "table":
                case "view":
                    {
                        loneServername = SelNode.Parent.Parent.Parent.Text;
                    }
                    break;
                case "column":
                    loneServername = SelNode.Parent.Parent.Parent.Parent.Text;
                    break;
            }

            return loneServername;
        }

        /// <summary>
        /// 得到选中的数据库名字
        /// </summary>
        /// <returns></returns>
        public string GetSelectedDBName()
        {
            TreeNode SelNode = treeView1.SelectedNode;
            if (SelNode == null)
                return "";
            string dbname = "";
            switch (SelNode.Tag.ToString())
            {
                case "serverlist":
                    return "";
                case "server":
                    {                        
                    }
                    break;
                case "db":
                    {
                        dbname = SelNode.Text;

                    }
                    break;
                case "tableroot":
                case "viewroot":
                    {
                        dbname = SelNode.Parent.Text;

                    }
                    break;
                case "table":
                case "view":
                    {
                        dbname = SelNode.Parent.Parent.Text;
                    }
                    break;
                case "column":
                    dbname = SelNode.Parent.Parent.Parent.Text;
                    break;
            }

            return dbname;
        }

        //根据服务器配置得到服务器节点字符串
        private string GetserverNodeText(string servername, string dbtype, string dbname)
        {
            string str = servername + "(" + dbtype + ")";
            if ((dbname.Trim() != "") && (dbname.Trim() != "master"))
            {
                str += "(" + dbname + ")";
            }
            return str;
        }

        #endregion

        #region 初始化服务器树
        /// <summary>
        /// 加载服务器根节点信息
        /// </summary>
        private void LoadServer()
        {
            this.treeView1.Nodes.Clear();
            serverlistNode = new TreeNode("服务器");
            serverlistNode.Tag = "serverlist";
            //serverlistNode.
            serverlistNode.ImageIndex = 0;
            serverlistNode.SelectedImageIndex = 0;
            treeView1.Nodes.Add(serverlistNode);

            Maticsoft.CmConfig.DbSettings[] dbs = Maticsoft.CmConfig.DbConfig.GetSettings();
            if (dbs != null)
            {
                foreach (Maticsoft.CmConfig.DbSettings db in dbs)
                {
                    string servername = db.Server;
                    string dbtype = db.DbType;
                    string dbname = db.DbName;
                    TreeNode serverNode = new TreeNode(GetserverNodeText(servername, dbtype, dbname));
                    serverNode.ImageIndex = 1;
                    serverNode.SelectedImageIndex = 1;
                    serverNode.Tag = "server";
                    serverlistNode.Nodes.Add(serverNode);
                }
                serverlistNode.Expand();
            }
        }

        #endregion

        #region 工具栏
        private void toolbtn_AddServer_Click(object sender, EventArgs e)
        {
            backgroundWorkerReg.RunWorkerAsync();
        }
        private void toolbtn_Connect_Click(object sender, EventArgs e)
        {
            if ((TreeClickNode == null) || (TreeClickNode.Tag.ToString() != "server"))
                return;
            if ((TreeClickNode.Tag.ToString() == "server") & (TreeClickNode.Nodes.Count > 0))
                return;

            backgroundWorkerCon.RunWorkerAsync();
        }

        private void toolbtn_unConnect_Click(object sender, EventArgs e)
        {
            if ((TreeClickNode == null) || (TreeClickNode.Tag.ToString() != "server"))
                return;

            try
            {
                if ((TreeClickNode.Tag.ToString() == "server") & (TreeClickNode.Nodes.Count > 0))
                {
                    TreeClickNode.Nodes.Clear();
                }
            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show("操作失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void toolbtn_Refrush_Click(object sender, EventArgs e)
        {

        }


        #endregion

        #region treeview 鼠标点击

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode SelNode = this.treeView1.SelectedNode;
                string selstr = SelNode.Text;
                string Nodetype = SelNode.Tag.ToString().ToLower();
                mainfrm = (MainForm)Application.OpenForms["MainForm"];

                DbBrowser dbrowfrm = (DbBrowser)Application.OpenForms["DbBrowser"];
                if (dbrowfrm != null)
                {
                    mainfrm.StatusLabel1.Text = "正在检索数据库.....";
                    dbrowfrm.SetListView(this);
                }

                CodeMaker codemakerfrm = (CodeMaker)Application.OpenForms["CodeMaker"];
                if (codemakerfrm != null)
                {
                    mainfrm.StatusLabel1.Text = "正在检索数据库.....";
                    codemakerfrm.SetListView(this);
                }
                CodeMakerM codemakermfrm = (CodeMakerM)Application.OpenForms["CodeMakerM"];
                if (codemakermfrm != null)
                {
                    mainfrm.StatusLabel1.Text = "正在检索数据库.....";
                    codemakermfrm.SetListView(this);
                }

                CodeTemplate codetempfrm = (CodeTemplate)Application.OpenForms["CodeTemplate"];
                if (codetempfrm != null)
                {
                    mainfrm.StatusLabel1.Text = "正在检索数据库.....";
                    codetempfrm.SetListView(this);
                }

                #region  选中某类型节点
                switch (Nodetype)
                {
                    case "serverlist":
                        {
                            mainfrm.toolComboBox_DB.Items.Clear();
                            mainfrm.toolComboBox_Table.Items.Clear();
                            mainfrm.toolComboBox_DB.Visible = false;
                            mainfrm.toolComboBox_Table.Visible = false;

                            mainfrm.生成ToolStripMenuItem.Visible = false;
                        }
                        break;
                    case "server":
                        {
                            mainfrm.toolComboBox_DB.Visible = true;
                            mainfrm.toolComboBox_Table.Visible = false;

                            mainfrm.生成ToolStripMenuItem.Visible = false;
                        }
                        break;
                    case "db":
                        {
                            #region
                            mainfrm.toolComboBox_DB.Visible = true;
                            mainfrm.toolComboBox_Table.Visible = true;
                            mainfrm.toolComboBox_DB.Text = SelNode.Parent.Text;

                            mainfrm.生成ToolStripMenuItem.Visible = false;
                            #endregion
                        }
                        break;
                    case "tableroot":
                    case "viewroot":
                        {
                            #region
                            mainfrm.toolComboBox_DB.Visible = true;
                            mainfrm.toolComboBox_Table.Visible = true;
                            mainfrm.toolComboBox_DB.Text = SelNode.Parent.Text;

                            mainfrm.生成ToolStripMenuItem.Visible = false;
                            #endregion

                        }
                        break;
                    case "table":
                        {
                            #region
                            mainfrm.toolComboBox_DB.Visible = true;
                            mainfrm.toolComboBox_Table.Visible = true;
                            mainfrm.toolComboBox_DB.Text = SelNode.Parent.Parent.Text;
                            mainfrm.toolComboBox_Table.Text = selstr;

                            mainfrm.生成ToolStripMenuItem.Visible = true;
                            mainfrm.对象定义ToolStripMenuItem.Visible = false;
                            mainfrm.toolStripMenuItem17.Visible = false;
                            mainfrm.生成存储过程ToolStripMenuItem.Visible = true;
                            mainfrm.生成数据脚本ToolStripMenuItem.Visible = true;

                            #endregion
                        }
                        break;
                    case "view":
                        {
                            #region
                            mainfrm.toolComboBox_DB.Visible = true;
                            mainfrm.toolComboBox_Table.Visible = true;
                            mainfrm.toolComboBox_DB.Text = SelNode.Parent.Parent.Text;
                            mainfrm.toolComboBox_Table.Text = selstr;

                            mainfrm.生成ToolStripMenuItem.Visible = true;
                            mainfrm.对象定义ToolStripMenuItem.Visible = true;
                            mainfrm.toolStripMenuItem17.Visible = true;
                            mainfrm.生成存储过程ToolStripMenuItem.Visible = false;
                            mainfrm.生成数据脚本ToolStripMenuItem.Visible = false;

                            #endregion
                        }
                        break;
                    case "proc":
                        {
                            mainfrm.生成ToolStripMenuItem.Visible = true;
                            mainfrm.对象定义ToolStripMenuItem.Visible = true;
                            mainfrm.toolStripMenuItem17.Visible = true;
                            mainfrm.生成存储过程ToolStripMenuItem.Visible = false;
                            mainfrm.生成数据脚本ToolStripMenuItem.Visible = false;
                        }
                        break;
                    default:
                        {
                            mainfrm.生成ToolStripMenuItem.Visible = false;
                        }
                        break;
                }
                #endregion

                mainfrm.StatusLabel1.Text = "就绪";
            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Point mpt = new Point(e.X, e.Y);
                TreeClickNode = this.treeView1.GetNodeAt(mpt);
                this.treeView1.SelectedNode = TreeClickNode;
                if (TreeClickNode != null)
                {
                    CreatMenu(TreeClickNode.Tag.ToString());
                    if (e.Button == MouseButtons.Right)
                    {
                        this.DbTreeContextMenu.Show(this.treeView1, mpt);
                        //this.treeView1.ContextMenu.Show(treeView1,mpt);
                    }
                }
                else
                {
                    DbTreeContextMenu.Items.Clear();
                }
            }
            catch(System.Exception ex)
            {
                LogInfo.WriteLog(ex);
            }
        }

        #endregion

        #region 创建treeview 右键菜单
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));

        private void CreatMenu(string NodeType)
        {
            this.DbTreeContextMenu.Items.Clear();
            switch (NodeType)
            {
                case "serverlist":
                    {
                        #region
                        ToolStripMenuItem 添加服务器Item = new ToolStripMenuItem();
                        添加服务器Item.Image = ((System.Drawing.Image)(resources.GetObject("toolbtn_AddServer.Image")));
                        //添加服务器Item.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
                        //添加服务器Item.ImageTransparentColor = System.Drawing.Color.Magenta;
                        添加服务器Item.Name = "添加服务器Item";
                        添加服务器Item.Text = "添加服务器";
                        添加服务器Item.Click += new System.EventHandler(添加服务器Item_Click);

                        ToolStripMenuItem 备份服务器配置Item = new ToolStripMenuItem();
                        备份服务器配置Item.Name = "备份服务器配置Item";
                        备份服务器配置Item.Text = "备份服务器配置";
                        备份服务器配置Item.Click += new System.EventHandler(备份服务器配置Item_Click);

                        ToolStripMenuItem 导入服务器配置Item = new ToolStripMenuItem();
                        导入服务器配置Item.Name = "导入服务器配置Item";
                        导入服务器配置Item.Text = "导入服务器配置";
                        导入服务器配置Item.Click += new System.EventHandler(导入服务器配置Item_Click);

                        ToolStripMenuItem 刷新Item = new ToolStripMenuItem();
                        刷新Item.Name = "刷新Item";
                        刷新Item.Text = "刷新";
                        刷新Item.Click += new System.EventHandler(刷新Item_Click);

                        ToolStripMenuItem 属性Item = new ToolStripMenuItem();
                        属性Item.Name = "属性Item";
                        属性Item.Text = "属性";
                        属性Item.Click += new System.EventHandler(属性Item_Click);

                        DbTreeContextMenu.Items.AddRange(
                            new System.Windows.Forms.ToolStripItem[] { 
                                添加服务器Item, 
                                备份服务器配置Item,
                                导入服务器配置Item,
                                刷新Item                                
                            }
                            );
                        #endregion
                    }
                    break;
                case "server":
                    {
                        #region
                        ToolStripMenuItem 连接服务器Item = new ToolStripMenuItem();
                        连接服务器Item.Image = ((System.Drawing.Image)(resources.GetObject("toolbtn_Connect.Image")));
                        //连接服务器Item.ImageTransparentColor = System.Drawing.Color.Magenta;
                        连接服务器Item.Name = "连接服务器Item";
                        连接服务器Item.Text = "连接服务器";
                        连接服务器Item.Click += new System.EventHandler(连接服务器Item_Click);

                        ToolStripMenuItem 注销服务器Item = new ToolStripMenuItem();
                        注销服务器Item.Name = "注销服务器Item";
                        注销服务器Item.Text = "注销服务器";
                        注销服务器Item.Click += new System.EventHandler(注销服务器Item_Click);

                        ToolStripMenuItem 属性Item = new ToolStripMenuItem();
                        属性Item.Name = "属性Item";
                        属性Item.Text = "刷新";
                        属性Item.Click += new System.EventHandler(server属性Item_Click);

                        DbTreeContextMenu.Items.AddRange(
                            new System.Windows.Forms.ToolStripItem[] { 
                                连接服务器Item, 
                                注销服务器Item,
                                属性Item
                            }
                            );
                        #endregion
                    }
                    break;
                case "db":
                    {
                        #region
                        ToolStripMenuItem 浏览数据库Item = new ToolStripMenuItem();
                        浏览数据库Item.Image = ((System.Drawing.Image)(resources.GetObject("数据库管理器ToolStripMenuItem.Image")));
                        浏览数据库Item.Name = "浏览数据库Item";
                        浏览数据库Item.Text = "浏览数据库";
                        浏览数据库Item.Click += new System.EventHandler(浏览数据库Item_Click);


                        ToolStripMenuItem 新建查询Item = new ToolStripMenuItem();
                        新建查询Item.Image = ((System.Drawing.Image)(resources.GetObject("查询分析器ToolStripMenuItem.Image")));
                        新建查询Item.Name = "新建查询Item";
                        新建查询Item.Text = "新建查询";
                        新建查询Item.Click += new System.EventHandler(新建查询Item_Click);

                        ToolStripMenuItem 新建NET项目Item = new ToolStripMenuItem();
                        新建NET项目Item.Image = ((System.Drawing.Image)(resources.GetObject("toolBtn_NewProject.Image")));
                        新建NET项目Item.Name = "新建NET项目Item";
                        新建NET项目Item.Text = "新建NET项目";
                        新建NET项目Item.Click += new System.EventHandler(新建NET项目Item_Click);

                        ToolStripSeparator Separator1 = new ToolStripSeparator();
                        Separator1.Name = "Separator1";

                        ToolStripMenuItem 生成存储过程dbItem = new ToolStripMenuItem();
                        生成存储过程dbItem.Name = "生成存储过程Item";
                        生成存储过程dbItem.Text = "生成存储过程";
                        生成存储过程dbItem.Click += new System.EventHandler(生成存储过程dbItem_Click);

                        ToolStripMenuItem 生成数据脚本dbItem = new ToolStripMenuItem();
                        生成数据脚本dbItem.Image = ((System.Drawing.Image)(resources.GetObject("dB脚本生成器ToolStripMenuItem.Image")));
                        生成数据脚本dbItem.Name = "生成数据脚本Item";
                        生成数据脚本dbItem.Text = "生成数据脚本";
                        生成数据脚本dbItem.Click += new System.EventHandler(生成数据脚本dbItem_Click);

                        ToolStripMenuItem 生成数据库文档dbItem = new ToolStripMenuItem();
                        生成数据库文档dbItem.Image = ((System.Drawing.Image)(resources.GetObject("生成数据库文档ToolStripMenuItem.Image")));
                        生成数据库文档dbItem.Name = "生成数据库文档dbItem";
                        生成数据库文档dbItem.Text = "生成数据库文档";
                        生成数据库文档dbItem.Click += new System.EventHandler(生成数据库文档dbItem_Click);


                        #region  导出文件

                        ToolStripMenuItem 导出文件dbItem = new ToolStripMenuItem();
                        导出文件dbItem.Name = "导出文件Item";
                        导出文件dbItem.Text = "导出文件";

                        ToolStripMenuItem 存储过程dbItem = new ToolStripMenuItem();
                        存储过程dbItem.Name = "存储过程Item";
                        存储过程dbItem.Text = "存储过程";
                        存储过程dbItem.Click += new System.EventHandler(存储过程dbItem_Click);

                        ToolStripMenuItem 数据脚本dbItem = new ToolStripMenuItem();
                        数据脚本dbItem.Name = "数据脚本Item";
                        数据脚本dbItem.Text = "数据脚本";
                        数据脚本dbItem.Click += new System.EventHandler(数据脚本dbItem_Click);

                        ToolStripMenuItem 表数据dbItem = new ToolStripMenuItem();
                        表数据dbItem.Name = "表数据Item";
                        表数据dbItem.Text = "表数据";
                        表数据dbItem.Click += new System.EventHandler(表数据dbItem_Click);

                        #endregion

                        导出文件dbItem.DropDownItems.AddRange(
                            new System.Windows.Forms.ToolStripItem[] { 
                                存储过程dbItem, 
                                数据脚本dbItem,
                                表数据dbItem                                
                            }
                            );

                        ToolStripSeparator Separator2 = new ToolStripSeparator();
                        Separator1.Name = "Separator2";

                        ToolStripMenuItem 父子表代码生成dbItem = new ToolStripMenuItem();
                        父子表代码生成dbItem.Name = "父子表代码生成Item";
                        父子表代码生成dbItem.Text = "父子表代码生成(事务)";
                        父子表代码生成dbItem.Click += new System.EventHandler(父子表代码生成dbItem_Click);


                        ToolStripMenuItem 多表事务代码生成dbItem = new ToolStripMenuItem();
                        多表事务代码生成dbItem.Name = "多表事务代码生成dbItem";
                        多表事务代码生成dbItem.Text = "多表事务代码生成";
                        多表事务代码生成dbItem.Click += new System.EventHandler(多表事务代码生成dbItem_Click);


                        ToolStripMenuItem 代码批量生成dbItem = new ToolStripMenuItem();
                        代码批量生成dbItem.Image = global::Codematic.Properties.Resources.batchcs2;
                        代码批量生成dbItem.Name = "代码批量生成dbItem";
                        代码批量生成dbItem.Text = "代码批量生成";
                        代码批量生成dbItem.Click += new System.EventHandler(代码批量生成dbItem_Click);

                        ToolStripMenuItem 模板代码批量生成dbItem = new ToolStripMenuItem();
                        模板代码批量生成dbItem.Image = global::Codematic.Properties.Resources.template;
                        模板代码批量生成dbItem.Name = "模板代码批量生成dbItem";
                        模板代码批量生成dbItem.Text = "模板代码批量生成";
                        模板代码批量生成dbItem.Click += new System.EventHandler(模板代码批量生成dbItem_Click);

                        ToolStripSeparator Separatordb2 = new ToolStripSeparator();
                        Separator1.Name = "Separatordb2";

                        ToolStripMenuItem 刷新dbItem = new ToolStripMenuItem();
                        刷新dbItem.Name = "刷新Item";
                        刷新dbItem.Text = "刷新";
                        刷新dbItem.Click += new System.EventHandler(刷新dbItem_Click);


                        DbTreeContextMenu.Items.AddRange(
                            new System.Windows.Forms.ToolStripItem[] { 
                                浏览数据库Item,
                                新建查询Item,
                                新建NET项目Item,
                                Separator1,
                                生成存储过程dbItem,
                                生成数据脚本dbItem,
                                生成数据库文档dbItem,
                                导出文件dbItem,
                                Separator2,   
                                代码批量生成dbItem,
                                父子表代码生成dbItem,
                                多表事务代码生成dbItem,
                                模板代码批量生成dbItem,                                                                
                                Separatordb2,
                                刷新dbItem
                            }
                            );
                        #endregion

                    }
                    break;
                case "tableroot":
                    break;
                case "viewroot":
                    break;
                case "procroot":
                    break;
                case "table":
                    {
                        #region

                        ToolStripMenuItem 生成SQL语句Item = new ToolStripMenuItem();
                        生成SQL语句Item.Name = "生成SQL语句Item";
                        生成SQL语句Item.Text = "生成SQL语句到";

                        #region 生成SQL语句到

                        ToolStripMenuItem SELECTItem = new ToolStripMenuItem();
                        SELECTItem.Name = "SELECTItem";
                        SELECTItem.Text = "SELECT(&S)";
                        SELECTItem.Click += new System.EventHandler(SELECTItem_Click);

                        ToolStripMenuItem UPDATEItem = new ToolStripMenuItem();
                        UPDATEItem.Name = "UPDATEItem";
                        UPDATEItem.Text = "UPDATE(&U)";
                        UPDATEItem.Click += new System.EventHandler(UPDATEItem_Click);

                        ToolStripMenuItem DELETEItem = new ToolStripMenuItem();
                        DELETEItem.Name = "DELETEItem";
                        DELETEItem.Text = "DELETE(&D)";
                        DELETEItem.Click += new System.EventHandler(DELETEItem_Click);

                        ToolStripMenuItem INSERTItem = new ToolStripMenuItem();
                        INSERTItem.Name = "INSERTItem";
                        INSERTItem.Text = "INSERT(&I)";
                        INSERTItem.Click += new System.EventHandler(INSERTItem_Click);

                        生成SQL语句Item.DropDownItems.AddRange(
                            new System.Windows.Forms.ToolStripItem[] { 
                                SELECTItem, 
                                UPDATEItem,
                                DELETEItem,
                                INSERTItem
                            }
                            );

                        #endregion

                        ToolStripMenuItem 查看表数据tabItem = new ToolStripMenuItem();
                        查看表数据tabItem.Name = "查看表数据tabItem";
                        查看表数据tabItem.Text = "浏览表数据";
                        查看表数据tabItem.Click += new System.EventHandler(查看表数据tabItem_Click);

                        ToolStripMenuItem 生成数据脚本tabItem = new ToolStripMenuItem();
                        生成数据脚本tabItem.Image = ((System.Drawing.Image)(resources.GetObject("dB脚本生成器ToolStripMenuItem.Image")));
                        生成数据脚本tabItem.Name = "生成数据脚本tabItem";
                        生成数据脚本tabItem.Text = "生成数据脚本";
                        生成数据脚本tabItem.Click += new System.EventHandler(生成数据脚本tabItem_Click);

                        ToolStripMenuItem 生成存储过程tabItem = new ToolStripMenuItem();
                        生成存储过程tabItem.Name = "生成存储过程tabItem";
                        生成存储过程tabItem.Text = "生成存储过程";
                        生成存储过程tabItem.Click += new System.EventHandler(生成存储过程tabItem_Click);

                        #region 导出文件Item
                        ToolStripMenuItem 导出文件tabItem = new ToolStripMenuItem();
                        导出文件tabItem.Name = "导出文件tabItem";
                        导出文件tabItem.Text = "导出文件";

                        ToolStripMenuItem 存储过程tabItem = new ToolStripMenuItem();
                        存储过程tabItem.Name = "存储过程tabItem";
                        存储过程tabItem.Text = "存储过程";
                        存储过程tabItem.Click += new System.EventHandler(存储过程tabItem_Click);

                        ToolStripMenuItem 数据脚本tabItem = new ToolStripMenuItem();
                        数据脚本tabItem.Name = "数据脚本tabItem";
                        数据脚本tabItem.Text = "数据脚本";
                        数据脚本tabItem.Click += new System.EventHandler(数据脚本tabItem_Click);

                        ToolStripMenuItem 表数据tabItem = new ToolStripMenuItem();
                        表数据tabItem.Name = "表数据tabItem";
                        表数据tabItem.Text = "表数据";
                        表数据tabItem.Click += new System.EventHandler(表数据tabItem_Click);

                        导出文件tabItem.DropDownItems.AddRange(
                            new System.Windows.Forms.ToolStripItem[] { 
                                存储过程tabItem, 
                                数据脚本tabItem
                                //表数据tabItem                                
                            }
                            );
                        #endregion


                        ToolStripSeparator Separator1 = new ToolStripSeparator();
                        Separator1.Name = "Separator1";

                        #region 代码生成Item

                        ToolStripMenuItem 代码生成Item = new ToolStripMenuItem();
                        代码生成Item.Image = ((System.Drawing.Image)(resources.GetObject("代码生成器ToolStripMenuItem.Image")));
                        代码生成Item.Name = "代码生成Item";
                        代码生成Item.Text = "单表代码生成器";
                        代码生成Item.Click += new System.EventHandler(代码生成Item_Click);

                        ToolStripMenuItem 模版代码生成Item = new ToolStripMenuItem();
                        模版代码生成Item.Image = global::Codematic.Properties.Resources.template;
                        模版代码生成Item.Name = "模版代码生成Item";
                        模版代码生成Item.Text = "模版代码生成";
                        模版代码生成Item.Click += new System.EventHandler(模版代码生成Item_Click);

                        ToolStripMenuItem 生成单类结构Item = new ToolStripMenuItem();
                        生成单类结构Item.Name = "生成单类结构Item";
                        生成单类结构Item.Text = "生成单类结构";
                        生成单类结构Item.Click += new System.EventHandler(生成单类结构Items_Click);

                        ToolStripMenuItem 生成ModelItem = new ToolStripMenuItem();
                        生成ModelItem.Name = "生成ModelItem";
                        生成ModelItem.Text = "生成Model";
                        生成ModelItem.Click += new System.EventHandler(生成ModelItem_Click);

                        ToolStripSeparator Separator3 = new ToolStripSeparator();
                        Separator3.Name = "Separator3";


                        ToolStripMenuItem 简单三层Item = new ToolStripMenuItem();
                        简单三层Item.Name = "简单三层Item";
                        简单三层Item.Text = "简单三层";

                        #region 简单三层

                        ToolStripMenuItem 生成DALS3Item = new ToolStripMenuItem();
                        生成DALS3Item.Name = "生成DALS3Item";
                        生成DALS3Item.Text = "生成DAL";
                        生成DALS3Item.Click += new System.EventHandler(生成DALS3Item_Click);

                        ToolStripMenuItem 生成BLLS3Item = new ToolStripMenuItem();
                        生成BLLS3Item.Name = "生成BLLS3Item";
                        生成BLLS3Item.Text = "生成BLL";
                        生成BLLS3Item.Click += new System.EventHandler(生成BLLS3Item_Click);

                        ToolStripMenuItem 生成全部S3Item = new ToolStripMenuItem();
                        生成全部S3Item.Name = "生成全部S3";
                        生成全部S3Item.Text = "生成全部";
                        生成全部S3Item.Click += new System.EventHandler(生成全部S3Item_Click);

                        简单三层Item.DropDownItems.AddRange(
                            new System.Windows.Forms.ToolStripItem[] { 
                                生成DALS3Item, 
                                生成BLLS3Item,
                                生成全部S3Item                                
                            }
                            );

                        #endregion


                        ToolStripMenuItem 工厂模式三层Item = new ToolStripMenuItem();
                        工厂模式三层Item.Name = "工厂模式三层Item";
                        工厂模式三层Item.Text = "工厂模式三层";

                        #region 工厂模式三层

                        ToolStripMenuItem 生成DALF3Item = new ToolStripMenuItem();
                        生成DALF3Item.Name = "生成DALF3Item";
                        生成DALF3Item.Text = "生成DAL";
                        生成DALF3Item.Click += new System.EventHandler(生成DALF3Item_Click);

                        ToolStripMenuItem 生成IDALItem = new ToolStripMenuItem();
                        生成IDALItem.Name = "生成IDALItem";
                        生成IDALItem.Text = "生成DAL";
                        生成IDALItem.Click += new System.EventHandler(生成IDALItem_Click);

                        ToolStripMenuItem 生成DALFactoryItem = new ToolStripMenuItem();
                        生成DALFactoryItem.Name = "生成DALFactoryItem";
                        生成DALFactoryItem.Text = "生成DALFactory";
                        生成DALFactoryItem.Click += new System.EventHandler(生成DALFactoryItem_Click);

                        ToolStripMenuItem 生成BLLF3Item = new ToolStripMenuItem();
                        生成BLLF3Item.Name = "生成BLLF3Item";
                        生成BLLF3Item.Text = "生成BLL";
                        生成BLLF3Item.Click += new System.EventHandler(生成BLLF3Item_Click);

                        ToolStripMenuItem 生成全部F3Item = new ToolStripMenuItem();
                        生成全部F3Item.Name = "生成全部F3Item";
                        生成全部F3Item.Text = "生成全部";
                        生成全部F3Item.Click += new System.EventHandler(生成全部F3Item_Click);

                        工厂模式三层Item.DropDownItems.AddRange(
                            new System.Windows.Forms.ToolStripItem[] { 
                                生成DALF3Item, 
                                生成IDALItem,
                                生成DALFactoryItem,
                                生成BLLF3Item,
                                生成全部F3Item
                            }
                            );


                        #endregion

                        ToolStripSeparator Separator5 = new ToolStripSeparator();
                        Separator5.Name = "Separator5";

                        ToolStripMenuItem 生成页面Item = new ToolStripMenuItem();
                        生成页面Item.Name = "生成页面Item";
                        生成页面Item.Text = "生成页面";
                        生成页面Item.Click += new System.EventHandler(生成页面Item_Click);

                        //代码生成Item.DropDownItems.AddRange(
                        //    new System.Windows.Forms.ToolStripItem[] { 
                        //        生成单类结构Item,
                        //        生成ModelItem, 
                        //        Separator3,
                        //        简单三层Item,
                        //        工厂模式三层Item,
                        //        Separator5,
                        //        生成页面Item
                        //    }
                        //    );

                        #endregion

                        ToolStripSeparator Separator2 = new ToolStripSeparator();
                        Separator2.Name = "Separator2";

                        ToolStripSeparator Separator6 = new ToolStripSeparator();
                        Separator5.Name = "Separator6";


                        ToolStripMenuItem 父子表代码生成Item = new ToolStripMenuItem();
                        父子表代码生成Item.Name = "父子表代码生成Item";
                        父子表代码生成Item.Text = "父子表代码生成(事务)";
                        父子表代码生成Item.Click += new System.EventHandler(父子表代码生成Item_Click);

                        ToolStripMenuItem 代码批量生成Item = new ToolStripMenuItem();
                        代码批量生成Item.Image = global::Codematic.Properties.Resources.batchcs2;
                        代码批量生成Item.Name = "代码批量生成";
                        代码批量生成Item.Text = "代码批量生成";
                        代码批量生成Item.Click += new System.EventHandler(代码批量生成Item_Click);

                        ToolStripMenuItem 模板代码批量生成Item = new ToolStripMenuItem();
                        模板代码批量生成Item.Image = global::Codematic.Properties.Resources.batchcs2;
                        模板代码批量生成Item.Name = "模板代码批量生成Item";
                        模板代码批量生成Item.Text = "模板代码批量生成";
                        模板代码批量生成Item.Click += new System.EventHandler(模板代码批量生成Item_Click);

                        ToolStripSeparator Separator4 = new ToolStripSeparator();
                        Separator4.Name = "Separator4";

                        ToolStripMenuItem 重命名tabItem = new ToolStripMenuItem();
                        重命名tabItem.Name = "重命名tabItem";
                        重命名tabItem.Text = "重命名";
                        重命名tabItem.Click += new System.EventHandler(重命名tabItem_Click);

                        ToolStripMenuItem 删除tabItem = new ToolStripMenuItem();
                        删除tabItem.Name = "删除tabItem";
                        删除tabItem.Text = "删除";
                        删除tabItem.Click += new System.EventHandler(删除tabItem_Click);

                        DbTreeContextMenu.Items.AddRange(
                            new System.Windows.Forms.ToolStripItem[] {                                 
                                生成SQL语句Item,
                                查看表数据tabItem,
                                生成数据脚本tabItem,
                                生成存储过程tabItem,                               
                                导出文件tabItem,
                                Separator1,
                                代码生成Item,
                                父子表代码生成Item,
                                代码批量生成Item,                                
                                 Separator6,
                                模版代码生成Item,  
                                模板代码批量生成Item,
                                Separator4,
                                重命名tabItem,
                                删除tabItem
                            }
                            );

                        #endregion
                    }
                    break;
                case "view":
                    {
                        #region
                        ToolStripMenuItem 脚本Item = new ToolStripMenuItem();
                        脚本Item.Name = "脚本Item";
                        脚本Item.Text = "脚本";

                        ToolStripMenuItem SELECTviewItem = new ToolStripMenuItem();
                        SELECTviewItem.Name = "SELECTItem";
                        SELECTviewItem.Text = "SELECT(&S)";
                        //SELECTviewItem.Click += new System.EventHandler(SELECTviewItem_Click);

                        ToolStripMenuItem AlterItem = new ToolStripMenuItem();
                        AlterItem.Name = "AlterItem";
                        AlterItem.Text = "ALTER(&U)";
                        //AlterItem.Click += new System.EventHandler(AlterItem_Click);

                        ToolStripMenuItem DropItem = new ToolStripMenuItem();
                        DropItem.Name = "DropItem";
                        DropItem.Text = "DROP(&D)";
                        //DropItem.Click += new System.EventHandler(DropItem_Click);


                        脚本Item.DropDownItems.AddRange(
                            new System.Windows.Forms.ToolStripItem[] { 
                                SELECTviewItem, 
                                AlterItem,
                                DropItem                               
                            }
                            );


                        ToolStripMenuItem 对象定义Item = new ToolStripMenuItem();
                        对象定义Item.Name = "对象定义Item";
                        对象定义Item.Text = "对象定义";
                        对象定义Item.Click += new System.EventHandler(对象定义Item_Click);

                        ToolStripMenuItem 查看表数据tabItem = new ToolStripMenuItem();
                        查看表数据tabItem.Name = "查看表数据tabItem";
                        查看表数据tabItem.Text = "浏览表数据";
                        查看表数据tabItem.Click += new System.EventHandler(查看表数据tabItem_Click);

                        ToolStripSeparator Separatorv1 = new ToolStripSeparator();
                        Separatorv1.Name = "Separatorv1";


                        ToolStripMenuItem 代码生成Item = new ToolStripMenuItem();
                        代码生成Item.Image = ((System.Drawing.Image)(resources.GetObject("代码生成器ToolStripMenuItem.Image")));
                        代码生成Item.Name = "代码生成Item";
                        代码生成Item.Text = "单表代码生成器";
                        代码生成Item.Click += new System.EventHandler(代码生成Item_Click);

                        ToolStripMenuItem 模版代码生成Item = new ToolStripMenuItem();
                        模版代码生成Item.Image = global::Codematic.Properties.Resources.template;
                        模版代码生成Item.Name = "模版代码生成Item";
                        模版代码生成Item.Text = "模版代码生成";
                        模版代码生成Item.Click += new System.EventHandler(模版代码生成Item_Click);


                        DbTreeContextMenu.Items.AddRange(
                            new System.Windows.Forms.ToolStripItem[] { 
                                脚本Item, 
                                对象定义Item,
                                查看表数据tabItem,
                                Separatorv1,
                                代码生成Item,
                                模版代码生成Item
                            }
                            );
                        #endregion
                    }
                    break;
                case "proc":
                    {
                        #region
                        ToolStripMenuItem 脚本Item = new ToolStripMenuItem();
                        脚本Item.Name = "脚本Item";
                        脚本Item.Text = "修改";
                        脚本Item.Click += new System.EventHandler(AlterItem_Click);


                        ToolStripMenuItem AlterItem = new ToolStripMenuItem();
                        AlterItem.Name = "AlterItem";
                        AlterItem.Text = "ALTER(&U)";
                        //AlterItem.Click += new System.EventHandler(AlterItem_Click);

                        ToolStripMenuItem DropItem = new ToolStripMenuItem();
                        DropItem.Name = "DropItem";
                        DropItem.Text = "DROP(&D)";
                        //DropItem.Click += new System.EventHandler(DropItem_Click);

                        ToolStripMenuItem EXECItem = new ToolStripMenuItem();
                        EXECItem.Name = "EXECItem";
                        EXECItem.Text = "EXEC(&I)";
                        //EXECItem.Click += new System.EventHandler(EXECItem_Click);

                        //脚本Item.DropDownItems.AddRange(
                        //    new System.Windows.Forms.ToolStripItem[] {                                 
                        //        AlterItem,
                        //        DropItem,
                        //        EXECItem
                        //    }
                        //    );

                        ToolStripMenuItem 对象定义Item = new ToolStripMenuItem();
                        对象定义Item.Name = "对象定义Item";
                        对象定义Item.Text = "对象定义";
                        对象定义Item.Click += new System.EventHandler(对象定义Item_Click);
                        
                        ToolStripMenuItem 模版代码生成Item = new ToolStripMenuItem();
                        模版代码生成Item.Image = global::Codematic.Properties.Resources.template;
                        模版代码生成Item.Name = "模版代码生成Item";
                        模版代码生成Item.Text = "模版代码生成";
                        模版代码生成Item.Click += new System.EventHandler(模版代码生成Item_Click);

                        ToolStripMenuItem 模板代码批量生成ProcItem = new ToolStripMenuItem();
                        模板代码批量生成ProcItem.Image = global::Codematic.Properties.Resources.batchcs2;
                        模板代码批量生成ProcItem.Name = "模板代码批量生成Item";
                        模板代码批量生成ProcItem.Text = "模板代码批量生成";
                        模板代码批量生成ProcItem.Click += new System.EventHandler(模板代码批量生成ProcItem_Click);

                        ToolStripSeparator Separatorv1 = new ToolStripSeparator();
                        Separatorv1.Name = "Separatorv1";

                        DbTreeContextMenu.Items.AddRange(
                            new System.Windows.Forms.ToolStripItem[] { 
                                脚本Item, 
                                对象定义Item,
                                Separatorv1,
                                模版代码生成Item,
                                模板代码批量生成ProcItem
                            }
                            );
                        #endregion
                    }
                    break;
                case "column":
                    break;
            }
        }

        #endregion

        #region  treeview 右键菜单事件

        #region serverlist_click

        private void 添加服务器Item_Click(object sender, EventArgs e)
        {
            backgroundWorkerReg.RunWorkerAsync();
        }
        private void 备份服务器配置Item_Click(object sender, EventArgs e)
        {
            SaveFileDialog sqlsavedlg = new SaveFileDialog();
            sqlsavedlg.Title = "保存服务器配置";
            sqlsavedlg.Filter = "DB Serverlist(*.config)|*.config|All files (*.*)|*.*";
            DialogResult dlgresult = sqlsavedlg.ShowDialog(this);
            if (dlgresult == DialogResult.OK)
            {
                string filename = sqlsavedlg.FileName;
                DataSet ds = Maticsoft.CmConfig.DbConfig.GetSettingDs();
                ds.WriteXml(filename);
            }
        }
        private void 导入服务器配置Item_Click(object sender, EventArgs e)
        {
            OpenFileDialog sqlfiledlg = new OpenFileDialog();
            sqlfiledlg.Title = "选择服务器配置文件";
            sqlfiledlg.Filter = "DB Serverlist(*.config)|*.config|All files (*.*)|*.*";
            DialogResult result = sqlfiledlg.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                try
                {
                    string filename = sqlfiledlg.FileName;
                    DataSet ds = new DataSet();
                    if (File.Exists(filename))
                    {
                        ds.ReadXml(filename);
                        string fileNamelocal = Application.StartupPath + "\\DbSetting.config";
                        ds.WriteXml(fileNamelocal);
                    }
                    LoadServer();
                }
                catch(System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show("读取配置文件失败！", "提示");
                }
            }
        }
        private void 刷新Item_Click(object sender, EventArgs e)
        {
            LoadServer();
        }
        private void 属性Item_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("刷新Item");
        }
        #endregion

        #region server_click

        private void 连接服务器Item_Click(object sender, EventArgs e)
        {
            backgroundWorkerCon.RunWorkerAsync();
        }

        private void 注销服务器Item_Click(object sender, EventArgs e)
        {
            try
            {
                if (TreeClickNode != null)
                {
                    string nodetext = TreeClickNode.Text;
                    Maticsoft.CmConfig.DbSettings dbset = Maticsoft.CmConfig.DbConfig.GetSetting(nodetext);
                    if (dbset != null)
                    {
                        Maticsoft.CmConfig.DbConfig.DelSetting(dbset.DbType, dbset.Server, dbset.DbName);
                    }
                    serverlistNode.Nodes.Remove(TreeClickNode);
                }
            }
            catch
            {
                MessageBox.Show("注销服务器失败，请关闭后重新打开再试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        //刷新服务器
        private void server属性Item_Click(object sender, EventArgs e)
        {
            backgroundWorkerCon.RunWorkerAsync();
        }
        #endregion

        #region db_click
        private void 浏览数据库Item_Click(object sender, EventArgs e)
        {
            AddSinglePage(new DbBrowser(), "摘要");

        }
        private void 新建查询Item_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                //在combox选中当前库
                string dbname = TreeClickNode.Text;
                string server = TreeClickNode.Parent.Text;
                string title = server + "." + dbname + "查询.sql";
                MainForm mainfrm = (MainForm)Application.OpenForms["MainForm"];
                AddTabPage(title, new DbQuery(mainfrm, ""), mainfrm);
                mainfrm.toolComboBox_DB.Text = dbname;
            }

        }
        private void 新建NET项目Item_Click(object sender, EventArgs e)
        {
            NewProject newpro = new NewProject();
            newpro.ShowDialog(mainfrm);
        }
        private void 生成存储过程dbItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Text;
                string dbname = TreeClickNode.Text;
                dbset = Maticsoft.CmConfig.DbConfig.GetSetting(longservername);
                Maticsoft.IDBO.IDbScriptBuilder dsb = ObjHelper.CreatDsb(longservername);
                dsb.ProcPrefix = dbset.ProcPrefix;
                dsb.ProjectName = dbset.ProjectName;
                string strSQL = dsb.GetPROCCode(dbname);
                string title = dbname + "存储过程.sql";
                AddTabPage(title, new DbQuery(mainfrm, strSQL));
            }
        }
        private void 生成数据脚本dbItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Text;
                string dbname = TreeClickNode.Text;
                DbToScript dts = new DbToScript(longservername, dbname);
                dts.ShowDialog(this);
            }
        }
        private void 生成数据库文档dbItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Text;
                string dbname = TreeClickNode.Text;                                                
                if (longservername == "")
                {
                    MessageBox.Show("没有可用的数据库连接，请先连接数据库服务器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                try
                {
                    DbToWord dbtoword = new DbToWord(longservername);
                    dbtoword.Show();
                }
                catch
                {
                    DialogResult dr = MessageBox.Show("启动文档生成器失败，请检查是否安装了Office软件，并确保左侧连接了数据库。\r\n 但你可以选择生成网页格式的文档，你要生成吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        DbToWeb dbtoweb = new DbToWeb(longservername);
                        dbtoweb.Show();
                    }
                }
            }
        }
        private void 存储过程dbItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sqlsavedlg = new SaveFileDialog();
            sqlsavedlg.Title = "保存当前脚本";
            sqlsavedlg.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
            DialogResult dlgresult = sqlsavedlg.ShowDialog(this);
            if (dlgresult == DialogResult.OK)
            {
                if (TreeClickNode != null)
                {
                    
                    string longservername = TreeClickNode.Parent.Text;
                    string dbname = TreeClickNode.Text;
                    dbset = Maticsoft.CmConfig.DbConfig.GetSetting(longservername);

                    Maticsoft.IDBO.IDbScriptBuilder dsb = ObjHelper.CreatDsb(longservername);
                    dsb.ProcPrefix = dbset.ProcPrefix;
                    dsb.ProjectName = dbset.ProjectName;
                    string strSQL = dsb.GetPROCCode(dbname);

                    string filename = sqlsavedlg.FileName;
                    StreamWriter sw = new StreamWriter(filename, false, Encoding.Default);//,false);
                    sw.Write(strSQL);
                    sw.Flush();//从缓冲区写入基础流（文件）
                    sw.Close();
                    MessageBox.Show("脚本生成成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }


        }
        private void 数据脚本dbItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Text;
                string dbname = TreeClickNode.Text;
                DbToScript dts = new DbToScript(longservername, dbname);
                dts.ShowDialog(this);
            }

        }
        private void 表数据dbItem_Click(object sender, EventArgs e)
        {

        }

        //代码生成器
        private void 代码生成dbItem_Click(object sender, EventArgs e)
        {
            string longservername = TreeClickNode.Parent.Text;
            if (longservername == "")
                return;
            mainfrm.AddSinglePage(new CodeMaker(), "代码生成器");
        }

        private void 父子表代码生成dbItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Text;
                string dbname = TreeClickNode.Text;
                if (longservername == "")
                    return;
                mainfrm.AddSinglePage(new CodeMakerM(dbname), "父子表代码生成");
            }
        }
        private void 多表事务代码生成dbItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Text;
                string dbname = TreeClickNode.Text;
                if (longservername == "")
                    return;
                mainfrm.AddSinglePage(new CodeMakerTran(dbname), "多表事务代码生成");
            }
        }
        private void 代码批量生成dbItem_Click(object sender, EventArgs e)
        {
            string longservername = TreeClickNode.Parent.Text;
            if (longservername == "")
                return;
            CodeExport ce = new CodeExport(longservername);
            ce.ShowDialog(this);
        }
        private void 模板代码批量生成dbItem_Click(object sender, EventArgs e)
        {
            string longservername = TreeClickNode.Parent.Text;
            if (longservername == "")
                return;
            TemplateBatch tb = new TemplateBatch(longservername,false);
            tb.ShowDialog(this);
        }

        private void 刷新dbItem_Click(object sender, EventArgs e)
        {
            TreeNode tn = TreeClickNode;
            string servercfg = tn.Parent.Text;
            Maticsoft.CmConfig.DbSettings dbset = Maticsoft.CmConfig.DbConfig.GetSetting(servercfg);
            string server = dbset.Server;
            string dbtype = dbset.DbType;
            //string dbname = dbset.DbName;  

            tn.Nodes.Clear();

            Maticsoft.IDBO.IDbObject dbobj2 = Maticsoft.DBFactory.DBOMaker.CreateDbObj(dbtype);
            dbobj2.DbConnectStr = dbset.ConnectStr;

            string dbname = tn.Text;
            mainfrm.StatusLabel1.Text = "加载数据库" + dbname + "...";

            TreeNode tabNode = new TreeNode("表");
            tabNode.ImageIndex = 3;
            tabNode.SelectedImageIndex = 4;
            tabNode.Tag = "tableroot";
            tn.Nodes.Add(tabNode);

            TreeNode viewNode = new TreeNode("视图");
            viewNode.ImageIndex = 3;
            viewNode.SelectedImageIndex = 4;
            viewNode.Tag = "viewroot";
            tn.Nodes.Add(viewNode);

            TreeNode procNode = new TreeNode("存储过程");
            procNode.ImageIndex = 3;
            procNode.SelectedImageIndex = 4;
            procNode.Tag = "procroot";
            tn.Nodes.Add(procNode);

            #region 表

            try
            {
                List<string> tabNames = dbobj2.GetTables(dbname);
                if (tabNames.Count > 0)
                {
                    //DataRow[] dRows = dt.Select("", "name ASC");
                    foreach (string tabname in tabNames)
                    {
                        TreeNode tbNode = new TreeNode(tabname);
                        tbNode.ImageIndex = 5;
                        tbNode.SelectedImageIndex = 5;
                        tbNode.Tag = "table";
                        tabNode.Nodes.Add(tbNode);

                        //加字段信息
                        List<ColumnInfo> collist = dbobj2.GetColumnList(dbname, tabname);
                        if ((collist != null) && (collist.Count > 0))
                        {
                            foreach (ColumnInfo col in collist)
                            {
                                string columnName = col.ColumnName;
                                string columnType = col.TypeName;
                                TreeNode colNode = new TreeNode(columnName + "[" + columnType + "]");
                                colNode.ImageIndex = 7;
                                colNode.SelectedImageIndex = 7;
                                colNode.Tag = "column";
                                tbNode.Nodes.Add(colNode);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show(this, "获取数据库" + dbname + "的表信息失败：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

            #region	视图

            try
            {
                DataTable dtv = dbobj2.GetVIEWs(dbname);
                if (dtv != null)
                {
                    DataRow[] dRows = dtv.Select("", "name ASC");
                    foreach (DataRow row in dRows)//循环每个表
                    {
                        string tabname = row["name"].ToString();
                        TreeNode tbNode = new TreeNode(tabname);
                        tbNode.ImageIndex = 6;
                        tbNode.SelectedImageIndex = 6;
                        tbNode.Tag = "view";
                        viewNode.Nodes.Add(tbNode);

                        //加字段信息
                        List<ColumnInfo> collist = dbobj2.GetColumnList(dbname, tabname);
                        if ((collist != null) && (collist.Count > 0))
                        {
                            foreach (ColumnInfo col in collist)
                            {
                                string columnName = col.ColumnName;
                                string columnType = col.TypeName;
                                TreeNode colNode = new TreeNode(columnName + "[" + columnType + "]");
                                colNode.ImageIndex = 7;
                                colNode.SelectedImageIndex = 7;
                                colNode.Tag = "column";
                                tbNode.Nodes.Add(colNode);
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show(this, "获取数据库" + dbname + "的视图信息失败：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

            #region 存储过程
            try
            {
                List<string> namelist = dbobj2.GetProcs(dbname);
                foreach (string spname in namelist)
                {
                    TreeNode tbNode = new TreeNode(spname);
                    tbNode.ImageIndex = 8;
                    tbNode.SelectedImageIndex = 8;
                    tbNode.Tag = "proc";
                    procNode.Nodes.Add(tbNode);

                    //加字段参数信息
                    List<ColumnInfo> collist = dbobj2.GetColumnList(dbname, spname);
                    if ((collist != null) && (collist.Count > 0))
                    {
                        foreach (ColumnInfo col in collist)
                        {
                            string columnName = col.ColumnName;
                            string columnType = col.TypeName;
                            TreeNode colNode = new TreeNode(columnName + "[" + columnType + "]");
                            colNode.ImageIndex = 9;
                            colNode.SelectedImageIndex = 9;
                            colNode.Tag = "column";
                            tbNode.Nodes.Add(colNode);
                        }
                    }
                }
                //if (dtp != null)
                //{
                //    DataRow[] dRows = dtp.Select("", "name ASC");
                //    foreach (DataRow row in dRows)//循环每个表
                //    {
                //string tabname = row["name"].ToString();
                //    }
                //}
            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show(this, "获取数据库" + dbname + "的视图信息失败：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion

            mainfrm.StatusLabel1.Text = "就绪";

        }
        #endregion

        #region table_click

        private void SELECTItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = TreeClickNode.Parent.Parent.Text;
                string tabname = TreeClickNode.Text;
                Maticsoft.IDBO.IDbScriptBuilder dsb = ObjHelper.CreatDsb(longservername);
                string strSQL = dsb.GetSQLSelect(dbname, tabname);
                string title = tabname + "查询.sql";

                AddTabPage(title, new DbQuery(mainfrm, strSQL));
            }
        }
        private void UPDATEItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {

                string longservername = TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = TreeClickNode.Parent.Parent.Text;
                string tabname = TreeClickNode.Text;
                Maticsoft.IDBO.IDbScriptBuilder dsb = ObjHelper.CreatDsb(longservername);
                string strSQL = dsb.GetSQLUpdate(dbname, tabname);
                string title = tabname + "查询.sql";
                //MainForm frm = (MainForm)MdiParentForm;
                AddTabPage(title, new DbQuery(mainfrm, strSQL));
            }

        }
        private void DELETEItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = TreeClickNode.Parent.Parent.Text;
                string tabname = TreeClickNode.Text;
                Maticsoft.IDBO.IDbScriptBuilder dsb = ObjHelper.CreatDsb(longservername);
                string strSQL = dsb.GetSQLDelete(dbname, tabname);
                string title = tabname + "查询.sql";
                //MainForm frm = (MainForm)MdiParentForm;
                AddTabPage(title, new DbQuery(mainfrm, strSQL));
            }
        }
        private void INSERTItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = TreeClickNode.Parent.Parent.Text;
                string tabname = TreeClickNode.Text;
                Maticsoft.IDBO.IDbScriptBuilder dsb = ObjHelper.CreatDsb(longservername);
                string strSQL = dsb.GetSQLInsert(dbname, tabname);
                string title = tabname + "查询.sql";
                AddTabPage(title, new DbQuery(mainfrm, strSQL));
            }

        }

        private void 查看表数据tabItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = TreeClickNode.Parent.Parent.Text;
                string tabname = TreeClickNode.Text;
                Maticsoft.IDBO.IDbObject dbobj = ObjHelper.CreatDbObj(longservername);
                DataList dl = new DataList(dbobj, dbname, tabname);
                AddTabPage(tabname, dl, mainfrm);
            }
        }

        private void 生成存储过程tabItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = TreeClickNode.Parent.Parent.Text;
                string tabname = TreeClickNode.Text;
                Maticsoft.IDBO.IDbScriptBuilder dsb = ObjHelper.CreatDsb(longservername);

                dbset = Maticsoft.CmConfig.DbConfig.GetSetting(longservername);

                dsb.DbName = dbname;
                dsb.TableName = tabname;
                dsb.ProjectName = dbset.ProjectName;
                dsb.ProcPrefix = dbset.ProcPrefix;
                dsb.Keys = new List<Maticsoft.CodeHelper.ColumnInfo>();
                dsb.Fieldlist = new List<Maticsoft.CodeHelper.ColumnInfo>();

                string strSQL = dsb.GetPROCCode(dbname, tabname);
                string title = tabname + "存储过程.sql";
                AddTabPage(title, new DbQuery(mainfrm, strSQL));
            }

        }
        private void 生成数据脚本tabItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(this, "如果该表数据量较大，直接生成将需要比较长的时间，\r\n确实需要直接生成吗？\r\n(建议采用脚本生成器生成。)", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                if (TreeClickNode != null)
                {
                    string longservername = TreeClickNode.Parent.Parent.Parent.Text;
                    string dbname = TreeClickNode.Parent.Parent.Text;
                    string tabname = TreeClickNode.Text;
                    Maticsoft.IDBO.IDbScriptBuilder dsb = ObjHelper.CreatDsb(longservername);
                    dsb.Fieldlist = new List<Maticsoft.CodeHelper.ColumnInfo>();
                    string strSQL = dsb.CreateTabScript(dbname, tabname);
                    string title = tabname + "脚本.sql";
                    AddTabPage(title, new DbQuery(mainfrm, strSQL));
                }
            }
            if (dr == DialogResult.No)
            {
                if (TreeClickNode != null)
                {
                    string longservername = TreeClickNode.Parent.Parent.Parent.Text;
                    string dbname = TreeClickNode.Parent.Parent.Text;
                    string tabname = TreeClickNode.Text;
                    DbToScript dts = new DbToScript(longservername, dbname);
                    dts.ShowDialog(this);
                }
            }


        }

        //导出文件
        private void 存储过程tabItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sqlsavedlg = new SaveFileDialog();
            sqlsavedlg.Title = "保存当前脚本";
            sqlsavedlg.Filter = "sql files (*.sql)|*.sql|All files (*.*)|*.*";
            DialogResult dlgresult = sqlsavedlg.ShowDialog(this);
            if (dlgresult == DialogResult.OK)
            {
                if (TreeClickNode == null)
                {
                    return;
                }
                string longservername = TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = TreeClickNode.Parent.Parent.Text;
                string tabname = TreeClickNode.Text;
                Maticsoft.IDBO.IDbScriptBuilder dsb = ObjHelper.CreatDsb(longservername);

                dbset = Maticsoft.CmConfig.DbConfig.GetSetting(longservername);

                dsb.DbName = dbname;
                dsb.TableName = tabname;
                dsb.ProjectName = dbset.ProjectName;
                dsb.ProcPrefix = dbset.ProcPrefix;
                dsb.Keys = new List<Maticsoft.CodeHelper.ColumnInfo>();
                dsb.Fieldlist = new List<Maticsoft.CodeHelper.ColumnInfo>();

                string strSQL = dsb.GetPROCCode(dbname, tabname);

                string filename = sqlsavedlg.FileName;
                StreamWriter sw = new StreamWriter(filename, false, Encoding.Default);//,false);
                sw.Write(strSQL);
                sw.Flush();//从缓冲区写入基础流（文件）
                sw.Close();
                MessageBox.Show("脚本生成成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        //导出文件
        private void 数据脚本tabItem_Click(object sender, EventArgs e)
        {
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            string dbname = TreeClickNode.Parent.Parent.Text;
            DbToScript dts = new DbToScript(longservername, dbname);
            dts.ShowDialog(this);
        }
        //导出文件
        private void 表数据tabItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode == null)
            {
                return;
            }
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            string dbname = TreeClickNode.Parent.Parent.Text;
            string tabname = TreeClickNode.Text;
            Maticsoft.IDBO.IDbObject dbobj = ObjHelper.CreatDbObj(longservername);

        }


        //代码生成器
        private void 代码生成Item_Click(object sender, EventArgs e)
        {
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            if (longservername == "")
                return;
            mainfrm.AddSinglePage(new CodeMaker(), "代码生成器");
        }

        private void 父子表代码生成Item_Click(object sender, EventArgs e)
        {
            if (TreeClickNode != null)
            {
                string longservername = TreeClickNode.Parent.Parent.Parent.Text;
                string dbname = TreeClickNode.Parent.Parent.Text;
                if (longservername == "")
                    return;
                mainfrm.AddSinglePage(new CodeMakerM(dbname), "父子表代码生成");
            }
        }

        private void 代码批量生成Item_Click(object sender, EventArgs e)
        {
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            if (longservername == "")
                return;
            CodeExport ce = new CodeExport(longservername);
            ce.ShowDialog(this);
        }
        private void 模板代码批量生成Item_Click(object sender, EventArgs e)
        {
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            if (longservername == "")
                return;
            TemplateBatch tb = new TemplateBatch(longservername,false);
            tb.ShowDialog(this);
        }


        private void 模版代码生成Item_Click(object sender, EventArgs e)
        {
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            if (longservername == "")
                return;
            mainfrm.AddSinglePage(new CodeTemplate(mainfrm), "模版代码生成器");
        }

        private void 生成ModelItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode == null)
            {
                return;
            }
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            string dbname = TreeClickNode.Parent.Parent.Text;
            string tabname = TreeClickNode.Text;
            Maticsoft.CodeBuild.CodeBuilders cb = ObjHelper.CreatCB(longservername);
            cb.DbName = dbname;
            cb.TableName = tabname;
            string strSQL = cb.GetCodeFrameS3Model();
            string title = tabname;
            AddTabPage(title, new DbQuery(mainfrm, strSQL));
        }

        private void 生成单类结构Items_Click(object sender, EventArgs e)
        {
        }
        private void 生成DALS3Item_Click(object sender, EventArgs e)
        {
        }
        private void 生成BLLS3Item_Click(object sender, EventArgs e)
        {
        }

        private void 生成全部S3Item_Click(object sender, EventArgs e)
        {
        }
        private void 生成DALF3Item_Click(object sender, EventArgs e)
        {
        }
        private void 生成IDALItem_Click(object sender, EventArgs e)
        {
        }
        private void 生成DALFactoryItem_Click(object sender, EventArgs e)
        {
        }

        private void 生成BLLF3Item_Click(object sender, EventArgs e)
        {
        }
        private void 生成全部F3Item_Click(object sender, EventArgs e)
        {
        }
        private void 生成页面Item_Click(object sender, EventArgs e)
        {
        }

        
        private void 重命名tabItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode == null)
            {
                return;
            }
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            string dbname = TreeClickNode.Parent.Parent.Text;
            string tabname = TreeClickNode.Text;
            Maticsoft.IDBO.IDbObject dbobj = ObjHelper.CreatDbObj(longservername);

            RenameFrm rnfrm = new RenameFrm();
            rnfrm.txtName.Text = tabname;
            DialogResult result = rnfrm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                string newName = rnfrm.txtName.Text.Trim();
                bool succ = dbobj.RenameTable(dbname, tabname, newName);
                if (succ)
                {
                    TreeClickNode.Text = newName;
                    MessageBox.Show(this, "表名修改成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "表名修改失败，请稍候重试！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void 删除tabItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode == null)
            {
                return;
            }
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            string dbname = TreeClickNode.Parent.Parent.Text;
            string tabname = TreeClickNode.Text;
            Maticsoft.IDBO.IDbObject dbobj = ObjHelper.CreatDbObj(longservername);

            DialogResult result = MessageBox.Show(this, "你确认要删除改表吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                bool succ = dbobj.DeleteTable(dbname, tabname);
                if (succ)
                {
                    TreeClickNode.Remove();
                    MessageBox.Show(this, "表" + tabname + "删除成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "表" + tabname + "删除失败，请稍候重试！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        #endregion

        #region view_click

        #endregion

        #region proc_click

        private void 对象定义Item_Click(object sender, EventArgs e)
        {
            if (TreeClickNode == null)
            {
                return;
            }
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            string dbname = TreeClickNode.Parent.Parent.Text;
            string name = TreeClickNode.Text;
            Maticsoft.IDBO.IDbObject dbobj = ObjHelper.CreatDbObj(longservername);
            string str = dbobj.GetObjectInfo(dbname, name);
            string title = name + "定义.sql";
            AddTabPage(title, new DbQuery(mainfrm, str));
        }
        private void AlterItem_Click(object sender, EventArgs e)
        {
            if (TreeClickNode == null)
            {
                return;
            }
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            string dbname = TreeClickNode.Parent.Parent.Text;
            string name = TreeClickNode.Text;
            Maticsoft.IDBO.IDbObject dbobj = ObjHelper.CreatDbObj(longservername);
            string str = dbobj.GetObjectInfo(dbname, name).Replace("CREATE PROCEDURE ", "ALTER PROCEDURE ");

            string title = name + "编辑.sql";
            AddTabPage(title, new DbQuery(mainfrm, str));
        }

        private void 模板代码批量生成ProcItem_Click(object sender, EventArgs e)
        {
            string longservername = TreeClickNode.Parent.Parent.Parent.Text;
            if (longservername == "")
                return;
            TemplateBatch tb = new TemplateBatch(longservername, true);
            tb.ShowDialog(this);
        }

        #endregion

        #endregion


        #region 注册服务器RegServer
        
        public void RegServer(object sender, DoWorkEventArgs e)
        {
            try
            {
                Application.DoEvents();
                string dbtype = "SQL2005";                
                DialogResult dbselresult = dbsel.ShowDialog(this);
                if (dbselresult == DialogResult.OK)
                {
                    treeView1.Enabled = false;
                    dbtype = dbsel.dbtype;
                    switch (dbtype)
                    {
                        case "SQL2000":
                        case "SQL2005":
                        case "SQL2008":
                        case "SQL2012":
                            LoginServer(e);
                            break;
                        case "Oracle":
                            LoginServerOra(e);
                            break;
                        case "OleDb":
                            LoginServerOledb(e);
                            break;
                        case "MySQL":
                            LoginServerMySQL(e);
                            break;
                        case "SQLite":
                            LoginServerSQLite(e);
                            break;
                        default:
                            LoginServer(e);
                            break;
                    }
                    if (serverlistNode != null)
                    {
                        serverlistNode.Expand();
                    }
                    treeView1.Enabled = true;

                }
                                
            }
            catch(System.Exception ex)
            {
                MessageBox.Show("连接服务器失败，请关闭后重新打开再试。\r\n"+ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            e.Result = -1;

        }
        #endregion

        #region 登录服务器LoginServer

        #region SQL
        private void LoginServer(DoWorkEventArgs e)
        {
            mainfrm = (MainForm)Application.OpenForms["MainForm"];
            DialogResult result = logo.ShowDialog(this);
            if (result == DialogResult.OK)
            {                
                Application.DoEvents();
                                
                string ServerIp = logo.comboBoxServer.Text;
                string dbname = logo.dbname;
                string constr = logo.constr;
                string dbtype = logo.GetSelVer();

                try
                {                    
                    mainfrm.StatusLabel1.Text = "正在验证和连接服务器.....";
                    CreatTree(dbtype, ServerIp, constr, dbname,e);                 

                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //Maticsoft.SplashScrForm.SplashScreen.SetStatus("引导系统模块...");

            }

        }
        #endregion

        #region Oracle
        private void LoginServerOra(DoWorkEventArgs e)
        {
            mainfrm = (MainForm)Application.OpenForms["MainForm"];
            DialogResult result = logoOra.ShowDialog(this);
            if (result == DialogResult.OK)
            {                
                Application.DoEvents();             
                string ServerIp = logoOra.txtServer.Text;
                string constr = logoOra.constr;
                string dbname = logoOra.dbname;
                try
                {                   
                    mainfrm.StatusLabel1.Text = "正在验证和连接服务器.....";
                    CreatTree("Oracle", ServerIp, constr, dbname,e);
                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //Maticsoft.SplashScrForm.SplashScreen.SetStatus("引导系统模块...");
            }
        }

        #endregion

        #region Oledb

        private void LoginServerOledb(DoWorkEventArgs e)
        {
            DialogResult result = logoOledb.ShowDialog(this);

            switch (result)
            {
                case DialogResult.OK:
                    {                        
                        Application.DoEvents();
                                             
                        string ServerIp = logoOledb.txtServer.Text;
                        string user = logoOledb.txtUser.Text;
                        string pass = logoOledb.txtPass.Text;
                        string constr = logoOledb.txtConstr.Text;

                        //string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ServerIp + ";Persist Security Info=False";

                        GetConstr(ServerIp, constr);
                        try
                        {
                            try
                            {
                                mainfrm.StatusLabel1.Text = "正在验证和连接服务器.....";
                            }
                            catch
                            { 
                            }
                            CreatTree("OleDb", ServerIp, constr, "",e);                         
                        }
                        catch (System.Exception ex)
                        {
                            LogInfo.WriteLog(ex);
                            MessageBox.Show(this, ex.Message+",请关闭重新打开再试。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //isSuccess = false;
                            break;
                        }
                    }
                    //Maticsoft.SplashScrForm.SplashScreen.SetStatus("引导系统模块...");

                    break;
                case DialogResult.Cancel:
                    break;
            }

        }

        private string GetConstr(string ServerIp, string txtConstr)
        {
            string constr = "";
            if (ServerIp != "")
            {
                constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ServerIp + ";Persist Security Info=False";
                if (constr.ToLower().IndexOf(".mdb") > 0) 
                {
                    isMdb = true;
                }
                else
                    if(constr.ToLower().IndexOf(".accdb") > 0)
                    {
                        isMdb = true;
                        constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ServerIp + ";Persist Security Info=False";
                    }                   
                else
                {
                    isMdb = false;
                }
            }
            else
            {
                constr = txtConstr;
                if ((constr.ToLower().IndexOf(".mdb") > 0) || (constr.ToLower().IndexOf(".accdb") > 0))
                {
                    isMdb = true;
                }
                else
                {
                    isMdb = false;
                }

            }
            return constr;
        }

        #endregion

        #region MySQL
        private void LoginServerMySQL(DoWorkEventArgs e)
        {
            mainfrm = (MainForm)Application.OpenForms["MainForm"];
            DialogResult result = loginMysql.ShowDialog(this);
            if (result == DialogResult.OK)
            {                
                Application.DoEvents();
             
                string ServerIp = loginMysql.comboBoxServer.Text;
                string constr = loginMysql.constr;
                string dbname = loginMysql.dbname;
                try
                {                    
                    mainfrm.StatusLabel1.Text = "正在验证和连接服务器.....";
                    CreatTree("MySQL", ServerIp, constr, dbname,e);
                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //Maticsoft.SplashScrForm.SplashScreen.SetStatus("引导系统模块...");
            }

        }
        #endregion

        #region SQLite
        private void LoginServerSQLite(DoWorkEventArgs e)
        {
            mainfrm = (MainForm)Application.OpenForms["MainForm"];
            DialogResult result = loginSQLite.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                Application.DoEvents();

                string ServerIp = loginSQLite.txtServer.Text;
                string pass = loginSQLite.txtPass.Text;
                string constr = loginSQLite.txtConstr.Text;
                constr = "Data Source=" + ServerIp;
                if (pass != "")
                {
                    constr += ";Password=" + pass;
                }
                try
                {
                    mainfrm.StatusLabel1.Text = "正在验证和连接服务器.....";
                    CreatTree("SQLite", ServerIp, constr, "", e);
                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //Maticsoft.SplashScrForm.SplashScreen.SetStatus("引导系统模块...");
            }

        }
        #endregion

        #endregion

        #region 创建库服务器树节点 CreatTree

        private void CreatTree(string dbtype, string ServerIp, string constr, string Dbname, DoWorkEventArgs e)
        {
            dbobj = Maticsoft.DBFactory.DBOMaker.CreateDbObj(dbtype);

            string nodetext = GetserverNodeText(ServerIp, dbtype, Dbname);
            TreeNode serverNode = new TreeNode(nodetext);
            serverNode.Tag = "server";
            bool isHasNode = false;
            if (serverlistNode == null)
            {
                MessageBox.Show(this, "请关闭软件重新打开。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (TreeNode node in serverlistNode.Nodes)
            {
                if (node.Text.Trim() == nodetext)
                {
                    isHasNode = true;
                    serverNode = node;
                }
            }
            if (!isHasNode)
            {
                AddTreeNode(serverlistNode, serverNode);
            }
            serverNode.ImageIndex = 1;
            serverNode.SelectedImageIndex = 1;

            //0 serverlist
            //1 server
            //2 db
            //3 folderclose
            //4 folderopen
            //5 table
            //6 view
            //7 fild
            this.treeView1.SelectedNode = serverNode;
            mainfrm.StatusLabel1.Text = "加载数据库树...";
            Application.DoEvents();
            
            dbobj.DbConnectStr = constr;

            #region SQLSERVER 数据库信息
            if ((dbtype == "SQL2000") || (dbtype == "SQL2005") || (dbtype == "SQL2008") || (dbtype == "SQL2012"))
            {
                try
                {
                    if ((logo.dbname == "master") || (logo.dbname == ""))
                    {
                        List<string> dblist = dbobj.GetDBList();
                        if (dblist != null)
                        {
                            if (dblist.Count > 0)
                            {
                                mainfrm.toolComboBox_DB.Items.Clear();
                                foreach (string dbname in dblist)
                                {
                                    TreeNode dbNode = new TreeNode(dbname);
                                    dbNode.ImageIndex = 2;
                                    dbNode.SelectedImageIndex = 2;
                                    dbNode.Tag = "db";                                    
                                    AddTreeNode(serverNode, dbNode);
                                    mainfrm.toolComboBox_DB.Items.Add(dbname);
                                }
                                if (mainfrm.toolComboBox_DB.Items.Count > 0)
                                {
                                    mainfrm.toolComboBox_DB.SelectedIndex = 0;
                                }
                            }
                        }

                    }
                    else
                    {
                        string dbname = logo.dbname;
                        TreeNode dbNode = new TreeNode(dbname);
                        dbNode.ImageIndex = 2;
                        dbNode.SelectedImageIndex = 2;
                        dbNode.Tag = "db";                        
                        AddTreeNode(serverNode, dbNode);
                        mainfrm.toolComboBox_DB.Items.Clear();
                        mainfrm.toolComboBox_DB.Items.Add(dbname);

                        //下拉框2
                        DataTable dto = dbobj.GetTabViews(dbname);
                        if (dto != null)
                        {
                            mainfrm.toolComboBox_Table.Items.Clear();
                            foreach (DataRow row in dto.Rows)//循环每个表
                            {
                                string tabname = row["name"].ToString();
                                mainfrm.toolComboBox_Table.Items.Add(dbname);
                            }
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    throw new Exception("获取数据库失败：" + ex.Message);
                }
            }

            #endregion

            #region Oracle 数据库单独处理
            if (dbtype == "Oracle")
            {
                string dbname = ServerIp;
                TreeNode dbNode = new TreeNode(dbname);
                dbNode.ImageIndex = 2;
                dbNode.SelectedImageIndex = 2;
                dbNode.Tag = "db";                
                AddTreeNode(serverNode, dbNode);
                mainfrm.toolComboBox_DB.Items.Add(dbname);

                //下拉框2
                DataTable dto = dbobj.GetTabViews(dbname);
                if (dto != null)
                {
                    mainfrm.toolComboBox_Table.Items.Clear();
                    foreach (DataRow row in dto.Rows)//循环每个表
                    {
                        string tabname = row["name"].ToString();
                        mainfrm.toolComboBox_Table.Items.Add(dbname);
                    }
                    if (mainfrm.toolComboBox_Table.Items.Count > 0)
                    {
                        mainfrm.toolComboBox_Table.SelectedIndex = 0;
                    }
                }

            }
            #endregion

            #region MySQL 数据库单独处理
            if (dbtype == "MySQL")
            {
                try
                {
                    if ((this.loginMysql.dbname == "mysql") || (loginMysql.dbname == ""))
                    {
                        List<string> dblist = dbobj.GetDBList();
                        if (dblist != null)
                        {
                            if (dblist.Count > 0)
                            {
                                mainfrm.toolComboBox_DB.Items.Clear();
                                foreach (string dbname in dblist)
                                {
                                    TreeNode dbNode = new TreeNode(dbname);
                                    dbNode.ImageIndex = 2;
                                    dbNode.SelectedImageIndex = 2;
                                    dbNode.Tag = "db";                                    
                                    AddTreeNode(serverNode, dbNode);
                                    mainfrm.toolComboBox_DB.Items.Add(dbname);
                                }
                                if (mainfrm.toolComboBox_DB.Items.Count > 0)
                                {
                                    mainfrm.toolComboBox_DB.SelectedIndex = 0;
                                }
                            }
                        }

                    }
                    else
                    {
                        string dbname = loginMysql.dbname;
                        TreeNode dbNode = new TreeNode(dbname);
                        dbNode.ImageIndex = 2;
                        dbNode.SelectedImageIndex = 2;
                        dbNode.Tag = "db";                        
                        AddTreeNode(serverNode, dbNode);
                        mainfrm.toolComboBox_DB.Items.Clear();
                        mainfrm.toolComboBox_DB.Items.Add(dbname);

                        //下拉框2
                        DataTable dto = dbobj.GetTabViews(dbname);
                        if (dto != null)
                        {
                            mainfrm.toolComboBox_Table.Items.Clear();
                            foreach (DataRow row in dto.Rows)//循环每个表
                            {
                                string tabname = row["name"].ToString();
                                mainfrm.toolComboBox_Table.Items.Add(dbname);
                            }
                            if (mainfrm.toolComboBox_Table.Items.Count > 0)
                            {
                                mainfrm.toolComboBox_Table.SelectedIndex = 0;
                            }
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    throw new Exception("获取数据库失败：" + ex.Message);
                }

            }
            #endregion

            #region OleDb 数据库单独处理
            if (dbtype == "OleDb")
            {
                string dbname = ServerIp.Substring(ServerIp.LastIndexOf("\\") + 1);
                TreeNode dbNode = new TreeNode(dbname);
                dbNode.ImageIndex = 2;
                dbNode.SelectedImageIndex = 2;
                dbNode.Tag = "db";                
                AddTreeNode(serverNode, dbNode);
                mainfrm.toolComboBox_DB.Items.Add(dbname);

                //下拉框2
                DataTable dto = dbobj.GetTabViews(dbname);
                if (dto != null)
                {
                    mainfrm.toolComboBox_Table.Items.Clear();
                    foreach (DataRow row in dto.Rows)//循环每个表
                    {
                        string tabname = row["name"].ToString();
                        mainfrm.toolComboBox_Table.Items.Add(dbname);
                    }
                    if (mainfrm.toolComboBox_Table.Items.Count > 0)
                    {
                        mainfrm.toolComboBox_Table.SelectedIndex = 0;
                    }
                }

            }
            #endregion

            #region SQLite 数据库单独处理
            if (dbtype == "SQLite")
            {
                string dbname = ServerIp.Substring(ServerIp.LastIndexOf("\\") + 1);
                TreeNode dbNode = new TreeNode(dbname);
                dbNode.ImageIndex = 2;
                dbNode.SelectedImageIndex = 2;
                dbNode.Tag = "db";
                AddTreeNode(serverNode, dbNode);
                mainfrm.toolComboBox_DB.Items.Add(dbname);

                //下拉框2
                DataTable dto = dbobj.GetTabViews(dbname);
                if (dto != null)
                {
                    mainfrm.toolComboBox_Table.Items.Clear();
                    foreach (DataRow row in dto.Rows)//循环每个表
                    {
                        string tabname = row["name"].ToString();
                        mainfrm.toolComboBox_Table.Items.Add(dbname);
                    }
                    if (mainfrm.toolComboBox_Table.Items.Count > 0)
                    {
                        mainfrm.toolComboBox_Table.SelectedIndex = 0;
                    }
                }

            }
            #endregion

            serverNode.ExpandAll();

            #region  循环数据库，建立表信息
            
            foreach (TreeNode tn in serverNode.Nodes)
            {
                string dbname = tn.Text;

                mainfrm.StatusLabel1.Text = "加载数据库 " + dbname + "...";
                SetTreeNodeFont(tn, new Font("宋体", 9, FontStyle.Bold));
                TreeNode tabNode = new TreeNode("表");
                tabNode.ImageIndex = 3;
                tabNode.SelectedImageIndex = 4;
                tabNode.Tag = "tableroot";                
                AddTreeNode(tn, tabNode);

                TreeNode viewNode = new TreeNode("视图");
                viewNode.ImageIndex = 3;
                viewNode.SelectedImageIndex = 4;
                viewNode.Tag = "viewroot";                
                AddTreeNode(tn, viewNode);

                TreeNode procNode = new TreeNode("存储过程");
                procNode.ImageIndex = 3;
                procNode.SelectedImageIndex = 4;
                procNode.Tag = "procroot";                
                //AddTreeNode(tn, procNode);
                if ((dbtype == "SQL2000") || (dbtype == "SQL2005")
                || (dbtype == "SQL2008") || (dbtype == "SQL2012") || (dbtype == "Oracle")
                || (dbtype == "MySQL"))
                {
                    AddTreeNode(tn, procNode);
                }

                #region 表

                try
                {
                    List<string> tabNames = dbobj.GetTables(dbname);
                    if (tabNames.Count > 0)
                    {
                        int pi = 1;
                        foreach (string tabname in tabNames)//循环每个表
                        {
                            if (backgroundWorkerReg.CancellationPending)
                            {
                                e.Cancel = true;
                            }
                            else
                            {
                                backgroundWorkerReg.ReportProgress(pi);
                            }
                            pi++;

                            if (logo.cmboxTabLoadtype.SelectedIndex == 1 && tabname.IndexOf(logo.txtTabLoadKeyword.Text.Trim()) == -1)
                            {
                                continue;
                            }
                            if (logo.cmboxTabLoadtype.SelectedIndex == 2 && tabname.IndexOf(logo.txtTabLoadKeyword.Text.Trim()) > -1)
                            {
                                continue;
                            }

                            mainfrm.StatusLabel1.Text = "加载数据库 " + dbname + "的表 " + tabname;
                            TreeNode tbNode = new TreeNode(tabname);
                            tbNode.ImageIndex = 5;
                            tbNode.SelectedImageIndex = 5;
                            tbNode.Tag = "table";                            
                            AddTreeNode(tabNode, tbNode);
                            
                            #region  加字段信息
                            if (!logo.chk_Simple.Checked)//if (!logo.chk_Simple.Checked)
                            {
                                List<ColumnInfo> collist = dbobj.GetColumnList(dbname, tabname);
                                if ((collist != null) && (collist.Count > 0))
                                {
                                    foreach (ColumnInfo col in collist)
                                    {
                                        string columnName = col.ColumnName;
                                        string columnType = col.TypeName;
                                        TreeNode colNode = new TreeNode(columnName + "[" + columnType + "]");
                                        colNode.ImageIndex = 7;
                                        colNode.SelectedImageIndex = 7;
                                        colNode.Tag = "column";                                        
                                        AddTreeNode(tbNode, colNode);
                                    }
                                }
                            }
                            #endregion

                        }
                    }

                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, "获取数据库" + dbname + "的表信息失败：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                #endregion

                #region	视图

                try
                {
                    DataTable dtv = dbobj.GetVIEWs(dbname);
                    if (dtv != null)
                    {
                        DataRow[] dRows = dtv.Select("", "name ASC");
                        foreach (DataRow row in dRows)//循环每个表
                        {
                            string tabname = row["name"].ToString();

                            if (logo.cmboxTabLoadtype.SelectedIndex == 1 && tabname.IndexOf(logo.txtTabLoadKeyword.Text.Trim()) == -1)
                            {
                                continue;
                            }
                            if (logo.cmboxTabLoadtype.SelectedIndex == 2 && tabname.IndexOf(logo.txtTabLoadKeyword.Text.Trim()) > -1)
                            {
                                continue;
                            }


                            mainfrm.StatusLabel1.Text = "加载数据库 " + dbname + "的视图 " + tabname;
                            TreeNode tbNode = new TreeNode(tabname);
                            tbNode.ImageIndex = 6;
                            tbNode.SelectedImageIndex = 6;
                            tbNode.Tag = "view";                            
                            AddTreeNode(viewNode, tbNode);

                            #region  加字段信息
                            if (!logo.chk_Simple.Checked)
                            {
                                List<ColumnInfo> collist = dbobj.GetColumnList(dbname, tabname);
                                if ((collist != null) && (collist.Count > 0))
                                {
                                    foreach (ColumnInfo col in collist)
                                    {
                                        string columnName = col.ColumnName;
                                        string columnType = col.TypeName;
                                        TreeNode colNode = new TreeNode(columnName + "[" + columnType + "]");
                                        colNode.ImageIndex = 7;
                                        colNode.SelectedImageIndex = 7;
                                        colNode.Tag = "column";
                                        //tbNode.Nodes.Add(colNode);
                                        AddTreeNode(tbNode, colNode);
                                    }
                                }
                            }
                            #endregion

                        }
                    }
                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, "获取数据库" + dbname + "的视图信息失败：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

                #region 存储过程
                try
                {
                    List<string> namelist = dbobj.GetProcs(dbname);
                    foreach (string spname in namelist)
                    {
                        mainfrm.StatusLabel1.Text = "加载数据库 " + dbname + "的存储过程 " + spname;
                        TreeNode tbNode = new TreeNode(spname);
                        tbNode.ImageIndex = 8;
                        tbNode.SelectedImageIndex = 8;
                        tbNode.Tag = "proc";
                        AddTreeNode(procNode, tbNode);

                        #region  加字段信息
                        if (!logo.chk_Simple.Checked)
                        {
                            List<ColumnInfo> collist = dbobj.GetColumnList(dbname, spname);
                            if ((collist != null) && (collist.Count > 0))
                            {
                                foreach (ColumnInfo col in collist)
                                {
                                    string columnName = col.ColumnName;
                                    string columnType = col.TypeName;

                                    TreeNode colNode = new TreeNode(columnName + "[" + columnType + "]");
                                    colNode.ImageIndex = 9;
                                    colNode.SelectedImageIndex = 9;
                                    colNode.Tag = "column";
                                    AddTreeNode(tbNode, colNode);
                                }
                            }
                        }
                        #endregion
                    }
                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, "获取数据库" + dbname + "的视图信息失败：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

                SetTreeNodeFont(tn, new Font("宋体", 9, FontStyle.Regular));

            }
            #endregion

            #region 选中根节点
            foreach (TreeNode node in this.treeView1.Nodes)
            {
                if (node.Text == ServerIp)
                {
                    this.treeView1.SelectedNode = node;
                    //node.BackColor=Color.FromArgb(10,36,106);
                    //node.ForeColor=Color.White;
                }
            }
            #endregion

        }

        #endregion

        #region 连接服务器ConnectServer

        public void DoConnect(object sender, DoWorkEventArgs e)
        {
            if (TreeClickNode != null)
            {
                string nodetext = TreeClickNode.Text;
                Maticsoft.CmConfig.DbSettings dbset = Maticsoft.CmConfig.DbConfig.GetSetting(nodetext);
                if (dbset == null)
                {
                    MessageBox.Show("该服务器信息已经不存在，请关闭软件然后重试。" , "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //string server = dbset.Server;
                //string dbtype = dbset.DbType;
                //string dbname = dbset.DbName;                
                //bool connectSimple = dbset.ConnectSimple;

                try
                {
                    treeView1.Enabled = false;
                    ConnectServer(TreeClickNode, dbset, e);
                    treeView1.Enabled = true;
                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show("加载数据库失败，请关闭后重新打开再试。\r\n"+ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            e.Result = -1;
        }

        public void ProgessChangedCon(object sender, ProgressChangedEventArgs e)
        {
            //this.progressBar1.Maximum = 1000;
            //this.progressBar1.Value = e.ProgressPercentage;
        }
        public void CompleteWorkCon(object sender, RunWorkerCompletedEventArgs e)
        {
            if (mainfrm != null)
            {
                mainfrm.StatusLabel1.Text = "完成";
            }
        }

        public void ProgessChangedReg(object sender, ProgressChangedEventArgs e)
        {
            //this.progressBar1.Maximum = 1000;
            //this.progressBar1.Value = e.ProgressPercentage;
        }
        public void CompleteWorkReg(object sender, RunWorkerCompletedEventArgs e)
        {
            if (mainfrm != null)
            {
                mainfrm.StatusLabel1.Text = "完成";
            }
        }
        
        //根据服务器节点创建下面库和表节点
        private void ConnectServer(TreeNode serverNode, Maticsoft.CmConfig.DbSettings dbset, DoWorkEventArgs e)
        {
            Maticsoft.IDBO.IDbObject dbobj2 = Maticsoft.DBFactory.DBOMaker.CreateDbObj(dbset.DbType);

            mainfrm.StatusLabel1.Text = "加载数据库树...";            
            Application.DoEvents();
                        
            dbobj2.DbConnectStr = dbset.ConnectStr;

            serverNode.Nodes.Clear();

            #region SQLSERVER 数据库信息
            if ((dbset.DbType == "SQL2000") || (dbset.DbType == "SQL2005")
                || (dbset.DbType == "SQL2008") || (dbset.DbType == "SQL2012"))
            {
                try
                {
                    if ((dbset.DbName == "master") || (dbset.DbName == ""))
                    {
                        List<string> dblist = dbobj2.GetDBList();
                        if (dblist.Count > 0)
                        {
                            mainfrm.toolComboBox_DB.Items.Clear();
                            foreach (string dbname in dblist)
                            {
                                TreeNode dbNode = new TreeNode(dbname);
                                dbNode.ImageIndex = 2;
                                dbNode.SelectedImageIndex = 2;
                                dbNode.Tag = "db";                                
                                AddTreeNode(serverNode, dbNode);
                                mainfrm.toolComboBox_DB.Items.Add(dbname);
                            }
                            if (mainfrm.toolComboBox_DB.Items.Count > 0)
                            {
                                mainfrm.toolComboBox_DB.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        TreeNode dbNode = new TreeNode(dbset.DbName);
                        dbNode.ImageIndex = 2;
                        dbNode.SelectedImageIndex = 2;
                        dbNode.Tag = "db";                        
                        AddTreeNode(serverNode, dbNode);
                        mainfrm.toolComboBox_DB.Items.Clear();
                        mainfrm.toolComboBox_DB.Items.Add(dbset.DbName);

                        //下拉框2
                        DataTable dto = dbobj2.GetTabViews(dbset.DbName);
                        if (dto != null)
                        {
                            mainfrm.toolComboBox_Table.Items.Clear();
                            foreach (DataRow row in dto.Rows)//循环每个表
                            {
                                string tabname = row["name"].ToString();
                                mainfrm.toolComboBox_Table.Items.Add(tabname);
                            }
                            if (mainfrm.toolComboBox_Table.Items.Count > 0)
                            {
                                mainfrm.toolComboBox_Table.SelectedIndex = 0;
                            }
                        }
                    }

                }
                catch(System.Exception ex)
                {
                    // throw new Exception("获取数据库失败：" + ex.Message);
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, "连接服务器失败！请检查服务器是否已经启动或工作正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            #endregion

            #region Oracle 数据库单独处理
            if (dbset.DbType == "Oracle")
            {
                TreeNode dbNode = new TreeNode(dbset.Server);
                dbNode.ImageIndex = 2;
                dbNode.SelectedImageIndex = 2;
                dbNode.Tag = "db";                
                AddTreeNode(serverNode, dbNode);
                mainfrm.toolComboBox_DB.Items.Add(dbset.Server);
                mainfrm.toolComboBox_DB.SelectedIndex = 0;

                //下拉框2
                DataTable dto = dbobj2.GetTabViews(dbset.Server);
                if (dto != null)
                {
                    mainfrm.toolComboBox_Table.Items.Clear();
                    foreach (DataRow row in dto.Rows)//循环每个表
                    {
                        string tabname = row["name"].ToString();
                        mainfrm.toolComboBox_Table.Items.Add(tabname);
                    }
                    if (mainfrm.toolComboBox_Table.Items.Count > 0)
                    {
                        mainfrm.toolComboBox_Table.SelectedIndex = 0;
                    }
                }

            }
            #endregion

            #region MySQL 数据库单独处理
            if (dbset.DbType == "MySQL")
            {
                try
                {
                    string dbname = dbset.DbName;

                    if ((dbname == "mysql") || (dbname == ""))
                    {
                        List<string> dblist = dbobj2.GetDBList();
                        if (dblist.Count > 0)
                        {
                            mainfrm.toolComboBox_DB.Items.Clear();
                            foreach (string db in dblist)
                            {
                                TreeNode dbNode = new TreeNode(db);
                                dbNode.ImageIndex = 2;
                                dbNode.SelectedImageIndex = 2;
                                dbNode.Tag = "db";                                
                                AddTreeNode(serverNode, dbNode);
                                mainfrm.toolComboBox_DB.Items.Add(db);
                            }
                            if (mainfrm.toolComboBox_DB.Items.Count > 0)
                            {
                                mainfrm.toolComboBox_DB.SelectedIndex = 0;
                            }
                        }

                    }
                    else
                    {                        
                        TreeNode dbNode = new TreeNode(dbname);
                        dbNode.ImageIndex = 2;
                        dbNode.SelectedImageIndex = 2;
                        dbNode.Tag = "db";                        
                        AddTreeNode(serverNode, dbNode);
                        mainfrm.toolComboBox_DB.Items.Clear();
                        mainfrm.toolComboBox_DB.Items.Add(dbname);

                        //下拉框2
                        DataTable dto = dbobj2.GetTabViews(dbname);
                        if (dto != null)
                        {
                            mainfrm.toolComboBox_Table.Items.Clear();
                            foreach (DataRow row in dto.Rows)//循环每个表
                            {
                                string tabname = row["name"].ToString();
                                mainfrm.toolComboBox_Table.Items.Add(dbname);
                            }
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, "连接服务器失败！请检查服务器是否已经启动或工作正常！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
            #endregion

            #region OleDb 数据库单独处理
            if (dbset.DbType == "OleDb")
            {
                string dbname = dbset.Server.Substring(dbset.Server.LastIndexOf("\\") + 1);
                TreeNode dbNode = new TreeNode(dbname);
                dbNode.ImageIndex = 2;
                dbNode.SelectedImageIndex = 2;
                dbNode.Tag = "db";                
                AddTreeNode(serverNode, dbNode);
                mainfrm.toolComboBox_DB.Items.Add(dbname);
                mainfrm.toolComboBox_DB.SelectedIndex = 0;

                //下拉框2
                DataTable dto = dbobj2.GetTabViews(dbname);
                if (dto != null)
                {
                    mainfrm.toolComboBox_Table.Items.Clear();
                    foreach (DataRow row in dto.Rows)//循环每个表
                    {
                        string tabname = row["name"].ToString();
                        mainfrm.toolComboBox_Table.Items.Add(tabname);
                    }
                    if (mainfrm.toolComboBox_Table.Items.Count > 0)
                    {
                        mainfrm.toolComboBox_Table.SelectedIndex = 0;
                    }
                }

            }
            #endregion
            
            #region SQLite 数据库单独处理
            if (dbset.DbType == "SQLite")
            {
                string dbname = dbset.Server.Substring(dbset.Server.LastIndexOf("\\") + 1);
                TreeNode dbNode = new TreeNode(dbname);
                dbNode.ImageIndex = 2;
                dbNode.SelectedImageIndex = 2;
                dbNode.Tag = "db";
                AddTreeNode(serverNode, dbNode);
                mainfrm.toolComboBox_DB.Items.Add(dbname);
                mainfrm.toolComboBox_DB.SelectedIndex = 0;

                //下拉框2
                DataTable dto = dbobj2.GetTabViews(dbname);
                if (dto != null)
                {
                    mainfrm.toolComboBox_Table.Items.Clear();
                    foreach (DataRow row in dto.Rows)//循环每个表
                    {
                        string tabname = row["name"].ToString();
                        mainfrm.toolComboBox_Table.Items.Add(tabname);
                    }
                    if (mainfrm.toolComboBox_Table.Items.Count > 0)
                    {
                        mainfrm.toolComboBox_Table.SelectedIndex = 0;
                    }
                }

            }
            #endregion


            serverNode.ExpandAll();

            #region 循环数据库，建立表信息
            foreach (TreeNode tn in serverNode.Nodes)
            {
                string dbname = tn.Text;
                mainfrm.StatusLabel1.Text = "加载数据库 " + dbname + "...";
                SetTreeNodeFont(tn, new Font("宋体", 9, FontStyle.Bold));
                TreeNode tabNode = new TreeNode("表");
                tabNode.ImageIndex = 3;
                tabNode.SelectedImageIndex = 4;
                tabNode.Tag = "tableroot";                
                AddTreeNode(tn, tabNode);

                TreeNode viewNode = new TreeNode("视图");
                viewNode.ImageIndex = 3;
                viewNode.SelectedImageIndex = 4;
                viewNode.Tag = "viewroot";                
                AddTreeNode(tn, viewNode);

                TreeNode procNode = new TreeNode("存储过程");
                procNode.ImageIndex = 3;
                procNode.SelectedImageIndex = 4;
                procNode.Tag = "procroot";
                if ((dbset.DbType == "SQL2000") || (dbset.DbType == "SQL2005")
                || (dbset.DbType == "SQL2008") || (dbset.DbType == "SQL2012") || (dbset.DbType == "Oracle")
                || (dbset.DbType == "MySQL"))
                {                    
                    AddTreeNode(tn, procNode);
                }

                #region 表

                try
                {
                    List<string> tabNames = dbobj2.GetTables(dbname);
                    if (tabNames.Count > 0)
                    {
                        int pi = 1;
                        foreach (string tabname in tabNames)//循环每个表
                        {
                            if (backgroundWorkerCon.CancellationPending)
                            {
                                e.Cancel = true;
                            }
                            else
                            {
                                backgroundWorkerCon.ReportProgress(pi);
                            } 
                            pi++;

                            if (dbset.TabLoadtype == 1 && tabname.IndexOf(dbset.TabLoadKeyword) == -1)
                            {
                                continue;
                            }
                            if (dbset.TabLoadtype == 2 && tabname.IndexOf(dbset.TabLoadKeyword) > -1)
                            {
                                continue;
                            }

                            mainfrm.StatusLabel1.Text = "加载数据库 " + dbname + "的表 " + tabname;
                            TreeNode tbNode = new TreeNode(tabname);
                            tbNode.ImageIndex = 5;
                            tbNode.SelectedImageIndex = 5;
                            tbNode.Tag = "table";                           
                            AddTreeNode(tabNode, tbNode);

                            #region  加字段信息
                            if (!dbset.ConnectSimple)
                            {
                                List<ColumnInfo> collist = dbobj2.GetColumnList(dbname, tabname);
                                if ((collist != null) && (collist.Count > 0))
                                {
                                    foreach (ColumnInfo col in collist)
                                    {
                                        string columnName = col.ColumnName;
                                        string columnType = col.TypeName;
                                        TreeNode colNode = new TreeNode(columnName + "[" + columnType + "]");
                                        colNode.ImageIndex = 7;
                                        colNode.SelectedImageIndex = 7;
                                        colNode.Tag = "column";                                        
                                        AddTreeNode(tbNode, colNode);
                                    }
                                }
                            }
                            #endregion

                        }
                    }
                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, "获取数据库" + dbname + "的表信息失败：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

                #region	视图

                try
                {
                    DataTable dtv = dbobj2.GetVIEWs(dbname);
                    if (dtv != null)
                    {
                        DataRow[] dRows = dtv.Select("", "name ASC");
                        foreach (DataRow row in dRows)//循环每个表
                        {
                            string tabname = row["name"].ToString();

                            if (dbset.TabLoadtype == 1 && tabname.IndexOf(dbset.TabLoadKeyword) == -1)
                            {
                                continue;
                            }
                            if (dbset.TabLoadtype == 2 && tabname.IndexOf(dbset.TabLoadKeyword) > -1)
                            {
                                continue;
                            }


                            mainfrm.StatusLabel1.Text = "加载数据库 " + dbname + "的视图 " + tabname;                          
                            TreeNode tbNode = new TreeNode(tabname);
                            tbNode.ImageIndex = 6;
                            tbNode.SelectedImageIndex = 6;
                            tbNode.Tag = "view";
                            //viewNode.Nodes.Add(tbNode);
                            AddTreeNode(viewNode, tbNode);

                            #region  加字段信息
                            if (!dbset.ConnectSimple)
                            {
                                List<ColumnInfo> collist = dbobj2.GetColumnList(dbname, tabname);
                                if ((collist != null) && (collist.Count > 0))
                                {
                                    foreach (ColumnInfo col in collist)//循环每个列
                                    {
                                        string columnName = col.ColumnName;
                                        string columnType = col.TypeName;
                                        TreeNode colNode = new TreeNode(columnName + "[" + columnType + "]");
                                        colNode.ImageIndex = 7;
                                        colNode.SelectedImageIndex = 7;
                                        colNode.Tag = "column";
                                        //tbNode.Nodes.Add(colNode);
                                        AddTreeNode(tbNode, colNode);
                                    }
                                }
                            }
                            #endregion

                        }
                    }
                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, "获取数据库" + dbname + "的视图信息失败：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

                #region 存储过程
                try
                {
                    List<string> namelist = dbobj2.GetProcs(dbname);
                    foreach (string spname in namelist)
                    {
                        mainfrm.StatusLabel1.Text = "加载数据库 " + dbname + "的存储过程 " + spname;
                        TreeNode tbNode = new TreeNode(spname);
                        tbNode.ImageIndex = 8;
                        tbNode.SelectedImageIndex = 8;
                        tbNode.Tag = "proc";
                        AddTreeNode(procNode, tbNode);

                        #region  加字段信息
                        if (!dbset.ConnectSimple)
                        {
                            List<ColumnInfo> collist = dbobj2.GetColumnList(dbname, spname);
                            if ((collist != null) && (collist.Count > 0))
                            {
                                foreach (ColumnInfo col in collist)//循环每个列
                                {
                                    string columnName = col.ColumnName;
                                    string columnType = col.TypeName;
                                    TreeNode colNode = new TreeNode(columnName + "[" + columnType + "]");
                                    colNode.ImageIndex = 9;
                                    colNode.SelectedImageIndex = 9;
                                    colNode.Tag = "column";
                                    //tbNode.Nodes.Add(colNode);
                                    AddTreeNode(tbNode, colNode);
                                }
                            }
                        }
                        #endregion
                    }
                    
                }
                catch (System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    MessageBox.Show(this, "获取数据库" + dbname + "的视图信息失败：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

                SetTreeNodeFont(tn, new Font("宋体", 9, FontStyle.Regular));
            }
            #endregion
                        
            #region 选中根节点
            foreach (TreeNode node in serverlistNode.Nodes)
            {
                if (node.Text == dbset.Server)
                {
                    this.treeView1.SelectedNode = node;
                    //node.BackColor=Color.FromArgb(10,36,106);
                    //node.ForeColor=Color.White;
                }
            }
            #endregion

            //mainfrm.StatusLabel1.Text = "就绪";
        }

        #endregion

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = (TreeNode)e.Item;          
            if ((node.Tag.ToString() == "table") || (node.Tag.ToString() == "view") || (node.Tag.ToString() == "column"))
            {
                DoDragDrop(node, System.Windows.Forms.DragDropEffects.Copy);
            }
        }

        private void DbView_Layout(object sender, LayoutEventArgs e)
        {
            if (m_bLayoutCalled == false)
            {
                m_bLayoutCalled = true;              
                //Maticsoft.SplashScrForm.SplashScreen.CloseForm();
                this.Activate();
            }
        }



    }
}