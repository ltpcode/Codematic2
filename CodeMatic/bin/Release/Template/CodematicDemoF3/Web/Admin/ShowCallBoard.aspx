<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../Controls/checkright.ascx" %>
<%@ Page language="c#" Codebehind="ShowCallBoard.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Admin.ShowCallBoard" %>
<%@ Register TagPrefix="uc1" TagName="CopyRight" Src="../Controls/CopyRight.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>show</title>		
		<LINK href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body >
		<form id="Form2" method="post" runat="server">
			<div align="center">
				<table width="600" border="0" cellspacing="0" cellpadding="0" align="center">
					<tr>
						<td height="22">
							<div align="right">[ <a href="main.aspx" onclick="javascript:history.back();return false;">
									·µ»Ø</a> ]
							</div>
						</td>
					</tr>
				</table>
				<table width="600" border="0" align="center" cellpadding="5" cellspacing="0">
					<tr>
						<td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
							<table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'>
								<tr>
									<td height="22">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td height="22" align="center">
													<asp:Label id="lblTitle" runat="server" Width="100%" Font-Bold="True" Font-Size="Small"></asp:Label></td>
											</tr>
											<tr>
												<td height="22" align="center">
													<asp:Label id="lblCreateDate" runat="server" Width="100%" Font-Italic="True" ForeColor="Black"></asp:Label></td>
											</tr>
											<tr>
												<td height="24" style="HEIGHT: 24px">
													<asp:Label id="lblContent" runat="server" Width="100%"></asp:Label></td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
			<uc1:CopyRight id="Copyright2" runat="server"></uc1:CopyRight>
		</form>
	</body>
</HTML>
