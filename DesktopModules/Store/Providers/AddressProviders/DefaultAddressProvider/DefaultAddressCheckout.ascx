<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnstore" TagName="address" Src="~/DesktopModules/Store/Providers/AddressProviders/DefaultAddressProvider/StoreAddress.ascx" %>
<%@ Control language="c#" CodeBehind="DefaultAddressCheckout.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Address.DefaultAddressProvider.DefaultAddressCheckout" AutoEventWireup="True" %>
<table cellspacing="0" cellpadding="0" width="100%" align="center" border="0" bgcolor="#ECEFF6">
	<tr>
		<td class="Normal">
				<div class="headerTableCart" style="height:20px">
					<dnn:label id="lblBillingAddressTitle" runat="server" CssClass="NormalBold" controlname="lblBillingAddressTitle"></dnn:label>
				</div>
				<asp:panel id="pnlBillingAddress" runat="server" HorizontalAlign="Center">
					<table border="0" cellpadding="1" cellspacing="0" summary="Billing Address Table" width="100%">
						<tr id="rowBillAddress" runat="server" visible="false">
							<td width="120">
								<dnn:label id="lblBillAddress" CssClass="Normal"  controlname="lblBillAddress" runat="server" suffix=":"></dnn:label></td>
							<td nowrap="nowrap" valign="top" visible="false">
								<asp:dropdownlist id="lstBillAddress" runat="server" Width="250px" AutoPostBack="true" onselectedindexchanged="lstBillAddress_SelectedIndexChanged"></asp:dropdownlist></td>
						</tr>
						<tr id="rowAddNewAddress" runat="server" visible="false">
							<td width="120"></td>
							<td valign="top" noWrap>
								<asp:linkbutton id="lnkAddNewAddress" tabIndex="1" runat="server" CausesValidation="False" onclick="lnkAddNewAddress_Click">Add New Address</asp:linkbutton></td>
						</tr>
					</table>
					
					<dnnstore:address id="addressBilling" runat="server" ControlColumnWidth="250" StartTabIndex="2" ></dnnstore:address>
					
				</asp:panel>

		</td>
	</tr>
	<tr><td>&nbsp;</td></tr>
	<tr><td bgcolor="#FFFFFF">&nbsp;</td></tr>
	<tr>
		<td>
				<div class="headerTableCart" style="height:20px">
					<dnn:label id="lblShippingAddressTitle" runat="server" CssClass="NormalBold" controlname="lblShippingAddressTitle"></dnn:label>
				</div>
				<asp:Panel id="pnlShippingAddress" runat="server" HorizontalAlign="Center">
					<table cellspacing="0" cellpadding="1" summary="Shipping Address Table" border="0" width="100%">
						<tr id="rowShipAddressOptions" runat="server">
							<td valign="top" width="120">
								<dnn:label id="lblShipAddressOptions" CssClass="Normal" controlname="lblShipAddressOptions" runat="server" suffix=":"></dnn:label></td>
							<td valign="top" noWrap>
								<asp:radiobutton id="radNone" tabIndex="16" runat="server" autopostback="True" groupname="radShipAddress" Visible="false" oncheckedchanged="radNone_CheckedChanged"></asp:radiobutton>
								<dnn:label id="lblNone" CssClass="Normal"  controlname="radNone" runat="server" visible="false"></dnn:label>
								<asp:radiobutton id="radBilling" tabIndex="17" runat="server" autopostback="True" groupname="radShipAddress" Checked="True" oncheckedchanged="radBilling_CheckedChanged"></asp:radiobutton>
								<dnn:label id="lblUseBillingAddress" CssClass="Normal"  controlname="radBilling" runat="server"></dnn:label>
								<asp:radiobutton id="radShipping" tabIndex="18" runat="server" autopostback="True" groupname="radShipAddress"
									 oncheckedchanged="radShipping_CheckedChanged"></asp:radiobutton>
								<dnn:label id="lblUseShippingAddress" CssClass="Normal"  controlname="radShipping" runat="server"></dnn:label></td>
						</tr>
						<tr id="rowShipAddress" runat="server" visible="false">
							<td width="120">
								<dnn:label id="lblShipAddress" CssClass="Normal"  controlname="lblShipAddress" runat="server" suffix=":"></dnn:label></td>
							<td valign="top" nowrap>
								<asp:dropdownlist id="lstShipAddress" tabIndex="19" runat="server" Width="250px" autopostback="True"
									cssclass="Normal" onselectedindexchanged="lstShipAddress_SelectedIndexChanged"></asp:dropdownlist></td>
						</tr>
					</table>
					<dnnstore:address id="addressShipping" runat="server" ControlColumnWidth="250" StartTabIndex="20"></dnnstore:address>
				</asp:Panel>
			
		</td>
	</tr>
	<tr><td>&nbsp;</td></tr>
</table>
