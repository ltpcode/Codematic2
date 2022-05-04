using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Maticsoft.CodeHelper;
namespace Codematic
{
    public partial class DbBrowser : Form
    {
        Maticsoft.IDBO.IDbObject dbobj;
        public DbBrowser()
        {
            InitializeComponent();
            DbView dbviewfrm = (DbView)Application.OpenForms["DbView"];
            SetListView(dbviewfrm);
        }

        public void SetListView(DbView dbviewfrm)
        {
            #region 得到类型对象
            TreeNode SelNode = dbviewfrm.treeView1.SelectedNode;
            if (SelNode == null)
                return;
            string servername = "";
            switch (SelNode.Tag.ToString())
            {
                case "serverlist":
                    return;
                case "server":
                    {
                        servername = SelNode.Text;
                        CreatDbObj(servername);
                        #region listView1

                        this.lblViewInfo.Text = " 服务器：" + servername;
                        this.lblNum.Text = SelNode.Nodes.Count.ToString() + "项";
                        this.listView1.Columns.Clear();
                        this.listView1.Items.Clear();
                        this.listView1.LargeImageList =this.imglistDB;
                        //this.listView1.SmallImageList = imglistView;
                        this.listView1.View = View.LargeIcon;
                        foreach (TreeNode node in SelNode.Nodes)
                        {
                            string dbname = node.Text;
                            ListViewItem item1 = new ListViewItem(dbname, 0);
                            item1.SubItems.Add(dbname);
                            item1.ImageIndex = 0;
                            listView1.Items.AddRange(new ListViewItem[] { item1 });
                        }
                        SetListViewMenu("db");
                        #endregion
                    }
                    break;
                case "db":
                    {
                        servername = SelNode.Parent.Text;
                        CreatDbObj(servername);
                        #region
                        this.lblViewInfo.Text = " 数据库：" + SelNode.Text;
                        this.lblNum.Text = SelNode.Nodes.Count.ToString() + "项";                        
                        SetListViewMenu("table");
                        BindlistViewTab(SelNode.Text, SelNode.Tag.ToString());
                        #endregion
                    }
                    break;
                case "tableroot":
                case "viewroot":
                case "procroot":
                    {
                        servername = SelNode.Parent.Parent.Text;
                        string dbname = SelNode.Parent.Text;
                        CreatDbObj(servername);

                        #region
                        this.lblViewInfo.Text = " 数据库：" + dbname;
                        this.lblNum.Text = SelNode.Nodes.Count.ToString() + "项";
                                                                
                        SetListViewMenu("table");
                        BindlistViewTab(dbname, SelNode.Tag.ToString());
                        #endregion
                    }
                    break;                    
                case "table":
                case "view":
                    {
                        servername = SelNode.Parent.Parent.Parent.Text;
                        string dbname = SelNode.Parent.Parent.Text;
                        string tabname = SelNode.Text;
                        CreatDbObj(servername);                       

                        #region

                        this.lblViewInfo.Text = " 表：" + tabname;
                        this.lblNum.Text = SelNode.Nodes.Count.ToString() + "项";

                        SetListViewMenu("column");
                        BindlistViewCol(dbname, tabname);

                        #endregion
                    }
                    break;
                case "proc":
                    {
                        servername = SelNode.Parent.Parent.Parent.Text;
                        string dbname = SelNode.Parent.Parent.Text;
                        string tabname = SelNode.Text;
                        CreatDbObj(servername);

                        #region

                        this.lblViewInfo.Text = " 存储过程：" + tabname;
                        this.lblNum.Text = SelNode.Nodes.Count.ToString() + "项";

                        //SetListViewMenu("column");
                        //BindlistViewCol(dbname, tabname);
                        //this.listView1.Columns.Clear();
                        this.listView1.Items.Clear();

                        #endregion
                    }
                    break;
                case "column":
                    servername = SelNode.Parent.Parent.Parent.Parent.Text;
                    break;
            }            
            #endregion
            
        }

