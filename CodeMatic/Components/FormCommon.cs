using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace Codematic
{
    /// <summary>
    /// 窗体共用操作类
    /// </summary>
    public static class FormCommon
    {
        /// <summary>
        /// 当前数据库管理器窗体
        /// </summary>
        public static DbView DbViewForm
        {
            get
            {
                if (Application.OpenForms["DbView"] == null)
                {
                    return null;
                }
                return (DbView)Application.OpenForms["DbView"];
            }
        }

        /// <summary>
        /// 得到当前数据库浏览器选中的服务器名称
        /// </summary>        
        public static string GetDbViewSelServer()
        {
            if (Application.OpenForms["DbView"] == null)
            {
                return "";
            }
            DbView dbviewfrm1 = (DbView)Application.OpenForms["DbView"];
            TreeNode SelNode = dbviewfrm1.treeView1.SelectedNode;
            if (SelNode == null)
                return "";
            string longservername = "";
            switch (SelNode.Tag.ToString())
            {
                case "serverlist":
                    return "";
                case "server":
                    {
                        longservername = SelNode.Text;
                    }
                    break;
                case "db":
                    {
                        longservername = SelNode.Parent.Text;
                    }
                    break;
                case "tableroot":
                case "viewroot":
                    {
                        longservername = SelNode.Parent.Parent.Text;
                    }
                    break;
                case "table":
                case "view":
                case "proc":
                    {
                        longservername = SelNode.Parent.Parent.Parent.Text;
                    }
                    break;
                case "column":
                    longservername = SelNode.Parent.Parent.Parent.Parent.Text;
                    break;
            }

            return longservername;
        }
    }
}
