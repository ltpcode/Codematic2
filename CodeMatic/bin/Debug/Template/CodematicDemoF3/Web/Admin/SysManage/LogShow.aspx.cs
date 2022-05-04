using System;
using System.Data;
namespace Maticsoft.Web.SysManage
{
	/// <summary>
	/// LogShow 的摘要说明。
	/// </summary>
	public partial class LogShow : System.Web.UI.Page
	{
	
		public string strtime,errmsg,Particular;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{				
				if(Request.Params["id"]!=null && Request.Params["id"].Trim()!="")
				{
					string id=Request.Params["id"];
					Maticsoft.BLL.SysManage sm=new Maticsoft.BLL.SysManage();
					DataRow row=sm.GetLog(id);
					strtime=row["datetime"].ToString();
					errmsg=row["loginfo"].ToString();
					Particular=row["Particular"].ToString().Replace("\r\n","<br>");	
					
				}	

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

		}
		#endregion
	}
}
