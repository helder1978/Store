<%@ Control language="c#" CodeBehind="PayPalPayment.ascx.cs" Inherits="DotNetNuke.Modules.Store.Cart.PayPalPayment" AutoEventWireup="True" %>


<%-- 

    This user control sends order info to paypal.
    
--%>
<p align="left">
    <asp:Label id="lblError" runat="server" CssClass="NormalRed"></asp:Label>
</p>
<asp:Panel ID="pnlProceedToPayPal" runat="server" Visible="true">
<table width="450" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td>
            <asp:Label id="lblConfirmMessage" runat="server" CssClass="Normal"></asp:Label>        
        </td>
    </tr>
	<tr>
		<td align="center" style="text-align: center">
		<br />
		    <asp:Image ID="paypalimage" runat="server" AlternateText="Click here to pay by PayPal using your credit/debit card or PayPal account" /><br />
			<asp:ImageButton id="imageButton1" runat="server" ImageUrl="Images/submit.gif" AlternateText="Click here to pay by PayPal using your credit/debit card or PayPal account" Visible="false"></asp:ImageButton>
            <asp:Button ID="btnConfirmOrder" runat="server" resourcekey="btnConfirmOrder" Text="Confirm Order" OnClick="btnConfirmOrder_Click" />
		</td>
	</tr>
</table>
</asp:Panel>
<asp:Panel ID="pnlContinue" runat="server" Visible="false">
    <table width="600" cellpadding="0" cellspacing="0" border="0" align="left">
        <tr>
            <td>
                <asp:Label ID="lblOrderNumber" runat="server" CssClass="Normal"></asp:Label>
                <asp:Button ID="btnContinue" runat="server" resourcekey="btnContinue" Text="Continue to PayPal >" /><br />
                <asp:Image ID="paypalimage2" runat="server" AlternateText="Pay by PayPal using your credit/debit card or PayPal account" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Panel>