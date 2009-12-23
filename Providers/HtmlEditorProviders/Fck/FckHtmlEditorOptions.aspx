<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Common.Controls" Assembly="DotNetNuke" %>
<%@ Page Language="vb" ValidateRequest="false" Trace="false" CodeBehind="FckHtmlEditorOptions.aspx.vb" AutoEventWireup="false" Inherits="DotNetNuke.HtmlEditor.FckHtmlEditorProvider.CustomOptionsPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD id="Head">
		<title>
			<%= Title %>
		</title>
		<style id="StylePlaceholder" runat="server"></style>
		<asp:placeholder id="CSS" runat="server"></asp:placeholder>
		<asp:placeholder id="FAVICON" runat="server"></asp:placeholder>
		<asp:Literal id="_clientScript" runat="server"></asp:Literal>
		<asp:placeholder id="phDNNHead" runat="server"></asp:placeholder>
		<base target="_self">
	</HEAD>
	<body ID="Body" runat="server" ONSCROLL="bodyscroll()" BOTTOMMARGIN="5" LEFTMARGIN="5"
		TOPMARGIN="5" RIGHTMARGIN="5">
		<SCRIPT language="JavaScript">
         function bodyscroll() 
         {
           var F=document.forms[0];
           F.ScrollTop.value=Body.scrollTop;
         }
         
		</SCRIPT>
		<form id="form" runat="server">
			<asp:PlaceHolder id="phControls" runat="server"></asp:PlaceHolder>
			<input id="ScrollTop" type="hidden" name="ScrollTop" runat="server"> <INPUT ID="__dnnVariable" runat="server" NAME="__dnnVariable" TYPE="hidden">
		</form>
	</body>
</HTML>
