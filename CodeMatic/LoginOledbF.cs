using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;
using System.IO;
namespace Codematic
{
    public partial class LoginOledbF : Form
    {
        Maticsoft.CmConfig.DbSettings dbobj = new Maticsoft.CmConfig.DbSettings();

        public LoginOledbF()
        {
            InitializeComponent();
           
        }
        private void LoginOledbF_Load(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(this.txtUser, "请保证该用户具有每个数据库的访问权！");
        }

        private void btn_SelDb_Click(object sender, EventArgs e)
        {            
            OpenFileDialog sqlfiledlg = new OpenFileDialog();           
            sqlfiledlg.Title = "选择数据库文件";
            sqlfiledlg.Filter = "Access files (*.mdb;*.accdb)|*.mdb;*.accdb|所有文件 (*.*)|*.*";
             DialogResult result = sqlfiledlg.ShowDialog(this);
             if (DialogResult.OK == result)
            {
                this.txtServer.Text = sqlfiledlg.FileName;
                //GetConstr();
                //this.txtConstr.Text=@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+txtServer.Text+";Persist Security Info=False";	
            }
        }

        

        private void radBtn_DB_Click(object sender, EventArgs e)
        {
            this.radBtn_Constr.Checked = false;
            this.checkBox1.Enabled = true;
            this.txtServer.Enabled = true;
            this.txtPass.Enabled = true;
            this.txtUser.Enabled = true;
            this.txtConstr.Enabled = false;		
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radBtn_Constr_Click(object sender, EventArgs e)
        {
            this.radBtn_DB.Checked = false;
            this.checkBox1.Enabled = false;
            this.txtServer.Enabled = false;
            this.txtPass.Enabled = false;
            this.txtUser.Enabled = false;
            this.txtConstr.Enabled = true;
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            try
            {
                string server = this.txtServer.Text.Trim();
                string user = this.txtUser.Text.Trim();
                string pass = this.txtPass.Text.Trim();

                if (this.radBtn_DB.Checked)
                {
                    GetConstr();
                    if (server == "")
                    {
                        MessageBox.Show(this, "数据库不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                if (this.radBtn_Constr.Checked)
                {
                    if (txtConstr.Text == "")
                    {
                        MessageBox.Show(this, "数据库不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                string constr = this.txtConstr.Text;

                //测试连接
                OleDbConnection myCn = new OleDbConnection(constr);
                try
                {
                    this.Text = "正在连接数据库，请稍候...";
                    myCn.Open();
                }
                catch
                {
                    this.Text = "连接数据库失败！";
                    MessageBox.Show(this, "连接数据库失败！请检查数据库地址或用户名密码是否正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                myCn.Close();
                this.Text = "连接数据库成功！";

                if (dbobj == null)
                    dbobj = new Maticsoft.CmConfig.DbSettings();

                //将当前配置写入配置文件
                dbobj.DbType = "OleDb";
                dbobj.Server = server;
                dbobj.ConnectStr = constr;
                dbobj.DbName = "";
                dbobj.DbHelperName = "DbHelperOleDb";
                int result = Maticsoft.CmConfig.DbConfig.AddSettings(dbobj);
                switch (result)
                {
                    case 0:
                        MessageBox.Show(this, "添加服务器配置失败，请检查是否有写入权限或文件是否存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    case 1:
                        break;
                    case 2:
                        {
                            DialogResult dr = MessageBox.Show(this, "该服务器信息已经存在！你确认是否覆盖当前数据库配置？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (dr == DialogResult.Yes)
                            {
                                Maticsoft.CmConfig.DbConfig.DelSetting(dbobj.DbType, dbobj.Server, dbobj.DbName);
                                result = Maticsoft.CmConfig.DbConfig.AddSettings(dbobj);
                                if (result != 1)
                                {
                                    MessageBox.Show(this, "建议卸载当前版本，并删除安装目录后重新安装最新版本！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        break;
                }

                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogInfo.WriteLog(ex);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.txtPass.Text = "";
                this.txtPass.Enabled = false;
            }
            else
            {
                this.txtPass.Enabled = true;
            }
        }


        #region 构建连接字符串
        private void GetConstr()
        {
            if (txtServer.Text.Trim().Length < 2)
            {
                return;
            }
            FileInfo file = new FileInfo(txtServer.Text);
            string ext = file.Extension;
            switch (ext.ToLower().Trim())
            {
                case ".mdb":
                    txtConstr.Text = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + txtServer.Text + ";Persist Security Info=False";
                    break;
                case ".accdb":
                    txtConstr.Text = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtServer.Text + ";Persist Security Info=False";
                    break;
                default:
                    txtConstr.Text = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + txtServer.Text + ";Persist Security Info=False";
                    break;
            }
        }
        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            GetConstr();	
        }
        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            GetConstr();	
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            GetConstr();
        }
        #endregion





    }
}
