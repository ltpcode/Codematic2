using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.SysManage
{
	/// <summary>
	/// treelist 的摘要说明。
	/// </summary>
	public partial class treelist : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				grid.BorderWidth=Unit.Pixel(1);
				grid.CellPadding=5;
				grid.CellSpacing=0;
				grid.BorderColor=ColorTranslator.FromHtml(Application[Session["Style"].ToString()+"xtable_bordercolorlight"].ToString());
				grid.HeaderStyle.BackColor=ColorTranslator.FromHtml(Application[Session["Style"].ToString()+"xtable_titlebgcolor"].ToString());
				int pageIndex=1;
				if(Request.Params["page"]!=null && Request.Params["page"].ToString()!="")
				{
					Session["Modulepagesys"]=Convert.ToInt32(Request.Params["page"]);
					pageIndex=Convert.ToInt32(Request.Params["page"]);
				}
				else
				{
					if(Session["Modulepagesys"]!=null && Session["Modulepagesys"].ToString()!="")
					{
						pageIndex=Convert.ToInt32(Session["Modulepagesys"]);
					}
					else
					{
						pageIndex=1;
						Session["Modulepagesys"]=1;
					}
				}
                BiudTree();	
				dataBind(pageIndex);
			}
		}
        private void BiudTree()
        {
            Maticsoft.BLL.SysManage sm = new Maticsoft.BLL.SysManage();
            DataTable dt = sm.GetTreeList("").Tables[0];
            
            this.listTarget.Items.Clear();
            //加载树
            this.listTarget.Items.Add(new ListItem("根目录", "0"));
            DataRow[] drs = dt.Select("ParentID= " + 0);
            
            foreach (DataRow r in drs)
            {
                string nodeid = r["NodeID"].ToString();
                string text = r["Text"].ToString();
                string parentid = r["ParentID"].ToString();
                string permissionid = r["PermissionID"].ToString();
                text = "╋" + text;
                this.listTarget.Items.Add(new ListItem(text, nodeid));
                int sonparentid = int.Parse(nodeid);
                string blank = "├";

                BindNode(sonparentid, dt, blank);

            }
            this.listTarget.DataBind();

        }
        private void BindNode(int parentid, DataTable dt, string blank)
        {
            DataRow[] drs = dt.Select("ParentID= " + parentid);

            foreach (DataRow r in drs)
            {
                string nodeid = r["NodeID"].ToString();
                string text = r["Text"].ToString();
                string permissionid = r["PermissionID"].ToString();
                text = blank + "『" + text + "』";

                this.listTarget.Items.Add(new ListItem(text, nodeid));
                int sonparentid = int.Parse(nodeid);
                string blank2 = blank + "─";


                BindNode(sonparentid, dt, blank2);
            }
        }

        protected void listTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listTarget.SelectedItem != null)
            { 
                dataBind(1);
            }
        }
		private void dataBind(int pageIndex)
		{
			pageIndex--;
			Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();	
			string strWhere="";
			if(Session["strWheresys"]!=null && Session["strWheresys"].ToString()!="")
			{
				strWhere=Session["strWheresys"].ToString();
			}
            if (listTarget.SelectedItem != null)
            {
                string nodeid = listTarget.SelectedValue;
                if (strWhere.Trim() != "")
                {
                    strWhere = strWhere + " and ";
                }
                strWhere+="ParentID= " + nodeid;
            }
            
			DataSet ds=new DataSet(); 
			ds=sm.GetTreeList(strWhere);
			grid.DataSource=ds.Tables[0].DefaultView;
			int record_Count=ds.Tables[0].Rows.Count;
			int page_Size=grid.PageSize;
			int totalPages = int.Parse(Math.Ceiling((double)record_Count/page_Size).ToString());
			if(totalPages>0)
			{
				if ((pageIndex+1)>totalPages) 
					pageIndex = totalPages-1;				
			}
			else
			{
				pageIndex=0;
			}
			grid.CurrentPageIndex=pageIndex;
			grid.DataBind();			
			int page_Count=grid.PageCount;			
			int page_Current=pageIndex+1;

			Page011.Record_Count=record_Count;
			Page011.Page_Count=page_Count;
			Page021.Page_Count=page_Count;

			Page011.Page_Size=page_Size;
			Page021.Page_Size=page_Size;
			Page011.Page_Current=page_Current;
			Page021.Page_Current=page_Current;
		}

		private void grid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			foreach(System.Web.UI.WebControls.HyperLink link in e.Item.Cells[7].Controls)
			{
				link.Attributes.Add("onClick","if (!window.confirm('您真的要删除这条记录吗？')){return false;}");
			}
			if(e.Item.ItemIndex>=0) 
			{
				string title=Application[Session["Style"].ToString()+"xtable_titlebgcolor"].ToString();
				string bgcolor=Application[Session["Style"].ToString()+"xtable_bgcolor"].ToString();
				e.Item.Attributes.Add("onMouseOver","this.style.backgroundColor='"+title+"'; this.style.cursor='hand';");
				e.Item.Attributes.Add("onMouseOut","this.style.backgroundColor='"+bgcolor+"';");  
			}

		}



		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{    
			this.grid.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grid_ItemDataBound);

		}
		#endregion

        

	
	}
}
