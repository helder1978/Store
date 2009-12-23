<%@ Control language="c#" CodeBehind="WorldPayAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Cart.WorldPayAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<TABLE height="100" cellSpacing="0" cellPadding="0" align="center">
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblTestMode" runat="server" controlname="lblTestMode" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:checkbox id="chkTestMode" runat="server" cssclass="Normal" Checked="false"></asp:checkbox>
	    </TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblWorldPayID" runat="server" controlname="txtWorldPayID" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtWorldPayID" runat="server" Width="200px" MaxLength="50" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblWorldPayPaymentURL" runat="server" controlname="txtWorldPayPaymentURL" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtWorldPayPaymentURL" runat="server" Width="300px" MaxLength="255" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<!--<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblWorldPayCallbackURL" runat="server" controlname="txtWorldPayCallbackURL" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtWorldPayCallbackURL" runat="server" Width="300px" MaxLength="255" ReadOnly="true" cssclass="Normal"></asp:textbox></TD>
	</TR>-->
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblWorldPayCallbackPassword" runat="server" controlname="txtWorldPayCallbackPassword" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtWorldPayCallbackPassword" runat="server" Width="100px" MaxLength="50" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblWorldPayLanguage" runat="server" controlname="txtWorldPayLanguage" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtWorldPayLanguage" runat="server" Width="30px" MaxLength="2" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblWorldPayCharset" runat="server" controlname="txtWorldPayCharset" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtWorldPayCharset" runat="server" Width="200px" MaxLength="25" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblWorldPayButtonURL" runat="server" controlname="txtWorldPayButtonURL" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtWorldPayButtonURL" runat="server" Width="300px" MaxLength="255" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblCurrency" runat="server" controlname="txtWorldPayCurrency" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtWorldPayCurrency" runat="server" Width="50px" MaxLength="3" cssclass="Normal">USD</asp:textbox></TD>
	</TR>
	<tr>
	    <td colspan="2"><asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="red"></asp:Label></td>
	</tr>
</TABLE>
