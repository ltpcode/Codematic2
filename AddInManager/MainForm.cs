using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Maticsoft.AddInManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            ////ÐÂÔö
            //if (Modify)
            //{
            //    DataRow[] drs = ds.Tables[0].Select("Remark='" + Remark + "'");
            //    drs[0]["Remark"] = Remark;
            //    drs[0]["IPAddress"] = IPAddress;
            //    drs[0]["SubnetMask"] = SubnetMask;
            //    drs[0]["GateWay"] = GateWay;
            //    drs[0]["DNS1"] = DNS1;
            //    drs[0]["DNS2"] = DNS2;

            //}
            //else
            //{
            //    DataRow rown = ds.Tables[0].NewRow();
            //    rown["Remark"] = Remark;
            //    rown["IPAddress"] = IPAddress;
            //    rown["SubnetMask"] = SubnetMask;
            //    rown["GateWay"] = GateWay;
            //    rown["DNS1"] = DNS1;
            //    rown["DNS2"] = DNS2;

            //    ds.Tables[0].Rows.Add(rown);
            //}

            //ds.WriteXml(path);
        }
    }
}