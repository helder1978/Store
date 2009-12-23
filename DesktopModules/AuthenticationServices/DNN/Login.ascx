<%@ Control language="vb" Inherits="DotNetNuke.Modules.Admin.Authentication.Login" CodeFile="Login.ascx.vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls"%>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<script type="text/javascript">
<!--
function setUserHelp(txt, show)
{
	if(txt.value == "" | txt.value == defaulttext)
		txt.value = show ? defaulttext : "";
}

-->
</script>
<table width="250px" cellspacing="3" cellpadding="0" summary="SignIn Design Table" style="background-color: #FFFFFF">
    <tr>
	    <td align="left" width="110px"><asp:Image ID="lblClientLogin"  ImageUrl="/images/canadean/client-login.gif" runat="server" /></td>
		<td align="right" width="110px"><a href="/Register/tabid/163/Default.aspx?returnurl=<%=Request.QueryString("returnurl") %>"><asp:Image ID="lblClientRegister" ImageUrl="/images/canadean/register.gif" runat="server"/></a></td>
		<td align="right"><a href="/Register/tabid/163/Default.aspx"><asp:Image ID="Image1" ImageUrl="/images/canadean/button_execute.gif" runat="server" /></a></td>
	</tr>
	<tr>
		<td align="left"><asp:textbox id="txtUsername" onclick="this.value='';" text="Username" width="110px" cssclass="NormalTextBox" runat="server"/></td>
		<td align="right"><asp:textbox id="txtPassword" text="password" onfocus="this.value=''" width="110px" textmode="password" cssclass="NormalTextBox" runat="server"/></td>
		<td align="right"><asp:ImageButton ID="cmdLogin" runat="server" ImageUrl="/images/canadean/button_execute.gif" /></td>
	</tr>
</table>
<table visible="false" runat="server" id="tblSigninDesign" cellspacing="0" cellpadding="3" border="0" summary="SignIn Design Table">
	<tr visible="false">
		<td visible="false" class="SubHead" align="left"><dnn:label Visible="false" id="plUsername" controlname="txtUsername" runat="server" resourcekey="Username" /></td>
	</tr>
	
	<tr id="rowVerification1" runat="server" visible="false">
		<td visible="false" class="SubHead" align="left"><dnn:label id="plVerification" controlname="txtVerification" runat="server" text="Verification Code:" visible="false"></dnn:label></td>
	</tr>
	<tr id="rowVerification2" runat="server" visible="false">
		<td align="left" visible="false"><asp:textbox visible="false" id="txtVerification" columns="9" width="150" cssclass="NormalTextBox" runat="server" /></td>
	</tr>
    <tr id="trCaptcha1" runat="server" visible="false">
	    <td class="SubHead" align="left" visible="false"><dnn:label visible="false" id="plCaptcha" controlname="ctlCaptcha" runat="server" resourcekey="Captcha" /></td>
    </tr>
    <tr visible="false" id="trCaptcha2" runat="server">
	    <td align="left" visible="false"><dnn:captchacontrol visible="false" id="ctlCaptcha" captchawidth="130" captchaheight="40" cssclass="Normal" runat="server" errorstyle-cssclass="NormalRed" /></td>
    </tr>
	<tr>
		<td class="SubHead" align="left" visible="false"><dnn:label visible="false" id="plPassword" controlname="txtPassword" runat="server" resourcekey="Password" /></td>
	</tr>
</table>
