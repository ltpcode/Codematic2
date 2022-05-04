using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Codematic.UserControls;
namespace Codematic
{
    public partial class TempView : Form
    {
        MainForm mainfrm;
        TempNode TreeClickNode; //右键菜单单击的节点
        DataSet ds ; //菜单数据
        Maticsoft.CmConfig.AppSettings settings;
        string tempfilepath = "temptree.xml"; //菜单树数据文件
        string TemplateFolder;//模板所在文件夹


        public TempView(Form mdiParentForm)
        {
            mainfrm = (MainForm)mdiParentForm;
            InitializeComponent();
            settings = Maticsoft.CmConfig.AppConfig.GetSettings();

            if (settings.TemplateFolder == "Template" ||
                settings.TemplateFolder == "Template\\TemplateFile" ||
                settings.TemplateFolder.Length == 0)
            {
                TemplateFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template\\TemplateFile");//                
            }
            else
            {
                DirectoryInfo tempdir = new DirectoryInfo(settings.TemplateFolder);
                if (tempdir.Exists)
                {
                    TemplateFolder = settings.TemplateFolder;
                }
                else
                {
                    TemplateFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template\\TemplateFile");
                }                
            }
            CreateFolderTree(TemplateFolder);
        }


        #region 加载目录结构

        private void CreateFolderTree(string templateFolder)
        {
            treeView1.Nodes.Clear();
            TempNode rootNode = new TempNode("代码模版");
            rootNode.NodeType = "root";
            rootNode.FilePath = templateFolder;
            rootNode.ImageIndex = 0;
            rootNode.SelectedImageIndex = 0;
            rootNode.Expand();
            treeView1.Nodes.Add(rootNode);

            LoadFolderTree(rootNode,templateFolder);
        }


        private void LoadFolderTree(TreeNode parentnode, string templateFolder)
        {           
            DirectoryInfo source = new DirectoryInfo(templateFolder);
            if (!source.Exists)
                return;

            DirectoryInfo[] sourceDirectories = source.GetDirectories();
            for (int j = 0; j < sourceDirectories.Length; ++j)
            {                
                TempNode node = new TempNode(sourceDirectories[j].Name);                
                node.NodeType = "folder";
                node.FilePath = sourceDirectories[j].FullName;
                node.ImageIndex = 0;
                node.SelectedImageIndex = 1;
                parentnode.Nodes.Add(node);
                LoadFolderTree(node,sourceDirectories[j].FullName);
            }

            FileInfo[] sourceFiles = source.GetFiles();
            int filescount = sourceFiles.Length;
            for (int i = 0; i < filescount; ++i)
            {
                if ((sourceFiles[i].Extension == ".tt") ||
                    (sourceFiles[i].Extension == ".cmt") || 
                    (sourceFiles[i].Extension == ".aspx") ||
                    (sourceFiles[i].Extension == ".cs") || 
                    (sourceFiles[i].Extension == ".vb")
                    )
                {
                    TempNode node = new TempNode(sourceFiles[i].Name);                    
                    node.FilePath = sourceFiles[i].FullName;
                    switch (sourceFiles[i].Extension)
                    {
                        case ".tt":
                            node.NodeType = "tt";
                            node.ImageIndex = 2;
                            node.SelectedImageIndex = 2;
                            break;
                        case ".cmt":
                            node.NodeType = "cmt";
                            node.ImageIndex = 2;
                            node.SelectedImageIndex = 2;
                            break;
                        case ".cs":
                            node.NodeType = "cs";
                            node.ImageIndex = 3;
                            node.SelectedImageIndex = 3;
                            break;
                        case ".vb":
                            node.NodeType = "vb";
                            node.ImageIndex = 4;
                            node.SelectedImageIndex = 4;
                            break;
                        case ".aspx":
                            node.NodeType = "aspx";
                            node.ImageIndex = 5;
                            node.SelectedImageIndex = 5;
                            break;
                        default:
                            node.ImageIndex = 2;
                            node.SelectedImageIndex = 2;
                            break;
                    }                    
                    parentnode.Nodes.Add(node);
                }
            }      
           


        }

        


        #endregion

        #region  加载数据初始化Treeview项

