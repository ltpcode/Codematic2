<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../Controls/CheckRight.ascx" %>
<%@ Page language="c#" Codebehind="Top.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Admin.Top" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Top</title>		
		<LINK href="style.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body  text="#000000" leftmargin="0" topmargin="0" marginwidth="0"
		marginheight="0">
		<form id="Form1" method="post" runat="server">
			<table width="100%" height="102" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td background='<%=Application[Session["Style"].ToString()+"xtopbj_bgimage"]%>'>
						<table width="778" height="102" border="0" cellpadding="0" cellspacing="0">
							<tr>
								<td colspan="3">
								<img src='<%=Application[Session["Style"].ToString()+"xtop1_bgimage"]%>' width="778" height="71" alt="管理系统"></td>
							</tr>
							<tr>
								<td width="217" background='<%=Application[Session["Style"].ToString()+"xtop2_bgimage"]%>'>
								<table width="100%" height="31" border="0" cellpadding="0" cellspacing="0">
										<tr>
											<td height="3" colspan="2"></td>
										</tr>
										<tr>
											<td width="30"><div align="center"><img src="images/bar_00.gif" width="16" height="16"></div>
											</td>
											<td>
												『当前用户：
												<asp:Label id="lblSignIn" runat="server"></asp:Label>』</td>
										</tr>
									</table>
								</td>
								<td width="486" background='<%=Application[Session["Style"].ToString()+"xtop3_bgimage"]%>'><table width="100%" height="31" border="0" cellpadding="0" cellspacing="0">
										<tr>
											<td width="9%">&nbsp;</td>
											<td>
												<table height="31" border="0" cellpadding="0" cellspacing="0">
													<tr>
														<td rowspan="2"><img src='<%=Application[Session["Style"].ToString()+"xtop4_bgimage"]%>' width="39" height="31"></td>
														<td height="8"><FONT face="宋体"></FONT></td>
													</tr>
													<tr>
														<td><a href="toindex.aspx" target="_top">首页</a></td>
													</tr>
												</table>
											</td>
											<td>
												<table height="31" border="0" cellpadding="0" cellspacing="0">
													<tr>
														<td rowspan="2"><img src='<%=Application[Session["Style"].ToString()+"xtop4_bgimage"]%>' width="39" height="31"></td>
														<td height="8"></td>
													</tr>
													<tr>
														<td><a href="#" onclick="javascript:parent.mainFrame.focus();parent.mainFrame.print();">打印</a></td>
													</tr>
												</table>
											</td>
											<td>
												<table height="31" border="0" cellpadding="0" cellspacing="0">
													<tr>
														<td rowspan="2"><img src='<%=Application[Session["Style"].ToString()+"xtop4_bgimage"]%>' width="39" height="31"></td>
														<td height="8"></td>
													</tr>
													<tr>
														<td><a href="javascript:history.go(-1)">后退</a></td>
													</tr>
												</table>
											</td>
											<td>
												<table height="31" border="0" cellpadding="0" cellspacing="0">
													<tr>
														<td rowspan="2"><img src='<%=Application[Session["Style"].ToString()+"xtop4_bgimage"]%>' width="39" height="31"></td>
														<td height="8"></td>
													</tr>
													<tr>
														<td><a href="javascript:history.go(1)">前进</a></td>
													</tr>
												</table>
											</td>
											<td>
												<table height="31" border="0" cellpadding="0" cellspacing="0">
													<tr>
														<td rowspan="2"><img src='<%=Application[Session["Style"].ToString()+"xtop4_bgimage"]%>' width="39" height="31"></td>
														<td height="8"></td>
													</tr>
													<tr>
														<td><a href="Relogin.aspx" target="_top" onClick="if (!window.confirm('您确认要注消当前登录用户吗？')){return false;}">注销</a></td>
													</tr>
												</table>
											</td>
											<td>
												<table height="31" border="0" cellpadding="0" cellspacing="0">
													<tr>
														<td rowspan="2"><img src='<%=Application[Session["Style"].ToString()+"xtop4_bgimage"]%>' width="39" height="31"></td>
														<td height="8"></td>
													</tr>
													<tr>
														<td><a href="Logout.aspx" target="_top">退出</a></td>
													</tr>
												</table>
											</td>
											<td>
												<table height="31" border="0" cellpadding="0" cellspacing="0">
													<tr>
														<td width="39" rowspan="2"><img src='<%=Application[Session["Style"].ToString()+"xtop4_bgimage"]%>' width="39" height="31"></td>
														<td height="8"></td>
													</tr>
													<tr>
														<td><img src="images/bar_07.gif" width="16" height="16" onClick="if(parent.topset.rows!='22,*'){parent.topset.rows='22,*';window.scroll(0,93)}else{parent.topset.rows='93,*'};"
																style="CURSOR: hand" title="点击这里可以收缩顶部"></td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
								</td>
								<td width="75"><img src='<%=Application[Session["Style"].ToString()+"xtop5_bgimage"]%>' width="75" height="31"></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<uc1:CheckRight id="CheckRight1" runat="server"></uc1:CheckRight>
		</form>
	</body>
</HTML>
