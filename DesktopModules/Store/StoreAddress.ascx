<%@ Control language="c#" CodeBehind="StoreAddress.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider.StoreAddress" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="wc" Namespace="DotNetNuke.UI.WebControls" Assembly="CountryListBox" %>
<input id="hiddenAddressId" type="hidden" runat="server">
<table cellSpacing="0" cellPadding="1" summary="Address Table" border="0">
	<tr id="rowDescription" runat="server">
		<td class="SubHead" width="120" nowrap="nowrap"><dnn:label id="plDescription" runat="server" suffix=":" controlname="txtDescription" text="Description:"></dnn:label></td>
		<td vAlign="top" noWrap><asp:textbox id="txtDescription" runat="server" MaxLength="50" cssclass="NormalTextBox" Width="200px"></asp:textbox><asp:requiredfieldvalidator id="valDescription" runat="server" CssClass="NormalRed" ControlToValidate="txtToName"
				ErrorMessage="Description required." Display="Dynamic"></asp:requiredfieldvalidator></td>
	</tr>
	<tr id="rowName" runat="server">
		<td class="SubHead" width="120" nowrap="nowrap"><dnn:label id="plToName" runat="server" suffix=":" controlname="txtToName" text="Your Name:"></dnn:label></td>
		<td vAlign="top" noWrap><asp:textbox id="txtToName" runat="server" MaxLength="50" cssclass="NormalTextBox" Width="200px"></asp:textbox><asp:requiredfieldvalidator id="valToName" runat="server" CssClass="NormalRed" ControlToValidate="txtToName"
				ErrorMessage="* Name required." Display="Dynamic"></asp:requiredfieldvalidator></td>
	</tr>
	<tr id="rowStreet" runat="server">
		<td class="SubHead" width="120" nowrap="nowrap"><dnn:label id="plStreet" runat="server" controlname="txtStreet" text="Address Line 1:"></dnn:label></td>
		<td vAlign="top" noWrap><asp:textbox id="txtStreet" runat="server" MaxLength="50" cssclass="NormalTextBox" Width="200px"></asp:textbox><asp:requiredfieldvalidator id="valStreet" runat="server" CssClass="NormalRed" ControlToValidate="txtStreet"
				ErrorMessage="* First line of address required." Display="Dynamic"></asp:requiredfieldvalidator></td>
	</tr>
	<tr id="rowUnit" runat="server">
		<td class="SubHead" width="120" nowrap="nowrap"><dnn:label id="plUnit" runat="server" controlname="txtUnit" text="Address Line 2:"></dnn:label></td>
		<td vAlign="top" noWrap><asp:textbox id="txtUnit" runat="server" MaxLength="50" cssclass="NormalTextBox" Width="200px"></asp:textbox></td>
	</tr>
	<tr id="rowCity" runat="server">
		<td class="SubHead" width="120" nowrap="nowrap"><dnn:label id="plCity" runat="server" controlname="txtCity" text="Town/City:"></dnn:label></td>
		<td vAlign="top" noWrap><asp:textbox id="txtCity" runat="server" MaxLength="50" cssclass="NormalTextBox" Width="200px"></asp:textbox><asp:requiredfieldvalidator id="valCity" runat="server" CssClass="NormalRed" ControlToValidate="txtCity" ErrorMessage="* Town/city required."
				Display="Dynamic"></asp:requiredfieldvalidator></td>
	</tr>
	<tr id="rowRegion" runat="server">
		<td class="SubHead" width="120" nowrap="nowrap"><dnn:label id="plRegion" runat="server" controlname="cboRegion" text="County:"></dnn:label></td>
		<td vAlign="top" nowrap="nowrap"><asp:dropdownlist id="cboRegion" runat="server" cssclass="NormalTextBox" Width="200px" DataValueField="Value" DataTextField="Text" Visible="False"></asp:dropdownlist><asp:textbox id="txtRegion" runat="server" MaxLength="50" cssclass="NormalTextBox" Width="200px"></asp:textbox><asp:requiredfieldvalidator id="valRegion1" runat="server" CssClass="NormalRed" ControlToValidate="cboRegion" ErrorMessage="* County required." Display="Dynamic"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="valRegion2" runat="server" CssClass="NormalRed" ControlToValidate="txtRegion" ErrorMessage="* County required." Display="Dynamic"></asp:requiredfieldvalidator></td></tr>
	<tr id="rowPostal" runat="server">
		<td class="SubHead" width="120" nowrap="nowrap"><dnn:label id="plPostal" runat="server" controlname="txtPostal" text="Post Code:"></dnn:label></td>
		<td vAlign="top" noWrap><asp:textbox id="txtPostal" runat="server" MaxLength="50" cssclass="NormalTextBox" Width="200px"></asp:textbox><asp:requiredfieldvalidator id="valPostal" runat="server" CssClass="NormalRed" ControlToValidate="txtPostal"
				ErrorMessage="* Post code required." Display="Dynamic"></asp:requiredfieldvalidator></td>
	</tr>
	<tr id="rowCountry" runat="server">
		<td class="SubHead" width="120" nowrap="nowrap"><dnn:label id="plCountry" runat="server" controlname="cboCountry" text="Country:"></dnn:label></td>
		<td vAlign="top" noWrap="noWrap"><wc:countrylistbox id="cboCountry" runat="server" Width="200px" CssClass="NormalTextBox" TestIP="yes"
				DataValueField="Value" DataTextField="Text" AutoPostBack="True" onselectedindexchanged="cboCountry_SelectedIndexChanged"></wc:countrylistbox><asp:requiredfieldvalidator id="valCountry" runat="server" CssClass="NormalRed" ControlToValidate="cboCountry"
				ErrorMessage="* Country required." Display="Dynamic"></asp:requiredfieldvalidator></td>
	</tr>	
	<tr id="rowTelephone" runat="server">
		<td class="SubHead" width="120" nowrap="nowrap"><dnn:label id="plTelephone" runat="server" controlname="txtTelephone" text="Telephone:"></dnn:label></td>
		<td vAlign="top" noWrap><asp:textbox id="txtTelephone" runat="server" MaxLength="50" cssclass="NormalTextBox" Width="200px"></asp:textbox><asp:requiredfieldvalidator id="valTelephone" runat="server" CssClass="NormalRed" ControlToValidate="txtTelephone"
				ErrorMessage="* Telephone number required." Display="Dynamic"></asp:requiredfieldvalidator></td>
	</tr>
	<tr id="rowCell" runat="server">
		<td class="SubHead" width="120" nowrap="nowrap"><dnn:label id="plCell" runat="server" controlname="txtCell" text="Cell:"></dnn:label></td>
		<td vAlign="top" noWrap><asp:textbox id="txtCell" runat="server" MaxLength="50" cssclass="NormalTextBox" Width="200px"></asp:textbox><asp:requiredfieldvalidator id="valCell" runat="server" CssClass="NormalRed" ControlToValidate="txtCell" ErrorMessage="* Mobile phone number required."
				Display="Dynamic"></asp:requiredfieldvalidator></td>
	</tr>
</table>
