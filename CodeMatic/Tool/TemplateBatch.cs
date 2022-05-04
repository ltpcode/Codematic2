using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Maticsoft.CodeBuild;
using Maticsoft.Utility;
using System.Threading;
using Codematic.UserControls;
using Maticsoft.CodeHelper;
namespace Codematic
{
    /// <summary>
    /// 模板代码批量生成
    /// </summary>
    public partial class TemplateBatch : Form
    {
        Thread mythread;
        string cmcfgfile = Application.StartupPath + @"\cmcfg.ini";
        Maticsoft.Utility.INIFile cfgfile;
        Maticsoft.IDBO.IDbObject dbobj;//数据库对象        
        Maticsoft.CmConfig.DbSettings dbset;//服务器配置   
        Maticsoft.CmConfig.AppSettings settings;

        delegate void SetBtnEnableCallback();
        delegate void SetBtnDisableCallback();
        delegate void SetlblStatuCallback(string text);
        delegate void SetProBar1MaxCallback(int val);
        delegate void SetProBar1ValCallback(int val);
        string dbname = "";
        string TemplateFolder;//模板所在文件夹                
        VSProject vsp = new VSProject();
        bool isProc = false;

        public TemplateBatch(string longservername, bool isproc)
        {
            InitializeComponent();
            dbset = Maticsoft.CmConfig.DbConfig.GetSetting(longservername);
            dbobj = Maticsoft.DBFactory.DBOMaker.CreateDbObj(dbset.DbType);
            dbobj.DbConnectStr = dbset.ConnectStr;
            this.lblServer.Text = dbset.Server;
            txtFolder.Text = dbset.Folder;
            isProc = isproc;
            if (isProc)
            {
                groupBox2.Text = "选择存储过程";
            }
        }

        private void TemplateBatch_Load(object sender, EventArgs e)
        {
            #region  加载库和表
            string mastedb = "master";
            switch (dbobj.DbType)
            {
                case "SQL2000":
                case "SQL2005":
                case "SQL2008":
                case "SQL2012":
                    mastedb = "master";
                    break;
                case "Oracle":
                case "OleDb":
                    {
                        mastedb = "";
                        label3.Visible = false;
                        cmbDB.Visible = false;
                    }
                    break;
                case "MySQL":
                    mastedb = "mysql";
                    break;
                case "SQLite":
                    mastedb = "sqlite_master";
                    break;
            }
            if ((dbset.DbName == "") || (dbset.DbName == mastedb))
            {
                List<string> dblist = dbobj.GetDBList();
                if (dblist != null)
                {
                    if (dblist.Count > 0)
                    {
                        foreach (string dbname in dblist)
                        {
                            this.cmbDB.Items.Add(dbname);
                        }
                    }
                }
            }
            else
            {
                this.cmbDB.Items.Add(dbset.DbName);
            }

            if (this.cmbDB.Items.Count > 0)
            {
                this.cmbDB.SelectedIndex = 0;
            }
            else
            {
                this.listTable1.Items.Clear();
                this.listTable2.Items.Clear();

                List<string> tabNames;
                if (isProc)
                {
                    tabNames = dbobj.GetProcs("");
                }
                else
                {
                    tabNames = dbobj.GetTableViews("");
                }
                if (tabNames.Count > 0)
                {
                    foreach (string tabname in tabNames)
                    {
                        listTable1.Items.Add(tabname);
                    }
                }
            }
            #endregion

            #region 保存路径
            this.btn_Export.Enabled = false;
            if (File.Exists(cmcfgfile))
            {
                cfgfile = new Maticsoft.Utility.INIFile(cmcfgfile);
                string lastpath = cfgfile.IniReadValue("Project", "lastpath");
                if (lastpath.Trim() != "")
                {
                    txtTargetFolder.Text = lastpath;
                }
            }
            #endregion

            #region 加载模板
            settings = Maticsoft.CmConfig.AppConfig.GetSettings();
            if (settings.TemplateFolder == "Template" ||
                settings.TemplateFolder == "Template\\TemplateFile" ||
                settings.TemplateFolder.Length == 0)
            {
                TemplateFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template\\TemplateFile");
            }
            else
            {
                DirectoryInfo tempdir = new DirectoryInfo(settings.TemplateFolder);
                if (tempdir.Exists)
                {
                    TemplateFolder = settings.TemplateFolder;
                }
                else
                {
                    TemplateFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template\\TemplateFile");
                }
            }
            CreateFolderTree(TemplateFolder);
            CheckTemplate();
            #endregion

            IsHasItem();
        }

