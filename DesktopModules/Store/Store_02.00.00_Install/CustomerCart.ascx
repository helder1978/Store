<%@ Control language="c#" CodeBehind="CustomerCart.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CustomerCart" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<div class="Store-Cart-Entity">
  <table cellspacing="0" cellpadding="0" border="0" width="680">
    <tr>
      <td valign="top" nowrap="nowrap"><dnn:label id="lblParentTitle" runat="server" visible="False" controlname="lblParentTitle"></dnn:label>
        <asp:placeholder id="plhCart" runat="server" />
      </td>
    </tr>
    <tr><td>&nbsp;</td></tr>
    <tr><td>&nbsp;</td></tr>
    <tr>
      <td class="Store-Cart-BtnViewCartMasterTD">
        <table cellspacing="0" cellpadding="0" border="0" class="Store-Cart-BtnViewCart">
          <tbody>
            <tr>
              <td><a href="/Shop/Intro/tabid/165/Default.aspx" target="_top"><img src="/images/canadean/shop/shop_continue_shopping.jpg" alt="continue shopping" border="0" /></a></td>
              <td width="75%">&nbsp;</td>
              <td>
                <asp:imagebutton id="imgCheckout" runat="server" cssclass="Normal" Visible="false"></asp:imagebutton>
                <asp:panel ID="panLoginRegister" runat="server" Visible="false">
                    <!--<a href="/ShoppingBasket/tabid/206/ctl/login/Default.aspx?returnurl=%2fShoppingBasket%2ftabid%2f206%2fDefault.aspx" target="_top"><img src="/images/canadean/shop/shop_login_checkout.jpg" alt="login or register" border="0" /></a>-->
                    <a href="/ShoppingBasket/tabid/206/ctl/login/Default.aspx?returnurl=%2fShopping_Basket.aspx" target="_top"><img src="/images/canadean/shop/shop_login_checkout.jpg" alt="login or register" border="0" /></a>                
                </asp:panel>
              </td>
            </tr>
            <tr>
                <td colspan="2"></td>
                <td>
                    <asp:Label ID="lblInfos" visible="false" runat="server" CssClass="NormalBold"></asp:Label>
                </td>
            </tr>
          </tbody>
        </table>
      </td>
    </tr>
  </table>
</div>
