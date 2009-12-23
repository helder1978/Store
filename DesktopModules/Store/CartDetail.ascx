<%@ Control Language="c#" AutoEventWireup="True" Codebehind="CartDetail.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CartDetail" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<table border="0" cellpadding="0" cellspacing="0" width="100%">
  <tr><td><img style="border: 0px;" src="/images/canadean/shop/top_basket.jpg" alt="shopping basket"/></td></tr>
  <tr><td>&nbsp;</td></tr>
  <tr>
    <td><asp:Label ID="lblBasketEmpty" runat="server" resourcekey="lblBasketEmpty.Text" Visible="false" CssClass="Normal"></asp:Label>
      <asp:datagrid id="grdItems" runat="server" showheader="true" showfooter="true" autogeneratecolumns="false"
				allowpaging="false" width="100%"
				HeaderStyle-CssClass="headerTableCart" HeaderStyle-Wrap="false"
				ItemStyle-CssClass="rowTableCart" CellPadding="5" GridLines="None" >
        <columns>
        <asp:templatecolumn headerstyle-horizontalalign="Left">
          <headertemplate>
            <asp:Label ID="lblProduct" Runat="server" cssclass="NormalBold" resourcekey="lblProduct.Text">Product</asp:Label>
          </headertemplate>
          <itemtemplate>
            <asp:label id="lblTitle" runat="server" cssclass="NormalBold"> <%# DataBinder.Eval(Container.DataItem, "ProductTitle") %> </asp:label>
          </itemtemplate>
          <FooterTemplate>
            <img src="/images/canadean/shop/shop_payment_methods_small.gif" alt="payment methods" border="0" />
          </FooterTemplate>
        </asp:templatecolumn>
        <asp:templatecolumn HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
          <headertemplate>
            <asp:Label ID="lblPriceHeader" Runat="server" resourcekey="lblPriceHeader.Text" class="NormalBold">Price</asp:Label>
          </headertemplate>
          <itemtemplate>
            <asp:label id="lblPrice" runat="server" cssclass="Normal"></asp:label>
          </itemtemplate>
          <footertemplate>
            <asp:Label ID="lblTotals" Runat="server" resourcekey="lblTotals.Text" class="NormalBold">Basket total:</asp:Label>
          </footertemplate>
        </asp:templatecolumn>
        <asp:templatecolumn headerstyle-cssclass="NormalBold" headerstyle-horizontalalign="Center" footerstyle-horizontalalign="Center"
						itemstyle-horizontalalign="Center" headerstyle-width="35">
          <headertemplate>
            <asp:Label ID="lblQty" Runat="server" resourcekey="lblQty.Text" class="NormalBold">Qty</asp:Label>
          </headertemplate>
          <itemtemplate>
            <asp:label id="lblQuantity" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "Quantity") %> </asp:label>
          </itemtemplate>
          <footertemplate>
            <asp:label id="lblCount" runat="server" cssclass="Normal"></asp:label>
          </footertemplate>
        </asp:templatecolumn>
        <asp:templatecolumn HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
          <headertemplate>
            <asp:Label ID="lblSubtotalHeader" Runat="server" resourcekey="lblSubtotal.Text" class="NormalBold">Subtotal</asp:Label>
          </headertemplate>
          <itemtemplate>
            <asp:label id="lblSubtotal" runat="server" cssclass="Normal"></asp:label>
          </itemtemplate>
          <footertemplate>
            <asp:label id="lblTotal" runat="server" cssclass="NormalBold"></asp:label>
          </footertemplate>
        </asp:templatecolumn>
        <asp:templatecolumn headerstyle-cssclass="NormalBold" headerstyle-horizontalalign="Center" itemstyle-horizontalalign="Center" ItemStyle-Wrap="false" HeaderStyle-Width="50px" ItemStyle-Width="50px">
          <headertemplate>
            <asp:Label ID="lblControlHeader" runat="server" class="NormalBold" resourcekey="lblControlHeader.Text">Quantity Control</asp:Label>
          </headertemplate>
          <itemtemplate>
            <asp:linkbutton id="lnkAdd" runat="server" resourcekey="lnkAdd" cssclass="CommandButton">Add One</asp:linkbutton>
            <asp:LinkButton ID="lnkRemove" runat="server" resourcekey="lnkRemove" CssClass="CommandButton">Remove One</asp:linkbutton>
            <asp:linkbutton id="lnkDelete" runat="server" resourcekey="lnkDelete" cssclass="CommandButton">Delete</asp:linkbutton>
          </itemtemplate>
          <footertemplate> </footertemplate>
        </asp:templatecolumn>
        </columns>
      </asp:datagrid>
    </td>
  </tr>
</table>
