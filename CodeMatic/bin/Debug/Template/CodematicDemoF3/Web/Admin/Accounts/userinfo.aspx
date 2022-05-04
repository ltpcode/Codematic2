<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../Controls/CheckRight.ascx" %>
<%@ Page language="c#" Codebehind="userinfo.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.userinfo" %>

<%@ Register Src="../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>userinfo</title>		
		<LINK href="../../style.css" type="text/css" rel="stylesheet">
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
          height=22 align=center>【 <STRONG>用户详细信息 </STRONG>】</td>
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
														真实姓名：</div>
												</td>
												<td height="22">
													<asp:Label id="lblTruename" runat="server"></asp:Label></td>
											</tr>
											<tr>
												<td align="right" width="150" height="22">性别：
												</td>
												<td height="22"><FONT face="宋体">
														<asp:Label id="lblSex" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</FONT></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">联系电话：</div>
												</td>
												<td height="22">
													<asp:Label id="lblPhone" runat="server"></asp:Label>
												</td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">电子邮箱：</div>
												</td>
												<td height="22">
													<asp:Label id="lblEmail" runat="server"></asp:Label></td>
											</tr>
											
											<tr>
												<td width="150" height="22"><div align="right">界面风格：</div>
												</td>
												<td height="22">
													<asp:Label id="lblStyle" runat="server"></asp:Label></td>
											</tr>
											<tr>
												<td width="150" height="22"><div align="right">当前IP：</div>
												</td>
												<td height="22">
													<asp:Label id="lblUserIP" runat="server"></asp:Label></td>
											</tr>
											<tr>
												<td width="150" height="22"><div align="right">当前余额：</div>
												</td>
												<td height="22">
													<asp:Label id="lblModeys" runat="server"></asp:Label>(元)</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td><asp:label id="RoleList" Visible="False" Runat="server"></asp:label></td>
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
