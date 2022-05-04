<%@ Page language="c#" Codebehind="makesql.aspx.cs" AutoEventWireup="True" Inherits="Maticsoft.Web.SysManage.makesql" %>

<%@ Register Src="../../Controls/checkright.ascx" TagName="checkright" TagPrefix="uc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>makesql</title>
    
    
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
  <body MS_POSITIONING="GridLayout">
	
    <form id="Form1" method="post" runat="server">
        <uc1:checkright ID="Checkright1" runat="server" />

     </form>
	
  </body>
</html>
