namespace Codematic
{
    partial class LoginSQLite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginSQLite));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.radBtn_DB = new System.Windows.Forms.RadioButton();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_SelDb = new WiB.Pinkie.Controls.ButtonXP();
            this.btn_Cancel = new WiB.Pinkie.Controls.ButtonXP();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtConstr = new System.Windows.Forms.TextBox();
            this.radBtn_Constr = new System.Windows.Forms.RadioButton();
            this.btn_Ok = new WiB.Pinkie.Controls.ButtonXP();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtServer);
            this.groupBox2.Controls.Add(this.radBtn_DB);
            this.groupBox2.Controls.Add(this.txtPass);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btn_SelDb);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(16, 10);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(491, 115);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择数据库";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(139, 30);
            this.txtServer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(287, 25);
            this.txtServer.TabIndex = 1;
            this.txtServer.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // radBtn_DB
            // 
            this.radBtn_DB.Checked = true;
            this.radBtn_DB.Location = new System.Drawing.Point(21, 28);
            this.radBtn_DB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radBtn_DB.Name = "radBtn_DB";
            this.radBtn_DB.Size = new System.Drawing.Size(139, 30);
            this.radBtn_DB.TabIndex = 20;
            this.radBtn_DB.TabStop = true;
            this.radBtn_DB.Text = "数据库文件：";
            this.radBtn_DB.Click += new System.EventHandler(this.radBtn_DB_Click);
            // 
            // txtPass
            // 
            this.txtPass.Enabled = false;
            this.txtPass.Location = new System.Drawing.Point(139, 65);
            this.txtPass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(159, 25);
            this.txtPass.TabIndex = 3;
            this.txtPass.TextChanged += new System.EventHandler(this.txtPass_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(83, 68);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "密码：";
            // 
            // btn_SelDb
            // 
            this.btn_SelDb._Image = null;
            this.btn_SelDb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btn_SelDb.DefaultScheme = false;
            this.btn_SelDb.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_SelDb.Image = null;
            this.btn_SelDb.Location = new System.Drawing.Point(427, 28);
            this.btn_SelDb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_SelDb.Name = "btn_SelDb";
            this.btn_SelDb.Scheme = WiB.Pinkie.Controls.ButtonXP.Schemes.Blue;
            this.btn_SelDb.Size = new System.Drawing.Size(53, 30);
            this.btn_SelDb.TabIndex = 19;
            this.btn_SelDb.Text = "...";
            this.btn_SelDb.Visible = false;
            this.btn_SelDb.Click += new System.EventHandler(this.btn_SelDb_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel._Image = null;
            this.btn_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btn_Cancel.DefaultScheme = false;
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Image = null;
            this.btn_Cancel.Location = new System.Drawing.Point(276, 201);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Scheme = WiB.Pinkie.Controls.ButtonXP.Schemes.Blue;
            this.btn_Cancel.Size = new System.Drawing.Size(100, 30);
            this.btn_Cancel.TabIndex = 25;
            this.btn_Cancel.Text = "取消(&C):";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtConstr);
            this.groupBox3.Controls.Add(this.radBtn_Constr);
            this.groupBox3.Location = new System.Drawing.Point(16, 132);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(491, 50);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
            // 
            // txtConstr
            // 
            this.txtConstr.Enabled = false;
            this.txtConstr.Location = new System.Drawing.Point(139, 15);
            this.txtConstr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtConstr.Name = "txtConstr";
            this.txtConstr.Size = new System.Drawing.Size(340, 25);
            this.txtConstr.TabIndex = 0;
            // 
            // radBtn_Constr
            // 
            this.radBtn_Constr.Location = new System.Drawing.Point(16, 12);
            this.radBtn_Constr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radBtn_Constr.Name = "radBtn_Constr";
            this.radBtn_Constr.Size = new System.Drawing.Size(139, 30);
            this.radBtn_Constr.TabIndex = 1;
            this.radBtn_Constr.Text = "连接字符串：";
            this.radBtn_Constr.Click += new System.EventHandler(this.radBtn_Constr_Click);
            // 
            // btn_Ok
            // 
            this.btn_Ok._Image = null;
            this.btn_Ok.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btn_Ok.DefaultScheme = false;
            this.btn_Ok.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btn_Ok.Image = null;
            this.btn_Ok.Location = new System.Drawing.Point(116, 201);
            this.btn_Ok.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Scheme = WiB.Pinkie.Controls.ButtonXP.Schemes.Blue;
            this.btn_Ok.Size = new System.Drawing.Size(100, 30);
            this.btn_Ok.TabIndex = 24;
            this.btn_Ok.Text = "确定(&O):";
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(292, 69);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "（可选）";
            // 
            // LoginSQLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 251);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btn_Ok);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginSQLite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择SQLite数据库";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.RadioButton radBtn_DB;
        private WiB.Pinkie.Controls.ButtonXP btn_SelDb;
        private WiB.Pinkie.Controls.ButtonXP btn_Cancel;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.TextBox txtConstr;
        private System.Windows.Forms.RadioButton radBtn_Constr;
        private WiB.Pinkie.Controls.ButtonXP btn_Ok;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.TextBox txtPass;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label1;

    }
}