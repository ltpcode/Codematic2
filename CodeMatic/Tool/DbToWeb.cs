using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Threading;
using Maticsoft.CodeHelper;

namespace Codematic
{
    public partial class DbToWeb : Form
    {
        Loading load = new Loading();
        Thread mythread;
        
        Maticsoft.IDBO.IDbObject dbobj;
        delegate void SetBtnEnableCallback();
        delegate void SetBtnDisableCallback();
        delegate void SetlblStatuCallback(string text);
        delegate void SetProBar1MaxCallback(int val);
        delegate void SetProBar1ValCallback(int val);
        Maticsoft.CmConfig.DbSettings dbset;       
        private Label label5;
        string dbname = "";

        public DbToWeb(string longservername)
        {
            InitializeComponent();
            dbset = Maticsoft.CmConfig.DbConfig.GetSetting(longservername);
            dbobj = Maticsoft.DBFactory.DBOMaker.CreateDbObj(dbset.DbType);
            dbobj.DbConnectStr = dbset.ConnectStr;
            this.lblServer.Text = dbset.Server;
        }

        private void DbToWeb_Load(object sender, EventArgs e)
        {
            this.btn_Creat.Enabled = false;
            
            ThreadWorkLoad();
            comboBox1.SelectedIndex = 0;
        }


        void ThreadWorkLoad()
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
            else
            {
                List<string> tabNames = dbobj.GetTables("");
                this.listTable1.Items.Clear();
                this.listTable2.Items.Clear();
                if (tabNames.Count > 0)
                {
                    SetprogressBar1Max(tabNames.Count);
                    SetprogressBar1Val(1);
                    SetlblStatuText("");
                    int s = 1;
                    foreach (string tabname in tabNames)
                    {
                        listTable1.Items.Add(tabname);
                        SetprogressBar1Val(s);
                        SetlblStatuText(tabname);
                    }
                }
            }                     
        }
        
        private void cmbDB_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string dbname = cmbDB.Text;
            List<string> tabNames = dbobj.GetTables(dbname);

            this.listTable1.Items.Clear();
            this.listTable2.Items.Clear();
            if (tabNames.Count > 0)
            {
                foreach (string tabname in tabNames)
                {
                    listTable1.Items.Add(tabname);
                }
            }

