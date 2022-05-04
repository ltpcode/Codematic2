using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Maticsoft.CodeHelper;
using System.Threading;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;

namespace Codematic
{
    /// <summary>
    /// 搜索表
    /// </summary>
    public partial class SearchTables : Form
    {
        //1. 查找含有指定字符字段的表和字段
        //2. 查找是指定类型的字段的表
        //3. 查找指定表被引用的外键表
        MainForm mainfrm;
        Maticsoft.IDBO.IDbObject dbobj;
        Maticsoft.CmConfig.DbSettings dbset;
        private Microsoft.Office.Interop.Word.Application WordApp = new Microsoft.Office.Interop.Word.ApplicationClass();
        Thread mythread;
        string currentDbname = "";
        protected string longServername;

        delegate void SetlblStatuCallback(string text);
        delegate void SetProBar1MaxCallback(int val);
        delegate void SetProBar1ValCallback(int val);


        public SearchTables(MainForm mdiParentForm, string longservername)
        {
            InitializeComponent();
            mainfrm = mdiParentForm;
            longServername = longservername;
            dbset = Maticsoft.CmConfig.DbConfig.GetSetting(longservername);
            dbobj = Maticsoft.DBFactory.DBOMaker.CreateDbObj(dbset.DbType);
            dbobj.DbConnectStr = dbset.ConnectStr;
            lblServer.Text = dbset.Server;
            CreatListView();
            WorkLoad();
            cmboxType.SelectedIndex = 0;
        }

        void WorkLoad()
        {
            string mastedb = "master";
            switch (dbobj.DbType)
            {
                case "SQL2000":
                case "SQL2005":
                case "SQL2008":
                case "SQL2012":
                    mastedb = "master";
                    break;
                case "Oracle":
                case "OleDb":
                    mastedb = dbset.DbName;
                    break;
                case "MySQL":
                    mastedb = "mysql";
                    break;
                case "SQLite":
                    mastedb = "sqlite_master";
                    break;
            }
            if ((dbset.DbName == "") || (dbset.DbName == mastedb))
            {
                List<string> dblist = dbobj.GetDBList();
                if (dblist != null)
                {
                    if (dblist.Count > 0)
                    {
                        foreach (string dbname in dblist)
                        {
                            this.cmbDB.Items.Add(dbname);
                        }
                    }
                }
            }
            else
            {
                this.cmbDB.Items.Add(dbset.DbName);
            }

            if (this.cmbDB.Items.Count > 0)
            {
                this.cmbDB.SelectedIndex = 0;
            }

        }

        private void CreatListView()
        {
            //创建列表
            this.listView1.Columns.Clear();
            this.listView1.Items.Clear();

            this.listView1.View = View.Details;
            this.listView1.GridLines = true;
            this.listView1.FullRowSelect = true;
            this.listView1.CheckBoxes = false;
            listView1.Columns.Add("选择", 40, HorizontalAlignment.Left);
            listView1.Columns.Add("表名", 200, HorizontalAlignment.Left);
            listView1.Columns.Add("字段", 200, HorizontalAlignment.Left);
        }

