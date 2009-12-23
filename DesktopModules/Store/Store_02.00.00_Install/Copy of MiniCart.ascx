<%@ Control Language="c#" AutoEventWireup="True" Codebehind="MiniCart.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.MiniCart" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div class="Store-MiniCart-Entity">
<table border="0" cellpadding="0" cellspacing="0" width="100%" class="MiniCartMasterTABLE">
  <tr>
    <td><asp:datagrid id="grdItems" runat="server" showheader="true" showfooter="true" autogeneratecolumns="false"
				width="100%" allowpaging="false" CssClass="Store-DataGrid">
        <columns>
        <asp:templatecolumn headerstyle-cssclass="NormalBold" headerstyle-horizontalalign="Center" footerstyle-horizontalalign="Right">
          <headertemplate>
            <asp:Label ID="lblProduct" Runat=server resourcekey="lblProduct" cssclass="NormalBold">Product</asp:Label>
          </headertemplate>
          <itemtemplate>
            <asp:label id="lblTitle" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "ModelName") %> </asp:label>
          </itemtemplate>
          <footertemplate>
            <asp:Label ID="lblTotals" Runat=server resourcekey="lblTotals" cssclass="NormalBold">Totals:</asp:Label>
          </footertemplate>
        </asp:templatecolumn>
        <asp:templatecolumn headerstyle-cssclass="NormalBold" headerstyle-horizontalalign="Center" footerstyle-horizontalalign="Center"
						itemstyle-horizontalalign="Center" itemstyle-width="35">
          <headertemplate>
            <asp:Label ID="lblQty" Runat=server resourcekey="lblQty" cssclass="NormalBold">Qty</asp:Label>
          </headertemplate>
          <itemtemplate>
            <asp:label id="lblQuantity" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "Quantity") %> </asp:label>
          </itemtemplate>
          <footertemplate>
            <asp:label id="lblCount" runat="server" cssclass="Normal"></asp:label>
          </footertemplate>
        </asp:templatecolumn>
        <asp:templatecolumn headerstyle-cssclass="NormalBold" headerstyle-horizontalalign="Center" itemstyle-horizontalalign="Right"
						footerstyle-horizontalalign="Right">
          <headertemplate>
            <asp:Label ID="lblPriceTag" Runat=server resourcekey="lblPriceTag" cssclass="NormalBold">Price</asp:Label>
          </headertemplate>
          <itemtemplate>
            <asp:label id="lblPrice" runat="server" cssclass="Normal"></asp:label>
          </itemtemplate>
          <footertemplate>
            <asp:label id="lblTotal" runat="server" cssclass="Normal"></asp:label>
          </footertemplate>
        </asp:templatecolumn>
        <asp:templatecolumn headerstyle-cssclass="NormalBold" headerstyle-horizontalalign="Center" itemstyle-horizontalalign="Center" itemstyle-width="50" itemstyle-wrap="False">
          <headertemplate>
            <asp:Label ID="lblControlHeader" runat="server" class="NormalBold" resourcekey="lblControlHeader.Text">Qty Ctrl</asp:Label>
          </headertemplate>
          <itemtemplate>
            <asp:linkbutton id="lnkAdd" runat="server" cssclass="Normal">+</asp:linkbutton>
            <asp:linkbutton id="lnkRemove" runat="server" cssclass="Normal">-</asp:linkbutton>
            <asp:linkbutton id="lnkDelete" runat="server" cssclass="Normal">x</asp:linkbutton>
          </itemtemplate>
          <footertemplate> </footertemplate>
        </asp:templatecolumn>
        </columns>
      </asp:datagrid>
    </td>
  </tr>
  <tr>
    <td class="Store-MiniCart-BtnViewCartMasterTD"><asp:linkbutton id="btnViewCart" runat="server" cssclass="Normal" resourcekey="btnViewCart">View Cart Details</asp:linkbutton>
    </td>
  </tr>
</table>
</div>