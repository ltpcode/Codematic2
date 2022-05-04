<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAdminList.aspx.cs" Inherits="Maticsoft.Web.Accounts.Admin.UserAdminList" %>
<%@ Register TagPrefix="asp" Namespace="LtpPageControl" Assembly="LtpPageControl" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../../Controls/CheckRight.ascx" %>
<%@ Register Src="../../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>Index</title>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312">
    <link href="../../../style.css" type="text/css" rel="stylesheet">
</head>
<body>
    <form id="Form1" method="post" runat="server">
        <div align="center">
            <table width="90%" border="0" cellspacing="0" cellpadding="0" align="center" id="Table1">
                <tr>
                    <td class="TableBody1" valign="top">
                        <table width="100%" border="0" align="center" cellpadding="5" cellspacing="0">
                            <tr>
                                <td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
                                    <table bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'
                                        cellpadding="5" width="100%" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>'
                                        border="1" cellspacing="0">
                                        <tr>
                                            <td height="25" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'
                                                align="center">
                                                <b>用户管理</b></td>
                                        </tr>
                                        <tr>
                                            <td height="22" valign="middle">
                                                &nbsp;&nbsp; 快速查询：<asp:DropDownList ID="DropUserType" runat="server">
                                                    <asp:ListItem Value="" Selected="True">全部用户</asp:ListItem>
                                                    <asp:ListItem Value="PP">PPC用户</asp:ListItem>
                                                    <asp:ListItem Value="SU">广告主</asp:ListItem>
                                                    <asp:ListItem Value="WS">网站主</asp:ListItem>
                                                    <asp:ListItem Value="AG">广告代理商</asp:ListItem>
                                                    <asp:ListItem Value="WG">网站代理商</asp:ListItem>
                                                    <asp:ListItem Value="PG">PPC代理商</asp:ListItem>
                                                    <asp:ListItem Value="AA">管理人员</asp:ListItem>
                                                    <asp:ListItem Value="SC">电话用户</asp:ListItem>
                                                </asp:DropDownList>&nbsp;
                                                <asp:Label ID="Label1" runat="server">用户名关键字：</asp:Label>
                                                <asp:TextBox ID="TextBox1" runat="server" Width="100px" BorderStyle="Groove"></asp:TextBox>
                                                <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="..\images\button_search.GIF"
                                                    OnClick="BtnSearch_Click"></asp:ImageButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                &nbsp;<a href="../add.aspx?List=List">【添加新用户】</a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                            <td>
                            	<table cellSpacing=0 cellPadding=0 width=100% align=center border=0>					
					<tr id="TrGrid" runat="server">
						<td align="left">○ 页次：<asp:label id="lblpage" runat="server" ForeColor="#E78A29"></asp:label>/
							<asp:label id="lblpagesum" runat="server"></asp:label>，共：<FONT color="#e78a29"><asp:label id="lblrowscount" runat="server"></asp:label></FONT>条</td>
						<td align="right"><asp:linkbutton id="btnFirst" runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage" CommandArgument="First"
								CommandName="Pager" Text="首 页">[首 页]</asp:linkbutton><asp:linkbutton id="btnPrev" runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage" CommandArgument="Prev"
								CommandName="Pager" Text="上一页">[上一页]</asp:linkbutton><asp:linkbutton id="btnNext" runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage" CommandArgument="Next"
								CommandName="Pager" Text="下一页">[下一页]</asp:linkbutton><asp:linkbutton id="btnLast" runat="server" ForeColor="#E78A29" OnCommand="NavigateToPage" CommandArgument="Last"
								CommandName="Pager" Text="尾 页">[尾 页]</asp:linkbutton></td>
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
                                                <ItemTemplate><a href='../../Agent/Role/RoleAssignment.aspx?UserID=<%# DataBinder.Eval(Container, "DataItem.UserID") %>&PageIndex=<%# DataGrid1.CurrentPageIndex %>'>
                                                    <%# DataBinder.Eval(Container, "DataItem.UserName") %>
                                                </a></ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="TrueName" SortExpression="TrueName" ReadOnly="True" HeaderText="真实姓名">
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Sex" SortExpression="Sex" ReadOnly="True" HeaderText="性别">
                                                <HeaderStyle Width="30px"></HeaderStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Phone" ReadOnly="True" HeaderText="联系电话"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Email" ReadOnly="True" HeaderText="电子邮件"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="DepartmentID" HeaderText="所属公司"></asp:BoundColumn>
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
        </div>
        <uc1:CheckRight ID="CheckRight1" runat="server" PermissionID="398"></uc1:CheckRight>
        <uc2:copyright ID="Copyright1" runat="server" />
    </form>
</body>
</html>
