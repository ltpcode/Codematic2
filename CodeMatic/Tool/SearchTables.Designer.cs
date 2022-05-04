namespace Codematic
{
    partial class SearchTables
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkClearList = new System.Windows.Forms.CheckBox();
            this.cmbDB = new System.Windows.Forms.ComboBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtKeyWord = new System.Windows.Forms.TextBox();
            this.cmboxType = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.所选表名ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部表名ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.所选字段ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部字段ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExpToWord = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTip = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.代码生成器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkClearList);
            this.groupBox1.Controls.Add(this.cmbDB);
            this.groupBox1.Controls.Add(this.lblServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtKeyWord);
            this.groupBox1.Controls.Add(this.cmboxType);
            this.groupBox1.Location = new System.Drawing.Point(17, 16);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(660, 125);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询选项";
            // 
            // chkClearList
            // 
            this.chkClearList.AutoSize = true;
            this.chkClearList.Checked = true;
            this.chkClearList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkClearList.Location = new System.Drawing.Point(221, 98);
            this.chkClearList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkClearList.Name = "chkClearList";
            this.chkClearList.Size = new System.Drawing.Size(199, 24);
            this.chkClearList.TabIndex = 7;
            this.chkClearList.Text = "清空查询结果列表";
            this.chkClearList.UseVisualStyleBackColor = true;
            // 
            // cmbDB
            // 
            this.cmbDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDB.Location = new System.Drawing.Point(415, 28);
            this.cmbDB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbDB.Name = "cmbDB";
            this.cmbDB.Size = new System.Drawing.Size(201, 23);
            this.cmbDB.TabIndex = 6;
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(159, 30);
            this.lblServer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(0, 15);
            this.lblServer.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "当前服务器：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(308, 30);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "选择数据库：";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(552, 60);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 58);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtKeyWord
            // 
            this.txtKeyWord.Location = new System.Drawing.Point(209, 61);
            this.txtKeyWord.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtKeyWord.Name = "txtKeyWord";
            this.txtKeyWord.Size = new System.Drawing.Size(335, 25);
            this.txtKeyWord.TabIndex = 1;
            // 
            // cmboxType
            // 
            this.cmboxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboxType.FormattingEnabled = true;
            this.cmboxType.Items.AddRange(new object[] {
            "字段名称包含",
            "字段类型是",
            "表名称包含"});
            this.cmboxType.Location = new System.Drawing.Point(8, 61);
            this.cmboxType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmboxType.Name = "cmboxType";
            this.cmboxType.Size = new System.Drawing.Size(191, 23);
            this.cmboxType.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listView1);
            this.groupBox2.Location = new System.Drawing.Point(16, 149);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(660, 261);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "查询结果";
            // 
            // listView1
            // 
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(4, 21);
            this.listView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(651, 235);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制ToolStripMenuItem,
            this.移除ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.代码生成器ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 104);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.所选表名ToolStripMenuItem,
            this.全部表名ToolStripMenuItem1,
            this.所选字段ToolStripMenuItem,
            this.全部字段ToolStripMenuItem});
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.复制ToolStripMenuItem.Text = "复制";
            // 
            // 所选表名ToolStripMenuItem
            // 
            this.所选表名ToolStripMenuItem.Name = "所选表名ToolStripMenuItem";
            this.所选表名ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.所选表名ToolStripMenuItem.Text = "所选表名";
            this.所选表名ToolStripMenuItem.Click += new System.EventHandler(this.所选表名ToolStripMenuItem_Click);
            // 
            // 全部表名ToolStripMenuItem1
            // 
            this.全部表名ToolStripMenuItem1.Name = "全部表名ToolStripMenuItem1";
            this.全部表名ToolStripMenuItem1.Size = new System.Drawing.Size(138, 24);
            this.全部表名ToolStripMenuItem1.Text = "全部表名";
            this.全部表名ToolStripMenuItem1.Click += new System.EventHandler(this.全部表名ToolStripMenuItem_Click);
            // 
            // 所选字段ToolStripMenuItem
            // 
            this.所选字段ToolStripMenuItem.Name = "所选字段ToolStripMenuItem";
            this.所选字段ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.所选字段ToolStripMenuItem.Text = "所选字段";
            this.所选字段ToolStripMenuItem.Click += new System.EventHandler(this.所选字段ToolStripMenuItem_Click);
            // 
            // 全部字段ToolStripMenuItem
            // 
            this.全部字段ToolStripMenuItem.Name = "全部字段ToolStripMenuItem";
            this.全部字段ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.全部字段ToolStripMenuItem.Text = "全部字段";
            this.全部字段ToolStripMenuItem.Click += new System.EventHandler(this.全部字段ToolStripMenuItem_Click);
            // 
            // 移除ToolStripMenuItem
            // 
            this.移除ToolStripMenuItem.Name = "移除ToolStripMenuItem";
            this.移除ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.移除ToolStripMenuItem.Text = "移除";
            this.移除ToolStripMenuItem.Click += new System.EventHandler(this.移除ToolStripMenuItem_Click);
            // 
            // btnExpToWord
            // 
            this.btnExpToWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpToWord.Location = new System.Drawing.Point(463, 425);
            this.btnExpToWord.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExpToWord.Name = "btnExpToWord";
            this.btnExpToWord.Size = new System.Drawing.Size(100, 29);
            this.btnExpToWord.TabIndex = 1;
            this.btnExpToWord.Text = "导出文档";
            this.btnExpToWord.UseVisualStyleBackColor = true;
            this.btnExpToWord.Click += new System.EventHandler(this.btnExpToWord_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(577, 425);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 29);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTip
            // 
            this.lblTip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTip.AutoSize = true;
            this.lblTip.Location = new System.Drawing.Point(25, 436);
            this.lblTip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(0, 15);
            this.lblTip.TabIndex = 2;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(4, 456);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(684, 25);
            this.progressBar1.TabIndex = 3;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(150, 6);
            // 
            // 代码生成器ToolStripMenuItem
            // 
            this.代码生成器ToolStripMenuItem.Name = "代码生成器ToolStripMenuItem";
            this.代码生成器ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.代码生成器ToolStripMenuItem.Text = "代码生成器";
            this.代码生成器ToolStripMenuItem.Click += new System.EventHandler(this.代码生成器ToolStripMenuItem_Click);
            // 
            // SearchTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 481);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblTip);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExpToWord);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchTables";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "搜索表";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnExpToWord;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtKeyWord;
        private System.Windows.Forms.ComboBox cmboxType;
        private System.Windows.Forms.ComboBox cmbDB;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 所选表名ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部表名ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 所选字段ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部字段ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移除ToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkClearList;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 代码生成器ToolStripMenuItem;
    }
}