        #region 搜索
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                mythread = new Thread(new ThreadStart(SearchThread));
                mythread.Start();
            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }
        void SearchThread()
        {
            currentDbname = cmbDB.Text;
            string keyword = txtKeyWord.Text.Trim().ToLower();
            List<string> tables = dbobj.GetTables(currentDbname);

            SetprogressBar1Max(tables.Count);
            SetprogressBar1Val(1);

            if (chkClearList.Checked)
            {
                listView1.Items.Clear();
            }

            int n = 0, i = 0;
            foreach (string tabname in tables)
            {
                if ((cmboxType.SelectedIndex == 2) && (tabname.ToLower().IndexOf(keyword) > -1)) //搜索表
                {
                    ListViewItem item1 = new ListViewItem("", 0);
                    item1.Checked = true;
                    item1.SubItems.Add(tabname);
                    item1.SubItems.Add("");
                    listView1.Items.AddRange(new ListViewItem[] { item1 });
                    n++;
                }
                else
                {
                    #region 搜索列
                    List<Maticsoft.CodeHelper.ColumnInfo> cols = dbobj.GetColumnList(currentDbname, tabname);
                    foreach (Maticsoft.CodeHelper.ColumnInfo col in cols)
                    {
                        SetlblStatuText(tabname + "-" + col);
                        #region 字段名
                        if ((cmboxType.SelectedIndex == 0) && (col.ColumnName.ToLower().IndexOf(keyword) > -1))
                        {
                            ListViewItem item1 = new ListViewItem("", 0);
                            item1.Checked = true;
                            item1.SubItems.Add(tabname);
                            item1.SubItems.Add(col.ColumnName);
                            listView1.Items.AddRange(new ListViewItem[] { item1 });
                            n++;
                        }
                        #endregion

                        #region 字段类型
                        if ((cmboxType.SelectedIndex == 1) && (col.TypeName.ToLower().Equals(keyword)))
                        {
                            ListViewItem item1 = new ListViewItem("", 0);
                            item1.Checked = true;
                            item1.SubItems.Add(tabname);
                            item1.SubItems.Add(col.ColumnName);
                            listView1.Items.AddRange(new ListViewItem[] { item1 });
                            n++;
                        }
                        #endregion
                    }
                    #endregion
                }

                SetprogressBar1Val(i + 1);
                i++;
            }
            SetlblStatuText("共搜索到 " + n.ToString() + "个结果。");
        }

        #endregion


        #region 导出word

