<%@ Control language="c#" CodeBehind="CustomerOrders.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.CustomerOrders" AutoEventWireup="True" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<p align="left">
  <asp:Label ID="lblPaymentError" runat="server" class="NormalRed"></asp:Label>
</p>
<table height="100%" cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
  <asp:placeholder id="plhGrid" runat="server">
    <tr>
      <td valign="top" nowrap="nowrap"><dnn:label id="lblParentTitle" runat="server" visible="False" controlname="lblParentTitle"></dnn:label>
        <asp:label class="NormalRed" id="lblError" runat="Server"></asp:label>
        <asp:datagrid id="grdOrders" runat="server" showheader="true" showfooter="false" autogeneratecolumns="false" width="100%" CellPadding="5" CssClass="Store-DataGrid" BorderWidth="0">
          <columns>
          <asp:TemplateColumn>
            <HeaderTemplate>
              <asp:Label ID="lblOrderNumberHeader" Runat="server" cssclass="NormalBold" resourcekey="lblOrderNumberHeader">Order Number</asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
              <asp:label id="lblName" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "OrderID").ToString() %> </asp:label>
            </ItemTemplate>
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <HeaderTemplate>
              <asp:Label ID="lblOrderCustomerUsername" Runat="server" cssclass="NormalBold">Customer</asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
              <asp:label id="lblCustomerUsername" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "CustomerUsername").ToString()%> </asp:label>
            </ItemTemplate>
          </asp:TemplateColumn>
          
          <asp:TemplateColumn>
            <HeaderTemplate>
              <asp:Label ID="lblOrderDateHeader" Runat="server" cssclass="NormalBold" resourcekey="lblOrderDateHeader">Order Date</asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
              <asp:label id="lblOrderDateText" runat="server" cssclass="Normal"></asp:label>
            </ItemTemplate>
          </asp:TemplateColumn>
          <asp:TemplateColumn HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right">
            <HeaderTemplate>
              <asp:Label ID="lblOrderTotalHeader" Runat="server" cssclass="NormalBold" resourcekey="lblOrderTotalHeader">Order Total</asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
              <asp:label id="lblOrderTotalText" runat="server" cssclass="Normal">Order Total</asp:label>
            </ItemTemplate>
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <HeaderTemplate>
              <asp:Label ID="lblOrderStatusHeader" Runat="server" cssclass="NormalBold" resourcekey="lblOrderStatusHeader">Status</asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
              <asp:label id="lblOrderStatusText" runat="server" cssclass="Normal">Order Status</asp:label>
            </ItemTemplate>
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <HeaderTemplate>
              <asp:Label ID="lblShipDateHeader" Runat="server" cssclass="NormalBold" resourcekey="lblShipDateHeader">Ship Date</asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
              <asp:label id="lblShipDateText" runat="server" cssclass="Normal">Ship Date</asp:label>
            </ItemTemplate>
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <HeaderTemplate> <span class="NormalBold">&nbsp;</span> </HeaderTemplate>
            <ItemTemplate>
              <asp:hyperlink id="lnkEdit" runat="server" cssclass="Normal" resourcekey="lnkEdit">Details</asp:hyperlink>
            </ItemTemplate>
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <HeaderTemplate> <span class="NormalBold">&nbsp;</span> </HeaderTemplate>
            <ItemTemplate>
              <asp:LinkButton id="lnkCancel" runat="server" cssclass="Normal" resourcekey="lnkCancel">Cancel</asp:LinkButton>
            </ItemTemplate>
          </asp:TemplateColumn>
          </columns>
        </asp:datagrid>
      </td>
    </tr>
    <tr>
      <td>&nbsp;</td>
    </tr>
  </asp:placeholder>
  <asp:placeholder id="plhForm" runat="server" visible="false">
    <tr>
      <td align="center"><asp:label class="SubHead" id="lblEditTitle" runat="server" resourcekey="lblEditTitle">Order Details</asp:label>
      </td>
    </tr>
    <tr>
      <td>&nbsp;</td>
    </tr>
  </asp:placeholder>
  <asp:panel id="pnlOrderDetails" runat="server" visible="false">
    <tr valign="top">
      <td nowrap="nowrap"><asp:Label id="lblDetailsError" runat="Server" CssClass="NormalRed" EnableViewState="false"></asp:Label>
        <table id="detailsTable" cellspacing="0" cellpadding="0" width="100%" border="0" runat="server" enableviewstate="false">
          <tr valign="top">
            <td class="Store-CommandDetails"><table cellpadding="0" width="100%" border="0">
                <tr>
                  <td class="NormalBold" nowrap="nowrap"><asp:Label id="lblOrderNumberTag" Runat="server" resourcekey="lblOrderNumberTag">Order Number</asp:Label>
                  </td>
                  <td class="NormalBold" nowrap="nowrap"><asp:Label id="lblOrderDateTag" Runat="server" resourcekey="lblOrderDateTag">Order Date</asp:Label>
                  </td>
                  <td id="TD1" class="NormalBold" nowrap="nowrap" runat="server" visible="false"><asp:Label id="lblShipDateTag" Runat="server" resourcekey="lblShipDateTag">Ship Date</asp:Label>
                  </td>
                  <td class="NormalBold" nowrap="nowrap">Company
                  </td>
                </tr>
                <tr>
                  <td class="Normal" valign="top"><asp:label id="lblOrderNumber" runat="server" enableviewstate="false"></asp:label>
                  </td>
                  <td class="Normal" valign="top"><asp:label id="lblOrderDate" runat="server" enableviewstate="false"></asp:label>
                  </td>
                  <td id="TD2" class="Normal" runat="server" visible="false" valign="top"><asp:label id="lblShipDate" runat="server" enableviewstate="false"></asp:label>
                  </td>
                  <td class="Normal" valign="top"><asp:label id="lblOrderCompany" runat="server" enableviewstate="false"></asp:label>
                  </td>
                </tr>
                <tr>
                  <td class="NormalBold" nowrap="nowrap"><asp:Label id="lblBillingAddressTag" Runat="server" resourcekey="lblBillingAddressTag">Billing Address&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label>
                  </td>
                  <td class="NormalBold" nowrap="nowrap"><asp:Label id="lblShippingAddressTag" Runat="server" resourcekey="lblShippingAddressTag">Shipping Address</asp:Label>
                  </td>
                  <td valign="top">
                    <asp:Button ID="btnPayViaPayPal" runat="server" resourcekey="btnPayViaPayPal" Text="Pay now via PayPal" />
                    <span id="spanBR" runat="server"><br /><br /></span>
                    <asp:Button ID="btnCancel" runat="server" resourcekey="btnCancel" Text="Cancel Order" />
                  </td  >
                </tr>
                <tr>
                  <td class="Normal" valign="top"><asp:label id="lblBillTo" runat="server" enableviewstate="false"></asp:label>
                  </td>
                  <td class="Normal" valign="top"><asp:label id="lblShipTo" runat="server" enableviewstate="false"></asp:label>
                  </td>
                  <td class="Normal" valign="top" nowrap="nowrap">
                    <asp:Label id="lblOrderStatusTag" Runat="server" resourcekey="lblOrderStatusTag" CssClass="NormalBold">Order Status</asp:Label><br />
                    <asp:label id="lblOrderStatus" runat="server" enableviewstate="false"></asp:label>
                    <asp:DropDownList ID="ddlOrderStatus" runat="server"></asp:DropDownList>
                    &nbsp;
                    <asp:LinkButton ID="lnkbtnSave" runat="server" resourcekey="lnkbtnSave">Save</asp:LinkButton>
                  </td>
                </tr>
              </table></td>
          </tr>
          <tr>
            <td></td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="Normal"><asp:datagrid id="grdOrderDetails" runat="server" showheader="true" showfooter="false" autogeneratecolumns="false" width="100%" CellPadding="5" CssClass="Store-DataGrid">
                <columns>
                <asp:templatecolumn>
                  <headertemplate>
                    <asp:Label ID="lblModelName" Runat="server" cssclass="NormalBold" resourcekey="lblModelName">Model Name</asp:Label>
                  </headertemplate>
                  <itemtemplate>
                    <asp:label id="Label1" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "ModelName") %> </asp:label>
                  </itemtemplate>
                </asp:templatecolumn>
                <asp:templatecolumn>
                  <headertemplate>
                    <asp:Label ID="lblProdReference" Runat="server" cssclass="NormalBold">Extract(s)</asp:Label>
                  </headertemplate>
                  <itemtemplate>
                    <asp:label id="LabelProdDescription" runat="server" cssclass="Normal"></asp:label>
                  </itemtemplate>
                </asp:templatecolumn>
                <asp:templatecolumn>
                  <headertemplate>
                    <asp:Label ID="lblDelivMethod" Runat="server" cssclass="NormalBold">Delivery</asp:Label>
                  </headertemplate>
                  <itemtemplate>
                    <asp:label id="LabelDel" runat="server" cssclass="Normal"> <%# getProdDeliveryMethodStr((Int32) DataBinder.Eval(Container.DataItem, "ProdDeliveryMethod")) %> </asp:label>
                  </itemtemplate>
                </asp:templatecolumn>
                <asp:templatecolumn HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right">
                  <headertemplate>
                    <asp:Label ID="lblQty" Runat="server" cssclass="NormalBold" resourcekey="lblQty">Qty</asp:Label>
                  </headertemplate>
                  <itemtemplate>
                    <asp:label id="Label3" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "Quantity") %> </asp:label>
                  </itemtemplate>
                </asp:templatecolumn>
                <asp:templatecolumn HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right">
                  <headertemplate>
                    <asp:Label ID="lblODPrice" Runat="server" cssclass="NormalBold" resourcekey="lblODPrice">Price</asp:Label>
                  </headertemplate>
                  <itemtemplate>
                    <asp:label id="lblODPriceText" runat="server" cssclass="Normal"></asp:label>
                  </itemtemplate>
                </asp:templatecolumn>
                <asp:templatecolumn HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right">
                  <headertemplate>
                    <asp:Label id="lblODSubtotal" Runat="server" cssclass="NormalBold" resourcekey="lblODSubtotal">Subtotal</asp:Label>
                  </headertemplate>
                  <itemtemplate>
                    <asp:label id="lblODSubtotalText" runat="server" cssclass="Normal"></asp:label>
                  </itemtemplate>
                </asp:templatecolumn>
                </columns>
              </asp:datagrid>
            </td>
          </tr>
          <tr>
            <td align="center"><br />
              <table cellpadding="5" cellspacing="0" border="0" align="right">
                <tr>
                  <td align="right"><asp:Label id="lblTotalTag" Runat="server" cssclass="NormalBold" resourcekey="lblTotalTag">Sub-total:</aSP:Label></td>
                  <td align="right"><asp:Label id="lblTotal" runat="server" EnableViewState="false" cssclass="Normal"></asp:Label></td>
                </tr>
                <tr>
                  <td align="right"><asp:Label id="lblPostageTag" Runat="server" cssclass="NormalBold" resourcekey="lblPostageTag">Shipping &amp; Handling:</aSP:Label></td>
                  <td align="right"><asp:Label id="lblPostage" runat="server" EnableViewState="false" cssclass="Normal"></asp:Label></td>
                </tr>
                <tr runat="server" id="trTax">
                  <td align="right"><asp:Label id="lblTaxTag" Runat="server" cssclass="NormalBold" resourcekey="lblTaxTag">Tax:</aSP:Label></td>
                  <td align="right"><asp:Label id="lblTax" runat="server" EnableViewState="false" cssclass="Normal"></asp:Label></td>
                </tr>
                <tr>
                  <td align="right"><asp:Label id="lblTotalIncPostageTag" Runat="server" cssclass="NormalBold" resourcekey="lblTotalIncPostageTag">Total:</aSP:Label></td>
                  <td align="right"><asp:Label id="lblTotalIncPostage" runat="server" EnableViewState="false" cssclass="Normal"></asp:Label></td>
                </tr>
              </table></td>
          </tr>
          <tr>
            <td align="center"><br />
              <input id="btnBack" onclick="javascript:history.back(1);" type="button" value=" &lt;&lt; " />
              <br />
              <br />
            </td>
          </tr>
        </table></td>
    </tr>
    </asp:panel>
  <asp:Panel ID="pnlPayPalTransfer" runat="server" Visible="false">
    <tr valign="top">
      <td ><asp:Label ID="lblGoToPayPal" runat="server" resourcekey="lblGoToPayPal" CssClass="Normal"></asp:Label>
        <asp:Button ID="btnGoToPayPal" runat="server" resourcekey="btnGoToPayPal" Text="Continue to PayPal &gt;" />
        <br />
        <asp:Image ID="paypalimage2" runat="server" resourcekey="paypalimage2" AlternateText="Pay by PayPal using your credit/debit card or PayPal account" Visible="false" />
      </td>
    </tr>
    </asp:Panel>
</table>
