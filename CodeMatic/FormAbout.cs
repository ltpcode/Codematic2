using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Maticsoft.AddInManager;

namespace Codematic
{
    public partial class FormAbout : Form
    {
        DataSet dsAddin;
        public FormAbout()
        {
            InitializeComponent();
        }

        private void FormAbout_Load(object sender, EventArgs e)
        {            
            AssemblyInfo ainfo = new AssemblyInfo();
            lblVer.Text = "°æ±¾£º" + Application.ProductVersion + " (Build " + ainfo.Description+ ")";
            lblCopyright.Text = ainfo.Copyright;
            BindListbox();
        }
        private void BindListbox()
        {            
            try
            {
                AddIn addin = new AddIn();
                dsAddin = addin.GetAddInList();
                this.listBox1.Items.Clear();
                if (dsAddin != null)
                {
                    if (dsAddin.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsAddin.Tables[0].Rows)
                        {
                            string guid = dr["Guid"].ToString();
                            string name = dr["Name"].ToString();
                            string ver = dr["Version"].ToString();
                            listBox1.Items.Add(name + "  " + ver + " " + guid);
                        }
                    }
                }
                
            }
            catch(SystemException ex)
            {
                string err = ex.Message;
            }            
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string val = listBox1.SelectedItem.ToString();
                int n=val.LastIndexOf(" ");
                string guid = val.Substring(n).Trim();
                DataRow[] drs = dsAddin.Tables[0].Select("Guid='" + guid + "'");
                if (drs.Length > 0)
                {
                    lblDesc.Text = drs[0]["Decription"].ToString();
                    
                    
                }
            }
        }
    }
}