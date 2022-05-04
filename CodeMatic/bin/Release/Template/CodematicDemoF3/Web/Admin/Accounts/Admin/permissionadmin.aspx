<%@ Page language="c#" Codebehind="PermissionAdmin.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.Admin.PermissionAdmin" %>

<%@ Register Src="../../../Controls/checkright.ascx" TagName="checkright" TagPrefix="uc2" %>

<%@ Register Src="../../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Index</title>
		
		
		
		
		<LINK href="../style/style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body text="#000000" bgColor="#ffffff" marginwidth="0" marginheight="0">
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table cellSpacing="0" cellPadding="5" width="600" align="center" border="0">
					<tr>
						<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>' align="center" height="25">
							<b><FONT size="3">【 </FONT>
								<asp:label id="LoginResult" runat="server" Font-Size="X-Small">权 限 管 理</asp:label><FONT size="3">】</FONT></b></td>
					</tr>
					<tr>
						<td height="22" bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>' >
							<table cellSpacing=0 
      borderColorDark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>' 
      cellPadding=2 width="100%" 
      borderColorLight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' 
      border=1>
								<tr>
									<td vAlign="middle" height="28">增加权限类别：
										<asp:textbox id="CategoriesName" runat="server" Width="156"></asp:textbox>&nbsp;<asp:imagebutton id="BtnAddCategory" runat="server" ImageUrl="../images/button_add.gif"></asp:imagebutton>
										<asp:Label id="lbltip1" runat="server" ForeColor="Red"></asp:Label></td>
								</tr>
								<tr>
									<td vAlign="middle" height="28">选择权限类别：
										<asp:dropdownlist id="ClassList" runat="server" Width="156px" AutoPostBack="True" onselectedindexchanged="ClassList_SelectedIndexChanged"></asp:dropdownlist>&nbsp;
										<asp:imagebutton id="BtnDelCategory" runat="server" ImageUrl="../images/button_del.gif" Visible="False"
											ToolTip="删除该类别"></asp:imagebutton></td>
								</tr>
								<tr>
									<td height="28">增加新权限：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
										<asp:textbox id="PermissionsName" runat="server" Width="156"></asp:textbox>&nbsp;
										<asp:imagebutton id="BtnAddPermissions" runat="server" ImageUrl="../images/button_add.gif" ToolTip="在该类别中增加新权限"></asp:imagebutton>
										<asp:Label id="lbltip2" runat="server" ForeColor="Red"></asp:Label></td>
								</tr>
								<tr>
									<td vAlign="middle" align="center" height="30" bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'>
										<STRONG>该类别的权限列表</STRONG></td>
								</tr>
								<tr>
									<td><asp:datagrid id="DataGrid1" runat="server" Width="100%" DataKeyField="PermissionID" CellPadding="0"
											AutoGenerateColumns="False">
											<FooterStyle Wrap="False"></FooterStyle>
											<SelectedItemStyle Wrap="False"></SelectedItemStyle>
											<EditItemStyle Wrap="False"></EditItemStyle>
											<AlternatingItemStyle Wrap="False"></AlternatingItemStyle>
											<ItemStyle Wrap="False"></ItemStyle>
											<Columns>
												<asp:BoundColumn DataField="PermissionID" HeaderText="权限编码">
													<HeaderStyle Width="50px"></HeaderStyle>
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:BoundColumn DataField="Description" HeaderText="权限名称">
													<ItemStyle Wrap="False"></ItemStyle>
												</asp:BoundColumn>
												<asp:TemplateColumn Visible="False" HeaderText="删除">
													<HeaderStyle Width="50px"></HeaderStyle>
													<ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
													<ItemTemplate>
														<asp:ImageButton id="btnDelete" runat="server" ImageUrl="..\images\button_del.gif" CommandName="Delete"></asp:ImageButton>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="修改">
													<HeaderStyle Width="50px"></HeaderStyle>
													<ItemTemplate>
														<FONT face="宋体">
															<asp:ImageButton id="btnEdit" runat="server" ImageUrl="..\images\button_edit.gif" CommandName="Edit"></asp:ImageButton></FONT>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
											<PagerStyle Wrap="False"></PagerStyle>
										</asp:datagrid></td>
								</tr>
								<tr>
									<td>
										<table id="TabEdit" cellSpacing="0" cellPadding="3" width="100%" border="0" runat="server">
											<tr>
												<td align="center" colSpan="2"  bgcolor="#E3EFFF">
													<STRONG>权限编码[</STRONG><asp:label id="lblPermId" runat="server" Font-Bold="True"></asp:label><STRONG>]的名称修改</STRONG></td>
											</tr>
											<tr>
												<td align="right" width="40%">新权限名称：
												</td>
												<td><asp:textbox id="txtNewName" runat="server"></asp:textbox>
													<asp:Label id="lbltip3" runat="server" ForeColor="Red"></asp:Label></td>
											</tr>
											<tr>
												<td align="center" colSpan="2">
													<asp:ImageButton id="btnupSave" runat="server" ImageUrl="..\images\button_save.gif"></asp:ImageButton><FONT face="宋体">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT>
													<asp:ImageButton id="btnCancel" runat="server" ImageUrl="..\images\button_cancel.gif"></asp:ImageButton></td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
                            <uc2:checkright ID="Checkright1" runat="server" />
						</td>
					</tr>
				</table>
			</div>
            <uc1:copyright ID="Copyright1" runat="server" />
		</form>
	</body>
</HTML>
