<%@ Page language="c#" Codebehind="RoleAdmin.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.Admin.RoleAdmin" %>

<%@ Register Src="../../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../../Controls/CheckRight.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>Index</title>
		<META http-equiv="Content-Type" content="text/html; charset=gb2312">
		
		
		
		
		<LINK href="../style/style.css" type="text/css" rel="stylesheet">
  </HEAD>
	<body text="#000000" bgColor="#ffffff" >
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table id="Table1" cellSpacing="0" cellPadding="0" width="600" align="center" border="0">
					<tr>
						<td vAlign="top" bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>' >
							<table cellSpacing="0" cellPadding="5" width="100%" align="center" border="0">
								<tr>
									<td>
										<table cellSpacing=0 
      borderColorDark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>' 
      cellPadding=5 width="100%" 
      borderColorLight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' 
      border=1>
											<tr>
												<td align="center" height="25" bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'>
													<STRONG>
														<asp:label id="LoginResult" runat="server">角色管理</asp:label></STRONG></td>
											</tr>
											<tr>
												<td height="26" style="HEIGHT: 26px">
													<P><asp:label id="Label1" runat="server">新增角色名：</asp:label><asp:textbox id="TextBox1" runat="server" MaxLength="30" Columns="20" BorderStyle="Groove"></asp:textbox>&nbsp;&nbsp;
														<asp:ImageButton id="BtnAdd" runat="server" ImageUrl="../images/button_add.gif"></asp:ImageButton>&nbsp;
														<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" Display="Dynamic"
															ErrorMessage="请指定角色名称"></asp:requiredfieldvalidator></P>
												</td>
											</tr>
											<tr>
												<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'><STRONG>点击角色进行权限编辑：</STRONG></td>
											</tr>
											<tr>
												<td>
													<table cellpadding="0" cellspacing="0" border="0" width="100%">
														<tr>
															<td width="20"></td>
															<td>
																<asp:DataList id="RoleList" runat="server" RepeatColumns="3" CellPadding="1" Width="100%">
																	<ItemTemplate>
																		【<a href='EditRole.aspx?RoleID=<%# DataBinder.Eval(Container.DataItem, "RoleID") %>'><%# DataBinder.Eval(Container.DataItem, "Description") %></a>】<br />
																	</ItemTemplate>
																</asp:DataList>
															</td>
														</tr>
													</table>
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
			<uc1:CheckRight id="CheckRight1" runat="server" PermissionID="4"></uc1:CheckRight>
            <uc2:copyright ID="Copyright1" runat="server" />
		</form>
	</body>
</HTML>
