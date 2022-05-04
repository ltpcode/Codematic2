<%@ Register TagPrefix="uc1" TagName="CopyRight" Src="../../Controls/copyright.ascx" %>

<%@ Page Language="c#" Codebehind="treelist.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.SysManage.treelist" %>

<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../Controls/CheckRight.ascx" %>
<%@ Register TagPrefix="cc1" Namespace="LtpPageControl" Assembly="LtpPageControl" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>treelist</title>
    <link href="../../style.css" type="text/css" rel="stylesheet">
</head>
<body ms_positioning="GridLayout">
    <form id="Form1" method="post" runat="server">
    <br>
        <table cellspacing="0" cellpadding="5" width="90%" align="center" border="0">
            <tr>
                <td width="100" height="15" style="height: 15px">
                    <div align="right">
                        ¸¸Àà£º</div>
                </td>
                <td height="15">
                    <asp:DropDownList ID="listTarget" runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="listTarget_SelectedIndexChanged">
                        <asp:ListItem Value="0" Selected="True">¸ùÄ¿Â¼</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
        </table>
        <cc1:Page01 ID="Page011" runat="server" Page_Index="treelist.aspx" Page_Add="add.aspx"
            Page_Makesql="makesql.aspx"></cc1:Page01>
        <table cellspacing="0" cellpadding="5" width="90%" align="center" border="0">
            <tr>
                <td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
                    <asp:DataGrid ID="grid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        Width="100%" DataKeyField="NodeID" PageSize="15">
                        <Columns>
                            <asp:BoundColumn DataField="NodeID" ReadOnly="True" HeaderText="±àºÅ">
                                <HeaderStyle Width="30px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="OrderID" HeaderText="Í¬ÀàÅÅÐò">
                                <HeaderStyle Width="55px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Text" ReadOnly="True" HeaderText="Ãû³Æ">
                                <HeaderStyle Width="120px"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="comment" ReadOnly="True" HeaderText="ÃèÊö"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Url" ReadOnly="True" HeaderText="Á´½Ó"></asp:BoundColumn>
                            <asp:HyperLinkColumn Text="ÏêÏ¸" DataNavigateUrlField="NodeID" DataNavigateUrlFormatString="show.aspx?id={0}"
                                HeaderText="ÏêÏ¸">
                                <HeaderStyle Width="30px"></HeaderStyle>
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn Text="ÐÞ¸Ä" DataNavigateUrlField="NodeID" DataNavigateUrlFormatString="modify.aspx?id={0}"
                                HeaderText="ÐÞ¸Ä">
                                <HeaderStyle Width="30px"></HeaderStyle>
                            </asp:HyperLinkColumn>
                            <asp:HyperLinkColumn Text="É¾³ý" DataNavigateUrlField="NodeID" DataNavigateUrlFormatString="delete.aspx?id={0}"
                                HeaderText="É¾³ý">
                                <HeaderStyle Width="30px"></HeaderStyle>
                            </asp:HyperLinkColumn>
                        </Columns>
                        <PagerStyle Visible="False"></PagerStyle>
                    </asp:DataGrid></td>
            </tr>
        </table>
        <cc1:Page02 ID="Page021" runat="server" Page_Index="treelist.aspx"></cc1:Page02>
        <uc1:CopyRight ID="CopyRight1" runat="server"></uc1:CopyRight>
        <uc1:CheckRight ID="CheckRight1" runat="server" PermissionID="2"></uc1:CheckRight>
    </form>
</body>
</html>
