<%@ Control Language="c#" AutoEventWireup="True" Codebehind="MiniCart.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.MiniCart" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div class="Store-MiniCart-Entity">
<table border="0" cellpadding="0" cellspacing="0" width="260" class="MiniCartMasterTABLE">
  <tr>
    <td colspan="2"><img src="/images/canadean/shop/minicart_shoppingbasket.gif" border="0" alt="Shopping Basket" /></td>
  </tr>
  <tr style="height:25px">
    <td colspan="2" class="Normal">You currently have <asp:label id="lblCount" runat="server" cssclass="Normal"></asp:label> item(s) in your basket</td>
  </tr>
  <tr>
    <td colspan="2">
        <asp:datagrid id="grdItems" runat="server" showheader="false" showfooter="false" autogeneratecolumns="false" 
				width="260" allowpaging="false" CssClass="Store-DataGrid" GridLines="None">
        <columns>
        <asp:templatecolumn headerstyle-cssclass="NormalBold" headerstyle-horizontalalign="Left" footerstyle-horizontalalign="Right">
          <headertemplate>
          </headertemplate>
          <itemtemplate>
            <asp:label id="lblTitle" runat="server" cssclass="Normal"> <%# DataBinder.Eval(Container.DataItem, "ModelName") %> </asp:label>
          </itemtemplate>
          <footertemplate>
          </footertemplate>
        </asp:templatecolumn>
        <asp:templatecolumn headerstyle-cssclass="NormalBold" headerstyle-horizontalalign="Left" itemstyle-horizontalalign="Center" itemstyle-width="50" itemstyle-wrap="False">
          <headertemplate>
          </headertemplate>
          <itemtemplate>
            <asp:linkbutton id="lnkDelete" runat="server" cssclass="Normal"><img src="/images/canadean/shop/minicart_remove.gif" border="0" alt="Remove" /></asp:linkbutton>
          </itemtemplate>
          <footertemplate> </footertemplate>
        </asp:templatecolumn>
        </columns>
      </asp:datagrid>
    </td>
  </tr>
  <tr><td colspan="2">&nbsp;</td></tr>
  <tr>
    <td class="Normal" width="70%"><asp:Label id="lblTotal" runat="server" cssclass="Normal" Visible="true"></asp:Label></td>
    <td class="Store-MiniCart-BtnViewCartMasterTD" align="right"><asp:linkbutton id="btnViewCart" runat="server" cssclass="Normal" resourcekey="btnViewCart">View Cart Details</asp:linkbutton>
    </td>
  </tr>
</table>
</div>