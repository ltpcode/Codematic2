namespace Codematic.UserControls
{
    partial class UcLanguage
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbUILan = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBrowe = new System.Windows.Forms.Button();
            this.txtTempPath = new System.Windows.Forms.TextBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbUILan
            // 
            this.cmbUILan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUILan.FormattingEnabled = true;
            this.cmbUILan.Items.AddRange(new object[] {
            "简体中文",
            "English"});
            this.cmbUILan.Location = new System.Drawing.Point(126, 33);
            this.cmbUILan.Margin = new System.Windows.Forms.Padding(4);
            this.cmbUILan.Name = "cmbUILan";
            this.cmbUILan.Size = new System.Drawing.Size(191, 23);
            this.cmbUILan.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择语言:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "设置将在下次启动后生效";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbUILan);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(15, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 121);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "语言";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnBrowe);
            this.groupBox2.Controls.Add(this.txtTempPath);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Location = new System.Drawing.Point(15, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(472, 87);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "模板文件目录";
            // 
            // btnBrowe
            // 
            this.btnBrowe.Location = new System.Drawing.Point(294, 52);
            this.btnBrowe.Name = "btnBrowe";
            this.btnBrowe.Size = new System.Drawing.Size(61, 23);
            this.btnBrowe.TabIndex = 2;
            this.btnBrowe.Text = "浏览";
            this.btnBrowe.UseVisualStyleBackColor = true;
            this.btnBrowe.Click += new System.EventHandler(this.btnBrowe_Click);
            // 
            // txtTempPath
            // 
            this.txtTempPath.Location = new System.Drawing.Point(107, 51);
            this.txtTempPath.Name = "txtTempPath";
            this.txtTempPath.Size = new System.Drawing.Size(180, 25);
            this.txtTempPath.TabIndex = 1;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(27, 54);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(73, 19);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.Text = "自定义";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(27, 25);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(88, 19);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "默认目录";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // UcLanguage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UcLanguage";
            this.Size = new System.Drawing.Size(500, 380);
            this.Load += new System.EventHandler(this.UcLanguage_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbUILan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrowe;
        public System.Windows.Forms.TextBox txtTempPath;
        public System.Windows.Forms.RadioButton radioButton2;
        public System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}