        private void LoadTreeview()
        {
            ds = new DataSet(); //菜单数据
            treeView1.Nodes.Clear();
            TempNode rootNode = new TempNode("代码模版");
            rootNode.NodeType = "root";
            rootNode.ImageIndex = 0;
            rootNode.SelectedImageIndex = 0;
            rootNode.Expand();
            treeView1.Nodes.Add(rootNode);
                        
            ds.ReadXml(tempfilepath);
            DataTable dt=ds.Tables[0];
            
            DataRow[] drs = dt.Select("ParentID= " + 0); //选出所有一级节点	
            foreach (DataRow r in drs)
            {
                string nodeid = r["NodeID"].ToString();
                string text = r["Text"].ToString();
                string filepath = r["FilePath"].ToString();
                string nodetype = r["NodeType"].ToString();
                
                TempNode node = new TempNode(text);
                node.NodeID = nodeid;
                node.NodeType = nodetype;
                node.FilePath = filepath;
                if (nodetype == "folder")
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 1;
                }
                else
                {
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                }                
                rootNode.Nodes.Add(node);
                
                int sonparentid = int.Parse(nodeid); // or =location
                CreateNode(sonparentid,node,dt);
            }
            

        }

        //邦定任意节点
        public void CreateNode(int parentid, TreeNode parentnode, DataTable dt)
        {
            DataRow[] drs = dt.Select("ParentID= " + parentid); //选出所有子节点			
            foreach (DataRow r in drs)
            {
                string nodeid = r["NodeID"].ToString();
                string text = r["Text"].ToString();
                string filepath = r["FilePath"].ToString();
                string nodetype = r["NodeType"].ToString();
                
                TempNode node = new TempNode(text);
                node.NodeID = nodeid;
                node.NodeType = nodetype;
                node.FilePath = filepath;
                if (nodetype == "folder")
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 1;
                }
                else
                {
                    node.ImageIndex = 2;
                    node.SelectedImageIndex = 2;
                }                        

                parentnode.Nodes.Add(node);

                int sonparentid = int.Parse(nodeid);// or =location
                CreateNode(sonparentid, node, dt);

            }//endforeach		

        }


        #endregion

        #region treeView1操作

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {            
        }
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                Point mpt = new Point(e.X, e.Y);
                TreeClickNode = (TempNode)this.treeView1.GetNodeAt(mpt);
                this.treeView1.SelectedNode = TreeClickNode;
                if (TreeClickNode != null)
                {                   
                    if (e.Button == MouseButtons.Right)
                    {
                        CreatMenu(TreeClickNode.NodeType);
                        contextMenuStrip1.Show(treeView1, mpt);
                    }
                }
            }
            catch
            {
            }
        }

        #region 创建treeview 右键菜单

        private void CreatMenu(string NodeType)
        {
            打开并生成ToolStripMenuItem.Visible = false;
            编辑查看ToolStripMenuItem.Visible = false;
            新建ToolStripMenuItem.Visible = false;
            设置模板文件夹ToolStripMenuItem.Visible = false;
            备份模板到ToolStripMenuItem.Visible = false;
            switch (NodeType)
            {
                case "root":
                    {                        
                        新建ToolStripMenuItem.Visible = true;
                        打开所在文件夹ToolStripMenuItem.Visible = true;
                        设置模板文件夹ToolStripMenuItem.Visible = true;
                        备份模板到ToolStripMenuItem.Visible = true;
                    }
                    break;
                case "folder":
                    {                        
                        新建ToolStripMenuItem.Visible = true;
                        设置模板文件夹ToolStripMenuItem.Visible = false;
                        备份模板到ToolStripMenuItem.Visible = false;
                        打开所在文件夹ToolStripMenuItem.Visible = true;
                    }
                    break;                
                default:
                    {
                        打开并生成ToolStripMenuItem.Visible = true;
                        编辑查看ToolStripMenuItem.Visible = true;
                        打开所在文件夹ToolStripMenuItem.Visible = false;
                    }
                    break;
            }
        }

        #endregion

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            //TreeNode node = (TreeNode)e.Item;          
            //if ((node.Tag.ToString() == "table") || (node.Tag.ToString() == "view") || (node.Tag.ToString() == "column"))
            //{
            //    DoDragDrop(node, DragDropEffects.Copy);
            //}
        }

        #endregion

        private string GetMaxNodeID(DataTable dt)
        {
            int maxval = 1;
            foreach (DataRow row in dt.Rows)
            {
                string nodeid = row["NodeID"].ToString();
                if (maxval < int.Parse(nodeid))
                {
                    maxval = int.Parse(nodeid);
                }
            }            
            return (maxval+1).ToString();

            //object obj = dt.Compute("max(CONVERT(int,NodeID))+1", "");
            //object obj = dt.Compute("max(NodeID)+1", "");
            //return obj.ToString();
        }
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TempNode SelNode = (TempNode)this.treeView1.SelectedNode;
            string nodeid = SelNode.NodeID;            
            string nodetype = SelNode.NodeType;
            switch (nodetype)
            {
                case "tt":
                case "cmt":
                case "vb":
                case "aspx":
                case "cs":
                    打开生成ToolStripMenuItem_Click(sender, e);
                    break;
            }
        }
        
        #region 右键菜单事件

        private void 打开生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TempNode SelNode = (TempNode)this.treeView1.SelectedNode;               
                string nodeid = SelNode.NodeID;
                string text = SelNode.Text;
                string filepath = SelNode.FilePath;
                string nodetype = SelNode.NodeType;

                if (filepath.Trim() != "")
                {
                    #region
                    switch (nodetype)
                    {
                        case "folder":
                            {                                
                            }
                            break;
                        case "tt":
                        case "cmt":
                            {
                                CodeTemplate codetempfrm = (CodeTemplate)Application.OpenForms["CodeTemplate"];
                                if (codetempfrm != null)
                                {
                                    codetempfrm.SettxtTemplate(filepath);
                                }
                                else
                                {
                                    MessageBox.Show("尚未打开模版代码生成器！请选中表，然后右键打开模版代码生成器。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            break;
                        default:
                            {                                
                                if (File.Exists(filepath))
                                {
                                    mainfrm.AddTabPage(text, new CodeEditor(filepath, nodetype, false));                                    
                                }
                            }
                            break;                        
                    }
                    #endregion

                }
                else
                {
                    MessageBox.Show("所选文件已经不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 编辑查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TempNode SelNode = (TempNode)this.treeView1.SelectedNode;
                string nodeid = SelNode.NodeID;
                string text = SelNode.Text;
                string filepath = SelNode.FilePath;
                string nodetype = SelNode.NodeType;

                if (filepath.Trim() != "" && nodetype != "folder")
                {                    
                    if (File.Exists(filepath))
                    {
                        mainfrm.AddTabPage(text, new CodeEditor(filepath, nodetype, false));
                    }
                }
                else
                {
                    MessageBox.Show("所选文件已经不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //新建文件夹
        private void 文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TempNode SelNode = (TempNode)this.treeView1.SelectedNode;
                string folder = SelNode.FilePath + "\\新文件夹";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                
                //在treeview上增加一个节点
                TempNode node = new TempNode("新文件夹");                
                node.FilePath = folder;
                node.NodeType = "folder";               
                node.ImageIndex = 0;
                node.SelectedImageIndex = 1;                
                SelNode.Nodes.Add(node);
                SelNode.Expand();

                treeView1.SelectedNode = node;
                treeView1.LabelEdit = true;
                if (!node.IsEditing)
                {
                    node.BeginEdit();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public string GetFileName(string path,string oldname,out string filename)
        {
            string filepath = path + "\\" + oldname + ".cmt";
            filename = oldname;
            int n = 1;            
            while (File.Exists(filepath))
            {
                filename = oldname + n;
                filepath = path + "\\" + filename + ".cmt";
                n++;
            }
            return filepath;
        }
        //新建模版文件
        private void 模版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TempNode SelNode = (TempNode)this.treeView1.SelectedNode;
                string filepath = SelNode.FilePath + "\\" + "新建模板.cmt";
                string filename = "新建模板";
                filepath = GetFileName(SelNode.FilePath, filename, out filename);
                //File.Create(filepath);

                StreamWriter sw = new StreamWriter(filepath, false,Encoding.UTF8);
                //sw.WriteLine("");
                sw.Close();
                
                TempNode node = new TempNode(filename+".cmt");
                //node.NodeID = newNodeid;
                //node.ParentID = nodeid;
                node.FilePath = filepath;
                node.NodeType = "cmt";
                node.ImageIndex = 2;
                node.SelectedImageIndex = 2;
                SelNode.Nodes.Add(node);

                treeView1.SelectedNode = node;
                treeView1.LabelEdit = true;
                if (!node.IsEditing)
                {
                    node.BeginEdit();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFolderTree(TemplateFolder);
        }
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TempNode SelNode = (TempNode)this.treeView1.SelectedNode;

                string nodeid = SelNode.NodeID;
                string text = SelNode.Text;
                string filepath = SelNode.FilePath;
                string nodetype = SelNode.NodeType;
                                
                if ((nodetype == "tt") ||
                    (nodetype == "cmt") ||
                    (nodetype == "aspx") ||
                    (nodetype == "cs") ||
                    (nodetype == "vb")
                    )
                {
                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);
                    }
                }
                if (nodetype == "folder")
                {
                    DirectoryInfo newdir = new DirectoryInfo(filepath);
                    if (newdir.Exists)
                    {
                        newdir.Delete();
                    }
                }

                //删除节点
                treeView1.Nodes.Remove(SelNode);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void 重命名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TempNode SelNode = (TempNode)this.treeView1.SelectedNode;
                if (SelNode != null && SelNode.Parent != null)
                {
                    treeView1.SelectedNode = SelNode;
                    treeView1.LabelEdit = true;
                    if (!SelNode.IsEditing)
                    {
                        SelNode.BeginEdit();
                    }                    
                }
                else
                {
                    MessageBox.Show("没有选择节点或该节点是根节点.\n" , "无效选择");
                }
                                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (e.Label.IndexOfAny(new char[] { '@',',', '!' }) == -1)
                    {
                        try
                        {
                            e.Node.EndEdit(false);
                            TempNode SelNode = (TempNode)e.Node;
                            string nodetype = SelNode.NodeType;
                            string newNodetext = e.Label;
                            int n = newNodetext.LastIndexOf(".");
                            if (n == 0)
                            {
                                MessageBox.Show("无效的文件名", "节点编辑");
                                return;
                            }
                            if (newNodetext.LastIndexOf(".") < 0)
                            {
                                newNodetext += ".cmt";
                            }
                            else
                            {
                                if (newNodetext.Substring(n).ToLower() != ".cmt")
                                {
                                    DialogResult dr = MessageBox.Show("如果要更改文件扩展名，则该文件可能无法使用。是否确实要更改它？", "节点编辑", MessageBoxButtons.YesNo);
                                    if (dr != DialogResult.Yes)
                                    {
                                        return;
                                    }
                                }

                            }
                            string filepath = SelNode.FilePath;
                            string newfilepath = filepath;

                            if ((nodetype == "tt") ||
                                (nodetype == "cmt") ||
                                (nodetype == "aspx") ||
                                (nodetype == "cs") ||
                                (nodetype == "vb")
                                )
                            {
                                int end = filepath.LastIndexOf("\\");
                                newfilepath = filepath.Substring(0, end) + "\\" + newNodetext;
                                File.Move(filepath, newfilepath);
                                SelNode.FilePath = newfilepath;
                            }
                            if (nodetype == "folder")
                            {
                                int end = filepath.LastIndexOf("\\");
                                newfilepath = filepath.Substring(0, end) + "\\" + newNodetext;
                                Directory.Move(filepath, newfilepath);
                            }
                        }
                        catch(Exception ex)
                        {
                            e.CancelEdit = true;
                            MessageBox.Show("重命名失败！请稍候再试。" + ex.Message);                            
                        }
                    }
                    else
                    {
                        e.CancelEdit = true;
                        MessageBox.Show("无效节点或无效字符: '@','.', ',', '!'","节点编辑");
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    e.CancelEdit = true;
                    MessageBox.Show("无效节点或节点名称不能为空！", "节点编辑");
                    e.Node.BeginEdit();
                }
                this.treeView1.LabelEdit = false;
            }

        }
        #endregion

        private void 设置模板文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                Maticsoft.CmConfig.AppSettings appsettings = Maticsoft.CmConfig.AppConfig.GetSettings();
                appsettings.TemplateFolder = folderName;
                Maticsoft.CmConfig.AppConfig.SaveSettings(appsettings);
                TemplateFolder = folderName;
                CreateFolderTree(folderName);
            }

        }
        private void 备份模板到ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                CopyDirectory(TemplateFolder, folderName);
                MessageBox.Show("备份完成！");
                try
                {
                    Process proc = new Process();
                    Process.Start("explorer.exe", folderName);
                }
                catch
                {
                    MessageBox.Show("打开目标文件夹失败，请手动打开该文件夹。");
                }
            }
        }


        #region  复制项目

        public void CopyDirectory(string SourceDirectory, string TargetDirectory)
        {
            DirectoryInfo source = new DirectoryInfo(SourceDirectory);
            DirectoryInfo target = new DirectoryInfo(TargetDirectory);
            //Check If we have valid source
            if (!source.Exists)
                return;
            if (!target.Exists)
                target.Create();
            //Copy Files
            FileInfo[] sourceFiles = source.GetFiles();
            int filescount = sourceFiles.Length;
            for (int i = 0; i < filescount; ++i)
            {
                File.Copy(sourceFiles[i].FullName, target.FullName + "\\" + sourceFiles[i].Name, true);
            }
            //Copy directories
            DirectoryInfo[] sourceDirectories = source.GetDirectories();
            for (int j = 0; j < sourceDirectories.Length; ++j)
            {
                CopyDirectory(sourceDirectories[j].FullName, target.FullName + "\\" + sourceDirectories[j].Name);
            }
        }

        #endregion

        private void 打开所在文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process proc = new Process();
                Process.Start("explorer.exe", TemplateFolder);
            }
            catch
            {
                MessageBox.Show("打开目标文件夹失败，请手动打开该文件夹。");
            }
        }
        

        

        

    }
}