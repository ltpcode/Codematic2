using System;
using System.Web;
using System.Collections;
using System.ComponentModel;
using System.Web.SessionState;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.Security;
using LTP.Accounts.Bus;
namespace <$$namespace$$>.Web 
{
	/// <summary>
	/// Global µÄÕªÒªËµÃ÷¡£
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		/// <summary>
		/// ±ØÐèµÄÉè¼ÆÆ÷±äÁ¿¡£
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{		

			#region Ä¬ÈÏÀ¶

						
			Application["1xtop1_bgimage"]="images/top-1.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["1xtop2_bgimage"]="images/top-2.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["1xtop3_bgimage"]="images/top-3.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["1xtop4_bgimage"]="images/top-4.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["1xtop5_bgimage"]="images/top-5.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["1xtopbj_bgimage"]="images/top-bj.gif"; //¶¥¿ò±³¾°Í¼Æ¬



			Application["1xtopbar_bgimage"]="images/topbar_01.jpg"; //¶¥¿ò¹¤¾ßÌõ±³¾°Í¼Æ¬
			Application["1xfirstpage_bgimage"]="images/dbsx_01.gif"; //Ê×Ò³±³¾°Í¼Æ¬
			Application["1xforumcolor"]="#f0f4fb";
			Application["1xleft_width"]="204"; //×ó¿ò¼Ü¿í¶È

			
			Application["1xtree_bgcolor"]="#e3eeff"; //×ó¿ò¼ÜÊ÷±³¾°É«
			Application["1xleft1_bgimage"]="images/left-1.gif"; 
			Application["1xleft2_bgimage"]="images/left-2.gif"; 
			Application["1xleft3_bgimage"]="images/left-3.gif"; 
			Application["1xleftbj_bgimage"]="images/left-bj.gif"; 


			Application["1xspliter_color"]="#6B7DDE"; //·Ö¸ô¿éÉ«


			Application["1xdesktop_bj"]="";//images/right-bj.gif
			Application["1xdesktop_bgimage"]="images/desktop_01.gif";//right.gif

			Application["1xtable_bgcolor"]="#F5F9FF"; //×îÍâ²ã±í¸ñ±³¾°
			Application["1xtable_bordercolorlight"]="#4F7FC9"; //ÖÐ²ã±í¸ñÁÁ±ß¿ò
			Application["1xtable_bordercolordark"]="#D3D8E0"; //ÖÐ²ã±í¸ñ°µ±ß¿ò
			Application["1xtable_titlebgcolor"]="#E3EFFF"; //ÖÐ²ã±í¸ñ±êÌâÀ¸


			Application["1xform_requestcolor"]="#E78A29"; //±íµ¥ÖÐ±ØÌî×Ö¶Î*ÑÕÉ«

			Application["1xfirstpage_topimage"]="images/top_01.gif";
			Application["1xfirstpage_bottomimage"]="images/bottom_01.gif";
			Application["1xfirstpage_middleimage"]="images/bg_01.gif";

			Application["1xabout_bgimage"]="images/about_01.gif"; //¹ØÓÚÎÒÃÇ±³¾°Í¼Æ¬

			#endregion

			#region ÂÌÉ«

			
			Application["2xtop1_bgimage"]="images/top-1-2.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["2xtop2_bgimage"]="images/top-2-2.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["2xtop3_bgimage"]="images/top-3-2.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["2xtop4_bgimage"]="images/top-4-2.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["2xtop5_bgimage"]="images/top-5-2.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["2xtopbj_bgimage"]="images/top-bj-2.gif"; //¶¥¿ò±³¾°Í¼Æ¬

			Application["2xtopbar_bgimage"]="images/topbar_01.jpg"; //¶¥¿ò¹¤¾ßÌõ±³¾°Í¼Æ¬
			Application["2xfirstpage_bgimage"]="images/dbsx_01.gif"; //Ê×Ò³±³¾°Í¼Æ¬
			Application["2xforumcolor"]="#f0f4fb";
			Application["2xleft_width"]="204"; //×ó¿ò¼Ü¿í¶È

			
			Application["2xtree_bgcolor"]="#e3ffe9"; //×ó¿ò¼ÜÊ÷±³¾°É«			
			Application["2xleft1_bgimage"]="images/left-1-2.gif"; 
			Application["2xleft2_bgimage"]="images/left-2-2.gif"; 
			Application["2xleft3_bgimage"]="images/left-3-2.gif"; 
			Application["2xleftbj_bgimage"]="images/left-bj-2.gif"; 

			Application["2xspliter_color"]="#51C94F"; //·Ö¸ô¿éÉ«


			Application["2xdesktop_bj"]="";//images/right-bj-2.gif
			Application["2xdesktop_bgimage"]="images/desktop_02.gif";//right-2.gif
			

			Application["2xtable_bgcolor"]="#F5FFF5"; //×îÍâ²ã±í¸ñ±³¾°
			Application["2xtable_bordercolorlight"]="#7DBD7B"; //ÖÐ²ã±í¸ñÁÁ±ß¿ò
			Application["2xtable_bordercolordark"]="#D3E0D3"; //ÖÐ²ã±í¸ñ°µ±ß¿ò
			Application["2xtable_titlebgcolor"]="#E4FFE3"; //ÖÐ²ã±í¸ñ±êÌâÀ¸


			Application["2xform_requestcolor"]="#E78A29"; //±íµ¥ÖÐ±ØÌî×Ö¶Î*ÑÕÉ«

			Application["2xfirstpage_topimage"]="images/top_01.gif";
			Application["2xfirstpage_bottomimage"]="images/bottom_01.gif";
			Application["2xfirstpage_middleimage"]="images/bg_01.gif";

			

			#endregion

			#region ºìÉ«

			Application["3xtop1_bgimage"]="images/top-1-1.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["3xtop2_bgimage"]="images/top-2-1.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["3xtop3_bgimage"]="images/top-3-1.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["3xtop4_bgimage"]="images/top-4-1.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["3xtop5_bgimage"]="images/top-5-1.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["3xtopbj_bgimage"]="images/top-bj-1.gif"; //¶¥¿ò±³¾°Í¼Æ¬

			Application["3xtopbar_bgimage"]="images/topbar_01.jpg"; //¶¥¿ò¹¤¾ßÌõ±³¾°Í¼Æ¬
			Application["3xfirstpage_bgimage"]="images/dbsx_01.gif"; //Ê×Ò³±³¾°Í¼Æ¬
			Application["3xforumcolor"]="#f0f4fb";
			Application["3xleft_width"]="204"; //×ó¿ò¼Ü¿í¶È

			
			Application["3xtree_bgcolor"]="#ffe3e5"; //×ó¿ò¼ÜÊ÷±³¾°É«			
			Application["3xleft1_bgimage"]="images/left-1-1.gif"; 
			Application["3xleft2_bgimage"]="images/left-2-1.gif"; 
			Application["3xleft3_bgimage"]="images/left-3-1.gif"; 
			Application["3xleftbj_bgimage"]="images/left-bj-1.gif"; 

			Application["3xspliter_color"]="#C94F4F"; //·Ö¸ô¿éÉ«


			Application["3xdesktop_bj"]="";//images/right-bj-1.gif
			Application["3xdesktop_bgimage"]="images/desktop_03.gif";//right-1.gif
			

			Application["3xtable_bgcolor"]="#FFF5F5"; //×îÍâ²ã±í¸ñ±³¾°
			Application["3xtable_bordercolorlight"]="#BD7B7B"; //ÖÐ²ã±í¸ñÁÁ±ß¿ò
			Application["3xtable_bordercolordark"]="#E1D3D3"; //ÖÐ²ã±í¸ñ°µ±ß¿ò
			Application["3xtable_titlebgcolor"]="#FFE3E3"; //ÖÐ²ã±í¸ñ±êÌâÀ¸


			Application["3xform_requestcolor"]="#E78A29"; //±íµ¥ÖÐ±ØÌî×Ö¶Î*ÑÕÉ«

			Application["3xfirstpage_topimage"]="images/top_01.gif";
			Application["3xfirstpage_bottomimage"]="images/bottom_01.gif";
			Application["3xfirstpage_middleimage"]="images/bg_01.gif";
		
			

			#endregion

			#region ÉîÂÌÉ«

			
			Application["4xtop1_bgimage"]="images/top-1-3.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["4xtop2_bgimage"]="images/top-2-3.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["4xtop3_bgimage"]="images/top-3-3.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["4xtop4_bgimage"]="images/top-4-3.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["4xtop5_bgimage"]="images/top-5-3.gif"; //¶¥¿ò±³¾°Í¼Æ¬
			Application["4xtopbj_bgimage"]="images/top-bj-3.gif"; //¶¥¿ò±³¾°Í¼Æ¬

			Application["4xtopbar_bgimage"]="images/topbar_01.jpg"; //¶¥¿ò¹¤¾ßÌõ±³¾°Í¼Æ¬
			Application["4xfirstpage_bgimage"]="images/dbsx_01.gif"; //Ê×Ò³±³¾°Í¼Æ¬
			Application["4xforumcolor"]="#f0f4fb";
			Application["4xleft_width"]="204"; //×ó¿ò¼Ü¿í¶È

			
			Application["4xtree_bgcolor"]="#e3ffe9"; //×ó¿ò¼ÜÊ÷±³¾°É«			
			Application["4xleft1_bgimage"]="images/left-1-3.gif"; 
			Application["4xleft2_bgimage"]="images/left-2-3.gif"; 
			Application["4xleft3_bgimage"]="images/left-3-3.gif"; 
			Application["4xleftbj_bgimage"]="images/left-bj-3.gif"; 

			Application["4xspliter_color"]="#51C94F"; //·Ö¸ô¿éÉ«


			Application["4xdesktop_bj"]="";//images/right-bj-3.gif
			Application["4xdesktop_bgimage"]="images/desktop_02.gif";//right-3.gif
			

			Application["4xtable_bgcolor"]="#F5FFF5"; //×îÍâ²ã±í¸ñ±³¾°
			Application["4xtable_bordercolorlight"]="#7DBD7B"; //ÖÐ²ã±í¸ñÁÁ±ß¿ò
			Application["4xtable_bordercolordark"]="#D3E0D3"; //ÖÐ²ã±í¸ñ°µ±ß¿ò
			Application["4xtable_titlebgcolor"]="#E4FFE3"; //ÖÐ²ã±í¸ñ±êÌâÀ¸


			Application["4xform_requestcolor"]="#E78A29"; //±íµ¥ÖÐ±ØÌî×Ö¶Î*ÑÕÉ«

			Application["4xfirstpage_topimage"]="images/top_01.gif";
			Application["4xfirstpage_bottomimage"]="images/bottom_01.gif";
			Application["4xfirstpage_middleimage"]="images/bg_01.gif";

			

			#endregion	

		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
			Session["Style"]=1;

            //ÊµÏÖ¶¯Ì¬ÓòÃûÇÐ»»£¬¸ù¾Ý²»Í¬ÓòÃû·ÃÎÊ²»Í¬Ò³Ãæ
            //string tempStr = Request.ServerVariables["SERVER_NAME"];
            //try
            //{
            //    if (tempStr != "")
            //    {                                      
            //        if (tempStr.Trim().StartsWith("www.maticsoft.com"))
            //        {
            //            Response.Redirect("Index.aspx");
            //        }
            //        else
            //        {
            //            Response.Redirect("Default.aspx");
            //        }
                    
            //    }
            //}
            //catch //(System.Exception ex)
            //{
            //    //Response.Write(tempStr+":"+ex.Message);
            //}

	        

		}
		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
		}
		protected void Application_EndRequest(Object sender, EventArgs e)
		{
		}
		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
		}
		protected void Application_Error(Object sender, EventArgs e)
		{
			#region


//			Exception ex=Server.GetLastError();		
////			bool LogInFile=bool.Parse(ConfigurationManager.AppSettings.Get("LogInFile"));
////			bool LogInDB=bool.Parse(ConfigurationManager.AppSettings.Get("LogInDB"));
//			string LogLastDays=ConfigurationManager.AppSettings.Get("LogLastDays");
//			string errmsg="";
//			string Particular="";
//			if(ex.InnerException!=null)
//			{
//				errmsg=ex.InnerException.Message;
//				Particular=ex.InnerException.StackTrace;					
//			}
//			else
//			{
//				errmsg=ex.Message;
//				Particular=ex.StackTrace;
//			}
////			if(LogInFile)
////			{
////				string filename=Server.MapPath("~/ErrorMessage.txt");					
////				string strTime=DateTime.Now.ToString();
////				StreamWriter sw=new StreamWriter(filename,true);
////				sw.WriteLine(strTime+"£º"+errmsg+Particular);
////				sw.Close();
////			}			
////			if(LogInDB)
////			{				
//				Assistant.AddLog(errmsg,Particular);
//				Assistant.DelOverdueLog(LogLastDays);
////			}
//			Server.Transfer("ErrorMsg.aspx");


			#endregion
		}
		protected void Session_End(Object sender, EventArgs e)
		{		
			
		}
		protected void Application_End(Object sender, EventArgs e)
		{
		}
			
		#region Web ´°ÌåÉè¼ÆÆ÷Éú³ÉµÄ´úÂë
		/// <summary>
		/// Éè¼ÆÆ÷Ö§³ÖËùÐèµÄ·½·¨ - ²»ÒªÊ¹ÓÃ´úÂë±à¼­Æ÷ÐÞ¸Ä
		/// ´Ë·½·¨µÄÄÚÈÝ¡£
		/// </summary>
		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}

