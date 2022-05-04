using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Codematic.UserControls
{
    /// <summary>
    /// 代码浏览控件
    /// </summary>
    public partial class UcCodeView : UserControl
    {
        private string codeType = "cs";
        public UcCodeView()
        {
            InitializeComponent();
            SettxtContent("CS", "");
        }
        #region Menu
        private void menu_Copy_Click(object sender, EventArgs e)
        {            
            string text = "";
            switch (codeType)
            {
                case "sql":
                    {
                        text = txtContent_SQL.ActiveTextAreaControl.SelectionManager.SelectedText;
                    }
                    break;
                case "cs":
                    {
                        text = txtContent_CS.ActiveTextAreaControl.SelectionManager.SelectedText;
                    }
                    break;
                case "htm":
                case "html":
                case "aspx":
                    {
                        text = txtContent_Web.ActiveTextAreaControl.SelectionManager.SelectedText;
                    }
                    break;
                case "xml":
                    {
                        text = txtContent_Web.ActiveTextAreaControl.SelectionManager.SelectedText;
                    }
                    break;
                default:
                    {
                        text = txtContent_CS.ActiveTextAreaControl.SelectionManager.SelectedText;
                    }
                    break;
            }
            Clipboard.SetDataObject(text);

        }
        private void menu_Save_Click(object sender, EventArgs e)
        {            
            SaveFileDialog sqlsavedlg = new SaveFileDialog();
            sqlsavedlg.Title = "保存当前代码";
            string text = "";
            switch (codeType)
            {
                case "sql":
                    {
                        sqlsavedlg.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
                        text = txtContent_SQL.Text; 
                    }
                    break;
                case "cs":
                    {
                        sqlsavedlg.Filter = "C# files (*.cs)|*.cs|All files (*.*)|*.*";
                        text = txtContent_CS.Text;
                    }
                    break;
                case "htm":
                case "html":
                case "aspx":
                    {
                        sqlsavedlg.Filter = "Aspx files (*.aspx)|*.aspx|All files (*.*)|*.*";
                        text = txtContent_Web.Text;
                    }
                    break;
                case "xml":
                    {
                        sqlsavedlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                        text = txtContent_Web.Text;
                    }
                    break;
                default:
                    {
                        sqlsavedlg.Filter = "All files (*.*)|*.*";
                        text = txtContent_CS.Text;
                    }
                    break;
            }                   
            DialogResult dlgresult = sqlsavedlg.ShowDialog(this);
            if (dlgresult == DialogResult.OK)
            {
                string filename = sqlsavedlg.FileName;                
                StreamWriter sw = new StreamWriter(filename, false, Encoding.Default);//,false);
                sw.Write(text);
                sw.Flush();//从缓冲区写入基础流（文件）
                sw.Close();
            }            
        }
        #endregion


        //设置代码控件内容
        public void SettxtContent(string Type, string strContent)
        {
            codeType=Type.ToLower();
            switch (codeType)
            {
                case "sql":
                    {
                        this.txtContent_SQL.Visible = true;
                        this.txtContent_CS.Visible = false;
                        this.txtContent_Web.Visible = false;
                        this.txtContent_XML.Visible = false;
                        this.txtContent_SQL.Dock = DockStyle.Fill;
                        this.txtContent_SQL.Text = strContent;

                    }
                    break;                
                case "htm":
                case "html":
                case "aspx":
                    {
                        this.txtContent_SQL.Visible = false;
                        this.txtContent_CS.Visible = false;
                        this.txtContent_XML.Visible = false;
                        this.txtContent_Web.Visible = true;
                        this.txtContent_Web.Dock = DockStyle.Fill;
                        this.txtContent_Web.Text = strContent;
                    }
                    break;
                case "xml":
                    {
                        this.txtContent_SQL.Visible = false;
                        this.txtContent_CS.Visible = false;
                        this.txtContent_Web.Visible = false;                        
                        this.txtContent_XML.Visible = true;
                        this.txtContent_XML.Dock = DockStyle.Fill;
                        this.txtContent_XML.Text = strContent;
                    }
                    break;
                default://默认cs
                    {
                        this.txtContent_SQL.Visible = false;
                        this.txtContent_CS.Visible = true;
                        this.txtContent_Web.Visible = false;
                        this.txtContent_XML.Visible = false;
                        this.txtContent_CS.Dock = DockStyle.Fill;
                        this.txtContent_CS.Text = strContent;
                    }
                    break;
            }           

        }
    }
}
