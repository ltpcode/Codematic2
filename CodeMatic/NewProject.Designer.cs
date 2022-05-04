namespace Codematic
{
    partial class NewProject
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProject));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTooltip = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProName = new System.Windows.Forms.TextBox();
            this.txtProPath = new System.Windows.Forms.TextBox();
            this.btn_browser = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.cmboxVS = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 30);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(163, 169);
            this.treeView1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Location = new System.Drawing.Point(181, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(379, 168);
            this.panel1.TabIndex = 1;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(379, 168);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "项目类型(&P)：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(179, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "模板(&T)：";
            // 
            // lblTooltip
            // 
            this.lblTooltip.Location = new System.Drawing.Point(12, 202);
            this.lblTooltip.Name = "lblTooltip";
            this.lblTooltip.ReadOnly = true;
            this.lblTooltip.Size = new System.Drawing.Size(548, 21);
            this.lblTooltip.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 302);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(548, 4);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 232);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "名称(&N)：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 258);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "位置(&L)：";
            // 
            // txtProName
            // 
            this.txtProName.Location = new System.Drawing.Point(99, 228);
            this.txtProName.Name = "txtProName";
            this.txtProName.Size = new System.Drawing.Size(383, 21);
            this.txtProName.TabIndex = 13;
            this.txtProName.Text = "CodematicDemo";
            // 
            // txtProPath
            // 
            this.txtProPath.Location = new System.Drawing.Point(99, 254);
            this.txtProPath.Name = "txtProPath";
            this.txtProPath.Size = new System.Drawing.Size(383, 21);
            this.txtProPath.TabIndex = 14;
            // 
            // btn_browser
            // 
            this.btn_browser.Location = new System.Drawing.Point(488, 253);
            this.btn_browser.Name = "btn_browser";
            this.btn_browser.Size = new System.Drawing.Size(75, 23);
            this.btn_browser.TabIndex = 15;
            this.btn_browser.Text = "浏览(&B)...";
            this.btn_browser.UseVisualStyleBackColor = true;
            this.btn_browser.Click += new System.EventHandler(this.btn_browser_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(485, 314);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(393, 314);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 16;
            this.btn_ok.Text = "下一步 >";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "one.gif");
            this.imageList1.Images.SetKeyName(1, "s3.gif");
            this.imageList1.Images.SetKeyName(2, "f3.gif");
            // 
            // cmboxVS
            // 
            this.cmboxVS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboxVS.FormattingEnabled = true;
            this.cmboxVS.Items.AddRange(new object[] {
            "Visual Studio 2010",
            "Visual Studio 2008"});
            this.cmboxVS.Location = new System.Drawing.Point(99, 280);
            this.cmboxVS.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmboxVS.Name = "cmboxVS";
            this.cmboxVS.Size = new System.Drawing.Size(167, 20);
            this.cmboxVS.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 283);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "开发工具版本：";
            // 
            // NewProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 350);
            this.Controls.Add(this.cmboxVS);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_browser);
            this.Controls.Add(this.txtProPath);
            this.Controls.Add(this.txtProName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTooltip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProject";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新建项目";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.NewProject_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox lblTooltip;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtProName;
        private System.Windows.Forms.TextBox txtProPath;
        private System.Windows.Forms.Button btn_browser;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ComboBox cmboxVS;
        private System.Windows.Forms.Label label5;
    }
}