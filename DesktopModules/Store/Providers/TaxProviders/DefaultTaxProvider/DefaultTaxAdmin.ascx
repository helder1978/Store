<%@ Control language="c#" CodeBehind="DefaultTaxAdmin.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Tax.DefaultTaxProvider.DefaultTaxAdmin" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table align="center">
	<tr>
	    <td valign="top" class="NormalBold" width="150"><dnn:label id="lblEnableTax" runat="server" controlname="cbEnableTax" suffix=":"></dnn:label></td>
	    <td valign="top" width="300"><asp:CheckBox ID="cbEnableTax" runat="server" /></td>
	</tr>
	<tr>
		<td valign="top" class="NormalBold" width="150" nowrap>
			<dnn:label id="lblTaxRate" runat="server" controlname="txtTaxRate" suffix=":"></dnn:label>
		</td>
		<td valign="top" align="left" nowrap>
			<asp:textbox id="txtTaxRate" runat="server" cssclass="Normal" />
			<br /><asp:Label ID="lblError" runat="server" CssClass="NormalBold" ForeColor="Red"></asp:Label>
		</td>
	</tr>
	<tr>
		<td colspan="2" class="Normal">
			<asp:linkbutton id="btnSaveTaxRates" runat="server" resourcekey="btnSaveTax">Update Tax Settings</asp:linkbutton>
		</td>
	</tr>
</table>
