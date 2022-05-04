<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAssignmentRole.aspx.cs" Inherits="Maticsoft.Web.Accounts.Admin.UserAssignmentRole" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="../../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../../Controls/CheckRight.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <META http-equiv="Content-Type" content="text/html; charset=gb2312">		
		<LINK href="../style/style.css" type="text/css" rel="stylesheet">
</head>
<body text="#000000" bgColor="#ffffff" marginheight="0" marginwidth="0">
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table id="Table1" cellSpacing="0" cellPadding="0" width="600" align="center" border="0">
					<tr>
						<td vAlign="top" bgColor="#ffffff">
							<table cellSpacing="0" cellPadding="5" width="100%" border="0">
								<tr>
									<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>' >
										<table cellSpacing=0 
      borderColorDark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>' 
      cellPadding=5 width="100%" 
      borderColorLight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' 
      border=1>
											<tr>
												<td align="center" height="25" 
												bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>' 
												>为用户指定可以管理分配的角色
													<asp:label id="lblTitle" runat="server" Font-Bold="True"></asp:label></td>
											</tr>
											<tr>
											<td align=left>
											选择用户：<asp:DropDownList ID="DropUserlist" runat="server" AutoPostBack="True" Width="201px" OnSelectedIndexChanged="DropUserlist_SelectedIndexChanged">
                                                </asp:DropDownList>
											</td>
											</tr>
											<tr>
												<td height="22">
													<asp:CheckBoxList id="chkboxRolelist" runat="server" RepeatColumns="3" Width="100%"></asp:CheckBoxList>
												</td>
											</tr>
											<tr>
												<td align="center">
													<asp:ImageButton id="BtnOk" OnClick="BtnOk_Click" runat="server" ImageUrl="../images/button_ok.gif"></asp:ImageButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
												</td>
											</tr>	
											<tr>
											<td align=left>
                                                <asp:Label ID="lblTip" runat="server"></asp:Label></td>
											</tr>										
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
			<uc1:CheckRight id="CheckRight1" runat="server"  PermissionID=-1></uc1:CheckRight>
            <uc2:copyright ID="Copyright1" runat="server" />
		</form>
	</body>
</html>
