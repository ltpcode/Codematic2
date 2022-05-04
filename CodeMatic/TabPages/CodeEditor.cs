using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using LTP.TextEditor;
namespace Codematic
{
    public partial class CodeEditor : Form
    {
        public string fileName = "";
        private string fileType = "cs";
        public CodeEditor()
        {
            InitializeComponent();
        }

        public CodeEditor(string tempFile, string FileType,bool NewCreate)
        {
            InitializeComponent();
            fileType = FileType;
            switch (FileType.ToLower())
            {
                case "cs":
                    txtContent.Language = TextEditorControlBase.Languages.CSHARP;
                    break;
                case "vb":
                    txtContent.Language = TextEditorControlBase.Languages.VBNET;
                    break;
                case "cmt":
                case "tt":
                case "html":
                    txtContent.Language = TextEditorControlBase.Languages.HTML;
                    break;
                case "sql":
                    txtContent.Language = TextEditorControlBase.Languages.SQL;
                    break;
                case "cpp":
                    txtContent.Language = TextEditorControlBase.Languages.CPP;
                    break;
                case "js":
                    txtContent.Language = TextEditorControlBase.Languages.JavaScript;
                    break;
                case "java":
                    txtContent.Language = TextEditorControlBase.Languages.Java;
                    break;
                case "xml":
                    txtContent.Language = TextEditorControlBase.Languages.XML;
                    break;
                case "txt":                                  
                    txtContent.Language = TextEditorControlBase.Languages.XML;
                    break;
                default:
                    txtContent.Language = TextEditorControlBase.Languages.CSHARP;
                    break;
            }
            if (!NewCreate)
            {
                fileName = tempFile;
            }
            StreamReader srFile = new StreamReader(tempFile, Encoding.Default);
            string Contents = srFile.ReadToEnd();
            srFile.Close();
            this.txtContent.Text = Contents;
        }

        public CodeEditor(string strCode, string FileType,string temp)
        {
            InitializeComponent();
            switch (FileType)
            {
                case "cs":
                    txtContent.Language = TextEditorControlBase.Languages.CSHARP;
                    break;
                case "vb":
                    txtContent.Language = TextEditorControlBase.Languages.VBNET;
                    break;
                case "html":
                    txtContent.Language = TextEditorControlBase.Languages.HTML;
                    break;
                case "sql":
                    txtContent.Language = TextEditorControlBase.Languages.SQL;
                    break;
                case "cpp":
                    txtContent.Language = TextEditorControlBase.Languages.CPP;
                    break;
                case "js":
                    txtContent.Language = TextEditorControlBase.Languages.JavaScript;
                    break;
                case "java":
                    txtContent.Language = TextEditorControlBase.Languages.Java;
                    break;
                case "xml":
                    txtContent.Language = TextEditorControlBase.Languages.XML;
                    break;
                case "txt":
                    txtContent.Language = TextEditorControlBase.Languages.XML;
                    break;
            }
            this.txtContent.Text = strCode;
        }


        public void Save()
        {
            if (fileName.Length > 1 && txtContent.Text.Trim().Length > 0)
            {
                StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8);//,false);
                sw.Write(txtContent.Text.Trim());
                sw.Flush();
                sw.Close();
                MessageBox.Show(this, "保存成功！", "提示", MessageBoxButtons.OK);
            }
            else
            {
                menu_SaveAs_Click(null, null);
            }
        }
        private void menu_Save_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void menu_SaveAs_Click(object sender, EventArgs e)
        {
            if (txtContent.Text.Trim().Length > 0)
            {
                SaveFileDialog sqlsavedlg = new SaveFileDialog();
                sqlsavedlg.Title = "保存文件";                
                switch (fileType)
                {
                    case "cs":
                        sqlsavedlg.Filter = "C# files (*.cs)|*.cs|All files (*.*)|*.*";
                        break;
                    case "vb":
                        sqlsavedlg.Filter = "VB files (*.vb)|*.vb|All files (*.*)|*.*";
                        break;
                    case "cmt":
                        sqlsavedlg.Filter = "Template files (*.cmt)|*.cmt|All files (*.*)|*.*";
                        break;
                    case "tt":
                        sqlsavedlg.Filter = "Template files (*.tt)|*.tt|All files (*.*)|*.*";
                        break;
                    case "html":
                        sqlsavedlg.Filter = "Html files (*.htm)|*.htm|All files (*.*)|*.*";
                        break;
                    case "sql":
                        sqlsavedlg.Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*";
                        break;
                    case "cpp":
                        sqlsavedlg.Filter = "CPP files (*.cpp)|*.cpp|All files (*.*)|*.*";
                        break;
                    case "js":
                        sqlsavedlg.Filter = "JS files (*.js)|*.js|All files (*.*)|*.*";
                        break;
                    case "java":
                        sqlsavedlg.Filter = "Java files (*.java)|*.java|All files (*.*)|*.*";
                        break;
                    case "xml":
                        sqlsavedlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                        break;
                    case "txt":
                        sqlsavedlg.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
                        break;
                    default:
                        sqlsavedlg.Filter = "C# files (*.cs)|*.cs|All files (*.*)|*.*";
                        break;
                        
                }
                DialogResult dlgresult = sqlsavedlg.ShowDialog(this);
                if (dlgresult == DialogResult.OK)
                {
                    string filename = sqlsavedlg.FileName;
                    StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8);//,false);
                    sw.Write(txtContent.Text.Trim());
                    sw.Flush();//从缓冲区写入基础流（文件）
                    sw.Close();
                    MessageBox.Show(this, "保存成功！", "提示", MessageBoxButtons.OK);
                } 
                
            }

        }
    }
}