        private void CreatDbObj(string servername)
        {
            try
            {
                Maticsoft.CmConfig.DbSettings dbset = Maticsoft.CmConfig.DbConfig.GetSetting(servername);
                dbobj = Maticsoft.DBFactory.DBOMaker.CreateDbObj(dbset.DbType);
                dbobj.DbConnectStr = dbset.ConnectStr;
            }
            catch
            {
                MessageBox.Show("加载数据库配置失败，可能是由于配置文件版本不一致所致。请注销该服务器，重新注册添加再试！");
            }
        }

       

        #region 为listView邦定 表 数据
        private void BindlistViewTab(string Dbname, string SelNodeType)
        {
           // SetListViewMenu("table");
           
            this.listView1.Columns.Clear();
            this.listView1.Items.Clear();
            this.listView1.LargeImageList = imglistView;
            this.listView1.SmallImageList = imglistView;
            this.listView1.View = View.Details;
            this.listView1.FullRowSelect = true;           
            listView1.Columns.Add("名称", 250, HorizontalAlignment.Left);
            listView1.Columns.Add("所有者", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("类型", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("创建日期", 200, HorizontalAlignment.Left);

            List<TableInfo> tablist = null;
            switch (SelNodeType)
            {
                case "db":
                    tablist = dbobj.GetTabViewsInfo(Dbname);
                    break;
                case "tableroot":
                    tablist = dbobj.GetTablesInfo(Dbname);
                    break;
                case "viewroot":
                    tablist = dbobj.GetVIEWsInfo(Dbname);
                    break;
                case "procroot":
                    tablist = dbobj.GetProcInfo(Dbname);
                    break;
            }
            if ((tablist!=null)&&(tablist.Count > 0))
            {
                foreach (TableInfo tab in tablist)
                {
                    string name = tab.TabName;
                    ListViewItem item1 = new ListViewItem(name, 0);

                    string user =tab.TabUser;
                    item1.SubItems.Add(user);

                    string type = tab.TabType;
                    switch (type.Trim())
                    {
                        case "S":
                            type = "系统";
                            break;
                        case "U":
                            type = "用户";
                            item1.ImageIndex = 2;
                            break;
                        case "TABLE":
                            type = "表";
                            item1.ImageIndex = 2;
                            break;
                        case "V":
                        case "VIEW":
                            type = "视图";
                            item1.ImageIndex = 3;
                            break;
                        case "P":
                            type = "存储过程";
                            item1.ImageIndex = 5;
                            break;
                        default:
                            type = "系统";
                            break;

                    }
                    item1.SubItems.Add(type);
                    string time = tab.TabDate;
                    item1.SubItems.Add(time);

                    listView1.Items.AddRange(new ListViewItem[] { item1 });

                }
            }

            #region
            //if (dt != null)
            //{
            //    DataRow[] dRows = dt.Select("", "type,name ASC");
            //    foreach (DataRow row in dRows)              
            //    {
            //        string name = row["name"].ToString();
            //        ListViewItem item1 = new ListViewItem(name, 0);

            //        string user = row["cuser"].ToString();
            //        item1.SubItems.Add(user);

            //        string type = row["type"].ToString();
            //        switch (type.Trim())
            //        {
            //            case "S":
            //                type = "系统";
            //                break;
            //            case "U":
            //                type = "用户";
            //                item1.ImageIndex = 2;
            //                break;
            //            case "TABLE":
            //                type = "表";
            //                item1.ImageIndex = 2;
            //                break;
            //            case "V":
            //            case "VIEW":
            //                type = "视图";
            //                item1.ImageIndex = 3;
            //                break;
            //            case "P":
            //                type = "存储过程";
            //                item1.ImageIndex = 5;
            //                break;
            //            default:
            //                type = "系统";
            //                break;

            //        }
            //        item1.SubItems.Add(type);
            //        string time = row["dates"].ToString();
            //        item1.SubItems.Add(time);

            //        listView1.Items.AddRange(new ListViewItem[] { item1 });

            //    }
            //}
            #endregion

        }

        #endregion

        #region  为listView邦定 列 数据

        private void BindlistViewCol(string Dbname, string TableName)
        {
            SetListViewMenu("colum");
            //创建列表
            this.listView1.Columns.Clear();
            this.listView1.Items.Clear();
            this.listView1.LargeImageList = imglistView;
            this.listView1.SmallImageList = imglistView;            
            this.listView1.View = View.Details;
            this.listView1.GridLines = true;
            this.listView1.FullRowSelect = true;
            
            listView1.Columns.Add("序号", 60, HorizontalAlignment.Left);
            listView1.Columns.Add("列名", 110, HorizontalAlignment.Left);
            listView1.Columns.Add("数据类型", 80, HorizontalAlignment.Left);
            listView1.Columns.Add("长度", 45, HorizontalAlignment.Left);
            listView1.Columns.Add("小数", 45, HorizontalAlignment.Left);
            listView1.Columns.Add("标识", 45, HorizontalAlignment.Center);
            listView1.Columns.Add("主键", 45, HorizontalAlignment.Center);
            listView1.Columns.Add("外键", 45, HorizontalAlignment.Center);
            listView1.Columns.Add("允许空", 60, HorizontalAlignment.Center);
            listView1.Columns.Add("默认值", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("字段说明", 100, HorizontalAlignment.Left);

            List<ColumnInfo> collist = dbobj.GetColumnInfoList(Dbname, TableName);
            if ((collist!=null)&&(collist.Count > 0))
            {                
                foreach (ColumnInfo col in collist)
                {
                    string order = col.ColumnOrder;
                    string columnName = col.ColumnName;
                    string columnType = col.TypeName;
                    string Length = col.Length;
                    switch (columnType)
                    {                        
                        
                        case "char":
                        case "nchar":
                        case "binary":   
                            {
                                Length = CodeCommon.GetDataTypeLenVal(columnType, Length);                                
                            }
                            break;
                        case "varchar":
                        case "nvarchar":                       
                        case "varbinary":
                            {
                                Length = CodeCommon.GetDataTypeLenVal(columnType.Trim().ToLower(), Length);
                                if (Length.Length == 0 || Length == "0" || Length == "-1")
                                {
                                    Length = "MAX";
                                }                                
                            }
                            break;
                        default:                          
                            break;
                    }

                    string Preci = col.Precision;
                    string Scale = col.Scale;
                    string defaultVal = col.DefaultVal;
                    string description = col.Description;
                    string IsIdentity = (col.IsIdentity) ? "√" : "";
                    string ispk = (col.IsPrimaryKey) ? "√" : "";
                    string isfk = (col.IsForeignKey) ? "√" : "";
                    string isnull = (col.Nullable) ? "√" : "";

                    ListViewItem item1 = new ListViewItem(order, 0);
                    item1.ImageIndex = 4;
                    item1.SubItems.Add(columnName);
                    item1.SubItems.Add(columnType);
                    item1.SubItems.Add(Length);
                    item1.SubItems.Add(Scale);
                    item1.SubItems.Add(IsIdentity);
                    if ((ispk == "√") && (isnull.Trim() == ""))
                    {
                    }
                    else
                    {
                        ispk = "";
                    }
                    item1.SubItems.Add(ispk);
                    if (isfk != "√")
                    {                    
                        isfk = "";
                    }
                    item1.SubItems.Add(isfk);
                    item1.SubItems.Add(isnull);
                    item1.SubItems.Add(defaultVal);
                    item1.SubItems.Add(description);

                    listView1.Items.AddRange(new ListViewItem[] { item1 });

                }
            }
        }
        #endregion
        
        #region 设定listview右键菜单
        private void SetListViewMenu(string itemType)
        {
            switch (itemType.ToLower())
            {
                case "server":
                    {                        
                    }
                    break;
                case "db":
                    {
                    }
                    break;
                case "table":
                    {                        
                    }
                    break;
                case "column":
                    {
                    }
                    break;
                default:
                    {                       
                    }
                    break;
            }
        }
        #endregion

        

    }
}