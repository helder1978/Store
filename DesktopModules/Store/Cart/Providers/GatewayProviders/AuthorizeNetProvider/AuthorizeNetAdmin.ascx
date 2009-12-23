<%@ Control language="c#" CodeBehind="AuthorizeNetAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Cart.AuthorizeNetAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<TABLE cellSpacing="0" cellPadding="0" border="0" align="center">
	<TR>
		<TD width="150" class="NormalBold" valign="top">
			<dnn:label id="lblGateway" runat="server" controlname="txtGateway" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:TextBox id="txtGateway" CssClass="Normal" runat="server" Width="300px" MaxLength="255"></asp:TextBox></TD>
	</TR>
	<TR>
		<TD width="150" class="NormalBold" valign="top">
			<dnn:label id="lblVersion" runat="server" controlname="txtVersion" suffix=":"></dnn:label>
		</TD>
		<TD valign="top">
			<asp:TextBox id="txtVersion" CssClass="Normal" runat="server" Width="60px" MaxLength="10"></asp:TextBox></TD>
	</TR>
	<TR>
		<TD width="150" class="NormalBold" valign="top">
			<dnn:label id="lblUsername" runat="server" controlname="txtUsername" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:TextBox id="txtUsername" CssClass="Normal" runat="server" Width="150px" MaxLength="50"></asp:TextBox></TD>
	</TR>
	<TR>
		<TD width="150" class="NormalBold" valign="top">
			<dnn:label id="lblPassword" runat="server" controlname="txtPassword" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:TextBox id="txtPassword" CssClass="Normal" runat="server" Width="150px" MaxLength="50"></asp:TextBox></TD>
	</TR>
	<TR>
		<TD width="150" class="NormalBold" valign="top">
			<dnn:label id="lblCaptureType" runat="server" controlname="ddlCapture" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:DropDownList id="ddlCapture" runat="server" CssClass="Normal">
				<asp:ListItem resourcekey="ddlCaptureAC" Value="AUTH_CAPTURE" Selected="True">Auth and Capture</asp:ListItem>
				<asp:ListItem resourcekey="ddlCaptureAO" Value="AUTH_ONLY">Auth Only</asp:ListItem>
				<asp:ListItem resourcekey="ddlCaptureCO" Value="CAPTURE_ONLY">Capture Only</asp:ListItem>
			</asp:DropDownList></TD>
	</TR>
	<TR>
		<TD width="150" class="NormalBold" valign="top">
			<dnn:label id="lblTestMode" runat="server" controlname="cbTest" suffix=":"></dnn:label></TD>
		<TD valign="top">
			<asp:CheckBox id="cbTest" runat="server" CssClass="Normal"></asp:CheckBox></TD>
	</TR>
</TABLE>
