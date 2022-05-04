<%@ Page language="c#" Codebehind="RoleAssignment.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.Admin.RoleAssignment" %>

<%@ Register Src="../../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../../Controls/CheckRight.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Index</title>
		<META http-equiv="Content-Type" content="text/html; charset=gb2312">
		
		
		
		
		<LINK href="../style/style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body text="#000000" bgColor="#ffffff" marginheight="0" marginwidth="0">
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table id="Table1" cellSpacing="0" cellPadding="0" width="600" align="center" border="0">
					<tr>
						<td vAlign="top" bgColor="#ffffff">
							<table cellSpacing="0" cellPadding="5" width="100%" border="0">
								<tr>
									<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>' >
										<table cellSpacing=0 
      borderColorDark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>' 
      cellPadding=5 width="100%" 
      borderColorLight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' 
      border=1>
											<tr>
												<td align="center" height="25" 
												bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>' 
												>
													<asp:label id="Label1" runat="server" Font-Bold="True"></asp:label></td>
											</tr>
											<tr>
												<td height="22">
													<asp:CheckBoxList id="CheckBoxList1" runat="server" RepeatColumns="3" Width="100%"></asp:CheckBoxList>
												</td>
											</tr>
											<tr>
												<td align="center">
													<asp:ImageButton id="BtnOk" runat="server" ImageUrl="../images/button_ok.gif"></asp:ImageButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:ImageButton id="Btnback" runat="server" ImageUrl="../images/button_back.gif"></asp:ImageButton>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
			<uc1:CheckRight id="CheckRight1" runat="server"  PermissionID=4></uc1:CheckRight>
            <uc2:copyright ID="Copyright1" runat="server" />
		</form>
	</body>
</HTML>
