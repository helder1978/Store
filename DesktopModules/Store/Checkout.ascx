<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="c#" CodeBehind="Checkout.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.Checkout" AutoEventWireup="True" %>

<div class="Store-checkout-Entity">
  <table cellspacing="3" cellpadding="0" border="0" width="100%" class="NormalBold">
  <tbody>
  <asp:placeholder id="plhCheckout" runat="server">
    <tr runat="Server" id="trRow1" class="NormalBold">
      <td align="center" class="NormalBold"><dnn:label id="lblCartTitle" runat="server" cssclass="NormalBold" controlname="lblCartTitle" visible="false"></dnn:label>
        <br/>
      </td>
    </tr>
    <tr runat="Server" id="trRow2" class="NormalBold">
      <td class="NormalBold"><table cellpadding="0" cellspacing="0" border="0" width="100%" align="center" class="NormalBold">
          <tr>
            <td class="NormalBold"><asp:placeholder id="plhCart" runat="server"></asp:placeholder>
            </td>
          </tr>
          <tr id="trShipping" runat="server" visible="false">
            <td class="NormalBold"><asp:PlaceHolder id="plhShippingCheckout" runat="server"></asp:PlaceHolder>
            </td>
          </tr>
          <tr runat="server" visible="true" id="trTax">
            <td class="NormalBold"><asp:PlaceHolder id="plhTaxCheckout" runat="server"></asp:PlaceHolder>
            </td>
          </tr>
          <tr>
            <td><table border="0" align="right" cellspacing="5" class="NormalBold">
                <tr>
                  <td align="right" width="105px"  valign="top" class="NormalBold"><dnn:label id="lblCartTotal" runat="server" cssclass="NormalBold" controlname="txtCartTotal" suffix=":"></dnn:label>
                  </td>
                  <td align="right"  valign="top" class="NormalBold"><asp:TextBox id="txtCartTotal" style="TEXT-ALIGN: right" runat="server" Font-Bold="True" ReadOnly="True" Width="120px" BorderStyle="None" CssClass="NormalTextBox"></asp:TextBox>
                  </td>
                </tr>
              </table></td>
          </tr>
        </table></td>
    </tr>
    <tr runat="Server" id="trRow4">
      <td>
      </td>
    </tr>
    <tr runat="Server" id="trRow5">
      <td valign="top" align="center"><asp:placeholder id="plhAddressCheckout" runat="server"></asp:placeholder>
      </td>
    </tr>
    <tr runat="Server" id="trRow6">
      <td>
      </td>
    </tr>
    <tr>
      <td align="center"><dnn:label id="lblGatewayTitle" runat="server" cssclass="NormalBold" controlname="lblGatewayTitle"></dnn:label>
        <br/>
        <asp:placeholder id="plhGateway" runat="server"></asp:placeholder>
      </td>
    </tr>
  </asp:placeholder>
  <asp:placeholder id="plhOrder" runat="server" visible="False">
    <tr>
      <td class="Normal" align="left"><asp:label id="lblOrderNumber" runat="server" cssclass="NormalBold"></asp:label>
        <br/>
        <br/>
        <asp:label id="lblOrderProcessed" Runat="server" resourcekey="lblOrderProcessed" cssclass="NormalBold">Your order has been successfully processed.</asp:label>
        <br/>
        <br/>
      </td>
    </tr>
    <tr>
      <td><hr/>
      </td>
    </tr>
    <tr>
    <td align="center">
    <asp:datalist id="lstOrder" runat="server" cssclass="Store-DataGrid" showheader="true" showfooter="true" width="100%" repeatcolumns="1" CellPadding="5">
      <headertemplate>
        <table border="0" cellpadding="0" cellspacing="3" width="100%">
        <tr>
          <td align="left" class="NormalBold" nowrap="nowrap"><asp:Label ID="lblModelName" Runat="server" resourcekey="lblModelName" cssclass="NormalBold">Product</asp:Label>
          </td>
          <td align="left" class="NormalBold" nowrap="nowrap" runat="server" visible="false"><asp:Label ID="lblModelNumber" Runat="server" resourcekey="lblModelNumber" cssclass="NormalBold">Model Number</asp:Label>
          </td>
          <td align="right" class="NormalBold" nowrap="nowrap"><asp:Label ID="lblPrice" Runat="server" resourcekey="lblPrice" cssclass="NormalBold">Price</asp:Label>
          </td>
          <td align="right" class="NormalBold" nowrap="nowrap"><asp:Label ID="lblQuantity" Runat="server" resourcekey="lblQuantity" cssclass="NormalBold">Quantity</asp:Label>
          </td>
          <td align="right" class="NormalBold" nowrap="nowrap"><asp:Label ID="lblSubtotal" Runat="server" resourcekey="lblSubtotal" cssclass="NormalBold">Subtotal</asp:Label>
          </td>
        </tr>
      </headertemplate>
      <itemtemplate>
        <tr>
        <td class="Normal" align="left">
        <%# DataBinder.Eval(Container.DataItem, "ModelName") %>
        </td>
        <tr>
          <td class="Normal"  align="left" runat="server" visible="false"><%# DataBinder.Eval(Container.DataItem, "ProdName") %> </td>
          <td class="Normal" align="right"><%# DataBinder.Eval(Container.DataItem, "ProdCost", "{0:c}") %> </td>
          <td class="Normal" align="right"><%# DataBinder.Eval(Container.DataItem, "Quantity") %> </td>
          <td class="Normal" align="right"><%# DataBinder.Eval(Container.DataItem, "ExtendedAmount", "{0:c}") %> </td>
        </tr>
      </itemtemplate>
      <footertemplate>
        </table>
      </footertemplate>
    </asp:datalist>
    </td>
    </tr>
    <tr runat="server" id="trRow8">
      <td><hr/>
      </td>
    </tr>
    <tr>
      <td align="right"><table>
          <tr id="trConfirmedOrderTax" runat="server">
            <td class="NormalBold" align="right"><asp:label id="lblTax" runat="server" resourcekey="lblTax" cssclass="NormalBold">Tax:</asp:label>
            </td>
            <td class="NormalBold" align="right"><asp:label id="lblTaxTotal" runat="server" cssclass="Normal"></asp:label>
            </td>
          </tr>
          <tr>
            <td class="NormalBold" align="right"><asp:label id="lblShipping" runat="server" resourcekey="lblShipping" cssclass="NormalBold">Shipping:</asp:label>
            </td>
            <td class="NormalBold" align="right"><asp:label id="lblShippingTotal" runat="server" cssclass="Normal"></asp:label>
            </td>
          </tr>
          <tr>
            <td class="NormalBold" align="right"><asp:label id="lblOrderTotal" runat="server" resourcekey="lblOrderTotal" cssclass="NormalBold">Order Total:</asp:label>
            </td>
            <td class="NormalBold" align="right"><asp:label id="lblTotal" runat="server" cssclass="Normal"></asp:label>
            </td>
          </tr>
        </table></td>
    </tr>
  </asp:placeholder>
  </tbody>
  </table>
  <p style="text-align: left;">
    <asp:Label ID="lblError" runat="server" Font-Bold="true" ForeColor="Red" Visible="false" CssClass="Normalred"></asp:Label>
  </p>
</div>
