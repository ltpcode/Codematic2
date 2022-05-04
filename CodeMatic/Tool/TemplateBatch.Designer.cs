namespace Codematic
{
    partial class TemplateBatch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("默认", 2, 2);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("实体类", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("C#代码");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("VB代码");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("页面");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("代码模版", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateBatch));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbDB = new System.Windows.Forms.ComboBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtTargetFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_TargetFold = new WiB.Pinkie.Controls.ButtonXP();
            this.btn_Addlist = new System.Windows.Forms.Button();
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_Dellist = new System.Windows.Forms.Button();
            this.btn_Export = new WiB.Pinkie.Controls.ButtonXP();
            this.btn_Del = new System.Windows.Forms.Button();
            this.listTable2 = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnInputTxt = new System.Windows.Forms.Button();
            this.listTable1 = new System.Windows.Forms.ListBox();
            this.btn_Cancle = new WiB.Pinkie.Controls.ButtonXP();
            this.labelNum = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listBoxTemplate = new System.Windows.Forms.ListBox();
            this.btnAddTemp = new System.Windows.Forms.Button();
            this.btnRemoveTemp = new System.Windows.Forms.Button();
            this.btnClearTemp = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.radbtn_TabTemp = new System.Windows.Forms.RadioButton();
            this.radbtn_TempMerger = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cmbDB);
            this.groupBox1.Controls.Add(this.lblServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(9, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(587, 48);
            this.groupBox1.TabIndex = 53;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择数据库";
            // 
            // cmbDB
            // 
            this.cmbDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDB.Location = new System.Drawing.Point(391, 21);
            this.cmbDB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbDB.Name = "cmbDB";
            this.cmbDB.Size = new System.Drawing.Size(153, 20);
            this.cmbDB.TabIndex = 2;
            this.cmbDB.SelectedIndexChanged += new System.EventHandler(this.cmbDB_SelectedIndexChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(104, 22);
            this.lblServer.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 12);
            this.lblServer.TabIndex = 1;
            this.lblServer.Text = "Server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前服务器：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(311, 22);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "选择数据库：";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.radbtn_TempMerger);
            this.groupBox3.Controls.Add(this.radbtn_TabTemp);
            this.groupBox3.Controls.Add(this.pictureBox1);
            this.groupBox3.Controls.Add(this.txtTargetFolder);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.btn_TargetFold);
            this.groupBox3.Location = new System.Drawing.Point(9, 357);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(587, 81);
            this.groupBox3.TabIndex = 57;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "保存位置";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Codematic.Properties.Resources.Control;
            this.pictureBox1.Location = new System.Drawing.Point(10, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 29);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 53;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // txtTargetFolder
            // 
            this.txtTargetFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetFolder.Location = new System.Drawing.Point(104, 18);
            this.txtTargetFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTargetFolder.Name = "txtTargetFolder";
            this.txtTargetFolder.Size = new System.Drawing.Size(395, 21);
            this.txtTargetFolder.TabIndex = 45;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 22);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 44;
            this.label2.Text = "输出目录：";
            // 
            // btn_TargetFold
            // 
            this.btn_TargetFold._Image = null;
            this.btn_TargetFold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btn_TargetFold.DefaultScheme = false;
            this.btn_TargetFold.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_TargetFold.Image = null;
            this.btn_TargetFold.Location = new System.Drawing.Point(505, 18);
            this.btn_TargetFold.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_TargetFold.Name = "btn_TargetFold";
            this.btn_TargetFold.Scheme = WiB.Pinkie.Controls.ButtonXP.Schemes.Blue;
            this.btn_TargetFold.Size = new System.Drawing.Size(57, 23);
            this.btn_TargetFold.TabIndex = 46;
            this.btn_TargetFold.Text = "选择...";
            this.btn_TargetFold.Click += new System.EventHandler(this.btn_TargetFold_Click);
            // 
            // btn_Addlist
            // 
            this.btn_Addlist.Enabled = false;
            this.btn_Addlist.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Addlist.Location = new System.Drawing.Point(275, 21);
            this.btn_Addlist.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Addlist.Name = "btn_Addlist";
            this.btn_Addlist.Size = new System.Drawing.Size(40, 23);
            this.btn_Addlist.TabIndex = 7;
            this.btn_Addlist.Text = ">>";
            this.btn_Addlist.Click += new System.EventHandler(this.btn_Addlist_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.Enabled = false;
            this.btn_Add.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Add.Location = new System.Drawing.Point(275, 50);
            this.btn_Add.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(40, 23);
            this.btn_Add.TabIndex = 8;
            this.btn_Add.Text = ">";
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_Dellist
            // 
            this.btn_Dellist.Enabled = false;
            this.btn_Dellist.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Dellist.Location = new System.Drawing.Point(275, 107);
            this.btn_Dellist.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Dellist.Name = "btn_Dellist";
            this.btn_Dellist.Size = new System.Drawing.Size(40, 23);
            this.btn_Dellist.TabIndex = 6;
            this.btn_Dellist.Text = "<<";
            this.btn_Dellist.Click += new System.EventHandler(this.btn_Dellist_Click);
            // 
            // btn_Export
            // 
            this.btn_Export._Image = null;
            this.btn_Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Export.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btn_Export.DefaultScheme = false;
            this.btn_Export.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Export.Image = null;
            this.btn_Export.Location = new System.Drawing.Point(398, 451);
            this.btn_Export.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Scheme = WiB.Pinkie.Controls.ButtonXP.Schemes.Blue;
            this.btn_Export.Size = new System.Drawing.Size(75, 26);
            this.btn_Export.TabIndex = 56;
            this.btn_Export.Text = "导出";
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // btn_Del
            // 
            this.btn_Del.Enabled = false;
            this.btn_Del.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Del.Location = new System.Drawing.Point(275, 78);
            this.btn_Del.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(40, 23);
            this.btn_Del.TabIndex = 5;
            this.btn_Del.Text = "<";
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // listTable2
            // 
            this.listTable2.ItemHeight = 12;
            this.listTable2.Location = new System.Drawing.Point(356, 21);
            this.listTable2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listTable2.Name = "listTable2";
            this.listTable2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listTable2.Size = new System.Drawing.Size(218, 112);
            this.listTable2.TabIndex = 1;
            this.listTable2.DoubleClick += new System.EventHandler(this.listTable2_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnInputTxt);
            this.groupBox2.Controls.Add(this.btn_Addlist);
            this.groupBox2.Controls.Add(this.btn_Add);
            this.groupBox2.Controls.Add(this.btn_Del);
            this.groupBox2.Controls.Add(this.btn_Dellist);
            this.groupBox2.Controls.Add(this.listTable2);
            this.groupBox2.Controls.Add(this.listTable1);
            this.groupBox2.Location = new System.Drawing.Point(9, 55);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(587, 140);
            this.groupBox2.TabIndex = 54;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择表和视图";
            // 
            // btnInputTxt
            // 
            this.btnInputTxt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnInputTxt.Location = new System.Drawing.Point(334, 109);
            this.btnInputTxt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnInputTxt.Name = "btnInputTxt";
            this.btnInputTxt.Size = new System.Drawing.Size(19, 23);
            this.btnInputTxt.TabIndex = 10;
            this.btnInputTxt.Text = "Txt";
            this.btnInputTxt.UseVisualStyleBackColor = true;
            this.btnInputTxt.Click += new System.EventHandler(this.btnInputTxt_Click);
            // 
            // listTable1
            // 
            this.listTable1.ItemHeight = 12;
            this.listTable1.Location = new System.Drawing.Point(10, 21);
            this.listTable1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listTable1.Name = "listTable1";
            this.listTable1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listTable1.Size = new System.Drawing.Size(219, 112);
            this.listTable1.TabIndex = 0;
            this.listTable1.DoubleClick += new System.EventHandler(this.listTable1_DoubleClick);
            // 
            // btn_Cancle
            // 
            this.btn_Cancle._Image = null;
            this.btn_Cancle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Cancle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btn_Cancle.DefaultScheme = false;
            this.btn_Cancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancle.Image = null;
            this.btn_Cancle.Location = new System.Drawing.Point(502, 451);
            this.btn_Cancle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Scheme = WiB.Pinkie.Controls.ButtonXP.Schemes.Blue;
            this.btn_Cancle.Size = new System.Drawing.Size(75, 26);
            this.btn_Cancle.TabIndex = 55;
            this.btn_Cancle.Text = "关闭";
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // labelNum
            // 
            this.labelNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelNum.Location = new System.Drawing.Point(7, 466);
            this.labelNum.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNum.Name = "labelNum";
            this.labelNum.Size = new System.Drawing.Size(90, 22);
            this.labelNum.TabIndex = 51;
            this.labelNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(2, 491);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(600, 19);
            this.progressBar1.TabIndex = 52;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listBoxTemplate);
            this.groupBox4.Controls.Add(this.btnAddTemp);
            this.groupBox4.Controls.Add(this.btnRemoveTemp);
            this.groupBox4.Controls.Add(this.btnClearTemp);
            this.groupBox4.Controls.Add(this.treeView1);
            this.groupBox4.Location = new System.Drawing.Point(9, 195);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox4.Size = new System.Drawing.Size(587, 121);
            this.groupBox4.TabIndex = 58;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "选择模板";
            // 
            // listBoxTemplate
            // 
            this.listBoxTemplate.ItemHeight = 12;
            this.listBoxTemplate.Location = new System.Drawing.Point(356, 15);
            this.listBoxTemplate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBoxTemplate.Name = "listBoxTemplate";
            this.listBoxTemplate.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxTemplate.Size = new System.Drawing.Size(218, 100);
            this.listBoxTemplate.TabIndex = 13;
            // 
            // btnAddTemp
            // 
            this.btnAddTemp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAddTemp.Location = new System.Drawing.Point(276, 28);
            this.btnAddTemp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAddTemp.Name = "btnAddTemp";
            this.btnAddTemp.Size = new System.Drawing.Size(40, 23);
            this.btnAddTemp.TabIndex = 12;
            this.btnAddTemp.Text = ">";
            this.btnAddTemp.Click += new System.EventHandler(this.btnAddTemp_Click);
            // 
            // btnRemoveTemp
            // 
            this.btnRemoveTemp.Enabled = false;
            this.btnRemoveTemp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemoveTemp.Location = new System.Drawing.Point(276, 54);
            this.btnRemoveTemp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnRemoveTemp.Name = "btnRemoveTemp";
            this.btnRemoveTemp.Size = new System.Drawing.Size(40, 23);
            this.btnRemoveTemp.TabIndex = 9;
            this.btnRemoveTemp.Text = "<";
            this.btnRemoveTemp.Click += new System.EventHandler(this.btnRemoveTemp_Click);
            // 
            // btnClearTemp
            // 
            this.btnClearTemp.Enabled = false;
            this.btnClearTemp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClearTemp.Location = new System.Drawing.Point(276, 81);
            this.btnClearTemp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnClearTemp.Name = "btnClearTemp";
            this.btnClearTemp.Size = new System.Drawing.Size(40, 23);
            this.btnClearTemp.TabIndex = 10;
            this.btnClearTemp.Text = "<<";
            this.btnClearTemp.Click += new System.EventHandler(this.btnClearTemp_Click);
            // 
            // treeView1
            // 
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(10, 15);
            this.treeView1.Name = "treeView1";
            treeNode1.ImageIndex = 2;
            treeNode1.Name = "节点6";
            treeNode1.SelectedImageIndex = 2;
            treeNode1.Text = "默认";
            treeNode2.Name = "节点4";
            treeNode2.Text = "实体类";
            treeNode3.Name = "节点2";
            treeNode3.Text = "C#代码";
            treeNode4.Name = "节点5";
            treeNode4.Text = "VB代码";
            treeNode5.Name = "节点3";
            treeNode5.Text = "页面";
            treeNode6.Name = "节点0";
            treeNode6.Text = "代码模版";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(219, 100);
            this.treeView1.TabIndex = 2;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folderclose.gif");
            this.imageList1.Images.SetKeyName(1, "Folderopen.gif");
            this.imageList1.Images.SetKeyName(2, "temp.png");
            this.imageList1.Images.SetKeyName(3, "cs32.gif");
            this.imageList1.Images.SetKeyName(4, "vb.gif");
            this.imageList1.Images.SetKeyName(5, "aspx.png");
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtFolder);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Location = new System.Drawing.Point(9, 316);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox5.Size = new System.Drawing.Size(587, 38);
            this.groupBox5.TabIndex = 59;
            this.groupBox5.TabStop = false;
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(161, 13);
            this.txtFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(125, 21);
            this.txtFolder.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 18);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "二级命名空间名：";
            // 
            // radbtn_TabTemp
            // 
            this.radbtn_TabTemp.AutoSize = true;
            this.radbtn_TabTemp.Checked = true;
            this.radbtn_TabTemp.Location = new System.Drawing.Point(104, 51);
            this.radbtn_TabTemp.Name = "radbtn_TabTemp";
            this.radbtn_TabTemp.Size = new System.Drawing.Size(131, 16);
            this.radbtn_TabTemp.TabIndex = 54;
            this.radbtn_TabTemp.TabStop = true;
            this.radbtn_TabTemp.Text = "按模板和表独立保存";
            this.radbtn_TabTemp.UseVisualStyleBackColor = true;
            // 
            // radbtn_TempMerger
            // 
            this.radbtn_TempMerger.AutoSize = true;
            this.radbtn_TempMerger.Location = new System.Drawing.Point(264, 51);
            this.radbtn_TempMerger.Name = "radbtn_TempMerger";
            this.radbtn_TempMerger.Size = new System.Drawing.Size(107, 16);
            this.radbtn_TempMerger.TabIndex = 54;
            this.radbtn_TempMerger.Text = "按模板合并保存";
            this.radbtn_TempMerger.UseVisualStyleBackColor = true;
            // 
            // TemplateBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancle;
            this.ClientSize = new System.Drawing.Size(608, 511);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.labelNum);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btn_Export);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn_Cancle);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TemplateBatch";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "模板代码批量生成";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.TemplateBatch_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbDB;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtTargetFolder;
        private System.Windows.Forms.Label label2;
        private WiB.Pinkie.Controls.ButtonXP btn_TargetFold;
        private System.Windows.Forms.Button btn_Addlist;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Dellist;
        private WiB.Pinkie.Controls.ButtonXP btn_Export;
        private System.Windows.Forms.Button btn_Del;
        private System.Windows.Forms.ListBox listTable2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listTable1;
        private WiB.Pinkie.Controls.ButtonXP btn_Cancle;
        private System.Windows.Forms.Label labelNum;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnAddTemp;
        private System.Windows.Forms.Button btnRemoveTemp;
        private System.Windows.Forms.Button btnClearTemp;
        private System.Windows.Forms.ListBox listBoxTemplate;
        private System.Windows.Forms.Button btnInputTxt;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radbtn_TabTemp;
        private System.Windows.Forms.RadioButton radbtn_TempMerger;
    }
}