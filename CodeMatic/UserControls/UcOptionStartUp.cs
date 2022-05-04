
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Codematic.UserControls
{
	/// <summary>
	/// .
	/// </summary>
	public class UcOptionStartUp : System.Windows.Forms.UserControl
    {
        public System.Windows.Forms.CheckBox chb_isTimespan;
        private System.Windows.Forms.Label label4;
        public ComboBox cmb_StartUpItem;
        private Label label1;
        public TextBox txtStartUpPage;
        public NumericUpDown num_Time;
        private Label label2;
        Maticsoft.CmConfig.AppSettings settings;
        private GroupBox groupBox1;
        public TextBox txtTempPath;
        public RadioButton radioButton2;
        public RadioButton radioButton1;
        private Button btnBrowe;
        private FolderBrowserDialog folderBrowserDialog1;

        #region
        /// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public UcOptionStartUp(Maticsoft.CmConfig.AppSettings setting)
		{			
			InitializeComponent();
            settings = setting;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
        }
        #endregion

        #region Component Designer generated code
        /// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.chb_isTimespan = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_StartUpItem = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStartUpPage = new System.Windows.Forms.TextBox();
            this.num_Time = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.txtTempPath = new System.Windows.Forms.TextBox();
            this.btnBrowe = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.num_Time)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chb_isTimespan
            // 
            this.chb_isTimespan.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chb_isTimespan.Location = new System.Drawing.Point(10, 94);
            this.chb_isTimespan.Name = "chb_isTimespan";
            this.chb_isTimespan.Size = new System.Drawing.Size(264, 24);
            this.chb_isTimespan.TabIndex = 10;
            this.chb_isTimespan.Text = "下载内容的时间间隔(&D)：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "启动时(&P):";
            // 
            // cmb_StartUpItem
            // 
            this.cmb_StartUpItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_StartUpItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_StartUpItem.FormattingEnabled = true;
            this.cmb_StartUpItem.Items.AddRange(new object[] {
            "显示起始页",
            "显示空环境",
            "打开主页"});
            this.cmb_StartUpItem.Location = new System.Drawing.Point(10, 23);
            this.cmb_StartUpItem.Name = "cmb_StartUpItem";
            this.cmb_StartUpItem.Size = new System.Drawing.Size(375, 23);
            this.cmb_StartUpItem.TabIndex = 13;
            this.cmb_StartUpItem.SelectedIndexChanged += new System.EventHandler(this.cmb_StartUpItem_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "启动页新闻频道(RSS)(&S):";
            // 
            // txtStartUpPage
            // 
            this.txtStartUpPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStartUpPage.Location = new System.Drawing.Point(10, 67);
            this.txtStartUpPage.Name = "txtStartUpPage";
            this.txtStartUpPage.Size = new System.Drawing.Size(375, 25);
            this.txtStartUpPage.TabIndex = 14;
            this.txtStartUpPage.Text = "http://ltp.cnblogs.com/Rss.aspx";
            // 
            // num_Time
            // 
            this.num_Time.Location = new System.Drawing.Point(25, 117);
            this.num_Time.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.num_Time.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_Time.Name = "num_Time";
            this.num_Time.Size = new System.Drawing.Size(40, 25);
            this.num_Time.TabIndex = 15;
            this.num_Time.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "分钟(&M)";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnBrowe);
            this.groupBox1.Controls.Add(this.txtTempPath);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(11, 165);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 87);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "模板文件目录";
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
            // txtTempPath
            // 
            this.txtTempPath.Location = new System.Drawing.Point(107, 51);
            this.txtTempPath.Name = "txtTempPath";
            this.txtTempPath.Size = new System.Drawing.Size(180, 25);
            this.txtTempPath.TabIndex = 1;
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
            // UcOptionStartUp
            // 
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.num_Time);
            this.Controls.Add(this.txtStartUpPage);
            this.Controls.Add(this.cmb_StartUpItem);
            this.Controls.Add(this.chb_isTimespan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Name = "UcOptionStartUp";
            this.Size = new System.Drawing.Size(405, 255);
            this.Load += new System.EventHandler(this.UcOptionStartUp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num_Time)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        
		private void UcOptionStartUp_Load(object sender, System.EventArgs e)
		{            
            switch (settings.AppStart)
            {
                case "startuppage"://显示起始页
                    cmb_StartUpItem.SelectedIndex = 0;
                    txtStartUpPage.Enabled = true;
                    break;
                case "blank"://显示空环境
                    cmb_StartUpItem.SelectedIndex = 1;
                    txtStartUpPage.Enabled = false;
                    break;
                case "homepage": //打开主页
                    cmb_StartUpItem.SelectedIndex = 2;
                    txtStartUpPage.Enabled = true;
                    break;
            }
            txtStartUpPage.Text = settings.StartUpPage;
            if (settings.TemplateFolder == "Template" || 
                settings.TemplateFolder == "Template\\TemplateFile" || 
                settings.TemplateFolder.Length == 0)
            {
                radioButton1.Checked = true;       
            }
            else
            {
                radioButton2.Checked = true;
                txtTempPath.Text = settings.TemplateFolder;
            }
                      
		}

        private void cmb_StartUpItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmb_StartUpItem.SelectedIndex)
            {
                case 0:
                    label1.Text = "启动页新闻频道(RSS)(&S):";
                    txtStartUpPage.Text = settings.StartUpPage;
                    txtStartUpPage.Enabled = true;
                    break;
                case 1:
                    txtStartUpPage.Enabled = false;
                    break;
                case 2:
                    label1.Text = "主页地址(&H):";
                    txtStartUpPage.Text = settings.HomePage;
                    txtStartUpPage.Enabled = true;
                    break;
            }
        }

        private void btnBrowe_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtTempPath.Text = folderBrowserDialog1.SelectedPath;                
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                txtTempPath.Enabled = false;
                btnBrowe.Enabled = false;
            }
            else
            {
                txtTempPath.Enabled = true;
                btnBrowe.Enabled = true;
            }
        }
	}
}
