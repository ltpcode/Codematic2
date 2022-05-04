using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Xml;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Codematic.UserControls
{
    public partial class UcDatatypeMap : UserControl
    {
        string filename = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\DatatypeMap.cfg";

        public UcDatatypeMap()
        {
            InitializeComponent();
            CreatListView();
            LoadData();
            
        }

        #region 初始化
        private void CreatListView()
        {
            //创建列表
            this.listView_DbToCS.Columns.Clear();
            this.listView_DbToCS.Items.Clear();
            this.listView_DbToCS.View = View.Details;
            this.listView_DbToCS.GridLines = true;
            this.listView_DbToCS.FullRowSelect = true;
            this.listView_DbToCS.CheckBoxes = false;
            //listView_DbToCS.Columns.Add("选择", 40, HorizontalAlignment.Left);
            listView_DbToCS.Columns.Add("DB类型", 200, HorizontalAlignment.Left);
            listView_DbToCS.Columns.Add("对应C#类型", 200, HorizontalAlignment.Left);

            this.listView_AccessDbType.Columns.Clear();
            this.listView_AccessDbType.Items.Clear();
            this.listView_AccessDbType.View = View.Details;
            this.listView_AccessDbType.GridLines = true;
            this.listView_AccessDbType.FullRowSelect = true;
            this.listView_AccessDbType.CheckBoxes = false;
            //listView_AccessDbType.Columns.Add("选择", 40, HorizontalAlignment.Left);
            listView_AccessDbType.Columns.Add("数字编号", 200, HorizontalAlignment.Left);
            listView_AccessDbType.Columns.Add("对应类型", 200, HorizontalAlignment.Left);

            this.listView_AddMark.Columns.Clear();
            this.listView_AddMark.Items.Clear();
            this.listView_AddMark.View = View.Details;
            this.listView_AddMark.GridLines = true;
            this.listView_AddMark.FullRowSelect = true;
            this.listView_AddMark.CheckBoxes = false;
            //listView_AddMark.Columns.Add("选择", 40, HorizontalAlignment.Left);
            listView_AddMark.Columns.Add("数据类型", 200, HorizontalAlignment.Left);
            listView_AddMark.Columns.Add("SQL语句中加引号", 200, HorizontalAlignment.Left);


            this.listView_MySQLProc.Columns.Clear();
            this.listView_MySQLProc.Items.Clear();
            this.listView_MySQLProc.View = View.Details;
            this.listView_MySQLProc.GridLines = true;
            this.listView_MySQLProc.FullRowSelect = true;
            this.listView_MySQLProc.CheckBoxes = false;
            //listView_MySQLProc.Columns.Add("选择", 40, HorizontalAlignment.Left);
            listView_MySQLProc.Columns.Add("类型", 200, HorizontalAlignment.Left);
            listView_MySQLProc.Columns.Add("对应MySqlDbType类型", 200, HorizontalAlignment.Left);


            this.listView_OledbProc.Columns.Clear();
            this.listView_OledbProc.Items.Clear();
            this.listView_OledbProc.View = View.Details;
            this.listView_OledbProc.GridLines = true;
            this.listView_OledbProc.FullRowSelect = true;
            this.listView_OledbProc.CheckBoxes = false;
            //listView_OledbProc.Columns.Add("选择", 40, HorizontalAlignment.Left);
            listView_OledbProc.Columns.Add("类型", 200, HorizontalAlignment.Left);
            listView_OledbProc.Columns.Add("对应OleDbType类型", 200, HorizontalAlignment.Left);

            this.listView_OraProc.Columns.Clear();
            this.listView_OraProc.Items.Clear();
            this.listView_OraProc.View = View.Details;
            this.listView_OraProc.GridLines = true;
            this.listView_OraProc.FullRowSelect = true;
            this.listView_OraProc.CheckBoxes = false;
            //listView_OraProc.Columns.Add("选择", 40, HorizontalAlignment.Left);
            listView_OraProc.Columns.Add("类型", 200, HorizontalAlignment.Left);
            listView_OraProc.Columns.Add("对应OracleType类型", 200, HorizontalAlignment.Left);

            this.listView_SQLiteProc.Columns.Clear();
            this.listView_SQLiteProc.Items.Clear();
            this.listView_SQLiteProc.View = View.Details;
            this.listView_SQLiteProc.GridLines = true;
            this.listView_SQLiteProc.FullRowSelect = true;
            this.listView_SQLiteProc.CheckBoxes = false;
            //listView_SQLiteProc.Columns.Add("选择", 40, HorizontalAlignment.Left);
            listView_SQLiteProc.Columns.Add("类型", 200, HorizontalAlignment.Left);
            listView_SQLiteProc.Columns.Add("对应DbType类型", 200, HorizontalAlignment.Left);

            this.listView_SQLProc.Columns.Clear();
            this.listView_SQLProc.Items.Clear();
            this.listView_SQLProc.View = View.Details;
            this.listView_SQLProc.GridLines = true;
            this.listView_SQLProc.FullRowSelect = true;
            this.listView_SQLProc.CheckBoxes = false;
            //listView_SQLProc.Columns.Add("选择", 40, HorizontalAlignment.Left);
            listView_SQLProc.Columns.Add("类型", 200, HorizontalAlignment.Left);
            listView_SQLProc.Columns.Add("对应SqlDbType类型", 200, HorizontalAlignment.Left);


            this.listView_ValueType.Columns.Clear();
            this.listView_ValueType.Items.Clear();
            this.listView_ValueType.View = View.Details;
            this.listView_ValueType.GridLines = true;
            this.listView_ValueType.FullRowSelect = true;
            this.listView_ValueType.CheckBoxes = false;
            //listView_ValueType.Columns.Add("选择", 40, HorizontalAlignment.Left);
            listView_ValueType.Columns.Add("类型", 200, HorizontalAlignment.Left);
            listView_ValueType.Columns.Add("是否值类型", 200, HorizontalAlignment.Left);

            this.listViewParamePrefix.Columns.Clear();
            this.listViewParamePrefix.Items.Clear();
            this.listViewParamePrefix.View = View.Details;
            this.listViewParamePrefix.GridLines = true;
            this.listViewParamePrefix.FullRowSelect = true;
            this.listViewParamePrefix.CheckBoxes = false;            
            listViewParamePrefix.Columns.Add("数据库", 200, HorizontalAlignment.Left);
            listViewParamePrefix.Columns.Add("对应参数符号", 200, HorizontalAlignment.Left);

        }

        private void LoadData()
        {
            if (!File.Exists(filename))
                return;
            XmlDocument xmldoc = new System.Xml.XmlDocument();
            xmldoc.Load(filename);
            Hashtable listDbToCS = Maticsoft.CmConfig.DatatypeMap.LoadFromCfg(xmldoc,  "DbToCS");
            Hashtable listSQLDbType = Maticsoft.CmConfig.DatatypeMap.LoadFromCfg(xmldoc,  "SQLDbType");
            Hashtable listOraDbType = Maticsoft.CmConfig.DatatypeMap.LoadFromCfg(xmldoc,  "OraDbType");
            Hashtable listMySQLDbType = Maticsoft.CmConfig.DatatypeMap.LoadFromCfg(xmldoc,  "MySQLDbType");
            Hashtable listOleDbDbType = Maticsoft.CmConfig.DatatypeMap.LoadFromCfg(xmldoc,  "OleDbDbType");
            Hashtable listSQLiteType = Maticsoft.CmConfig.DatatypeMap.LoadFromCfg(xmldoc,  "SQLiteType");
            Hashtable listAddMark = Maticsoft.CmConfig.DatatypeMap.LoadFromCfg(xmldoc,  "AddMark");
            Hashtable listValueType = Maticsoft.CmConfig.DatatypeMap.LoadFromCfg(xmldoc,  "ValueType");
            Hashtable listAccessTypeMap = Maticsoft.CmConfig.DatatypeMap.LoadFromCfg(xmldoc,  "AccessTypeMap");
            Hashtable listParamePrefix = Maticsoft.CmConfig.DatatypeMap.LoadFromCfg(xmldoc, "ParamePrefix");
            
            ListViewItem item;
            foreach (DictionaryEntry de in listDbToCS)
            {
                item = new ListViewItem(de.Key.ToString(), 0);
                //item.SubItems.Add(de.Key.ToString());
                item.SubItems.Add(de.Value.ToString());
                this.listView_DbToCS.Items.AddRange(new ListViewItem[] { item });
            }
            foreach (DictionaryEntry de in listSQLDbType)
            {
                item = new ListViewItem(de.Key.ToString(), 0);
                //item.SubItems.Add(de.Key.ToString());
                item.SubItems.Add(de.Value.ToString());
                this.listView_SQLProc.Items.AddRange(new ListViewItem[] { item });
            }
            foreach (DictionaryEntry de in listParamePrefix)
            {
                item = new ListViewItem(de.Key.ToString(), 0);                
                item.SubItems.Add(de.Value.ToString());
                this.listViewParamePrefix.Items.AddRange(new ListViewItem[] { item });
            }
            foreach (DictionaryEntry de in listOraDbType)
            {
                item = new ListViewItem(de.Key.ToString(), 0);
                //item.SubItems.Add(de.Key.ToString());
                item.SubItems.Add(de.Value.ToString());
                this.listView_OraProc.Items.AddRange(new ListViewItem[] { item });
            }
            foreach (DictionaryEntry de in listMySQLDbType)
            {
                item = new ListViewItem(de.Key.ToString(), 0);
                //item.SubItems.Add(de.Key.ToString());
                item.SubItems.Add(de.Value.ToString());
                this.listView_MySQLProc.Items.AddRange(new ListViewItem[] { item });
            }
            foreach (DictionaryEntry de in listOleDbDbType)
            {
                item = new ListViewItem(de.Key.ToString(), 0);
                //item.SubItems.Add(de.Key.ToString());
                item.SubItems.Add(de.Value.ToString());
                this.listView_OledbProc.Items.AddRange(new ListViewItem[] { item });
            }
            foreach (DictionaryEntry de in listSQLiteType)
            {
                item = new ListViewItem(de.Key.ToString(), 0);
                //item.SubItems.Add(de.Key.ToString());
                item.SubItems.Add(de.Value.ToString());
                this.listView_SQLiteProc.Items.AddRange(new ListViewItem[] { item });
            }
            foreach (DictionaryEntry de in listAddMark)
            {
                item = new ListViewItem(de.Key.ToString(), 0);
                //item.SubItems.Add(de.Key.ToString());
                item.SubItems.Add(de.Value.ToString());
                this.listView_AddMark.Items.AddRange(new ListViewItem[] { item });
            }
            foreach (DictionaryEntry de in listValueType)
            {
                item = new ListViewItem(de.Key.ToString(), 0);                
                item.SubItems.Add(de.Value.ToString());
                this.listView_ValueType.Items.AddRange(new ListViewItem[] { item });
            }
            foreach (DictionaryEntry de in listAccessTypeMap)
            {
                item = new ListViewItem(de.Key.ToString(), 0);                
                item.SubItems.Add(de.Value.ToString());
                this.listView_AccessDbType.Items.AddRange(new ListViewItem[] { item });
            }


        }
        #endregion

        #region 保存
        public void SaveData()
        {            
            try
            {

                XmlDocument xmldoc = new XmlDocument();
                XmlNode xmlnode = xmldoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                xmldoc.AppendChild(xmlnode);


                XmlElement root=null;
                Hashtable list=null;
                root = xmldoc.CreateElement("Map");
                xmldoc.AppendChild(root);
                                

                #region DbToCS
                //list = new Hashtable();
                //root = xmldoc.CreateElement("", "DbToCS", "");
                //xmldoc.AppendChild(root);                
                //foreach (ListViewItem item in listView_DbToCS.Items)
                //{                    
                //    string key = item.SubItems[0].Text;
                //    string value = item.SubItems[1].Text;
                //    if (!list.Contains(key))
                //    {
                //        XmlElement xml = xmldoc.CreateElement("", "DbToCS", "");
                //        XmlAttribute xmlKey = xmldoc.CreateAttribute("key");
                //        xmlKey.Value = key;
                //        xml.Attributes.Append(xmlKey);

                //        XmlAttribute xmlValue = xmldoc.CreateAttribute("value");
                //        xmlValue.Value = value;
                //        xml.Attributes.Append(xmlValue);
                //        root.AppendChild(xml);

                //        list.Add(key, value);
                //    }
                //}
                #endregion

                MakeXmlDoc(xmldoc, list, root, listView_DbToCS, "DbToCS");
                MakeXmlDoc(xmldoc, list, root, listView_SQLProc, "SQLDbType");
                MakeXmlDoc(xmldoc, list, root, listView_OraProc, "OraDbType");
                MakeXmlDoc(xmldoc, list, root, listView_MySQLProc, "MySQLDbType");
                MakeXmlDoc(xmldoc, list, root, listView_OledbProc, "OleDbDbType");
                MakeXmlDoc(xmldoc, list, root, listView_SQLiteProc, "SQLiteType");
                MakeXmlDoc(xmldoc, list, root, listView_AddMark, "AddMark");
                MakeXmlDoc(xmldoc, list, root, listView_ValueType, "ValueType");
                MakeXmlDoc(xmldoc, list, root, listView_AccessDbType, "AccessTypeMap");
                MakeXmlDoc(xmldoc, list, root, listViewParamePrefix, "ParamePrefix");
                
                xmldoc.Save(filename);
                
            }
            catch (System.Exception ex)
            {
                throw new Exception("Save DatatypeMap file fail:" + ex.Message);
            }
 
        }
        private void MakeXmlDoc(XmlDocument xmldoc, Hashtable list, XmlElement root, 
            ListView listview, string NodeText)
        {
            list = new Hashtable();
            XmlElement subroot = xmldoc.CreateElement("", NodeText, "");
            root.AppendChild(subroot);
            foreach (ListViewItem item in listview.Items)
            {
                string key = item.SubItems[0].Text;
                string value = item.SubItems[1].Text;
                if (!list.Contains(key))
                {
                    XmlElement xml = xmldoc.CreateElement("", NodeText, "");

                    XmlAttribute xmlKey = xmldoc.CreateAttribute("key");
                    xmlKey.Value = key;
                    xml.Attributes.Append(xmlKey);

                    XmlAttribute xmlValue = xmldoc.CreateAttribute("value");
                    xmlValue.Value = value;
                    xml.Attributes.Append(xmlValue);

                    subroot.AppendChild(xml);

                    list.Add(key, value);
                }
            }
            //root.AppendChild(subroot);
        }

        private void SaveListView(ListView listview,string NodeText)
        {
            //Hashtable list;
            //foreach (ListViewItem item in listview.Items)
            //{
            //    list = new Hashtable();                
            //    string key = item.SubItems[0].Text;
            //    string value = item.SubItems[1].Text;
            //    if (!list.Contains(key))
            //    {
            //        list.Add(key, value);
            //    }
            //    Maticsoft.CmConfig.DatatypeMap.SaveCfg(filename, NodeText, list);
            //}
        }
        #endregion

        #region button
        private void btnDbToCS_Click(object sender, EventArgs e)
        {
            if (txtDbToCS1.Text.Length > 0 && txtDbToCS2.Text.Length > 0)
            {
                ListViewItem item1 = new ListViewItem(txtDbToCS1.Text, 0);
                //item1.SubItems.Add(txtDbToCS1.Text);
                item1.SubItems.Add(txtDbToCS2.Text);
                this.listView_DbToCS.Items.AddRange(new ListViewItem[] { item1 });
            }
        }

        private void btnValueType_Click(object sender, EventArgs e)
        {
            if (this.txtValueType.Text.Length > 0)
            {
                ListViewItem item1 = new ListViewItem(txtValueType.Text, 0);
                //item1.SubItems.Add(txtValueType.Text);
                item1.SubItems.Add("true");
                this.listView_ValueType.Items.AddRange(new ListViewItem[] { item1 });
            }
        }

        private void btnAddMark_Click(object sender, EventArgs e)
        {
            if (this.txtAddMark.Text.Length > 0)
            {
                ListViewItem item1 = new ListViewItem(txtAddMark.Text, 0);
                //item1.SubItems.Add(txtAddMark.Text);
                item1.SubItems.Add("true");
                this.listView_AddMark.Items.AddRange(new ListViewItem[] { item1 });
            }
        }

        private void btnSQLProc_Click(object sender, EventArgs e)
        {
            if (this.txtSQLProc1.Text.Length > 0 && txtSQLProc2.Text.Length > 0)
            {
                ListViewItem item1 = new ListViewItem(txtSQLProc1.Text, 0);
                //item1.SubItems.Add(txtSQLProc1.Text);
                item1.SubItems.Add(txtSQLProc2.Text);
                this.listView_SQLProc.Items.AddRange(new ListViewItem[] { item1 });
            }
        }

        private void btnOraProc_Click(object sender, EventArgs e)
        {
            if (this.txtOraProc1.Text.Length > 0 && txtOraProc2.Text.Length > 0)
            {
                ListViewItem item1 = new ListViewItem(txtOraProc1.Text, 0);
                //item1.SubItems.Add(txtOraProc1.Text);
                item1.SubItems.Add(txtOraProc2.Text);
                this.listView_OraProc.Items.AddRange(new ListViewItem[] { item1 });
            }
        }

        private void btnMySQLProc_Click(object sender, EventArgs e)
        {
            if (this.txtMySQLProc1.Text.Length > 0 && txtMySQLProc2.Text.Length > 0)
            {
                ListViewItem item1 = new ListViewItem(txtMySQLProc1.Text, 0);
                //item1.SubItems.Add(txtMySQLProc1.Text);
                item1.SubItems.Add(txtMySQLProc2.Text);
                this.listView_MySQLProc.Items.AddRange(new ListViewItem[] { item1 });
            }
        }

        private void btnOledbProc_Click(object sender, EventArgs e)
        {
            if (this.txtOledbProc1.Text.Length > 0 && txtOledbProc2.Text.Length > 0)
            {
                ListViewItem item1 = new ListViewItem(txtOledbProc1.Text, 0);
                //item1.SubItems.Add(txtOledbProc1.Text);
                item1.SubItems.Add(txtOledbProc2.Text);
                this.listView_OledbProc.Items.AddRange(new ListViewItem[] { item1 });
            }
        }

        private void btnSQLiteProc_Click(object sender, EventArgs e)
        {
            if (this.txtSQLiteProc1.Text.Length > 0 && txtSQLiteProc2.Text.Length > 0)
            {
                ListViewItem item1 = new ListViewItem(txtSQLiteProc1.Text, 0);
                //item1.SubItems.Add(txtSQLiteProc1.Text);
                item1.SubItems.Add(txtSQLiteProc2.Text);
                this.listView_SQLiteProc.Items.AddRange(new ListViewItem[] { item1 });
            }
        }

        private void btnAccessDbType_Click(object sender, EventArgs e)
        {
            if (this.txtAccessDbType1.Text.Length > 0 && txtAccessDbType2.Text.Length > 0)
            {
                ListViewItem item1 = new ListViewItem(txtAccessDbType1.Text, 0);                
                item1.SubItems.Add(txtAccessDbType2.Text);
                this.listView_AccessDbType.Items.AddRange(new ListViewItem[] { item1 });
            }
        }
        private void btnParamePrefix_Click(object sender, EventArgs e)
        {
            if (this.txtDBType.Text.Length > 0 && this.txtParamePrefix.Text.Length > 0)
            {
                switch (txtDBType.Text.Trim().ToUpper())
                {
                    case "SQL2000":
                    case "SQL2005":
                    case "SQL2008":
                    case "SQL2012":
                    case "ORACLE":
                    case "OLEDB":
                    case "MYSQL":
                    case "SQLITE":
                        {
                            ListViewItem item1 = new ListViewItem(txtDBType.Text.Trim().ToUpper(), 0);
                            item1.SubItems.Add(txtParamePrefix.Text);
                            this.listViewParamePrefix.Items.AddRange(new ListViewItem[] { item1 });
                        }
                        break;
                    default:
                        MessageBox.Show("不支持该数据库类型或者输入字符串不正确！");
                        break;
                }
                
            }
        }


        #endregion

        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedTab.Name)
            {
                case "tabPageDbToCS":
                    {
                        foreach (ListViewItem item in listView_DbToCS.SelectedItems)
                        {
                            listView_DbToCS.Items.Remove(item);
                        }
                    }
                    break;
                case "tabPageisValueType":
                    {
                        foreach (ListViewItem item in listView_ValueType.SelectedItems)
                        {
                            listView_ValueType.Items.Remove(item);
                        }
                    }
                    break;
                case "tabPageIsAddMark":
                    {
                        foreach (ListViewItem item in listView_AddMark.SelectedItems)
                        {
                            listView_AddMark.Items.Remove(item);
                        }
                    }
                    break;
                case "tabPageToSQLProc":
                    {
                        foreach (ListViewItem item in listView_SQLProc.SelectedItems)
                        {
                            listView_SQLProc.Items.Remove(item);
                        }
                    }
                    break;
                case "tabPageParamePrefix":
                    {
                        foreach (ListViewItem item in listViewParamePrefix.SelectedItems)
                        {
                            listViewParamePrefix.Items.Remove(item);
                        }
                    }
                    break;
                case "tabPageToOraProc":
                    {
                        foreach (ListViewItem item in listView_OraProc.SelectedItems)
                        {
                            listView_OraProc.Items.Remove(item);
                        }
                    }
                    break;
                case "tabPageToMySQLProc":
                    {
                        foreach (ListViewItem item in listView_MySQLProc.SelectedItems)
                        {
                            listView_MySQLProc.Items.Remove(item);
                        }
                    }
                    break;
                case "tabPageToOleDbProc":
                    {
                        foreach (ListViewItem item in listView_OledbProc.SelectedItems)
                        {
                            listView_OledbProc.Items.Remove(item);
                        }
                    }
                    break;
                case "tabPageToSQLiteProc":
                    {
                        foreach (ListViewItem item in listView_SQLiteProc.SelectedItems)
                        {
                            listView_SQLiteProc.Items.Remove(item);
                        }
                    }
                    break;
                case "tabPageAccessDbTypeMap":
                    {
                        foreach (ListViewItem item in listView_AccessDbType.SelectedItems)
                        {
                            listView_AccessDbType.Items.Remove(item);
                        }
                    }
                    break;
            }
        }

       


    }
}
