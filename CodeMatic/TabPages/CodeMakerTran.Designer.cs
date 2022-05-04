namespace Codematic
{
    partial class CodeMakerTran
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeMakerTran));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbox_Field = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.radbtn_Update = new System.Windows.Forms.RadioButton();
            this.radbtn_Delete = new System.Windows.Forms.RadioButton();
            this.radbtn_Insert = new System.Windows.Forms.RadioButton();
            this.listTable = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_Create = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtNameSpace2 = new System.Windows.Forms.TextBox();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbox_Field);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.radbtn_Update);
            this.groupBox1.Controls.Add(this.radbtn_Delete);
            this.groupBox1.Controls.Add(this.radbtn_Insert);
            this.groupBox1.Controls.Add(this.listTable);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(551, 198);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择表和操作";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(305, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "条件字段：";
            this.label3.Visible = false;
            // 
            // cmbox_Field
            // 
            this.cmbox_Field.FormattingEnabled = true;
            this.cmbox_Field.Location = new System.Drawing.Point(305, 100);
            this.cmbox_Field.Name = "cmbox_Field";
            this.cmbox_Field.Size = new System.Drawing.Size(211, 20);
            this.cmbox_Field.TabIndex = 3;
            this.cmbox_Field.Visible = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(425, 127);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(91, 32);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加操作";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // radbtn_Update
            // 
            this.radbtn_Update.AutoSize = true;
            this.radbtn_Update.Location = new System.Drawing.Point(380, 41);
            this.radbtn_Update.Name = "radbtn_Update";
            this.radbtn_Update.Size = new System.Drawing.Size(59, 16);
            this.radbtn_Update.TabIndex = 1;
            this.radbtn_Update.TabStop = true;
            this.radbtn_Update.Text = "Update";
            this.radbtn_Update.UseVisualStyleBackColor = true;
            this.radbtn_Update.Click += new System.EventHandler(this.radbtn_Click);
            // 
            // radbtn_Delete
            // 
            this.radbtn_Delete.AutoSize = true;
            this.radbtn_Delete.Location = new System.Drawing.Point(453, 41);
            this.radbtn_Delete.Name = "radbtn_Delete";
            this.radbtn_Delete.Size = new System.Drawing.Size(59, 16);
            this.radbtn_Delete.TabIndex = 1;
            this.radbtn_Delete.TabStop = true;
            this.radbtn_Delete.Text = "Delete";
            this.radbtn_Delete.UseVisualStyleBackColor = true;
            this.radbtn_Delete.Click += new System.EventHandler(this.radbtn_Click);
            // 
            // radbtn_Insert
            // 
            this.radbtn_Insert.AutoSize = true;
            this.radbtn_Insert.Checked = true;
            this.radbtn_Insert.Location = new System.Drawing.Point(307, 41);
            this.radbtn_Insert.Name = "radbtn_Insert";
            this.radbtn_Insert.Size = new System.Drawing.Size(59, 16);
            this.radbtn_Insert.TabIndex = 1;
            this.radbtn_Insert.TabStop = true;
            this.radbtn_Insert.Text = "Insert";
            this.radbtn_Insert.UseVisualStyleBackColor = true;
            this.radbtn_Insert.Click += new System.EventHandler(this.radbtn_Click);
            // 
            // listTable
            // 
            this.listTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listTable.FormattingEnabled = true;
            this.listTable.ItemHeight = 12;
            this.listTable.Location = new System.Drawing.Point(9, 20);
            this.listTable.Name = "listTable";
            this.listTable.Size = new System.Drawing.Size(271, 172);
            this.listTable.TabIndex = 0;
            this.listTable.SelectedIndexChanged += new System.EventHandler(this.listTable_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listView1);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(551, 134);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "事务操作";
            // 
            // listView1
            // 
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(3, 17);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(545, 114);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除DToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(118, 26);
            // 
            // 删除DToolStripMenuItem
            // 
            this.删除DToolStripMenuItem.Name = "删除DToolStripMenuItem";
            this.删除DToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除DToolStripMenuItem.Text = "删除(&D)";
            this.删除DToolStripMenuItem.Click += new System.EventHandler(this.删除DToolStripMenuItem_Click);
            // 
            // btn_Create
            // 
            this.btn_Create.Location = new System.Drawing.Point(428, 226);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(102, 41);
            this.btn_Create.TabIndex = 2;
            this.btn_Create.Text = "生成代码";
            this.btn_Create.UseVisualStyleBackColor = true;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtNameSpace2);
            this.groupBox3.Controls.Add(this.txtNameSpace);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(0, 137);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(548, 76);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "参数";
            // 
            // txtNameSpace2
            // 
            this.txtNameSpace2.Location = new System.Drawing.Point(360, 20);
            this.txtNameSpace2.Name = "txtNameSpace2";
            this.txtNameSpace2.Size = new System.Drawing.Size(162, 21);
            this.txtNameSpace2.TabIndex = 1;
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(104, 20);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(141, 21);
            this.txtNameSpace.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(269, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "二级命名空间：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "顶级命名空间：";
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(565, 538);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.ImageIndex = 0;
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(557, 511);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "事务设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.ImageIndex = 1;
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(547, 511);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "代码查看";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "tab2.gif");
            this.imageList1.Images.SetKeyName(1, "cs.png");
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel2.Controls.Add(this.btn_Create);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(551, 505);
            this.splitContainer1.SplitterDistance = 198;
            this.splitContainer1.TabIndex = 4;
            // 
            // CodeMakerTran
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 538);
            this.Controls.Add(this.tabControl1);
            this.Name = "CodeMakerTran";
            this.Text = "事务代码生成器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radbtn_Insert;
        private System.Windows.Forms.ListBox listTable;
        private System.Windows.Forms.RadioButton radbtn_Update;
        private System.Windows.Forms.RadioButton radbtn_Delete;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btn_Create;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNameSpace2;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.ComboBox cmbox_Field;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 删除DToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}