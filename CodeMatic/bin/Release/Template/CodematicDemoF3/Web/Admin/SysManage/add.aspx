<%@ Register TagPrefix="cc1" Namespace="LtpPageControl" Assembly="LtpPageControl" %>
<%@ Register TagPrefix="uc1" TagName="CheckRight" Src="../../Controls/CheckRight.ascx" %>
<%@ Page language="c#"  ValidateRequest="false" Codebehind="add.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.SysManage.add" %>
<%@ Register TagPrefix="uc1" TagName="CopyRight" Src="../../Controls/copyright.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>TreeAdd</title>
		
		
		
		
		<LINK href="../../style.css" type="text/css" rel="stylesheet">
		<script language="javascript">
		function imgchang()
		{			
			if(document.Form1.imgsel.selectedIndex!=0)
			{
			document.Form1.imgview.src='../'+document.Form1.imgsel.options[document.Form1.imgsel.selectedIndex].value;
			document.Form1.hideimgurl.value=document.Form1.imgsel.options[document.Form1.imgsel.selectedIndex].value;
			}
			else
			{
			document.Form1.imgview.src='../Images/MenuImg/folder16.gif';
			document.Form1.hideimgurl.value='Images/MenuImg/folder16.gif';			
			}
		}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<div align="center">
				<cc1:navigation01 id="Navigation011" runat="server" Table_Name="Form1" Key_Str="id" Page_Modify="modify.aspx"
					Page_Index="treelist.aspx" Page_Add="add.aspx"></cc1:navigation01>
				<table cellSpacing="0" cellPadding="5" width="600" border="0">
					<tr>
						<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_bgcolor"]%>'>
							<table cellSpacing=0 
      borderColorDark='<%=Application[Session["Style"].ToString()+"xtable_bordercolordark"]%>' cellPadding=5 
      width="100%" 
      borderColorLight='<%=Application[Session["Style"].ToString()+"xtable_bordercolorlight"]%>' border=1 
      >
								<tr bgColor="#e4e4e4">
									<td bgColor='<%=Application[Session["Style"].ToString()+"xtable_titlebgcolor"]%>' height=22 
          >信息添加，请详细填写下列信息，带有 <font 
            color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
            >*</font> 的必须填写。</td>
								</tr>
								<tr>
									<td height="22">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td width="150" height="22">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> 名称：</div>
												</td>
												<td height="22"><asp:textbox id="txtName" runat="server" Width="200px" MaxLength="20" ToolTip="菜单名称"></asp:textbox></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> 父类：</div>
												</td>
												<td height="22"><asp:dropdownlist id="listTarget" runat="server" Width="200px">
														<asp:ListItem Value="0" Selected="True">根目录</asp:ListItem>
													</asp:dropdownlist></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right"><font 
                  color='<%=Application[Session["Style"].ToString()+"xform_requestcolor"]%>' 
                  >*</font> 排序号：</div>
												</td>
												<td height="22"><asp:textbox id="txtId" runat="server" Width="200px" MaxLength="10" ToolTip="该父类下子节点的排列顺序号"></asp:textbox></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">链接路径：</div>
												</td>
												<td height="22"><asp:textbox id="txtUrl" runat="server" Width="300px" MaxLength="100" ToolTip="该菜单链接的页面路径"></asp:textbox></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">图标(16x16)：</div>
												</td>
												<td height="22">
													<asp:textbox id="txtImgUrl" runat="server" Width="10px" MaxLength="100" Visible="False">Images/folder16.gif</asp:textbox>
													<SELECT id="imgsel" onchange="imgchang()" runat="server" NAME="imgsel">
														<OPTION selected></OPTION>
													</SELECT>
													<IMG id="imgview" src="../Images/MenuImg/folder16.gif" border="0" runat="server">
													<INPUT id="hideimgurl" style="WIDTH: 24px; HEIGHT: 22px" type="hidden" size="1" runat="server"
														NAME="hideimgurl" value="Images/MenuImg/folder16.gif">
												</td>
											</tr>
											<tr>
												<td style="HEIGHT: 12px" width="150" height="12"><div align="right">绑定权限：</div>
												</td>
												<td style="HEIGHT: 15px" height="15"><asp:dropdownlist id="listPermission" runat="server" Width="300px"></asp:dropdownlist>
                                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="是否创建同名权限" ToolTip="选择该选项,将创建同名权限,并自动绑定.注意:如果父类为根目录则不能使用该设置" /></td>
											</tr>
											<tr>
												<td width="150" height="22">
													<div align="right">说明：</div>
												</td>
												<td height="22"><asp:textbox id="txtDescription" runat="server" Width="300px"></asp:textbox></td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td height="22">
										<table cellSpacing="0" cellPadding="0" width="100%" border="0">
											<tr>
												<td width="150" height="22">&nbsp;</td>
												<td height="22"><asp:checkbox id="chkAddContinue" runat="server" Text="连续添加"></asp:checkbox>&nbsp;[ 
													添加成功后直接跳回此页进行再次添加 ]
												</td>
											</tr>
										</table>
									</td>
								</tr>
								<tr>
									<td style="height: 22px">
										<div align="center"><asp:button id="btnAdd" runat="server" Text="· 提交 ·" onclick="btnAdd_Click"></asp:button><FONT face="宋体">&nbsp;</FONT>
											<asp:button id="btnCancel" runat="server" Text="· 重填 ·" onclick="btnCancel_Click"></asp:button></div>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</div>
			<uc1:CopyRight id="CopyRight1" runat="server"></uc1:CopyRight>
			<uc1:CheckRight id="CheckRight1" runat="server" PermissionID=2></uc1:CheckRight>
		</form>
	</body>
</HTML>
