<%@ Page Language="c#" Codebehind="EditRole.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.Admin.EditRole" %>
<%@ Register Src="../../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc2" %>
<%@ Register Src="../../../Controls/checkright.ascx" TagName="checkright" TagPrefix="uc1" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>EditRole</title>
    <link rel="stylesheet" href="../style/style.css" type="text/css">
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div align="center">
            <table cellspacing="0" cellpadding="5" width="80%" align="center" border="0">
                <tr>
                    <td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
                        <table cellspacing="0" bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'
                            cellpadding="5" width="100%" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>'
                            border="1">
                            <tr>
                                <td align="center" height="30" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'
                                    colspan="2">
                                     <strong>编辑角色信息 </strong></td>
                            </tr>
                            <tr height="30" align="left">
                                <td width="50%" valign="middle" colspan=2>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;新名称：
                                    <asp:TextBox ID="TxtNewname" runat="server" Width="120px"></asp:TextBox>&nbsp;
                                    <asp:ImageButton ID="BtnUpName" runat="server" ImageUrl="../images/button_update.gif">
                                    </asp:ImageButton>
                                
                                    &nbsp;&nbsp;&nbsp;<asp:Button ID="RemoveRoleButton" runat="server" Width="85px" Text="删除当前角色"
                                        Height="20px" BorderColor="DarkGray" BackColor="GhostWhite"
                                        Font-Size="9pt" OnClick="RemoveRoleButton_Click" BorderStyle="Groove"></asp:Button>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                         <a href="javascript:window.history.back();"><img border=0 src="../images/button_back.gif" alt="返回角色列表" /></a>
                                         <br><br>
                                </td>
                            </tr>                        
                            <tr bgcolor="#e4e4e4">
                                <td align="center" height="30" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'
                                    colspan="2">
                                    <b>
                                        为角色：<asp:Label ID="RoleLabel" runat="server"></asp:Label> 分配权限</b></td>
                            </tr>
                            <tr>
                                <td height="30" valign="middle" colspan="2">
                                    <b>&nbsp;&nbsp; 选择权限类别</b>：<asp:DropDownList ID="CategoryDownList" runat="server" AutoPostBack="True"
                                        Width="245px" BackColor="GhostWhite" OnSelectedIndexChanged="CategoryDownList_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td height="22" colspan="2">
                                    <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0" height="100%">
                                        <tr>
                                            <td valign="top" align="center" width="43%">
                                                <asp:ListBox ID="CategoryList" runat="server" Width="100%" Rows="15" BackColor="AliceBlue"
                                                    Font-Size="9pt" SelectionMode="Multiple"></asp:ListBox>
                                            </td>
                                            <td align="center" valign="middle" width="14%">
                                                <p>
                                                    <asp:Button ID="AddPermissionButton" runat="server" Width="85px" Text="增加权限 >>" Height="22px"
                                                        BorderStyle="Solid" BorderColor="DarkGray" BackColor="GhostWhite" Font-Size="9pt"
                                                        OnClick="AddPermissionButton_Click"></asp:Button></p>
                                                <p>
                                                    <asp:Button ID="RemovePermissionButton" runat="server" Width="85px" Text="<< 移除权限"
                                                        Height="22px" BorderStyle="Solid" BorderColor="DarkGray" BackColor="GhostWhite"
                                                        Font-Size="9pt" OnClick="RemovePermissionButton_Click"></asp:Button></p>
                                            </td>
                                            <td valign="top" align="center" width="43%">
                                                <asp:ListBox ID="PermissionList" runat="server" Width="100%" Rows="15" BackColor="AliceBlue"
                                                    Font-Size="9pt" SelectionMode="Multiple"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" height="25" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'
                                    colspan="2">
                                    【 <strong>所属用户列表</strong>】</td>
                            </tr>
                            <tr height="30" align="left">
                                <td colspan=2>
                                <table cellpadding=0 cellspacing=0 width="100%" border=0>
                                <tr>
                                <td>
                                    <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                                        <tr id="TrGrid" runat="server">
                                            <td align="left">
                                                ○ 页次：<asp:Label ID="lblpage" runat="server" ForeColor="#E78A29"></asp:Label>/
                                                <asp:Label ID="lblpagesum" runat="server"></asp:Label>，共：<font color="#e78a29"><asp:Label
                                                    ID="lblrowscount" runat="server"></asp:Label></font>条</td>
                                            <td align="right">
                                                <asp:LinkButton ID="btnFirst" runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage"
                                                    CommandArgument="First" CommandName="Pager" Text="首 页">[首 页]</asp:LinkButton><asp:LinkButton
                                                        ID="btnPrev" runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage" CommandArgument="Prev"
                                                        CommandName="Pager" Text="上一页">[上一页]</asp:LinkButton><asp:LinkButton ID="btnNext"
                                                            runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage" CommandArgument="Next"
                                                            CommandName="Pager" Text="下一页">[下一页]</asp:LinkButton><asp:LinkButton ID="btnLast"
                                                                runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage" CommandArgument="Last"
                                                                CommandName="Pager" Text="尾 页">[尾 页]</asp:LinkButton></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
                                    <asp:DataGrid ID="DataGrid1" runat="server" Width="100%" AutoGenerateColumns="False"
                                        AllowPaging="True" HorizontalAlign="Center" PageSize="20">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="编号">
                                                <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <%# DataGrid1.CurrentPageIndex*DataGrid1.PageSize+DataGrid1.Items.Count+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn SortExpression="UserID" HeaderText="用户名">
                                                <ItemTemplate>                                                    
                                                        <%# DataBinder.Eval(Container, "DataItem.UserName") %>                                                    
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="TrueName" SortExpression="TrueName" ReadOnly="True" HeaderText="真实姓名">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Sex" SortExpression="Sex" ReadOnly="True" HeaderText="性别">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Phone" ReadOnly="True" HeaderText="联系电话"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Email" ReadOnly="True" HeaderText="电子邮件"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="DepartmentID" HeaderText="单位"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="修改">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="BtnEdit" runat="server" ImageUrl="..\images\button_edit.gif"
                                                        CommandName="BtnEdit"></asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="删除">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="BtnDel" runat="server" ImageUrl="..\images\button_del.gif" CommandName="BtnDel">
                                                    </asp:ImageButton>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn Visible="False" DataField="UserID" ReadOnly="True" HeaderText="用户ID">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                            </asp:BoundColumn>
                                        </Columns>
                                        <PagerStyle Font-Size="Medium" HorizontalAlign="Right" Wrap="False" Mode="NumericPages">
                                        </PagerStyle>
                                    </asp:DataGrid>
                                </td>
                            </tr>
                                </table>
                                </td>
                            </tr>
                        </table>
                        
                        <br>
                    </td>
                </tr>
            </table>
            <div align="center">
                &nbsp;&nbsp;
                <input type="button" value="· 返 回 ·" name="button2" id="Button2" runat="server" onserverclick="Button2_ServerClick">
                <uc1:checkright ID="Checkright1" runat="server" />
                <uc2:copyright ID="Copyright1" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