        private void btnExpToWord_Click(object sender, EventArgs e)
        {
            try
            {
                currentDbname = this.cmbDB.Text;
                mythread = new Thread(new ThreadStart(ThreadWork));
                mythread.Start();

            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
                
        void ThreadWork()
        {
            try
            {
                //SetBtnDisable();
                string strtitle = "数据库名：" + currentDbname;
                int tblCount = listView1.Items.Count;

                SetprogressBar1Max(tblCount);
                SetprogressBar1Val(1);
                SetlblStatuText("0");

                #region 产生文档，写入标题

                object oMissing = System.Reflection.Missing.Value;
                object oEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */

                //创建文档
                Word._Application oWord = new Word.Application();
                Word._Document oDoc;
                oWord.Visible = false;
                oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                //设置页眉
                oWord.ActiveWindow.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdOutlineView;
                oWord.ActiveWindow.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekPrimaryHeader;
                oWord.ActiveWindow.ActivePane.Selection.InsertAfter("动软自动生成器 www.maticsoft.com");
                oWord.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;//设置右对齐
                oWord.ActiveWindow.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;//跳出页眉设置

                //库名
                Word.Paragraph oPara1;
                oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);
                oPara1.Range.Text = strtitle;
                oPara1.Range.Font.Bold = 1;
                oPara1.Range.Font.Name = "宋体";
                oPara1.Range.Font.Size = 12;
                oPara1.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                oPara1.Format.SpaceAfter = 5;    //24 pt spacing after paragraph.
                oPara1.Range.InsertParagraphAfter();

                #endregion


                #region 表的扩展属性

                DataTable dtEx = dbobj.GetTablesExProperty(currentDbname);

                #endregion


                #region 循环每个表
                List<string> tablist = new List<string>();
                int i = 0;
                foreach (ListViewItem item in this.listView1.SelectedItems)
                {
                    string tablename = item.SubItems[1].Text;
                    string tabletitle = "表名：" + tablename;

                    if (tablist.Contains(tablename))
                    {                        
                        continue;
                    }
                    tablist.Add(tablename);

                    #region 循环每一个列，产生一行数据

                    List<ColumnInfo> collist = dbobj.GetColumnInfoList(currentDbname, tablename);
                    int rc = collist.Count;
                    if ((collist != null) && (collist.Count > 0))
                    {
                        //表名
                        Word.Paragraph oPara2;
                        object oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                        oPara2 = oDoc.Content.Paragraphs.Add(ref oRng);
                        oPara2.Range.Text = tabletitle;
                        oPara2.Range.Font.Bold = 1;
                        oPara2.Range.Font.Name = "宋体";
                        oPara2.Range.Font.Size = 10;
                        oPara2.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        oPara2.Format.SpaceBefore = 15;
                        oPara2.Format.SpaceAfter = 1;
                        oPara2.Range.InsertParagraphAfter();

                        //描述信息
                        string tabdesc = "";
                        if (dtEx != null)
                        {
                            try
                            {
                                DataRow[] drs = dtEx.Select("objname='" + tablename + "'");
                                if (drs.Length > 0)
                                {
                                    if (drs[0]["value"] != null)
                                    {
                                        tabdesc = drs[0]["value"].ToString();
                                    }
                                }
                            }
                            catch
                            { }
                        }

                        Word.Paragraph oPara3;
                        oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                        oPara3 = oDoc.Content.Paragraphs.Add(ref oRng);
                        oPara3.Range.Text = tabdesc;
                        oPara3.Range.Font.Bold = 0;
                        oPara3.Range.Font.Name = "宋体";
                        oPara3.Range.Font.Size = 9;
                        oPara3.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;//.wdAlignParagraphCenter;
                        oPara3.Format.SpaceBefore = 1;
                        oPara3.Format.SpaceAfter = 1;
                        oPara3.Range.InsertParagraphAfter();

                        //插入表格          
                        Word.Table oTable;
                        Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                        oTable = oDoc.Tables.Add(wrdRng, rc + 1, 10, ref oMissing, ref oMissing);
                        oTable.Range.Font.Name = "宋体";
                        oTable.Range.Font.Size = 9;
                        oTable.Borders.Enable = 1;
                        oTable.Rows.Height = 10;
                        oTable.AllowAutoFit = true;
                        //wrdRng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        //oTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleThickThinLargeGap;
                        //oTable.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                        //oTable.Range.ParagraphFormat.SpaceAfter = 2;//表格里面的内容段落后空格

                        //填充表格内容
                        oTable.Cell(1, 1).Range.Text = "序号"; //在表格的第一行第一列填入内容。                
                        oTable.Cell(1, 2).Range.Text = "列名";
                        oTable.Cell(1, 3).Range.Text = "数据类型";
                        oTable.Cell(1, 4).Range.Text = "长度";
                        oTable.Cell(1, 5).Range.Text = "小数位";
                        oTable.Cell(1, 6).Range.Text = "标识";
                        oTable.Cell(1, 7).Range.Text = "主键";
                        oTable.Cell(1, 8).Range.Text = "允许空";
                        oTable.Cell(1, 9).Range.Text = "默认值";
                        oTable.Cell(1, 10).Range.Text = "说明";

                        oTable.Columns[1].Width = 33;
                        //oTable.Columns[2].Width=80;
                        oTable.Columns[3].Width = 60;
                        oTable.Columns[4].Width = 33;
                        oTable.Columns[5].Width = 43;
                        oTable.Columns[6].Width = 33;
                        oTable.Columns[7].Width = 33;
                        oTable.Columns[8].Width = 43;
                        //tbl.Columns[9].Width=80;

                        int r, c;
                        string strText;
                        for (r = 0; r < rc; r++)
                        {
                            ColumnInfo col = (ColumnInfo)collist[r];
                            string order = col.ColumnOrder;
                            string colum = col.ColumnName;
                            string typename = col.TypeName;
                            string length = col.Length;
                            string scale = col.Scale;
                            string IsIdentity = col.IsIdentity.ToString().ToLower() == "true" ? "是" : "";
                            string PK = col.IsPrimaryKey.ToString().ToLower() == "true" ? "是" : "";
                            string isnull = col.Nullable.ToString().ToLower() == "true" ? "是" : "否";
                            string defaultstr = col.DefaultVal.ToString();
                            string description = col.Description.ToString();
                            if (length.Trim() == "-1")
                            {
                                length = "MAX";
                            }

                            oTable.Cell(r + 2, 1).Range.Text = order;
                            oTable.Cell(r + 2, 2).Range.Text = colum;
                            oTable.Cell(r + 2, 3).Range.Text = typename;
                            oTable.Cell(r + 2, 4).Range.Text = length;
                            oTable.Cell(r + 2, 5).Range.Text = scale;
                            oTable.Cell(r + 2, 6).Range.Text = IsIdentity;
                            oTable.Cell(r + 2, 7).Range.Text = PK;
                            oTable.Cell(r + 2, 8).Range.Text = isnull;
                            oTable.Cell(r + 2, 9).Range.Text = defaultstr;
                            oTable.Cell(r + 2, 10).Range.Text = description;
                        }
                        oTable.Rows[1].Range.Font.Bold = 1;
                        oTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                        //oTable.Rows[1].Range.Font.Italic = 1;
                        //oTable.Columns[1].Width = oWord.InchesToPoints(2); //Change width of columns 1 & 2
                        //oTable.Columns[2].Width = oWord.InchesToPoints(3);
                        oTable.Rows.First.Shading.Texture = Word.WdTextureIndex.wdTexture25Percent;//设置阴影
                        //oTable.Rows.First.Range.Font.Bold = 1;
                        //oTable.Rows.First.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                    }
                    #endregion

                    SetprogressBar1Val(i + 1);
                    SetlblStatuText("已生成" + (i + 1).ToString());

                    i++;
                }
                #endregion

                oWord.Visible = true;
                oDoc.Activate();

                //SetBtnEnable();
                MessageBox.Show("文档生成成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show("文档生成失败！(" + ex.Message + ")。\r\n请关闭重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        #endregion

        
        #region 异步方法
        public void SetlblStatuText(string text)
        {
            if (this.lblTip.InvokeRequired)
            {
                SetlblStatuCallback d = new SetlblStatuCallback(SetlblStatuText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblTip.Text = text;
            }
        }
        /// <summary>
        /// 循环网址进度最大值
        /// </summary>
        /// <param name="val"></param>
        public void SetprogressBar1Max(int val)
        {
            if (this.progressBar1.InvokeRequired)
            {
                SetProBar1MaxCallback d = new SetProBar1MaxCallback(SetprogressBar1Max);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.progressBar1.Maximum = val;

            }
        }
        /// <summary>
        /// 循环网址进度
        /// </summary>
        /// <param name="val"></param>
        public void SetprogressBar1Val(int val)
        {
            if (this.progressBar1.InvokeRequired)
            {
                SetProBar1ValCallback d = new SetProBar1ValCallback(SetprogressBar1Val);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.progressBar1.Value = val;

            }
        }
        #endregion

        #region 操作

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 全部表名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Maticsoft.Utility.StringPlus str = new Maticsoft.Utility.StringPlus();
            List<string> tablist = new List<string>();
            foreach (ListViewItem item in this.listView1.Items)
            {
                string tablename = item.SubItems[1].Text;
                if (!tablist.Contains(tablename))
                {
                    tablist.Add(tablename);
                    str.AppendLine(tablename);
                }
            }
            Clipboard.SetDataObject(str.Value);

        }

        private void 所选表名ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Maticsoft.Utility.StringPlus str = new Maticsoft.Utility.StringPlus();
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                str.AppendLine(item.SubItems[1].Text);
            }
            Clipboard.SetDataObject(str.Value);
        }

        private void 所选字段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Maticsoft.Utility.StringPlus str = new Maticsoft.Utility.StringPlus();
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                str.AppendLine(item.SubItems[2].Text);
            }
            Clipboard.SetDataObject(str.Value);
        }

        private void 全部字段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Maticsoft.Utility.StringPlus str = new Maticsoft.Utility.StringPlus();
            List<string> list = new List<string>();
            foreach (ListViewItem item in this.listView1.Items)
            {
                string colname = item.SubItems[2].Text;
                if (!list.Contains(colname))
                {
                    list.Add(colname);
                    str.AppendLine(colname);
                }
            }
            Clipboard.SetDataObject(str.Value);
        }

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                listView1.Items.Remove(item);
            }
        }

        private void 代码生成器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (longServername == "" || currentDbname=="")
                return;
            if (listView1.SelectedItems.Count > 0)
            {
                string tablename = listView1.SelectedItems[0].SubItems[1].Text;
                mainfrm.AddSinglePage(new CodeMaker(longServername, currentDbname, tablename), "代码生成器");
                this.Close();
            }
        }

        #endregion

    }
}
