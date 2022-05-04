<%@ Page language="c#" Codebehind="EditRole.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.Admin.EditRole" %>
<%@ Register Src="../../../Controls/checkright.ascx" TagName="checkright" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>EditRole</title>
		
		
		
		
		<link rel="stylesheet" href="../style/style.css" type="text/css">
	</HEAD>
	<body  >
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table cellSpacing="0" cellPadding="5" width="600" align="center" border="0">
					<tr>
						<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>' 
    >
							<table cellSpacing=0 
      borderColorDark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>' 
      cellPadding=5 width="100%" 
      borderColorLight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' 
      border=1>
								<tr>
									<td align="center" height="25" bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>' colspan="2">
										【 <STRONG>编辑角色信息 </STRONG>】</td>
								</tr>
								<tr height="30" align="left">
									<td width="50%" valign="middle">
										&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;新名称：
										<asp:TextBox id="TxtNewname" runat="server" Width="120px"></asp:TextBox>&nbsp;
										<asp:ImageButton id="BtnUpName" runat="server" ImageUrl="../images/button_update.gif"></asp:ImageButton>
									</td>
									<td>
										&nbsp;&nbsp;&nbsp;<asp:Button id="RemoveRoleButton" runat="server" Width="90px" Text="删除当前角色" Height="22px" BorderStyle="Solid"
											BorderColor="DarkGray" BackColor="GhostWhite" Font-Size="9pt" onclick="RemoveRoleButton_Click"></asp:Button>
									</td>
								</tr>
								<tr>
									<td colspan="2"></td>
								</tr>
								<TR bgColor="#e4e4e4">
									<TD align="center" height="25" bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>' colspan="2"><B>
											<asp:label id="RoleLabel" Runat="server"></asp:label></B></TD>
								</TR>
								<tr>
									<td height="30" valign="middle" colspan="2">
										<B>&nbsp;&nbsp; 权限类别</B>：<asp:DropDownList id="CategoryDownList" runat="server" AutoPostBack="True" Width="245px" BackColor="GhostWhite" onselectedindexchanged="CategoryDownList_SelectedIndexChanged"></asp:DropDownList>
									</td>
								</tr>
								<TR>
									<TD height="22" colspan="2">
										<TABLE id="Table2" cellSpacing="0" cellPadding="0" width="100%" border="0" height="100%">
											<TR>
												<TD vAlign="top" align="center" width="43%">
													<asp:listbox id="CategoryList" runat="server" Width="100%" Rows="15" BackColor="AliceBlue" Font-Size="9pt"></asp:listbox>
												</TD>
												<td align="center" valign="middle" width="14%">
													<P>
														<asp:Button id="AddPermissionButton" runat="server" Width="85px" Text="增加权限 >>" Height="22px"
															BorderStyle="Solid" BorderColor="DarkGray" BackColor="GhostWhite" Font-Size="9pt" onclick="AddPermissionButton_Click"></asp:Button></P>
													<P>
														<asp:Button id="RemovePermissionButton" runat="server" Width="85px" Text="<< 移除权限" Height="22px"
															BorderStyle="Solid" BorderColor="DarkGray" BackColor="GhostWhite" Font-Size="9pt" onclick="RemovePermissionButton_Click"></asp:Button></P>
												</td>
												<TD vAlign="top" align="center" width="43%">
													<asp:listbox id="PermissionList" runat="server" Width="100%" Rows="15" BackColor="AliceBlue"
														Font-Size="9pt"></asp:listbox>
												</TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</table>
							<DIV></DIV>
							<br>
						</td>
					</tr>
				</table>
				<div align="center"><INPUT type="button" value="· 返 回 ·" name="button2" id="Button2" runat="server" onserverclick="Button2_ServerClick"></div>
				<uc2:checkright ID="Checkright1" runat="server" />
		</form>
		</DIV>
	</body>
</HTML>
