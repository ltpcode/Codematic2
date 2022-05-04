using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using MySql.Data.MySqlClient;
namespace Codematic
{
    public partial class LoginMySQL : Form
    {
        Maticsoft.CmConfig.DbSettings dbobj = new Maticsoft.CmConfig.DbSettings();
        public string constr;       
        public string dbname = "mysql";

        public LoginMySQL()
        {
            InitializeComponent();
        }

        private void btn_ConTest_Click(object sender, EventArgs e)
        {
            try
            {
                string server = this.comboBoxServer.Text.Trim();
                string user = this.txtUser.Text.Trim();
                string pass = this.txtPass.Text.Trim();
                string port = this.textBox1.Text.Trim();
                if ((user == "") || (server == ""))
                {
                    MessageBox.Show(this, "服务器或用户名不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //constr = String.Format("server={0};uid={1}; Port={2};pwd={3}; database=mysql; pooling=false", server, user, port, pass);
                constr = String.Format("server={0};uid={1}; Port={2};pwd={3}; pooling=false", server, user, port, pass);
                try
                {
                    this.Text = "正在连接服务器，请稍候...";

                    Maticsoft.IDBO.IDbObject dbobj;                    
                    dbobj = Maticsoft.DBFactory.DBOMaker.CreateDbObj("MySQL");

                    dbobj.DbConnectStr = constr;
                    List<string> dblist = dbobj.GetDBList();
                    this.cmbDBlist.Enabled = true;
                    this.cmbDBlist.Items.Clear();
                    this.cmbDBlist.Items.Add("全部库");
                    if (dblist != null)
                    {
                        if (dblist.Count > 0)
                        {
                            foreach (string dbname in dblist)
                            {
                                this.cmbDBlist.Items.Add(dbname);
                            }
                        }
                    }
                    this.cmbDBlist.SelectedIndex = 0;
                    this.Text = "连接服务器成功！";

                }
                catch(System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    this.Text = "连接服务器或获取数据信息失败！";
                    DialogResult drs = MessageBox.Show(this, "连接服务器或获取数据信息失败！\r\n请检查服务器地址或用户名密码是否正确！查看帮助文件以帮助您解决问题？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    if (drs == DialogResult.OK)
                    {
                        try
                        {
                            Process proc = new Process();
                            Process.Start("IExplore.exe", "http://help.maticsoft.com");
                        }
                        catch
                        {
                            MessageBox.Show("请访问：http://www.maticsoft.com", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    return;
                }

            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show(this, ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);                
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            try
            {
                string server = this.comboBoxServer.Text.Trim();
                string user = this.txtUser.Text.Trim();
                string pass = this.txtPass.Text.Trim();
                string port = this.textBox1.Text.Trim();
                if ((user == "") || (server == ""))
                {
                    MessageBox.Show(this, "服务器或用户名不能为空！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (this.cmbDBlist.SelectedIndex > 0)
                {
                    dbname = cmbDBlist.Text;
                    constr = String.Format("server={0};user id={1}; Port={2};password={3}; database={4}; pooling=false", server, user, port, pass, dbname);
                }
                else
                {
                    dbname = "";
                    constr = String.Format("server={0};user id={1}; Port={2};password={3}; pooling=false", server, user, port, pass);
                }                
                //constr = String.Format("server={0};user id={1}; Port={2};password={3}; database={4}; pooling=false", server, user, port,pass, dbname);
                //测试连接
                MySqlConnection myCn = new MySqlConnection(constr);
                try
                {
                    this.Text = "正在连接服务器，请稍候...";
                    myCn.Open();
                }
                catch(System.Exception ex)
                {
                    LogInfo.WriteLog(ex);
                    this.Text = "连接服务器失败！";
                    MessageBox.Show(this, "连接服务器失败！请检查服务器地址或用户名密码是否正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    myCn.Close();
                }
                this.Text = "连接服务器成功！";
                if (dbobj == null)
                {
                    dbobj = new Maticsoft.CmConfig.DbSettings();
                }                              
                string strtype = "MySQL";
                //将当前配置写入配置文件
                dbobj.DbType = strtype;
                dbobj.Server = server;
                dbobj.ConnectStr = constr;
                dbobj.DbName = dbname;
                dbobj.DbHelperName = "DbHelperMySQL";
                dbobj.ConnectSimple = chk_Simple.Checked;
                int result = Maticsoft.CmConfig.DbConfig.AddSettings(dbobj);
                switch (result)
                {
                    case 0:
                        MessageBox.Show(this, "添加服务器配置失败，请检查安装目录是否有写入权限或文件是否存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoginMySQL_Load(object sender, EventArgs e)
        {
            //comboBoxServerVer.SelectedIndex = 0;
            //comboBox_Verified.SelectedIndex = 0;
        }
    }
}