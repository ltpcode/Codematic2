<%@ Page language="c#" Codebehind="userPass.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.userPass" %>

<%@ Register Src="../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../Controls/CheckRight.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>userPass</title>	
		<LINK href="../../../style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table cellSpacing="0" cellPadding="5" width="600" align="center" border="0">
					<tr>
						<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>' 
    >
							<table cellSpacing=0 
      borderColorDark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>' 
      cellPadding=5 width="100%" 
      borderColorLight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' 
      border=1>
								<tr bgColor="#e4e4e4">
									<td 
          bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>' 
          height=22 align=center><STRONG>【用户密码修改】</STRONG></td>
								</tr>
								<tr>
									<td height="22">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td width="150" height="22">
													<div align="right">用户名：</div>
												</td>
												<td height="22"><asp:label id="lblName" runat="server"></asp:label></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">
														原密码：</div>
												</td>
												<td height="22"><asp:textbox id="txtOldPassword" runat="server" TextMode="Password">*</asp:textbox></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">新密码：</div>
												</td>
												<td height="22"><asp:textbox id="txtPassword" runat="server" TextMode="Password"></asp:textbox></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">
														新密码确认：</div>
												</td>
												<td height="22"><asp:textbox id="txtPassword1" runat="server" TextMode="Password"></asp:textbox></td>
											</tr>
											<tr>
												<td colSpan="2" height="22"><asp:label id="lblMsg" runat="server"></asp:label></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td height="22">
										<div align="center"><asp:button id="btnAdd" runat="server" Text="· 提交 ·" onclick="btnAdd_Click"></asp:button><FONT face="宋体">&nbsp;</FONT>
											<asp:button id="btnCancel" runat="server" Text="· 重填 ·" onclick="btnCancel_Click"></asp:button></div>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
			<uc1:CheckRight id="CheckRight1" runat="server"></uc1:CheckRight>
            <uc2:copyright ID="Copyright1" runat="server" />
		</form>
	</body>
</HTML>
