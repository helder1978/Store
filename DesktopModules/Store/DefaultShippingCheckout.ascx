<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="c#" CodeBehind="DefaultShippingCheckout.ascx.cs" Inherits="DotNetNuke.Modules.Store.Providers.Shipping.DefaultShippingProvider.DefaultShippingCheckout" AutoEventWireup="True" %>
<table width="100%" border="0" align="right" cellspacing="0" cellpadding="0" bgcolor="yellow">
	<tr>
		<td align="right" valign="top"><dnn:label id="lblShippingTotal" runat="server" cssclass="NormalBold" controlname="txtShippingTotal"
				suffix=":"></dnn:label></td>
		<td align="right" valign="top">
			<asp:TextBox id="txtShippingTotal" CssClass="NormalTextBox" runat="server" BorderStyle="None" Width="90px" ReadOnly="True" style="text-align:right"></asp:TextBox>
		</td>
		<td width="20">&nbsp;</td>
	</tr>
</table>