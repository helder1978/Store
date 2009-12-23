<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnstore" TagName="address" Src="~/DesktopModules/Store/Providers/AddressProviders/DefaultAddressProvider/StoreAddress.ascx" %>
<%@ Control language="c#" CodeBehind="DefaultAddressCheckout.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider.DefaultAddressCheckout" AutoEventWireup="True" %>
<table cellspacing="5" width="80%" align="center" border="0">
	<tr>
		<td>
			<fieldset>
				<legend>
					<dnn:label id="lblBillingAddressTitle" runat="server" controlname="lblBillingAddressTitle"></dnn:label>
				</legend>
				<asp:panel id="pnlBillingAddress" runat="server" HorizontalAlign="Center">
					<table border="0" cellpadding="1" cellspacing="0" summary="Billing Address Table">
						<tr id="rowBillAddress" runat="server" visible="false">
							<td class="SubHead" width="120">
								<dnn:label id="lblBillAddress" controlname="lblBillAddress" runat="server" suffix=":"></dnn:label></td>
							<td nowrap="nowrap" valign="top" visible="false">
								<asp:dropdownlist id="lstBillAddress" runat="server" Width="300px" AutoPostBack="true" onselectedindexchanged="lstBillAddress_SelectedIndexChanged"></asp:dropdownlist></td>
						</tr>
						<tr id="rowAddNewAddress" runat="server" visible="false">
							<td class="SubHead" width="120"></td>
							<td valign="top" noWrap>
								<asp:linkbutton id="lnkAddNewAddress" tabIndex="1" runat="server" CausesValidation="False" onclick="lnkAddNewAddress_Click">Add New Address</asp:linkbutton></td>
						</tr>
					</table>
					
					<dnnstore:address id="addressBilling" runat="server" ControlColumnWidth="300" StartTabIndex="2"></dnnstore:address>
					
				</asp:panel>
			</fieldset>
		</td>
	</tr>
	<tr>
		<td>
			<fieldset>
				<legend>
					<dnn:label id="lblShippingAddressTitle" runat="server" controlname="lblShippingAddressTitle"></dnn:label>
				</legend>
				<asp:Panel id="pnlShippingAddress" runat="server" HorizontalAlign="Center">
					<table cellspacing="0" cellpadding="1" summary="Shipping Address Table" border="0">
						<tr id="rowShipAddressOptions" runat="server">
							<td class="SubHead" valign="top" width="120">
								<dnn:label id="lblShipAddressOptions" controlname="lblShipAddressOptions" runat="server" suffix=":"></dnn:label></td>
							<td valign="top" noWrap>
								<asp:radiobutton id="radNone" tabIndex="16" runat="server" autopostback="True" groupname="radShipAddress" Visible="false" oncheckedchanged="radNone_CheckedChanged"></asp:radiobutton>
								<dnn:label id="lblNone" controlname="radNone" runat="server" visible="false"></dnn:label>
								<asp:radiobutton id="radBilling" tabIndex="17" runat="server" autopostback="True" groupname="radShipAddress" Checked="True" oncheckedchanged="radBilling_CheckedChanged"></asp:radiobutton>
								<dnn:label id="lblUseBillingAddress" controlname="radBilling" runat="server"></dnn:label>
								<asp:radiobutton id="radShipping" tabIndex="18" runat="server" autopostback="True" groupname="radShipAddress"
									 oncheckedchanged="radShipping_CheckedChanged"></asp:radiobutton>
								<dnn:label id="lblUseShippingAddress" controlname="radShipping" runat="server"></dnn:label></td>
						</tr>
						<tr id="rowShipAddress" runat="server" visible="false">
							<td class="SubHead" width="120">
								<dnn:label id="lblShipAddress" controlname="lblShipAddress" runat="server" suffix=":"></dnn:label></td>
							<td valign="top" noWrap>
								<asp:dropdownlist id="lstShipAddress" tabIndex="19" runat="server" Width="300px" autopostback="True"
									cssclass="Normal" onselectedindexchanged="lstShipAddress_SelectedIndexChanged"></asp:dropdownlist></td>
						</tr>
					</table>
					<dnnstore:address id="addressShipping" runat="server" ControlColumnWidth="300" StartTabIndex="20"></dnnstore:address>
				</asp:Panel>
			</fieldset>
		</td>
	</tr>
</table>
