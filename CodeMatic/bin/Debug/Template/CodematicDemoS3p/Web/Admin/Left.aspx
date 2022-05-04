<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../Controls/CheckRight.ascx" %>
<%@ Page language="c#" Codebehind="Left.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Admin.Left" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Left</title>
				
		<LINK href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body bgColor='<%=Application[Session["Style"].ToString()+"xtree_bgcolor"]%>' 
	text=#000000 leftMargin=0 topMargin=0 onload="" marginheight="0" marginwidth="0" >
		<form id="Form1" method="post" runat="server">
			<table width="204" height="100%" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td height="27"><img src='<%=Application[Session["Style"].ToString()+"xleft1_bgimage"]%>' width="200" height="27"></td>
					<td rowspan="3" bgColor='<%=Application[Session["Style"].ToString()+"xtree_bgcolor"]%>'></td>
				</tr>
				<tr>
					<td height="100%" valign="top" background='<%=Application[Session["Style"].ToString()+"xleftbj_bgimage"]%>'>
						<div align="left"><font color="#314a72">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%= strWelcome %></font></div>
						<br>
						&nbsp;
						<iewc:treeview id="TreeView1" runat="server" SelectExpands="True"></iewc:treeview></td>
				</tr>
				<tr>
					<td height="19"><img src='<%=Application[Session["Style"].ToString()+"xleft2_bgimage"]%>' width="200" height="19"></td>
				</tr>
			</table>
			<uc1:CheckRight id="CheckRight1" runat="server"></uc1:CheckRight>
		</form>
	</body>
</HTML>
