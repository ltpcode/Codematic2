using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Maticsoft.AddInManager;
namespace Codematic.UserControls
{
    /// <summary>
    /// 基本设置
    /// </summary>
    public partial class UcCSSet : UserControl
    {
        //LTP.CmConfig.ModuleSettings setting;
        OptionFrm opfrm;
        DALTypeAddIn cm_daltype;
        DALTypeAddIn cm_blltype;
        DALTypeAddIn cm_webtype;


        public UcCSSet(OptionFrm optionfrm)
        {
            InitializeComponent();
            opfrm = optionfrm;
            LTP.CmConfig.DbSettings[] dbs = LTP.CmConfig.DbConfig.GetSettings();
            if (dbs != null)
            {
                foreach (LTP.CmConfig.DbSettings db in dbs)
                {
                    string servername = db.Server;
                    string dbtype = db.DbType;
                    string dbname = db.DbName;
                    cmboxServers.Items.Add(GetserverNodeText(servername, dbtype, dbname));                    
                }                
            }

            
            //setting = LTP.CmConfig.ModuleConfig.GetSettings();
           
  
            #region 加载插件

            cm_daltype = new DALTypeAddIn("LTP.IBuilder.IBuilderDAL");
            cm_daltype.Title = "DAL";
            groupBox5.Controls.Add(cm_daltype);
            cm_daltype.Location = new System.Drawing.Point(10, 18);
            //cm_daltype.SetSelectedDALType(setting.DALType.Trim());

            cm_blltype = new DALTypeAddIn("LTP.IBuilder.IBuilderBLL");
            cm_blltype.Title = "BLL";
            groupBox5.Controls.Add(cm_blltype);
            cm_blltype.Location = new System.Drawing.Point(10, 42);
            //cm_blltype.SetSelectedDALType(setting.BLLType.Trim());

            cm_webtype = new DALTypeAddIn("LTP.IBuilder.IBuilderWeb");
            cm_webtype.Title = "Web";
            groupBox5.Controls.Add(cm_webtype);
            cm_webtype.Location = new System.Drawing.Point(10, 66);
            //cm_webtype.SetSelectedDALType(setting.WebType.Trim()); 

            #endregion

            if (cmboxServers.Items.Count > 0)
            {
                cmboxServers.SelectedIndex = 0;
                string servername = cmboxServers.Items[0].ToString();
                LoadServerConfig(servername);
            }

            
        }
        //根据服务器配置得到服务器节点字符串
        private string GetserverNodeText(string servername, string dbtype, string dbname)
        {
            string str = servername + "(" + dbtype + ")";
            if ((dbname.Trim() != "") && (dbname.Trim() != "master"))
            {
                str += "(" + dbname + ")";
            }
            return str;
        }
        private void cmboxServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmboxServers.SelectedItem != null)
            {
                string servername = cmboxServers.Text;
                LoadServerConfig(servername);
            }
        }

        private void LoadServerConfig(string servername)
        {
            LTP.CmConfig.DbSettings dbset = LTP.CmConfig.DbConfig.GetSetting(servername);
            if (dbset == null)
            {
                MessageBox.Show("该服务器信息已经不存在，请关闭软件然后重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string server = dbset.Server;
            string dbtype = dbset.DbType;
            string dbname = dbset.DbName;
            bool connectSimple = dbset.ConnectSimple;
            
            switch (dbset.AppFrame.Trim().ToLower())
            {
                case "one":
                    radbtn_Frame_One.Checked = true;
                    break;
                case "s3":
                    radbtn_Frame_S3.Checked = true;
                    break;
                case "f3":
                    radbtn_Frame_F3.Checked = true;
                    break;
            }

            cm_daltype.SetSelectedDALType(dbset.DALType.Trim());
            cm_blltype.SetSelectedDALType(dbset.BLLType.Trim());
            cm_webtype.SetSelectedDALType(dbset.WebType.Trim());

            txtDbHelperName.Text = dbset.DbHelperName;
            txtNamepace.Text = dbset.Namepace;
            txtProjectName.Text = dbset.ProjectName;
            txtProcPrefix.Text = dbset.ProcPrefix;

            opfrm.ucnameset.txtBLL_Prefix.Text = dbset.BLLPrefix;
            opfrm.ucnameset.txtBLL_Suffix.Text = dbset.BLLSuffix;
            opfrm.ucnameset.txtDAL_Prefix.Text = dbset.DALPrefix;
            opfrm.ucnameset.txtDAL_Suffix.Text = dbset.DALSuffix;
            opfrm.ucnameset.txtModel_Prefix.Text = dbset.ModelPrefix;
            opfrm.ucnameset.txtModel_Suffix.Text = dbset.ModelSuffix;

            //表命名规则
            switch (dbset.TabNameRule.ToLower())
            {
                case "same":
                    opfrm.ucnameset.radbtn_Same.Checked = true;
                    break;
                case "lower":
                    opfrm.ucnameset.radbtn_Lower.Checked = true;
                    break;
                case "upper":
                    opfrm.ucnameset.radbtn_Upper.Checked = true;
                    break;
                case "firstupper":
                    opfrm.ucnameset.radbtn_firstUpper.Checked = true;
                    break;
                default:
                    opfrm.ucnameset.radbtn_Same.Checked = true;
                    break;
            }


        }


        /// <summary>
        /// 当前选中的服务器配置
        /// </summary>
        /// <returns></returns>
        public LTP.CmConfig.DbSettings GetCurrentDbSetting()
        {
            LTP.CmConfig.DbSettings dbset=null;
            if (cmboxServers.Items.Count > 0)
            {                
                string servername = cmboxServers.SelectedItem.ToString();
                dbset = LTP.CmConfig.DbConfig.GetSetting(servername);
                if (dbset != null)
                {
                    string server = dbset.Server;
                    string dbtype = dbset.DbType;
                    string dbname = dbset.DbName;
                    bool connectSimple = dbset.ConnectSimple;
                }                
            }
            return dbset;
        }


        //得到数据层类型
        public string GetDALType()
        {
            string daltype = "";
            daltype = cm_daltype.AppGuid;
            if ((daltype == "") || (daltype == "System.Data.DataRowView"))
            {
                MessageBox.Show("选择的数据层类型有误，请关闭后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
            return daltype;
        }

        //得到数据层类型
        public string GetBLLType()
        {
            string blltype = "";
            blltype = cm_blltype.AppGuid;
            if ((blltype == "") || (blltype == "System.Data.DataRowView"))
            {
                MessageBox.Show("选择的数据层类型有误，请关闭后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
            return blltype;
        }

        //得到数据层类型
        public string GetWebType()
        {
            string webtype = "";
            webtype = cm_webtype.AppGuid;
            if ((webtype == "") || (webtype == "System.Data.DataRowView"))
            {
                MessageBox.Show("选择的表示层类型有误，请关闭后重试！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return "";
            }
            return webtype;
        }

        

       
    }
}
