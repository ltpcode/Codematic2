using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Codematic.UserControls
{
    public partial class UcLanguage : UserControl
    {
        Maticsoft.CmConfig.AppSettings settings;
        public UcLanguage(Maticsoft.CmConfig.AppSettings setting)
        {
            InitializeComponent();
            settings = setting;
            Language = setting.Language;

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
        public string Language
        {
            get
            {
                string strlan = "zh-cn";
                switch(cmbUILan.Text)
                {
                    case "简体中文":
                        strlan = "zh-cn";
                        break;
                    case "English":
                        strlan = "en";
                        break;
                    //case "":
                    //    return "";
                    //    break;
                    default:
                        strlan = "zh-cn";
                        break;
                }
                return strlan;
            }
            set
            {
                switch (value)
                {
                    case "zh-cn":
                        cmbUILan.SelectedIndex=0;
                        break;
                    case "en":
                        cmbUILan.SelectedIndex = 1;
                        break;
                    //case "":
                    //    return "";
                    //    break;
                    default:
                        cmbUILan.SelectedIndex = 0;
                        break;
                }
            }
        }

        private void UcLanguage_Load(object sender, EventArgs e)
        {           
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

        private void btnBrowe_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtTempPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
