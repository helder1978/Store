<%@ Control language="c#" CodeBehind="PayPalAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Cart.PayPalAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<TABLE height="100" cellSpacing="0" cellPadding="0" align="center">
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblUseSandbox" runat="server" controlname="lblUseSandbox" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:checkbox id="chkUseSandbox" runat="server" cssclass="Normal" Checked="false"></asp:checkbox>
	    </TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblPayPalID" runat="server" controlname="txtPayPalID" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtPayPalID" runat="server" Width="200px" MaxLength="50" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblPayPalVerificationURL" runat="server" controlname="txtPayPalVerificationURL" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtPayPalVerificationURL" runat="server" Width="300px" MaxLength="255" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblPayPalPaymentURL" runat="server" controlname="txtPayPalPaymentURL" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtPayPalPaymentURL" runat="server" Width="300px" MaxLength="255" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblPayPalLanguage" runat="server" controlname="txtPayPalLanguage" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtPayPalLanguage" runat="server" Width="30px" MaxLength="2" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblPayPalCharset" runat="server" controlname="txtPayPalCharset" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtPayPalCharset" runat="server" Width="200px" MaxLength="25" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblPayPalButtonURL" runat="server" controlname="txtPayPalButtonURL" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtPayPalButtonURL" runat="server" Width="300px" MaxLength="255" cssclass="Normal"></asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblCurrency" runat="server" controlname="txtPayPalCurrency" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtPayPalCurrency" runat="server" Width="50px" MaxLength="3" cssclass="Normal">USD</asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblSurchargePercent" runat="server" controlname="txtSurchargePercent" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtSurchargePercent" runat="server" cssclass="Normal">USD</asp:textbox></TD>
	</TR>
	<TR>
		<TD width="150" valign="top" class="NormalBold">
			<dnn:label id="lblSurchargeFixed" runat="server" controlname="txtSurchargeFixed" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:textbox id="txtSurchargeFixed" runat="server" cssclass="Normal">USD</asp:textbox>
	    </TD>
	</TR>
	<tr>
	    <td colspan="2"><asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="red"></asp:Label></td>
	</tr>
</TABLE>
