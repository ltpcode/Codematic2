<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexTree.aspx.cs" Inherits="Maticsoft.Web.SysManage.IndexTree" %>

<%@ Register Src="../../Controls/checkright.ascx" TagName="checkright" TagPrefix="uc2" %>
<%@ Register Src="../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc1" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls, Version=1.0.2.226, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../../style.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
   <div style="text-align: center">
       
            <!-- 数据列表 -->
             <table cellspacing="0" cellpadding="5" width="600" border="0">
                <tr>
                    <td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
                        <table cellspacing="0" bordercolordark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>'
                            cellpadding="5" width="100%" bordercolorlight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>'
                            border="1">
                            <tr >
                                <td bgcolor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>'
                                     align="left">
                                    菜单编辑。
                                    
                                    
                                    </td>
                                    
                            </tr>
                            <tr>
                                <td align="left">
                        <iewc:treeview id="TreeView1" runat="server" SelectExpands="True" SelectedStyle="color:#000000;background:Transparent;" HoverStyle="color:#000000;background:Transparent;"></iewc:treeview>
                        <asp:Label ID="lblTip" runat="server" Visible="False" ForeColor="Red">没有任何分类数据！！</asp:Label>
                        </td>
                        </tr>
                        </table>
                        </td></tr>
                
            </table>
        </div>
        <uc1:copyright ID="Copyright1" runat="server" />
        <uc2:checkright id="Checkright1" runat="server">
        </uc2:checkright>
    </form>
</body>
</html>
