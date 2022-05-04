<%@ Page language="c#" Codebehind="Spliter.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.Admin.Spliter" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>Spliter</title>
    <style type="text/css">
	<!--
	a { color: <%=Application[Session["Style"].ToString()+"xtree_bgcolor"]%>; text-decoration: none}
	a:hover { color:red;text-decoration: none}
	-->
	</style>
    
    
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
  <body bgcolor='<%=Application[Session["Style"].ToString()+"xspliter_color"]%>' leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" onMouseOver="if(parent.middleset.cols='0,*'){parent.middleset.cols='<%=Application[Session["Style"].ToString()+"xleft_width"]%>,*'}" onClick="if(parent.middleset.cols!='0,*'){parent.middleset.cols='0,*'}else{parent.middleset.cols='<%=Application[Session["Style"].ToString()+"xleft_width"]%>,*'};" style="cursor: hand">
	
    <form id="Form1" method="post" runat="server">
	<br>
	<br>
	<br>
	<br>
	<br>
	<br>
	<br>
	</p>
	<br>
	<table width="5" height="70" border="0" cellpadding="0" cellspacing="0" bgcolor='<%=Application[Session["Style"].ToString()+"xspliter_color"]%>'>
	<tr> 
		<td height="2" bgcolor='<%=Application[Session["Style"].ToString()+"xtree_bgcolor"]%>'></td>
	</tr>
	<tr> 
		<td><font color='<%=Application[Session["Style"].ToString()+"xtree_bgcolor"]%>'>]</font></td>
	</tr>
	<tr>
		<td height="2" bgcolor='<%=Application[Session["Style"].ToString()+"xtree_bgcolor"]%>'></td>
	</tr>
	</table>
     </form>
	
  </body>
</html>
