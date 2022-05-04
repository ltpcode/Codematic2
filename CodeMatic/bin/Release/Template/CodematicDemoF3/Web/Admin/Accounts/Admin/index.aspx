
<%@ Page language="c#" Codebehind="Index.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.Index" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Index</title>		
		<link rel="stylesheet" href="../style/style.css" type="text/css">
	</HEAD>
	<body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
		<form id="Form1" method="post" runat="server">
			<br>
			<table width="400" border="0" cellspacing="0" cellpadding="0" align="center" id="Table1"
				style="LEFT: 230px; POSITION: absolute">
				<tr>
					<td></td>
				</tr>
				<tr>
					<td valign="top">
						<table width="100%" border="0" align="center" cellpadding="5" cellspacing="0">
							<tr>
								<td class="TableBody1">
									<table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolorlight="#00aeff"
										bordercolordark="#e4e4ff" class="table">
										<tr>
											<td height="25" class="Tabletitle" align="center"><b><asp:Label id="LoginResult" runat="server">网站后台管理</asp:Label></b></td>
										</tr>
										<tr>
											<td height="22">
												<table width="100%" border="0" cellspacing="0" cellpadding="0">
													<tr>
														<td height="22">
															<a href="UserAdmin.aspx">用户管理</a>
														</td>
													</tr>
													<tr>
														<td height="22">
															<a href="RoleAdmin.aspx">角色管理</a>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
