namespace Codematic
{
    partial class DbToWeb
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelNum = new System.Windows.Forms.Label();
            this.btn_Cancle = new WiB.Pinkie.Controls.ButtonXP();
            this.btn_Addlist = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbDB = new System.Windows.Forms.ComboBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Add = new System.Windows.Forms.Button();
            this.btn_Del = new System.Windows.Forms.Button();
            this.btn_Creat = new WiB.Pinkie.Controls.ButtonXP();
            this.btn_Dellist = new System.Windows.Forms.Button();
            this.listTable2 = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listTable1 = new System.Windows.Forms.ListBox();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.comboBox1);
            this.groupBox3.Controls.Add(this.radioButton2);
            this.groupBox3.Location = new System.Drawing.Point(8, 290);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(464, 69);
            this.groupBox3.TabIndex = 51;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "格式";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(142, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 46;
            this.label5.Text = "选择风格：";
            this.label5.Visible = false;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "风格一",
            "风格二"});
            this.comboBox1.Location = new System.Drawing.Point(208, 29);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 45;
            this.comboBox1.Visible = false;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(53, 29);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(71, 16);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "网页格式";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Codematic.Properties.Resources.Control;
            this.pictureBox1.Location = new System.Drawing.Point(213, 148);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 28);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 53;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 302);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 12);
            this.label2.TabIndex = 50;
            // 
            // labelNum
            // 
            this.labelNum.Location = new System.Drawing.Point(16, 180);
            this.labelNum.Name = "labelNum";
            this.labelNum.Size = new System.Drawing.Size(46, 22);
            this.labelNum.TabIndex = 9;
            this.labelNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Cancle
            // 
            this.btn_Cancle._Image = null;
            this.btn_Cancle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btn_Cancle.DefaultScheme = false;
            this.btn_Cancle.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Cancle.Image = null;
            this.btn_Cancle.Location = new System.Drawing.Point(350, 371);
            this.btn_Cancle.Name = "btn_Cancle";
            this.btn_Cancle.Scheme = WiB.Pinkie.Controls.ButtonXP.Schemes.Blue;
            this.btn_Cancle.Size = new System.Drawing.Size(75, 26);
            this.btn_Cancle.TabIndex = 49;
            this.btn_Cancle.Text = "取  消";
            this.btn_Cancle.Click += new System.EventHandler(this.btn_Cancle_Click);
            // 
            // btn_Addlist
            // 
            this.btn_Addlist.Enabled = false;
            this.btn_Addlist.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Addlist.Location = new System.Drawing.Point(208, 29);
            this.btn_Addlist.Name = "btn_Addlist";
            this.btn_Addlist.Size = new System.Drawing.Size(40, 23);
            this.btn_Addlist.TabIndex = 7;
            this.btn_Addlist.Text = ">>";
            this.btn_Addlist.Click += new System.EventHandler(this.btn_Addlist_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(64, 182);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(376, 20);
            this.progressBar1.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbDB);
            this.groupBox1.Controls.Add(this.lblServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(8, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 64);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择数据库";
            // 
            // cmbDB
            // 
            this.cmbDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDB.Location = new System.Drawing.Point(296, 24);
            this.cmbDB.Name = "cmbDB";
            this.cmbDB.Size = new System.Drawing.Size(152, 20);
            this.cmbDB.TabIndex = 2;
            this.cmbDB.SelectedIndexChanged += new System.EventHandler(this.cmbDB_SelectedIndexChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(104, 26);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(41, 12);
            this.lblServer.TabIndex = 1;
            this.lblServer.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前服务器：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "选择数据库：";
            // 
            // btn_Add
            // 
            this.btn_Add.Enabled = false;
            this.btn_Add.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Add.Location = new System.Drawing.Point(208, 60);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(40, 23);
            this.btn_Add.TabIndex = 8;
            this.btn_Add.Text = ">";
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_Del
            // 
            this.btn_Del.Enabled = false;
            this.btn_Del.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Del.Location = new System.Drawing.Point(208, 91);
            this.btn_Del.Name = "btn_Del";
            this.btn_Del.Size = new System.Drawing.Size(40, 23);
            this.btn_Del.TabIndex = 5;
            this.btn_Del.Text = "<";
            this.btn_Del.Click += new System.EventHandler(this.btn_Del_Click);
            // 
            // btn_Creat
            // 
            this.btn_Creat._Image = null;
            this.btn_Creat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btn_Creat.DefaultScheme = false;
            this.btn_Creat.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Creat.Image = null;
            this.btn_Creat.Location = new System.Drawing.Point(245, 371);
            this.btn_Creat.Name = "btn_Creat";
            this.btn_Creat.Scheme = WiB.Pinkie.Controls.ButtonXP.Schemes.Blue;
            this.btn_Creat.Size = new System.Drawing.Size(75, 26);
            this.btn_Creat.TabIndex = 48;
            this.btn_Creat.Text = "生  成";
            this.btn_Creat.Click += new System.EventHandler(this.btn_Creat_Click);
            // 
            // btn_Dellist
            // 
            this.btn_Dellist.Enabled = false;
            this.btn_Dellist.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Dellist.Location = new System.Drawing.Point(208, 122);
            this.btn_Dellist.Name = "btn_Dellist";
            this.btn_Dellist.Size = new System.Drawing.Size(40, 23);
            this.btn_Dellist.TabIndex = 6;
            this.btn_Dellist.Text = "<<";
            this.btn_Dellist.Click += new System.EventHandler(this.btn_Dellist_Click);
            // 
            // listTable2
            // 
            this.listTable2.ItemHeight = 12;
            this.listTable2.Location = new System.Drawing.Point(288, 24);
            this.listTable2.Name = "listTable2";
            this.listTable2.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listTable2.Size = new System.Drawing.Size(152, 148);
            this.listTable2.TabIndex = 1;
            this.listTable2.DoubleClick += new System.EventHandler(this.listTable2_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.progressBar1);
            this.groupBox2.Controls.Add(this.labelNum);
            this.groupBox2.Controls.Add(this.btn_Addlist);
            this.groupBox2.Controls.Add(this.btn_Add);
            this.groupBox2.Controls.Add(this.btn_Del);
            this.groupBox2.Controls.Add(this.btn_Dellist);
            this.groupBox2.Controls.Add(this.listTable2);
            this.groupBox2.Controls.Add(this.listTable1);
            this.groupBox2.Location = new System.Drawing.Point(8, 78);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(464, 208);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择表";
            // 
            // listTable1
            // 
            this.listTable1.ItemHeight = 12;
            this.listTable1.Location = new System.Drawing.Point(16, 24);
            this.listTable1.Name = "listTable1";
            this.listTable1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listTable1.Size = new System.Drawing.Size(152, 148);
            this.listTable1.TabIndex = 0;
            this.listTable1.Click += new System.EventHandler(this.listTable1_Click);
            this.listTable1.DoubleClick += new System.EventHandler(this.listTable1_DoubleClick);
            // 
            // DbToWeb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 406);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Cancle);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_Creat);
            this.Controls.Add(this.groupBox2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DbToWeb";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成数据库文档";
            this.Load += new System.EventHandler(this.DbToWeb_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        //private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelNum;
        private WiB.Pinkie.Controls.ButtonXP btn_Cancle;
        private System.Windows.Forms.Button btn_Addlist;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbDB;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Del;
        private WiB.Pinkie.Controls.ButtonXP btn_Creat;
        private System.Windows.Forms.Button btn_Dellist;
        private System.Windows.Forms.ListBox listTable2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listTable1;
    }
}