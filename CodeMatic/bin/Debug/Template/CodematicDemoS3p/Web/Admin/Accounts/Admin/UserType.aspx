<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserType.aspx.cs" Inherits="Maticsoft.Web.Accounts.Admin.UserType" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../../Controls/checkright.ascx" TagName="checkright" TagPrefix="uc2" %>
<%@ Register Src="../../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../style/style.css" type="text/css" rel="stylesheet">
</head>
<body text="#000000" bgcolor="#ffffff" marginwidth="0" marginheight="0">
    <form id="Form1" method="post" runat="server">
        <div align="center">
            <table cellspacing="0" cellpadding="5" width="600" align="center" border="0">
                <tr>
                    <td                         align="center" height="25">
                        <b>用户类型管理</b></td>
                </tr>
                <tr>
                    <td height="22" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
                        <table cellspacing="0" bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'
                            cellpadding="2" width="100%" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>'
                            border="1">
                            <tr>
                                <td  align="left" height="25">&nbsp;
                                    类型编码：
                                    <asp:TextBox ID="txtUserType" runat="server" Width="156"></asp:TextBox></td>
                            </tr>                            
                            <tr>
                                <td height="25" align="left">&nbsp;
                                    类型描述：
                                    <asp:TextBox ID="txtDescription" runat="server" Width="156"></asp:TextBox>
                                    &nbsp;
                                    <asp:ImageButton ID="BtnAdd" runat="server" ImageUrl="../images/button_add.gif"
                                        ToolTip="增加新类型" OnClick="BtnAdd_Click"></asp:ImageButton>
                                    <asp:Label ID="lbltip2" runat="server" ForeColor="Red"></asp:Label>
                                    
                                    
                                    </td>
                            </tr>
                            
                            <tr>
                                <td valign="middle" align="center" height="30" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'>
                                    <strong>用户类型列表</strong></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" DataKeyField="UserType"
                                        CellPadding="0" AutoGenerateColumns="False" >
                                        <FooterStyle Wrap="False"></FooterStyle>
                                        <SelectedItemStyle Wrap="False"></SelectedItemStyle>
                                        <EditItemStyle Wrap="False"></EditItemStyle>
                                        <AlternatingItemStyle Wrap="False"></AlternatingItemStyle>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                        <Columns>
                                            <asp:BoundColumn DataField="UserType" HeaderText="类型编码" ReadOnly="True">
                                                <HeaderStyle Width="55px"></HeaderStyle>
                                                <ItemStyle Wrap="False"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Description" HeaderText="类型描述">                                                
                                            </asp:BoundColumn>
                                            <asp:EditCommandColumn UpdateText="更新" HeaderText="编辑" CancelText="取消" EditText="编辑">
										<HeaderStyle Width="65px"></HeaderStyle>
										<ItemStyle Font-Bold="True" Wrap="False" HorizontalAlign="Center"></ItemStyle>
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
