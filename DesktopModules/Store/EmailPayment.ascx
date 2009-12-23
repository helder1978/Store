<%@ Control language="c#" CodeBehind="EmailPayment.ascx.cs" Inherits="DotNetNuke.Modules.Store.Cart.EmailPayment" AutoEventWireup="True" %>


<%-- 

    This user control sends order info by email.  No payment is taken, the store owner has to act.
    
--%>
<asp:Label id="lblError" runat="server" CssClass="NormalRed"></asp:Label>
<asp:Panel ID="pnlProceedToEmail" runat="server" Visible="true">
<table width="450" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td>
            <asp:Label id="lblConfirmMessage" runat="server" CssClass="Normal"></asp:Label>        
        </td>
    </tr>
	<tr>
		<td align="center" style="text-align: center">
		<br />
            <asp:Button ID="btnConfirmOrder" runat="server" resourcekey="btnConfirmOrder" Text="Confirm Order" OnClick="btnConfirmOrder_Click" />
		</td>
	</tr>
</table>
</asp:Panel>
<asp:Panel ID="pnlConfirmed" runat="server" Visible="false">
    <table width="450" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td>
            <asp:Label ID="lblOrderNumber" runat="server" CssClass="Normal"></asp:Label>
        </td>
    </tr>
</table>
</asp:Panel>