        private void cmbDB_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string dbname = cmbDB.Text;
            List<string> tabNames;
            if (isProc)
            {
                tabNames = dbobj.GetProcs(dbname);
            }
            else
            {
                tabNames = dbobj.GetTableViews(dbname);
            }
            this.listTable1.Items.Clear();
            this.listTable2.Items.Clear();
            if (tabNames.Count > 0)
            {
                foreach (string tabname in tabNames)
                {
                    listTable1.Items.Add(tabname);
                }
            }
            IsHasItem();
        }

        #region 加载目录结构

        private void CreateFolderTree(string templateFolder)
        {
            treeView1.Nodes.Clear();
            TempNode rootNode = new TempNode("代码模版");
            rootNode.NodeType = "root";
            rootNode.FilePath = templateFolder;
            rootNode.ImageIndex = 0;
            rootNode.SelectedImageIndex = 0;
            rootNode.Expand();
            treeView1.Nodes.Add(rootNode);

            LoadFolderTree(rootNode, templateFolder);
        }
        private void LoadFolderTree(TreeNode parentnode, string templateFolder)
        {
            DirectoryInfo source = new DirectoryInfo(templateFolder);
            if (!source.Exists)
                return;

            DirectoryInfo[] sourceDirectories = source.GetDirectories();
            for (int j = 0; j < sourceDirectories.Length; ++j)
            {
                TempNode node = new TempNode(sourceDirectories[j].Name);
                node.NodeType = "folder";
                node.FilePath = sourceDirectories[j].FullName;
                node.ImageIndex = 0;
                node.SelectedImageIndex = 1;
                parentnode.Nodes.Add(node);
                LoadFolderTree(node, sourceDirectories[j].FullName);
            }

            FileInfo[] sourceFiles = source.GetFiles();
            int filescount = sourceFiles.Length;
            for (int i = 0; i < filescount; ++i)
            {
                if ((sourceFiles[i].Extension == ".tt") ||
                    (sourceFiles[i].Extension == ".cmt") ||
                    (sourceFiles[i].Extension == ".aspx") ||
                    (sourceFiles[i].Extension == ".cs") ||
                    (sourceFiles[i].Extension == ".vb")
                    )
                {
                    if ("temp~.cmt" == sourceFiles[i].Name)
                        continue;
                    TempNode node = new TempNode(sourceFiles[i].Name);
                    node.FilePath = sourceFiles[i].FullName;
                    switch (sourceFiles[i].Extension)
                    {
                        case ".tt":
                            node.NodeType = "tt";
                            node.ImageIndex = 2;
                            node.SelectedImageIndex = 2;
                            break;
                        case ".cmt":
                            node.NodeType = "cmt";
                            node.ImageIndex = 2;
                            node.SelectedImageIndex = 2;
                            break;
                        case ".cs":
                            node.NodeType = "cs";
                            node.ImageIndex = 3;
                            node.SelectedImageIndex = 3;
                            break;
                        case ".vb":
                            node.NodeType = "vb";
                            node.ImageIndex = 4;
                            node.SelectedImageIndex = 4;
                            break;
                        case ".aspx":
                            node.NodeType = "aspx";
                            node.ImageIndex = 5;
                            node.SelectedImageIndex = 5;
                            break;
                        default:
                            node.ImageIndex = 2;
                            node.SelectedImageIndex = 2;
                            break;
                    }
                    parentnode.Nodes.Add(node);
                }
            }
        }