            IsHasItem();
        }



        #region 按钮操作

        private void btn_Addlist_Click(object sender, System.EventArgs e)
        {
            int c = this.listTable1.Items.Count;
            for (int i = 0; i < c; i++)
            {
                this.listTable2.Items.Add(this.listTable1.Items[i]);
            }
            this.listTable1.Items.Clear();

            IsHasItem();
        }

        private void btn_Add_Click(object sender, System.EventArgs e)
        {

            int c = this.listTable1.SelectedItems.Count;
            ListBox.SelectedObjectCollection objs = this.listTable1.SelectedItems;
            for (int i = 0; i < c; i++)
            {
                this.listTable2.Items.Add(listTable1.SelectedItems[i]);

            }
            for (int i = 0; i < c; i++)
            {
                if (this.listTable1.SelectedItems.Count > 0)
                {
                    this.listTable1.Items.Remove(listTable1.SelectedItems[0]);
                }
            }
            IsHasItem();
        }

        private void btn_Del_Click(object sender, System.EventArgs e)
        {
            int c = this.listTable2.SelectedItems.Count;
            ListBox.SelectedObjectCollection objs = this.listTable2.SelectedItems;
            for (int i = 0; i < c; i++)
            {
                this.listTable1.Items.Add(listTable2.SelectedItems[i]);

            }
            for (int i = 0; i < c; i++)
            {
                if (this.listTable2.SelectedItems.Count > 0)
                {
                    this.listTable2.Items.Remove(listTable2.SelectedItems[0]);
                }
            }

            IsHasItem();
        }

        private void btn_Dellist_Click(object sender, System.EventArgs e)
        {
            int c = this.listTable2.Items.Count;
            for (int i = 0; i < c; i++)
            {
                this.listTable1.Items.Add(this.listTable2.Items[i]);
            }
            this.listTable2.Items.Clear();
            IsHasItem();
        }
        #endregion

        #region listbox操作
        private void listTable1_Click(object sender, EventArgs e)
        {
            if (this.listTable1.SelectedItem != null)
            {
                IsHasItem();
            }
        }

        private void listTable1_DoubleClick(object sender, System.EventArgs e)
        {
            if (this.listTable1.SelectedItem != null)
            {
                this.listTable2.Items.Add(listTable1.SelectedItem);
                this.listTable1.Items.Remove(this.listTable1.SelectedItem);
                IsHasItem();
            }
        }

        private void listTable2_DoubleClick(object sender, System.EventArgs e)
        {
            if (this.listTable2.SelectedItem != null)
            {
                this.listTable1.Items.Add(listTable2.SelectedItem);
                this.listTable2.Items.Remove(this.listTable2.SelectedItem);
                IsHasItem();
            }
        }

        /// <summary>
        /// 判断listbox有没有项目
        /// </summary>
        private void IsHasItem()
        {
            if (this.listTable1.Items.Count > 0)
            {
                this.btn_Add.Enabled = true;
                this.btn_Addlist.Enabled = true;
            }
            else
            {
                this.btn_Add.Enabled = false;
                this.btn_Addlist.Enabled = false;
            }
            if (this.listTable2.Items.Count > 0)
            {
                this.btn_Del.Enabled = true;
                this.btn_Dellist.Enabled = true;
                this.btn_Creat.Enabled = true;
            }
            else
            {
                this.btn_Del.Enabled = false;
                this.btn_Dellist.Enabled = false;
                this.btn_Creat.Enabled = false;
            }
        }
        #endregion

        #region 异步控件设置
        public void SetBtnEnable()
        {
            if (this.btn_Creat.InvokeRequired)
            {
                SetBtnEnableCallback d = new SetBtnEnableCallback(SetBtnEnable);
                this.Invoke(d, null);
            }
            else
            {
                this.btn_Creat.Enabled = true;
                //this.btn_Cancle.Enabled = true;
            }
        }
        public void SetBtnDisable()
        {
            if (this.btn_Creat.InvokeRequired)
            {
                SetBtnDisableCallback d = new SetBtnDisableCallback(SetBtnDisable);
                this.Invoke(d, null);
            }
            else
            {
                this.btn_Creat.Enabled = false;
                //this.btn_Cancle.Enabled = false;
            }
        }
        public void SetlblStatuText(string text)
        {
            if (this.labelNum.InvokeRequired)
            {
                SetlblStatuCallback d = new SetlblStatuCallback(SetlblStatuText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.labelNum.Text = text;
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

        private void btn_Cancle_Click(object sender, System.EventArgs e)
        {
            
            this.Close();
        }

        #region 生成word
        private void btn_Creat_Click(object sender, System.EventArgs e)
        {
            try
            {
                dbname = this.cmbDB.Text;
                pictureBox1.Visible = true;
                mythread = new Thread(new ThreadStart(ThreadWorkhtml));
                mythread.Start();
            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

       


        #region 生成html格式
        void ThreadWorkhtml()
        {
            try
            {
                SetBtnDisable();
                string strtitle = "数据库名：" + dbname;
                int tblCount = this.listTable2.Items.Count;

                SetprogressBar1Max(tblCount);
                SetprogressBar1Val(1);
                SetlblStatuText("0");

                StringBuilder htmlBody = new StringBuilder();


                #region 产生文档，写入标题

                htmlBody.Append("<div class=\"styledb\">"+strtitle+"</div>");
               
                #endregion

                #region 表的扩展属性

                DataTable dtEx = dbobj.GetTablesExProperty(dbname);
           
                #endregion

                #region 循环每个表

                for (int i = 0; i < tblCount; i++)
                {
                    string tablename = this.listTable2.Items[i].ToString();                 
                    string tabletitle = "表名：" + tablename;

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


                    #region 循环每一个列，产生一行数据

                    List<ColumnInfo> collist = dbobj.GetColumnInfoList(dbname, tablename);
                    int rc = collist.Count;
                    if ((collist != null) && (collist.Count > 0))
                    {
                        htmlBody.Append("<div class=\"styletab\">" + tabletitle + "</div>");
                        htmlBody.Append("<div  align=\"left\">" + tabdesc + "</div>");
                        htmlBody.Append("<div><table border=\"0\" cellpadding=\"5\" cellspacing=\"0\" width=\"90%\">");
                        
                        if (comboBox1.SelectedIndex==0)
                        {                           
                            htmlBody.Append("<tr><td bgcolor=\"#FBFBFB\">");
                            htmlBody.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" width=\"100%\" bordercolorlight=\"#D7D7E5\" bordercolordark=\"#D3D8E0\">");
                            htmlBody.Append("<tr bgcolor=\"#F0F0F0\">");
                        }
                        else
                        {
                            htmlBody.Append("<tr><td bgcolor=\"#F5F9FF\">");
                            htmlBody.Append("<table cellspacing=\"0\" cellpadding=\"5\" border=\"1\" width=\"100%\" bordercolorlight=\"#4F7FC9\" bordercolordark=\"#D3D8E0\">");
                            htmlBody.Append("<tr bgcolor=\"#E3EFFF\">");
                        }
                        
                        htmlBody.Append("<td>序号</td>");
                        htmlBody.Append("<td>列名</td>");
                        htmlBody.Append("<td>数据类型</td>");
                        htmlBody.Append("<td>长度</td>");
                        htmlBody.Append("<td>小数位</td>");
                        htmlBody.Append("<td>标识</td>");
                        htmlBody.Append("<td>主键</td>");
                        htmlBody.Append("<td>允许空</td>");
                        htmlBody.Append("<td>默认值</td>");
                        htmlBody.Append("<td>说明</td>");
                        htmlBody.Append("</tr>");

                        int r, c;
                        //string strText;
                        for (r = 0; r < rc; r++)
                        {
                            ColumnInfo col = (ColumnInfo)collist[r];
                            string order = col.ColumnOrder;
                            string colum = col.ColumnName;
                            string typename = col.TypeName;
                            string length = col.Length == "" ? "&nbsp;" : col.Length;
                            string scale = col.Scale == "" ? "&nbsp;" : col.Scale;
                            string IsIdentity = col.IsIdentity.ToString().ToLower() == "true" ? "是" : "&nbsp;";
                            string PK = col.IsPrimaryKey.ToString().ToLower() == "true" ? "是" : "&nbsp;";
                            string isnull = col.Nullable.ToString().ToLower() == "true" ? "是" : "否";
                            string defaultstr = col.DefaultVal.ToString().Trim() == "" ? "&nbsp;" : col.DefaultVal.ToString();
                            string description = col.Description.ToString().Trim() == "" ? "&nbsp;" : col.Description.ToString();
                            if (length.Trim() == "-1")
                            {
                                length = "MAX";
                            }

                            htmlBody.Append("<tr>");
                            htmlBody.Append("<td>" + order + "</td>");
                            htmlBody.Append("<td>" + colum + "</td>");
                            htmlBody.Append("<td>" + typename + "</td>");
                            htmlBody.Append("<td>" + length + "</td>");
                            htmlBody.Append("<td>" + scale + "</td>");
                            htmlBody.Append("<td>" + IsIdentity + "</td>");
                            htmlBody.Append("<td>" + PK + "</td>");
                            htmlBody.Append("<td>" + isnull + "</td>");
                            htmlBody.Append("<td>" + defaultstr + "</td>");
                            htmlBody.Append("<td align=\"left\" >" + description + "</td>");
                            htmlBody.Append("</tr>");
                                                      
                        }
                        
                    }
                    htmlBody.Append("</table>");
                    htmlBody.Append("</td>");
                    htmlBody.Append("</tr>");
                    htmlBody.Append("</table>");
                    htmlBody.Append("</div>");

                    #endregion

                    SetprogressBar1Val(i + 1);
                    SetlblStatuText((i + 1).ToString());
                }               
                #endregion

                string tempstr = "";
                string temphtml = Application.StartupPath + @"\Template\table.htm";
                if (File.Exists(temphtml))
                {
                    using (StreamReader sr = new StreamReader(temphtml, Encoding.Default))
                    {
                        tempstr = sr.ReadToEnd().Replace("<$$tablestruct$$>", htmlBody.ToString());
                        sr.Close();
                    }

                    SaveFileDialog savedlg = new SaveFileDialog();
                    savedlg.Title = "保存表结构";
                    savedlg.Filter = "htm files (*.htm)|*.htm|All files (*.*)|*.*";
                    DialogResult dlgresult = savedlg.ShowDialog(this);
                    if (dlgresult == DialogResult.OK)
                    {
                        string filename = savedlg.FileName;                        
                        StreamWriter sw = new StreamWriter(filename, false, Encoding.Default);//,false);
                        sw.Write(tempstr);
                        sw.Flush();
                        sw.Close();
                    }                    
                }                
                SetBtnEnable();
                MessageBox.Show(this,"文档生成成功！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (System.Exception ex)
            {
                LogInfo.WriteLog(ex);
                MessageBox.Show(this,"文档生成失败！(" + ex.Message + ")。\r\n请关闭重试。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        #endregion 

        

       
  

        #endregion


    }
}
