<%@ Control language="c#" CodeBehind="DefaultTaxCheckout.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Address.DefaultTaxProvider.DefaultTaxCheckout" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table border="0" align="right" cellspacing="5">
	<tr>
		<td align="right"  valign="top"><dnn:label id="lblTaxTotal" runat="server" cssclass="NormalBold" controlname="txtTaxTotal"
				suffix=":"></dnn:label></td>
		<td align="right"  valign="top">
			<asp:TextBox id="txtTaxTotal" runat="server" CssClass="NormalTextBox" BorderStyle="None" Width="90px" ReadOnly="True" style="text-align:right"></asp:TextBox>
		</td>
	</tr>
</table>