        #endregion


        #region listbox 操作

        private void btn_Addlist_Click(object sender, System.EventArgs e)
        {
            int c = this.listTable1.Items.Count;
            for (int i = 0; i < c; i++)
            {
                this.listTable2.Items.Add(this.listTable1.Items[i]);
            }
            this.listTable1.Items.Clear();

            IsHasItem();
        }

        private void btn_Add_Click(object sender, System.EventArgs e)
        {
            int c = this.listTable1.SelectedItems.Count;
            ListBox.SelectedObjectCollection objs = this.listTable1.SelectedItems;
            for (int i = 0; i < c; i++)
            {
                this.listTable2.Items.Add(listTable1.SelectedItems[i]);

            }
            for (int i = 0; i < c; i++)
            {
                if (this.listTable1.SelectedItems.Count > 0)
                {
                    this.listTable1.Items.Remove(listTable1.SelectedItems[0]);
                }
            }
            IsHasItem();
        }

        private void btn_Del_Click(object sender, System.EventArgs e)
        {
            int c = this.listTable2.SelectedItems.Count;
            //ListBox.SelectedObjectCollection objs = this.listTable2.SelectedItems;
            for (int i = 0; i < c; i++)
            {
                this.listTable1.Items.Add(listTable2.SelectedItems[i]);

            }
            for (int i = 0; i < c; i++)
            {
                if (this.listTable2.SelectedItems.Count > 0)
                {
                    this.listTable2.Items.Remove(listTable2.SelectedItems[0]);
                }
            }
            IsHasItem();
        }

        private void btn_Dellist_Click(object sender, System.EventArgs e)
        {
            int c = this.listTable2.Items.Count;
            for (int i = 0; i < c; i++)
            {
                this.listTable1.Items.Add(this.listTable2.Items[i]);
            }
            this.listTable2.Items.Clear();
            IsHasItem();
        }

        private void listTable1_DoubleClick(object sender, System.EventArgs e)
        {
            if (this.listTable1.SelectedItem != null)
            {
                this.listTable2.Items.Add(listTable1.SelectedItem);
                this.listTable1.Items.Remove(this.listTable1.SelectedItem);
                IsHasItem();
            }
        }

