<%@ Page language="c#" Codebehind="Login.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Admin.Login" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>:系统登录</title>
		
		<LINK href="images/login.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout" leftMargin="0" topMargin="0" marginheight="0" marginwidth="0">
		<form id="Form1" method="post" runat="server">
			<FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体"></FONT><FONT face="宋体">
			</FONT><FONT face="宋体"></FONT>
			<br>
			<br>
			<br>
			<br>
			<br>
			<TABLE width="620" border="0" align="center" cellPadding="0" cellSpacing="0">
				<TBODY>
					<TR>
						<TD width="620"><IMG height="11" src="images/login_p_img02.gif" width="650"></TD>
					</TR>
					<TR>
						<TD align="center" background="images/login_p_img03.gif"><br>
							<table width="570" border="0" cellspacing="0" cellpadding="0">
								<tr>
									<td><TABLE cellSpacing="0" cellPadding="0" width="570" border="0">
											<TBODY>
												<TR>
													<TD width="245" height="80" align="center" valign="top"><IMG height="67" src="images/member_t04.jpg" width="245"></TD>
													<TD align="right" valign="top"><br>
														&nbsp; <IMG height="9" src="images/point07.gif" width="13" border="0"><A href="#" onClick="window.external.addFavorite('http://www.maticsoft.com','动软管理系统')">加入收藏</A></TD>
												</TR>
											</TBODY>
										</TABLE>
									</td>
								</tr>
								<tr>
									<td>&nbsp;</td>
								</tr>
								<tr>
									<td><img src="images/a_te01.gif" width="570" height="3"></td>
								</tr>
								<tr>
									<td align="center" background="images/a_te02.gif"><table width="520" border="0" cellspacing="0" cellpadding="0">
											<tr>
												<td width="123" height="120"><IMG height="95" src="images/login_p_img05.gif" width="123" border="0"></td>
												<td align="center"><TABLE cellSpacing="0" cellPadding="0" border="0">
														
															<TR>
																<TD width="210" height="25" vAlign="top">
																	用户名： <INPUT class="nemo01" tabIndex="1" maxLength="22" size="22" name="user" id="txtUsername"
																		runat="server">																</TD>
																<TD width="80" rowSpan="3" align="right" vAlign="middle">
																	<asp:ImageButton id="btnLogin" runat="server" ImageUrl="images/login_p_img11.gif"></asp:ImageButton></TD>
															</TR>
															<TR>
																<TD vAlign="bottom" height="12">
																	密&nbsp;&nbsp; 码： <INPUT name="user" type="password" class="nemo01" tabIndex="1" size="22" maxLength="22"
																		id="txtPass" runat="server">																</TD>
															</TR>
															<TR>
															  <TD vAlign="bottom" height="13"><table width="100%%" height="25" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                  <td width="70%" align="left">
                                                                        验证码：
                                                                        <input class="nemo01" id="CheckCode" tabindex="3" maxlength="22" size="11" name="user"
                                                                            runat="server">
                                                                  </td>
                                                                  <td width="30%" align="left"><asp:Image ID="ImageCheck" runat="server" ImageUrl="../ValidateCode.aspx"  ToolTip="不区分大小写!红色数字,黑色字母!"></asp:Image></td>
                                                                </tr>
                                                              </table></TD>
												  </TR>
																										
													</TABLE>
													<br>
													<asp:Label id="lblMsg" runat="server" BackColor="Transparent" ForeColor="Red"></asp:Label>
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td bgcolor="#d5d5d5"></td>
								</tr>
								<tr>
									<td><table width="100%" border="0" cellspacing="0" cellpadding="0">
											<tr>
												<td height="70" align="center">
													Copyright(C) 2004-2008 Maticsoft All Rights Reserved.
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</TD>
					</TR>
					<TR>
						<TD><IMG height="11" src="images/login_p_img04.gif" width="650"></TD>
					</TR>
				</TBODY>
			</TABLE>
			<br>
		</form>
	</body>
</HTML>
