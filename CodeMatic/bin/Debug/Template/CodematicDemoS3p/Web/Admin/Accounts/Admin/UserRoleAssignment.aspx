<%@ Page Language="C#" AutoEventWireup="true" Codebehind="UserRoleAssignment.aspx.cs"
    Inherits="Maticsoft.Web.Accounts.Admin.UserRoleAssignment" %>

<%@ Register Src="../../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc1" %>
<%@ Register Src="../../../Controls/checkright.ascx" TagName="checkright" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户管理的角色分配</title>
    <link rel="stylesheet" href="../../style.css" type="text/css" />

    <script type="text/javascript" src="../../../js/CommonHandler.js"></script>

</head>
<body topmargin="5">
    <form id="Form1" method="post" runat="server">
        <!--Doing放在页面最上方-->
        <div id="doing" runat="server" style="z-index: 12000; left: 0px; width: 100%; cursor: wait;
            position: absolute; top: 0px; height: 100%; display: none; background-color: Black;
            filter: Alpha(Opacity=0); opacity: 0;">
        </div>
        <!--Doing放在页面最上方-->
        <div align="center">
            <table cellspacing="0" cellpadding="5" width="600" align="center" border="0">
                <tr>
                    <td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
                        <table cellspacing="0" bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'
                            cellpadding="5" width="100%" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>'
                            border="1">
                            <tr>
                                <td align="center" height="25" bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'
                                    colspan="2">
                                    <strong>用户管理的角色分配</strong></td>
                            </tr>
                            <tr>
                                <td height="30" valign="middle" align="left" colspan="2">
                                    <b>&nbsp;&nbsp; 选择用户</b>：<asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="True"
                                        Width="120px" BackColor="GhostWhite" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td height="22" colspan="2">
                                    <table id="Table2" cellspacing="0" cellpadding="0" width="100%" border="0" height="100%">
                                        <tr>
                                            <td valign="top" align="center" width="43%">
                                                <asp:ListBox ID="AllRoleList" runat="server" Width="100%" Rows="15" BackColor="AliceBlue"
                                                    Font-Size="9pt" SelectionMode="Multiple"></asp:ListBox>
                                            </td>
                                            <td align="center" valign="middle" width="14%">
                                                <p>
                                                    <asp:Button ID="btnAdd" runat="server" Width="85px" Text="增 加 >>" Height="22px" BorderStyle="Solid"
                                                        BorderColor="DarkGray" BackColor="GhostWhite" Font-Size="9pt" OnClick="btnAdd_Click"
                                                        OnClientClick="AvoidSubmit('doing',30);"></asp:Button></p>
                                                <p>
                                                    <asp:Button ID="btnRemove" runat="server" Width="85px" Text="<< 移 除" Height="22px"
                                                        BorderStyle="Solid" BorderColor="DarkGray" BackColor="GhostWhite" Font-Size="9pt"
                                                        OnClick="btnRemove_Click" OnClientClick="AvoidSubmit('doing',30);"></asp:Button></p>
                                            </td>
                                            <td valign="top" align="center" width="43%">
                                                <asp:ListBox ID="SelectedRoleList" runat="server" Width="100%" Rows="15" BackColor="AliceBlue"
                                                    Font-Size="9pt" SelectionMode="Multiple"></asp:ListBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
                    <uc1:copyright id="Copyright1" runat="server">
            </uc1:copyright>
            <uc2:checkright id="Checkright1" runat="server">
            </uc2:checkright>
    </form>
</body>
</html>
