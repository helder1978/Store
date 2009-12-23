<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="c#" CodeBehind="Account.ascx.cs" Inherits="DotNetNuke.Modules.Store.WebControls.Account" AutoEventWireup="True" %>

<div class="Store-Account-Entity">
  <table cellpadding="0" cellspacing="0" width="100%" border="0" class="Store-Account-MasterTable">
    <tr runat="server" visible="false">
      <td class="ListContainer-Title">
          <asp:Label id="lblParentTitle" runat="server" cssclass="SubHead"></asp:Label>
        <div class="Store-Account-BtnTD">
          <asp:linkbutton id="btnCart" cssclass="Normal" runat="server" resourcekey="btnCart" CausesValidation="False" onclick="btnCart_Click">Cart</asp:linkbutton>
          <asp:label id="lblSpacer1" cssclass="Normal" runat="server">|</asp:label>
          <asp:linkbutton id="btnProfile" cssclass="Normal" runat="server" resourcekey="btnProfile" CausesValidation="False" Visible="false" onclick="btnProfile_Click">Profile</asp:linkbutton>
          <asp:label id="lblSpacer2" cssclass="Normal" runat="server" Visible="false">|</asp:label>
          <asp:linkbutton id="btnOrders" cssclass="Normal" runat="server" resourcekey="btnOrders" CausesValidation="False" onclick="btnOrders_Click">Order History</asp:linkbutton>
        </div></td>
    </tr>
    <tr>
      <td align="center" colspan="2"><asp:placeholder id="plhAccountControl" runat="server"></asp:placeholder>
      </td>
    </tr>
  </table>
</div>
