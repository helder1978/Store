<%@ Control language="c#" CodeBehind="WorldPayPayment.ascx.cs" Inherits="DotNetNuke.Modules.Store.Cart.WorldPayPayment" AutoEventWireup="True" %>


<%-- 

    This user control sends order info to WorldPay.
    
--%>
<p align="left">
    <asp:Label id="lblError" runat="server" CssClass="NormalRed"></asp:Label>
</p>
<asp:Panel ID="pnlProceedToWorldPay" runat="server" Visible="true">
<table width="450" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td colspan="3">
            <asp:Label id="lblConfirmMessage" runat="server" CssClass="Normal"></asp:Label>        
        </td>
    </tr>
	<tr>
		<td align="center" style="text-align: center">
		    <asp:Image ID="worldpayimage" runat="server" AlternateText="Click here to pay by WorldPay using your credit/debit card or WorldPay account" visible="false"/><br />
        </td>
        <td width="70%">&nbsp;</td>
        <td>
			<asp:ImageButton id="imageButton1" runat="server" ImageUrl="/images/canadean/shop/shop_confirm_order.jpg" AlternateText="Click here to pay by WorldPay using your credit/debit card or WorldPay account" Visible="true"></asp:ImageButton>
            <asp:Button ID="btnConfirmOrder" runat="server" resourcekey="btnConfirmOrder" Text="Confirm Order" OnClick="btnConfirmOrder_Click" visible="false" />
		</td>
	</tr>
</table>
</asp:Panel>
<asp:Panel ID="pnlContinue" runat="server" Visible="false">
    <table width="600" cellpadding="0" cellspacing="0" border="0" align="left">
        <tr>
            <td>
                <asp:Label ID="lblOrderNumber" runat="server" CssClass="Normal"></asp:Label>
                <asp:Button ID="btnContinue" runat="server"  Visible="false" resourcekey="btnContinue" Text="Continue to WorldPay >" />
                <asp:Image ID="worldpayimage2" runat="server" AlternateText="Pay by WorldPay using your credit/debit card or WorldPay account" Visible="false" />
			    <asp:ImageButton id="imageButton2" runat="server" ImageUrl="/images/canadean/shop/shop_continue_worldpay.jpg" AlternateText="Click here to pay by WorldPay using your credit/debit card or WorldPay account" Visible="true"></asp:ImageButton>
			    <br />
            </td>
        </tr>
    </table>
</asp:Panel>