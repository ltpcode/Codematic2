using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI.WebControls;
namespace Maticsoft.Web.SysManage
{
    public partial class IndexTree : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindTreeView();
                if (this.TreeView1.Nodes.Count == 0)
                {
                    lblTip.Visible = true;
                }
            }
        }


        //邦定根节点
        public void BindTreeView()
        {
            Maticsoft.BLL.SysManage bll = new Maticsoft.BLL.SysManage();
            DataTable dt = bll.GetTreeList("").Tables[0];
            DataRow[] drs = dt.Select("ParentID= " + 0);//选出所有子节点	

            //菜单状态           
            bool menuExpand = false;
            TreeView1.Nodes.Clear(); // 清空树
            foreach (DataRow r in drs)
            {
                string nodeid = r["NodeID"].ToString();
                string text = r["Text"].ToString();
                string parentid = r["ParentID"].ToString();
                string location = r["Location"].ToString();
                string url = r["Url"].ToString();
                string imageurl = r["ImageUrl"].ToString();
                int permissionid = int.Parse(r["PermissionID"].ToString().Trim());

                //treeview set
                this.TreeView1.Font.Name = "宋体";
                this.TreeView1.Font.Size = FontUnit.Parse("9");

                Microsoft.Web.UI.WebControls.TreeNode rootnode = new Microsoft.Web.UI.WebControls.TreeNode();
                rootnode.Text = text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"modify.aspx?id=" + nodeid + "\">修改</a> "+
                    "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick=\"if (!window.confirm('您真的要删除这条记录吗？')){return false;}\" href=\"delete.aspx?id=" + nodeid + "\">删除</a>"+
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"add.aspx?nodeid=" + nodeid + "\">增加节点</a>";
                rootnode.NodeData = nodeid;
                //rootnode.NavigateUrl = url;
                //rootnode.Target = framename;
                rootnode.Expanded = menuExpand;
                rootnode.ImageUrl = "../" + imageurl;

                TreeView1.Nodes.Add(rootnode);

                int sonparentid = int.Parse(nodeid);// or =location
                CreateNode(sonparentid, rootnode, dt);

            }

        }

        //邦定任意节点
        public void CreateNode(int parentid, Microsoft.Web.UI.WebControls.TreeNode parentnode, DataTable dt)
        {

            DataRow[] drs = dt.Select("ParentID= " + parentid);//选出所有子节点			
            foreach (DataRow r in drs)
            {
                string nodeid = r["NodeID"].ToString();
                string text = r["Text"].ToString();
                string location = r["Location"].ToString();
                string url = r["Url"].ToString();
                string imageurl = r["ImageUrl"].ToString();
                int permissionid = int.Parse(r["PermissionID"].ToString().Trim());


                Microsoft.Web.UI.WebControls.TreeNode node = new Microsoft.Web.UI.WebControls.TreeNode();
                node.Text = text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"modify.aspx?id=" + nodeid + "\">修改</a> " +
                    "&nbsp;&nbsp;&nbsp;&nbsp;<a onClick=\"if (!window.confirm('您真的要删除这条记录吗？')){return false;}\" href=\"delete.aspx?id=" + nodeid + "\">删除</a>" +
                    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"add.aspx?nodeid="+nodeid+"\">增加节点</a>";
                node.NodeData = nodeid;
                //node.NavigateUrl = url;
                //node.Target = TargetFrame;
                node.ImageUrl = "../" + imageurl;
                node.Expanded = false;
                int sonparentid = int.Parse(nodeid);// or =location

                if (parentnode == null)
                {
                    TreeView1.Nodes.Clear();
                    parentnode = new Microsoft.Web.UI.WebControls.TreeNode();
                    TreeView1.Nodes.Add(parentnode);
                }
                parentnode.Nodes.Add(node);
                CreateNode(sonparentid, node, dt);


            }//endforeach		

        }

    }
}
