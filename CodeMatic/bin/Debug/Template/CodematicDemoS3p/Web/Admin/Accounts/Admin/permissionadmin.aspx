<%@ Page Language="c#" Codebehind="PermissionAdmin.aspx.cs" AutoEventWireup="True"
    Inherits="Maticsoft.Web.Accounts.Admin.PermissionAdmin" %>

<%@ Register Src="../../../Controls/checkright.ascx" TagName="checkright" TagPrefix="uc2" %>
<%@ Register Src="../../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Index</title>
    <link href="../style/style.css" type="text/css" rel="stylesheet">
</head>
<body text="#000000" bgcolor="#ffffff" marginwidth="0" marginheight="0">
    <form id="Form1" method="post" runat="server">
        <div align="center">
            <table cellspacing="0" cellpadding="5" width="90%" align="center" border="0">
                
                <tr>
                    <td height="22" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
                        <table cellspacing="0" bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'
                            cellpadding="2" width="100%" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>'
                            border="1">
                            <tr>
												<td align="center" height="30" bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'>
													<STRONG>
														权 限 管 理</STRONG></td>
											</tr>
                            <tr>
                                <td valign="middle" height="28">
                                    新增权限类别：
                                    <asp:TextBox ID="CategoriesName" runat="server" Width="156"></asp:TextBox>&nbsp;<asp:ImageButton
                                        ID="BtnAddCategory" runat="server" ImageUrl="../images/button_add.gif" ToolTip="增加新的权限类别"></asp:ImageButton>
                                    <asp:Label ID="lbltip1" runat="server" ForeColor="Red"></asp:Label></td>
                            </tr>
                            <tr>
                                <td valign="middle" height="28">
                                    选择权限类别：
                                    <asp:DropDownList ID="ClassList" runat="server" Width="156px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ClassList_SelectedIndexChanged">
                                    </asp:DropDownList>&nbsp;
                                    <asp:ImageButton ID="BtnDelCategory" runat="server" ImageUrl="../images/button_del.gif"
                                        Visible="False" ToolTip="删除该类别"></asp:ImageButton></td>
                            </tr>
                            <tr>
                                <td height="28">
                                    增加新权限：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:TextBox ID="PermissionsName" runat="server" Width="156"></asp:TextBox>
                                    <asp:ImageButton ID="BtnAddPermissions" runat="server" ImageUrl="../images/button_add.gif"
                                        ToolTip="在该类别中增加新权限"></asp:ImageButton>
                                    <asp:Label ID="lbltip2" runat="server" ForeColor="Red"></asp:Label></td>
                            </tr>
                            <tr>
                                <td valign="middle" align="center" height="30" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'>
                                    <strong>该类别的权限列表</strong></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" DataKeyField="PermissionID"
                                        CellPadding="3" AutoGenerateColumns="False"  >
                                        <FooterStyle Wrap="False"></FooterStyle>
                                        <SelectedItemStyle Wrap="False"></SelectedItemStyle>
                                        <EditItemStyle Wrap="False"></EditItemStyle>
                                        <AlternatingItemStyle Wrap="False"></AlternatingItemStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="PermissionID" HeaderText="权限编码" ReadOnly="True">
                                                <HeaderStyle Width="55px" Height="30px" ></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Description" HeaderText="权限名称">                                                
                                            </asp:BoundColumn>
                                            <asp:EditCommandColumn UpdateText="更新" HeaderText="编辑" CancelText="取消" EditText="[编辑]">
										<HeaderStyle Width="65px"></HeaderStyle>
										<ItemStyle  Wrap="False" HorizontalAlign="Center"></ItemStyle>
									</asp:EditCommandColumn>
                                            <asp:TemplateColumn  HeaderText="删除">
                                                <HeaderStyle Width="50px"></HeaderStyle>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="..\images\button_del.gif"
                                                        CommandName="Delete"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                            
                                        </Columns>
                                        <PagerStyle Wrap="False"></PagerStyle>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:DataGrid></td>
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
</html>
