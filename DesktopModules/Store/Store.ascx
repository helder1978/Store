<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="c#" CodeBehind="Store.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.Store" AutoEventWireup="True" %>

<table cellpadding="0" cellspacing="0" width="100%" border="0" class="ContentHead">
  <tr>
    <td align="left" class="Store-Account-BtnTD"><asp:Label id="lblParentTitle" runat="server" cssclass="SubHead"></asp:Label>
    </td>
    <td align="right">
      <asp:linkbutton id="btnStoreInfo" cssclass="Normal" runat="server" resourcekey="btnStoreInfo">Store Info</asp:linkbutton>
      <asp:label id="lblSpacer1" cssclass="Normal" runat="server">|</asp:label>
      <asp:linkbutton id="btnCustomers" cssclass="Normal" runat="server" resourcekey="btnCustomers">Customers</asp:linkbutton>
      <asp:label id="lblSpacer2" cssclass="Normal" runat="server">|</asp:label>
      <asp:linkbutton id="btnCategories" cssclass="Normal" runat="server" resourcekey="btnCategories">Categories</asp:linkbutton>
      <asp:label id="lblSpacer3" cssclass="Normal" runat="server">|</asp:label>
      <asp:linkbutton id="btnProducts" cssclass="Normal" runat="server" resourcekey="btnProducts">Products</asp:linkbutton>
      <asp:label id="lblSpacer4" cssclass="Normal" runat="server" Visible="false">|</asp:label>
      <asp:linkbutton id="btnReviews" cssclass="Normal" runat="server" Visible="false" resourcekey="btnReviews">Reviews</asp:linkbutton>
      <asp:label id="lblSpacer5" cssclass="Normal" runat="server" Visible="False">|</asp:label>
      <asp:linkbutton id="btnHelp" cssclass="Normal" runat="server" Visible="False" resourcekey="btnHelp">Help</asp:linkbutton>
    </td>
  </tr>
  <tr>
    <td colspan="2" height="25"><hr>
    </td>
  </tr>
  <tr>
    <td align="center" colspan="2"><asp:placeholder id="plhAdminControl" runat="server"></asp:placeholder>
    </td>
  </tr>
</table>