        private void listTable2_DoubleClick(object sender, System.EventArgs e)
        {
            if (this.listTable2.SelectedItem != null)
            {
                this.listTable1.Items.Add(listTable2.SelectedItem);
                this.listTable2.Items.Remove(this.listTable2.SelectedItem);
                IsHasItem();
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TempNode SelNode = (TempNode)this.treeView1.SelectedNode;
            if (SelNode == null)
                return;
            string nodeid = SelNode.NodeID;
            string text = SelNode.Text;
            string filepath = SelNode.FilePath;
            string nodetype = SelNode.NodeType;
            switch (nodetype)
            {
                case "tt":
                case "cmt":
                case "vb":
                case "aspx":
                case "cs":
                    {
                        if (filepath.Trim() != "")
                        {
                            string filename = filepath.Replace(TemplateFolder + "\\", "");
                            if (!hasTheFile(filename))
                            {
                                this.listBoxTemplate.Items.Add(filename);
                            }
                        }
                        else
                        {
                            MessageBox.Show("所选文件已经不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    break;
            }
            CheckTemplate();

        }

        private void btnAddTemp_Click(object sender, EventArgs e)
        {
            treeView1_DoubleClick(null, null);
        }

        private void btnRemoveTemp_Click(object sender, EventArgs e)
        {
            int c = this.listBoxTemplate.SelectedItems.Count;
            for (int i = 0; i < c; i++)
            {
                if (this.listBoxTemplate.SelectedItems.Count > 0)
                {
                    this.listBoxTemplate.Items.Remove(listBoxTemplate.SelectedItems[0]);
                }
            }
            CheckTemplate();
        }

        private void btnClearTemp_Click(object sender, EventArgs e)
        {
            this.listBoxTemplate.Items.Clear();
            CheckTemplate();
        }

        /// <summary>
        /// 判断listbox有没有项目
        /// </summary>
        private void IsHasItem()
        {
            if (this.listTable1.Items.Count > 0)
            {
                this.btn_Add.Enabled = true;
                this.btn_Addlist.Enabled = true;
            }
            else
            {
                this.btn_Add.Enabled = false;
                this.btn_Addlist.Enabled = false;
            }


            if (this.listTable2.Items.Count > 0  )
            {
                this.btn_Del.Enabled = true;
                this.btn_Dellist.Enabled = true;
                if(listBoxTemplate.Items.Count > 0)
                {
                    this.btn_Export.Enabled = true;
                    SetlblStatuText("当前选择：" + listTable2.Items.Count.ToString());
                }
            }
            else
            {
                this.btn_Del.Enabled = false;
                this.btn_Dellist.Enabled = false;
                this.btn_Export.Enabled = false;
            }
        }
        private void CheckTemplate()
        {
            if (this.listBoxTemplate.Items.Count > 0)
            {
                this.btnRemoveTemp.Enabled = true;
                this.btnClearTemp.Enabled = true;
                this.btn_Export.Enabled = true;
            }
            else
            {
                this.btnRemoveTemp.Enabled = false;
                this.btnClearTemp.Enabled = false;
                this.btn_Export.Enabled = false;
            }
        }
        private bool hasTheFile(string tempFilename)
        {
            bool hasThisFile = false;
            foreach (object obj in listBoxTemplate.Items)
            {
                if (obj.ToString() == tempFilename)
                {
                    hasThisFile = true;
                    break;
                }
            }
            return hasThisFile;
        }
        #endregion

        #region 异步控件设置
        public void SetBtnEnable()
        {
            if (this.btn_Export.InvokeRequired)
            {
                SetBtnEnableCallback d = new SetBtnEnableCallback(SetBtnEnable);
                this.Invoke(d, null);
            }
            else
            {
                this.btn_Export.Enabled = true;
                this.btn_Cancle.Enabled = true;
            }
        }
        public void SetBtnDisable()
        {
            if (this.btn_Export.InvokeRequired)
            {
                SetBtnDisableCallback d = new SetBtnDisableCallback(SetBtnDisable);
                this.Invoke(d, null);
            }
            else
            {
                this.btn_Export.Enabled = false;
                this.btn_Cancle.Enabled = false;
            }
        }
        public void SetlblStatuText(string text)
        {
            if (this.labelNum.InvokeRequired)
            {
                SetlblStatuCallback d = new SetlblStatuCallback(SetlblStatuText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelNum.Text = text;
            }
        }
        /// <summary>
        /// 循环网址进度最大值
        /// </summary>
        /// <param name="val"></param>
        public void SetprogressBar1Max(int val)
        {
            if (this.progressBar1.InvokeRequired)
            {
                SetProBar1MaxCallback d = new SetProBar1MaxCallback(SetprogressBar1Max);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.progressBar1.Maximum = val;

            }
        }
        /// <summary>
        /// 循环网址进度
        /// </summary>
        /// <param name="val"></param>
        public void SetprogressBar1Val(int val)
        {
            if (this.progressBar1.InvokeRequired)
            {
                SetProBar1ValCallback d = new SetProBar1ValCallback(SetprogressBar1Val);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.progressBar1.Value = val;

            }
        }
        #endregion


        #region 按钮
        private void btn_Cancle_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btn_Export_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (txtTargetFolder.Text.Trim() == "")
                {
                    MessageBox.Show("目标文件夹为空！");
                    return;
                }
                string TargetFolder = txtTargetFolder.Text;
                if (!Directory.Exists(TargetFolder))
                {
                    Directory.CreateDirectory(TargetFolder);
                }
                cfgfile.IniWriteValue("Project", "lastpath", txtTargetFolder.Text.Trim());
                dbname = this.cmbDB.Text;
                pictureBox1.Visible = true;
                mythread = new Thread(new ThreadStart(ThreadWork));
                mythread.Start();
                //ThreadWork();
            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }
        private void btn_TargetFold_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            DialogResult result = folder.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                this.txtTargetFolder.Text = folder.SelectedPath;
            }
        }
        #endregion

        void ThreadWork()
        {
            try
            {
                SetBtnDisable();
                int tblCount = this.listTable2.Items.Count;
                int tempCount = listBoxTemplate.Items.Count;
                SetlblStatuText("0");
                dbset.Folder = txtFolder.Text.Trim();
                DataTable dtEx = dbobj.GetTablesExProperty(dbname);

                CodeInfo codeinfo = null;
                bool hasError = false;


                if (radbtn_TempMerger.Checked) //按模板合并所有表保存
                {
                    SetprogressBar1Max(tempCount);
                    SetprogressBar1Val(1);

                    #region 循环模板

                    for (int t = 0; t < tempCount; t++)
                    {
                        string tempItem = listBoxTemplate.Items[t].ToString();
                        string Templatefilename = TemplateFolder + "\\" + tempItem;
                        if (!File.Exists(Templatefilename))
                        {
                            continue;
                        }

                        StringBuilder strcode = new StringBuilder();
                        string objtype = isProc ? "proc" : "table";

                        #region 循环表
                        for (int i = 0; i < tblCount; i++)
                        {
                            string tablename = this.listTable2.Items[i].ToString();

                            #region 表描述
                            string TableDescription = tablename;
                            if (dtEx != null)
                            {
                                try
                                {
                                    DataRow[] drs = dtEx.Select("objname='" + tablename + "'");
                                    if (drs.Length > 0)
                                    {
                                        if (drs[0]["value"] != null)
                                        {
                                            TableDescription = drs[0]["value"].ToString();
                                        }
                                    }
                                }
                                catch
                                { }
                            }
                            #endregion

                            #region 字段信息
                            //DataTable dtkey = dbobj.GetKeyName(dbname, tablename);
                            List<ColumnInfo> Fieldlist;
                            if (isProc)
                            {
                                Fieldlist = dbobj.GetColumnList(dbname, tablename);
                            }
                            else
                            {
                                Fieldlist = dbobj.GetColumnInfoList(dbname, tablename);
                            }
                            List<ColumnInfo> Keys = dbobj.GetKeyList(dbname, tablename);// CodeCommon.GetColumnInfos(dtkey);
                            List<ColumnInfo> FKeys = dbobj.GetFKeyList(dbname, tablename);
                            #endregion


                            #region 生成
                            try
                            {
                                codeinfo = new CodeInfo();
                                BuilderTemp bt = new BuilderTemp(dbobj, dbname, tablename, TableDescription, Fieldlist, Keys, FKeys,
                                    Templatefilename, dbset, objtype);
                                codeinfo = bt.GetCode();
                                if (codeinfo.ErrorMsg != null && codeinfo.ErrorMsg.Length > 0)
                                {
                                    hasError = true;
                                    if (codeinfo.ErrorMsg.IndexOf("无法将类型为“Maticsoft.CodeEngine.TableHost”的对象强制转换为类型“Maticsoft.CodeEngine.ProcedureHost”") > 0)
                                    {
                                        strcode.AppendLine(codeinfo.Code + Environment.NewLine +
                                                "/*代码生成时出现错误: 无法将表和视图与存储过程的模板进行匹配，请选择正确的模板！*/");
                                    }
                                    else
                                        if (codeinfo.ErrorMsg.IndexOf("无法将类型为“Maticsoft.CodeEngine.ProcedureHost”的对象强制转换为类型“Maticsoft.CodeEngine.TableHost”") > 0)
                                        {
                                            strcode.AppendLine(codeinfo.Code + Environment.NewLine +
                                                "/*代码生成时出现错误: 无法将存储过程与表和视图的模板进行匹配，请选择正确的模板！*/");
                                        }
                                        else
                                        {
                                            strcode.AppendLine(codeinfo.Code + Environment.NewLine +
                                                "/*------ 代码生成时出现错误: ------" + Environment.NewLine + codeinfo.ErrorMsg + "*/");
                                        }
                                }
                                else
                                {
                                    strcode.AppendLine(codeinfo.Code);
                                }
                            }
                            catch (System.Exception ex)
                            {
                                hasError = true;
                                strcode.AppendLine(ex.Message);
                            }
                            #endregion

                        }
                        #endregion

                        SetprogressBar1Val(t + 1);
                        SetlblStatuText((t + 1).ToString());


                        #region 保存
                        //string outputFilePath =txtTargetFolder.Text;
                        string outputFilePath = Path.Combine(txtTargetFolder.Text, tempItem.ToString());
                        string outputfileName = Path.GetFileNameWithoutExtension(outputFilePath);
                        string TargetFolder = txtTargetFolder.Text; //Path.Combine(Path.GetDirectoryName(outputFilePath), outputFolderName);
                        if (!Directory.Exists(TargetFolder))
                        {
                            Directory.CreateDirectory(TargetFolder);
                        }
                        if (isProc)
                        {
                            string outputFileName = Path.Combine(TargetFolder, dbname + codeinfo.FileExtension);
                            File.AppendAllText(outputFileName, strcode.ToString(), Encoding.UTF8);
                        }
                        else
                        {
                            string outputFileName = Path.Combine(TargetFolder, outputfileName + codeinfo.FileExtension);
                            File.WriteAllText(outputFileName, strcode.ToString(), Encoding.UTF8);
                        }
                        #endregion

                    }

                    #endregion

                }
                else
                {
                    SetprogressBar1Max(tblCount);
                    SetprogressBar1Val(1);

                    #region 循环每个表
                    for (int i = 0; i < tblCount; i++)
                    {
                        string tablename = this.listTable2.Items[i].ToString();

                        #region 表描述
                        string TableDescription = tablename;
                        if (dtEx != null)
                        {
                            try
                            {
                                DataRow[] drs = dtEx.Select("objname='" + tablename + "'");
                                if (drs.Length > 0)
                                {
                                    if (drs[0]["value"] != null)
                                    {
                                        TableDescription = drs[0]["value"].ToString();
                                    }
                                }
                            }
                            catch
                            { }
                        }
                        #endregion

                        #region 字段信息
                        //DataTable dtkey = dbobj.GetKeyName(dbname, tablename);
                        List<ColumnInfo> Fieldlist;
                        if (isProc)
                        {
                            Fieldlist = dbobj.GetColumnList(dbname, tablename);
                        }
                        else
                        {
                            Fieldlist = dbobj.GetColumnInfoList(dbname, tablename);
                        }
                        List<ColumnInfo> Keys = dbobj.GetKeyList(dbname, tablename);// CodeCommon.GetColumnInfos(dtkey);
                        List<ColumnInfo> FKeys = dbobj.GetFKeyList(dbname, tablename);
                        #endregion


                        //循环模板
                        foreach (object tempItem in listBoxTemplate.Items)
                        {
                            string Templatefilename = TemplateFolder + "\\" + tempItem.ToString();
                            if (!File.Exists(Templatefilename))
                            {
                                continue;
                            }

                            string strcode = "";
                            string objtype = isProc ? "proc" : "table";

                            #region 生成
                            try
                            {
                                codeinfo = new CodeInfo();
                                BuilderTemp bt = new BuilderTemp(dbobj, dbname, tablename, TableDescription, Fieldlist, Keys, FKeys,
                                    Templatefilename, dbset, objtype);
                                codeinfo = bt.GetCode();
                                if (codeinfo.ErrorMsg != null && codeinfo.ErrorMsg.Length > 0)
                                {
                                    hasError = true;
                                    if (codeinfo.ErrorMsg.IndexOf("无法将类型为“Maticsoft.CodeEngine.TableHost”的对象强制转换为类型“Maticsoft.CodeEngine.ProcedureHost”") > 0)
                                    {
                                        strcode = codeinfo.Code + Environment.NewLine +
                                                "/*代码生成时出现错误: 无法将表和视图与存储过程的模板进行匹配，请选择正确的模板！*/";
                                    }
                                    else
                                        if (codeinfo.ErrorMsg.IndexOf("无法将类型为“Maticsoft.CodeEngine.ProcedureHost”的对象强制转换为类型“Maticsoft.CodeEngine.TableHost”") > 0)
                                        {
                                            strcode = codeinfo.Code + Environment.NewLine +
                                                "/*代码生成时出现错误: 无法将存储过程与表和视图的模板进行匹配，请选择正确的模板！*/";
                                        }
                                        else
                                        {
                                            strcode = codeinfo.Code + Environment.NewLine +
                                                "/*------ 代码生成时出现错误: ------" + Environment.NewLine + codeinfo.ErrorMsg + "*/";
                                        }
                                }
                                else
                                {
                                    strcode = codeinfo.Code;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                hasError = true;
                                strcode += ex.Message;
                            }
                            #endregion

                            #region 保存
                            string outputFilePath = Path.Combine(txtTargetFolder.Text, tempItem.ToString());
                            string outputFolderName = Path.GetFileNameWithoutExtension(outputFilePath);
                            string TargetFolder = Path.Combine(Path.GetDirectoryName(outputFilePath), outputFolderName);
                            if (!Directory.Exists(TargetFolder))
                            {
                                Directory.CreateDirectory(TargetFolder);
                            }
                            if (isProc)
                            {
                                string outputFileName = Path.Combine(TargetFolder, dbname + codeinfo.FileExtension);
                                File.AppendAllText(outputFileName, strcode, Encoding.UTF8);
                            }
                            else
                            {
                                string outputFileName = Path.Combine(TargetFolder, tablename + codeinfo.FileExtension);
                                File.WriteAllText(outputFileName, strcode, Encoding.UTF8);
                            }
                            #endregion

                        }
                        SetprogressBar1Val(i + 1);
                        SetlblStatuText((i + 1).ToString());
                    }

                    #endregion

                }


                SetBtnEnable();
                if (hasError)
                {
                    MessageBox.Show(this, "代码生成完成，但有错误，请查看生成代码！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, "代码生成成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (System.Exception er)
            {
                LogInfo.WriteLog(er);
                MessageBox.Show(er.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        #region 生成 C#代码

        //把代码写入指定文件
        private void WriteFile(string Filename, string strCode)
        {
            StreamWriter sw = new StreamWriter(Filename, false, Encoding.UTF8);//,false);
            sw.Write(strCode);
            sw.Flush();
            sw.Close();
        }

        #endregion

        #region 公共方法
        private void FolderCheck(string Folder)
        {
            DirectoryInfo target = new DirectoryInfo(Folder);
            if (!target.Exists)
            {
                target.Create();
            }
        }
        /// <summary>
        ///  修改项目文件
        /// </summary>
        /// <param name="ProjectFile">项目文件名</param>
        /// <param name="classFileName">类文件名</param>
        /// <param name="ProType">项目类型</param>
        private void AddClassFile(string ProjectFile, string classFileName, string ProType)
        {
            if (File.Exists(ProjectFile))
            {
                switch (ProType)
                {
                    case "2003":
                        vsp.AddClass2003(ProjectFile, classFileName);
                        break;
                    case "2005":
                        vsp.AddClass2005(ProjectFile, classFileName);
                        break;
                    default:
                        vsp.AddClass(ProjectFile, classFileName);
                        break;
                }
            }
        }
        /// <summary>
        /// 命名空间名检查
        /// </summary>
        /// <param name="SourceDirectory"></param>
        public void CheckDirectory(string SourceDirectory)
        {
            DirectoryInfo source = new DirectoryInfo(SourceDirectory);
            if (!source.Exists)
                return;

            FileInfo[] sourceFiles = source.GetFiles();
            int filescount = sourceFiles.Length;
            for (int i = 0; i < filescount; ++i)
            {
                switch (sourceFiles[i].Extension)
                {
                    case ".csproj":
                        //ReplaceNamespaceProj(sourceFiles[i].FullName, txtNamespace.Text.Trim());
                        break;
                    case ".cs":
                    case ".ascx":
                    case ".aspx":
                    case ".asax":
                    case ".master":
                        //ReplaceNamespace(sourceFiles[i].FullName, txtNamespace.Text.Trim());
                        break;
                    default:
                        break;
                }
            }

            DirectoryInfo[] sourceDirectories = source.GetDirectories();
            for (int j = 0; j < sourceDirectories.Length; ++j)
            {
                CheckDirectory(sourceDirectories[j].FullName);
            }
        }
        private void ReplaceNamespace(string filename, string spacename)
        {
            StreamReader sr = new StreamReader(filename, Encoding.Default);
            string text = sr.ReadToEnd();
            sr.Close();

            text = text.Replace("<$$namespace$$>", spacename);
            //text = text.Replace("namespace Maticsoft", "namespace " + spacename);
            //text = text.Replace("Inherits=\"Maticsoft", "Inherits=\"" + spacename);

            StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8);//,false);
            sw.Write(text);
            sw.Flush();//从缓冲区写入基础流（文件）
            sw.Close();
        }
        private void ReplaceNamespaceProj(string filename, string spacename)
        {
            StreamReader sr = new StreamReader(filename, Encoding.Default);
            string text = sr.ReadToEnd();
            sr.Close();

            text = text.Replace("<AssemblyName>Maticsoft.", "<AssemblyName>" + spacename + ".");
            text = text.Replace("<RootNamespace>Maticsoft.", "<RootNamespace>" + spacename + ".");

            StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8);//,false);
            sw.Write(text);
            sw.Flush();//从缓冲区写入基础流（文件）
            sw.Close();
        }

        /// <summary>
        /// 表的描述信息处理
        /// </summary>       
        private string CutTableDescription(string TableName, string TableDescription)
        {
            string newDeText = "";
            if (TableDescription.Trim().Length > 0)
            {

                int n = 0;
                int n1 = TableDescription.IndexOf(";");
                int n2 = TableDescription.IndexOf("，");
                int n3 = TableDescription.IndexOf(",");

                n = Math.Min(n1, n2);
                if (n < 0)
                {
                    n = Math.Max(n1, n2);
                }
                n = Math.Min(n, n3);
                if (n < 0)
                {
                    n = Math.Max(n1, n2);
                }

                if (n > 0)
                {
                    newDeText = TableDescription.Trim().Substring(0, n);
                }
                else
                {
                    if (TableDescription.Trim().Length > 10)
                    {
                        newDeText = TableDescription.Trim().Substring(0, 10);
                    }
                    else
                    {
                        newDeText = TableDescription.Trim();
                    }
                }
            }
            else
            {
                newDeText = TableName;
            }
            return newDeText;
        }

        #endregion


        /// <summary>
        /// 手工输入表名，快速选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInputTxt_Click(object sender, EventArgs e)
        {
            CodeExpTabInput tabForm = new CodeExpTabInput();
            DialogResult dr = tabForm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                listTable2.Items.Clear();
                foreach (string line in tabForm.txtTableList.Lines)
                {
                    listTable2.Items.Add(line);
                }
            }
            IsHasItem();
        }





    }
}
