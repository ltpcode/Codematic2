<%@ Page language="c#" Codebehind="Add.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Accounts.Add" %>

<%@ Register Src="../../Controls/checkright.ascx" TagName="checkright" TagPrefix="uc2" %>

<%@ Register Src="../../Controls/copyright.ascx" TagName="copyright" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Add</title>		
		<LINK href="../../style.css" type="text/css" rel="stylesheet">
		<script language="javascript" src="../../js/date.js"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<table cellSpacing="0" cellPadding="5" width="600" align="center" border="0">
					<tr>
						<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>' >
							<table cellSpacing=0 
      borderColorDark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>' 
      cellPadding=5 width="100%" 
      borderColorLight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' 
      border=1>
								<tr bgColor="#e4e4e4">
									<td 
          bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>' 
          height=25 align=center>【 <STRONG>增加用户 </STRONG>】</td>
								</tr>
								<tr>
									<td height="25">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td width="150" height="25">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> 用户名：</div>
												</td>
												<td height="25">
													<asp:textbox id="txtUserName" tabIndex="1" runat="server" Width="249px" MaxLength="20" BorderStyle="Groove"></asp:textbox>
													<asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" ErrorMessage="用户名不能为空" Display="Dynamic"
														ControlToValidate="txtUserName"></asp:requiredfieldvalidator></td>
											</tr>
											<tr>
												<td width="150" height="25">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> 密&nbsp;&nbsp;&nbsp;&nbsp;码：</div>
												</td>
												<td height="25">
													<asp:textbox id="txtPassword" tabIndex="2" runat="server" Width="249px" MaxLength="20" BorderStyle="Groove"
														TextMode="Password"></asp:textbox>
													<asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" ErrorMessage="密码不能为空" Display="Dynamic"
														ControlToValidate="txtPassword"></asp:requiredfieldvalidator></td>
											</tr>
											<tr>
												<td width="150" height="25">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> 密码验证：</div>
												</td>
												<td height="25">
													<asp:textbox id="txtPassword1" tabIndex="3" runat="server" Width="249px" MaxLength="20" BorderStyle="Groove"
														TextMode="Password"></asp:textbox>
													<asp:comparevalidator id="CompareValidator1" runat="server" ErrorMessage="两次密码输入不符" Display="Dynamic"
														ControlToValidate="txtPassword1" ControlToCompare="txtPassword"></asp:comparevalidator></td>
											</tr>
											<tr>
												<td width="150" height="25">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> 真实姓名：</div>
												</td>
												<td height="25">
													<asp:textbox id="txtTrueName" tabIndex="4" runat="server" Width="249px" MaxLength="20" BorderStyle="Groove"></asp:textbox>
													<asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" ErrorMessage="真实姓名不能为空" Display="Dynamic"
														ControlToValidate="txtTrueName"></asp:requiredfieldvalidator></td>
											</tr>
											<tr>
												<td align="right" width="150" height="25">用户性别：
												</td>
												<td height="25"><FONT face="宋体">&nbsp;&nbsp;&nbsp;<asp:radiobutton id="RadioButton1" runat="server" GroupName="optSex" Checked="True" Text="男"></asp:radiobutton>&nbsp;&nbsp;&nbsp;&nbsp;<asp:radiobutton id="RadioButton2" runat="server" GroupName="optSex" Text="女"></asp:radiobutton></FONT></td>
											</tr>
											<tr>
												<td width="150" height="25">
													<div align="right">联系电话：</div>
												</td>
												<td style="HEIGHT: 3px" height="3">
													<asp:TextBox id="txtPhone" runat="server" Width="200px"></asp:TextBox>
												</td>
											</tr>
											<tr>
												<td width="150" height="25">
													<div align="right">电子邮箱：</div>
												</td>
												<td height="25"><asp:textbox id="txtEmail" runat="server" Width="200px"></asp:textbox></td>
											</tr>
											
											<tr>
												<td style="HEIGHT: 5px" width="150" height="5"><div align="right">界面风格：</div>
												</td>
												<td style="HEIGHT: 5px" height="5"><asp:dropdownlist id="dropStyle" runat="server" Width="200px">
														<asp:ListItem Value="1">默认蓝</asp:ListItem>
														<asp:ListItem Value="2">橄榄绿</asp:ListItem>
														<asp:ListItem Value="3">深红</asp:ListItem>
														<asp:ListItem Value="4">深绿</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
											
											<tr>
												<td colSpan="2"><asp:label id="lblMsg" runat="server" ForeColor="Red"></asp:label></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td height="25">
										<div align="center"><asp:button id="btnAdd" runat="server" Text="· 提交 ·" onclick="btnAdd_Click"></asp:button>
											<input type="button" name="button1" value="· 返 回 ·" onclick="history.back()"></div>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
            <uc1:copyright ID="Copyright1" runat="server" />
            <uc2:checkright ID="Checkright1" runat="server" />
		</form>
	</body>
</HTML>
