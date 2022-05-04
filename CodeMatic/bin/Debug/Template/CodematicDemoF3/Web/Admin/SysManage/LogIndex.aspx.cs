using System;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;

namespace Maticsoft.Web.SysManage
{
	/// <summary>
	/// LogIndex 的摘要说明。
	/// </summary>
	public partial class LogIndex : System.Web.UI.Page
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
					Session["pagelog"]=Convert.ToInt32(Request.Params["page"]);
					pageIndex=Convert.ToInt32(Request.Params["page"]);
				}
				else
				{
					if(Session["pagelog"]!=null && Session["pagelog"].ToString()!="")
					{
						pageIndex=Convert.ToInt32(Session["pagelog"]);
					}
					else
					{
						pageIndex=1;
						Session["pagelog"]=1;
					}
				}

				dataBind(pageIndex);
			}
		}
		private void dataBind(int pageIndex)
		{
			pageIndex--;
			Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();
			DataSet ds=new DataSet(); 
			ds=sm.GetLogs("");
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
			Page021.Page_Count=page_Count;		
			Page021.Page_Size=page_Size;			
			Page021.Page_Current=page_Current;
		}
		protected string FormatString(string str) 
		{ 
//			str=str.Replace(" ","&nbsp;&nbsp;"); 
//			str=str.Replace("<","&lt;"); 
//			str=str.Replace(">","&gt;"); 
//			str=str.Replace('\n'.ToString(),"<br>"); 
			if(str.Length>16)
			{
				str=str.Substring(0,16)+"...";
			}			
			return str; 
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

		}
		#endregion

		protected void Confirm_Click(object sender, System.EventArgs e)
		{
			string dgIDs = "";
			bool BxsChkd = false; 
			foreach (DataGridItem item in grid.Items) 
			{
				CheckBox deleteChkBxItem = (CheckBox) item.FindControl ("DeleteThis");
				if (deleteChkBxItem.Checked) 
				{					
					BxsChkd = true;					
					dgIDs += item.Cells[0].Text + ",";
				}
			}						
			if(BxsChkd)
			{ 
				dgIDs=dgIDs.Substring(0,dgIDs.LastIndexOf(","));
				Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();
				sm.DeleteLog(dgIDs);
				Response.Redirect("logindex.aspx");
			}			
		}

		protected void btnDelAll_Click(object sender, System.EventArgs e)
		{
			Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();
			sm.DeleteLog("");
			Response.Redirect("logindex.aspx");
		}
	}
}
