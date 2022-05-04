namespace Codematic.UserControls
{
    partial class UcCodeSet
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
            this.cmboxServers = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNamepace = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.radbtn_Frame_One = new System.Windows.Forms.RadioButton();
            this.radbtn_Frame_F3 = new System.Windows.Forms.RadioButton();
            this.radbtn_Frame_S3 = new System.Windows.Forms.RadioButton();
            this.txtProcPrefix = new System.Windows.Forms.TextBox();
            this.txtDbHelperName = new System.Windows.Forms.TextBox();
            this.txtNamepace = new System.Windows.Forms.TextBox();
            this.lblDbHelperName = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radbtn_firstUpper = new System.Windows.Forms.RadioButton();
            this.radbtn_Lower = new System.Windows.Forms.RadioButton();
            this.radbtn_Upper = new System.Windows.Forms.RadioButton();
            this.radbtn_Same = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDAL_Prefix = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblTabDAL = new System.Windows.Forms.Label();
            this.txtDAL_Suffix = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTabModel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtModel_Suffix = new System.Windows.Forms.TextBox();
            this.txtModel_Prefix = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBLL_Prefix = new System.Windows.Forms.TextBox();
            this.lblTabBLL = new System.Windows.Forms.Label();
            this.txtBLL_Suffix = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtOldStr = new System.Windows.Forms.TextBox();
            this.txtNewStr = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmboxServers
            // 
            this.cmboxServers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboxServers.FormattingEnabled = true;
            this.cmboxServers.Location = new System.Drawing.Point(149, 9);
            this.cmboxServers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmboxServers.Name = "cmboxServers";
            this.cmboxServers.Size = new System.Drawing.Size(321, 23);
            this.cmboxServers.TabIndex = 55;
            this.cmboxServers.SelectedIndexChanged += new System.EventHandler(this.cmboxServers_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(33, 12);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 15);
            this.label3.TabIndex = 54;
            this.label3.Text = "选择服务器：";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(4, 38);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(500, 344);
            this.tabControl1.TabIndex = 56;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.txtProjectName);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.lblNamepace);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.txtProcPrefix);
            this.tabPage1.Controls.Add(this.txtDbHelperName);
            this.tabPage1.Controls.Add(this.txtNamepace);
            this.tabPage1.Controls.Add(this.lblDbHelperName);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(492, 315);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本设置";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Location = new System.Drawing.Point(4, 191);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox5.Size = new System.Drawing.Size(480, 120);
            this.groupBox5.TabIndex = 61;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "选择默认代码模板";
            // 
            // txtProjectName
            // 
            this.txtProjectName.AcceptsReturn = true;
            this.txtProjectName.Location = new System.Drawing.Point(155, 71);
            this.txtProjectName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(189, 25);
            this.txtProjectName.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 55;
            this.label2.Text = "存储过程前缀：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 76);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 53;
            this.label1.Text = "项目名称：";
            // 
            // lblNamepace
            // 
            this.lblNamepace.AutoSize = true;
            this.lblNamepace.Location = new System.Drawing.Point(23, 16);
            this.lblNamepace.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNamepace.Name = "lblNamepace";
            this.lblNamepace.Size = new System.Drawing.Size(112, 15);
            this.lblNamepace.TabIndex = 56;
            this.lblNamepace.Text = "顶级命名空间：";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.radbtn_Frame_One);
            this.groupBox6.Controls.Add(this.radbtn_Frame_F3);
            this.groupBox6.Controls.Add(this.radbtn_Frame_S3);
            this.groupBox6.Location = new System.Drawing.Point(4, 134);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox6.Size = new System.Drawing.Size(480, 55);
            this.groupBox6.TabIndex = 62;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "默认架构类型";
            // 
            // radbtn_Frame_One
            // 
            this.radbtn_Frame_One.AutoSize = true;
            this.radbtn_Frame_One.Location = new System.Drawing.Point(28, 22);
            this.radbtn_Frame_One.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radbtn_Frame_One.Name = "radbtn_Frame_One";
            this.radbtn_Frame_One.Size = new System.Drawing.Size(117, 24);
            this.radbtn_Frame_One.TabIndex = 0;
            this.radbtn_Frame_One.Text = "单类结构";
            this.radbtn_Frame_One.UseVisualStyleBackColor = true;
            // 
            // radbtn_Frame_F3
            // 
            this.radbtn_Frame_F3.AutoSize = true;
            this.radbtn_Frame_F3.Checked = true;
            this.radbtn_Frame_F3.Location = new System.Drawing.Point(271, 22);
            this.radbtn_Frame_F3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radbtn_Frame_F3.Name = "radbtn_Frame_F3";
            this.radbtn_Frame_F3.Size = new System.Drawing.Size(157, 24);
            this.radbtn_Frame_F3.TabIndex = 0;
            this.radbtn_Frame_F3.TabStop = true;
            this.radbtn_Frame_F3.Text = "工厂模式三层";
            this.radbtn_Frame_F3.UseVisualStyleBackColor = true;
            // 
            // radbtn_Frame_S3
            // 
            this.radbtn_Frame_S3.AutoSize = true;
            this.radbtn_Frame_S3.Location = new System.Drawing.Point(149, 22);
            this.radbtn_Frame_S3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radbtn_Frame_S3.Name = "radbtn_Frame_S3";
            this.radbtn_Frame_S3.Size = new System.Drawing.Size(117, 24);
            this.radbtn_Frame_S3.TabIndex = 0;
            this.radbtn_Frame_S3.Text = "简单三层";
            this.radbtn_Frame_S3.UseVisualStyleBackColor = true;
            // 
            // txtProcPrefix
            // 
            this.txtProcPrefix.Location = new System.Drawing.Point(155, 101);
            this.txtProcPrefix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtProcPrefix.Name = "txtProcPrefix";
            this.txtProcPrefix.Size = new System.Drawing.Size(189, 25);
            this.txtProcPrefix.TabIndex = 59;
            // 
            // txtDbHelperName
            // 
            this.txtDbHelperName.Location = new System.Drawing.Point(155, 41);
            this.txtDbHelperName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDbHelperName.Name = "txtDbHelperName";
            this.txtDbHelperName.Size = new System.Drawing.Size(189, 25);
            this.txtDbHelperName.TabIndex = 57;
            // 
            // txtNamepace
            // 
            this.txtNamepace.Location = new System.Drawing.Point(155, 11);
            this.txtNamepace.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNamepace.Name = "txtNamepace";
            this.txtNamepace.Size = new System.Drawing.Size(189, 25);
            this.txtNamepace.TabIndex = 58;
            // 
            // lblDbHelperName
            // 
            this.lblDbHelperName.AutoSize = true;
            this.lblDbHelperName.Location = new System.Drawing.Point(23, 46);
            this.lblDbHelperName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDbHelperName.Name = "lblDbHelperName";
            this.lblDbHelperName.Size = new System.Drawing.Size(112, 15);
            this.lblDbHelperName.TabIndex = 54;
            this.lblDbHelperName.Text = "数据访问基类：";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(492, 315);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "命名规则";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.txtNewStr);
            this.groupBox4.Controls.Add(this.txtOldStr);
            this.groupBox4.Controls.Add(this.radbtn_firstUpper);
            this.groupBox4.Controls.Add(this.radbtn_Lower);
            this.groupBox4.Controls.Add(this.radbtn_Upper);
            this.groupBox4.Controls.Add(this.radbtn_Same);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Location = new System.Drawing.Point(7, 31);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(472, 110);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "表名规则";
            // 
            // radbtn_firstUpper
            // 
            this.radbtn_firstUpper.AutoSize = true;
            this.radbtn_firstUpper.Location = new System.Drawing.Point(352, 31);
            this.radbtn_firstUpper.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radbtn_firstUpper.Name = "radbtn_firstUpper";
            this.radbtn_firstUpper.Size = new System.Drawing.Size(103, 19);
            this.radbtn_firstUpper.TabIndex = 0;
            this.radbtn_firstUpper.Text = "首字母大写";
            this.radbtn_firstUpper.UseVisualStyleBackColor = true;
            // 
            // radbtn_Lower
            // 
            this.radbtn_Lower.AutoSize = true;
            this.radbtn_Lower.Location = new System.Drawing.Point(236, 31);
            this.radbtn_Lower.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radbtn_Lower.Name = "radbtn_Lower";
            this.radbtn_Lower.Size = new System.Drawing.Size(103, 19);
            this.radbtn_Lower.TabIndex = 0;
            this.radbtn_Lower.Text = "表名全小写";
            this.radbtn_Lower.UseVisualStyleBackColor = true;
            // 
            // radbtn_Upper
            // 
            this.radbtn_Upper.AutoSize = true;
            this.radbtn_Upper.Location = new System.Drawing.Point(120, 31);
            this.radbtn_Upper.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radbtn_Upper.Name = "radbtn_Upper";
            this.radbtn_Upper.Size = new System.Drawing.Size(103, 19);
            this.radbtn_Upper.TabIndex = 0;
            this.radbtn_Upper.Text = "表名全大写";
            this.radbtn_Upper.UseVisualStyleBackColor = true;
            // 
            // radbtn_Same
            // 
            this.radbtn_Same.AutoSize = true;
            this.radbtn_Same.Checked = true;
            this.radbtn_Same.Location = new System.Drawing.Point(20, 31);
            this.radbtn_Same.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radbtn_Same.Name = "radbtn_Same";
            this.radbtn_Same.Size = new System.Drawing.Size(88, 19);
            this.radbtn_Same.TabIndex = 0;
            this.radbtn_Same.TabStop = true;
            this.radbtn_Same.Text = "表名不变";
            this.radbtn_Same.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(310, 95);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 15);
            this.label9.TabIndex = 1;
            this.label9.Text = "+";
            // 
            // txtDAL_Prefix
            // 
            this.txtDAL_Prefix.Location = new System.Drawing.Point(114, 90);
            this.txtDAL_Prefix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDAL_Prefix.Name = "txtDAL_Prefix";
            this.txtDAL_Prefix.Size = new System.Drawing.Size(85, 25);
            this.txtDAL_Prefix.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(220, 95);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 15);
            this.label7.TabIndex = 1;
            this.label7.Text = "+";
            // 
            // lblTabDAL
            // 
            this.lblTabDAL.AutoSize = true;
            this.lblTabDAL.Location = new System.Drawing.Point(253, 95);
            this.lblTabDAL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTabDAL.Name = "lblTabDAL";
            this.lblTabDAL.Size = new System.Drawing.Size(37, 15);
            this.lblTabDAL.TabIndex = 1;
            this.lblTabDAL.Text = "表名";
            // 
            // txtDAL_Suffix
            // 
            this.txtDAL_Suffix.Location = new System.Drawing.Point(344, 90);
            this.txtDAL_Suffix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDAL_Suffix.Name = "txtDAL_Suffix";
            this.txtDAL_Suffix.Size = new System.Drawing.Size(85, 25);
            this.txtDAL_Suffix.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtDAL_Prefix);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblTabDAL);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtDAL_Suffix);
            this.groupBox1.Controls.Add(this.txtBLL_Prefix);
            this.groupBox1.Controls.Add(this.lblTabModel);
            this.groupBox1.Controls.Add(this.lblTabBLL);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtBLL_Suffix);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtModel_Suffix);
            this.groupBox1.Controls.Add(this.txtModel_Prefix);
            this.groupBox1.Location = new System.Drawing.Point(7, 149);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(472, 133);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "类命名规则";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(310, 31);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "+";
            // 
            // lblTabModel
            // 
            this.lblTabModel.AutoSize = true;
            this.lblTabModel.Location = new System.Drawing.Point(253, 31);
            this.lblTabModel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTabModel.Name = "lblTabModel";
            this.lblTabModel.Size = new System.Drawing.Size(37, 15);
            this.lblTabModel.TabIndex = 1;
            this.lblTabModel.Text = "表名";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(220, 31);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 15);
            this.label8.TabIndex = 1;
            this.label8.Text = "+";
            // 
            // txtModel_Suffix
            // 
            this.txtModel_Suffix.Location = new System.Drawing.Point(344, 26);
            this.txtModel_Suffix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtModel_Suffix.Name = "txtModel_Suffix";
            this.txtModel_Suffix.Size = new System.Drawing.Size(85, 25);
            this.txtModel_Suffix.TabIndex = 0;
            // 
            // txtModel_Prefix
            // 
            this.txtModel_Prefix.Location = new System.Drawing.Point(114, 26);
            this.txtModel_Prefix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtModel_Prefix.Name = "txtModel_Prefix";
            this.txtModel_Prefix.Size = new System.Drawing.Size(85, 25);
            this.txtModel_Prefix.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 9);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(427, 15);
            this.label10.TabIndex = 8;
            this.label10.Text = "注：默认情况下，将以表名作为类名生成，可通过下面设置更改";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(310, 63);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "+";
            // 
            // txtBLL_Prefix
            // 
            this.txtBLL_Prefix.Location = new System.Drawing.Point(114, 58);
            this.txtBLL_Prefix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBLL_Prefix.Name = "txtBLL_Prefix";
            this.txtBLL_Prefix.Size = new System.Drawing.Size(85, 25);
            this.txtBLL_Prefix.TabIndex = 0;
            // 
            // lblTabBLL
            // 
            this.lblTabBLL.AutoSize = true;
            this.lblTabBLL.Location = new System.Drawing.Point(253, 63);
            this.lblTabBLL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTabBLL.Name = "lblTabBLL";
            this.lblTabBLL.Size = new System.Drawing.Size(37, 15);
            this.lblTabBLL.TabIndex = 1;
            this.lblTabBLL.Text = "表名";
            // 
            // txtBLL_Suffix
            // 
            this.txtBLL_Suffix.Location = new System.Drawing.Point(344, 58);
            this.txtBLL_Suffix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBLL_Suffix.Name = "txtBLL_Suffix";
            this.txtBLL_Suffix.Size = new System.Drawing.Size(85, 25);
            this.txtBLL_Suffix.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(220, 63);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "+";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(25, 63);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 15);
            this.label11.TabIndex = 2;
            this.label11.Text = "BLL层类";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(25, 31);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 15);
            this.label12.TabIndex = 2;
            this.label12.Text = "Model类";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(25, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(61, 15);
            this.label13.TabIndex = 2;
            this.label13.Text = "DAL层类";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(18, 71);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(127, 15);
            this.label14.TabIndex = 1;
            this.label14.Text = "替换表名中字符：";
            // 
            // txtOldStr
            // 
            this.txtOldStr.Location = new System.Drawing.Point(144, 66);
            this.txtOldStr.Name = "txtOldStr";
            this.txtOldStr.Size = new System.Drawing.Size(100, 25);
            this.txtOldStr.TabIndex = 2;
            // 
            // txtNewStr
            // 
            this.txtNewStr.Location = new System.Drawing.Point(272, 66);
            this.txtNewStr.Name = "txtNewStr";
            this.txtNewStr.Size = new System.Drawing.Size(100, 25);
            this.txtNewStr.TabIndex = 2;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(247, 71);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(23, 15);
            this.label15.TabIndex = 1;
            this.label15.Text = "->";
            // 
            // UcCodeSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cmboxServers);
            this.Controls.Add(this.label3);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UcCodeSet";
            this.Size = new System.Drawing.Size(508, 385);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmboxServers;
        public System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox5;
        public System.Windows.Forms.TextBox txtProjectName;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label lblNamepace;
        private System.Windows.Forms.GroupBox groupBox6;
        public System.Windows.Forms.RadioButton radbtn_Frame_One;
        public System.Windows.Forms.RadioButton radbtn_Frame_F3;
        public System.Windows.Forms.RadioButton radbtn_Frame_S3;
        public System.Windows.Forms.TextBox txtProcPrefix;
        public System.Windows.Forms.TextBox txtDbHelperName;
        public System.Windows.Forms.TextBox txtNamepace;
        public System.Windows.Forms.Label lblDbHelperName;
        private System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.RadioButton radbtn_firstUpper;
        public System.Windows.Forms.RadioButton radbtn_Lower;
        public System.Windows.Forms.RadioButton radbtn_Upper;
        public System.Windows.Forms.RadioButton radbtn_Same;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox txtDAL_Prefix;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblTabDAL;
        public System.Windows.Forms.TextBox txtDAL_Suffix;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTabModel;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox txtModel_Suffix;
        public System.Windows.Forms.TextBox txtModel_Prefix;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox txtBLL_Prefix;
        private System.Windows.Forms.Label lblTabBLL;
        public System.Windows.Forms.TextBox txtBLL_Suffix;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.TextBox txtOldStr;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.TextBox txtNewStr;
    }
}
